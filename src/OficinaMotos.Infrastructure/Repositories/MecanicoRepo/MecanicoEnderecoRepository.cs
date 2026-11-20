using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoEnderecoRepository : Repository<MecanicoEndereco>, IMecanicoEnderecoRepository
    {
        public MecanicoEnderecoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
