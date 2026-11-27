using System;
using System.Collections.Generic;
using OficinaMotos.Application.DTOs.Responses.Pessoas;

namespace OficinaMotos.Application.DTOs.Responses.Fornecedor
{
    public class FornecedorResponseDTO
    {
        public long Id { get; set; }
        public long PessoaId { get; set; }
        public PessoaResponseDTO? Pessoa { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public long? SegmentoPrincipalId { get; set; }
        public FornecedorSegmentoResponseDTO? SegmentoPrincipal { get; set; }
        public string Status { get; set; } = "ATIVO";
        public string? CondicaoPagamentoPadrao { get; set; }
        public int? PrazoEntregaMedio { get; set; }
        public decimal? NotaMedia { get; set; }
        public string? PrazoGarantiaPadrao { get; set; }
        public string? TermosNegociados { get; set; }
        public bool AtendimentoPersonalizado { get; set; }
        public bool RetiradaLocal { get; set; }
        public decimal? RatingLogistica { get; set; }
        public decimal? RatingQualidade { get; set; }
        public string? Observacoes { get; set; }
        public ICollection<FornecedorSegmentoRelResponseDTO> Segmentos { get; set; } = new HashSet<FornecedorSegmentoRelResponseDTO>();
        public ICollection<FornecedorRepresentanteResponseDTO> Representantes { get; set; } = new HashSet<FornecedorRepresentanteResponseDTO>();
        public ICollection<FornecedorBancoResponseDTO> Bancos { get; set; } = new HashSet<FornecedorBancoResponseDTO>();
        public ICollection<FornecedorCertificacaoResponseDTO> Certificacoes { get; set; } = new HashSet<FornecedorCertificacaoResponseDTO>();
        public ICollection<FornecedorAvaliacaoResponseDTO> Avaliacoes { get; set; } = new HashSet<FornecedorAvaliacaoResponseDTO>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
