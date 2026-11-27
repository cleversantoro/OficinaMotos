using System;

namespace OficinaMotos.Application.DTOs.Responses.Financeiro
{
    public class FinanceiroLancamentoResponseDTO
    {
        public long Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataCompetencia { get; set; }
        public DateTime DataLancamento { get; set; }
        public string? OrigemTipo { get; set; }
        public long? OrigemId { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
