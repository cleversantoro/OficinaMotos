using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Domain.Interfaces.Repositories.Mecanico
{
    public interface IPrestadorRepository : IRepository<Prestador>
    {
        Task<Prestador?> GetByPessoaIdAsync(long pessoaId);
        Task<Prestador?> GetByCodigoAsync(string codigo);
    }

    public interface IAtividadeProfissionalRepository : IRepository<AtividadeProfissional>
    {
    }
}
