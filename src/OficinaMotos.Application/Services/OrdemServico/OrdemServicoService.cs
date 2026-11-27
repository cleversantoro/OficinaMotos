using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Application.Validators.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;

namespace OficinaMotos.Application.Services.OrdemServico
{
    public class OrdemServicoService : IOrdemServicoService
    {
        private readonly IOrdemServicoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOrdemServicoPagamentoRepository _pagamentoRepository;
        private readonly IFinanceiroContaReceberRepository _contaReceberRepository;
        private readonly CreateOrdemServicoValidator _createValidator = new();

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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await _createValidator.ValidateAndThrowAsync(request);

            var entity = _mapper.Map<Domain.Entities.OrdemServico>(request);
            entity.VeiculoId = request.VeiculoId;
            if (request.DataAbertura != default)
            {
                entity.DataAbertura = request.DataAbertura;
            }
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoResponseDTO>(created);
        }

        public async Task<OrdemServicoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            if (entity.Status != request.Status)
            {
                EnsureValidStatusTransition(entity.Status, request.Status);
            }

            entity.Numero = request.Numero;
            entity.ClienteId = request.ClienteId;
            entity.VeiculoId = request.VeiculoId;
            entity.DescricaoProblema = request.DescricaoProblema;
            entity.Diagnostico = request.Diagnostico;
            entity.Status = request.Status;
            entity.Prioridade = request.Prioridade;
            entity.DataAbertura = request.DataAbertura == default ? entity.DataAbertura : request.DataAbertura;
            entity.PrevisaoEntrega = request.PrevisaoEntrega;
            entity.DataConclusao = request.Status == OrdemServicoEnum.Concluida
                ? (request.DataConclusao ?? entity.DataConclusao ?? DateTime.UtcNow)
                : request.DataConclusao;
            entity.ValorPecas = request.ValorPecas;
            entity.ValorServicos = request.ValorServicos;
            entity.Desconto = request.Desconto;
            entity.ValorTotal = request.ValorTotal;
            entity.Observacoes = request.Observacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoResponseDTO>(entity);
        }

        public async Task<OrdemServicoResponseDTO?> ChangeStatusAsync(long id, OrdemServicoEnum novoStatus)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            EnsureValidStatusTransition(entity.Status, novoStatus);

            entity.Status = novoStatus;
            if (novoStatus == OrdemServicoEnum.Concluida && !entity.DataConclusao.HasValue)
            {
                entity.DataConclusao = DateTime.UtcNow;
            }
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoResponseDTO>(entity);
        }

        public decimal CalculateTotal(Domain.Entities.OrdemServico ordem)
        {
            if (ordem?.Itens == null || ordem.Itens.Count == 0)
            {
                return 0m;
            }

            return ordem.Itens.Sum(i => i.Total > 0 ? i.Total : (i.Quantidade * i.ValorUnitario));
        }

        public async Task<decimal> CalculateTotalAsync(long id)
        {
            var ordem = await _repository.GetByIdAsync(id);
            if (ordem == null)
            {
                throw new KeyNotFoundException($"Ordem de serviÃ§o #{id} nÃ£o encontrada.");
            }

            return CalculateTotal(ordem);
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
                throw new KeyNotFoundException($"Ordem de serviÃ§o #{request.OrdemServicoId} nÃ£o encontrada.");
            }

            if (ordem.Status == OrdemServicoEnum.Cancelada)
            {
                throw new InvalidOperationException("NÃ£o Ã© possÃ­vel registrar pagamento em uma Ordem de ServiÃ§o cancelada.");
            }

            // 1. Persistir o pagamento da OS
            var pagamento = _mapper.Map<OrdemServicoPagamento>(request);
            if (!Enum.IsDefined(typeof(OrdemServicoPagamentoStatus), pagamento.Status))
            {
                pagamento.Status = OrdemServicoPagamentoStatus.Pago;
            }
            if (!pagamento.DataPagamento.HasValue)
            {
                pagamento.DataPagamento = DateTime.UtcNow;
            }
            var pagamentoCriado = await _pagamentoRepository.AddAsync(pagamento);

            // 2. Avaliar quitaÃ§Ã£o e atualizar status para Concluida se total pago atingir total de itens
            var totalItens = CalculateTotal(ordem);

            var totalPagoAnterior = (ordem.Pagamentos != null && ordem.Pagamentos.Count > 0)
                ? ordem.Pagamentos.Sum(p => p.Valor)
                : 0m;

            var novoTotalPago = totalPagoAnterior + request.Valor;

            if (totalItens > 0 && novoTotalPago >= totalItens)
            {
                ordem.Status = OrdemServicoEnum.Concluida;
                ordem.DataConclusao = DateTime.UtcNow;
                ordem.SetUpdated();

                ordem.Cliente = null;
                ordem.Veiculo = null;
                ordem.Itens?.Clear();
                ordem.Pagamentos?.Clear();
                ordem.Anexos?.Clear();
                ordem.Avaliacoes?.Clear();
                ordem.Checklists?.Clear();
                ordem.ListaObservacoes?.Clear();
                ordem.Historico?.Clear();

                await _repository.UpdateAsync(ordem);
            }

            // 3. Gerar lanÃ§amento automÃ¡tico em ContasReceber
            var dataRef = request.DataPagamento ?? DateTime.UtcNow;
            var contaReceber = new FinanceiroContaReceber
            {
                ClienteId = ordem.ClienteId > 0 ? ordem.ClienteId : null,
                OrdemServicoId = ordem.Id,
                Descricao = $"Pagamento OS #{ordem.Id} - {request.Metodo ?? "Geral"}",
                ValorOriginal = request.Valor,
                ValorRecebido = request.Valor,
                DataEmissao = dataRef.Date,
                DataVencimento = dataRef.Date,
                Status = "RECEBIDO",
                Observacoes = string.IsNullOrWhiteSpace(request.Observacao)
                    ? $"Referente Ã  OS #{ordem.Id}"
                    : $"{request.Observacao} (OS #{ordem.Id})"
            };

            await _contaReceberRepository.AddAsync(contaReceber);

            var response = _mapper.Map<OrdemServicoPagamentoResponseDTO>(pagamentoCriado);
            response.StatusOrdemServico = ordem.Status.ToString();
            return response;
        }

        private static void EnsureValidStatusTransition(OrdemServicoEnum statusAtual, OrdemServicoEnum novoStatus)
        {
            if (!Enum.IsDefined(typeof(OrdemServicoEnum), novoStatus))
            {
                throw new DomainException($"Status '{novoStatus}' invÃ¡lido para Ordem de ServiÃ§o.");
            }

            var isValid = statusAtual switch
            {
                OrdemServicoEnum.Aberta => novoStatus is OrdemServicoEnum.EmAndamento
                    or OrdemServicoEnum.AguardandoPeca
                    or OrdemServicoEnum.Cancelada,
                OrdemServicoEnum.EmAndamento => novoStatus is OrdemServicoEnum.AguardandoPeca
                    or OrdemServicoEnum.Concluida
                    or OrdemServicoEnum.Cancelada,
                OrdemServicoEnum.AguardandoPeca => novoStatus is OrdemServicoEnum.EmAndamento
                    or OrdemServicoEnum.Concluida
                    or OrdemServicoEnum.Cancelada,
                OrdemServicoEnum.Concluida => false,
                OrdemServicoEnum.Cancelada => false,
                _ => false
            };

            if (!isValid)
            {
                throw new DomainException(
                    $"TransiÃ§Ã£o de status invÃ¡lida na Ordem de ServiÃ§o: nÃ£o Ã© permitido alterar de '{statusAtual}' para '{novoStatus}'.");
            }
        }
    }
}
