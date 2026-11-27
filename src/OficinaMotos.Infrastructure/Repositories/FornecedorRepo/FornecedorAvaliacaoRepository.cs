using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorAvaliacaoRepository : Repository<FornecedorAvaliacao>, IFornecedorAvaliacaoRepository
    {
        public FornecedorAvaliacaoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
