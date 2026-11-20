using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.DTOs.Responses.Mecanico;
using OficinaMotos.Application.Interfaces.Mecanico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Mecanico
{
    public class MecanicoEspecialidadeService : IMecanicoEspecialidadeService
    {
        private readonly IMecanicoEspecialidadeRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoEspecialidadeService(IMecanicoEspecialidadeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoEspecialidadeResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoEspecialidadeResponseDTO>>(items);
        }

        public async Task<MecanicoEspecialidadeResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoEspecialidadeResponseDTO?>(item);
        }

        public async Task<MecanicoEspecialidadeResponseDTO> CreateAsync(CreateMecanicoEspecialidadeDTO request)
        {
            var entity = _mapper.Map<MecanicoEspecialidade>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoEspecialidadeResponseDTO>(created);
        }

        public async Task<MecanicoEspecialidadeResponseDTO?> UpdateAsync(long id, UpdateMecanicoEspecialidadeDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Codigo = request.Codigo;
            entity.Nome = request.Nome;
            entity.Descricao = request.Descricao;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoEspecialidadeResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
