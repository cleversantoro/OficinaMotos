using OficinaMotos.Domain.Entities;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo
{
    public interface IEstoquePecaRepository : IRepository<EstoquePeca>
    {
        Task<bool> ExistsByCodigoAsync(string codigo);
    }
}
