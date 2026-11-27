using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoPagamentoService
    {
        Task<List<OrdemServicoPagamentoResponseDTO>> GetAllAsync();
        Task<OrdemServicoPagamentoResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoPagamentoResponseDTO> CreateAsync(CreateOrdemServicoPagamentoDTO request);
        Task<OrdemServicoPagamentoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoPagamentoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
