using System;

namespace OficinaMotos.Application.DTOs.Requests.Prestador
{
    public class CreatePrestadorExperienciaDTO
    {
        public long PrestadorId { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Atual { get; set; }
    }
}
