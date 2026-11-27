using System;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServico
{
    public class OrdemServicoHistoricoResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }
        public long? UsuarioId { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string? Campo { get; set; }
        public string? ValorAntigo { get; set; }
        public string? ValorNovo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
