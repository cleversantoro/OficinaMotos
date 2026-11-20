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
    public class MecanicoEspecialidadeRelService : IMecanicoEspecialidadeRelService
    {
        private readonly IMecanicoEspecialidadeRelRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoEspecialidadeRelService(IMecanicoEspecialidadeRelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoEspecialidadeRelResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoEspecialidadeRelResponseDTO>>(items);
        }

        public async Task<MecanicoEspecialidadeRelResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoEspecialidadeRelResponseDTO?>(item);
        }

        public async Task<MecanicoEspecialidadeRelResponseDTO> CreateAsync(CreateMecanicoEspecialidadeRelDTO request)
        {
            var entity = _mapper.Map<MecanicoEspecialidadeRel>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoEspecialidadeRelResponseDTO>(created);
        }

        public async Task<MecanicoEspecialidadeRelResponseDTO?> UpdateAsync(long id, UpdateMecanicoEspecialidadeRelDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MecanicoId = request.MecanicoId;
            entity.EspecialidadeId = request.EspecialidadeId;
            entity.Nivel = request.Nivel;
            entity.Principal = request.Principal;
            entity.Anotacoes = request.Anotacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoEspecialidadeRelResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
