using System;

namespace OficinaMotos.Application.DTOs.Responses.Mecanico
{
    public class MecanicoResponseDTO
    {
        public long Id { get; set; }
        public long PrestadorId { get; set; }
        public string? Matricula { get; set; }
        public string? Nivel { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
