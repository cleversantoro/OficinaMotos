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
    public class MecanicoExperienciaService : IMecanicoExperienciaService
    {
        private readonly IMecanicoExperienciaRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoExperienciaService(IMecanicoExperienciaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoExperienciaResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoExperienciaResponseDTO>>(items);
        }

        public async Task<MecanicoExperienciaResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoExperienciaResponseDTO?>(item);
        }

        public async Task<MecanicoExperienciaResponseDTO> CreateAsync(CreateMecanicoExperienciaDTO request)
        {
            var entity = _mapper.Map<MecanicoExperiencia>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoExperienciaResponseDTO>(created);
        }

        public async Task<MecanicoExperienciaResponseDTO?> UpdateAsync(long id, UpdateMecanicoExperienciaDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MecanicoId = request.MecanicoId;
            entity.Empresa = request.Empresa;
            entity.Cargo = request.Cargo;
            entity.DataInicio = request.DataInicio;
            entity.DataFim = request.DataFim;
            entity.ResumoAtividades = request.ResumoAtividades;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoExperienciaResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
