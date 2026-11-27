using System;

namespace OficinaMotos.Application.DTOs.Responses.Prestador
{
    public class AtividadeProfissionalResponseDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
