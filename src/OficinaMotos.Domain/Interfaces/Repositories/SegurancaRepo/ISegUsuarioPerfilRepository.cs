using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo
{
    public interface ISegUsuarioPerfilRepository
    {
        Task<List<SegUsuarioPerfil>> GetByUsuarioAsync(long usuarioId);
        Task<SegUsuarioPerfil?> GetByUsuarioAndPerfilAsync(long usuarioId, long perfilId);
        Task AddAsync(SegUsuarioPerfil entity);
        Task DeleteAsync(long usuarioId, long perfilId);
    }
}
