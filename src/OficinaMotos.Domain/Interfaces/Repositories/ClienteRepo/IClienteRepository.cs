using OficinaMotos.Domain.Entities;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<List<Cliente>> GetAllForTableAsync();        
    }
}
