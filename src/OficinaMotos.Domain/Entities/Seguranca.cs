using OficinaMotos.Domain.Common;

namespace OficinaMotos.Domain.Entities
{
    // ──────────────────────────────────────────────
    // seg_modulos
    // ──────────────────────────────────────────────
    public class SegModulo : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Icone { get; set; }
        public string? Rota { get; set; }
        public long? ModuloPaiId { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; } = true;

        public SegModulo? ModuloPai { get; set; }
        public ICollection<SegModulo> SubModulos { get; set; } = new HashSet<SegModulo>();
        public ICollection<SegPermissao> Permissoes { get; set; } = new HashSet<SegPermissao>();
    }

    // ──────────────────────────────────────────────
    // seg_perfis
    // ──────────────────────────────────────────────
    public class SegPerfil : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        /// <summary>1 = máximo acesso; valores maiores = acesso mais restrito.</summary>
        public int Nivel { get; set; } = 99;
        /// <summary>0 = Inativo | 1 = Ativo</summary>
        public int Status { get; set; } = 1;

        public ICollection<SegPerfilPermissao> PerfisPermissoes { get; set; } = new HashSet<SegPerfilPermissao>();
        public ICollection<SegUsuarioPerfil> UsuariosPerfis { get; set; } = new HashSet<SegUsuarioPerfil>();
    }

    // ──────────────────────────────────────────────
    // seg_permissoes
    // ──────────────────────────────────────────────
    public class SegPermissao : BaseEntity
    {
        public long ModuloId { get; set; }
        /// <summary>visualizar | criar | editar | excluir | exportar | aprovar</summary>
        public string Acao { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        public SegModulo? Modulo { get; set; }
        public ICollection<SegPerfilPermissao> PerfisPermissoes { get; set; } = new HashSet<SegPerfilPermissao>();
    }

    // ──────────────────────────────────────────────
    // seg_perfis_permissoes
    // ──────────────────────────────────────────────
    public class SegPerfilPermissao : BaseEntity
    {
        public long PerfilId { get; set; }
        public long PermissaoId { get; set; }

        public SegPerfil? Perfil { get; set; }
        public SegPermissao? Permissao { get; set; }
    }

    // ──────────────────────────────────────────────
    // seg_usuarios
    // ──────────────────────────────────────────────
    public class SegUsuario : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        /// <summary>Hash bcrypt custo 12. Nunca armazenar senha em texto puro.</summary>
        public string Senha { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? FotoUrl { get; set; }
        /// <summary>0 = Inativo | 1 = Ativo | 2 = Suspenso | 3 = Bloqueado</summary>
        public int Status { get; set; } = 1;
        public DateTime? UltimoLogin { get; set; }
        public string? TokenReset { get; set; }
        public DateTime? TokenResetExpiraEm { get; set; }
        public int TentativasLogin { get; set; } = 0;
        public DateTime? BloqueadoAte { get; set; }
        public long? CriadoPor { get; set; }

        public SegUsuario? CriadoPorUsuario { get; set; }
        public ICollection<SegUsuarioPerfil> UsuariosPerfis { get; set; } = new HashSet<SegUsuarioPerfil>();
        public ICollection<SegAuditLog> AuditLogs { get; set; } = new HashSet<SegAuditLog>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();

        public void RegistrarLoginSucesso()
        {
            UltimoLogin = DateTime.UtcNow;
            TentativasLogin = 0;
            BloqueadoAte = null;
            SetUpdated();
        }

        public void RegistrarFalhaLogin()
        {
            TentativasLogin++;
            if (TentativasLogin >= 5)
                BloqueadoAte = DateTime.UtcNow.AddMinutes(30);
            SetUpdated();
        }

        public bool EstaBloqueado() =>
            BloqueadoAte.HasValue && BloqueadoAte.Value > DateTime.UtcNow;
    }

    // ──────────────────────────────────────────────
    // seg_usuarios_perfis
    // ──────────────────────────────────────────────
    public class SegUsuarioPerfil : BaseEntity
    {
        public long UsuarioId { get; set; }
        public long PerfilId { get; set; }
        public bool Ativo { get; set; } = true;

        public SegUsuario? Usuario { get; set; }
        public SegPerfil? Perfil { get; set; }
    }

    // ──────────────────────────────────────────────
    // seg_audit_log  (INSERT-ONLY — não atualizar/excluir)
    // ──────────────────────────────────────────────
    public class SegAuditLog : BaseEntity
    {
        /// <summary>NULL para eventos sem autenticação (ex.: LOGIN_FAIL).</summary>
        public long? UsuarioId { get; set; }
        public string? Login { get; set; }
        /// <summary>LOGIN | LOGOUT | LOGIN_FAIL | CREATE | UPDATE | DELETE | VIEW | EXPORT | APPROVE</summary>
        public string Acao { get; set; } = string.Empty;
        public string? Modulo { get; set; }
        public string? Tabela { get; set; }
        public string? RegistroId { get; set; }
        public string? Descricao { get; set; }
        /// <summary>JSON com os dados antes da alteração.</summary>
        public string? DadosAntes { get; set; }
        /// <summary>JSON com os dados após a alteração.</summary>
        public string? DadosDepois { get; set; }
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }

        public SegUsuario? Usuario { get; set; }
    }
}
