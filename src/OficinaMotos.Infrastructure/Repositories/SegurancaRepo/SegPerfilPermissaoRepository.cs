using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.SegurancaRepo
{
    public class SegPerfilPermissaoRepository : ISegPerfilPermissaoRepository
    {
        private readonly OficinaContext _context;
        private readonly DbSet<SegPerfilPermissao> _dbSet;

        public SegPerfilPermissaoRepository(OficinaContext context)
        {
            _context = context;
            _dbSet = context.Set<SegPerfilPermissao>();
        }

        public async Task<List<SegPerfilPermissao>> GetByPerfilAsync(long perfilId)
            => await _dbSet.Where(pp => pp.PerfilId == perfilId).AsNoTracking().ToListAsync();

        public async Task AddAsync(SegPerfilPermissao entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByPerfilAsync(long perfilId)
        {
            var items = await _dbSet.Where(pp => pp.PerfilId == perfilId).ToListAsync();
            _dbSet.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(long perfilId, long permissaoId)
            => await _dbSet.AnyAsync(pp => pp.PerfilId == perfilId && pp.PermissaoId == permissaoId);
    }
}
