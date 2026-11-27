using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using OficinaMotos.Application.Interfaces.Prestador;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Prestador
{
    public class PrestadorExperienciaService : IPrestadorExperienciaService
    {
        private readonly IPrestadorExperienciaRepository _repository;
        private readonly IMapper _mapper;

        public PrestadorExperienciaService(IPrestadorExperienciaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrestadorExperienciaResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PrestadorExperienciaResponseDTO>>(items);
        }

        public async Task<PrestadorExperienciaResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<PrestadorExperienciaResponseDTO?>(item);
        }

        public async Task<PrestadorExperienciaResponseDTO> CreateAsync(CreatePrestadorExperienciaDTO request)
        {
            var entity = _mapper.Map<PrestadorExperiencia>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PrestadorExperienciaResponseDTO>(created);
        }

        public async Task<PrestadorExperienciaResponseDTO?> UpdateAsync(long id, UpdatePrestadorExperienciaDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PrestadorId = request.PrestadorId;
            entity.Empresa = request.Empresa;
            entity.Cargo = request.Cargo;
            entity.Descricao = request.Descricao;
            entity.DataInicio = request.DataInicio;
            entity.DataFim = request.DataFim;
            entity.Atual = request.Atual;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PrestadorExperienciaResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
