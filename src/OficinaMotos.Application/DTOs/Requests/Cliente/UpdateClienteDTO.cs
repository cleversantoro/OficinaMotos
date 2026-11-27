using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class UpdateClienteDTO
    {
        public long PessoaId { get; set; }
        public long Codigo { get; set; }
        public ClienteStatus Status { get; set; } = ClienteStatus.Ativo;
        public bool Vip { get; set; }
        public long? OrigemId { get; set; }
        public string? Observacoes { get; set; }
    }
}
