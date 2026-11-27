namespace OficinaMotos.Application.DTOs.Requests.Fornecedor
{
    public class UpdateFornecedorRepresentanteDTO
    {
        public long FornecedorId { get; set; }
        public long PessoaId { get; set; }
        public string? Cargo { get; set; }
        public bool Principal { get; set; }
        public bool Preferencial { get; set; }
        public string? Observacoes { get; set; }
    }
}
