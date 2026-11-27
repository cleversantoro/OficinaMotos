namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class UpdateFinanceiroAnexoDTO
    {
        public long? ContaPagarId { get; set; }
        public long? ContaReceberId { get; set; }
        public long? PagamentoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
    }
}
