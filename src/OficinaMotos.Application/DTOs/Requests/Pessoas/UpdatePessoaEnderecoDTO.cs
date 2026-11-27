namespace OficinaMotos.Application.DTOs.Requests.Pessoas
{
    public class UpdatePessoaEnderecoDTO
    {
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = "PRINCIPAL";
        public string? Cep { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Pais { get; set; } = "Brasil";
        public string? Complemento { get; set; }
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
    }
}
