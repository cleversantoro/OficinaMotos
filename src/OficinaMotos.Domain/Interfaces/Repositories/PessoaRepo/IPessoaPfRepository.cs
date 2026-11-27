using OficinaMotos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo
{
    public interface IPessoaPfRepository
    {
        Task<List<PessoaPf>> GetAllAsync();
        Task<PessoaPf?> GetByPessoaIdAsync(long pessoaId);
        Task<PessoaPf> AddAsync(PessoaPf entity);
        Task UpdateAsync(PessoaPf entity);
        Task DeleteAsync(long pessoaId);
        Task<bool> ExistsAsync(long pessoaId);
    }
}
