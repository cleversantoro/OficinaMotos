using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoService
    {
        Task<List<OrdemServicoResponseDTO>> GetAllAsync();
        Task<OrdemServicoResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoResponseDTO> CreateAsync(CreateOrdemServicoDTO request);
        Task<OrdemServicoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoDTO request);
        Task<bool> DeleteAsync(long id);
        Task<OrdemServicoPagamentoResponseDTO> RegistrarPagamentoAsync(CreateOrdemServicoPagamentoDTO request);
    }
}
