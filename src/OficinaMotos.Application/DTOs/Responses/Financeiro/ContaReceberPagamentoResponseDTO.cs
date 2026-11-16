namespace OficinaMotos.Application.DTOs.Responses.Financeiro
{
    public class ContaReceberPagamentoResponseDTO
    {
        public long ContaReceberId { get; set; }
        public long PagamentoId { get; set; }
        public decimal ValorPago { get; set; }
        public decimal SaldoResidual { get; set; }
        public bool BaixaIntegral { get; set; }
        public FinanceiroContaReceberResponseDTO Conta { get; set; } = new();
    }
}
