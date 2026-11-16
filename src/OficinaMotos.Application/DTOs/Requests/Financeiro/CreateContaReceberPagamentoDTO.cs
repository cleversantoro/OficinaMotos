using System;

namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class CreateContaReceberPagamentoDTO
    {
        public long ContaReceberId { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public long MetodoId { get; set; }
        public string? Observacao { get; set; }
    }
}
