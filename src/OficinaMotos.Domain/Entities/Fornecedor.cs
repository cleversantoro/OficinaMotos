using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class FornecedorSegmento : BaseEntity
    {
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
        public long? SegmentoPrincipalId { get; set; }
        public FornecedorSegmento? SegmentoPrincipal { get; set; }
        public string Status { get; set; } = "ATIVO";
        public string? CondicaoPagamentoPadrao { get; set; }
        public int? PrazoEntregaMedio { get; set; }
        public decimal? NotaMedia { get; set; }
        public string? PrazoGarantiaPadrao { get; set; }
        public string? TermosNegociados { get; set; }
        public bool AtendimentoPersonalizado { get; set; }
        public bool RetiradaLocal { get; set; }
        public decimal? RatingLogistica { get; set; }
        public decimal? RatingQualidade { get; set; }
        public string? Observacoes { get; set; }
        
        

        public ICollection<FornecedorSegmentoRel> Segmentos { get; set; } = new HashSet<FornecedorSegmentoRel>();
        public ICollection<FornecedorRepresentante> Representantes { get; set; } = new HashSet<FornecedorRepresentante>();
        public ICollection<FornecedorBanco> Bancos { get; set; } = new HashSet<FornecedorBanco>();
        public ICollection<FornecedorCertificacao> Certificacoes { get; set; } = new HashSet<FornecedorCertificacao>();
        public ICollection<FornecedorAvaliacao> Avaliacoes { get; set; } = new HashSet<FornecedorAvaliacao>();
        public ICollection<EstoquePecaFornecedor> PecasOfertadas { get; set; } = new HashSet<EstoquePecaFornecedor>();
        public ICollection<FinanceiroContaPagar> ContasAPagar { get; set; } = new HashSet<FinanceiroContaPagar>();
    }

    public class FornecedorSegmentoRel
    {
        public long FornecedorId { get; set; }
        public long SegmentoId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Fornecedor? Fornecedor { get; set; }
        public FornecedorSegmento? Segmento { get; set; }
    }

    public class FornecedorRepresentante : BaseEntity
    {
        public long FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }

        public long PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }

        public string? Cargo { get; set; }
        public bool Principal { get; set; }
        public bool Preferencial { get; set; }
        public string? Observacoes { get; set; }
    }

    public class FornecedorBanco : BaseEntity
    {
        public long FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        
        public string Banco { get; set; } = string.Empty;
        public string? Agencia { get; set; }
        public string? Conta { get; set; }
        public string? Digito { get; set; }
        public string? TipoConta { get; set; }
        public string? PixChave { get; set; }
        public string? Observacoes { get; set; }
        public bool Principal { get; set; }

    }

    public class FornecedorCertificacao : BaseEntity
    {
        public long FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        
        public string Titulo { get; set; } = string.Empty;
        public string? Instituicao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? CodigoCertificacao { get; set; }
        public string? Escopo { get; set; }

    }

    public class FornecedorAvaliacao : BaseEntity
    {
        public long FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        
        public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;
        public string? AvaliadoPor { get; set; }
        public string? Categoria { get; set; }
        public decimal Nota { get; set; }
        public string? Comentarios { get; set; }

    }
}
