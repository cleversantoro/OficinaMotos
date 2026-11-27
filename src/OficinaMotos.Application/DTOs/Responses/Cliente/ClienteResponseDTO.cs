using System;
using System.Collections.Generic;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Application.DTOs.Responses.Cliente
{
    public class ClienteResponseDTO
    {
        public long Id { get; set; }
        public long Codigo { get; set; }

        public long? OrigemId { get; set; }
        public ClienteOrigemResponseDTO? Origem { get; set; }

        public long? PessoaId { get; set; }
        public PessoaResponseDTO? Pessoa { get; set; }

        public ClienteStatus Status { get; set; }
        public bool Vip { get; set; }
        
        
        public string? Observacoes { get; set; }

        public ClienteFinanceiroResponseDTO? Financeiro { get; set; }

        public ICollection<ClienteIndicacaoResponseDTO> Indicacoes { get; set; } = new HashSet<ClienteIndicacaoResponseDTO>();
        public ICollection<ClienteLgpdConsentimentoResponseDTO> ConsentimentosLgpd { get; set; } = new HashSet<ClienteLgpdConsentimentoResponseDTO>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ClienteResponseTableDTO
    {
        public long Id { get; set; }
        public long Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string Documento { get; set; } = string.Empty;
        public int Tipo { get; set; }
        public string? TipoDescricao { get; set; }
        public int Status { get; set; }
        public string? StatusDescricao { get; set; }
        public bool Vip { get; set; }
        public string? Observacoes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
