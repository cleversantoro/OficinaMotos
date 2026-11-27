using System;

namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class CreateContaPagarPagamentoDTO
    {
        public long ContaPagarId { get; set; }
        public long? MetodoPagamentoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.UtcNow;
        public string Tipo { get; set; } = "SAIDA";
        public string Status { get; set; } = "CONFIRMADO";
        public string? ReferenciaExterna { get; set; }
        public string? Observacao { get; set; }
    }
}
