using System;

namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class CreateClienteLgpdConsentimentoDTO
    {
        public long ClienteId { get; set; }
        public int Tipo { get; set; }
        public bool Aceito { get; set; }
        public DateTime? DataConsentimento { get; set; }
        public DateTime? ValidoAte { get; set; }
        public string? Observacoes { get; set; }
        public string Canal { get; set; } = string.Empty;
    }
}
