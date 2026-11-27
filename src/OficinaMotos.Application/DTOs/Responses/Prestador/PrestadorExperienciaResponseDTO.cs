using System;

namespace OficinaMotos.Application.DTOs.Responses.Prestador
{
    public class PrestadorExperienciaResponseDTO
    {
        public long Id { get; set; }
        public long PrestadorId { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Atual { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
