using System;

namespace OficinaMotos.Application.DTOs.Responses.Pessoas
{
    public class PessoaContatoResponseDTO
    {
        public long Id { get; set; }
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
