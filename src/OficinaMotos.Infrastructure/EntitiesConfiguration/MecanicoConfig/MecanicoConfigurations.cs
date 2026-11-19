using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.MecanicoConfig
{
    public class MecanicoEspecialidadeConfiguration : IEntityTypeConfiguration<MecanicoEspecialidade>
    {
        public void Configure(EntityTypeBuilder<MecanicoEspecialidade> builder)
        {
            builder.ToTable("cad_mecanicos_especialidades");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Codigo).HasMaxLength(40).IsRequired();
            builder.Property(e => e.Nome).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Descricao).HasMaxLength(300);
            builder.Property(e => e.Ativo).HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasMany(e => e.MecanicosPrincipais)
                   .WithOne(m => m.EspecialidadePrincipal)
                   .HasForeignKey(m => m.EspecialidadePrincipalId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.MecanicoEspecialidades)
                   .WithOne(rel => rel.Especialidade)
                   .HasForeignKey(rel => rel.EspecialidadeId);
        }
    }

    public class MecanicoConfiguration : IEntityTypeConfiguration<Mecanico>
    {
        public void Configure(EntityTypeBuilder<Mecanico> builder)
        {
            builder.ToTable("cad_mecanicos");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PrestadorId).HasColumnName("prestador_id");
            builder.Property(e => e.Matricula).HasColumnName("matricula").HasMaxLength(40);
            builder.HasOne(e => e.Prestador).WithOne(p => p.Mecanico).HasForeignKey<Mecanico>(e => e.PrestadorId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.Nivel).HasColumnName("nivel").HasMaxLength(40);
            builder.Property(e => e.DataAdmissao).HasColumnName("data_admissao");
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.Ignore(e => e.IsDeleted);
            builder.Ignore(e => e.Codigo);
            builder.Ignore(e => e.Nome);
            builder.Ignore(e => e.Sobrenome);
            builder.Ignore(e => e.NomeSocial);
            builder.Ignore(e => e.DocumentoPrincipal);
            builder.Ignore(e => e.TipoDocumento);
            builder.Ignore(e => e.DataNascimento);
            builder.Ignore(e => e.DataDemissao);
            builder.Ignore(e => e.Status);
            builder.Ignore(e => e.EspecialidadePrincipalId);
            builder.Ignore(e => e.EspecialidadePrincipal);
            builder.Ignore(e => e.ValorHora);
            builder.Ignore(e => e.CargaHorariaSemanal);
            builder.Ignore(e => e.Certificacoes);
            builder.Ignore(e => e.Contatos);
            builder.Ignore(e => e.Disponibilidades);
            builder.Ignore(e => e.Documentos);
            builder.Ignore(e => e.Enderecos);
            builder.Ignore(e => e.Especialidades);
            builder.Ignore(e => e.Experiencias);
            builder.Ignore(e => e.OrdensServico);

            builder.HasMany(e => e.Certificacoes)
                   .WithOne(c => c.Mecanico)
                   .HasForeignKey(c => c.MecanicoId);

            builder.HasMany(e => e.Contatos)
                   .WithOne(c => c.Mecanico)
                   .HasForeignKey(c => c.MecanicoId);

            builder.HasMany(e => e.Disponibilidades)
                   .WithOne(d => d.Mecanico)
                   .HasForeignKey(d => d.MecanicoId);

            builder.HasMany(e => e.Documentos)
                   .WithOne(d => d.Mecanico)
                   .HasForeignKey(d => d.MecanicoId);

            builder.HasMany(e => e.Enderecos)
                   .WithOne(ei => ei.Mecanico)
                   .HasForeignKey(ei => ei.MecanicoId);

            builder.HasMany(e => e.Especialidades)
                   .WithOne(rel => rel.Mecanico)
                   .HasForeignKey(rel => rel.MecanicoId);

            builder.HasMany(e => e.Experiencias)
                   .WithOne(exp => exp.Mecanico)
                   .HasForeignKey(exp => exp.MecanicoId);
        }
    }

    public class MecanicoCertificacaoConfiguration : IEntityTypeConfiguration<MecanicoCertificacao>
    {
        public void Configure(EntityTypeBuilder<MecanicoCertificacao> builder)
        {
            builder.ToTable("cad_mecanicos_certificacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MecanicoId).HasColumnName("Mecanico_Id");
            builder.Property(e => e.EspecialidadeId).HasColumnName("Especialidade_Id");
            builder.Property(e => e.Titulo).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Instituicao).HasMaxLength(160);
            builder.Property(e => e.CodigoCertificacao).HasColumnName("Codigo_Certificacao").HasMaxLength(120);
            builder.Property(e => e.DataConclusao).HasColumnName("Data_Conclusao");
            builder.Property(e => e.DataValidade).HasColumnName("Data_Validade");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.Certificacoes)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Especialidade)
                   .WithMany()
                   .HasForeignKey(e => e.EspecialidadeId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class MecanicoContatoConfiguration : IEntityTypeConfiguration<MecanicoContato>
    {
        public void Configure(EntityTypeBuilder<MecanicoContato> builder)
        {
            builder.ToTable("cad_mecanicos_contatos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MecanicoId).HasColumnName("Mecanico_Id");
            builder.Property(e => e.Tipo).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Valor).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Principal).HasDefaultValue(false);
            builder.Property(e => e.Observacao).HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.Contatos)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MecanicoDisponibilidadeConfiguration : IEntityTypeConfiguration<MecanicoDisponibilidade>
    {
        public void Configure(EntityTypeBuilder<MecanicoDisponibilidade> builder)
        {
            builder.ToTable("cad_mecanicos_disponibilidades");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MecanicoId).HasColumnName("Mecanico_Id");
            builder.Property(e => e.DiaSemana).HasColumnName("Dia_Semana").IsRequired();
            builder.Property(e => e.HoraInicio).HasColumnName("Hora_Inicio");
            builder.Property(e => e.HoraFim).HasColumnName("Hora_Fim");
            builder.Property(e => e.CapacidadeAtendimentos).HasColumnName("Capacidade_Atendimentos").HasDefaultValue(5);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.Disponibilidades)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MecanicoDocumentoConfiguration : IEntityTypeConfiguration<MecanicoDocumento>
    {
        public void Configure(EntityTypeBuilder<MecanicoDocumento> builder)
        {
            builder.ToTable("cad_mecanicos_documentos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MecanicoId).HasColumnName("Mecanico_Id");
            builder.Property(e => e.Tipo).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Numero).HasMaxLength(120).IsRequired();
            builder.Property(e => e.OrgaoExpedidor).HasColumnName("Orgao_Expedidor").HasMaxLength(120);
            builder.Property(e => e.ArquivoUrl).HasColumnName("Arquivo_Url").HasMaxLength(500);
            builder.Property(e => e.DataEmissao).HasColumnName("Data_Emissao");
            builder.Property(e => e.DataValidade).HasColumnName("Data_Validade");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.Documentos)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MecanicoEnderecoConfiguration : IEntityTypeConfiguration<MecanicoEndereco>
    {
        public void Configure(EntityTypeBuilder<MecanicoEndereco> builder)
        {
            builder.ToTable("cad_mecanicos_enderecos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MecanicoId).HasColumnName("Mecanico_Id");
            builder.Property(e => e.Tipo).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Cep).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Logradouro).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Numero).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Bairro).HasMaxLength(120).IsRequired();
            builder.Property(e => e.Cidade).HasMaxLength(120).IsRequired();
            builder.Property(e => e.Estado).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Pais).HasMaxLength(80);
            builder.Property(e => e.Complemento).HasMaxLength(120);
            builder.Property(e => e.Principal).HasDefaultValue(false);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.Enderecos)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MecanicoEspecialidadeRelConfiguration : IEntityTypeConfiguration<MecanicoEspecialidadeRel>
    {
        public void Configure(EntityTypeBuilder<MecanicoEspecialidadeRel> builder)
        {
            builder.ToTable("cad_mecanicos_especialidades_rel");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MecanicoId).HasColumnName("Mecanico_Id");
            builder.Property(e => e.EspecialidadeId).HasColumnName("Especialidade_Id");
            builder.Property(e => e.Nivel).HasMaxLength(50).HasDefaultValue("Pleno");
            builder.Property(e => e.Principal).HasDefaultValue(false);
            builder.Property(e => e.Anotacoes).HasMaxLength(300);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.Especialidades)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Especialidade)
                   .WithMany(e => e.MecanicoEspecialidades)
                   .HasForeignKey(e => e.EspecialidadeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MecanicoExperienciaConfiguration : IEntityTypeConfiguration<MecanicoExperiencia>
    {
        public void Configure(EntityTypeBuilder<MecanicoExperiencia> builder)
        {
            builder.ToTable("cad_mecanicos_experiencias");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MecanicoId).HasColumnName("Mecanico_Id");
            builder.Property(e => e.Empresa).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Cargo).HasMaxLength(160).IsRequired();
            builder.Property(e => e.ResumoAtividades).HasColumnName("Resumo_Atividades").HasMaxLength(500);
            builder.Property(e => e.DataInicio).HasColumnName("Data_Inicio");
            builder.Property(e => e.DataFim).HasColumnName("Data_Fim");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.Experiencias)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
