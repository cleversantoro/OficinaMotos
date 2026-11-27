using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.ClienteRepo
{
    public class ClienteFinanceiroRepository : Repository<ClienteFinanceiro>, IClienteFinanceiroRepository
    {
        public ClienteFinanceiroRepository(OficinaContext context) : base(context)
        {
        }
    }
}
