using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash);
        Task<RefreshToken?> GetActiveByUserAsync(long usuarioId);
        Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(long usuarioId);
    }
}
