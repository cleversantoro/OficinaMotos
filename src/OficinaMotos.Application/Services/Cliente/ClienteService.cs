using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using OficinaMotos.Application.Interfaces.Cliente;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClienteEntity = OficinaMotos.Domain.Entities.Cliente;

namespace OficinaMotos.Application.Services.Cliente
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ClienteResponseDTO>> GetAllAsync()
        {
            var clientes = await _repository.GetAllAsync();
            var result = _mapper.Map<List<ClienteResponseDTO>>(clientes);
            for (var i = 0; i < clientes.Count; i++)
            {
                var pessoa = clientes[i].Pessoa;
                if (pessoa == null) continue;
                result[i].PessoaId = clientes[i].PessoaId;
                if (result[i].Pessoa != null)
                {
                    result[i].Pessoa!.Documento = pessoa.Documento;
                    result[i].Pessoa!.Nome = pessoa.Nome;
                    result[i].Pessoa!.NomeExibicao = pessoa.NomeExibicao;
                }
            }
            return result;
        }

        public async Task<List<ClienteResponseTableDTO>> GetAllForTableAsync()
        {
            var clientes = await _repository.GetAllForTableAsync();
            return _mapper.Map<List<ClienteResponseTableDTO>>(clientes);
        }

        public async Task<ClienteResponseDTO?> GetByIdAsync(long id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<ClienteResponseDTO?>(cliente);
            if (response != null && cliente?.Pessoa != null)
            {
                response.PessoaId = cliente.PessoaId;
                if (response.Pessoa != null)
                {
                    response.Pessoa.Documento = cliente.Pessoa.Documento;
                    response.Pessoa.Nome = cliente.Pessoa.Nome;
                    response.Pessoa.NomeExibicao = cliente.Pessoa.NomeExibicao;
                }
            }
            return response;
        }

        public async Task<ClienteResponseDTO> CreateAsync(CreateClienteDTO request)
        {
            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o cliente.");

            var cliente = _mapper.Map<ClienteEntity>(request);
            cliente.Pessoa = null;
            cliente.Origem = null;

            var novoCliente = await _repository.AddAsync(cliente);
            return _mapper.Map<ClienteResponseDTO>(novoCliente);
        }

        public async Task<ClienteResponseDTO?> UpdateAsync(long id, UpdateClienteDTO request)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null) return null;

            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o cliente.");

            cliente.Codigo = request.Codigo;
            cliente.PessoaId = request.PessoaId;
            cliente.OrigemId = request.OrigemId;
            cliente.Status = request.Status;
            cliente.Vip = request.Vip;
            cliente.Observacoes = request.Observacoes;
            cliente.SetUpdated();

            await _repository.UpdateAsync(cliente);
            return _mapper.Map<ClienteResponseDTO>(cliente);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
