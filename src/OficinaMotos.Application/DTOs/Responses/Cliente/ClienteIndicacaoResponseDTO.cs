namespace OficinaMotos.Application.DTOs.Responses.Cliente
{
    public class ClienteIndicacaoResponseDTO
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public string IndicadorNome { get; set; } = string.Empty;
        public string? IndicadorTelefone { get; set; }
        public string? Observacao { get; set; }
    }
}
