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
    public class FornecedorAvaliacaoService : IFornecedorAvaliacaoService
    {
        private readonly IFornecedorAvaliacaoRepository _repository;
        private readonly IMapper _mapper;

        public FornecedorAvaliacaoService(IFornecedorAvaliacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FornecedorAvaliacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FornecedorAvaliacaoResponseDTO>>(items);
        }

        public async Task<FornecedorAvaliacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FornecedorAvaliacaoResponseDTO?>(item);
        }

        public async Task<FornecedorAvaliacaoResponseDTO> CreateAsync(CreateFornecedorAvaliacaoDTO request)
        {
            var entity = _mapper.Map<FornecedorAvaliacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FornecedorAvaliacaoResponseDTO>(created);
        }

        public async Task<FornecedorAvaliacaoResponseDTO?> UpdateAsync(long id, UpdateFornecedorAvaliacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.FornecedorId = request.FornecedorId;
            entity.DataAvaliacao = request.DataAvaliacao;
            entity.AvaliadoPor = request.AvaliadoPor;
            entity.Categoria = request.Categoria;
            entity.Nota = request.Nota;
            entity.Comentarios = request.Comentarios;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FornecedorAvaliacaoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
