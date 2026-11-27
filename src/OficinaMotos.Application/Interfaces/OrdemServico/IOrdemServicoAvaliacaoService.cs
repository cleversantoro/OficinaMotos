using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoAvaliacaoService
    {
        Task<List<OrdemServicoAvaliacaoResponseDTO>> GetAllAsync();
        Task<OrdemServicoAvaliacaoResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoAvaliacaoResponseDTO> CreateAsync(CreateOrdemServicoAvaliacaoDTO request);
        Task<OrdemServicoAvaliacaoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoAvaliacaoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
