using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoHistoricoService
    {
        Task<List<OrdemServicoHistoricoResponseDTO>> GetAllAsync();
        Task<OrdemServicoHistoricoResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoHistoricoResponseDTO> CreateAsync(CreateOrdemServicoHistoricoDTO request);
        Task<OrdemServicoHistoricoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoHistoricoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
