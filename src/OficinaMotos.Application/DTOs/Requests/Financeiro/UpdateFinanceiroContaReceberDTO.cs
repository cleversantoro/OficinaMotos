using System;

namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class UpdateFinanceiroContaReceberDTO
    {
        public long? ClienteId { get; set; }
        public long? OrdemServicoId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal ValorOriginal { get; set; }
        public decimal ValorRecebido { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Status { get; set; } = "ABERTA";
        public string? Observacoes { get; set; }
    }
}
