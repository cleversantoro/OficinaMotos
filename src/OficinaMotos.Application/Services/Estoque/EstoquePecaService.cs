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
    public class EstoquePecaService : IEstoquePecaService
    {
        private readonly IEstoquePecaRepository _repository;
        private readonly IMapper _mapper;

        public EstoquePecaService(IEstoquePecaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoquePecaResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoquePecaResponseDTO>>(items);
        }

        public async Task<EstoquePecaResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoquePecaResponseDTO?>(item);
        }

        public async Task<EstoquePecaResponseDTO> CreateAsync(CreateEstoquePecaDTO request)
        {
            if (await _repository.ExistsByCodigoAsync(request.Codigo))
            {
                throw new Exception("JÃ¡ existe uma peÃ§a com esse cÃ³digo.");
            }

            var entity = _mapper.Map<EstoquePeca>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoquePecaResponseDTO>(created);
        }

        public async Task<EstoquePecaResponseDTO?> UpdateAsync(long id, UpdateEstoquePecaDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            if (!string.Equals(entity.Codigo, request.Codigo, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _repository.ExistsByCodigoAsync(request.Codigo);
                if (exists)
                {
                    throw new Exception("JÃ¡ existe uma peÃ§a com esse cÃ³digo.");
                }
            }

            entity.Codigo = request.Codigo;
            entity.CodigoBarras = request.CodigoBarras;
            entity.Nome = request.Nome;
            entity.Descricao = request.Descricao;
            entity.FabricanteId = request.FabricanteId;
            entity.CategoriaId = request.CategoriaId;
            entity.LocalizacaoId = request.LocalizacaoId;
            entity.Unidade = request.Unidade;
            entity.QuantidadeEstoque = request.QuantidadeEstoque;
            entity.EstoqueMinimo = request.EstoqueMinimo;
            entity.EstoqueMaximo = request.EstoqueMaximo;
            entity.CustoMedio = request.CustoMedio;
            entity.PrecoVenda = request.PrecoVenda;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<EstoquePecaResponseDTO>(entity);
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
