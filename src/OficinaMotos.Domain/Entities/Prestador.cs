using OficinaMotos.Domain.Common;


namespace OficinaMotos.Domain.Entities
{
    public class AtividadeProfissional : BaseEntity
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
        public ICollection<PrestadorAtividade> Prestadores { get; set; } = new HashSet<PrestadorAtividade>();
    }

    public class Prestador : BaseEntity
    {
        public long PessoaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public PrestadorVinculo TipoVinculo { get; set; } = PrestadorVinculo.Interno;
        public PrestadorStatus Status { get; set; } = PrestadorStatus.Ativo;
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal? ValorHora { get; set; }
        public string? Observacoes { get; set; }

        public Pessoa? Pessoa { get; set; }
        public Mecanico? Mecanico { get; set; }
        public ICollection<PrestadorAtividade> Atividades { get; set; } = new HashSet<PrestadorAtividade>();
        public ICollection<PrestadorDisponibilidade> Disponibilidades { get; set; } = new HashSet<PrestadorDisponibilidade>();
        public ICollection<PrestadorExperiencia> Experiencias { get; set; } = new HashSet<PrestadorExperiencia>();
        public ICollection<PrestadorCertificacao> Certificacoes { get; set; } = new HashSet<PrestadorCertificacao>();
    }

    public class PrestadorAtividade
    {
        public long PrestadorId { get; set; }
        public long AtividadeProfissionalId { get; set; }
        public string? Nivel { get; set; }
        public bool Principal { get; set; }
        public bool Ativo { get; set; } = true;
        public long? PrestadorPrincipalId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Prestador? Prestador { get; set; }
        public AtividadeProfissional? AtividadeProfissional { get; set; }
    }

    public class PrestadorDisponibilidade : BaseEntity
    {
        public long PrestadorId { get; set; }
        public byte DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public bool Ativo { get; set; } = true;
        public Prestador? Prestador { get; set; }
    }

    public class PrestadorExperiencia : BaseEntity
    {
        public long PrestadorId { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Atual { get; set; }
        public Prestador? Prestador { get; set; }
    }

    public class PrestadorCertificacao : BaseEntity
    {
        public long PrestadorId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Instituicao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? CodigoCertificacao { get; set; }
        public string? ArquivoUrl { get; set; }
        public Prestador? Prestador { get; set; }
    }

    public enum PrestadorStatus
    {
        Inativo = 0,
        Ativo = 1,
        Suspenso = 2,
        Bloqueado = 3
    }

    public enum PrestadorVinculo
    {
        Interno = 1,
        Terceirizado = 2

    }
}
