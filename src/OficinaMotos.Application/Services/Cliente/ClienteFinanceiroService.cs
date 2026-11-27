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
    public class ClienteFinanceiroService : IClienteFinanceiroService
    {
        private readonly IClienteFinanceiroRepository _repository;
        private readonly IMapper _mapper;

        public ClienteFinanceiroService(IClienteFinanceiroRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ClienteFinanceiroResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<ClienteFinanceiroResponseDTO>>(items);
        }

        public async Task<ClienteFinanceiroResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClienteFinanceiroResponseDTO?>(item);
        }

        public async Task<ClienteFinanceiroResponseDTO> CreateAsync(CreateClienteFinanceiroDTO request)
        {
            var entity = _mapper.Map<ClienteFinanceiro>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ClienteFinanceiroResponseDTO>(created);
        }

        public async Task<ClienteFinanceiroResponseDTO?> UpdateAsync(long id, UpdateClienteFinanceiroDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ClienteId = request.ClienteId;
            entity.LimiteCredito = request.LimiteCredito;
            entity.PrazoPagamento = request.PrazoPagamento;
            entity.Bloqueado = request.Bloqueado;
            entity.Observacoes = request.Observacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<ClienteFinanceiroResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
