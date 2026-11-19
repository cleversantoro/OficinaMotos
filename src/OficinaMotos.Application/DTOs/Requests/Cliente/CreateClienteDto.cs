using System.ComponentModel.DataAnnotations; // Usaremos FluentValidation, mas DataAnnotations ajudam no Swagger

namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class CreateClienteDTO
    {
        public long PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NomeExibicao { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string? Documento { get; set; }
        public int Tipo { get; set; } = 1;
        public int Status { get; set; } = 1;
        public bool Vip { get; set; }
        public string? Observacoes { get; set; }
        public long? OrigemId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}
