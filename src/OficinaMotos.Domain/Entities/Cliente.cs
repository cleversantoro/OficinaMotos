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
        public string Nome { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public ClienteTipo Tipo { get; set; } = ClienteTipo.PessoaFisica;
        public ClienteStatus Status { get; set; } = ClienteStatus.Ativo;
        public bool Vip { get; set; }
        public string? Observacoes { get; set; }
        public int OrigemCadastroId { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public long? OrigemId { get; set; }
        public ClienteOrigem? Origem { get; set; }

        public ClientePf? PessoaFisica { get; set; }
        public ClientePj? PessoaJuridica { get; set; }
        public ClienteFinanceiro? Financeiro { get; set; }

        public ICollection<ClienteAnexo> Anexos { get; set; } = new HashSet<ClienteAnexo>();
        public ICollection<ClienteContato> Contatos { get; set; } = new HashSet<ClienteContato>();
        public ICollection<ClienteDocumento> Documentos { get; set; } = new HashSet<ClienteDocumento>();
        public ICollection<ClienteEndereco> Enderecos { get; set; } = new HashSet<ClienteEndereco>();
        public ICollection<ClienteIndicacao> Indicacoes { get; set; } = new HashSet<ClienteIndicacao>();
        public ICollection<ClienteLgpdConsentimento> ConsentimentosLgpd { get; set; } = new HashSet<ClienteLgpdConsentimento>();

        public ICollection<Veiculo> Veiculos { get; set; } = new HashSet<Veiculo>();
        public ICollection<OrdemServico> OrdensServico { get; set; } = new HashSet<OrdemServico>();
    }

    public class ClienteOrigem : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        public ICollection<Cliente> Clientes { get; set; } = new HashSet<Cliente>();
    }

    public class ClienteAnexo : BaseEntity
    {
        public long ClienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public class ClienteContato : BaseEntity
    {
        public long ClienteId { get; set; }
        public int Tipo { get; set; }
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public class ClienteDocumento : BaseEntity
    {
        public long ClienteId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public bool Principal { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public class ClienteEndereco : BaseEntity
    {
        public long ClienteId { get; set; }
        public int Tipo { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Pais { get; set; }
        public string? Complemento { get; set; }
        public bool Principal { get; set; }

        public Cliente? Cliente { get; set; }
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
        public DateTime? Data { get; set; }
        public DateTime? ValidoAte { get; set; }
        public string? Observacoes { get; set; }
        public string Canal { get; set; } = string.Empty;

        public Cliente? Cliente { get; set; }
    }

    public class ClientePf : BaseEntity
    {
        public long ClienteId { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Profissao { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public class ClientePj : BaseEntity
    {
        public long ClienteId { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? Responsavel { get; set; }

        public Cliente? Cliente { get; set; }
    }

    public enum ClienteTipo
    {
        PessoaFisica = 1,
        PessoaJuridica = 2
    }

    public enum ClienteStatus
    {
        Inativo = 0,
        Ativo = 1,
        Suspenso = 2,
        Bloqueado = 3
    }

    public enum ClienteOrigemEnum
    {
        Presencial = 1,
        Online = 2,
        Indicacao = 3,
        Outro = 4
    }

    public enum ClienteEnderecoTipo
    {
        Residencial = 1,
        Comercial = 2,
        Correspondencia = 3,
        Outro = 4
    }

    public enum ClienteContatoTipo
    {
        Telefone = 1,
        Celular = 2,
        Email = 3,
        Whatsapp = 4,
        Outro = 5
    }

    public enum ClienteConsentimentoTipo
    {
        Marketing = 1,
        CompartilhamentoDados = 2,
        Comunicacoes = 3,
        Outros = 99
    }

}
