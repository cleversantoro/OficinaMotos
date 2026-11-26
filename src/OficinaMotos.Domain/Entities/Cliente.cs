using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public long PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }
        public long Codigo { get; set; }
        public ClienteStatus Status { get; set; } = ClienteStatus.Ativo;
        public bool Vip { get; set; }
        public long? OrigemId { get; set; }
        public ClienteOrigem? Origem { get; set; }
        public string? Observacoes { get; set; }
        public ClienteFinanceiro? Financeiro { get; set; }

        public ICollection<ClienteIndicacao> Indicacoes { get; set; } = new HashSet<ClienteIndicacao>();
        public ICollection<ClienteLgpdConsentimento> ConsentimentosLgpd { get; set; } = new HashSet<ClienteLgpdConsentimento>();

        public ICollection<Veiculo> Veiculos { get; set; } = new HashSet<Veiculo>();
        public ICollection<OrdemServico> OrdensServico { get; set; } = new HashSet<OrdemServico>();
    }

    public class ClienteOrigem : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<Cliente> Clientes { get; set; } = new HashSet<Cliente>();
    }

    

    public class ClienteFinanceiro : BaseEntity
    {
        public long ClienteId { get; set; }
        public decimal? LimiteCredito { get; set; }
        public int? PrazoPagamento { get; set; }
        public bool Bloqueado { get; set; }
        public string? Observacoes { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public class ClienteIndicacao : BaseEntity
    {
        public long ClienteId { get; set; }
        public string IndicadorNome { get; set; } = string.Empty;
        public string? IndicadorTelefone { get; set; }
        public string? Observacao { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public class ClienteLgpdConsentimento : BaseEntity
    {
        public long ClienteId { get; set; }
        public int Tipo { get; set; }
        public bool Aceito { get; set; }
        public DateTime? DataConsentimento { get; set; }
        public DateTime? ValidoAte { get; set; }
        public string? Observacoes { get; set; }
        public string Canal { get; set; } = string.Empty;

        public Cliente? Cliente { get; set; }
    }

    

    public enum ClienteStatus
    {
        Inativo = 0,
        Ativo = 1,
        Suspenso = 2,
        Bloqueado = 3
    }

    

}
