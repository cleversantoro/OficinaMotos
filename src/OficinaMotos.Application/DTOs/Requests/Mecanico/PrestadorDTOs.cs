using System;
using System.Collections.Generic;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Application.DTOs.Requests.Mecanico
{
    public class CreatePrestadorDTO
    {
        public long PessoaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public PrestadorVinculo TipoVinculo { get; set; } = PrestadorVinculo.Interno;
        public PrestadorStatus Status { get; set; } = PrestadorStatus.Ativo;
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal? ValorHora { get; set; }
        public string? Observacoes { get; set; }
        public bool MecanicamenteEspecializado { get; set; }
        public string? Matricula { get; set; }
        public string? NivelMecanico { get; set; }
        public List<PrestadorAtividadeDTO> Atividades { get; set; } = new();
    }

    public class UpdatePrestadorDTO : CreatePrestadorDTO { }

    public class PrestadorAtividadeDTO
    {
        public long AtividadeProfissionalId { get; set; }
        public string? Nivel { get; set; }
        public bool Principal { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
