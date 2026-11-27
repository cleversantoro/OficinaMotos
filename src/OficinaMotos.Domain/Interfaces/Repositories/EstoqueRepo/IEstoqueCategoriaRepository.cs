using OficinaMotos.Domain.Entities;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo
{
    public interface IEstoqueCategoriaRepository : IRepository<EstoqueCategoria>
    {
        Task<bool> ExistsByNomeAsync(string nome);
    }
}
