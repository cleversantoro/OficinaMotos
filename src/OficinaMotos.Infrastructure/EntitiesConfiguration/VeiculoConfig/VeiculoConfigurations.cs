using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.VeiculoConfig
{
    public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.ToTable("cad_veiculos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ClienteId).HasColumnName("cliente_id");
            builder.Property(e => e.MarcaId).HasColumnName("marca_id");
            builder.Property(e => e.ModeloId).HasColumnName("modelo_id");
            builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(40);
            builder.Property(e => e.Placa).HasColumnName("placa").HasMaxLength(10);
            builder.Property(e => e.Renavam).HasColumnName("renavam").HasMaxLength(20);
            builder.Property(e => e.Chassi).HasColumnName("chassi").HasMaxLength(40);
            builder.Property(e => e.AnoFabricacao).HasColumnName("ano_fabricacao");
            builder.Property(e => e.AnoModelo).HasColumnName("ano_modelo");
            builder.Property(e => e.Cor).HasColumnName("cor").HasMaxLength(40);
            builder.Property(e => e.Combustivel).HasColumnName("combustivel").HasMaxLength(30);
            builder.Property(e => e.Quilometragem).HasColumnName("quilometragem");
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Cliente)
                   .WithMany(c => c.Veiculos)
                   .HasForeignKey(e => e.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Marca)
                   .WithMany()
                   .HasForeignKey(e => e.MarcaId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.Modelo)
                   .WithMany(m => m.Veiculos)
                   .HasForeignKey(e => e.ModeloId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class VeiculoMarcaConfiguration : IEntityTypeConfiguration<VeiculoMarca>
    {
        public void Configure(EntityTypeBuilder<VeiculoMarca> builder)
        {
            builder.ToTable("cad_veiculos_marcas");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasMany(e => e.Modelos)
                   .WithOne(m => m.Marca)
                   .HasForeignKey(m => m.MarcaId);
        }
    }

    public class VeiculoModeloConfiguration : IEntityTypeConfiguration<VeiculoModelo>
    {
        public void Configure(EntityTypeBuilder<VeiculoModelo> builder)
        {
            builder.ToTable("cad_veiculos_modelos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.MarcaId).HasColumnName("marca_id");
            builder.Property(e => e.Nome).HasColumnName("nome").HasMaxLength(120).IsRequired();
            builder.Property(e => e.Cilindrada).HasColumnName("cilindrada");
            builder.Property(e => e.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            builder.HasOne(e => e.Marca)
                   .WithMany(m => m.Modelos)
                   .HasForeignKey(e => e.MarcaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
