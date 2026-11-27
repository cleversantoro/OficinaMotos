using System;

namespace OficinaMotos.Application.DTOs.Responses.Pessoas
{
    public class PessoaPfResponseDTO
    {
        public long PessoaId { get; set; }
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Profissao { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
