using AutoMapper;
using OficinaMotos.Application.Interfaces.Veiculo;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using VeiculoEntity = OficinaMotos.Domain.Entities.Veiculo;
using System.Collections.Generic;
using System.Threading.Tasks;
using OficinaMotos.Application.DTOs.Requests.Veiculo;
using OficinaMotos.Application.DTOs.Responses.VeiculoDTO;

namespace OficinaMotos.Application.Services.Veiculo
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IVeiculoRepository _repository;
        private readonly IMapper _mapper;

        public VeiculoService(IVeiculoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<VeiculoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<VeiculoResponseDTO>>(items);
        }

        public async Task<VeiculoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<VeiculoResponseDTO?>(item);
        }

        public async Task<VeiculoResponseDTO> CreateAsync(CreateVeiculoDTO request)
        {
            var entity = _mapper.Map<VeiculoEntity>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<VeiculoResponseDTO>(created);
        }

        public async Task<VeiculoResponseDTO?> UpdateAsync(long id, UpdateVeiculoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.ClienteId = request.ClienteId;
            entity.Placa = request.Placa;
            entity.ModeloId = request.ModeloId;
            entity.AnoFab = request.AnoFab;
            entity.AnoMod = request.AnoMod;
            entity.Cor = request.Cor;
            entity.Chassi = request.Chassi;
            entity.Renavam = request.Renavam;
            entity.Km = request.Km;
            entity.ProximoKmRevisao = request.ProximoKmRevisao;
            entity.Combustivel = request.Combustivel;
            entity.Observacao = request.Observacao;
            entity.Principal = request.Principal;
            entity.Ativo = request.Ativo;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<VeiculoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
