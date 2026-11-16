namespace OficinaMotos.Application.DTOs.Responses.Financeiro
{
    public class ContaPagarPagamentoResponseDTO
    {
        public long ContaPagarId { get; set; }
        public long PagamentoId { get; set; }
        public decimal ValorPago { get; set; }
        public decimal SaldoResidual { get; set; }
        public bool BaixaIntegral { get; set; }
        public FinanceiroContaPagarResponseDTO Conta { get; set; } = new();
    }
}
