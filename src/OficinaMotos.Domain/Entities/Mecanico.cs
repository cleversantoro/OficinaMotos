using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class MecanicoEspecialidade : BaseEntity
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<Mecanico> MecanicosPrincipais { get; set; } = new HashSet<Mecanico>();
        public ICollection<MecanicoEspecialidadeRel> MecanicoEspecialidades { get; set; } = new HashSet<MecanicoEspecialidadeRel>();
    }

    public class Mecanico : BaseEntity
    {
        public long? PrestadorId { get; set; }
        public Prestador? Prestador { get; set; }
        public string? Matricula { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? Sobrenome { get; set; }
        public string? NomeSocial { get; set; }
        public string DocumentoPrincipal { get; set; } = string.Empty;
        public int TipoDocumento { get; set; } = 1;
        public DateTime? DataNascimento { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public string Status { get; set; } = "Ativo";
        public long? EspecialidadePrincipalId { get; set; }
        public MecanicoEspecialidade? EspecialidadePrincipal { get; set; }
        public string Nivel { get; set; } = "Pleno";
        public decimal ValorHora { get; set; }
        public int CargaHorariaSemanal { get; set; } = 44;
        public string? Observacoes { get; set; }

        public ICollection<MecanicoCertificacao> Certificacoes { get; set; } = new HashSet<MecanicoCertificacao>();
        public ICollection<MecanicoContato> Contatos { get; set; } = new HashSet<MecanicoContato>();
        public ICollection<MecanicoDisponibilidade> Disponibilidades { get; set; } = new HashSet<MecanicoDisponibilidade>();
        public ICollection<MecanicoDocumento> Documentos { get; set; } = new HashSet<MecanicoDocumento>();
        public ICollection<MecanicoEndereco> Enderecos { get; set; } = new HashSet<MecanicoEndereco>();
        public ICollection<MecanicoEspecialidadeRel> Especialidades { get; set; } = new HashSet<MecanicoEspecialidadeRel>();
        public ICollection<MecanicoExperiencia> Experiencias { get; set; } = new HashSet<MecanicoExperiencia>();
        public ICollection<OrdemServico> OrdensServico { get; set; } = new HashSet<OrdemServico>();
    }

    public class MecanicoCertificacao : BaseEntity
    {
        public long MecanicoId { get; set; }
        public long? EspecialidadeId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Instituicao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? CodigoCertificacao { get; set; }

        public Mecanico? Mecanico { get; set; }
        public MecanicoEspecialidade? Especialidade { get; set; }
    }

    public class MecanicoContato : BaseEntity
    {
        public long MecanicoId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }

        public Mecanico? Mecanico { get; set; }
    }

    public class MecanicoDisponibilidade : BaseEntity
    {
        public long MecanicoId { get; set; }
        public int DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public int CapacidadeAtendimentos { get; set; } = 5;

        public Mecanico? Mecanico { get; set; }
    }

    public class MecanicoDocumento : BaseEntity
    {
        public long MecanicoId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? ArquivoUrl { get; set; }

        public Mecanico? Mecanico { get; set; }
    }

    public class MecanicoEndereco : BaseEntity
    {
        public long MecanicoId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Pais { get; set; }
        public string? Complemento { get; set; }
        public bool Principal { get; set; }

        public Mecanico? Mecanico { get; set; }
    }

    public class MecanicoEspecialidadeRel : BaseEntity
    {
        public long MecanicoId { get; set; }
        public long EspecialidadeId { get; set; }
        public string Nivel { get; set; } = "Pleno";
        public bool Principal { get; set; }
        public string? Anotacoes { get; set; }

        public Mecanico? Mecanico { get; set; }
        public MecanicoEspecialidade? Especialidade { get; set; }
    }

    public class MecanicoExperiencia : BaseEntity
    {
        public long MecanicoId { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? ResumoAtividades { get; set; }

        public Mecanico? Mecanico { get; set; }
    }
}
