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
    public class FinanceiroAnexoService : IFinanceiroAnexoService
    {
        private readonly IFinanceiroAnexoRepository _repository;
        private readonly IMapper _mapper;

        public FinanceiroAnexoService(IFinanceiroAnexoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FinanceiroAnexoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FinanceiroAnexoResponseDTO>>(items);
        }

        public async Task<FinanceiroAnexoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FinanceiroAnexoResponseDTO?>(item);
        }

        public async Task<FinanceiroAnexoResponseDTO> CreateAsync(CreateFinanceiroAnexoDTO request)
        {
            var entity = _mapper.Map<FinanceiroAnexo>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FinanceiroAnexoResponseDTO>(created);
        }

        public async Task<FinanceiroAnexoResponseDTO?> UpdateAsync(long id, UpdateFinanceiroAnexoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PagamentoId = request.PagamentoId;
            entity.ContaPagarId = request.ContaPagarId;
            entity.ContaReceberId = request.ContaReceberId;
            entity.Nome = request.Nome;
            entity.Tipo = request.Tipo;
            entity.Url = request.Url;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FinanceiroAnexoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
