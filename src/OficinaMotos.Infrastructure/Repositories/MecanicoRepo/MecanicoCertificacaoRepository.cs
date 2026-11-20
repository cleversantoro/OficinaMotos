using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoCertificacaoRepository : Repository<MecanicoCertificacao>, IMecanicoCertificacaoRepository
    {
        public MecanicoCertificacaoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
