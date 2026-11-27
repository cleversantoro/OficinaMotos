using System;

namespace OficinaMotos.Application.DTOs.Responses.Pessoas
{
    public class PessoaAnexoResponseDTO
    {
        public long Id { get; set; }
        public long PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public long? TamanhoBytes { get; set; }
        public string? MimeType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
