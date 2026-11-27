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
    public class AtividadeProfissionalService : IAtividadeProfissionalService
    {
        private readonly IAtividadeProfissionalRepository _repository;
        private readonly IMapper _mapper;

        public AtividadeProfissionalService(IAtividadeProfissionalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AtividadeProfissionalResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<AtividadeProfissionalResponseDTO>>(items);
        }

        public async Task<AtividadeProfissionalResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<AtividadeProfissionalResponseDTO?>(item);
        }

        public async Task<AtividadeProfissionalResponseDTO> CreateAsync(CreateAtividadeProfissionalDTO request)
        {
            var entity = _mapper.Map<AtividadeProfissional>(request);
            entity.Codigo = request.Codigo.Trim();
            entity.Nome = request.Nome.Trim();
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<AtividadeProfissionalResponseDTO>(created);
        }

        public async Task<AtividadeProfissionalResponseDTO?> UpdateAsync(long id, UpdateAtividadeProfissionalDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Codigo = request.Codigo.Trim();
            entity.Nome = request.Nome.Trim();
            entity.Descricao = request.Descricao;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<AtividadeProfissionalResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
