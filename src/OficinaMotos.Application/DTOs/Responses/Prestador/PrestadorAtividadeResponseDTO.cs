using System;

namespace OficinaMotos.Application.DTOs.Responses.Prestador
{
    public class PrestadorAtividadeResponseDTO
    {
        public long PrestadorId { get; set; }
        public long AtividadeProfissionalId { get; set; }
        public string? AtividadeNome { get; set; }
        public string? Nivel { get; set; }
        public bool Principal { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
