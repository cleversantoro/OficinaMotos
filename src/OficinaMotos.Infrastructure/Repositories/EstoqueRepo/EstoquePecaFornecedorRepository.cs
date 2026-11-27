using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.EstoqueRepo
{
    public class EstoquePecaFornecedorRepository : Repository<EstoquePecaFornecedor>, IEstoquePecaFornecedorRepository
    {
        public EstoquePecaFornecedorRepository(OficinaContext context) : base(context)
        {
        }
    }
}
