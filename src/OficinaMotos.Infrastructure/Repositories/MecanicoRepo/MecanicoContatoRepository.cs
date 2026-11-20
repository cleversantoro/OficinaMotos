using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoContatoRepository : Repository<MecanicoContato>, IMecanicoContatoRepository
    {
        public MecanicoContatoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
