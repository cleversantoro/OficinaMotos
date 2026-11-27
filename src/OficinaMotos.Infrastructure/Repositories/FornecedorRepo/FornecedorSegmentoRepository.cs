using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorSegmentoRepository : Repository<FornecedorSegmento>, IFornecedorSegmentoRepository
    {
        public FornecedorSegmentoRepository(OficinaContext context) : base(context)
        {
        }
    }
}
