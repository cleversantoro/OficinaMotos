namespace OficinaMotos.Application.DTOs.Requests.Estoque
{
    public class CreateEstoquePecaAnexoDTO
    {
        public long PecaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
    }
}
