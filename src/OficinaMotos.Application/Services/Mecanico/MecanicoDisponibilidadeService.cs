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
    public class MecanicoDisponibilidadeService : IMecanicoDisponibilidadeService
    {
        private readonly IMecanicoDisponibilidadeRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoDisponibilidadeService(IMecanicoDisponibilidadeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoDisponibilidadeResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoDisponibilidadeResponseDTO>>(items);
        }

        public async Task<MecanicoDisponibilidadeResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoDisponibilidadeResponseDTO?>(item);
        }

        public async Task<MecanicoDisponibilidadeResponseDTO> CreateAsync(CreateMecanicoDisponibilidadeDTO request)
        {
            var entity = _mapper.Map<MecanicoDisponibilidade>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoDisponibilidadeResponseDTO>(created);
        }

        public async Task<MecanicoDisponibilidadeResponseDTO?> UpdateAsync(long id, UpdateMecanicoDisponibilidadeDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MecanicoId = request.MecanicoId;
            entity.DiaSemana = request.DiaSemana;
            entity.HoraInicio = request.HoraInicio;
            entity.HoraFim = request.HoraFim;
            entity.CapacidadeAtendimentos = request.CapacidadeAtendimentos;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoDisponibilidadeResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
