using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.EntitiesConfiguration.SegurancaConfig
{
    public class SegModuloConfiguration : IEntityTypeConfiguration<SegModulo>
    {
        public void Configure(EntityTypeBuilder<SegModulo> builder)
        {
            builder.ToTable("seg_modulos");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Nome).HasColumnName("Nome").HasMaxLength(80).IsRequired();
            builder.Property(m => m.Descricao).HasColumnName("Descricao").HasMaxLength(240);
            builder.Property(m => m.Icone).HasColumnName("Icone").HasMaxLength(100);
            builder.Property(m => m.Rota).HasColumnName("Rota").HasMaxLength(200);
            builder.Property(m => m.ModuloPaiId).HasColumnName("Modulo_Pai_Id");
            builder.Property(m => m.Ordem).HasColumnName("Ordem").IsRequired();
            builder.Property(m => m.Ativo).HasColumnName("Ativo").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(m => m.CreatedAt).HasColumnName("Created_At");
            builder.Property(m => m.UpdatedAt).HasColumnName("Updated_At");

            builder.HasOne(m => m.ModuloPai)
                .WithMany(m => m.SubModulos)
                .HasForeignKey(m => m.ModuloPaiId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(m => m.Permissoes)
                .WithOne(p => p.Modulo)
                .HasForeignKey(p => p.ModuloId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SegPerfilConfiguration : IEntityTypeConfiguration<SegPerfil>
    {
        public void Configure(EntityTypeBuilder<SegPerfil> builder)
        {
            builder.ToTable("seg_perfis");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasColumnName("Nome").HasMaxLength(80).IsRequired();
            builder.Property(p => p.Descricao).HasColumnName("Descricao").HasMaxLength(240);
            builder.Property(p => p.Nivel).HasColumnName("Nivel").IsRequired();
            builder.Property(p => p.Status).HasColumnName("Status").IsRequired();
            builder.Property(p => p.CreatedAt).HasColumnName("Created_At");
            builder.Property(p => p.UpdatedAt).HasColumnName("Updated_At");

            builder.HasMany(p => p.PerfisPermissoes)
                .WithOne(pp => pp.Perfil)
                .HasForeignKey(pp => pp.PerfilId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.UsuariosPerfis)
                .WithOne(up => up.Perfil)
                .HasForeignKey(up => up.PerfilId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SegPermissaoConfiguration : IEntityTypeConfiguration<SegPermissao>
    {
        public void Configure(EntityTypeBuilder<SegPermissao> builder)
        {
            builder.ToTable("seg_permissoes");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ModuloId).HasColumnName("Modulo_Id").IsRequired();
            builder.Property(p => p.Acao).HasColumnName("Acao").HasMaxLength(30).IsRequired();
            builder.Property(p => p.Descricao).HasColumnName("Descricao").HasMaxLength(200);
            builder.Property(p => p.CreatedAt).HasColumnName("Created_At");
            // seg_permissoes não tem Updated_At
            builder.Ignore(p => p.UpdatedAt);

            builder.HasIndex(p => new { p.ModuloId, p.Acao }).IsUnique().HasDatabaseName("UQ_seg_permissoes_modulo_acao");

            builder.HasMany(p => p.PerfisPermissoes)
                .WithOne(pp => pp.Permissao)
                .HasForeignKey(pp => pp.PermissaoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SegPerfilPermissaoConfiguration : IEntityTypeConfiguration<SegPerfilPermissao>
    {
        public void Configure(EntityTypeBuilder<SegPerfilPermissao> builder)
        {
            builder.ToTable("seg_perfis_permissoes");
            builder.HasKey(pp => pp.Id);

            builder.Property(pp => pp.PerfilId).HasColumnName("Perfil_Id").IsRequired();
            builder.Property(pp => pp.PermissaoId).HasColumnName("Permissao_Id").IsRequired();
            builder.Property(pp => pp.CreatedAt).HasColumnName("Created_At");
            // seg_perfis_permissoes não tem Updated_At
            builder.Ignore(pp => pp.UpdatedAt);

            builder.HasIndex(pp => new { pp.PerfilId, pp.PermissaoId }).IsUnique().HasDatabaseName("UQ_seg_perfis_permissoes");
        }
    }

    public class SegUsuarioConfiguration : IEntityTypeConfiguration<SegUsuario>
    {
        public void Configure(EntityTypeBuilder<SegUsuario> builder)
        {
            builder.ToTable("seg_usuarios");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome).HasColumnName("Nome").HasMaxLength(160).IsRequired();
            builder.Property(u => u.Email).HasColumnName("Email").HasMaxLength(160).IsRequired();
            builder.Property(u => u.Login).HasColumnName("Login").HasMaxLength(80).IsRequired();
            builder.Property(u => u.Senha).HasColumnName("Senha").HasMaxLength(255).IsRequired();
            builder.Property(u => u.Telefone).HasColumnName("Telefone").HasMaxLength(20);
            builder.Property(u => u.FotoUrl).HasColumnName("Foto_Url").HasMaxLength(500);
            builder.Property(u => u.Status).HasColumnName("Status").IsRequired();
            builder.Property(u => u.UltimoLogin).HasColumnName("Ultimo_Login");
            builder.Property(u => u.TokenReset).HasColumnName("Token_Reset").HasMaxLength(255);
            builder.Property(u => u.TokenResetExpiraEm).HasColumnName("Token_Reset_Expira_Em");
            builder.Property(u => u.TentativasLogin).HasColumnName("Tentativas_Login").IsRequired();
            builder.Property(u => u.BloqueadoAte).HasColumnName("Bloqueado_Ate");
            builder.Property(u => u.CriadoPor).HasColumnName("Criado_Por");
            builder.Property(u => u.CreatedAt).HasColumnName("Created_At");
            builder.Property(u => u.UpdatedAt).HasColumnName("Updated_At");

            builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("UQ_seg_usuarios_email");
            builder.HasIndex(u => u.Login).IsUnique().HasDatabaseName("UQ_seg_usuarios_login");

            builder.HasOne(u => u.CriadoPorUsuario)
                .WithMany()
                .HasForeignKey(u => u.CriadoPor)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.UsuariosPerfis)
                .WithOne(up => up.Usuario)
                .HasForeignKey(up => up.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.AuditLogs)
                .WithOne(al => al.Usuario)
                .HasForeignKey(al => al.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class SegUsuarioPerfilConfiguration : IEntityTypeConfiguration<SegUsuarioPerfil>
    {
        public void Configure(EntityTypeBuilder<SegUsuarioPerfil> builder)
        {
            builder.ToTable("seg_usuarios_perfis");
            builder.HasKey(up => up.Id);

            builder.Property(up => up.UsuarioId).HasColumnName("Usuario_Id").IsRequired();
            builder.Property(up => up.PerfilId).HasColumnName("Perfil_Id").IsRequired();
            builder.Property(up => up.Ativo).HasColumnName("Ativo").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(up => up.CreatedAt).HasColumnName("Created_At");
            builder.Property(up => up.UpdatedAt).HasColumnName("Updated_At");

            builder.HasIndex(up => new { up.UsuarioId, up.PerfilId }).IsUnique().HasDatabaseName("UQ_seg_usuarios_perfis");
        }
    }

    public class SegAuditLogConfiguration : IEntityTypeConfiguration<SegAuditLog>
    {
        public void Configure(EntityTypeBuilder<SegAuditLog> builder)
        {
            builder.ToTable("seg_audit_log");
            builder.HasKey(al => al.Id);

            builder.Property(al => al.UsuarioId).HasColumnName("Usuario_Id");
            builder.Property(al => al.Login).HasColumnName("Login").HasMaxLength(80);
            builder.Property(al => al.Acao).HasColumnName("Acao").HasMaxLength(30).IsRequired();
            builder.Property(al => al.Modulo).HasColumnName("Modulo").HasMaxLength(80);
            builder.Property(al => al.Tabela).HasColumnName("Tabela").HasMaxLength(80);
            builder.Property(al => al.RegistroId).HasColumnName("Registro_Id").HasMaxLength(40);
            builder.Property(al => al.Descricao).HasColumnName("Descricao").HasMaxLength(500);
            builder.Property(al => al.DadosAntes).HasColumnName("Dados_Antes").HasColumnType("json");
            builder.Property(al => al.DadosDepois).HasColumnName("Dados_Depois").HasColumnType("json");
            builder.Property(al => al.Ip).HasColumnName("Ip").HasMaxLength(45);
            builder.Property(al => al.UserAgent).HasColumnName("User_Agent").HasMaxLength(500);
            builder.Property(al => al.CreatedAt).HasColumnName("Created_At");
            // seg_audit_log é INSERT-ONLY — sem Updated_At
            builder.Ignore(al => al.UpdatedAt);
        }
    }

    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("seg_refresh_tokens");
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.UsuarioId).HasColumnName("Usuario_Id").IsRequired();
            builder.Property(rt => rt.TokenHash).HasColumnName("Token_Hash").HasMaxLength(255).IsRequired();
            builder.Property(rt => rt.ExpiraEm).HasColumnName("Expira_Em").IsRequired();
            builder.Property(rt => rt.RevogadoEm).HasColumnName("Revogado_Em");
            builder.Property(rt => rt.MotivoRevogacao).HasColumnName("Motivo_Revogacao").HasMaxLength(160);
            builder.Property(rt => rt.IpCriacao).HasColumnName("Ip_Criacao").HasMaxLength(45);
            builder.Property(rt => rt.UserAgentCriacao).HasColumnName("User_Agent_Criacao").HasMaxLength(500);
            builder.Property(rt => rt.UltimoUsoEm).HasColumnName("Ultimo_Uso_Em");
            builder.Property(rt => rt.CreatedAt).HasColumnName("Created_At");
            builder.Property(rt => rt.UpdatedAt).HasColumnName("Updated_At");

            builder.HasIndex(rt => rt.TokenHash).IsUnique().HasDatabaseName("UQ_seg_refresh_tokens_hash");

            builder.HasOne(rt => rt.Usuario)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
