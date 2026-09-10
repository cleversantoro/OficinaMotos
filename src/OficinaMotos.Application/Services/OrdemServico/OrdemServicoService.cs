using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;

namespace OficinaMotos.Application.Services.OrdemServicoRepo
{
    public class OrdemServicoService : IOrdemServicoService
    {
        private readonly IOrdemServicoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOrdemServicoPagamentoRepository _pagamentoRepository;
        private readonly IFinanceiroContaReceberRepository _contaReceberRepository;

        public OrdemServicoService(
            IOrdemServicoRepository repository,
            IMapper mapper,
            IOrdemServicoPagamentoRepository pagamentoRepository,
            IFinanceiroContaReceberRepository contaReceberRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _pagamentoRepository = pagamentoRepository;
            _contaReceberRepository = contaReceberRepository;
        }

        public async Task<List<OrdemServicoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoResponseDTO>>(items);
        }

        public async Task<OrdemServicoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoResponseDTO?>(item);
        }

        public async Task<OrdemServicoResponseDTO> CreateAsync(CreateOrdemServicoDTO request)
        {
            var entity = _mapper.Map<OrdemServico>(request);
            entity.VeiculoId = request.VeiculoId;
            if (request.DataAbertura.HasValue)
            {
                entity.DataAbertura = request.DataAbertura.Value;
            }
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoResponseDTO>(created);
        }

        public async Task<OrdemServicoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ClienteId = request.ClienteId;
            entity.MecanicoId = request.MecanicoId;
            entity.VeiculoId = request.VeiculoId;
            entity.DescricaoProblema = request.DescricaoProblema;
            entity.Status = request.Status;
            entity.DataAbertura = request.DataAbertura ?? entity.DataAbertura;
            entity.DataConclusao = request.DataConclusao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<OrdemServicoPagamentoResponseDTO> RegistrarPagamentoAsync(CreateOrdemServicoPagamentoDTO request)
        {
            var ordem = await _repository.GetByIdAsync(request.OrdemServicoId);
            if (ordem == null)
            {
                throw new KeyNotFoundException($"Ordem de serviço #{request.OrdemServicoId} não encontrada.");
            }

            if (ordem.Status == OrdemServicoStatus.Cancelada)
            {
                throw new InvalidOperationException("Não é possível registrar pagamento em uma Ordem de Serviço cancelada.");
            }

            // 1. Persistir o pagamento da OS
            var pagamento = _mapper.Map<OrdemServicoPagamento>(request);
            if (string.IsNullOrWhiteSpace(pagamento.Status))
            {
                pagamento.Status = "Liquidado";
            }
            if (!pagamento.DataPagamento.HasValue)
            {
                pagamento.DataPagamento = DateTime.UtcNow;
            }
            var pagamentoCriado = await _pagamentoRepository.AddAsync(pagamento);

            // 2. Avaliar quitação e atualizar status para Concluida se total pago atingir total de itens
            var totalItens = (ordem.Itens != null && ordem.Itens.Count > 0)
                ? ordem.Itens.Sum(i => i.Total > 0 ? i.Total : (i.Quantidade * i.ValorUnitario))
                : 0m;

            var totalPagoAnterior = (ordem.Pagamentos != null && ordem.Pagamentos.Count > 0)
                ? ordem.Pagamentos.Sum(p => p.Valor)
                : 0m;

            var novoTotalPago = totalPagoAnterior + request.Valor;

            if (totalItens > 0 && novoTotalPago >= totalItens)
            {
                ordem.Status = OrdemServicoStatus.Concluida;
                ordem.DataConclusao = DateTime.UtcNow;
                ordem.SetUpdated();

                ordem.Cliente = null;
                ordem.Mecanico = null;
                ordem.Veiculo = null;
                ordem.Itens?.Clear();
                ordem.Pagamentos?.Clear();
                ordem.Anexos?.Clear();
                ordem.Avaliacoes?.Clear();
                ordem.Checklists?.Clear();
                ordem.Observacoes?.Clear();
                ordem.Historico?.Clear();

                await _repository.UpdateAsync(ordem);
            }

            // 3. Gerar lançamento automático em ContasReceber
            var contaReceber = new FinanceiroContaReceber
            {
                ClienteId = ordem.ClienteId > 0 ? ordem.ClienteId : null,
                Descricao = $"Pagamento OS #{ordem.Id} - {request.Metodo ?? "Geral"}",
                Valor = request.Valor,
                Vencimento = request.DataPagamento ?? DateTime.UtcNow,
                DataRecebimento = request.DataPagamento ?? DateTime.UtcNow,
                Status = "Recebido",
                Observacao = string.IsNullOrWhiteSpace(request.Observacao)
                    ? $"Referente à OS #{ordem.Id}"
                    : $"{request.Observacao} (OS #{ordem.Id})"
            };

            await _contaReceberRepository.AddAsync(contaReceber);

            var response = _mapper.Map<OrdemServicoPagamentoResponseDTO>(pagamentoCriado);
            response.StatusOrdemServico = ordem.Status.ToString();
            return response;
        }
    }
}
