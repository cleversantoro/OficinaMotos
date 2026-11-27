using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System.Collections.Generic;
using System.Threading.Tasks;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;

namespace OficinaMotos.Application.Services.OrdemServicoRepo
{
    public class OrdemServicoItemService : IOrdemServicoItemService
    {
        private readonly IOrdemServicoItemRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPrestadorRepository _prestadorRepository;
        private readonly IAtividadeProfissionalRepository _atividadeRepository;

        public OrdemServicoItemService(IOrdemServicoItemRepository repository, IMapper mapper,
            IPrestadorRepository prestadorRepository, IAtividadeProfissionalRepository atividadeRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _prestadorRepository = prestadorRepository;
            _atividadeRepository = atividadeRepository;
        }

        public async Task<List<OrdemServicoItemResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<OrdemServicoItemResponseDTO>>(items);
        }

        public async Task<OrdemServicoItemResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<OrdemServicoItemResponseDTO?>(item);
        }

        public async Task<OrdemServicoItemResponseDTO> CreateAsync(CreateOrdemServicoItemDTO request)
        {
            await ValidarExecucao(request.PrestadorId, request.AtividadeProfissionalId);
            var entity = _mapper.Map<OrdemServicoItem>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<OrdemServicoItemResponseDTO>(created);
        }

        public async Task<OrdemServicoItemResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoItemDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.OrdemServicoId = request.OrdemServicoId;
            entity.PecaId = request.PecaId;
            entity.AtividadeProfissionalId = request.AtividadeProfissionalId;
            entity.PrestadorId = request.PrestadorId;
            await ValidarExecucao(request.PrestadorId, request.AtividadeProfissionalId);
            entity.Descricao = request.Descricao;
            entity.Quantidade = request.Quantidade;
            entity.ValorUnitario = request.ValorUnitario;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<OrdemServicoItemResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        private async Task ValidarExecucao(long? prestadorId, long? atividadeId)
        {
            if (prestadorId.HasValue != atividadeId.HasValue)
                throw new DomainException("Prestador e atividade profissional devem ser informados juntos.");
            if (!prestadorId.HasValue) return;

            var prestador = await _prestadorRepository.GetByIdAsync(prestadorId.Value);
            if (prestador == null)
                throw new DomainException($"Prestador #{prestadorId.Value} não encontrado.");

            var atividade = await _atividadeRepository.GetByIdAsync(atividadeId!.Value);
            if (atividade == null)
                throw new DomainException($"Atividade profissional #{atividadeId.Value} não encontrada.");

            var habilitado = prestador.Atividades.Any(x =>
                x.AtividadeProfissionalId == atividadeId.Value && x.Ativo);
            if (!habilitado)
                throw new DomainException("O prestador não está habilitado para esta atividade profissional.");
        }
    }
}
