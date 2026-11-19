using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class FornecedorSegmento : BaseEntity
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<Fornecedor> FornecedoresPrincipais { get; set; } = new HashSet<Fornecedor>();
        public ICollection<FornecedorSegmentoRel> FornecedorSegmentos { get; set; } = new HashSet<FornecedorSegmentoRel>();
    }

    public class Fornecedor : BaseEntity
    {
        public long PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public long? SegmentoPrincipalId { get; set; }
        public FornecedorSegmento? SegmentoPrincipal { get; set; }
        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? TelefonePrincipal { get; set; }
        public string Status { get; set; } = "ATIVO";
        public string? CondicaoPagamentoPadrao { get; set; }
        public int? PrazoEntregaMedio { get; set; }
        public decimal? NotaMedia { get; set; }
        public string? Observacoes { get; set; }
        public string? PrazoGarantiaPadrao { get; set; }
        public string? TermosNegociados { get; set; }
        public bool AtendimentoPersonalizado { get; set; }
        public bool RetiradaLocal { get; set; }
        public decimal? RatingLogistica { get; set; }
        public decimal? RatingQualidade { get; set; }

        public ICollection<FornecedorSegmentoRel> Segmentos { get; set; } = new HashSet<FornecedorSegmentoRel>();
        public ICollection<FornecedorEndereco> Enderecos { get; set; } = new HashSet<FornecedorEndereco>();
        public ICollection<FornecedorContato> Contatos { get; set; } = new HashSet<FornecedorContato>();
        public ICollection<FornecedorRepresentante> Representantes { get; set; } = new HashSet<FornecedorRepresentante>();
        public ICollection<FornecedorBanco> Bancos { get; set; } = new HashSet<FornecedorBanco>();
        public ICollection<FornecedorDocumento> Documentos { get; set; } = new HashSet<FornecedorDocumento>();
        public ICollection<FornecedorCertificacao> Certificacoes { get; set; } = new HashSet<FornecedorCertificacao>();
        public ICollection<FornecedorAvaliacao> Avaliacoes { get; set; } = new HashSet<FornecedorAvaliacao>();
        public ICollection<EstoquePecaFornecedor> PecasOfertadas { get; set; } = new HashSet<EstoquePecaFornecedor>();
        public ICollection<FinanceiroContaPagar> ContasAPagar { get; set; } = new HashSet<FinanceiroContaPagar>();
        public ICollection<FinanceiroPagamento> Pagamentos { get; set; } = new HashSet<FinanceiroPagamento>();
    }

    public class FornecedorSegmentoRel : BaseEntity
    {
        public long FornecedorId { get; set; }
        public long SegmentoId { get; set; }
        public bool Principal { get; set; }
        public string? Observacoes { get; set; }

        public Fornecedor? Fornecedor { get; set; }
        public FornecedorSegmento? Segmento { get; set; }
    }

    public class FornecedorEndereco : BaseEntity
    {
        public long FornecedorId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Cep { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Pais { get; set; }
        public string? Complemento { get; set; }
        public bool Principal { get; set; }
        public string? Observacao { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }

    public class FornecedorContato : BaseEntity
    {
        public long FornecedorId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }

    public class FornecedorRepresentante : BaseEntity
    {
        public long FornecedorId { get; set; }
        public long PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? PreferenciaContato { get; set; }
        public bool Principal { get; set; }
        public string? Observacoes { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }

    public class FornecedorBanco : BaseEntity
    {
        public long FornecedorId { get; set; }
        public string Banco { get; set; } = string.Empty;
        public string? Agencia { get; set; }
        public string? Conta { get; set; }
        public string? Digito { get; set; }
        public string? TipoConta { get; set; }
        public string? PixChave { get; set; }
        public string? Observacoes { get; set; }
        public bool Principal { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }

    public class FornecedorDocumento : BaseEntity
    {
        public long FornecedorId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? ArquivoUrl { get; set; }
        public string? Observacoes { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }

    public class FornecedorCertificacao : BaseEntity
    {
        public long FornecedorId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Instituicao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? CodigoCertificacao { get; set; }
        public string? Escopo { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }

    public class FornecedorAvaliacao : BaseEntity
    {
        public long FornecedorId { get; set; }
        public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;
        public string? AvaliadoPor { get; set; }
        public string? Categoria { get; set; }
        public decimal Nota { get; set; }
        public string? Comentarios { get; set; }

        public Fornecedor? Fornecedor { get; set; }
    }
}
