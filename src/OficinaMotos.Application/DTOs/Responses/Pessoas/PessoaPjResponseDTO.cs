using System;

namespace OficinaMotos.Application.DTOs.Responses.Pessoas
{
    public class PessoaPjResponseDTO
    {
        public long PessoaId { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? Responsavel { get; set; }
        public string? Website { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
