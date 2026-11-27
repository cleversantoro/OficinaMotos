namespace OficinaMotos.Application.DTOs.Requests.Estoque
{
    public class CreateEstoqueFabricanteDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Documento { get; set; }
        public string? Website { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
