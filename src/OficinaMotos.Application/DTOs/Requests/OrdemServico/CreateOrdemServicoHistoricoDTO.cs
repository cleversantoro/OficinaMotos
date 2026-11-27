using System;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class CreateOrdemServicoHistoricoDTO
    {
        public long OrdemServicoId { get; set; }
        public long? UsuarioId { get; set; }
        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;
        public string? Campo { get; set; }
        public string? ValorAntigo { get; set; }
        public string? ValorNovo { get; set; }
    }
}
