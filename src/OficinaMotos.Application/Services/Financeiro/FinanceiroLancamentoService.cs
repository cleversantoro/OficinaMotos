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
    public class FinanceiroLancamentoService : IFinanceiroLancamentoService
    {
        private readonly IFinanceiroLancamentoRepository _repository;
        private readonly IMapper _mapper;

        public FinanceiroLancamentoService(IFinanceiroLancamentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FinanceiroLancamentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FinanceiroLancamentoResponseDTO>>(items);
        }

        public async Task<FinanceiroLancamentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FinanceiroLancamentoResponseDTO?>(item);
        }

        public async Task<FinanceiroLancamentoResponseDTO> CreateAsync(CreateFinanceiroLancamentoDTO request)
        {
            var entity = _mapper.Map<FinanceiroLancamento>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FinanceiroLancamentoResponseDTO>(created);
        }

        public async Task<FinanceiroLancamentoResponseDTO?> UpdateAsync(long id, UpdateFinanceiroLancamentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Tipo = request.Tipo;
            entity.Categoria = request.Categoria;
            entity.Descricao = request.Descricao;
            entity.Valor = request.Valor;
            entity.DataCompetencia = request.DataCompetencia;
            entity.DataLancamento = request.DataLancamento;
            entity.OrigemTipo = request.OrigemTipo;
            entity.OrigemId = request.OrigemId;
            entity.Observacoes = request.Observacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FinanceiroLancamentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
