using AutoMapper;
using OficinaMotos.Application.Interfaces.Mecanico;
using OficinaMotos.Domain.Entities;
using MecanicoEntity = OficinaMotos.Domain.Entities.Mecanico;
using System.Collections.Generic;
using System.Threading.Tasks;
using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.DTOs.Responses.Mecanico;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;

namespace OficinaMotos.Application.Services.Mecanico
{
    public class MecanicoService : IMecanicoService
    {
        private readonly IMecanicoRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoService(IMecanicoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoResponseDTO>>(items);
        }

        public async Task<MecanicoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoResponseDTO?>(item);
        }

        public async Task<MecanicoResponseDTO> CreateAsync(CreateMecanicoDTO request)
        {
            var entity = _mapper.Map<MecanicoEntity>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoResponseDTO>(created);
        }

        public async Task<MecanicoResponseDTO?> UpdateAsync(long id, UpdateMecanicoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Codigo = request.Codigo;
            entity.Nome = request.Nome;
            entity.Sobrenome = request.Sobrenome;
            entity.NomeSocial = request.NomeSocial;
            entity.DocumentoPrincipal = request.DocumentoPrincipal;
            entity.TipoDocumento = request.TipoDocumento;
            entity.DataNascimento = request.DataNascimento;
            entity.DataAdmissao = request.DataAdmissao;
            entity.DataDemissao = request.DataDemissao;
            entity.Status = request.Status;
            entity.EspecialidadePrincipalId = request.EspecialidadePrincipalId;
            entity.Nivel = request.Nivel;
            entity.ValorHora = request.ValorHora;
            entity.CargaHorariaSemanal = request.CargaHorariaSemanal;
            entity.Observacoes = request.Observacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
