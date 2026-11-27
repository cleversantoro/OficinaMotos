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
    public class PessoaEnderecoService : IPessoaEnderecoService
    {
        private readonly IPessoaEnderecoRepository _repository;
        private readonly IMapper _mapper;

        public PessoaEnderecoService(IPessoaEnderecoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PessoaEnderecoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PessoaEnderecoResponseDTO>>(items);
        }

        public async Task<PessoaEnderecoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<PessoaEnderecoResponseDTO?>(item);
        }

        public async Task<PessoaEnderecoResponseDTO> CreateAsync(CreatePessoaEnderecoDTO request)
        {
            var entity = _mapper.Map<PessoaEndereco>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PessoaEnderecoResponseDTO>(created);
        }

        public async Task<PessoaEnderecoResponseDTO?> UpdateAsync(long id, UpdatePessoaEnderecoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.PessoaId = request.PessoaId;
            entity.Tipo = request.Tipo;
            entity.Cep = request.Cep;
            entity.Logradouro = request.Logradouro;
            entity.Numero = request.Numero;
            entity.Bairro = request.Bairro;
            entity.Cidade = request.Cidade;
            entity.Estado = request.Estado;
            entity.Pais = request.Pais;
            entity.Complemento = request.Complemento;
            entity.Principal = request.Principal;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PessoaEnderecoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
