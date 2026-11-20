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
    public class MecanicoEnderecoService : IMecanicoEnderecoService
    {
        private readonly IMecanicoEnderecoRepository _repository;
        private readonly IMapper _mapper;

        public MecanicoEnderecoService(IMecanicoEnderecoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MecanicoEnderecoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<MecanicoEnderecoResponseDTO>>(items);
        }

        public async Task<MecanicoEnderecoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<MecanicoEnderecoResponseDTO?>(item);
        }

        public async Task<MecanicoEnderecoResponseDTO> CreateAsync(CreateMecanicoEnderecoDTO request)
        {
            var entity = _mapper.Map<MecanicoEndereco>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MecanicoEnderecoResponseDTO>(created);
        }

        public async Task<MecanicoEnderecoResponseDTO?> UpdateAsync(long id, UpdateMecanicoEnderecoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.MecanicoId = request.MecanicoId;
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
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<MecanicoEnderecoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
