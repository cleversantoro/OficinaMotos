using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.OrdemServicoRepo
{
    public class OrdemServicoPagamentoService : IOrdemServicoPagamentoService
    {
        private readonly IOrdemServicoPagamentoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOrdemServicoService _ordemServicoService;

        public OrdemServicoPagamentoService(
            IOrdemServicoPagamentoRepository repository,
            IMapper mapper,
            IOrdemServicoService ordemServicoService)
        {
            _repository = repository;
            _mapper = mapper;
            _ordemServicoService = ordemServicoService;
        }

        public async Task<List<OrdemServicoPagamentoResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoPagamentoResponseDTO>>(items);
        }

        public async Task<OrdemServicoPagamentoResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoPagamentoResponseDTO?>(item);
        }

        public async Task<OrdemServicoPagamentoResponseDTO> CreateAsync(CreateOrdemServicoPagamentoDTO request)
        {
            return await _ordemServicoService.RegistrarPagamentoAsync(request);
        }

        public async Task<OrdemServicoPagamentoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoPagamentoDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            entity.Valor = request.Valor;
            entity.Status = request.Status;
            entity.DataPagamento = request.DataPagamento;
            entity.Metodo = request.Metodo;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoPagamentoResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
