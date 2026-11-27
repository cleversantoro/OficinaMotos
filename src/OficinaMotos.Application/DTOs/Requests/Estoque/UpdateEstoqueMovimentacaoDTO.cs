namespace OficinaMotos.Application.DTOs.Requests.Estoque
{
    public class UpdateEstoqueMovimentacaoDTO
    {
        public long PecaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal? CustoUnitario { get; set; }
        public string? OrigemTipo { get; set; }
        public long? OrigemId { get; set; }
        public string? Observacao { get; set; }
        public long? UsuarioId { get; set; }
    }
}
