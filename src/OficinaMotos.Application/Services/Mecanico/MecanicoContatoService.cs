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
    public class MecanicoContatoService : IMecanicoContatoService
    {
        private readonly IMecanicoContatoRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoContatoService(IMecanicoContatoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoContatoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoContatoResponseDTO>>(items);
        }

        public async Task<MecanicoContatoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoContatoResponseDTO?>(item);
        }

        public async Task<MecanicoContatoResponseDTO> CreateAsync(CreateMecanicoContatoDTO request)
        {
            var entity = _mapper.Map<MecanicoContato>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoContatoResponseDTO>(created);
        }

        public async Task<MecanicoContatoResponseDTO?> UpdateAsync(long id, UpdateMecanicoContatoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MecanicoId = request.MecanicoId;
            entity.Tipo = request.Tipo;
            entity.Valor = request.Valor;
            entity.Principal = request.Principal;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoContatoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
