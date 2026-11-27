using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;
using OficinaMotos.Application.DTOs.Responses.Fornecedor;
using OficinaMotos.Application.Interfaces.Fornecedor;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Fornecedor
{
    public class FornecedorBancoService : IFornecedorBancoService
    {
        private readonly IFornecedorBancoRepository _repository;
        private readonly IMapper _mapper;

        public FornecedorBancoService(IFornecedorBancoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FornecedorBancoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FornecedorBancoResponseDTO>>(items);
        }

        public async Task<FornecedorBancoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FornecedorBancoResponseDTO?>(item);
        }

        public async Task<FornecedorBancoResponseDTO> CreateAsync(CreateFornecedorBancoDTO request)
        {
            var entity = _mapper.Map<FornecedorBanco>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FornecedorBancoResponseDTO>(created);
        }

        public async Task<FornecedorBancoResponseDTO?> UpdateAsync(long id, UpdateFornecedorBancoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.FornecedorId = request.FornecedorId;
            entity.Banco = request.Banco;
            entity.Agencia = request.Agencia;
            entity.Conta = request.Conta;
            entity.Digito = request.Digito;
            entity.TipoConta = request.TipoConta;
            entity.PixChave = request.PixChave;
            entity.Observacoes = request.Observacoes;
            entity.Principal = request.Principal;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FornecedorBancoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
