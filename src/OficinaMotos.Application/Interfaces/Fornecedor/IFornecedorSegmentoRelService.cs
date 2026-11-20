using OficinaMotos.Application.DTOs.Requests.Fornecedor;
using OficinaMotos.Application.DTOs.Responses.Fornecedor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Fornecedor
{
    public interface IFornecedorSegmentoRelService
    {
        Task<List<FornecedorSegmentoRelResponseDTO>> GetAllAsync();
        Task<FornecedorSegmentoRelResponseDTO?> GetByKeysAsync(long fornecedorId, long segmentoId);
        Task<FornecedorSegmentoRelResponseDTO> CreateAsync(CreateFornecedorSegmentoRelDTO request);
        Task<bool> DeleteAsync(long fornecedorId, long segmentoId);
    }
}
