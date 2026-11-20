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
    public class MecanicoCertificacaoService : IMecanicoCertificacaoService
    {
        private readonly IMecanicoCertificacaoRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoCertificacaoService(IMecanicoCertificacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoCertificacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoCertificacaoResponseDTO>>(items);
        }

        public async Task<MecanicoCertificacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoCertificacaoResponseDTO?>(item);
        }

        public async Task<MecanicoCertificacaoResponseDTO> CreateAsync(CreateMecanicoCertificacaoDTO request)
        {
            var entity = _mapper.Map<MecanicoCertificacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoCertificacaoResponseDTO>(created);
        }

        public async Task<MecanicoCertificacaoResponseDTO?> UpdateAsync(long id, UpdateMecanicoCertificacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MecanicoId = request.MecanicoId;
            entity.EspecialidadeId = request.EspecialidadeId;
            entity.Titulo = request.Titulo;
            entity.Instituicao = request.Instituicao;
            entity.DataConclusao = request.DataConclusao;
            entity.DataValidade = request.DataValidade;
            entity.CodigoCertificacao = request.CodigoCertificacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoCertificacaoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
