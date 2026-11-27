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
    public class PessoaContatoService : IPessoaContatoService
    {
        private readonly IPessoaContatoRepository _repository;
        private readonly IMapper _mapper;

        public PessoaContatoService(IPessoaContatoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PessoaContatoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PessoaContatoResponseDTO>>(items);
        }

        public async Task<PessoaContatoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<PessoaContatoResponseDTO?>(item);
        }

        public async Task<PessoaContatoResponseDTO> CreateAsync(CreatePessoaContatoDTO request)
        {
            var entity = _mapper.Map<PessoaContato>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PessoaContatoResponseDTO>(created);
        }

        public async Task<PessoaContatoResponseDTO?> UpdateAsync(long id, UpdatePessoaContatoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PessoaId = request.PessoaId;
            entity.Tipo = request.Tipo;
            entity.Valor = request.Valor;
            entity.Principal = request.Principal;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PessoaContatoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
