using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorRepresentanteRepository : Repository<FornecedorRepresentante>, IFornecedorRepresentanteRepository
    {
        public FornecedorRepresentanteRepository(OficinaContext context) : base(context)
        {
        }
    }
}
