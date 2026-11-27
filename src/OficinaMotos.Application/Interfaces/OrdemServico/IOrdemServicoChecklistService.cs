using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoChecklistService
    {
        Task<List<OrdemServicoChecklistResponseDTO>> GetAllAsync();
        Task<OrdemServicoChecklistResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoChecklistResponseDTO> CreateAsync(CreateOrdemServicoChecklistDTO request);
        Task<OrdemServicoChecklistResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoChecklistDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
