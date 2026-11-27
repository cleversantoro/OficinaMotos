namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class CreateOrdemServicoChecklistDTO
    {
        public long OrdemServicoId { get; set; }
        public long? ResponsavelPrestadorId { get; set; }
        public string Item { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Status { get; set; } = "PENDENTE";
        public string? Observacao { get; set; }
    }
}
