using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using OficinaMotos.Infrastructure.Context;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.EstoqueRepo
{
    public class EstoqueFabricanteRepository : Repository<EstoqueFabricante>, IEstoqueFabricanteRepository
    {
        public EstoqueFabricanteRepository(OficinaContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNomeAsync(string nome)
        {
            return await _dbSet.AnyAsync(f => f.Nome == nome);
        }
    }
}
