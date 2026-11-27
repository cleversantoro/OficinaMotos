using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.DTOs.Responses.Financeiro;
using OficinaMotos.Application.Interfaces.Financeiro;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Financeiro
{
    public class FinanceiroMetodoPagamentoService : IFinanceiroMetodoPagamentoService
    {
        private readonly IFinanceiroMetodoPagamentoRepository _repository;
        private readonly IMapper _mapper;

        public FinanceiroMetodoPagamentoService(IFinanceiroMetodoPagamentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FinanceiroMetodoPagamentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FinanceiroMetodoPagamentoResponseDTO>>(items);
        }

        public async Task<FinanceiroMetodoPagamentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FinanceiroMetodoPagamentoResponseDTO?>(item);
        }

        public async Task<FinanceiroMetodoPagamentoResponseDTO> CreateAsync(CreateFinanceiroMetodoPagamentoDTO request)
        {
            var entity = _mapper.Map<FinanceiroMetodoPagamento>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FinanceiroMetodoPagamentoResponseDTO>(created);
        }

        public async Task<FinanceiroMetodoPagamentoResponseDTO?> UpdateAsync(long id, UpdateFinanceiroMetodoPagamentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Nome = request.Nome;
            entity.Tipo = request.Tipo;
            entity.Ativo = request.Ativo;
            entity.TaxaPercentual = request.TaxaPercentual;
            entity.TaxaFixa = request.TaxaFixa;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FinanceiroMetodoPagamentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
