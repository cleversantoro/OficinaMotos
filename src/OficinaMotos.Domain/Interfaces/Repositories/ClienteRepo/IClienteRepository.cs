using OficinaMotos.Domain.Entities;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<List<Cliente>> GetAllForTableAsync();

        Task<Cliente?> GetByCpfAsync(string cpf);
        
        Task<Cliente?> GetByEmailAsync(string email);
        Task SyncPessoaContatosAsync(long pessoaId, string? email, string? telefone);
    }
}
