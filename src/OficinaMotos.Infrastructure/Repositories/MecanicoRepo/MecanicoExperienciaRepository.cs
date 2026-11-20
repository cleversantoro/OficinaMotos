using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoExperienciaRepository : Repository<MecanicoExperiencia>, IMecanicoExperienciaRepository
    {
        public MecanicoExperienciaRepository(OficinaContext context) : base(context)
        {
        }
    }
}
