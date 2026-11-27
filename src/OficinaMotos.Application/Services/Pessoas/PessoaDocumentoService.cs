using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Application.Interfaces.Pessoas;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Pessoas
{
    public class PessoaDocumentoService : IPessoaDocumentoService
    {
        private readonly IPessoaDocumentoRepository _repository;
        private readonly IMapper _mapper;

        public PessoaDocumentoService(IPessoaDocumentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PessoaDocumentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PessoaDocumentoResponseDTO>>(items);
        }

        public async Task<PessoaDocumentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<PessoaDocumentoResponseDTO?>(item);
        }

        public async Task<PessoaDocumentoResponseDTO> CreateAsync(CreatePessoaDocumentoDTO request)
        {
            var entity = _mapper.Map<PessoaDocumento>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PessoaDocumentoResponseDTO>(created);
        }

        public async Task<PessoaDocumentoResponseDTO?> UpdateAsync(long id, UpdatePessoaDocumentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PessoaId = request.PessoaId;
            entity.Tipo = request.Tipo;
            entity.Numero = request.Numero;
            entity.DataEmissao = request.DataEmissao;
            entity.DataValidade = request.DataValidade;
            entity.OrgaoExpedidor = request.OrgaoExpedidor;
            entity.ArquivoUrl = request.ArquivoUrl;
            entity.Observacoes = request.Observacoes;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PessoaDocumentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
