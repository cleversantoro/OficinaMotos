using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using OficinaMotos.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.VeiculoRepo
{
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<Veiculo>> GetAllAsync()
        {
            return await _dbSet
                .Include(v => v.Modelo)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Veiculo?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(v => v.Modelo)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
