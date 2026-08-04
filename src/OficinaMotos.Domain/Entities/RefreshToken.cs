using OficinaMotos.Domain.Common;

namespace OficinaMotos.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public long UsuarioId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
        public DateTime? RevogadoEm { get; set; }
        public string? MotivoRevogacao { get; set; }
        public string? IpCriacao { get; set; }
        public string? UserAgentCriacao { get; set; }
        public DateTime? UltimoUsoEm { get; set; }

        public SegUsuario? Usuario { get; set; }

        public bool EstaAtivo() => RevogadoEm == null && ExpiraEm > DateTime.UtcNow;

        public void Revogar(string motivo)
        {
            RevogadoEm = DateTime.UtcNow;
            MotivoRevogacao = motivo;
            SetUpdated();
        }

        public void MarcarUso()
        {
            UltimoUsoEm = DateTime.UtcNow;
            SetUpdated();
        }
    }
}
