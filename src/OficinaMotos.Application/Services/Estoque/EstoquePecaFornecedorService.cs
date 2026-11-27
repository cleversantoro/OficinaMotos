using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Estoque;
using OficinaMotos.Application.DTOs.Responses.Estoque;
using OficinaMotos.Application.Interfaces.Estoque;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Estoque
{
    public class EstoquePecaFornecedorService : IEstoquePecaFornecedorService
    {
        private readonly IEstoquePecaFornecedorRepository _repository;
        private readonly IMapper _mapper;

        public EstoquePecaFornecedorService(IEstoquePecaFornecedorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EstoquePecaFornecedorResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<EstoquePecaFornecedorResponseDTO>>(items);
        }

        public async Task<EstoquePecaFornecedorResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<EstoquePecaFornecedorResponseDTO?>(item);
        }

        public async Task<EstoquePecaFornecedorResponseDTO> CreateAsync(CreateEstoquePecaFornecedorDTO request)
        {
            var entity = _mapper.Map<EstoquePecaFornecedor>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<EstoquePecaFornecedorResponseDTO>(created);
        }

        public async Task<EstoquePecaFornecedorResponseDTO?> UpdateAsync(long id, UpdateEstoquePecaFornecedorDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            entity.PecaId = request.PecaId;
            entity.FornecedorId = request.FornecedorId;
            entity.CodigoFornecedor = request.CodigoFornecedor;
            entity.CustoAtual = request.CustoAtual;
            entity.PrazoEntregaDias = request.PrazoEntregaDias;
            entity.Principal = request.Principal;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<EstoquePecaFornecedorResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                return false;
            }

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
