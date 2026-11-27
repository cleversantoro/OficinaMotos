using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;
using OficinaMotos.Application.DTOs.Responses.Fornecedor;
using OficinaMotos.Application.Interfaces.Fornecedor;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.Fornecedor;
using System.Collections.Generic;
using System.Threading.Tasks;
using OficinaMotos.Domain.Exceptions;

namespace OficinaMotos.Application.Services.Fornecedor
{
    public class FornecedorRepresentanteService : IFornecedorRepresentanteService
    {
        private readonly IFornecedorRepresentanteRepository _repository;
        private readonly IMapper _mapper;

        public FornecedorRepresentanteService(IFornecedorRepresentanteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FornecedorRepresentanteResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FornecedorRepresentanteResponseDTO>>(items);
        }

        public async Task<FornecedorRepresentanteResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FornecedorRepresentanteResponseDTO?>(item);
        }

        public async Task<FornecedorRepresentanteResponseDTO> CreateAsync(CreateFornecedorRepresentanteDTO request)
        {
            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o representante.");
            var entity = _mapper.Map<FornecedorRepresentante>(request);
            entity.PessoaId = request.PessoaId;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FornecedorRepresentanteResponseDTO>(created);
        }

        public async Task<FornecedorRepresentanteResponseDTO?> UpdateAsync(long id, UpdateFornecedorRepresentanteDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o representante.");

            entity.FornecedorId = request.FornecedorId;
            entity.PessoaId = request.PessoaId;
            entity.Cargo = request.Cargo;
            entity.Principal = request.Principal;
            entity.Preferencial = request.Preferencial;
            entity.Observacoes = request.Observacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FornecedorRepresentanteResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
