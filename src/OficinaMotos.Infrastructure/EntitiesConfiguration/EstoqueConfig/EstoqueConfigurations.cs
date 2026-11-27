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

            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(240);
            builder.Property(e => e.CategoriaPaiId).HasColumnName("categoria_pai_id");
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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
            builder.Property(e => e.Documento).HasColumnName("documento").HasMaxLength(30);
            builder.Property(e => e.Website).HasColumnName("website").HasMaxLength(200);
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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

            builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(40).IsRequired();
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(240);
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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

            builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(60).IsRequired();
            builder.Property(e => e.CodigoBarras).HasColumnName("codigo_barras").HasMaxLength(80);
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(180).IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(500);
            builder.Property(e => e.FabricanteId).HasColumnName("fabricante_id");
            builder.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            builder.Property(e => e.LocalizacaoId).HasColumnName("localizacao_id");
            builder.Property(e => e.Unidade).HasColumnName("unidade").HasMaxLength(20).HasDefaultValue("UN");
            builder.Property(e => e.QuantidadeEstoque).HasColumnName("quantidade_estoque").HasPrecision(18, 3).HasDefaultValue(0m);
            builder.Property(e => e.EstoqueMinimo).HasColumnName("estoque_minimo").HasPrecision(18, 3).HasDefaultValue(0m);
            builder.Property(e => e.EstoqueMaximo).HasColumnName("estoque_maximo").HasPrecision(18, 3);
            builder.Property(e => e.CustoMedio).HasColumnName("custo_medio").HasPrecision(18, 2);
            builder.Property(e => e.PrecoVenda).HasColumnName("preco_venda").HasPrecision(18, 2);
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(30).IsRequired();
            builder.Property(e => e.Quantidade).HasColumnName("quantidade").HasPrecision(18, 3).IsRequired();
            builder.Property(e => e.CustoUnitario).HasColumnName("custo_unitario").HasPrecision(18, 2);
            builder.Property(e => e.OrigemTipo).HasColumnName("origem_tipo").HasMaxLength(40);
            builder.Property(e => e.OrigemId).HasColumnName("origem_id");
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(300);
            builder.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.Movimentacoes)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Usuario)
                   .WithMany()
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class EstoquePecaAnexoConfiguration : IEntityTypeConfiguration<EstoquePecaAnexo>
    {
        public void Configure(EntityTypeBuilder<EstoquePecaAnexo> builder)
        {
            builder.ToTable("est_pecas_anexos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PecaId).HasColumnName("peca_id");
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(200).IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Url).HasColumnName("url").HasMaxLength(500).IsRequired();
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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
            builder.Property(e => e.CodigoFornecedor).HasColumnName("codigo_fornecedor").HasMaxLength(80);
            builder.Property(e => e.CustoAtual).HasColumnName("custo_atual").HasPrecision(18, 2);
            builder.Property(e => e.PrazoEntregaDias).HasColumnName("prazo_entrega_dias");
            builder.Property(e => e.Principal).HasColumnName("principal").HasColumnType("tinyint(1)");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

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
            builder.Property(e => e.Campo).HasColumnName("campo").HasMaxLength(80).IsRequired();
            builder.Property(e => e.ValorAnterior).HasColumnName("valor_anterior").HasMaxLength(500);
            builder.Property(e => e.ValorNovo).HasColumnName("valor_novo").HasMaxLength(500);
            builder.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.Historico)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Usuario)
                   .WithMany()
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
