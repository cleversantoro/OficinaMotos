namespace OficinaMotos.Application.DTOs.Requests.Estoque
{
    public class CreateEstoquePecaHistoricoDTO
    {
        public long PecaId { get; set; }
        public string Campo { get; set; } = string.Empty;
        public string? ValorAnterior { get; set; }
        public string? ValorNovo { get; set; }
        public long? UsuarioId { get; set; }
    }
}
