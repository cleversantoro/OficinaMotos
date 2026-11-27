using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using OficinaMotos.Infrastructure.Context;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.EstoqueRepo
{
    public class EstoquePecaRepository : Repository<EstoquePeca>, IEstoquePecaRepository
    {
        public EstoquePecaRepository(OficinaContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo)
        {
            return await _dbSet.AnyAsync(p => p.Codigo == codigo);
        }
    }
}
