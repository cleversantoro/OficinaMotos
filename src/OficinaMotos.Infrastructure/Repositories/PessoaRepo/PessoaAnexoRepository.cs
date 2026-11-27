using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    public class PessoaAnexoRepository : Repository<PessoaAnexo>, IPessoaAnexoRepository
    {
        public PessoaAnexoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
