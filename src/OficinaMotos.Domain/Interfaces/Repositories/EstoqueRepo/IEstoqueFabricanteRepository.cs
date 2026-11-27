using OficinaMotos.Domain.Entities;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo
{
    public interface IEstoqueFabricanteRepository : IRepository<EstoqueFabricante>
    {
        Task<bool> ExistsByNomeAsync(string nome);
    }
}
