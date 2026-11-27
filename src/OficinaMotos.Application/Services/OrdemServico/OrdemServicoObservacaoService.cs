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
    public class OrdemServicoObservacaoService : IOrdemServicoObservacaoService
    {
        private readonly IOrdemServicoObservacaoRepository _repository;
        private readonly IMapper _mapper;

        public OrdemServicoObservacaoService(IOrdemServicoObservacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrdemServicoObservacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoObservacaoResponseDTO>>(items);
        }

        public async Task<OrdemServicoObservacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoObservacaoResponseDTO?>(item);
        }

        public async Task<OrdemServicoObservacaoResponseDTO> CreateAsync(CreateOrdemServicoObservacaoDTO request)
        {
            var entity = _mapper.Map<OrdemServicoObservacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoObservacaoResponseDTO>(created);
        }

        public async Task<OrdemServicoObservacaoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoObservacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            //entity.Usuario = request.Usuario;
            entity.Texto = request.Texto;
            entity.Tipo = request.Tipo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoObservacaoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
