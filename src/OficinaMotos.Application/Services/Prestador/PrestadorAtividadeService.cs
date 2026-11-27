using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using OficinaMotos.Application.Interfaces.Prestador;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Prestador
{
    public class PrestadorAtividadeService : IPrestadorAtividadeService
    {
        private readonly IPrestadorAtividadeRepository _repository;
        private readonly IMapper _mapper;

        public PrestadorAtividadeService(IPrestadorAtividadeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrestadorAtividadeResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PrestadorAtividadeResponseDTO>>(items);
        }

        public async Task<PrestadorAtividadeResponseDTO?> GetByIdAsync(long prestadorId, long atividadeProfissionalId)
        {
            var item = await _repository.GetByIdAsync(prestadorId, atividadeProfissionalId);
            return _mapper.Map<PrestadorAtividadeResponseDTO?>(item);
        }

        public async Task<PrestadorAtividadeResponseDTO> CreateAsync(CreatePrestadorAtividadeDTO request)
        {
            var entity = _mapper.Map<PrestadorAtividade>(request);
            entity.CreatedAt = DateTime.UtcNow;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PrestadorAtividadeResponseDTO>(created);
        }

        public async Task<PrestadorAtividadeResponseDTO?> UpdateAsync(long prestadorId, long atividadeProfissionalId, UpdatePrestadorAtividadeDTO request)
        {
            var entity = await _repository.GetByIdAsync(prestadorId, atividadeProfissionalId);
            if (entity == null) return null;

            entity.Nivel = request.Nivel;
            entity.Principal = request.Principal;
            entity.Ativo = request.Ativo;

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PrestadorAtividadeResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long prestadorId, long atividadeProfissionalId)
        {
            if (!await _repository.ExistsAsync(prestadorId, atividadeProfissionalId)) return false;
            await _repository.DeleteAsync(prestadorId, atividadeProfissionalId);
            return true;
        }
    }
}
