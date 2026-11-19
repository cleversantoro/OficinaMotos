using OficinaMotos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class CreateOrdemServicoDTO
    {
        [Required]
        public long ClienteId { get; set; }
        [Obsolete("Use PrestadorId e AtividadeProfissionalId nos itens.")]
        public long MecanicoId { get; set; }
        public long? VeiculoId { get; set; }
        [Required]
        [StringLength(500)]
        public string DescricaoProblema { get; set; } = string.Empty;
        public OrdemServicoStatus Status { get; set; } = OrdemServicoStatus.Aberta;
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}
