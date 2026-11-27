using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorSegmentoRelRepository : IFornecedorSegmentoRelRepository
    {
        private readonly OficinaContext _context;
        private readonly DbSet<FornecedorSegmentoRel> _dbSet;

        public FornecedorSegmentoRelRepository(OficinaContext context)
        {
            _context = context;
            _dbSet = context.Set<FornecedorSegmentoRel>();
        }

        public async Task<List<FornecedorSegmentoRel>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<FornecedorSegmentoRel?> GetByKeysAsync(long fornecedorId, long segmentoId)
            => await _dbSet.FindAsync(fornecedorId, segmentoId);

        public async Task<FornecedorSegmentoRel> AddAsync(FornecedorSegmentoRel entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(long fornecedorId, long segmentoId)
        {
            var entity = await _dbSet.FindAsync(fornecedorId, segmentoId);
            if (entity == null)
            {
                return;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(long fornecedorId, long segmentoId)
            => await _dbSet.AnyAsync(x => x.FornecedorId == fornecedorId && x.SegmentoId == segmentoId);
    }
}
