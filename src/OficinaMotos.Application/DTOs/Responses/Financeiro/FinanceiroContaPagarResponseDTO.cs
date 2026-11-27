using System;

namespace OficinaMotos.Application.DTOs.Responses.Financeiro
{
    public class FinanceiroContaPagarResponseDTO
    {
        public long Id { get; set; }
        public long? FornecedorId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? Documento { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = "ABERTA";
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
