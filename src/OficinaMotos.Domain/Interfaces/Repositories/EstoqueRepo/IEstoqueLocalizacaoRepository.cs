using OficinaMotos.Domain.Entities;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories.EstoqueRepo
{
    public interface IEstoqueLocalizacaoRepository : IRepository<EstoqueLocalizacao>
    {
        Task<bool> ExistsByDescricaoAsync(string descricao);
    }
}
