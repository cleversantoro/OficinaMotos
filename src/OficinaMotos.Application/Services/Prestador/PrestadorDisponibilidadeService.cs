using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using OficinaMotos.Application.Interfaces.Prestador;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Prestador
{
    public class PrestadorDisponibilidadeService : IPrestadorDisponibilidadeService
    {
        private readonly IPrestadorDisponibilidadeRepository _repository;
        private readonly IMapper _mapper;

        public PrestadorDisponibilidadeService(IPrestadorDisponibilidadeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrestadorDisponibilidadeResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PrestadorDisponibilidadeResponseDTO>>(items);
        }

        public async Task<PrestadorDisponibilidadeResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<PrestadorDisponibilidadeResponseDTO?>(item);
        }

        public async Task<PrestadorDisponibilidadeResponseDTO> CreateAsync(CreatePrestadorDisponibilidadeDTO request)
        {
            var entity = _mapper.Map<PrestadorDisponibilidade>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PrestadorDisponibilidadeResponseDTO>(created);
        }

        public async Task<PrestadorDisponibilidadeResponseDTO?> UpdateAsync(long id, UpdatePrestadorDisponibilidadeDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PrestadorId = request.PrestadorId;
            entity.DiaSemana = request.DiaSemana;
            entity.HoraInicio = request.HoraInicio;
            entity.HoraFim = request.HoraFim;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PrestadorDisponibilidadeResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
