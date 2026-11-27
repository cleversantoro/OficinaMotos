using System;

namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class CreateFinanceiroLancamentoDTO
    {
        public string Tipo { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataCompetencia { get; set; }
        public DateTime DataLancamento { get; set; } = DateTime.UtcNow;
        public string? OrigemTipo { get; set; }
        public long? OrigemId { get; set; }
        public string? Observacoes { get; set; }
    }
}
