using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using System;

namespace OficinaMotos.Application.DTOs.Responses.OrdemServico
{
    public class OrdemServicoObservacaoResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }

        public long? UsuarioId { get; set; }
        public SegUsuarioResponseDTO? Usuario { get; set; }

        public string Texto { get; set; } = string.Empty;
        public OrdemServicoObservacaoTipo Tipo { get; set; } = OrdemServicoObservacaoTipo.Interna;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
