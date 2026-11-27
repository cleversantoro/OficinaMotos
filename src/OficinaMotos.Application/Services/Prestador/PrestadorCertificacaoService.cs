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
    public class PrestadorCertificacaoService : IPrestadorCertificacaoService
    {
        private readonly IPrestadorCertificacaoRepository _repository;
        private readonly IMapper _mapper;

        public PrestadorCertificacaoService(IPrestadorCertificacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrestadorCertificacaoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PrestadorCertificacaoResponseDTO>>(items);
        }

        public async Task<PrestadorCertificacaoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<PrestadorCertificacaoResponseDTO?>(item);
        }

        public async Task<PrestadorCertificacaoResponseDTO> CreateAsync(CreatePrestadorCertificacaoDTO request)
        {
            var entity = _mapper.Map<PrestadorCertificacao>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PrestadorCertificacaoResponseDTO>(created);
        }

        public async Task<PrestadorCertificacaoResponseDTO?> UpdateAsync(long id, UpdatePrestadorCertificacaoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PrestadorId = request.PrestadorId;
            entity.Titulo = request.Titulo;
            entity.Instituicao = request.Instituicao;
            entity.DataEmissao = request.DataEmissao;
            entity.DataValidade = request.DataValidade;
            entity.CodigoCertificacao = request.CodigoCertificacao;
            entity.ArquivoUrl = request.ArquivoUrl;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PrestadorCertificacaoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
