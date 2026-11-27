using System;
using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServico
{
    public class OrdemServicoItemResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }
        public long? PecaId { get; set; }
        public long? AtividadeProfissionalId { get; set; }
        public long? PrestadorId { get; set; }
        public OrdemServicoEnum Status { get; set; }
        public OrdemServicoTipo Tipo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
