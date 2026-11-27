using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    public class PessoaDocumentoRepository : Repository<PessoaDocumento>, IPessoaDocumentoRepository
    {
        public PessoaDocumentoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
