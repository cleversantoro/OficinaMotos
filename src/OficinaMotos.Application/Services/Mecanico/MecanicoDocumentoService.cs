using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.DTOs.Responses.Mecanico;
using OficinaMotos.Application.Interfaces.Mecanico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Mecanico
{
    public class MecanicoDocumentoService : IMecanicoDocumentoService
    {
        private readonly IMecanicoDocumentoRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoDocumentoService(IMecanicoDocumentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoDocumentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoDocumentoResponseDTO>>(items);
        }

        public async Task<MecanicoDocumentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoDocumentoResponseDTO?>(item);
        }

        public async Task<MecanicoDocumentoResponseDTO> CreateAsync(CreateMecanicoDocumentoDTO request)
        {
            var entity = _mapper.Map<MecanicoDocumento>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoDocumentoResponseDTO>(created);
        }

        public async Task<MecanicoDocumentoResponseDTO?> UpdateAsync(long id, UpdateMecanicoDocumentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MecanicoId = request.MecanicoId;
            entity.Tipo = request.Tipo;
            entity.Numero = request.Numero;
            entity.DataEmissao = request.DataEmissao;
            entity.DataValidade = request.DataValidade;
            entity.OrgaoExpedidor = request.OrgaoExpedidor;
            entity.ArquivoUrl = request.ArquivoUrl;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoDocumentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
