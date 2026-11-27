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
    public class FornecedorSegmentoService : IFornecedorSegmentoService
    {
        private readonly IFornecedorSegmentoRepository _repository;
        private readonly IMapper _mapper;

        public FornecedorSegmentoService(IFornecedorSegmentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FornecedorSegmentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FornecedorSegmentoResponseDTO>>(items);
        }

        public async Task<FornecedorSegmentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FornecedorSegmentoResponseDTO?>(item);
        }

        public async Task<FornecedorSegmentoResponseDTO> CreateAsync(CreateFornecedorSegmentoDTO request)
        {
            var entity = _mapper.Map<FornecedorSegmento>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FornecedorSegmentoResponseDTO>(created);
        }

        public async Task<FornecedorSegmentoResponseDTO?> UpdateAsync(long id, UpdateFornecedorSegmentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Nome = request.Nome;
            entity.Descricao = request.Descricao;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FornecedorSegmentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
