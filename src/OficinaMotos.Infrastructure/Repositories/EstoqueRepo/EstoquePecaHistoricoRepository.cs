using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.EstoqueRepo
{
    public class EstoquePecaHistoricoRepository : Repository<EstoquePecaHistorico>, IEstoquePecaHistoricoRepository
    {
        public EstoquePecaHistoricoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
