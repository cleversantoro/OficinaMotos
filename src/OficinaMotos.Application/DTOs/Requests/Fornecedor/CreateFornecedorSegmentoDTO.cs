namespace OficinaMotos.Application.DTOs.Requests.Fornecedor
{
    public class CreateFornecedorSegmentoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
