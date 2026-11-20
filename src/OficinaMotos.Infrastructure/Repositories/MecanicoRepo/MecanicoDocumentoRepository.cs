using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoDocumentoRepository : Repository<MecanicoDocumento>, IMecanicoDocumentoRepository
    {
        public MecanicoDocumentoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
