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
            builder.Property(c => c.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
            builder.Property(c => c.Vip).HasColumnName("vip").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(c => c.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(c => c.OrigemId).HasColumnName("origem_id");
            builder.Property(c => c.CreatedAt).HasColumnName("created_at");
            builder.Property(c => c.UpdatedAt).HasColumnName("updated_at");
            builder.Property(c => c.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(c => c.DeletedAt == null);

            builder.Ignore(c => c.Nome);
            builder.Ignore(c => c.NomeExibicao);
            builder.Ignore(c => c.Documento);
            builder.Ignore(c => c.Tipo);
            builder.Ignore(c => c.OrigemCadastroId);
            builder.Ignore(c => c.Telefone);
            builder.Ignore(c => c.Email);
            builder.Ignore(c => c.PessoaFisica);
            builder.Ignore(c => c.PessoaJuridica);
            builder.Ignore(c => c.Financeiro);
            builder.Ignore(c => c.Anexos);
            builder.Ignore(c => c.Contatos);
            builder.Ignore(c => c.Documentos);
            builder.Ignore(c => c.Enderecos);
            builder.Ignore(c => c.Indicacoes);
            builder.Ignore(c => c.ConsentimentosLgpd);

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
            builder.Property(f => f.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(f => f.LimiteCredito).HasColumnName("Limite_Credito");
            builder.Property(f => f.PrazoPagamento).HasColumnName("Prazo_Pagamento");
            builder.Property(f => f.Bloqueado).HasColumnName("Bloqueado").HasColumnType("tinyint(1)");
            builder.Property(f => f.Observacoes).HasColumnName("Observacoes").HasMaxLength(500);
            builder.Property(f => f.CreatedAt).HasColumnName("Created_At");
            builder.Property(f => f.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(f => f.Cliente)
                .WithOne(c => c.Financeiro)
                .HasForeignKey<ClienteFinanceiro>(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClienteIndicacaoConfiguration : IEntityTypeConfiguration<ClienteIndicacao>
    {
        public void Configure(EntityTypeBuilder<ClienteIndicacao> builder)
        {
            builder.ToTable("cad_clientes_indicacoes");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(i => i.IndicadorNome).HasColumnName("Indicador_Nome").HasMaxLength(160).IsRequired();
            builder.Property(i => i.IndicadorTelefone).HasColumnName("Indicador_Telefone").HasMaxLength(20);
            builder.Property(i => i.Observacao).HasColumnName("Observacao").HasMaxLength(240);
            builder.Property(i => i.CreatedAt).HasColumnName("Created_At");
            builder.Property(i => i.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(i => i.Cliente)
                .WithMany(c => c.Indicacoes)
                .HasForeignKey(i => i.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClienteLgpdConsentimentoConfiguration : IEntityTypeConfiguration<ClienteLgpdConsentimento>
    {
        public void Configure(EntityTypeBuilder<ClienteLgpdConsentimento> builder)
        {
            builder.ToTable("cad_clientes_lgpd_consentimentos");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(c => c.Tipo).HasColumnName("Tipo").IsRequired();
            builder.Property(c => c.Aceito).HasColumnName("Aceito").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(c => c.Data).HasColumnName("Data");
            builder.Property(c => c.ValidoAte).HasColumnName("Valido_Ate");
            builder.Property(c => c.Observacoes).HasColumnName("Observacoes").HasMaxLength(240);
            builder.Property(c => c.Canal).HasColumnName("Canal").HasMaxLength(80).IsRequired();
            builder.Property(c => c.CreatedAt).HasColumnName("Created_At");
            builder.Property(c => c.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(c => c.Cliente)
                .WithMany(cl => cl.ConsentimentosLgpd)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClienteOrigemConfiguration : IEntityTypeConfiguration<ClienteOrigem>
    {
        public void Configure(EntityTypeBuilder<ClienteOrigem> builder)
        {
            builder.ToTable("cad_clientes_origens");

            builder.HasKey(o => o.Id);


            builder.Property(o => o.Nome).HasColumnName("Nome").HasMaxLength(120).IsRequired();
            builder.Property(o => o.Descricao).HasColumnName("Descricao").HasMaxLength(240);
            builder.Property(o => o.CreatedAt).HasColumnName("Created_At");
            builder.Property(o => o.UpdatedAt).HasColumnName("Updated_At");
        }
    }



}
