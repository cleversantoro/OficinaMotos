using OficinaMotos.Domain.Entities;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Application.DTOs.Responses.Prestador
{
    public class PrestadorResponseDTO
    {
        public long Id { get; set; }
        public long PessoaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public PrestadorVinculo TipoVinculo { get; set; }
        public PrestadorStatus Status { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal? ValorHora { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<PrestadorAtividadeResponseDTO> Atividades { get; set; } = new();
    }
}
