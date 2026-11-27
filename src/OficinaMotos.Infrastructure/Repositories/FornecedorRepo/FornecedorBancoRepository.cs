using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorBancoRepository : Repository<FornecedorBanco>, IFornecedorBancoRepository
    {
        public FornecedorBancoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
