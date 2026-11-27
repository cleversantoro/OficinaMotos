namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class UpdateClienteFinanceiroDTO
    {
        public long ClienteId { get; set; }
        public decimal? LimiteCredito { get; set; }
        public int? PrazoPagamento { get; set; }
        public bool Bloqueado { get; set; }
        public string? Observacoes { get; set; }
    }
}
