using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("cad_pessoas");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Codigo).HasColumnName("codigo").IsRequired();
            builder.Property(x => x.TipoPessoa).HasColumnName("tipo_pessoa").HasColumnType("enum('PF','PJ')").IsRequired();
            builder.Property(x => x.Documento).HasColumnName("documento").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(180).IsRequired();
            builder.Property(x => x.NomeExibicao).HasColumnName("nome_exibicao").HasMaxLength(180).IsRequired();
            builder.Property(x => x.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(x => x.Observacoes).HasColumnName("observacoes").HasMaxLength(600);
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            builder.Ignore(x => x.IsDeleted);
            builder.HasIndex(x => x.Codigo).IsUnique();
            builder.HasIndex(x => x.Documento).IsUnique();
            builder.HasQueryFilter(x => x.DeletedAt == null);

            builder.HasOne(x => x.PessoaFisica).WithOne(x => x.Pessoa).HasForeignKey<PessoaPf>(x => x.PessoaId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.PessoaJuridica).WithOne(x => x.Pessoa).HasForeignKey<PessoaPj>(x => x.PessoaId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Contatos).WithOne(x => x.Pessoa).HasForeignKey(x => x.PessoaId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Enderecos).WithOne(x => x.Pessoa).HasForeignKey(x => x.PessoaId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Documentos).WithOne(x => x.Pessoa).HasForeignKey(x => x.PessoaId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Anexos).WithOne(x => x.Pessoa).HasForeignKey(x => x.PessoaId).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PessoaPfConfiguration : IEntityTypeConfiguration<PessoaPf>
    {
        public void Configure(EntityTypeBuilder<PessoaPf> builder)
        {
            builder.ToTable("cad_pessoas_pf");
            builder.HasKey(x => x.PessoaId);
            builder.Property(x => x.PessoaId).HasColumnName("pessoa_id");
            builder.Property(x => x.Rg).HasColumnName("rg").HasMaxLength(30);
            builder.Property(x => x.DataNascimento).HasColumnName("data_nascimento");
            builder.Property(x => x.Genero).HasColumnName("genero").HasMaxLength(30);
            builder.Property(x => x.EstadoCivil).HasColumnName("estado_civil").HasMaxLength(30);
            builder.Property(x => x.Profissao).HasColumnName("profissao").HasMaxLength(120);
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        }
    }

    public class PessoaPjConfiguration : IEntityTypeConfiguration<PessoaPj>
    {
        public void Configure(EntityTypeBuilder<PessoaPj> builder)
        {
            builder.ToTable("cad_pessoas_pj");
            builder.HasKey(x => x.PessoaId);
            builder.Property(x => x.PessoaId).HasColumnName("pessoa_id");
            builder.Property(x => x.RazaoSocial).HasColumnName("razao_social").HasMaxLength(200).IsRequired();
            builder.Property(x => x.NomeFantasia).HasColumnName("nome_fantasia").HasMaxLength(200);
            builder.Property(x => x.InscricaoEstadual).HasColumnName("inscricao_estadual").HasMaxLength(50);
            builder.Property(x => x.InscricaoMunicipal).HasColumnName("inscricao_municipal").HasMaxLength(50);
            builder.Property(x => x.Responsavel).HasColumnName("responsavel").HasMaxLength(160);
            builder.Property(x => x.Website).HasColumnName("website").HasMaxLength(200);
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        }
    }

    public abstract class PessoaDetailConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);

        protected static void ConfigureBase(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            builder.Ignore(x => x.IsDeleted);
        }
    }

    public class PessoaContatoConfiguration : PessoaDetailConfiguration<PessoaContato>
    {
        public override void Configure(EntityTypeBuilder<PessoaContato> builder)
        {
            builder.ToTable("cad_pessoas_contatos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PessoaId).HasColumnName("pessoa_id");
            builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(40).IsRequired();
            builder.Property(x => x.Valor).HasColumnName("valor").HasMaxLength(180).IsRequired();
            builder.Property(x => x.Principal).HasColumnName("principal").HasColumnType("tinyint(1)");
            builder.Property(x => x.Observacao).HasColumnName("observacao").HasMaxLength(240);
            ConfigureBase(builder);
            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }

    public class PessoaEnderecoConfiguration : PessoaDetailConfiguration<PessoaEndereco>
    {
        public override void Configure(EntityTypeBuilder<PessoaEndereco> builder)
        {
            builder.ToTable("cad_pessoas_enderecos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PessoaId).HasColumnName("pessoa_id");
            builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(40).IsRequired();
            builder.Property(x => x.Cep).HasColumnName("cep").HasMaxLength(12);
            builder.Property(x => x.Logradouro).HasColumnName("logradouro").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Numero).HasColumnName("numero").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Bairro).HasColumnName("bairro").HasMaxLength(120).IsRequired();
            builder.Property(x => x.Cidade).HasColumnName("cidade").HasMaxLength(120).IsRequired();
            builder.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(60).IsRequired();
            builder.Property(x => x.Pais).HasColumnName("pais").HasMaxLength(80).IsRequired();
            builder.Property(x => x.Complemento).HasColumnName("complemento").HasMaxLength(160);
            builder.Property(x => x.Principal).HasColumnName("principal").HasColumnType("tinyint(1)");
            builder.Property(x => x.Observacao).HasColumnName("observacao").HasMaxLength(240);
            ConfigureBase(builder);
            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }

    public class PessoaDocumentoConfiguration : PessoaDetailConfiguration<PessoaDocumento>
    {
        public override void Configure(EntityTypeBuilder<PessoaDocumento> builder)
        {
            builder.ToTable("cad_pessoas_documentos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PessoaId).HasColumnName("pessoa_id");
            builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Numero).HasColumnName("numero").HasMaxLength(80).IsRequired();
            builder.Property(x => x.DataEmissao).HasColumnName("data_emissao");
            builder.Property(x => x.DataValidade).HasColumnName("data_validade");
            builder.Property(x => x.OrgaoExpedidor).HasColumnName("orgao_expedidor").HasMaxLength(100);
            builder.Property(x => x.ArquivoUrl).HasColumnName("arquivo_url").HasMaxLength(500);
            builder.Property(x => x.Observacoes).HasColumnName("observacoes").HasMaxLength(240);
            ConfigureBase(builder);
            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }

    public class PessoaAnexoConfiguration : PessoaDetailConfiguration<PessoaAnexo>
    {
        public override void Configure(EntityTypeBuilder<PessoaAnexo> builder)
        {
            builder.ToTable("cad_pessoas_anexos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PessoaId).HasColumnName("pessoa_id");
            builder.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Url).HasColumnName("url").HasMaxLength(500).IsRequired();
            builder.Property(x => x.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(x => x.TamanhoBytes).HasColumnName("tamanho_bytes");
            builder.Property(x => x.MimeType).HasColumnName("mime_type").HasMaxLength(120);
            ConfigureBase(builder);
            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}
