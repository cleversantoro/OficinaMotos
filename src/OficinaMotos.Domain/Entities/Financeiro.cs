using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class FinanceiroMetodoPagamento : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public decimal TaxaPercentual { get; set; }
        public decimal TaxaFixa { get; set; }

        public ICollection<FinanceiroPagamento> Pagamentos { get; set; } = new HashSet<FinanceiroPagamento>();
    }

    public class FinanceiroContaPagar : BaseEntity
    {
        public long? FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Documento { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = "ABERTA";
        public string? Observacoes { get; set; }

        public ICollection<FinanceiroPagamento> Pagamentos { get; set; } = new HashSet<FinanceiroPagamento>();
        public ICollection<FinanceiroAnexo> Anexos { get; set; } = new HashSet<FinanceiroAnexo>();
    }

    public class FinanceiroContaReceber : BaseEntity
    {
        public long? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public long? OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorOriginal { get; set; }
        public decimal ValorRecebido { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = "ABERTA";
        public string? Observacoes { get; set; }

        public ICollection<FinanceiroPagamento> Pagamentos { get; set; } = new HashSet<FinanceiroPagamento>();
        public ICollection<FinanceiroAnexo> Anexos { get; set; } = new HashSet<FinanceiroAnexo>();
    }

    public class FinanceiroPagamento : BaseEntity
    {
        public long? ContaPagarId { get; set; }
        public FinanceiroContaPagar? ContaPagar { get; set; }
        public long? ContaReceberId { get; set; }
        public FinanceiroContaReceber? ContaReceber { get; set; }
        public long? MetodoPagamentoId { get; set; }
        public FinanceiroMetodoPagamento? MetodoPagamento { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = "CONFIRMADO";
        public string? ReferenciaExterna { get; set; }
        public string? Observacao { get; set; }

        public ICollection<FinanceiroAnexo> Anexos { get; set; } = new HashSet<FinanceiroAnexo>();
    }

    public class FinanceiroLancamento : BaseEntity
    {
        public string Tipo { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataCompetencia { get; set; }
        public DateTime DataLancamento { get; set; } = DateTime.UtcNow;
        public string? OrigemTipo { get; set; }
        public long? OrigemId { get; set; }
        public string? Observacoes { get; set; }
    }

    public class FinanceiroHistorico : BaseEntity
    {
        public string Entidade { get; set; } = string.Empty;
        public long EntidadeId { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? DadosAnteriores { get; set; }
        public string? DadosNovos { get; set; }
        public long? UsuarioId { get; set; }
        public SegUsuario? Usuario { get; set; }
    }

    public class FinanceiroAnexo : BaseEntity
    {
        public long? ContaPagarId { get; set; }
        public FinanceiroContaPagar? ContaPagar { get; set; }
        public long? ContaReceberId { get; set; }
        public FinanceiroContaReceber? ContaReceber { get; set; }
        public long? PagamentoId { get; set; }
        public FinanceiroPagamento? Pagamento { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
    }
}
