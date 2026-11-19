namespace OficinaMotos.Application.DTOs.Responses.Fornecedor
{
    public class FornecedorRepresentanteResponseDTO
    {
        public long Id { get; set; }
        public long FornecedorId { get; set; }
        public long PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Cargo { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Celular { get; set; }
        public string? PreferenciaContato { get; set; }
        public bool Principal { get; set; }
        public string? Observacoes { get; set; }
    }
}
