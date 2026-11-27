using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Application.Interfaces.Pessoas;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Pessoas
{
    public class PessoaPfService : IPessoaPfService
    {
        private readonly IPessoaPfRepository _repository;
        private readonly IMapper _mapper;

        public PessoaPfService(IPessoaPfRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PessoaPfResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PessoaPfResponseDTO>>(items);
        }

        public async Task<PessoaPfResponseDTO?> GetByPessoaIdAsync(long pessoaId)
        {
            var item = await _repository.GetByPessoaIdAsync(pessoaId);
            return _mapper.Map<PessoaPfResponseDTO?>(item);
        }

        public async Task<PessoaPfResponseDTO> CreateAsync(CreatePessoaPfDTO request)
        {
            var entity = _mapper.Map<PessoaPf>(request);
            entity.CreatedAt = DateTime.UtcNow;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PessoaPfResponseDTO>(created);
        }

        public async Task<PessoaPfResponseDTO?> UpdateAsync(long pessoaId, UpdatePessoaPfDTO request)
        {
            var entity = await _repository.GetByPessoaIdAsync(pessoaId);
            if (entity == null) return null;

            entity.Rg = request.Rg;
            entity.DataNascimento = request.DataNascimento;
            entity.Genero = request.Genero;
            entity.EstadoCivil = request.EstadoCivil;
            entity.Profissao = request.Profissao;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PessoaPfResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long pessoaId)
        {
            if (!await _repository.ExistsAsync(pessoaId)) return false;
            await _repository.DeleteAsync(pessoaId);
            return true;
        }
    }
}
