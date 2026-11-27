namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class CreateClienteIndicacaoDTO
    {
        public long ClienteId { get; set; }
        public string IndicadorNome { get; set; } = string.Empty;
        public string? IndicadorTelefone { get; set; }
        public string? Observacao { get; set; }
    }
}
