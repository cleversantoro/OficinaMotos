using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.OrdemServicoConfig
{
    public class OrdemServicoConfiguration : IEntityTypeConfiguration<OrdemServico>
    {
        public void Configure(EntityTypeBuilder<OrdemServico> builder)
        {
            builder.ToTable("os_ordens");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Numero).HasColumnName("numero").IsRequired();
            builder.Property(e => e.ClienteId).HasColumnName("cliente_id");
            builder.Property(e => e.VeiculoId).HasColumnName("veiculo_id");
            builder.Property(e => e.DescricaoProblema).HasColumnName("descricao_problema").HasMaxLength(1000).IsRequired();
            builder.Property(e => e.Diagnostico).HasColumnName("diagnostico").HasColumnType("text");
            builder.Property(e => e.Status).HasColumnName("status").HasDefaultValue(OrdemServicoEnum.Aberta);
            builder.Property(e => e.Prioridade).HasColumnName("prioridade").HasDefaultValue(OrdemServicoPrioridade.Normal);
            builder.Property(e => e.DataAbertura).HasColumnName("data_abertura");
            builder.Property(e => e.PrevisaoEntrega).HasColumnName("previsao_entrega");
            builder.Property(e => e.DataConclusao).HasColumnName("data_conclusao");
            builder.Property(e => e.ValorPecas).HasColumnName("valor_pecas").HasPrecision(18, 2);
            builder.Property(e => e.ValorServicos).HasColumnName("valor_servicos").HasPrecision(18, 2);
            builder.Property(e => e.Desconto).HasColumnName("desconto").HasPrecision(18, 2);
            builder.Property(e => e.ValorTotal).HasColumnName("valor_total").HasPrecision(18, 2);
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(1000);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            builder.HasQueryFilter(e => e.DeletedAt == null);

            builder.HasOne(e => e.Cliente)
                   .WithMany(c => c.OrdensServico)
                   .HasForeignKey(e => e.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Veiculo)
                   .WithMany(v => v.OrdensServico)
                   .HasForeignKey(e => e.VeiculoId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Anexos)
                   .WithOne(a => a.OrdemServico)
                   .HasForeignKey(a => a.OrdemServicoId);

            builder.HasMany(e => e.Avaliacoes)
                   .WithOne(a => a.OrdemServico)
                   .HasForeignKey(a => a.OrdemServicoId);

            builder.HasMany(e => e.Checklists)
                   .WithOne(c => c.OrdemServico)
                   .HasForeignKey(c => c.OrdemServicoId);

            builder.HasMany(e => e.Itens)
                   .WithOne(i => i.OrdemServico)
                   .HasForeignKey(i => i.OrdemServicoId);

            builder.HasMany(e => e.ListaObservacoes)
                   .WithOne(o => o.OrdemServico)
                   .HasForeignKey(o => o.OrdemServicoId);

            builder.HasMany(e => e.Historico)
                   .WithOne(h => h.OrdemServico)
                   .HasForeignKey(h => h.OrdemServicoId);

            builder.HasMany(e => e.Pagamentos)
                   .WithOne(p => p.OrdemServico)
                   .HasForeignKey(p => p.OrdemServicoId);
        }
    }

    public class OrdemServicoAnexoConfiguration : IEntityTypeConfiguration<OrdemServicoAnexo>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoAnexo> builder)
        {
            builder.ToTable("os_anexos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(200).IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").IsRequired();
            builder.Property(e => e.Url).HasColumnName("url").HasMaxLength(500).IsRequired();
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        }
    }

    public class OrdemServicoAvaliacaoConfiguration : IEntityTypeConfiguration<OrdemServicoAvaliacao>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoAvaliacao> builder)
        {
            builder.ToTable("os_avaliacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.ClienteId).HasColumnName("cliente_id");
            builder.Property(e => e.Nota).HasColumnName("nota").IsRequired();
            builder.Property(e => e.Comentario).HasColumnName("comentario").HasMaxLength(1000);
            builder.Property(e => e.DataAvaliacao).HasColumnName("data_avaliacao");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Cliente)
                   .WithMany()
                   .HasForeignKey(e => e.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class OrdemServicoChecklistConfiguration : IEntityTypeConfiguration<OrdemServicoChecklist>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoChecklist> builder)
        {
            builder.ToTable("os_checklists");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.ResponsavelPrestadorId).HasColumnName("responsavel_prestador_id");
            builder.Property(e => e.Item).HasColumnName("item").HasMaxLength(200).IsRequired();
            builder.Property(e => e.Categoria).HasColumnName("categoria").HasMaxLength(80);
            builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(30).HasDefaultValue("PENDENTE").IsRequired();
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(300);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.ResponsavelPrestador)
                   .WithMany()
                   .HasForeignKey(e => e.ResponsavelPrestadorId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class OrdemServicoItemConfiguration : IEntityTypeConfiguration<OrdemServicoItem>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoItem> builder)
        {
            builder.ToTable("os_itens");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.PecaId).HasColumnName("peca_id");
            builder.Property(e => e.AtividadeProfissionalId).HasColumnName("atividade_profissional_id");
            builder.Property(e => e.PrestadorId).HasColumnName("prestador_id");
            builder.Property(e => e.Status).HasColumnName("status").IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").IsRequired();
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(500).IsRequired();
            builder.Property(e => e.Quantidade).HasColumnName("quantidade").HasPrecision(18, 3).IsRequired();
            builder.Property(e => e.ValorUnitario).HasColumnName("valor_unitario").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.Desconto).HasColumnName("desconto").HasPrecision(18, 2);
            builder.Property(e => e.Total).HasColumnName("total").HasPrecision(18, 2);
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.OrdemServicoItens)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.AtividadeProfissional)
                   .WithMany()
                   .HasForeignKey(e => e.AtividadeProfissionalId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Prestador)
                   .WithMany()
                   .HasForeignKey(e => e.PrestadorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class OrdemServicoObservacaoConfiguration : IEntityTypeConfiguration<OrdemServicoObservacao>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoObservacao> builder)
        {
            builder.ToTable("os_observacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            builder.Property(e => e.Texto).HasColumnName("texto").HasMaxLength(1000).IsRequired();
            builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(30).HasDefaultValue("INTERNA").IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Usuario)
                   .WithMany()
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class OrdemServicoHistoricoConfiguration : IEntityTypeConfiguration<OrdemServicoHistorico>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoHistorico> builder)
        {
            builder.ToTable("os_ordens_historico");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            builder.Property(e => e.DataAlteracao).HasColumnName("data_alteracao");
            builder.Property(e => e.Campo).HasColumnName("campo").HasMaxLength(120);
            builder.Property(e => e.ValorAntigo).HasColumnName("valor_antigo").HasMaxLength(500);
            builder.Property(e => e.ValorNovo).HasColumnName("valor_novo").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Usuario)
                   .WithMany()
                   .HasForeignKey(e => e.UsuarioId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class OrdemServicoPagamentoConfiguration : IEntityTypeConfiguration<OrdemServicoPagamento>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoPagamento> builder)
        {
            builder.ToTable("os_pagamentos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Valor).HasColumnName("valor").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
            builder.Property(e => e.Metodo).HasColumnName("metodo").HasMaxLength(120);
            builder.Property(e => e.Observacao).HasColumnName("observacao").HasMaxLength(240);
            builder.Property(e => e.DataPagamento).HasColumnName("data_pagamento");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        }
    }
}
