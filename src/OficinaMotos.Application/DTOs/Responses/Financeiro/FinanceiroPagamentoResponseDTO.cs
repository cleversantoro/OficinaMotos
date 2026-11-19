using System;

namespace OficinaMotos.Application.DTOs.Responses.Financeiro
{
    public class FinanceiroPagamentoResponseDTO
    {
        public long Id { get; set; }
        public long? ContaPagarId { get; set; }
        public long? ContaReceberId { get; set; }
        public long? OrdemServicoId { get; set; }
        public long? ClienteId { get; set; }
        public long? FornecedorId { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
        public long? MetodoId { get; set; }
        public string? Observacao { get; set; }
    }
}
