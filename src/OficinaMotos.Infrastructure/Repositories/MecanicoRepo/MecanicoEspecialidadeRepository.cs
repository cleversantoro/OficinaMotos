using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoEspecialidadeRepository : Repository<MecanicoEspecialidade>, IMecanicoEspecialidadeRepository
    {
        public MecanicoEspecialidadeRepository(OficinaContext context) : base(context)
        {
        }
    }
}
