using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IOrdemServicoObservacaoService
    {
        Task<List<OrdemServicoObservacaoResponseDTO>> GetAllAsync();
        Task<OrdemServicoObservacaoResponseDTO?> GetByIdAsync(long id);
        Task<OrdemServicoObservacaoResponseDTO> CreateAsync(CreateOrdemServicoObservacaoDTO request);
        Task<OrdemServicoObservacaoResponseDTO?> UpdateAsync(long id, UpdateOrdemServicoObservacaoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
