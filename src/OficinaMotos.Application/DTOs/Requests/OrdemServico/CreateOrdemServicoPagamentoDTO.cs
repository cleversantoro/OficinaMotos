using System;
using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class CreateOrdemServicoPagamentoDTO
    {
        public long OrdemServicoId { get; set; }
        public decimal Valor { get; set; }
        public OrdemServicoPagamentoStatus Status { get; set; } = OrdemServicoPagamentoStatus.Pendente;
        public DateTime? DataPagamento { get; set; }
        public string? Metodo { get; set; }
        public string? Observacao { get; set; }
    }
}
