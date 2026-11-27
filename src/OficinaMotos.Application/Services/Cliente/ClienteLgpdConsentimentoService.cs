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
    public class ClienteLgpdConsentimentoService : IClienteLgpdConsentimentoService
    {
        private readonly IClienteLgpdConsentimentoRepository _repository;
        private readonly IMapper _mapper;

        public ClienteLgpdConsentimentoService(IClienteLgpdConsentimentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ClienteLgpdConsentimentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<ClienteLgpdConsentimentoResponseDTO>>(items);
        }

        public async Task<ClienteLgpdConsentimentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClienteLgpdConsentimentoResponseDTO?>(item);
        }

        public async Task<ClienteLgpdConsentimentoResponseDTO> CreateAsync(CreateClienteLgpdConsentimentoDTO request)
        {
            var entity = _mapper.Map<ClienteLgpdConsentimento>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ClienteLgpdConsentimentoResponseDTO>(created);
        }

        public async Task<ClienteLgpdConsentimentoResponseDTO?> UpdateAsync(long id, UpdateClienteLgpdConsentimentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ClienteId = request.ClienteId;
            entity.Tipo = request.Tipo;
            entity.Aceito = request.Aceito;
            entity.DataConsentimento = request.DataConsentimento;
            entity.ValidoAte = request.ValidoAte;
            entity.Observacoes = request.Observacoes;
            entity.Canal = request.Canal;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<ClienteLgpdConsentimentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
