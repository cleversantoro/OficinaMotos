using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.FornecedorConfig
{
    public class FornecedorSegmentoConfiguration : IEntityTypeConfiguration<FornecedorSegmento>
    {
        public void Configure(EntityTypeBuilder<FornecedorSegmento> builder)
        {
            builder.ToTable("cad_fornecedores_segmentos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(240);
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasMany(e => e.FornecedoresPrincipais)
                   .WithOne(f => f.SegmentoPrincipal)
                   .HasForeignKey(f => f.SegmentoPrincipalId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.FornecedorSegmentos)
                   .WithOne(rel => rel.Segmento)
                   .HasForeignKey(rel => rel.SegmentoId);
        }
    }

    public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("cad_fornecedores");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PessoaId).HasColumnName("pessoa_id").IsRequired();
            builder.HasOne(e => e.Pessoa).WithMany().HasForeignKey(e => e.PessoaId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(40).IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(50).IsRequired();
            builder.Property(e => e.SegmentoPrincipalId).HasColumnName("segmento_principal_id");
            builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(30).HasDefaultValue("ATIVO");
            builder.Property(e => e.CondicaoPagamentoPadrao).HasColumnName("condicao_pagamento_padrao").HasMaxLength(120);
            builder.Property(e => e.PrazoEntregaMedio).HasColumnName("prazo_entrega_medio");
            builder.Property(e => e.NotaMedia).HasColumnName("nota_media").HasPrecision(4, 2);
            builder.Property(e => e.PrazoGarantiaPadrao).HasColumnName("prazo_garantia_padrao").HasMaxLength(120);
            builder.Property(e => e.TermosNegociados).HasColumnName("termos_negociados").HasMaxLength(240);
            builder.Property(e => e.AtendimentoPersonalizado).HasColumnName("atendimento_personalizado").HasColumnType("tinyint(1)");
            builder.Property(e => e.RetiradaLocal).HasColumnName("retirada_local").HasColumnType("tinyint(1)");
            builder.Property(e => e.RatingLogistica).HasColumnName("rating_logistica").HasPrecision(4, 2);
            builder.Property(e => e.RatingQualidade).HasColumnName("rating_qualidade").HasPrecision(4, 2);
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(600);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(e => e.DeletedAt == null);

            builder.HasMany(e => e.Segmentos)
                   .WithOne(rel => rel.Fornecedor)
                   .HasForeignKey(rel => rel.FornecedorId);

            builder.HasMany(e => e.Representantes)
                   .WithOne(r => r.Fornecedor)
                   .HasForeignKey(r => r.FornecedorId);

            builder.HasMany(e => e.Bancos)
                   .WithOne(b => b.Fornecedor)
                   .HasForeignKey(b => b.FornecedorId);

            builder.HasMany(e => e.Certificacoes)
                   .WithOne(c => c.Fornecedor)
                   .HasForeignKey(c => c.FornecedorId);

            builder.HasMany(e => e.Avaliacoes)
                   .WithOne(a => a.Fornecedor)
                   .HasForeignKey(a => a.FornecedorId);
        }
    }

    public class FornecedorSegmentoRelConfiguration : IEntityTypeConfiguration<FornecedorSegmentoRel>
    {
        public void Configure(EntityTypeBuilder<FornecedorSegmentoRel> builder)
        {
            builder.ToTable("cad_fornecedores_segmentos_rel");
            builder.HasKey(e => new { e.FornecedorId, e.SegmentoId });

            builder.Property(e => e.FornecedorId).HasColumnName("fornecedor_id").IsRequired();
            builder.Property(e => e.SegmentoId).HasColumnName("segmento_id").IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Segmentos)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Segmento)
                   .WithMany(s => s.FornecedorSegmentos)
                   .HasForeignKey(e => e.SegmentoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class FornecedorRepresentanteConfiguration : IEntityTypeConfiguration<FornecedorRepresentante>
    {
        public void Configure(EntityTypeBuilder<FornecedorRepresentante> builder)
        {
            builder.ToTable("cad_fornecedores_representantes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("fornecedor_id").IsRequired();
            builder.Property(e => e.PessoaId).HasColumnName("pessoa_id").IsRequired();
            builder.Property(e => e.Cargo).HasColumnName("cargo").HasMaxLength(120);
            builder.Property(e => e.Principal).HasColumnName("principal").HasColumnType("tinyint(1)").HasDefaultValue(false);
            builder.Property(e => e.Preferencial).HasColumnName("preferencial").HasColumnType("tinyint(1)").HasDefaultValue(false);
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Pessoa)
                   .WithMany()
                   .HasForeignKey(e => e.PessoaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Representantes)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class FornecedorBancoConfiguration : IEntityTypeConfiguration<FornecedorBanco>
    {
        public void Configure(EntityTypeBuilder<FornecedorBanco> builder)
        {
            builder.ToTable("cad_fornecedores_bancos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            builder.Property(e => e.Banco).HasColumnName("banco").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Agencia).HasColumnName("agencia").HasMaxLength(30);
            builder.Property(e => e.Conta).HasColumnName("conta").HasMaxLength(30);
            builder.Property(e => e.Digito).HasColumnName("digito").HasMaxLength(5);
            builder.Property(e => e.TipoConta).HasColumnName("tipo_conta").HasMaxLength(50);
            builder.Property(e => e.PixChave).HasColumnName("pix_chave").HasMaxLength(160);
            builder.Property(e => e.Principal).HasColumnName("principal").HasColumnType("tinyint(1)").HasDefaultValue(false);
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Bancos)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class FornecedorCertificacaoConfiguration : IEntityTypeConfiguration<FornecedorCertificacao>
    {
        public void Configure(EntityTypeBuilder<FornecedorCertificacao> builder)
        {
            builder.ToTable("cad_fornecedores_certificacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            builder.Property(e => e.Titulo).HasColumnName("titulo").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Instituicao).HasColumnName("instituicao").HasMaxLength(160);
            builder.Property(e => e.CodigoCertificacao).HasColumnName("codigo_certificacao").HasMaxLength(60);
            builder.Property(e => e.Escopo).HasColumnName("escopo").HasMaxLength(200);
            builder.Property(e => e.DataEmissao).HasColumnName("data_emissao");
            builder.Property(e => e.DataValidade).HasColumnName("data_validade");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Certificacoes)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class FornecedorAvaliacaoConfiguration : IEntityTypeConfiguration<FornecedorAvaliacao>
    {
        public void Configure(EntityTypeBuilder<FornecedorAvaliacao> builder)
        {
            builder.ToTable("cad_fornecedores_avaliacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            builder.Property(e => e.DataAvaliacao).HasColumnName("data_avaliacao").IsRequired();
            builder.Property(e => e.AvaliadoPor).HasColumnName("avaliado_por").HasMaxLength(120);
            builder.Property(e => e.Categoria).HasColumnName("categoria").HasMaxLength(60);
            builder.Property(e => e.Nota).HasColumnName("nota").HasPrecision(4, 2).IsRequired();
            builder.Property(e => e.Comentarios).HasColumnName("comentarios").HasMaxLength(400);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Avaliacoes)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
