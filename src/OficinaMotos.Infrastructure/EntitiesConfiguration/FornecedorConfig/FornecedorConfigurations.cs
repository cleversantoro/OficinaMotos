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

            builder.Property(e => e.Codigo).HasColumnName("Codigo").HasMaxLength(40).IsRequired();
            builder.Property(e => e.Nome).HasColumnName("Nome").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("Descricao").HasMaxLength(300);
            builder.Property(e => e.Ativo).HasColumnName("Ativo").HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

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

            builder.Property(e => e.Codigo).HasColumnName("Codigo").HasMaxLength(40).IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("Tipo").HasMaxLength(50).IsRequired();
            builder.Property(e => e.RazaoSocial).HasColumnName("Razao_Social").HasMaxLength(200).IsRequired();
            builder.Property(e => e.NomeFantasia).HasColumnName("Nome_Fantasia").HasMaxLength(200);
            builder.Property(e => e.Documento).HasColumnName("Documento").HasMaxLength(32).IsRequired();
            builder.Property(e => e.InscricaoEstadual).HasColumnName("Inscricao_Estadual").HasMaxLength(50);
            builder.Property(e => e.InscricaoMunicipal).HasColumnName("Inscricao_Municipal").HasMaxLength(50);
            builder.Property(e => e.SegmentoPrincipalId).HasColumnName("Segmento_Principal_Id");
            builder.Property(e => e.Website).HasColumnName("Website").HasMaxLength(200);
            builder.Property(e => e.Email).HasColumnName("Email").HasMaxLength(160);
            builder.Property(e => e.TelefonePrincipal).HasColumnName("Telefone_Principal").HasMaxLength(40);
            builder.Property(e => e.Status).HasColumnName("Status").HasMaxLength(50).HasDefaultValue("ATIVO");
            builder.Property(e => e.CondicaoPagamentoPadrao).HasColumnName("Condicao_Pagamento_Padrao").HasMaxLength(120);
            builder.Property(e => e.PrazoEntregaMedio).HasColumnName("Prazo_Entrega_Medio");
            builder.Property(e => e.NotaMedia).HasColumnName("Nota_Media").HasPrecision(4, 2);
            builder.Property(e => e.Observacoes).HasColumnName("Observacoes").HasMaxLength(600);
            builder.Property(e => e.PrazoGarantiaPadrao).HasColumnName("Prazo_Garantia_Padrao").HasMaxLength(120);
            builder.Property(e => e.TermosNegociados).HasColumnName("Termos_Negociados").HasMaxLength(240);
            builder.Property(e => e.AtendimentoPersonalizado).HasColumnName("Atendimento_Personalizado").HasDefaultValue(false);
            builder.Property(e => e.RetiradaLocal).HasColumnName("Retirada_Local").HasDefaultValue(false);
            builder.Property(e => e.RatingLogistica).HasColumnName("Rating_Logistica").HasPrecision(4, 2);
            builder.Property(e => e.RatingQualidade).HasColumnName("Rating_Qualidade").HasPrecision(4, 2);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasMany(e => e.Segmentos)
                   .WithOne(rel => rel.Fornecedor)
                   .HasForeignKey(rel => rel.FornecedorId);

            builder.HasMany(e => e.Enderecos)
                   .WithOne(end => end.Fornecedor)
                   .HasForeignKey(end => end.FornecedorId);

            builder.HasMany(e => e.Contatos)
                   .WithOne(c => c.Fornecedor)
                   .HasForeignKey(c => c.FornecedorId);

            builder.HasMany(e => e.Representantes)
                   .WithOne(r => r.Fornecedor)
                   .HasForeignKey(r => r.FornecedorId);

            builder.HasMany(e => e.Bancos)
                   .WithOne(b => b.Fornecedor)
                   .HasForeignKey(b => b.FornecedorId);

            builder.HasMany(e => e.Documentos)
                   .WithOne(d => d.Fornecedor)
                   .HasForeignKey(d => d.FornecedorId);

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
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.SegmentoId).HasColumnName("Segmento_Id");
            builder.Property(e => e.Principal).HasColumnName("Principal").HasDefaultValue(false);
            builder.Property(e => e.Observacoes).HasColumnName("Observacoes").HasMaxLength(300);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Segmentos)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Segmento)
                   .WithMany(s => s.FornecedorSegmentos)
                   .HasForeignKey(e => e.SegmentoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FornecedorEnderecoConfiguration : IEntityTypeConfiguration<FornecedorEndereco>
    {
        public void Configure(EntityTypeBuilder<FornecedorEndereco> builder)
        {
            builder.ToTable("cad_fornecedores_enderecos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.Tipo).HasColumnName("Tipo").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Cep).HasColumnName("Cep").HasMaxLength(20);
            builder.Property(e => e.Logradouro).HasColumnName("Logradouro").HasMaxLength(200).IsRequired();
            builder.Property(e => e.Numero).HasColumnName("Numero").HasMaxLength(20).IsRequired();
            builder.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Estado).HasColumnName("Estado").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Pais).HasColumnName("Pais").HasMaxLength(80);
            builder.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(120);
            builder.Property(e => e.Principal).HasColumnName("Principal").HasDefaultValue(false);
            builder.Property(e => e.Observacao).HasColumnName("Observacao").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Enderecos)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FornecedorContatoConfiguration : IEntityTypeConfiguration<FornecedorContato>
    {
        public void Configure(EntityTypeBuilder<FornecedorContato> builder)
        {
            builder.ToTable("cad_fornecedores_contatos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.Tipo).HasColumnName("Tipo").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Valor).HasColumnName("Valor").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Principal).HasColumnName("Principal").HasDefaultValue(false);
            builder.Property(e => e.Observacao).HasColumnName("Observacao").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Contatos)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FornecedorRepresentanteConfiguration : IEntityTypeConfiguration<FornecedorRepresentante>
    {
        public void Configure(EntityTypeBuilder<FornecedorRepresentante> builder)
        {
            builder.ToTable("cad_fornecedores_representantes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.PessoaId).HasColumnName("pessoa_id").IsRequired();
            builder.Ignore(e => e.Nome);
            builder.Ignore(e => e.Email);
            builder.Ignore(e => e.Telefone);
            builder.Ignore(e => e.Celular);
            builder.Ignore(e => e.PreferenciaContato);
            builder.HasOne(e => e.Pessoa).WithMany().HasForeignKey(e => e.PessoaId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.Nome).HasColumnName("Nome").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Cargo).HasColumnName("Cargo").HasMaxLength(100);
            builder.Property(e => e.Email).HasColumnName("Email").HasMaxLength(160);
            builder.Property(e => e.Telefone).HasColumnName("Telefone").HasMaxLength(40);
            builder.Property(e => e.Celular).HasColumnName("Celular").HasMaxLength(40);
            builder.Property(e => e.PreferenciaContato).HasColumnName("Preferencia_Contato").HasMaxLength(60);
            builder.Property(e => e.Principal).HasColumnName("Principal").HasDefaultValue(false);
            builder.Property(e => e.Observacoes).HasColumnName("Observacoes").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Representantes)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FornecedorBancoConfiguration : IEntityTypeConfiguration<FornecedorBanco>
    {
        public void Configure(EntityTypeBuilder<FornecedorBanco> builder)
        {
            builder.ToTable("cad_fornecedores_bancos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.Banco).HasColumnName("Banco").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Agencia).HasColumnName("Agencia").HasMaxLength(30);
            builder.Property(e => e.Conta).HasColumnName("Conta").HasMaxLength(30);
            builder.Property(e => e.Digito).HasColumnName("Digito").HasMaxLength(5);
            builder.Property(e => e.TipoConta).HasColumnName("Tipo_Conta").HasMaxLength(50);
            builder.Property(e => e.PixChave).HasColumnName("Pix_Chave").HasMaxLength(160);
            builder.Property(e => e.Principal).HasColumnName("Principal").HasDefaultValue(false);
            builder.Property(e => e.Observacoes).HasColumnName("Observacoes").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Bancos)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FornecedorDocumentoConfiguration : IEntityTypeConfiguration<FornecedorDocumento>
    {
        public void Configure(EntityTypeBuilder<FornecedorDocumento> builder)
        {
            builder.ToTable("cad_fornecedores_documentos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.Tipo).HasColumnName("Tipo").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Numero).HasColumnName("Numero").HasMaxLength(120).IsRequired();
            builder.Property(e => e.OrgaoExpedidor).HasColumnName("Orgao_Expedidor").HasMaxLength(120);
            builder.Property(e => e.ArquivoUrl).HasColumnName("Arquivo_Url").HasMaxLength(240);
            builder.Property(e => e.Observacoes).HasColumnName("Observacoes").HasMaxLength(240);
            builder.Property(e => e.DataEmissao).HasColumnName("Data_Emissao");
            builder.Property(e => e.DataValidade).HasColumnName("Data_Validade");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Documentos)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FornecedorCertificacaoConfiguration : IEntityTypeConfiguration<FornecedorCertificacao>
    {
        public void Configure(EntityTypeBuilder<FornecedorCertificacao> builder)
        {
            builder.ToTable("cad_fornecedores_certificacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.Titulo).HasColumnName("Titulo").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Instituicao).HasColumnName("Instituicao").HasMaxLength(160);
            builder.Property(e => e.CodigoCertificacao).HasColumnName("Codigo_Certificacao").HasMaxLength(60);
            builder.Property(e => e.Escopo).HasColumnName("Escopo").HasMaxLength(200);
            builder.Property(e => e.DataEmissao).HasColumnName("Data_Emissao");
            builder.Property(e => e.DataValidade).HasColumnName("Data_Validade");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Certificacoes)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FornecedorAvaliacaoConfiguration : IEntityTypeConfiguration<FornecedorAvaliacao>
    {
        public void Configure(EntityTypeBuilder<FornecedorAvaliacao> builder)
        {
            builder.ToTable("cad_fornecedores_avaliacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("Fornecedor_Id");
            builder.Property(e => e.DataAvaliacao).HasColumnName("Data_Avaliacao").IsRequired();
            builder.Property(e => e.AvaliadoPor).HasColumnName("Avaliado_Por").HasMaxLength(120);
            builder.Property(e => e.Categoria).HasColumnName("Categoria").HasMaxLength(60);
            builder.Property(e => e.Nota).HasColumnName("Nota").HasPrecision(4, 2).IsRequired();
            builder.Property(e => e.Comentarios).HasColumnName("Comentarios").HasMaxLength(400);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.Avaliacoes)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
