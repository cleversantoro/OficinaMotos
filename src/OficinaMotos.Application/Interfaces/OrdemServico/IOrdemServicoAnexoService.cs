using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoAnexoService
    {
        Task<List<OrdemServicoAnexoResponseDTO>> GetAllAsync();
        Task<OrdemServicoAnexoResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoAnexoResponseDTO> CreateAsync(CreateOrdemServicoAnexoDTO request);
        Task<OrdemServicoAnexoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoAnexoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
