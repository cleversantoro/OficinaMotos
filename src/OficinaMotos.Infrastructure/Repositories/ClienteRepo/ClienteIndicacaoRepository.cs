using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.ClienteRepo
{
    public class ClienteIndicacaoRepository : Repository<ClienteIndicacao>, IClienteIndicacaoRepository
    {
        public ClienteIndicacaoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
