namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class UpdateClienteDTO
    {
        public long PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public int Tipo { get; set; }
        public int Status { get; set; }
        public bool Vip { get; set; }
        public string? Observacoes { get; set; }
        public int OrigemCadastroId { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public long? OrigemId { get; set; }
    }
}
