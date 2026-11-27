using System;
using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServico
{
    public class OrdemServicoResponseDTO
    {
        public long Id { get; set; }
        public long Numero { get; set; }
        public long ClienteId { get; set; }
        public long? VeiculoId { get; set; }
        public string DescricaoProblema { get; set; } = string.Empty;
        public string? Diagnostico { get; set; }
        public OrdemServicoEnum Status { get; set; }
        public OrdemServicoPrioridade Prioridade { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? PrevisaoEntrega { get; set; }
        public DateTime? DataConclusao { get; set; }
        public decimal ValorPecas { get; set; }
        public decimal ValorServicos { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
