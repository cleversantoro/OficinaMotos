using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Mecanico;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Prestador;
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
                Pessoa = MapPessoa(prestador.Pessoa),
                Codigo = prestador.Codigo,
                TipoVinculo = prestador.TipoVinculo,
                Status = prestador.Status,
                DataInicio = prestador.DataInicio,
                DataFim = prestador.DataFim,
                ValorHora = prestador.ValorHora,
                Observacoes = prestador.Observacoes,
                CreatedAt = prestador.CreatedAt,
                UpdatedAt = prestador.UpdatedAt,
                Mecanico = prestador.Mecanico == null ? null : new MecanicoResponseDTO
                {
                    Id = prestador.Mecanico.Id,
                    PrestadorId = prestador.Mecanico.PrestadorId,
                    Matricula = prestador.Mecanico.Matricula,
                    Nivel = prestador.Mecanico.Nivel,
                    DataAdmissao = prestador.Mecanico.DataAdmissao,
                    Observacoes = prestador.Mecanico.Observacoes,
                    CreatedAt = prestador.Mecanico.CreatedAt,
                    UpdatedAt = prestador.Mecanico.UpdatedAt
                },
                Atividades = prestador.Atividades.Select(x => new PrestadorAtividadeResponseDTO
                {
                    PrestadorId = x.PrestadorId,
                    AtividadeProfissionalId = x.AtividadeProfissionalId,
                    AtividadeNome = x.AtividadeProfissional?.Nome,
                    Nivel = x.Nivel,
                    Principal = x.Principal,
                    Ativo = x.Ativo
                }).ToList(),
                Disponibilidades = prestador.Disponibilidades.Select(x => new PrestadorDisponibilidadeResponseDTO
                {
                    Id = x.Id,
                    PrestadorId = x.PrestadorId,
                    DiaSemana = x.DiaSemana,
                    HoraInicio = x.HoraInicio,
                    HoraFim = x.HoraFim,
                    Ativo = x.Ativo,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList(),
                Experiencias = prestador.Experiencias.Select(x => new PrestadorExperienciaResponseDTO
                {
                    Id = x.Id,
                    PrestadorId = x.PrestadorId,
                    Empresa = x.Empresa,
                    Cargo = x.Cargo,
                    Descricao = x.Descricao,
                    DataInicio = x.DataInicio,
                    DataFim = x.DataFim,
                    Atual = x.Atual,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList(),
                Certificacoes = prestador.Certificacoes.Select(x => new PrestadorCertificacaoResponseDTO
                {
                    Id = x.Id,
                    PrestadorId = x.PrestadorId,
                    Titulo = x.Titulo,
                    Instituicao = x.Instituicao,
                    DataEmissao = x.DataEmissao,
                    DataValidade = x.DataValidade,
                    CodigoCertificacao = x.CodigoCertificacao,
                    ArquivoUrl = x.ArquivoUrl,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList()
            };
        }

        private static PessoaResponseDTO? MapPessoa(Pessoa? pessoa)
        {
            if (pessoa == null) return null;

            var tipo = pessoa.TipoPessoa?.Trim().ToUpperInvariant();

            return new PessoaResponseDTO
            {
                Id = pessoa.Id,
                Codigo = pessoa.Codigo,
                TipoPessoa = pessoa.TipoPessoa ?? string.Empty,
                Documento = pessoa.Documento,
                Nome = pessoa.Nome,
                NomeExibicao = pessoa.NomeExibicao,
                Ativo = pessoa.Ativo,
                Observacoes = pessoa.Observacoes,
                CreatedAt = pessoa.CreatedAt,
                UpdatedAt = pessoa.UpdatedAt,
                PessoaFisica = tipo == "PJ" || pessoa.PessoaFisica == null ? null : new PessoaPfResponseDTO
                {
                    PessoaId = pessoa.PessoaFisica.PessoaId,
                    Rg = pessoa.PessoaFisica.Rg,
                    DataNascimento = pessoa.PessoaFisica.DataNascimento,
                    Genero = pessoa.PessoaFisica.Genero,
                    EstadoCivil = pessoa.PessoaFisica.EstadoCivil,
                    Profissao = pessoa.PessoaFisica.Profissao,
                    CreatedAt = pessoa.PessoaFisica.CreatedAt,
                    UpdatedAt = pessoa.PessoaFisica.UpdatedAt
                },
                PessoaJuridica = tipo == "PF" || pessoa.PessoaJuridica == null ? null : new PessoaPjResponseDTO
                {
                    PessoaId = pessoa.PessoaJuridica.PessoaId,
                    RazaoSocial = pessoa.PessoaJuridica.RazaoSocial,
                    NomeFantasia = pessoa.PessoaJuridica.NomeFantasia,
                    InscricaoEstadual = pessoa.PessoaJuridica.InscricaoEstadual,
                    InscricaoMunicipal = pessoa.PessoaJuridica.InscricaoMunicipal,
                    Responsavel = pessoa.PessoaJuridica.Responsavel,
                    Website = pessoa.PessoaJuridica.Website,
                    CreatedAt = pessoa.PessoaJuridica.CreatedAt,
                    UpdatedAt = pessoa.PessoaJuridica.UpdatedAt
                },
                Contatos = pessoa.Contatos.Select(x => new PessoaContatoResponseDTO
                {
                    Id = x.Id,
                    PessoaId = x.PessoaId,
                    Tipo = x.Tipo,
                    Valor = x.Valor,
                    Principal = x.Principal,
                    Observacao = x.Observacao,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList(),
                Enderecos = pessoa.Enderecos.Select(x => new PessoaEnderecoResponseDTO
                {
                    Id = x.Id,
                    PessoaId = x.PessoaId,
                    Tipo = x.Tipo,
                    Cep = x.Cep,
                    Logradouro = x.Logradouro,
                    Numero = x.Numero,
                    Bairro = x.Bairro,
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Pais = x.Pais,
                    Complemento = x.Complemento,
                    Principal = x.Principal,
                    Observacao = x.Observacao,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList(),
                Documentos = pessoa.Documentos.Select(x => new PessoaDocumentoResponseDTO
                {
                    Id = x.Id,
                    PessoaId = x.PessoaId,
                    Tipo = x.Tipo,
                    Numero = x.Numero,
                    DataEmissao = x.DataEmissao,
                    DataValidade = x.DataValidade,
                    OrgaoExpedidor = x.OrgaoExpedidor,
                    ArquivoUrl = x.ArquivoUrl,
                    Observacoes = x.Observacoes,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList(),
                Anexos = pessoa.Anexos.Select(x => new PessoaAnexoResponseDTO
                {
                    Id = x.Id,
                    PessoaId = x.PessoaId,
                    Nome = x.Nome,
                    Tipo = x.Tipo,
                    Url = x.Url,
                    Observacao = x.Observacao,
                    TamanhoBytes = x.TamanhoBytes,
                    MimeType = x.MimeType,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToList()
            };
        }
    }
}
