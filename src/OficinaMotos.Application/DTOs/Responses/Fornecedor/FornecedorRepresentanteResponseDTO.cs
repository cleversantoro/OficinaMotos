using System;

namespace OficinaMotos.Application.DTOs.Responses.Fornecedor
{
    public class FornecedorRepresentanteResponseDTO
    {
        public long Id { get; set; }
        public long FornecedorId { get; set; }
        public long PessoaId { get; set; }
        public string? Cargo { get; set; }
        public bool Principal { get; set; }
        public bool Preferencial { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
