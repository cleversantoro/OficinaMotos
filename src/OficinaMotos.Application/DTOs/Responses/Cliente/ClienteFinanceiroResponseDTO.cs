namespace OficinaMotos.Application.DTOs.Responses.Cliente
{
    public class ClienteFinanceiroResponseDTO
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public decimal? LimiteCredito { get; set; }
        public int? PrazoPagamento { get; set; }
        public bool Bloqueado { get; set; }
        public string? Observacoes { get; set; }
    }
}
