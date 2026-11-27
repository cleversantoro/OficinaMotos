using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.ClienteConfig
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("cad_clientes");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.PessoaId).HasColumnName("pessoa_id").IsRequired();
            builder.HasOne(c => c.Pessoa).WithMany().HasForeignKey(c => c.PessoaId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Codigo).HasColumnName("codigo").IsRequired();
            builder.Property(c => c.Status).HasColumnName("status").IsRequired();
            builder.Property(c => c.Vip).HasColumnName("vip").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(c => c.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(c => c.OrigemId).HasColumnName("origem_id");
            builder.Property(c => c.CreatedAt).HasColumnName("created_at");
            builder.Property(c => c.UpdatedAt).HasColumnName("updated_at");
            builder.Property(c => c.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(c => c.DeletedAt == null);

            builder.HasOne(c => c.Origem).WithMany(o => o.Clientes).HasForeignKey(c => c.OrigemId).OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Veiculos).WithOne(a => a.Cliente).HasForeignKey(a => a.ClienteId);
            builder.HasMany(c => c.OrdensServico).WithOne(a => a.Cliente).HasForeignKey(a => a.ClienteId);
        }
    }

    public class ClienteFinanceiroConfiguration : IEntityTypeConfiguration<ClienteFinanceiro>
    {
        public void Configure(EntityTypeBuilder<ClienteFinanceiro> builder)
        {
            builder.ToTable("cad_clientes_financeiro");

            builder.HasKey(f => f.Id);
            builder.Property(f => f.ClienteId).HasColumnName("cliente_id").IsRequired();
            builder.Property(f => f.LimiteCredito).HasColumnName("limite_credito").HasPrecision(18, 2);
            builder.Property(f => f.PrazoPagamento).HasColumnName("prazo_pagamento");
            builder.Property(f => f.Bloqueado).HasColumnName("bloqueado").HasColumnType("tinyint(1)");
            builder.Property(f => f.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(f => f.CreatedAt).HasColumnName("created_at");
            builder.Property(f => f.UpdatedAt).HasColumnName("updated_at");
            builder.Property(f => f.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(f => f.DeletedAt == null);

            builder.HasOne(f => f.Cliente)
                .WithOne(c => c.Financeiro)
                .HasForeignKey<ClienteFinanceiro>(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ClienteIndicacaoConfiguration : IEntityTypeConfiguration<ClienteIndicacao>
    {
        public void Configure(EntityTypeBuilder<ClienteIndicacao> builder)
        {
            builder.ToTable("cad_clientes_indicacoes");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.ClienteId).HasColumnName("cliente_id").IsRequired();
            builder.Property(i => i.IndicadorNome).HasColumnName("indicador_nome").HasMaxLength(160).IsRequired();
            builder.Property(i => i.IndicadorTelefone).HasColumnName("indicador_telefone").HasMaxLength(30);
            builder.Property(i => i.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(i => i.CreatedAt).HasColumnName("created_at");
            builder.Property(i => i.UpdatedAt).HasColumnName("updated_at");
            builder.Property(i => i.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(i => i.DeletedAt == null);

            builder.HasOne(i => i.Cliente)
                .WithMany(c => c.Indicacoes)
                .HasForeignKey(i => i.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ClienteLgpdConsentimentoConfiguration : IEntityTypeConfiguration<ClienteLgpdConsentimento>
    {
        public void Configure(EntityTypeBuilder<ClienteLgpdConsentimento> builder)
        {
            builder.ToTable("cad_clientes_lgpd_consentimentos");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.ClienteId).HasColumnName("cliente_id").IsRequired();
            builder.Property(c => c.Tipo).HasColumnName("tipo").IsRequired();
            builder.Property(c => c.Aceito).HasColumnName("aceito").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(c => c.DataConsentimento).HasColumnName("data_consentimento");
            builder.Property(c => c.ValidoAte).HasColumnName("valido_ate");
            builder.Property(c => c.Observacoes).HasColumnName("observacoes").HasMaxLength(240);
            builder.Property(c => c.Canal).HasColumnName("canal").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedAt).HasColumnName("created_at");
            builder.Property(c => c.UpdatedAt).HasColumnName("updated_at");
            builder.Property(c => c.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(c => c.DeletedAt == null);

            builder.HasOne(c => c.Cliente)
                .WithMany(cl => cl.ConsentimentosLgpd)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ClienteOrigemConfiguration : IEntityTypeConfiguration<ClienteOrigem>
    {
        public void Configure(EntityTypeBuilder<ClienteOrigem> builder)
        {
            builder.ToTable("cad_clientes_origens");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Nome).HasColumnName("nome").HasMaxLength(120).IsRequired();
            builder.Property(o => o.Descricao).HasColumnName("descricao").HasMaxLength(240);
            builder.Property(o => o.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").HasDefaultValue(true);
            builder.Property(o => o.CreatedAt).HasColumnName("created_at");
            builder.Property(o => o.UpdatedAt).HasColumnName("updated_at");
            builder.Property(o => o.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(o => o.DeletedAt == null);
        }
    }
}
