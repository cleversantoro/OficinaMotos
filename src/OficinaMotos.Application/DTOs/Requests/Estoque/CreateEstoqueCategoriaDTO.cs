namespace OficinaMotos.Application.DTOs.Requests.Estoque
{
    public class CreateEstoqueCategoriaDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public long? CategoriaPaiId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
