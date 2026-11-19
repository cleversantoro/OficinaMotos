using System;
using System.Collections.Generic;
using OficinaMotos.Application.DTOs.Responses;

namespace OficinaMotos.Application.DTOs.Responses.Cliente
{
    public class ClienteResponseDTO
    {
        public long Id { get; set; }
        public long PessoaId { get; set; }
        public long Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string Documento { get; set; } = string.Empty;
        public int Tipo { get; set; }
        public int Status { get; set; }
        public bool Vip { get; set; }
        public string? Observacoes { get; set; }
        public int OrigemCadastroId { get; set; }
        public long? OrigemId { get; set; }
        public string? OrigemDescricao { get; set; }

        public ClienteOrigemResponseDTO? Origem { get; set; }
        public ClientePfResponseDTO? PessoaFisica { get; set; }
        public ClientePjResponseDTO? PessoaJuridica { get; set; }
        public ClienteFinanceiroResponseDTO? Financeiro { get; set; }

        public ICollection<ClienteAnexoResponseDTO> Anexos { get; set; } = new HashSet<ClienteAnexoResponseDTO>();
        public ICollection<ClienteContatoResponseDTO> Contatos { get; set; } = new HashSet<ClienteContatoResponseDTO>();
        public ICollection<ClienteDocumentoResponseDTO> Documentos { get; set; } = new HashSet<ClienteDocumentoResponseDTO>();
        public ICollection<ClienteEnderecoResponseDTO> Enderecos { get; set; } = new HashSet<ClienteEnderecoResponseDTO>();
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
        //public int OrigemCadastroId { get; set; }
        //public long? OrigemId { get; set; }
        //public string? OrigemDescricao { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
