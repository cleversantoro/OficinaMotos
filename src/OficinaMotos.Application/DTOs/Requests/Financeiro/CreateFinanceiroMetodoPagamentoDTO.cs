namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class CreateFinanceiroMetodoPagamentoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public decimal TaxaPercentual { get; set; }
        public decimal TaxaFixa { get; set; }
    }
}
