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

            builder.Ignore(c => c.IsDeleted);
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

    public class ClienteAnexoConfiguration : IEntityTypeConfiguration<ClienteAnexo>
    {
        public void Configure(EntityTypeBuilder<ClienteAnexo> builder)
        {
            builder.ToTable("cad_clientes_anexos");

            builder.HasKey(a => a.Id);
            //builder.Ignore(a => a.IsDeleted);
            builder.Property(a => a.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(a => a.Nome).HasColumnName("Nome").HasMaxLength(200).IsRequired();
            builder.Property(a => a.Tipo).HasColumnName("Tipo").HasMaxLength(100).IsRequired();
            builder.Property(a => a.Url).HasColumnName("Url").HasMaxLength(500).IsRequired();
            builder.Property(a => a.Observacao).HasColumnName("Observacao").HasMaxLength(240);
            builder.Property(a => a.CreatedAt).HasColumnName("Created_At");
            builder.Property(a => a.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(a => a.Cliente)
                .WithMany(c => c.Anexos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClienteContatoConfiguration : IEntityTypeConfiguration<ClienteContato>
    {
        public void Configure(EntityTypeBuilder<ClienteContato> builder)
        {
            builder.ToTable("cad_clientes_contatos");

            builder.HasKey(c => c.Id);
            //builder.Ignore(c => c.IsDeleted);
            builder.Property(c => c.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(c => c.Tipo).HasColumnName("Tipo").IsRequired();
            builder.Property(c => c.Valor).HasColumnName("Valor").HasMaxLength(160).IsRequired();
            builder.Property(c => c.Principal).HasColumnName("Principal").HasColumnType("tinyint(1)");
            builder.Property(c => c.Observacao).HasColumnName("Observacao").HasMaxLength(240);
            builder.Property(c => c.CreatedAt).HasColumnName("Created_At");
            builder.Property(c => c.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(c => c.Cliente)
                .WithMany(cl => cl.Contatos)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClienteDocumentoConfiguration : IEntityTypeConfiguration<ClienteDocumento>
    {
        public void Configure(EntityTypeBuilder<ClienteDocumento> builder)
        {
            builder.ToTable("cad_clientes_documentos");

            builder.HasKey(d => d.Id);
            //builder.Ignore(d => d.IsDeleted);
            builder.Property(d => d.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(d => d.Tipo).HasColumnName("Tipo").HasMaxLength(30).IsRequired();
            builder.Property(d => d.Documento).HasColumnName("Documento").HasMaxLength(40).IsRequired();
            builder.Property(d => d.DataEmissao).HasColumnName("Data_Emissao");
            builder.Property(d => d.DataValidade).HasColumnName("Data_Validade");
            builder.Property(d => d.OrgaoExpedidor).HasColumnName("Orgao_Expedidor").HasMaxLength(80);
            builder.Property(d => d.Principal).HasColumnName("Principal").HasColumnType("tinyint(1)");
            builder.Property(d => d.CreatedAt).HasColumnName("Created_At");
            builder.Property(d => d.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(d => d.Cliente)
                .WithMany(c => c.Documentos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClienteEnderecoConfiguration : IEntityTypeConfiguration<ClienteEndereco>
    {
        public void Configure(EntityTypeBuilder<ClienteEndereco> builder)
        {
            builder.ToTable("cad_clientes_enderecos");

            builder.HasKey(e => e.Id);
            //builder.Ignore(e => e.IsDeleted);
            builder.Property(e => e.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("Tipo").IsRequired();
            builder.Property(e => e.Cep).HasColumnName("Cep").HasMaxLength(12).IsRequired();
            builder.Property(e => e.Logradouro).HasColumnName("Logradouro").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Numero).HasColumnName("Numero").HasMaxLength(20).IsRequired();
            builder.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Estado).HasColumnName("Estado").HasMaxLength(60).IsRequired();
            builder.Property(e => e.Pais).HasColumnName("Pais").HasMaxLength(80);
            builder.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(120);
            builder.Property(e => e.Principal).HasColumnName("Principal").HasColumnType("tinyint(1)");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Enderecos)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClienteFinanceiroConfiguration : IEntityTypeConfiguration<ClienteFinanceiro>
    {
        public void Configure(EntityTypeBuilder<ClienteFinanceiro> builder)
        {
            builder.ToTable("cad_clientes_financeiro");

            builder.HasKey(f => f.Id);
            //builder.Ignore(f => f.IsDeleted);
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
            //builder.Ignore(i => i.IsDeleted);
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
            //builder.Ignore(c => c.IsDeleted);
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

            //builder.Ignore(o => o.IsDeleted);

            builder.Property(o => o.Nome).HasColumnName("Nome").HasMaxLength(120).IsRequired();
            builder.Property(o => o.Descricao).HasColumnName("Descricao").HasMaxLength(240);
            builder.Property(o => o.CreatedAt).HasColumnName("Created_At");
            builder.Property(o => o.UpdatedAt).HasColumnName("Updated_At");
        }
    }

    public class ClientePfConfiguration : IEntityTypeConfiguration<ClientePf>
    {
        public void Configure(EntityTypeBuilder<ClientePf> builder)
        {
            builder.ToTable("cad_clientes_pf");

            builder.HasKey(p => p.Id);
            //builder.Ignore(p => p.IsDeleted);
            builder.Property(p => p.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(p => p.Cpf).HasColumnName("Cpf").HasMaxLength(14).IsRequired();
            builder.Property(p => p.Rg).HasColumnName("Rg").HasMaxLength(20);
            builder.Property(p => p.DataNascimento).HasColumnName("Data_Nascimento");
            builder.Property(p => p.Genero).HasColumnName("Genero").HasMaxLength(30);
            builder.Property(p => p.EstadoCivil).HasColumnName("Estado_Civil").HasMaxLength(20);
            builder.Property(p => p.Profissao).HasColumnName("Profissao").HasMaxLength(120);
            builder.Property(p => p.CreatedAt).HasColumnName("Created_At");
            builder.Property(p => p.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(p => p.Cliente)
                .WithOne(c => c.PessoaFisica)
                .HasForeignKey<ClientePf>(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ClientePjConfiguration : IEntityTypeConfiguration<ClientePj>
    {
        public void Configure(EntityTypeBuilder<ClientePj> builder)
        {
            builder.ToTable("cad_clientes_pj");

            builder.HasKey(p => p.Id);
            //builder.Ignore(p => p.IsDeleted);
            builder.Property(p => p.ClienteId).HasColumnName("Cliente_Id").IsRequired();
            builder.Property(p => p.Cnpj).HasColumnName("Cnpj").HasMaxLength(20).IsRequired();
            builder.Property(p => p.RazaoSocial).HasColumnName("Razao_Social").HasMaxLength(180).IsRequired();
            builder.Property(p => p.NomeFantasia).HasColumnName("Nome_Fantasia").HasMaxLength(180);
            builder.Property(p => p.InscricaoEstadual).HasColumnName("Inscricao_Estadual").HasMaxLength(30);
            builder.Property(p => p.InscricaoMunicipal).HasColumnName("Inscricao_Municipal").HasMaxLength(30);
            builder.Property(p => p.Responsavel).HasColumnName("Responsavel").HasMaxLength(120);
            builder.Property(p => p.CreatedAt).HasColumnName("Created_At");
            builder.Property(p => p.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(p => p.Cliente)
                .WithOne(c => c.PessoaJuridica)
                .HasForeignKey<ClientePj>(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
