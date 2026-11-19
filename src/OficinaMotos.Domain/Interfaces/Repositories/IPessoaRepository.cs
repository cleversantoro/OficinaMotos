using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Domain.Interfaces.Repositories
{
    public interface IPessoaRepository : IRepository<Pessoa>
    {
        Task<Pessoa?> GetByDocumentoAsync(string documento);
        Task<long> GetNextCodigoAsync();
    }
}
