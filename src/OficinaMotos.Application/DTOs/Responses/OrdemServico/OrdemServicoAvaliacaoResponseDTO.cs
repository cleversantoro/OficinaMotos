using System;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServico
{
    public class OrdemServicoAvaliacaoResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }
        public long ClienteId { get; set; }
        public byte Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
