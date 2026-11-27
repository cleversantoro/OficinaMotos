namespace OficinaMotos.Application.DTOs.Requests.Fornecedor
{
    public class UpdateFornecedorSegmentoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
