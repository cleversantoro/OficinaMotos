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
    public class ClienteIndicacaoService : IClienteIndicacaoService
    {
        private readonly IClienteIndicacaoRepository _repository;
        private readonly IMapper _mapper;

        public ClienteIndicacaoService(IClienteIndicacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ClienteIndicacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<ClienteIndicacaoResponseDTO>>(items);
        }

        public async Task<ClienteIndicacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClienteIndicacaoResponseDTO?>(item);
        }

        public async Task<ClienteIndicacaoResponseDTO> CreateAsync(CreateClienteIndicacaoDTO request)
        {
            var entity = _mapper.Map<ClienteIndicacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ClienteIndicacaoResponseDTO>(created);
        }

        public async Task<ClienteIndicacaoResponseDTO?> UpdateAsync(long id, UpdateClienteIndicacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ClienteId = request.ClienteId;
            entity.IndicadorNome = request.IndicadorNome;
            entity.IndicadorTelefone = request.IndicadorTelefone;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<ClienteIndicacaoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
