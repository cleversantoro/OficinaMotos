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

            builder.Property(e => e.Placa).HasMaxLength(12).IsRequired();
            builder.Property(e => e.AnoFab).HasMaxLength(4).HasColumnName("Ano_Fab");
            builder.Property(e => e.AnoMod).HasMaxLength(4).HasColumnName("Ano_Mod");
            builder.Property(e => e.Cor).HasMaxLength(50);
            builder.Property(e => e.Chassi).HasMaxLength(80);
            builder.Property(e => e.Renavam).HasMaxLength(20);
            builder.Property(e => e.Km).HasMaxLength(20);
            builder.Property(e => e.ProximoKmRevisao).HasColumnName("proximo_km_revisao");
            builder.Property(e => e.Combustivel).HasMaxLength(50);
            builder.Property(e => e.Observacao).HasMaxLength(240);
            builder.Property(e => e.ClienteId).HasColumnName("Cliente_Id");
            builder.Property(e => e.ModeloId).HasColumnName("Modelo_Id");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Cliente)
                   .WithMany(c => c.Veiculos)
                   .HasForeignKey(e => e.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade);

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

            builder.Property(e => e.Nome).HasMaxLength(160).IsRequired();
            builder.Property(e => e.Pais).HasMaxLength(120);
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

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

            builder.Property(e => e.Nome).HasMaxLength(160).IsRequired();
            builder.Property(e => e.AnoInicio).HasColumnName("Ano_Inicio");
            builder.Property(e => e.AnoFim).HasColumnName("Ano_Fim");
            builder.Property(e => e.MarcaId).HasColumnName("Marca_Id");
            builder.Property(e => e.CreatedAt).HasColumnName("Created_At");
            builder.Property(e => e.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(e => e.Marca)
                   .WithMany(m => m.Modelos)
                   .HasForeignKey(e => e.MarcaId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
