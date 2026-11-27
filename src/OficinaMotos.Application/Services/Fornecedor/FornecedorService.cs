using AutoMapper;
using OficinaMotos.Application.Interfaces.Fornecedor;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.Fornecedor;
using FornecedorEntity = OficinaMotos.Domain.Entities.Fornecedor;
using System.Collections.Generic;
using System.Threading.Tasks;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;
using OficinaMotos.Application.DTOs.Responses.Fornecedor;
using OficinaMotos.Domain.Exceptions;

namespace OficinaMotos.Application.Services.Fornecedor
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _repository;
        private readonly IMapper _mapper;

        public FornecedorService(IFornecedorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FornecedorResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FornecedorResponseDTO>>(items);
        }

        public async Task<FornecedorResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FornecedorResponseDTO?>(item);
        }

        public async Task<FornecedorResponseDTO> CreateAsync(CreateFornecedorDTO request)
        {
            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o fornecedor.");
            var entity = _mapper.Map<FornecedorEntity>(request);
            entity.PessoaId = request.PessoaId;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FornecedorResponseDTO>(created);
        }

        public async Task<FornecedorResponseDTO?> UpdateAsync(long id, UpdateFornecedorDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            if (request.PessoaId <= 0)
                throw new DomainException("PessoaId é obrigatório para vincular o fornecedor.");

            entity.PessoaId = request.PessoaId;
            entity.Codigo = request.Codigo;
            entity.Tipo = request.Tipo;
            entity.SegmentoPrincipalId = request.SegmentoPrincipalId;
            entity.Status = request.Status;
            entity.CondicaoPagamentoPadrao = request.CondicaoPagamentoPadrao;
            entity.PrazoEntregaMedio = request.PrazoEntregaMedio;
            entity.NotaMedia = request.NotaMedia;
            entity.PrazoGarantiaPadrao = request.PrazoGarantiaPadrao;
            entity.TermosNegociados = request.TermosNegociados;
            entity.AtendimentoPersonalizado = request.AtendimentoPersonalizado;
            entity.RetiradaLocal = request.RetiradaLocal;
            entity.RatingLogistica = request.RatingLogistica;
            entity.RatingQualidade = request.RatingQualidade;
            entity.Observacoes = request.Observacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FornecedorResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
