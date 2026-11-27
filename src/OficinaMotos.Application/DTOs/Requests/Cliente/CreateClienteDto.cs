using OficinaMotos.Application.DTOs.Responses.Cliente;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Application.DTOs.Requests.Cliente
{
    public class CreateClienteDTO
    {
        public long Codigo { get; set; }
        public long PessoaId { get; set; }
        public PessoaResponseDTO? Pessoa { get; set; }

        public long? OrigemId { get; set; }
        public ClienteOrigemResponseDTO? Origem { get; set; }

        public ClienteStatus Status { get; set; } = ClienteStatus.Ativo;
        public bool Vip { get; set; }
       
        public string? Observacoes { get; set; }
    }
}
