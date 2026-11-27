using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.DTOs.Responses.PrestadorRepo;
using OficinaMotos.Application.Interfaces.Prestador;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using PrestadorEntity = OficinaMotos.Domain.Entities.Prestador;

namespace OficinaMotos.Application.Services.Prestador
{
    public class PrestadorService : IPrestadorService
    {
        private readonly IPrestadorRepository _repository;
        private readonly IAtividadeProfissionalRepository _atividadeRepository;

        public PrestadorService(IPrestadorRepository repository, IAtividadeProfissionalRepository atividadeRepository)
        {
            _repository = repository;
            _atividadeRepository = atividadeRepository;
        }

        public async Task<List<PrestadorResponseDTO>> GetAllAsync()
        {
            var itens = await _repository.GetAllAsync();
            return itens.Select(Map).ToList();
        }

        public async Task<PrestadorResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item == null ? null : Map(item);
        }

        public async Task<PrestadorResponseDTO> CreateAsync(CreatePrestadorDTO request)
        {
            ValidarAtividades(request.Atividades);
            if (request.PessoaId <= 0) throw new DomainException("PessoaId é obrigatório para o prestador.");
            if (await _repository.GetByPessoaIdAsync(request.PessoaId) != null)
                throw new DomainException("Esta pessoa já possui vínculo de prestador.");
            if (await _repository.GetByCodigoAsync(request.Codigo) != null)
                throw new DomainException("Já existe prestador com este código.");

            await ValidarExistenciaAtividades(request.Atividades);

            var prestador = new PrestadorEntity
            {
                PessoaId = request.PessoaId,
                Codigo = request.Codigo.Trim(),
                TipoVinculo = request.TipoVinculo,
                Status = request.Status,
                DataInicio = request.DataInicio,
                DataFim = request.DataFim,
                ValorHora = request.ValorHora,
                Observacoes = request.Observacoes
            };

            prestador.Atividades = request.Atividades.Select(x => new PrestadorAtividade
            {
                AtividadeProfissionalId = x.AtividadeProfissionalId,
                Nivel = x.Nivel,
                Principal = x.Principal,
                Ativo = x.Ativo,
                PrestadorPrincipalId = x.Principal && x.Ativo ? prestador.Id : null
            }).ToHashSet();

            //if (request.MecanicamenteEspecializado)
            //{
            //    prestador.Mecanico = new MecanicoEntity
            //    {
            //        Prestador = prestador,
            //        Codigo = request.Matricula ?? request.Codigo,
            //        Matricula = request.Matricula,
            //        Nivel = request.NivelMecanico ?? "Pleno",
            //        DataAdmissao = request.DataInicio ?? DateTime.UtcNow
            //    };
            //}

            var criado = await _repository.AddAsync(prestador);
            return Map(criado);
        }

        public async Task<PrestadorResponseDTO?> UpdateAsync(long id, UpdatePrestadorDTO request)
        {
            var prestador = await _repository.GetByIdAsync(id);
            if (prestador == null) return null;
            ValidarAtividades(request.Atividades);
            await ValidarExistenciaAtividades(request.Atividades);

            prestador.PessoaId = request.PessoaId;
            prestador.Codigo = request.Codigo.Trim();
            prestador.TipoVinculo = request.TipoVinculo;
            prestador.Status = request.Status;
            prestador.DataInicio = request.DataInicio;
            prestador.DataFim = request.DataFim;
            prestador.ValorHora = request.ValorHora;
            prestador.Observacoes = request.Observacoes;
            prestador.SetUpdated();

            prestador.Atividades = request.Atividades.Select(x => new PrestadorAtividade
            {
                PrestadorId = prestador.Id,
                AtividadeProfissionalId = x.AtividadeProfissionalId,
                Nivel = x.Nivel,
                Principal = x.Principal,
                Ativo = x.Ativo,
                PrestadorPrincipalId = x.Principal && x.Ativo ? prestador.Id : null
            }).ToHashSet();

            await _repository.UpdateAsync(prestador);
            return Map(prestador);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        private static void ValidarAtividades(List<PrestadorAtividadeDTO> atividades)
        {
            var principaisAtivas = atividades.Count(x => x.Principal && x.Ativo);
            if (principaisAtivas > 1)
                throw new DomainException("O prestador pode possuir no máximo uma atividade principal ativa.");
        }

        private async Task ValidarExistenciaAtividades(List<PrestadorAtividadeDTO> atividades)
        {
            foreach (var atividade in atividades)
            {
                if (await _atividadeRepository.GetByIdAsync(atividade.AtividadeProfissionalId) == null)
                    throw new DomainException($"Atividade profissional #{atividade.AtividadeProfissionalId} não encontrada.");
            }
        }

        private static PrestadorResponseDTO Map(PrestadorEntity prestador)
        {
            return new PrestadorResponseDTO
            {
                Id = prestador.Id,
                PessoaId = prestador.PessoaId,
                Codigo = prestador.Codigo,
                TipoVinculo = prestador.TipoVinculo,
                Status = prestador.Status,
                DataInicio = prestador.DataInicio,
                DataFim = prestador.DataFim,
                ValorHora = prestador.ValorHora,
                Observacoes = prestador.Observacoes,
                Atividades = prestador.Atividades.Select(x => new PrestadorAtividadeResponseDTO
                {
                    PrestadorId = x.PrestadorId,
                    AtividadeProfissionalId = x.AtividadeProfissionalId,
                    AtividadeNome = x.AtividadeProfissional?.Nome,
                    Nivel = x.Nivel,
                    Principal = x.Principal,
                    Ativo = x.Ativo
                }).ToList()
            };
        }
    }
}
