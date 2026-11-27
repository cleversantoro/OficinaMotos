using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Veiculo;
using OficinaMotos.Application.DTOs.Responses.Veiculo;
using OficinaMotos.Application.Interfaces.Veiculo;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Veiculo
{
    public class VeiculoModeloService : IVeiculoModeloService
    {
        private readonly IVeiculoModeloRepository _repository;
        private readonly IMapper _mapper;

        public VeiculoModeloService(IVeiculoModeloRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<VeiculoModeloResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<VeiculoModeloResponseDTO>>(items);
        }

        public async Task<VeiculoModeloResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<VeiculoModeloResponseDTO?>(item);
        }

        public async Task<VeiculoModeloResponseDTO> CreateAsync(CreateVeiculoModeloDTO request)
        {
            var entity = _mapper.Map<VeiculoModelo>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<VeiculoModeloResponseDTO>(created);
        }

        public async Task<VeiculoModeloResponseDTO?> UpdateAsync(long id, UpdateVeiculoModeloDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MarcaId = request.MarcaId;
            entity.Nome = request.Nome;
            //entity.AnoInicio = request.AnoInicio;
            //entity.AnoFim = request.AnoFim;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<VeiculoModeloResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
