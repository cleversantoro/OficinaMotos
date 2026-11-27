namespace OficinaMotos.Application.DTOs.Requests.Estoque
{
    public class CreateEstoquePecaFornecedorDTO
    {
        public long PecaId { get; set; }
        public long FornecedorId { get; set; }
        public string? CodigoFornecedor { get; set; }
        public decimal? CustoAtual { get; set; }
        public int? PrazoEntregaDias { get; set; }
        public bool Principal { get; set; }
    }
}
