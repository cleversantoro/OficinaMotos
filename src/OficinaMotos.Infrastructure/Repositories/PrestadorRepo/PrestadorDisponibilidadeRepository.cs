using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PrestadorRepo
{
    public class PrestadorDisponibilidadeRepository : Repository<PrestadorDisponibilidade>, IPrestadorDisponibilidadeRepository
    {
        public PrestadorDisponibilidadeRepository(OficinaContext context) : base(context)
        {
        }
    }
}
