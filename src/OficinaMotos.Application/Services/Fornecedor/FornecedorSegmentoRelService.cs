using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;
using OficinaMotos.Application.DTOs.Responses.Fornecedor;
using OficinaMotos.Application.Interfaces.Fornecedor;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Fornecedor
{
    public class FornecedorSegmentoRelService : IFornecedorSegmentoRelService
    {
        private readonly IFornecedorSegmentoRelRepository _repository;
        private readonly IMapper _mapper;

        public FornecedorSegmentoRelService(IFornecedorSegmentoRelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FornecedorSegmentoRelResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FornecedorSegmentoRelResponseDTO>>(items);
        }

        public async Task<FornecedorSegmentoRelResponseDTO?> GetByKeysAsync(long fornecedorId, long segmentoId)
        {
            var item = await _repository.GetByKeysAsync(fornecedorId, segmentoId);
            return _mapper.Map<FornecedorSegmentoRelResponseDTO?>(item);
        }

        public async Task<FornecedorSegmentoRelResponseDTO> CreateAsync(CreateFornecedorSegmentoRelDTO request)
        {
            var entity = _mapper.Map<FornecedorSegmentoRel>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FornecedorSegmentoRelResponseDTO>(created);
        }

        public async Task<bool> DeleteAsync(long fornecedorId, long segmentoId)
        {
            if (!await _repository.ExistsAsync(fornecedorId, segmentoId)) return false;
            await _repository.DeleteAsync(fornecedorId, segmentoId);
            return true;
        }
    }
}
