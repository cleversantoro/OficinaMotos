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
    public class EstoquePecaHistoricoService : IEstoquePecaHistoricoService
    {
        private readonly IEstoquePecaHistoricoRepository _repository;
        private readonly IMapper _mapper;

        public EstoquePecaHistoricoService(IEstoquePecaHistoricoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoquePecaHistoricoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoquePecaHistoricoResponseDTO>>(items);
        }

        public async Task<EstoquePecaHistoricoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoquePecaHistoricoResponseDTO?>(item);
        }

        public async Task<EstoquePecaHistoricoResponseDTO> CreateAsync(CreateEstoquePecaHistoricoDTO request)
        {
            var entity = _mapper.Map<EstoquePecaHistorico>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoquePecaHistoricoResponseDTO>(created);
        }

        public async Task<EstoquePecaHistoricoResponseDTO?> UpdateAsync(long id, UpdateEstoquePecaHistoricoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            entity.PecaId = request.PecaId;
            entity.Campo = request.Campo;
            entity.ValorAnterior = request.ValorAnterior;
            entity.ValorNovo = request.ValorNovo;
            entity.UsuarioId = request.UsuarioId;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<EstoquePecaHistoricoResponseDTO>(entity);
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
