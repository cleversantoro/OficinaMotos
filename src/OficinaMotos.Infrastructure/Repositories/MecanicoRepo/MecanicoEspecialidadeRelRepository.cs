using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoEspecialidadeRelRepository : Repository<MecanicoEspecialidadeRel>, IMecanicoEspecialidadeRelRepository
    {
        public MecanicoEspecialidadeRelRepository(OficinaContext context) : base(context)
        {
        }
    }
}
