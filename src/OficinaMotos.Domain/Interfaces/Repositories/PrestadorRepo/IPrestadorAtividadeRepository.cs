using OficinaMotos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo
{
    public interface IPrestadorAtividadeRepository
    {
        Task<List<PrestadorAtividade>> GetAllAsync();
        Task<PrestadorAtividade?> GetByIdAsync(long prestadorId, long atividadeProfissionalId);
        Task<PrestadorAtividade> AddAsync(PrestadorAtividade entity);
        Task UpdateAsync(PrestadorAtividade entity);
        Task DeleteAsync(long prestadorId, long atividadeProfissionalId);
        Task<bool> ExistsAsync(long prestadorId, long atividadeProfissionalId);
    }
}
