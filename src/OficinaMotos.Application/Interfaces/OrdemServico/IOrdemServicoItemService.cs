using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoItemService
    {
        Task<List<OrdemServicoItemResponseDTO>> GetAllAsync();
        Task<OrdemServicoItemResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoItemResponseDTO> CreateAsync(CreateOrdemServicoItemDTO request);
        Task<OrdemServicoItemResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoItemDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
