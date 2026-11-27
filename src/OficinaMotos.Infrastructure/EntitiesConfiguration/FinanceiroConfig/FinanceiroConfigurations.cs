using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.FinanceiroConfig
{
    public class FinanceiroMetodoPagamentoConfiguration : IEntityTypeConfiguration<FinanceiroMetodoPagamento>
    {
        public void Configure(EntityTypeBuilder<FinanceiroMetodoPagamento> builder)
        {
            builder.ToTable("fin_metodos_pagamento");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(40).IsRequired();
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)");
            builder.Property(e => e.TaxaPercentual).HasColumnName("taxa_percentual").HasPrecision(8, 4);
            builder.Property(e => e.TaxaFixa).HasColumnName("taxa_fixa").HasPrecision(18, 2);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasMany(e => e.Pagamentos)
                   .WithOne(p => p.MetodoPagamento)
                   .HasForeignKey(p => p.MetodoPagamentoId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class FinanceiroPagamentoConfiguration : IEntityTypeConfiguration<FinanceiroPagamento>
    {
        public void Configure(EntityTypeBuilder<FinanceiroPagamento> builder)
        {
            builder.ToTable("fin_pagamentos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ContaPagarId).HasColumnName("conta_pagar_id");
            builder.Property(e => e.ContaReceberId).HasColumnName("conta_receber_id");
            builder.Property(e => e.MetodoPagamentoId).HasColumnName("metodo_pagamento_id");
            builder.Property(e => e.Valor).HasColumnName("valor").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.DataPagamento).HasColumnName("data_pagamento").IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(20).IsRequired();
            builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
            builder.Property(e => e.ReferenciaExterna).HasColumnName("referencia_externa").HasMaxLength(120);
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(300);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.MetodoPagamento)
                   .WithMany(m => m.Pagamentos)
                   .HasForeignKey(e => e.MetodoPagamentoId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.ContaPagar)
                   .WithMany(c => c.Pagamentos)
                   .HasForeignKey(e => e.ContaPagarId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ContaReceber)
                   .WithMany(c => c.Pagamentos)
                   .HasForeignKey(e => e.ContaReceberId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Anexos)
                   .WithOne(a => a.Pagamento)
                   .HasForeignKey(a => a.PagamentoId);
        }
    }

    public class FinanceiroAnexoConfiguration : IEntityTypeConfiguration<FinanceiroAnexo>
    {
        public void Configure(EntityTypeBuilder<FinanceiroAnexo> builder)
        {
            builder.ToTable("fin_anexos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ContaPagarId).HasColumnName("conta_pagar_id");
            builder.Property(e => e.ContaReceberId).HasColumnName("conta_receber_id");
            builder.Property(e => e.PagamentoId).HasColumnName("pagamento_id");
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(200).IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Url).HasColumnName("url").HasMaxLength(500).IsRequired();
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Pagamento)
                   .WithMany(p => p.Anexos)
                   .HasForeignKey(e => e.PagamentoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ContaPagar)
                   .WithMany(c => c.Anexos)
                   .HasForeignKey(e => e.ContaPagarId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ContaReceber)
                   .WithMany(c => c.Anexos)
                   .HasForeignKey(e => e.ContaReceberId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class FinanceiroContaPagarConfiguration : IEntityTypeConfiguration<FinanceiroContaPagar>
    {
        public void Configure(EntityTypeBuilder<FinanceiroContaPagar> builder)
        {
            builder.ToTable("fin_contas_pagar");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(240).IsRequired();
            builder.Property(e => e.Documento).HasColumnName("documento").HasMaxLength(80);
            builder.Property(e => e.ValorOriginal).HasColumnName("valor_original").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.ValorPago).HasColumnName("valor_pago").HasPrecision(18, 2);
            builder.Property(e => e.DataEmissao).HasColumnName("data_emissao");
            builder.Property(e => e.DataVencimento).HasColumnName("data_vencimento").IsRequired();
            builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Fornecedor)
                   .WithMany(f => f.ContasAPagar)
                   .HasForeignKey(e => e.FornecedorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Anexos)
                   .WithOne(a => a.ContaPagar)
                   .HasForeignKey(a => a.ContaPagarId);
        }
    }

    public class FinanceiroContaReceberConfiguration : IEntityTypeConfiguration<FinanceiroContaReceber>
    {
        public void Configure(EntityTypeBuilder<FinanceiroContaReceber> builder)
        {
            builder.ToTable("fin_contas_receber");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ClienteId).HasColumnName("cliente_id");
            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(240).IsRequired();
            builder.Property(e => e.ValorOriginal).HasColumnName("valor_original").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.ValorRecebido).HasColumnName("valor_recebido").HasPrecision(18, 2);
            builder.Property(e => e.DataEmissao).HasColumnName("data_emissao");
            builder.Property(e => e.DataVencimento).HasColumnName("data_vencimento").IsRequired();
            builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Cliente)
                   .WithMany()
                   .HasForeignKey(e => e.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.OrdemServico)
                   .WithMany(o => o.ContasReceber)
                   .HasForeignKey(e => e.OrdemServicoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Anexos)
                   .WithOne(a => a.ContaReceber)
                   .HasForeignKey(a => a.ContaReceberId);
        }
    }

    public class FinanceiroHistoricoConfiguration : IEntityTypeConfiguration<FinanceiroHistorico>
    {
        public void Configure(EntityTypeBuilder<FinanceiroHistorico> builder)
        {
            builder.ToTable("fin_historico");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Entidade).HasColumnName("entidade").HasMaxLength(80).IsRequired();
            builder.Property(e => e.EntidadeId).HasColumnName("entidade_id").IsRequired();
            builder.Property(e => e.Acao).HasColumnName("acao").HasMaxLength(40).IsRequired();
            builder.Property(e => e.DadosAnteriores).HasColumnName("dados_anteriores").HasColumnType("json");
            builder.Property(e => e.DadosNovos).HasColumnName("dados_novos").HasColumnType("json");
            builder.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Usuario)
                   .WithMany()
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class FinanceiroLancamentoConfiguration : IEntityTypeConfiguration<FinanceiroLancamento>
    {
        public void Configure(EntityTypeBuilder<FinanceiroLancamento> builder)
        {
            builder.ToTable("fin_lancamentos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(20).IsRequired();
            builder.Property(e => e.Categoria).HasColumnName("categoria").HasMaxLength(100);
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(240).IsRequired();
            builder.Property(e => e.Valor).HasColumnName("valor").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.DataCompetencia).HasColumnName("data_competencia").IsRequired();
            builder.Property(e => e.DataLancamento).HasColumnName("data_lancamento").IsRequired();
            builder.Property(e => e.OrigemTipo).HasColumnName("origem_tipo").HasMaxLength(40);
            builder.Property(e => e.OrigemId).HasColumnName("origem_id");
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        }
    }
}
