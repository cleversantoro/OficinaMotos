using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.VeiculoRepo
{
    public class VeiculoMarcaRepository : Repository<VeiculoMarca>, IVeiculoMarcaRepository
    {
        public VeiculoMarcaRepository(OficinaContext context) : base(context)
        {
        }
    }
}
