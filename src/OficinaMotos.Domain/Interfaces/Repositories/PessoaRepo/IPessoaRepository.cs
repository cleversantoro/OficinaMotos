using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo
{
    public interface IPessoaRepository : IRepository<Pessoa>
    {
        Task<Pessoa?> GetByDocumentoAsync(string documento);
        Task<long> GetNextCodigoAsync();
    }
}
