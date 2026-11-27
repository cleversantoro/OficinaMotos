using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.OrdemServicoRepo
{
    public class OrdemServicoAnexoService : IOrdemServicoAnexoService
    {
        private readonly IOrdemServicoAnexoRepository _repository;
        private readonly IMapper _mapper;

        public OrdemServicoAnexoService(IOrdemServicoAnexoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrdemServicoAnexoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoAnexoResponseDTO>>(items);
        }

        public async Task<OrdemServicoAnexoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoAnexoResponseDTO?>(item);
        }

        public async Task<OrdemServicoAnexoResponseDTO> CreateAsync(CreateOrdemServicoAnexoDTO request)
        {
            var entity = _mapper.Map<OrdemServicoAnexo>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoAnexoResponseDTO>(created);
        }

        public async Task<OrdemServicoAnexoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoAnexoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            entity.Nome = request.Nome;
            entity.Tipo = request.Tipo;
            entity.Url = request.Url;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoAnexoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
