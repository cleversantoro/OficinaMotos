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

            builder.Property(m => m.Nome).HasColumnName("nome").HasMaxLength(80).IsRequired();
            builder.Property(m => m.Descricao).HasColumnName("descricao").HasMaxLength(240);
            builder.Property(m => m.Icone).HasColumnName("icone").HasMaxLength(100);
            builder.Property(m => m.Rota).HasColumnName("rota").HasMaxLength(200);
            builder.Property(m => m.ModuloPaiId).HasColumnName("modulo_pai_id");
            builder.Property(m => m.Ordem).HasColumnName("ordem").IsRequired();
            builder.Property(m => m.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(m => m.CreatedAt).HasColumnName("created_at");
            builder.Property(m => m.UpdatedAt).HasColumnName("updated_at");
            builder.Ignore(m => m.IsDeleted);
            builder.Ignore(m => m.DeletedAt);

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

            builder.Property(p => p.Nome).HasColumnName("nome").HasMaxLength(80).IsRequired();
            builder.Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(240);
            builder.Property(p => p.Nivel).HasColumnName("nivel").IsRequired();
            builder.Property(p => p.Status).HasColumnName("status").IsRequired();
            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");
            builder.Property(p => p.DeletedAt).HasColumnName("deleted_at");
            builder.Ignore(p => p.IsDeleted);
            builder.HasQueryFilter(p => p.DeletedAt == null);

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

            builder.Property(p => p.ModuloId).HasColumnName("modulo_id").IsRequired();
            builder.Property(p => p.Acao).HasColumnName("acao").HasMaxLength(30).IsRequired();
            builder.Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(200);
            builder.Property(p => p.CreatedAt).HasColumnName("created_at");
            builder.Ignore(p => p.IsDeleted);
            builder.Ignore(p => p.DeletedAt);
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

            builder.Property(pp => pp.PerfilId).HasColumnName("perfil_id").IsRequired();
            builder.Property(pp => pp.PermissaoId).HasColumnName("permissao_id").IsRequired();
            builder.Property(pp => pp.CreatedAt).HasColumnName("created_at");
            builder.Ignore(pp => pp.IsDeleted);
            builder.Ignore(pp => pp.DeletedAt);
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

            builder.Property(u => u.Nome).HasColumnName("nome").HasMaxLength(160).IsRequired();
            builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(160).IsRequired();
            builder.Property(u => u.Login).HasColumnName("login").HasMaxLength(80).IsRequired();
            builder.Property(u => u.Senha).HasColumnName("senha").HasMaxLength(255).IsRequired();
            builder.Property(u => u.Telefone).HasColumnName("telefone").HasMaxLength(20);
            builder.Property(u => u.FotoUrl).HasColumnName("foto_url").HasMaxLength(500);
            builder.Property(u => u.Status).HasColumnName("status").IsRequired();
            builder.Property(u => u.UltimoLogin).HasColumnName("ultimo_login");
            builder.Property(u => u.TokenReset).HasColumnName("token_reset").HasMaxLength(255);
            builder.Property(u => u.TokenResetExpiraEm).HasColumnName("token_reset_expira_em");
            builder.Property(u => u.TentativasLogin).HasColumnName("tentativas_login").IsRequired();
            builder.Property(u => u.BloqueadoAte).HasColumnName("bloqueado_ate");
            builder.Property(u => u.CriadoPor).HasColumnName("criado_por");
            builder.Property(u => u.CreatedAt).HasColumnName("created_at");
            builder.Property(u => u.UpdatedAt).HasColumnName("updated_at");
            builder.Property(u => u.DeletedAt).HasColumnName("deleted_at");
            builder.Ignore(u => u.IsDeleted);
            builder.HasQueryFilter(u => u.DeletedAt == null);

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

            builder.Property(up => up.UsuarioId).HasColumnName("usuario_id").IsRequired();
            builder.Property(up => up.PerfilId).HasColumnName("perfil_id").IsRequired();
            builder.Property(up => up.Ativo).HasColumnName("ativo").HasColumnType("tinyint(1)").IsRequired();
            builder.Property(up => up.CreatedAt).HasColumnName("created_at");
            builder.Ignore(up => up.UpdatedAt);
            builder.Ignore(up => up.IsDeleted);
            builder.Ignore(up => up.DeletedAt);

            builder.HasIndex(up => new { up.UsuarioId, up.PerfilId }).IsUnique().HasDatabaseName("UQ_seg_usuarios_perfis");
        }
    }

    public class SegAuditLogConfiguration : IEntityTypeConfiguration<SegAuditLog>
    {
        public void Configure(EntityTypeBuilder<SegAuditLog> builder)
        {
            builder.ToTable("seg_audit_log");
            builder.HasKey(al => al.Id);

            builder.Property(al => al.UsuarioId).HasColumnName("usuario_id");
            builder.Property(al => al.Login).HasColumnName("login").HasMaxLength(80);
            builder.Property(al => al.Acao).HasColumnName("acao").HasMaxLength(30).IsRequired();
            builder.Property(al => al.Modulo).HasColumnName("modulo").HasMaxLength(80);
            builder.Property(al => al.Tabela).HasColumnName("tabela").HasMaxLength(80);
            builder.Property(al => al.RegistroId).HasColumnName("registro_id").HasMaxLength(40);
            builder.Property(al => al.Descricao).HasColumnName("descricao").HasMaxLength(500);
            builder.Property(al => al.DadosAntes).HasColumnName("dados_antes").HasColumnType("json");
            builder.Property(al => al.DadosDepois).HasColumnName("dados_depois").HasColumnType("json");
            builder.Property(al => al.Ip).HasColumnName("ip").HasMaxLength(45);
            builder.Property(al => al.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
            builder.Property(al => al.CreatedAt).HasColumnName("created_at");
            builder.Ignore(al => al.IsDeleted);
            builder.Ignore(al => al.DeletedAt);
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

            builder.Property(rt => rt.UsuarioId).HasColumnName("usuario_id").IsRequired();
            builder.Property(rt => rt.TokenHash).HasColumnName("token_hash").HasMaxLength(255).IsRequired();
            builder.Property(rt => rt.ExpiraEm).HasColumnName("expira_em").IsRequired();
            builder.Property(rt => rt.RevogadoEm).HasColumnName("revogado_em");
            builder.Property(rt => rt.MotivoRevogacao).HasColumnName("motivo_revogacao").HasMaxLength(160);
            builder.Property(rt => rt.IpCriacao).HasColumnName("ip_criacao").HasMaxLength(45);
            builder.Property(rt => rt.UserAgentCriacao).HasColumnName("user_agent_criacao").HasMaxLength(500);
            builder.Property(rt => rt.UltimoUsoEm).HasColumnName("ultimo_uso_em");
            builder.Property(rt => rt.CreatedAt).HasColumnName("created_at");
            builder.Property(rt => rt.UpdatedAt).HasColumnName("updated_at");
            builder.Ignore(rt => rt.IsDeleted);
            builder.Ignore(rt => rt.DeletedAt);

            builder.HasIndex(rt => rt.TokenHash).IsUnique().HasDatabaseName("UQ_seg_refresh_tokens_hash");

            builder.HasOne(rt => rt.Usuario)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
