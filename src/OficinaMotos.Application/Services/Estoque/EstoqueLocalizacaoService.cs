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
    public class EstoqueLocalizacaoService : IEstoqueLocalizacaoService
    {
        private readonly IEstoqueLocalizacaoRepository _repository;
        private readonly IMapper _mapper;

        public EstoqueLocalizacaoService(IEstoqueLocalizacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoqueLocalizacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoqueLocalizacaoResponseDTO>>(items);
        }

        public async Task<EstoqueLocalizacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoqueLocalizacaoResponseDTO?>(item);
        }

        public async Task<EstoqueLocalizacaoResponseDTO> CreateAsync(CreateEstoqueLocalizacaoDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.Descricao) && await _repository.ExistsByDescricaoAsync(request.Descricao))
            {
                throw new Exception("Já existe uma localização com essa descrição.");
            }

            var entity = _mapper.Map<EstoqueLocalizacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoqueLocalizacaoResponseDTO>(created);
        }

        public async Task<EstoqueLocalizacaoResponseDTO?> UpdateAsync(long id, UpdateEstoqueLocalizacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(request.Descricao) && !string.Equals(entity.Descricao, request.Descricao, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _repository.ExistsByDescricaoAsync(request.Descricao);
                if (exists)
                {
                    throw new Exception("Já existe uma localização com essa descrição.");
                }
            }

            entity.Codigo = request.Codigo;
            entity.Nome = request.Nome;
            entity.Descricao = request.Descricao;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<EstoqueLocalizacaoResponseDTO>(entity);
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
