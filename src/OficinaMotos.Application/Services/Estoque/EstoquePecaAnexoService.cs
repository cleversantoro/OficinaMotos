using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Estoque;
using OficinaMotos.Application.DTOs.Responses.Estoque;
using OficinaMotos.Application.Interfaces.Estoque;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Estoque
{
    public class EstoquePecaAnexoService : IEstoquePecaAnexoService
    {
        private readonly IEstoquePecaAnexoRepository _repository;
        private readonly IMapper _mapper;

        public EstoquePecaAnexoService(IEstoquePecaAnexoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoquePecaAnexoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoquePecaAnexoResponseDTO>>(items);
        }

        public async Task<EstoquePecaAnexoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoquePecaAnexoResponseDTO?>(item);
        }

        public async Task<EstoquePecaAnexoResponseDTO> CreateAsync(CreateEstoquePecaAnexoDTO request)
        {
            var entity = _mapper.Map<EstoquePecaAnexo>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoquePecaAnexoResponseDTO>(created);
        }

        public async Task<EstoquePecaAnexoResponseDTO?> UpdateAsync(long id, UpdateEstoquePecaAnexoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            entity.PecaId = request.PecaId;
            entity.Nome = request.Nome;
            entity.Tipo = request.Tipo;
            entity.Url = request.Url;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<EstoquePecaAnexoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
