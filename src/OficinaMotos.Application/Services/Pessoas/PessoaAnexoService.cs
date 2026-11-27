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
    public class PessoaAnexoService : IPessoaAnexoService
    {
        private readonly IPessoaAnexoRepository _repository;
        private readonly IMapper _mapper;

        public PessoaAnexoService(IPessoaAnexoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PessoaAnexoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PessoaAnexoResponseDTO>>(items);
        }

        public async Task<PessoaAnexoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<PessoaAnexoResponseDTO?>(item);
        }

        public async Task<PessoaAnexoResponseDTO> CreateAsync(CreatePessoaAnexoDTO request)
        {
            var entity = _mapper.Map<PessoaAnexo>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PessoaAnexoResponseDTO>(created);
        }

        public async Task<PessoaAnexoResponseDTO?> UpdateAsync(long id, UpdatePessoaAnexoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PessoaId = request.PessoaId;
            entity.Nome = request.Nome;
            entity.Tipo = request.Tipo;
            entity.Url = request.Url;
            entity.Observacao = request.Observacao;
            entity.TamanhoBytes = request.TamanhoBytes;
            entity.MimeType = request.MimeType;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PessoaAnexoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
