using System;

namespace OficinaMotos.Application.DTOs.Responses.Estoque
{
    public class EstoquePecaHistoricoResponseDTO
    {
        public long Id { get; set; }
        public long PecaId { get; set; }
        public string Campo { get; set; } = string.Empty;
        public string? ValorAnterior { get; set; }
        public string? ValorNovo { get; set; }
        public long? UsuarioId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
