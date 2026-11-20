using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoDisponibilidadeRepository : Repository<MecanicoDisponibilidade>, IMecanicoDisponibilidadeRepository
    {
        public MecanicoDisponibilidadeRepository(OficinaContext context) : base(context)
        {
        }
    }
}
