using OficinaMotos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO
{
    public class OrdemServicoResponseDTO
    {
        public long Id { get; set; }
        public long ClienteId { get; set; }
        public long MecanicoId { get; set; }
        public long VeiculoId { get; set; }
        public string DescricaoProblema { get; set; } = string.Empty;
        public OrdemServicoStatus Status { get; set; } = OrdemServicoStatus.Aberta;
        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }

        public ICollection<OrdemServicoAnexoResponseDTO> Anexos { get; set; } = new List<OrdemServicoAnexoResponseDTO>();
        public ICollection<OrdemServicoAvaliacaoResponseDTO> Avaliacoes { get; set; } = new List<OrdemServicoAvaliacaoResponseDTO>();
        public ICollection<OrdemServicoChecklistResponseDTO> Checklists { get; set; } = new List<OrdemServicoChecklistResponseDTO>();
        public ICollection<OrdemServicoItemResponseDTO> Itens { get; set; } = new List<OrdemServicoItemResponseDTO>();
        public ICollection<OrdemServicoObservacaoResponseDTO> Observacoes { get; set; } = new List<OrdemServicoObservacaoResponseDTO>();
        public ICollection<OrdemServicoHistoricoResponseDTO> Historico { get; set; } = new List<OrdemServicoHistoricoResponseDTO>();
        public ICollection<OrdemServicoPagamentoResponseDTO> Pagamentos { get; set; } = new List<OrdemServicoPagamentoResponseDTO>();
    }
}
