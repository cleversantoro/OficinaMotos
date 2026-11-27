using OficinaMotos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo
{
    public interface IPessoaPjRepository
    {
        Task<List<PessoaPj>> GetAllAsync();
        Task<PessoaPj?> GetByPessoaIdAsync(long pessoaId);
        Task<PessoaPj> AddAsync(PessoaPj entity);
        Task UpdateAsync(PessoaPj entity);
        Task DeleteAsync(long pessoaId);
        Task<bool> ExistsAsync(long pessoaId);
    }
}
