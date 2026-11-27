using System;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class UpdateOrdemServicoAvaliacaoDTO
    {
        public long OrdemServicoId { get; set; }
        public long ClienteId { get; set; }
        public byte Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;
    }
}
