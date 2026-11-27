using System;

namespace OficinaMotos.Application.DTOs.Responses.Prestador
{
    public class PrestadorDisponibilidadeResponseDTO
    {
        public long Id { get; set; }
        public long PrestadorId { get; set; }
        public byte DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
