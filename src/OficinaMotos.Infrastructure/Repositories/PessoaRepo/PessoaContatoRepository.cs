using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    public class PessoaContatoRepository : Repository<PessoaContato>, IPessoaContatoRepository
    {
        public PessoaContatoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
