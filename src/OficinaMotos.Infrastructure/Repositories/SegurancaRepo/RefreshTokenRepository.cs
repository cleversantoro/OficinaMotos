using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories;

namespace OficinaMotos.Infrastructure.Repositories.SegurancaRepo
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(OficinaContext context) : base(context) { }

        public async Task<RefreshToken?> GetByHashAsync(string tokenHash)
            => await _dbSet.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        public async Task<RefreshToken?> GetActiveByUserAsync(long usuarioId)
            => await _dbSet
                .Where(rt => rt.UsuarioId == usuarioId && rt.RevogadoEm == null && rt.ExpiraEm > DateTime.UtcNow)
                .OrderByDescending(rt => rt.CreatedAt)
                .FirstOrDefaultAsync();

        public async Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(long usuarioId)
            => await _dbSet
                .Where(rt => rt.UsuarioId == usuarioId && rt.RevogadoEm == null && rt.ExpiraEm > DateTime.UtcNow)
                .OrderByDescending(rt => rt.CreatedAt)
                .ToListAsync();
    }
}
