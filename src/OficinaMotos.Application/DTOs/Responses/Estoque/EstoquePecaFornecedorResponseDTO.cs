using System;

namespace OficinaMotos.Application.DTOs.Responses.Estoque
{
    public class EstoquePecaFornecedorResponseDTO
    {
        public long Id { get; set; }
        public long PecaId { get; set; }
        public long FornecedorId { get; set; }
        public string? CodigoFornecedor { get; set; }
        public decimal? CustoAtual { get; set; }
        public int? PrazoEntregaDias { get; set; }
        public bool Principal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
