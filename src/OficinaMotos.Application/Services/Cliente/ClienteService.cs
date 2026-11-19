using AutoMapper;
using OficinaMotos.Application.Interfaces.Cliente;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using ClienteEntity = OficinaMotos.Domain.Entities.Cliente;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;

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
            return _mapper.Map<List<ClienteResponseDTO>>(clientes);
        }

        public async Task<List<ClienteResponseTableDTO>> GetAllForTableAsync()
        {
            var clientes = await _repository.GetAllForTableAsync();
            return _mapper.Map<List<ClienteResponseTableDTO>>(clientes);
        }

        public async Task<ClienteResponseDTO?> GetByIdAsync(long id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClienteResponseDTO?>(cliente);
        }

        public async Task<ClienteResponseDTO> CreateAsync(CreateClienteDTO request)
        {
            var doc = !string.IsNullOrWhiteSpace(request.Documento) ? request.Documento : request.Cpf;
            var clienteExistente = await _repository.GetByCpfAsync(doc);
            if (clienteExistente != null)
            {
                var tipoDoc = request.Tipo == 2 ? "CNPJ" : "CPF";
                throw new DomainException($"Ja existe um cliente cadastrado com este {tipoDoc}.");
            }

            var cliente = _mapper.Map<ClienteEntity>(request);
            cliente.Tipo = request.Tipo == 2 ? ClienteTipo.PessoaJuridica : ClienteTipo.PessoaFisica;
            cliente.Documento = doc;
            cliente.NomeExibicao = string.IsNullOrWhiteSpace(request.NomeExibicao) ? request.Nome : request.NomeExibicao;
            var novoCliente = await _repository.AddAsync(cliente);

            return _mapper.Map<ClienteResponseDTO>(novoCliente);
        }

        public async Task<ClienteResponseDTO?> UpdateAsync(long id, UpdateClienteDTO request)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null) return null;

            cliente.Nome = request.Nome;
            cliente.NomeExibicao = string.IsNullOrWhiteSpace(request.NomeExibicao) ? request.Nome : request.NomeExibicao;
            cliente.Documento = request.Documento;
            cliente.Tipo = (ClienteTipo)request.Tipo;
            cliente.Status = (ClienteStatus)request.Status;
            cliente.Vip = request.Vip;
            cliente.Observacoes = request.Observacoes;
            cliente.OrigemCadastroId = request.OrigemCadastroId;
            cliente.Telefone = request.Telefone;
            cliente.Email = request.Email;
            cliente.OrigemId = request.OrigemId;
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
