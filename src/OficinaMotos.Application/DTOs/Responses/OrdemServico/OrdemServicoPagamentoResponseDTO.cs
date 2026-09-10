using System;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO
{
    public class OrdemServicoPagamentoResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
        public string? Metodo { get; set; }
        public string? Observacao { get; set; }
        public string? StatusOrdemServico { get; set; }
    }
}
