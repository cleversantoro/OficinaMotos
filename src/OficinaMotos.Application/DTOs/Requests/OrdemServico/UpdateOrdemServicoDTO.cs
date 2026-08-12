using OficinaMotos.Domain.Enums;
using System;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class UpdateOrdemServicoDTO
    {
        public long ClienteId { get; set; }
        public long MecanicoId { get; set; }
        public string DescricaoProblema { get; set; } = string.Empty;
        public OrdemServicoStatus Status { get; set; } = OrdemServicoStatus.Aberta;
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}
