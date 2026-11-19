namespace OficinaMotos.Application.DTOs.Requests.Fornecedor
{
    public class UpdateFornecedorDTO
    {
        public long PessoaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public long? SegmentoPrincipalId { get; set; }
        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? TelefonePrincipal { get; set; }
        public string Status { get; set; } = "ATIVO";
        public string? CondicaoPagamentoPadrao { get; set; }
        public int? PrazoEntregaMedio { get; set; }
        public decimal? NotaMedia { get; set; }
        public string? Observacoes { get; set; }
        public string? PrazoGarantiaPadrao { get; set; }
        public string? TermosNegociados { get; set; }
        public bool AtendimentoPersonalizado { get; set; }
        public bool RetiradaLocal { get; set; }
        public decimal? RatingLogistica { get; set; }
        public decimal? RatingQualidade { get; set; }
    }
}
