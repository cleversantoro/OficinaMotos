using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class EstoqueCategoria : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public long? CategoriaPaiId { get; set; }
        public bool Ativo { get; set; } = true;
        public EstoqueCategoria? CategoriaPai { get; set; }

        public ICollection<EstoquePeca> Pecas { get; set; } = new HashSet<EstoquePeca>();
    }

    public class EstoqueFabricante : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Documento { get; set; }
        public string? Website { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<EstoquePeca> Pecas { get; set; } = new HashSet<EstoquePeca>();
    }

    public class EstoqueLocalizacao : BaseEntity
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<EstoquePeca> Pecas { get; set; } = new HashSet<EstoquePeca>();
    }

    public class EstoquePeca : BaseEntity
    {
        public string Codigo { get; set; } = string.Empty;
        public string? CodigoBarras { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public long? FabricanteId { get; set; }
        public EstoqueFabricante? Fabricante { get; set; }
        public long? CategoriaId { get; set; }
        public EstoqueCategoria? Categoria { get; set; }
        public long? LocalizacaoId { get; set; }
        public EstoqueLocalizacao? Localizacao { get; set; }
        public string Unidade { get; set; } = "UN";
        public decimal QuantidadeEstoque { get; set; }
        public decimal EstoqueMinimo { get; set; }
        public decimal? EstoqueMaximo { get; set; }
        public decimal CustoMedio { get; set; }
        public decimal PrecoVenda { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<EstoquePecaAnexo> Anexos { get; set; } = new HashSet<EstoquePecaAnexo>();
        public ICollection<EstoquePecaFornecedor> Fornecedores { get; set; } = new HashSet<EstoquePecaFornecedor>();
        public ICollection<EstoqueMovimentacao> Movimentacoes { get; set; } = new HashSet<EstoqueMovimentacao>();
        public ICollection<EstoquePecaHistorico> Historico { get; set; } = new HashSet<EstoquePecaHistorico>();
        public ICollection<OrdemServicoItem> OrdemServicoItens { get; set; } = new HashSet<OrdemServicoItem>();
    }

    public class EstoquePecaFornecedor : BaseEntity
    {
        public long PecaId { get; set; }
        public EstoquePeca? Peca { get; set; }
        public long FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        public string? CodigoFornecedor { get; set; }
        public decimal? CustoAtual { get; set; }
        public int? PrazoEntregaDias { get; set; }
        public bool Principal { get; set; }
    }

    public class EstoquePecaAnexo : BaseEntity
    {
        public long PecaId { get; set; }
        public EstoquePeca? Peca { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
    }

    public class EstoqueMovimentacao : BaseEntity
    {
        public long PecaId { get; set; }
        public EstoquePeca? Peca { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal? CustoUnitario { get; set; }
        public string? OrigemTipo { get; set; }
        public long? OrigemId { get; set; }
        public string? Observacao { get; set; }
        public long? UsuarioId { get; set; }
        public SegUsuario? Usuario { get; set; }
               
    }

    public class EstoquePecaHistorico : BaseEntity
    {
        public long PecaId { get; set; }
        public EstoquePeca? Peca { get; set; }
        public string Campo { get; set; } = string.Empty;
        public string? ValorAnterior { get; set; }
        public string? ValorNovo { get; set; }
        public long? UsuarioId { get; set; }
        public SegUsuario? Usuario { get; set; }

    }
}
