using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo
{
    public interface ISegPerfilPermissaoRepository
    {
        Task<List<SegPerfilPermissao>> GetByPerfilAsync(long perfilId);
        Task AddAsync(SegPerfilPermissao entity);
        Task DeleteByPerfilAsync(long perfilId);
        Task<bool> ExistsAsync(long perfilId, long permissaoId);
    }
}
