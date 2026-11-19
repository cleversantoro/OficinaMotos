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
        public string? Descricao { get; set; }

        public ICollection<FinanceiroPagamento> Pagamentos { get; set; } = new HashSet<FinanceiroPagamento>();
        public ICollection<FinanceiroContaPagar> ContasPagar { get; set; } = new HashSet<FinanceiroContaPagar>();
        public ICollection<FinanceiroContaReceber> ContasReceber { get; set; } = new HashSet<FinanceiroContaReceber>();
    }

    public class FinanceiroPagamento : BaseEntity
    {
        public long? ContaPagarId { get; set; }
        public FinanceiroContaPagar? ContaPagar { get; set; }
        public long? ContaReceberId { get; set; }
        public FinanceiroContaReceber? ContaReceber { get; set; }
        public long? OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public long? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public long? FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
        public long? MetodoId { get; set; }
        public FinanceiroMetodoPagamento? Metodo { get; set; }
        public string? Observacao { get; set; }

        public ICollection<FinanceiroAnexo> Anexos { get; set; } = new HashSet<FinanceiroAnexo>();
    }

    public class FinanceiroAnexo : BaseEntity
    {
        public long? PagamentoId { get; set; }
        public FinanceiroPagamento? Pagamento { get; set; }
        public long? ContaPagarId { get; set; }
        public FinanceiroContaPagar? ContaPagar { get; set; }
        public long? ContaReceberId { get; set; }
        public FinanceiroContaReceber? ContaReceber { get; set; }
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public string? Url { get; set; }
        public string? Observacao { get; set; }
        public DateTime? DataUpload { get; set; }
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
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
        public long? MetodoId { get; set; }
        public FinanceiroMetodoPagamento? Metodo { get; set; }
        public string? Observacao { get; set; }

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
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataRecebimento { get; set; }
        public long? MetodoId { get; set; }
        public FinanceiroMetodoPagamento? Metodo { get; set; }
        public string? Observacao { get; set; }

        public ICollection<FinanceiroAnexo> Anexos { get; set; } = new HashSet<FinanceiroAnexo>();
    }

    public class FinanceiroHistorico : BaseEntity
    {
        public string Entidade { get; set; } = string.Empty;
        public long EntidadeId { get; set; }
        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;
        public string? Usuario { get; set; }
        public string? Campo { get; set; }
        public string? ValorAntigo { get; set; }
        public string? ValorNovo { get; set; }
    }

    public class FinanceiroLancamento : BaseEntity
    {
        public string Tipo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataLancamento { get; set; } = DateTime.UtcNow;
        public string? Referencia { get; set; }
        public string? Observacao { get; set; }
    }
}
