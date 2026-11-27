using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Estoque;
using OficinaMotos.Application.DTOs.Responses.Estoque;
using OficinaMotos.Application.Interfaces.Estoque;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.Estoque;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Estoque
{
    public class EstoqueFabricanteService : IEstoqueFabricanteService
    {
        private readonly IEstoqueFabricanteRepository _repository;
        private readonly IMapper _mapper;

        public EstoqueFabricanteService(IEstoqueFabricanteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoqueFabricanteResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoqueFabricanteResponseDTO>>(items);
        }

        public async Task<EstoqueFabricanteResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoqueFabricanteResponseDTO?>(item);
        }

        public async Task<EstoqueFabricanteResponseDTO> CreateAsync(CreateEstoqueFabricanteDTO request)
        {
            if (await _repository.ExistsByNomeAsync(request.Nome))
            {
                throw new Exception("Já existe um fabricante com esse nome.");
            }

            var entity = _mapper.Map<EstoqueFabricante>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoqueFabricanteResponseDTO>(created);
        }

        public async Task<EstoqueFabricanteResponseDTO?> UpdateAsync(long id, UpdateEstoqueFabricanteDTO request)
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
                    throw new Exception("Já existe um fabricante com esse nome.");
                }
            }

            entity.Nome = request.Nome;
            entity.Documento = request.Documento;
            entity.Website = request.Website;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<EstoqueFabricanteResponseDTO>(entity);
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
