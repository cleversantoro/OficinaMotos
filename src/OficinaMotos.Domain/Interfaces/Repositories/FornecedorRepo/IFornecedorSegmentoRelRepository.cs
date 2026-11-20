using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Domain.Interfaces.Repositories.Fornecedor
{
    public interface IFornecedorSegmentoRelRepository
    {
        Task<List<FornecedorSegmentoRel>> GetAllAsync();
        Task<FornecedorSegmentoRel?> GetByKeysAsync(long fornecedorId, long segmentoId);
        Task<FornecedorSegmentoRel> AddAsync(FornecedorSegmentoRel entity);
        Task DeleteAsync(long fornecedorId, long segmentoId);
        Task<bool> ExistsAsync(long fornecedorId, long segmentoId);
    }
}
