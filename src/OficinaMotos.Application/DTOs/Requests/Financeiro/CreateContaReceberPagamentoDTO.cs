using System;

namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class CreateContaReceberPagamentoDTO
    {
        public long ContaReceberId { get; set; }
        public long? MetodoPagamentoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.UtcNow;
        public string Tipo { get; set; } = "ENTRADA";
        public string Status { get; set; } = "CONFIRMADO";
        public string? ReferenciaExterna { get; set; }
        public string? Observacao { get; set; }
    }
}
