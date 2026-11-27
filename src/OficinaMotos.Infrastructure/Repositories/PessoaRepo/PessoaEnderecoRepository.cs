using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    public class PessoaEnderecoRepository : Repository<PessoaEndereco>, IPessoaEnderecoRepository
    {
        public PessoaEnderecoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
