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
    public class PessoaPjService : IPessoaPjService
    {
        private readonly IPessoaPjRepository _repository;
        private readonly IMapper _mapper;

        public PessoaPjService(IPessoaPjRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PessoaPjResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<PessoaPjResponseDTO>>(items);
        }

        public async Task<PessoaPjResponseDTO?> GetByPessoaIdAsync(long pessoaId)
        {
            var item = await _repository.GetByPessoaIdAsync(pessoaId);
            return _mapper.Map<PessoaPjResponseDTO?>(item);
        }

        public async Task<PessoaPjResponseDTO> CreateAsync(CreatePessoaPjDTO request)
        {
            var entity = _mapper.Map<PessoaPj>(request);
            entity.CreatedAt = DateTime.UtcNow;
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<PessoaPjResponseDTO>(created);
        }

        public async Task<PessoaPjResponseDTO?> UpdateAsync(long pessoaId, UpdatePessoaPjDTO request)
        {
            var entity = await _repository.GetByPessoaIdAsync(pessoaId);
            if (entity == null) return null;

            entity.RazaoSocial = request.RazaoSocial;
            entity.NomeFantasia = request.NomeFantasia;
            entity.InscricaoEstadual = request.InscricaoEstadual;
            entity.InscricaoMunicipal = request.InscricaoMunicipal;
            entity.Responsavel = request.Responsavel;
            entity.Website = request.Website;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
            return _mapper.Map<PessoaPjResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long pessoaId)
        {
            if (!await _repository.ExistsAsync(pessoaId)) return false;
            await _repository.DeleteAsync(pessoaId);
            return true;
        }
    }
}
