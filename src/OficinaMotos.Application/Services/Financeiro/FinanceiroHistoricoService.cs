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
    public class FinanceiroHistoricoService : IFinanceiroHistoricoService
    {
        private readonly IFinanceiroHistoricoRepository _repository;
        private readonly IMapper _mapper;

        public FinanceiroHistoricoService(IFinanceiroHistoricoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FinanceiroHistoricoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FinanceiroHistoricoResponseDTO>>(items);
        }

        public async Task<FinanceiroHistoricoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FinanceiroHistoricoResponseDTO?>(item);
        }

        public async Task<FinanceiroHistoricoResponseDTO> CreateAsync(CreateFinanceiroHistoricoDTO request)
        {
            var entity = _mapper.Map<FinanceiroHistorico>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FinanceiroHistoricoResponseDTO>(created);
        }

        public async Task<FinanceiroHistoricoResponseDTO?> UpdateAsync(long id, UpdateFinanceiroHistoricoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Entidade = request.Entidade;
            entity.EntidadeId = request.EntidadeId;
            entity.Acao = request.Acao;
            entity.DadosAnteriores = request.DadosAnteriores;
            entity.DadosNovos = request.DadosNovos;
            entity.UsuarioId = request.UsuarioId;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FinanceiroHistoricoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
