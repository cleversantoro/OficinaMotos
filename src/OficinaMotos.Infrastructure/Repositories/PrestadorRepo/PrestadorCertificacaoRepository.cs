using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PrestadorRepo
{
    public class PrestadorCertificacaoRepository : Repository<PrestadorCertificacao>, IPrestadorCertificacaoRepository
    {
        public PrestadorCertificacaoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
