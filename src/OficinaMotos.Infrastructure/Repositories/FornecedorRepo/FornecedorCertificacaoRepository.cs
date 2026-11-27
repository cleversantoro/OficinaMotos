using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorCertificacaoRepository : Repository<FornecedorCertificacao>, IFornecedorCertificacaoRepository
    {
        public FornecedorCertificacaoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
