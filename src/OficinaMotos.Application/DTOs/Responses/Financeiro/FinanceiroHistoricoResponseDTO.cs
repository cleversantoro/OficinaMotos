using System;

namespace OficinaMotos.Application.DTOs.Responses.Financeiro
{
    public class FinanceiroHistoricoResponseDTO
    {
        public long Id { get; set; }
        public string Entidade { get; set; } = string.Empty;
        public long EntidadeId { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? DadosAnteriores { get; set; }
        public string? DadosNovos { get; set; }
        public long? UsuarioId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
