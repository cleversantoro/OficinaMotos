using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.VeiculoRepo
{
    public class VeiculoModeloRepository : Repository<VeiculoModelo>, IVeiculoModeloRepository
    {
        public VeiculoModeloRepository(OficinaContext context) : base(context)
        {
        }
    }
}
