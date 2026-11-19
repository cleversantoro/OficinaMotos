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
            var result = _mapper.Map<List<ClienteResponseDTO>>(clientes);
            for (var i = 0; i < clientes.Count; i++)
            {
                var pessoa = clientes[i].Pessoa;
                if (pessoa == null) continue;
                result[i].PessoaId = clientes[i].PessoaId;
                result[i].Documento = pessoa.Documento;
                result[i].Nome = pessoa.Nome;
                result[i].NomeExibicao = pessoa.NomeExibicao;
                result[i].Email = pessoa.Contatos.FirstOrDefault(c => c.Tipo.Equals("EMAIL", StringComparison.OrdinalIgnoreCase))?.Valor;
                result[i].Telefone = pessoa.Contatos.FirstOrDefault(c => c.Tipo.Equals("TELEFONE", StringComparison.OrdinalIgnoreCase) || c.Tipo.Equals("CELULAR", StringComparison.OrdinalIgnoreCase))?.Valor;
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
                response.Documento = cliente.Pessoa.Documento;
                response.Nome = cliente.Pessoa.Nome;
                response.NomeExibicao = cliente.Pessoa.NomeExibicao;
                response.Email = cliente.Pessoa.Contatos.FirstOrDefault(c => c.Tipo.Equals("EMAIL", StringComparison.OrdinalIgnoreCase))?.Valor;
                response.Telefone = cliente.Pessoa.Contatos.FirstOrDefault(c => c.Tipo.Equals("TELEFONE", StringComparison.OrdinalIgnoreCase) || c.Tipo.Equals("CELULAR", StringComparison.OrdinalIgnoreCase))?.Valor;
            }
            return response;
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
            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o cliente.");
            cliente.PessoaId = request.PessoaId;
            cliente.Tipo = request.Tipo == 2 ? ClienteTipo.PessoaJuridica : ClienteTipo.PessoaFisica;
            cliente.Documento = doc;
            cliente.NomeExibicao = string.IsNullOrWhiteSpace(request.NomeExibicao) ? request.Nome : request.NomeExibicao;
            var novoCliente = await _repository.AddAsync(cliente);
            await _repository.SyncPessoaContatosAsync(cliente.PessoaId, request.Email, request.Telefone);
            var response = _mapper.Map<ClienteResponseDTO>(novoCliente);
            response.PessoaId = novoCliente.PessoaId;
            response.Documento = doc;
            response.Nome = request.Nome;
            response.NomeExibicao = string.IsNullOrWhiteSpace(request.NomeExibicao) ? request.Nome : request.NomeExibicao;
            response.Email = request.Email;
            response.Telefone = request.Telefone;
            response.Tipo = request.Tipo;
            return response;
        }

        public async Task<ClienteResponseDTO?> UpdateAsync(long id, UpdateClienteDTO request)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null) return null;
            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o cliente.");

            cliente.PessoaId = request.PessoaId;
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
            await _repository.SyncPessoaContatosAsync(cliente.PessoaId, request.Email, request.Telefone);
            var response = _mapper.Map<ClienteResponseDTO>(cliente);
            response.PessoaId = cliente.PessoaId;
            response.Documento = request.Documento;
            response.Nome = request.Nome;
            response.NomeExibicao = string.IsNullOrWhiteSpace(request.NomeExibicao) ? request.Nome : request.NomeExibicao;
            response.Email = request.Email;
            response.Telefone = request.Telefone;
            response.Tipo = request.Tipo;
            return response;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
