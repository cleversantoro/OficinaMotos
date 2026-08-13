using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.OrdemServicoRepo
{
    public class OrdemServicoService : IOrdemServicoService
    {
        private readonly IOrdemServicoRepository _repository;
        private readonly IMapper _mapper;

        public OrdemServicoService(IOrdemServicoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrdemServicoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoResponseDTO>>(items);
        }

        public async Task<OrdemServicoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoResponseDTO?>(item);
        }

        public async Task<OrdemServicoResponseDTO> CreateAsync(CreateOrdemServicoDTO request)
        {
            var entity = _mapper.Map<OrdemServico>(request);
            entity.VeiculoId = request.VeiculoId;
            if (request.DataAbertura.HasValue)
            {
                entity.DataAbertura = request.DataAbertura.Value;
            }
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoResponseDTO>(created);
        }

        public async Task<OrdemServicoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ClienteId = request.ClienteId;
            entity.MecanicoId = request.MecanicoId;
            entity.VeiculoId = request.VeiculoId;
            entity.DescricaoProblema = request.DescricaoProblema;
            entity.Status = request.Status;
            entity.DataAbertura = request.DataAbertura ?? entity.DataAbertura;
            entity.DataConclusao = request.DataConclusao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
