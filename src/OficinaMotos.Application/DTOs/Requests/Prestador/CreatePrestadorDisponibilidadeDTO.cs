using System;

namespace OficinaMotos.Application.DTOs.Requests.Prestador
{
    public class CreatePrestadorDisponibilidadeDTO
    {
        public long PrestadorId { get; set; }
        public byte DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
