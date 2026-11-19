using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.DTOs.Responses.Financeiro;
using OficinaMotos.Application.Interfaces.Financeiro;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using System.Collections.Generic;
using System.Threading.Tasks;
using OficinaMotos.Domain.Exceptions;

namespace OficinaMotos.Application.Services.Financeiro
{
    public class FinanceiroPagamentoService : IFinanceiroPagamentoService
    {
        private readonly IFinanceiroPagamentoRepository _repository;
        private readonly IMapper _mapper;

        public FinanceiroPagamentoService(IFinanceiroPagamentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FinanceiroPagamentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FinanceiroPagamentoResponseDTO>>(items);
        }

        public async Task<FinanceiroPagamentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FinanceiroPagamentoResponseDTO?>(item);
        }

        public async Task<FinanceiroPagamentoResponseDTO> CreateAsync(CreateFinanceiroPagamentoDTO request)
        {
            if ((request.ContaPagarId.HasValue) == (request.ContaReceberId.HasValue))
                throw new DomainException("O pagamento deve estar vinculado a exatamente uma conta: pagar ou receber.");
            var entity = _mapper.Map<FinanceiroPagamento>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FinanceiroPagamentoResponseDTO>(created);
        }

        public async Task<FinanceiroPagamentoResponseDTO?> UpdateAsync(long id, UpdateFinanceiroPagamentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            entity.ClienteId = request.ClienteId;
            entity.FornecedorId = request.FornecedorId;
            entity.Valor = request.Valor;
            entity.Status = request.Status;
            entity.DataPagamento = request.DataPagamento;
            entity.MetodoId = request.MetodoId;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FinanceiroPagamentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
