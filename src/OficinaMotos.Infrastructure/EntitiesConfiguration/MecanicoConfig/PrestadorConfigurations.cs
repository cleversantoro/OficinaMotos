using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.MecanicoConfig
{
    public class PrestadorConfiguration : IEntityTypeConfiguration<Prestador>
    {
        public void Configure(EntityTypeBuilder<Prestador> builder)
        {
            builder.ToTable("cad_prestadores");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PessoaId).HasColumnName("pessoa_id").IsRequired();
            builder.Property(x => x.Codigo).HasColumnName("codigo").HasMaxLength(40).IsRequired();
            builder.Property(x => x.TipoVinculo).HasColumnName("tipo_vinculo").HasMaxLength(30).IsRequired();
            builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
            builder.Property(x => x.DataInicio).HasColumnName("data_inicio");
            builder.Property(x => x.DataFim).HasColumnName("data_fim");
            builder.Property(x => x.ValorHora).HasColumnName("valor_hora").HasPrecision(18, 2);
            builder.Property(x => x.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            builder.Ignore(x => x.IsDeleted);
            builder.HasQueryFilter(x => x.DeletedAt == null);
            builder.HasIndex(x => x.PessoaId).IsUnique();
            builder.HasIndex(x => x.Codigo).IsUnique();
            builder.HasOne(x => x.Pessoa).WithMany().HasForeignKey(x => x.PessoaId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Mecanico).WithOne(x => x.Prestador).HasForeignKey<Mecanico>(x => x.PrestadorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Atividades).WithOne(x => x.Prestador).HasForeignKey(x => x.PrestadorId).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class AtividadeProfissionalConfiguration : IEntityTypeConfiguration<AtividadeProfissional>
    {
        public void Configure(EntityTypeBuilder<AtividadeProfissional> builder)
        {
            builder.ToTable("cad_atividades_profissionais");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Codigo).HasColumnName("codigo").HasMaxLength(40).IsRequired();
            builder.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(120).IsRequired();
            builder.Property(x => x.Descricao).HasColumnName("descricao").HasMaxLength(300);
            builder.Property(x => x.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.HasIndex(x => x.Codigo).IsUnique();
            builder.HasIndex(x => x.Nome).IsUnique();
        }
    }

    public class PrestadorAtividadeConfiguration : IEntityTypeConfiguration<PrestadorAtividade>
    {
        public void Configure(EntityTypeBuilder<PrestadorAtividade> builder)
        {
            builder.ToTable("cad_prestadores_atividades");
            builder.HasKey(x => new { x.PrestadorId, x.AtividadeProfissionalId });
            builder.Property(x => x.PrestadorId).HasColumnName("prestador_id");
            builder.Property(x => x.AtividadeProfissionalId).HasColumnName("atividade_profissional_id");
            builder.Property(x => x.Nivel).HasColumnName("nivel").HasMaxLength(40);
            builder.Property(x => x.Principal).HasColumnName("principal").HasColumnType("tinyint(1)");
            builder.Property(x => x.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(x => x.PrestadorPrincipalId)
                .HasColumnName("prestador_principal_id")
                .HasComputedColumnSql("CASE WHEN principal = 1 THEN prestador_id ELSE NULL END", stored: true);
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.HasOne(x => x.Prestador).WithMany(x => x.Atividades).HasForeignKey(x => x.PrestadorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.AtividadeProfissional).WithMany(x => x.Prestadores).HasForeignKey(x => x.AtividadeProfissionalId).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PrestadorDisponibilidadeConfiguration : IEntityTypeConfiguration<PrestadorDisponibilidade>
    {
        public void Configure(EntityTypeBuilder<PrestadorDisponibilidade> builder)
        {
            builder.ToTable("cad_prestadores_disponibilidades");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PrestadorId).HasColumnName("prestador_id");
            builder.Property(x => x.DiaSemana).HasColumnName("dia_semana");
            builder.Property(x => x.HoraInicio).HasColumnName("hora_inicio");
            builder.Property(x => x.HoraFim).HasColumnName("hora_fim");
            builder.Property(x => x.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.HasOne(x => x.Prestador).WithMany(x => x.Disponibilidades).HasForeignKey(x => x.PrestadorId).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PrestadorExperienciaConfiguration : IEntityTypeConfiguration<PrestadorExperiencia>
    {
        public void Configure(EntityTypeBuilder<PrestadorExperiencia> builder)
        {
            builder.ToTable("cad_prestadores_experiencias");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PrestadorId).HasColumnName("prestador_id");
            builder.Property(x => x.Empresa).HasColumnName("empresa").HasMaxLength(160).IsRequired();
            builder.Property(x => x.Cargo).HasColumnName("cargo").HasMaxLength(120);
            builder.Property(x => x.Descricao).HasColumnName("descricao").HasMaxLength(500);
            builder.Property(x => x.DataInicio).HasColumnName("data_inicio");
            builder.Property(x => x.DataFim).HasColumnName("data_fim");
            builder.Property(x => x.Atual).HasColumnName("atual").HasColumnType("tinyint(1)");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.HasOne(x => x.Prestador).WithMany(x => x.Experiencias).HasForeignKey(x => x.PrestadorId).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PrestadorCertificacaoConfiguration : IEntityTypeConfiguration<PrestadorCertificacao>
    {
        public void Configure(EntityTypeBuilder<PrestadorCertificacao> builder)
        {
            builder.ToTable("cad_prestadores_certificacoes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PrestadorId).HasColumnName("prestador_id");
            builder.Property(x => x.Titulo).HasColumnName("titulo").HasMaxLength(160).IsRequired();
            builder.Property(x => x.Instituicao).HasColumnName("instituicao").HasMaxLength(160);
            builder.Property(x => x.DataEmissao).HasColumnName("data_emissao");
            builder.Property(x => x.DataValidade).HasColumnName("data_validade");
            builder.Property(x => x.CodigoCertificacao).HasColumnName("codigo_certificacao").HasMaxLength(60);
            builder.Property(x => x.ArquivoUrl).HasColumnName("arquivo_url").HasMaxLength(500);
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.HasOne(x => x.Prestador).WithMany(x => x.Certificacoes).HasForeignKey(x => x.PrestadorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
