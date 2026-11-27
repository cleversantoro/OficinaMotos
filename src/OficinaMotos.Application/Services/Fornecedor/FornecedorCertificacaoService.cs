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
    public class FornecedorCertificacaoService : IFornecedorCertificacaoService
    {
        private readonly IFornecedorCertificacaoRepository _repository;
        private readonly IMapper _mapper;

        public FornecedorCertificacaoService(IFornecedorCertificacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FornecedorCertificacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FornecedorCertificacaoResponseDTO>>(items);
        }

        public async Task<FornecedorCertificacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FornecedorCertificacaoResponseDTO?>(item);
        }

        public async Task<FornecedorCertificacaoResponseDTO> CreateAsync(CreateFornecedorCertificacaoDTO request)
        {
            var entity = _mapper.Map<FornecedorCertificacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FornecedorCertificacaoResponseDTO>(created);
        }

        public async Task<FornecedorCertificacaoResponseDTO?> UpdateAsync(long id, UpdateFornecedorCertificacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.FornecedorId = request.FornecedorId;
            entity.Titulo = request.Titulo;
            entity.Instituicao = request.Instituicao;
            entity.DataEmissao = request.DataEmissao;
            entity.DataValidade = request.DataValidade;
            entity.CodigoCertificacao = request.CodigoCertificacao;
            entity.Escopo = request.Escopo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FornecedorCertificacaoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
