using OficinaMotos.Domain.Common;

namespace OficinaMotos.Domain.Entities
{
    public class Pessoa : BaseEntity
    {
        public long Codigo { get; set; }
        public string TipoPessoa { get; set; } = "PF";
        public string Documento { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public string? Observacoes { get; set; }

        public PessoaPf? PessoaFisica { get; set; }
        public PessoaPj? PessoaJuridica { get; set; }
        public ICollection<PessoaContato> Contatos { get; set; } = new HashSet<PessoaContato>();
        public ICollection<PessoaEndereco> Enderecos { get; set; } = new HashSet<PessoaEndereco>();
        public ICollection<PessoaDocumento> Documentos { get; set; } = new HashSet<PessoaDocumento>();
        public ICollection<PessoaAnexo> Anexos { get; set; } = new HashSet<PessoaAnexo>();
    }

    public class PessoaPf
    {
        public long PessoaId { get; set; }
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Profissao { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Pessoa? Pessoa { get; set; }
    }

    public class PessoaPj
    {
        public long PessoaId { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? Responsavel { get; set; }
        public string? Website { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Pessoa? Pessoa { get; set; }
    }

    public class PessoaContato : BaseEntity
    {
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }

        public Pessoa? Pessoa { get; set; }
    }

    public class PessoaEndereco : BaseEntity
    {
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = "PRINCIPAL";
        public string? Cep { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Pais { get; set; } = "Brasil";
        public string? Complemento { get; set; }
        public bool Principal { get; set; }
        public string? Observacao { get; set; }

        public Pessoa? Pessoa { get; set; }
    }

    public class PessoaDocumento : BaseEntity
    {
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? ArquivoUrl { get; set; }
        public string? Observacoes { get; set; }

        public Pessoa? Pessoa { get; set; }
    }

    public class PessoaAnexo : BaseEntity
    {
        public long PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public long? TamanhoBytes { get; set; }
        public string? MimeType { get; set; }

        public Pessoa? Pessoa { get; set; }
    }
}
