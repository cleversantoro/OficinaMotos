using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.SegurancaRepo
{
    public class SegUsuarioPerfilRepository : ISegUsuarioPerfilRepository
    {
        private readonly OficinaContext _context;
        private readonly DbSet<SegUsuarioPerfil> _dbSet;

        public SegUsuarioPerfilRepository(OficinaContext context)
        {
            _context = context;
            _dbSet = context.Set<SegUsuarioPerfil>();
        }

        public async Task<List<SegUsuarioPerfil>> GetByUsuarioAsync(long usuarioId)
            => await _dbSet.Where(up => up.UsuarioId == usuarioId).Include(up => up.Perfil).AsNoTracking().ToListAsync();

        public async Task<SegUsuarioPerfil?> GetByUsuarioAndPerfilAsync(long usuarioId, long perfilId)
            => await _dbSet.FirstOrDefaultAsync(up => up.UsuarioId == usuarioId && up.PerfilId == perfilId);

        public async Task AddAsync(SegUsuarioPerfil entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long usuarioId, long perfilId)
        {
            var entity = await _dbSet.FindAsync(usuarioId, perfilId);
            if (entity == null)
            {
                return;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
