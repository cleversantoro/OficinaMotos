namespace OficinaMotos.Application.DTOs.Requests.Pessoas
{
    public class UpdatePessoaContatoDTO
    {
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
    }
}
