using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using OficinaMotos.Application.Interfaces.Cliente;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Cliente
{
    public class ClienteOrigemService : IClienteOrigemService
    {
        private readonly IClienteOrigemRepository _repository;
        private readonly IMapper _mapper;

        public ClienteOrigemService(IClienteOrigemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ClienteOrigemResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<ClienteOrigemResponseDTO>>(items);
        }

        public async Task<ClienteOrigemResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClienteOrigemResponseDTO?>(item);
        }

        public async Task<ClienteOrigemResponseDTO> CreateAsync(CreateClienteOrigemDTO request)
        {
            var entity = _mapper.Map<ClienteOrigem>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ClienteOrigemResponseDTO>(created);
        }

        public async Task<ClienteOrigemResponseDTO?> UpdateAsync(long id, UpdateClienteOrigemDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Nome = request.Nome;
            entity.Descricao = request.Descricao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<ClienteOrigemResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
