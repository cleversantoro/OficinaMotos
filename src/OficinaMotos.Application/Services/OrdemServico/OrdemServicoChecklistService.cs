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
    public class OrdemServicoChecklistService : IOrdemServicoChecklistService
    {
        private readonly IOrdemServicoChecklistRepository _repository;
        private readonly IMapper _mapper;

        public OrdemServicoChecklistService(IOrdemServicoChecklistRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrdemServicoChecklistResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoChecklistResponseDTO>>(items);
        }

        public async Task<OrdemServicoChecklistResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoChecklistResponseDTO?>(item);
        }

        public async Task<OrdemServicoChecklistResponseDTO> CreateAsync(CreateOrdemServicoChecklistDTO request)
        {
            var entity = _mapper.Map<OrdemServicoChecklist>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoChecklistResponseDTO>(created);
        }

        public async Task<OrdemServicoChecklistResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoChecklistDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            entity.Item = request.Item;
            entity.Realizado = request.Realizado;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoChecklistResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
