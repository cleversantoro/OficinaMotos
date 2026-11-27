using System;
using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServico
{
    public class OrdemServicoAnexoResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public OrdemServicoAnexoTipo Tipo { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
