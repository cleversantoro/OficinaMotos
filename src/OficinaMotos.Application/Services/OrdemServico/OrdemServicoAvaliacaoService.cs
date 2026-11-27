using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.OrdemServico
{
    public class OrdemServicoAvaliacaoService : IOrdemServicoAvaliacaoService
    {
        private readonly IOrdemServicoAvaliacaoRepository _repository;
        private readonly IMapper _mapper;

        public OrdemServicoAvaliacaoService(IOrdemServicoAvaliacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrdemServicoAvaliacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoAvaliacaoResponseDTO>>(items);
        }

        public async Task<OrdemServicoAvaliacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoAvaliacaoResponseDTO?>(item);
        }

        public async Task<OrdemServicoAvaliacaoResponseDTO> CreateAsync(CreateOrdemServicoAvaliacaoDTO request)
        {
            var entity = _mapper.Map<OrdemServicoAvaliacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoAvaliacaoResponseDTO>(created);
        }

        public async Task<OrdemServicoAvaliacaoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoAvaliacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            entity.ClienteId = request.ClienteId;
            entity.Nota = request.Nota;
            entity.Comentario = request.Comentario;
            entity.DataAvaliacao = request.DataAvaliacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoAvaliacaoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
