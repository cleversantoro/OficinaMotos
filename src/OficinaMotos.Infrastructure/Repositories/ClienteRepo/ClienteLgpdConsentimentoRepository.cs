using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.ClienteRepo
{
    public class ClienteLgpdConsentimentoRepository : Repository<ClienteLgpdConsentimento>, IClienteLgpdConsentimentoRepository
    {
        public ClienteLgpdConsentimentoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
