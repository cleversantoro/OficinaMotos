using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.EstoqueConfig
{
    public class EstoqueCategoriaConfiguration : IEntityTypeConfiguration<EstoqueCategoria>
    {
        public void Configure(EntityTypeBuilder<EstoqueCategoria> builder)
        {
            builder.ToTable("est_categorias");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(300);
            builder.Property(e => e.CategoriaPaiId).HasColumnName("categoria_pai_id");
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasMany(e => e.Pecas)
                   .WithOne(p => p.Categoria)
                   .HasForeignKey(p => p.CategoriaId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class EstoqueFabricanteConfiguration : IEntityTypeConfiguration<EstoqueFabricante>
    {
        public void Configure(EntityTypeBuilder<EstoqueFabricante> builder)
        {
            builder.ToTable("est_fabricantes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(160).IsRequired();
            builder.Property(e => e.Cnpj).HasColumnName("cnpj").HasMaxLength(32);
            builder.Property(e => e.Documento).HasColumnName("documento").HasMaxLength(30);
            builder.Property(e => e.Website).HasColumnName("website").HasMaxLength(200);
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(e => e.Contato).HasColumnName("contato").HasMaxLength(160);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasMany(e => e.Pecas)
                   .WithOne(p => p.Fabricante)
                   .HasForeignKey(p => p.FabricanteId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class EstoqueLocalizacaoConfiguration : IEntityTypeConfiguration<EstoqueLocalizacao>
    {
        public void Configure(EntityTypeBuilder<EstoqueLocalizacao> builder)
        {
            builder.ToTable("est_localizacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(200).IsRequired();
            builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(40).IsRequired();
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(e => e.Corredor).HasColumnName("corredor").HasMaxLength(50);
            builder.Property(e => e.Prateleira).HasColumnName("prateleira").HasMaxLength(50);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasMany(e => e.Pecas)
                   .WithOne(p => p.Localizacao)
                   .HasForeignKey(p => p.LocalizacaoId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class EstoquePecaConfiguration : IEntityTypeConfiguration<EstoquePeca>
    {
        public void Configure(EntityTypeBuilder<EstoquePeca> builder)
        {
            builder.ToTable("est_pecas");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(80).IsRequired();
            builder.Property(e => e.CodigoBarras).HasColumnName("codigo_barras").HasMaxLength(80);
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(180).IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(240).IsRequired();
            builder.Property(e => e.FabricanteId).HasColumnName("fabricante_id");
            builder.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            builder.Property(e => e.LocalizacaoId).HasColumnName("localizacao_id");
            builder.Property(e => e.PrecoUnitario).HasColumnName("preco_unitario").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.CustoMedio).HasColumnName("custo_medio").HasPrecision(18,2);
            builder.Property(e => e.PrecoVenda).HasColumnName("preco_venda").HasPrecision(18,2);
            builder.Property(e => e.Quantidade).HasColumnName("quantidade").IsRequired();
            builder.Property(e => e.EstoqueMinimo).HasColumnName("estoque_minimo").HasDefaultValue(0);
            builder.Property(e => e.EstoqueMaximo).HasColumnName("estoque_maximo").HasDefaultValue(0);
            builder.Property(e => e.Unidade).HasColumnName("unidade").HasMaxLength(10).HasDefaultValue("UN");
            builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).HasDefaultValue("Ativo");
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.DataCadastro).HasColumnName("data_cadastro");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(e => e.Fabricante)
                   .WithMany(f => f.Pecas)
                   .HasForeignKey(e => e.FabricanteId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.Categoria)
                   .WithMany(c => c.Pecas)
                   .HasForeignKey(e => e.CategoriaId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.Localizacao)
                   .WithMany(l => l.Pecas)
                   .HasForeignKey(e => e.LocalizacaoId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Anexos)
                   .WithOne(a => a.Peca)
                   .HasForeignKey(a => a.PecaId);

            builder.HasMany(e => e.Fornecedores)
                   .WithOne(f => f.Peca)
                   .HasForeignKey(f => f.PecaId);

            builder.HasMany(e => e.Movimentacoes)
                   .WithOne(m => m.Peca)
                   .HasForeignKey(m => m.PecaId);

            builder.HasMany(e => e.Historico)
                   .WithOne(h => h.Peca)
                   .HasForeignKey(h => h.PecaId);
        }
    }

    public class EstoqueMovimentacaoConfiguration : IEntityTypeConfiguration<EstoqueMovimentacao>
    {
        public void Configure(EntityTypeBuilder<EstoqueMovimentacao> builder)
        {
            builder.ToTable("est_movimentacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PecaId).HasColumnName("peca_id");
            builder.Property(e => e.Quantidade).HasColumnName("quantidade").IsRequired();
            builder.Property(e => e.CustoUnitario).HasColumnName("custo_unitario").HasPrecision(18,2);
            builder.Property(e => e.OrigemTipo).HasColumnName("origem_tipo").HasMaxLength(40);
            builder.Property(e => e.OrigemId).HasColumnName("origem_id");
            builder.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Referencia).HasColumnName("referencia").HasMaxLength(120);
            builder.Property(e => e.Usuario).HasColumnName("usuario").HasMaxLength(120);
            builder.Property(e => e.DataMovimentacao).HasColumnName("data_movimentacao");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.Movimentacoes)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class EstoquePecaAnexoConfiguration : IEntityTypeConfiguration<EstoquePecaAnexo>
    {
        public void Configure(EntityTypeBuilder<EstoquePecaAnexo> builder)
        {
            builder.ToTable("est_pecas_anexos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PecaId).HasColumnName("peca_id");
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(200);
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(100);
            builder.Property(e => e.Url).HasColumnName("url").HasMaxLength(500);
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(e => e.DataUpload).HasColumnName("data_upload");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.Anexos)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class EstoquePecaFornecedorConfiguration : IEntityTypeConfiguration<EstoquePecaFornecedor>
    {
        public void Configure(EntityTypeBuilder<EstoquePecaFornecedor> builder)
        {
            builder.ToTable("est_pecas_fornecedores");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PecaId).HasColumnName("peca_id");
            builder.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            builder.Property(e => e.Preco).HasColumnName("preco").HasPrecision(18, 2);
            builder.Property(e => e.CodigoFornecedor).HasColumnName("codigo_fornecedor").HasMaxLength(80);
            builder.Property(e => e.CustoAtual).HasColumnName("custo_atual").HasPrecision(18,2);
            builder.Property(e => e.PrazoEntregaDias).HasColumnName("prazo_entrega_dias");
            builder.Property(e => e.Principal).HasColumnName("principal").HasColumnType("tinyint(1)");
            builder.Property(e => e.PrazoEntrega).HasColumnName("prazo_entrega");
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.Fornecedores)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.PecasOfertadas)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class EstoquePecaHistoricoConfiguration : IEntityTypeConfiguration<EstoquePecaHistorico>
    {
        public void Configure(EntityTypeBuilder<EstoquePecaHistorico> builder)
        {
            builder.ToTable("est_pecas_historico");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PecaId).HasColumnName("peca_id");
            builder.Property(e => e.Usuario).HasColumnName("usuario").HasMaxLength(120);
            builder.Property(e => e.Campo).HasColumnName("campo").HasMaxLength(120);
            builder.Property(e => e.ValorAntigo).HasColumnName("valor_antigo").HasMaxLength(300);
            builder.Property(e => e.ValorNovo).HasColumnName("valor_novo").HasMaxLength(300);
            builder.Property(e => e.DataAlteracao).HasColumnName("data_alteracao");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.Historico)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
