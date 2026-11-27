namespace OficinaMotos.Application.DTOs.Requests.Estoque
{
    public class UpdateEstoqueLocalizacaoDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
