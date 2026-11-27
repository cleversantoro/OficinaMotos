using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Estoque;
using OficinaMotos.Application.DTOs.Responses.Estoque;
using OficinaMotos.Application.Interfaces.Estoque;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Estoque
{
    public class EstoqueCategoriaService : IEstoqueCategoriaService
    {
        private readonly IEstoqueCategoriaRepository _repository;
        private readonly IMapper _mapper;

        public EstoqueCategoriaService(IEstoqueCategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoqueCategoriaResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoqueCategoriaResponseDTO>>(items);
        }

        public async Task<EstoqueCategoriaResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoqueCategoriaResponseDTO?>(item);
        }

        public async Task<EstoqueCategoriaResponseDTO> CreateAsync(CreateEstoqueCategoriaDTO request)
        {
            if (await _repository.ExistsByNomeAsync(request.Nome))
            {
                throw new Exception("JÃ¡ existe uma categoria com esse nome.");
            }

            var entity = _mapper.Map<EstoqueCategoria>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoqueCategoriaResponseDTO>(created);
        }

        public async Task<EstoqueCategoriaResponseDTO?> UpdateAsync(long id, UpdateEstoqueCategoriaDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            if (!string.Equals(entity.Nome, request.Nome, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _repository.ExistsByNomeAsync(request.Nome);
                if (exists)
                {
                    throw new Exception("JÃ¡ existe uma categoria com esse nome.");
                }
            }

            entity.Nome = request.Nome;
            entity.Descricao = request.Descricao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);

            return _mapper.Map<EstoqueCategoriaResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
