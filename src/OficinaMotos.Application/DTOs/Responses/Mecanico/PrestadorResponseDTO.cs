namespace OficinaMotos.Application.DTOs.Responses.Mecanico
{
    public class PrestadorResponseDTO
    {
        public long Id { get; set; }
        public long PessoaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string TipoVinculo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public decimal? ValorHora { get; set; }
        public string? Observacoes { get; set; }
        public List<PrestadorAtividadeResponseDTO> Atividades { get; set; } = new();
    }

    public class PrestadorAtividadeResponseDTO
    {
        public long PrestadorId { get; set; }
        public long AtividadeProfissionalId { get; set; }
        public string? AtividadeNome { get; set; }
        public string? Nivel { get; set; }
        public bool Principal { get; set; }
        public bool Ativo { get; set; }
    }
}
