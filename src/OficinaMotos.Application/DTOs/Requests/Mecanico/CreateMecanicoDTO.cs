using System;

namespace OficinaMotos.Application.DTOs.Requests.Mecanico
{
    public class CreateMecanicoDTO
    {
        public long PrestadorId { get; set; }
        public string? Matricula { get; set; }
        public string? Nivel { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public string? Observacoes { get; set; }
    }
}
