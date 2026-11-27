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
    public class VeiculoMarcaService : IVeiculoMarcaService
    {
        private readonly IVeiculoMarcaRepository _repository;
        private readonly IMapper _mapper;

        public VeiculoMarcaService(IVeiculoMarcaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<VeiculoMarcaResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<VeiculoMarcaResponseDTO>>(items);
        }

        public async Task<VeiculoMarcaResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<VeiculoMarcaResponseDTO?>(item);
        }

        public async Task<VeiculoMarcaResponseDTO> CreateAsync(CreateVeiculoMarcaDTO request)
        {
            var entity = _mapper.Map<VeiculoMarca>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<VeiculoMarcaResponseDTO>(created);
        }

        public async Task<VeiculoMarcaResponseDTO?> UpdateAsync(long id, UpdateVeiculoMarcaDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Nome = request.Nome;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<VeiculoMarcaResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
