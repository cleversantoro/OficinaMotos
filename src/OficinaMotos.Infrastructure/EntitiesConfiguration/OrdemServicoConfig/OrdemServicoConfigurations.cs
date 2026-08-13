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

            builder.Property(e => e.ClienteId).HasColumnName("cliente_id");
            builder.Property(e => e.MecanicoId).HasColumnName("mecanico_id");
            builder.Property(e => e.VeiculoId).HasColumnName("veiculo_id");
            builder.Property(e => e.DescricaoProblema).HasColumnName("descricao_problema").HasMaxLength(500).IsRequired();
            builder.Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => OrdemServicoStatusExtensions.ParseLegacy(v))
                .HasColumnName("status")
                .HasMaxLength(50)
                .HasDefaultValue(OrdemServicoStatus.Aberta);
            builder.Property(e => e.DataAbertura).HasColumnName("data_abertura");
            builder.Property(e => e.DataConclusao).HasColumnName("data_conclusao");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");


            builder.HasOne(e => e.Cliente)
                   .WithMany(c => c.OrdensServico)
                   .HasForeignKey(e => e.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Mecanico)
                   .WithMany(m => m.OrdensServico)
                   .HasForeignKey(e => e.MecanicoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Veiculo)
                   .WithMany(v => v.OrdensServico)
                   .HasForeignKey(e => e.VeiculoId)
                   .OnDelete(DeleteBehavior.Restrict);

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

            builder.HasMany(e => e.Observacoes)
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
            builder.Property(e => e.Nome).HasMaxLength(200);
            builder.Property(e => e.Tipo).HasMaxLength(100);
            builder.Property(e => e.Url).HasMaxLength(500);
            builder.Property(e => e.Observacao).HasMaxLength(240);
            builder.Property(e => e.DataUpload).HasColumnName("data_upload");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        }
    }

    public class OrdemServicoAvaliacaoConfiguration : IEntityTypeConfiguration<OrdemServicoAvaliacao>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoAvaliacao> builder)
        {
            builder.ToTable("os_avaliacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Nota).IsRequired();
            builder.Property(e => e.Comentario).HasMaxLength(500);
            builder.Property(e => e.Usuario).HasMaxLength(160);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        }
    }

    public class OrdemServicoChecklistConfiguration : IEntityTypeConfiguration<OrdemServicoChecklist>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoChecklist> builder)
        {
            builder.ToTable("os_checklists");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Item).HasMaxLength(240).IsRequired();
            builder.Property(e => e.Realizado).HasDefaultValue(false);
            builder.Property(e => e.Observacao).HasMaxLength(240);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
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
            builder.Property(e => e.Descricao).HasMaxLength(240).IsRequired();
            builder.Property(e => e.Quantidade).HasColumnName("quantidade").IsRequired();
            builder.Property(e => e.ValorUnitario).HasColumnName("valor_unitario").HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.Total)
                   .HasColumnName("total")
                   .HasPrecision(18, 2)
                   .ValueGeneratedOnAddOrUpdate()
                   .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(e => e.Peca)
                   .WithMany(p => p.OrdemServicoItens)
                   .HasForeignKey(e => e.PecaId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class OrdemServicoObservacaoConfiguration : IEntityTypeConfiguration<OrdemServicoObservacao>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoObservacao> builder)
        {
            builder.ToTable("os_observacoes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Usuario).HasMaxLength(160);
            builder.Property(e => e.Texto).HasMaxLength(500).IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        }
    }

    public class OrdemServicoHistoricoConfiguration : IEntityTypeConfiguration<OrdemServicoHistorico>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoHistorico> builder)
        {
            builder.ToTable("os_ordens_historico");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Usuario).HasMaxLength(160);
            builder.Property(e => e.Campo).HasMaxLength(120);
            builder.Property(e => e.ValorAntigo).HasColumnName("valor_antigo").HasMaxLength(300);
            builder.Property(e => e.ValorNovo).HasColumnName("valor_novo").HasMaxLength(300);
            builder.Property(e => e.DataAlteracao).HasColumnName("data_alteracao");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        }
    }

    public class OrdemServicoPagamentoConfiguration : IEntityTypeConfiguration<OrdemServicoPagamento>
    {
        public void Configure(EntityTypeBuilder<OrdemServicoPagamento> builder)
        {
            builder.ToTable("os_pagamentos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrdemServicoId).HasColumnName("ordem_servico_id");
            builder.Property(e => e.Valor).HasPrecision(18, 2).IsRequired();
            builder.Property(e => e.Status).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Metodo).HasMaxLength(120);
            builder.Property(e => e.Observacao).HasMaxLength(240);
            builder.Property(e => e.DataPagamento).HasColumnName("data_pagamento");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        }
    }
}
