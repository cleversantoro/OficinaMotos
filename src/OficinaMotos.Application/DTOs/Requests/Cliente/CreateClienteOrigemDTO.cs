namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class CreateClienteOrigemDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
