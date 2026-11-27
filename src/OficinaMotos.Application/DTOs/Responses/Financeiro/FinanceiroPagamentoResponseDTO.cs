using System;

namespace OficinaMotos.Application.DTOs.Responses.Financeiro
{
    public class FinanceiroPagamentoResponseDTO
    {
        public long Id { get; set; }
        public long? ContaPagarId { get; set; }
        public long? ContaReceberId { get; set; }
        public long? MetodoPagamentoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = "CONFIRMADO";
        public string? ReferenciaExterna { get; set; }
        public string? Observacao { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
