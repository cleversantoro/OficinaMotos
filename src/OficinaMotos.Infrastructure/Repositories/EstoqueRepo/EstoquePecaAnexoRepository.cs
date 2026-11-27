using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.EstoqueRepo
{
    public class EstoquePecaAnexoRepository : Repository<EstoquePecaAnexo>, IEstoquePecaAnexoRepository
    {
        public EstoquePecaAnexoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
