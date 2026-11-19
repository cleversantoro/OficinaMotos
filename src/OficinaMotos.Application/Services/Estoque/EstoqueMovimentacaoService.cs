using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Estoque;
using OficinaMotos.Application.DTOs.Responses.Estoque;
using OficinaMotos.Application.Interfaces.Estoque;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.Estoque;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Estoque
{
    public class EstoqueMovimentacaoService : IEstoqueMovimentacaoService
    {
        private readonly IEstoqueMovimentacaoRepository _repository;
        private readonly IMapper _mapper;

        public EstoqueMovimentacaoService(IEstoqueMovimentacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoqueMovimentacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoqueMovimentacaoResponseDTO>>(items);
        }

        public async Task<EstoqueMovimentacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoqueMovimentacaoResponseDTO?>(item);
        }

        public async Task<EstoqueMovimentacaoResponseDTO> CreateAsync(CreateEstoqueMovimentacaoDTO request)
        {
            var entity = _mapper.Map<EstoqueMovimentacao>(request);
            entity.QuantidadeDecimal = request.Quantidade;
            entity.CustoUnitario = request.CustoUnitario;
            entity.OrigemTipo = request.OrigemTipo;
            entity.OrigemId = request.OrigemId;
            entity.UsuarioId = request.UsuarioId;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoqueMovimentacaoResponseDTO>(created);
        }

        public async Task<EstoqueMovimentacaoResponseDTO?> UpdateAsync(long id, UpdateEstoqueMovimentacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            entity.PecaId = request.PecaId;
            entity.Quantidade = request.Quantidade;
            entity.Tipo = request.Tipo;
            entity.Referencia = request.Referencia;
            entity.Usuario = request.Usuario;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<EstoqueMovimentacaoResponseDTO>(entity);
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
