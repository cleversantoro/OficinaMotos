using System;

namespace OficinaMotos.Application.DTOs.Responses.Estoque
{
    public class EstoquePecaResponseDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string? CodigoBarras { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public long? FabricanteId { get; set; }
        public long? CategoriaId { get; set; }
        public long? LocalizacaoId { get; set; }
        public string Unidade { get; set; } = "UN";
        public decimal QuantidadeEstoque { get; set; }
        public decimal EstoqueMinimo { get; set; }
        public decimal? EstoqueMaximo { get; set; }
        public decimal CustoMedio { get; set; }
        public decimal PrecoVenda { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
