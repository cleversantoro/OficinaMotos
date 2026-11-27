using System;

namespace OficinaMotos.Application.DTOs.Responses.Pessoas
{
    public class PessoaEnderecoResponseDTO
    {
        public long Id { get; set; }
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Cep { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
