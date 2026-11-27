using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PrestadorRepo
{
    public class PrestadorExperienciaRepository : Repository<PrestadorExperiencia>, IPrestadorExperienciaRepository
    {
        public PrestadorExperienciaRepository(OficinaContext context) : base(context)
        {
        }
    }
}
