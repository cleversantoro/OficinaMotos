using System;

namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class UpdateFinanceiroPagamentoDTO
    {
        public long? ContaPagarId { get; set; }
        public long? ContaReceberId { get; set; }
        public long? MetodoPagamentoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = "CONFIRMADO";
        public string? ReferenciaExterna { get; set; }
        public string? Observacao { get; set; }
    }
}
