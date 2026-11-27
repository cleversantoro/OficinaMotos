using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.MecanicoConfig
{
    public class MecanicoConfiguration : IEntityTypeConfiguration<Mecanico>
    {
        public void Configure(EntityTypeBuilder<Mecanico> builder)
        {
            builder.ToTable("cad_mecanicos");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PrestadorId).HasColumnName("prestador_id");
            builder.Property(e => e.Matricula).HasColumnName("matricula").HasMaxLength(40);
            builder.HasOne(e => e.Prestador).WithOne(p => p.Mecanico).HasForeignKey<Mecanico>(e => e.PrestadorId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.Nivel).HasColumnName("nivel").HasMaxLength(40);
            builder.Property(e => e.DataAdmissao).HasColumnName("data_admissao");
            builder.Property(e => e.Observacoes).HasColumnName("observacoes").HasMaxLength(500);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

            //builder.Ignore(e => e.Codigo);
            //builder.Ignore(e => e.Nome);
            //builder.Ignore(e => e.Sobrenome);
            //builder.Ignore(e => e.NomeSocial);
            //builder.Ignore(e => e.DocumentoPrincipal);
            //builder.Ignore(e => e.TipoDocumento);
            //builder.Ignore(e => e.DataNascimento);
            //builder.Ignore(e => e.DataDemissao);
            //builder.Ignore(e => e.Status);
            //builder.Ignore(e => e.EspecialidadePrincipalId);
            //builder.Ignore(e => e.EspecialidadePrincipal);
            //builder.Ignore(e => e.ValorHora);
            //builder.Ignore(e => e.CargaHorariaSemanal);
            //builder.Ignore(e => e.Certificacoes);
            //builder.Ignore(e => e.Contatos);
            //builder.Ignore(e => e.Disponibilidades);
            //builder.Ignore(e => e.Documentos);
            //builder.Ignore(e => e.Enderecos);
            //builder.Ignore(e => e.Especialidades);
            //builder.Ignore(e => e.Experiencias);
            //builder.Ignore(e => e.OrdensServico);

            //builder.HasMany(e => e.Certificacoes)
            //       .WithOne(c => c.Mecanico)
            //       .HasForeignKey(c => c.MecanicoId);

            //builder.HasMany(e => e.Contatos)
            //       .WithOne(c => c.Mecanico)
            //       .HasForeignKey(c => c.MecanicoId);

            //builder.HasMany(e => e.Disponibilidades)
            //       .WithOne(d => d.Mecanico)
            //       .HasForeignKey(d => d.MecanicoId);

            //builder.HasMany(e => e.Documentos)
            //       .WithOne(d => d.Mecanico)
            //       .HasForeignKey(d => d.MecanicoId);

            //builder.HasMany(e => e.Enderecos)
            //       .WithOne(ei => ei.Mecanico)
            //       .HasForeignKey(ei => ei.MecanicoId);

            //builder.HasMany(e => e.Especialidades)
            //       .WithOne(rel => rel.Mecanico)
            //       .HasForeignKey(rel => rel.MecanicoId);

            //builder.HasMany(e => e.Experiencias)
            //       .WithOne(exp => exp.Mecanico)
            //       .HasForeignKey(exp => exp.MecanicoId);
        }
    }


}
