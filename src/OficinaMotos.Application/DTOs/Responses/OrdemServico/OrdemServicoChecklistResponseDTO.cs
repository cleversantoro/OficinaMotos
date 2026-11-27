using System;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServico
{
    public class OrdemServicoChecklistResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }
        public long? ResponsavelPrestadorId { get; set; }
        public string Item { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Status { get; set; } = "PENDENTE";
        public string? Observacao { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
