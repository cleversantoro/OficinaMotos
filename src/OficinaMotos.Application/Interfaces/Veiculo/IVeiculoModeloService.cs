using OficinaMotos.Application.DTOs.Requests.Veiculo;
using OficinaMotos.Application.DTOs.Responses.Veiculo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Veiculo
{
    public interface IVeiculoModeloService
    {
        Task<List<VeiculoModeloResponseDTO>> GetAllAsync();
        Task<VeiculoModeloResponseDTO?> GetByIdAsync(long id);
        Task<VeiculoModeloResponseDTO> CreateAsync(CreateVeiculoModeloDTO request);
        Task<VeiculoModeloResponseDTO?> UpdateAsync(long id, UpdateVeiculoModeloDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
