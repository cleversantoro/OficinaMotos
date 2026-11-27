using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.OrdemServicoRepo
{
    public class OrdemServicoHistoricoService : IOrdemServicoHistoricoService
    {
        private readonly IOrdemServicoHistoricoRepository _repository;
        private readonly IMapper _mapper;

        public OrdemServicoHistoricoService(IOrdemServicoHistoricoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrdemServicoHistoricoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoHistoricoResponseDTO>>(items);
        }

        public async Task<OrdemServicoHistoricoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoHistoricoResponseDTO?>(item);
        }

        public async Task<OrdemServicoHistoricoResponseDTO> CreateAsync(CreateOrdemServicoHistoricoDTO request)
        {
            var entity = _mapper.Map<OrdemServicoHistorico>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoHistoricoResponseDTO>(created);
        }

        public async Task<OrdemServicoHistoricoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoHistoricoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            entity.DataAlteracao = request.DataAlteracao;
            entity.Usuario = request.Usuario;
            entity.Campo = request.Campo;
            entity.ValorAntigo = request.ValorAntigo;
            entity.ValorNovo = request.ValorNovo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoHistoricoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
