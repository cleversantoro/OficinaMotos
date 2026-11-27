using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Domain.Enums;
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
        Task<OrdemServicoResponseDTO?> ChangeStatusAsync(long id, OrdemServicoEnum novoStatus);
        decimal CalculateTotal(Domain.Entities.OrdemServico ordem);
        Task<decimal> CalculateTotalAsync(long id);
        Task<bool> DeleteAsync(long id);
        Task<OrdemServicoPagamentoResponseDTO> RegistrarPagamentoAsync(CreateOrdemServicoPagamentoDTO request);
    }
}
