namespace OficinaMotos.Application.DTOs.Requests.Pessoas
{
    public class UpdatePessoaAnexoDTO
    {
        public long PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public long? TamanhoBytes { get; set; }
        public string? MimeType { get; set; }
    }
}
