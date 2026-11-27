using OficinaMotos.Application.DTOs.Requests.Veiculo;
using OficinaMotos.Application.DTOs.Responses.Veiculo;
using OficinaMotos.Application.DTOs.Responses.Veiculo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Veiculo
{
    public interface IVeiculoService
    {
        Task<List<VeiculoResponseDTO>> GetAllAsync();
        Task<VeiculoResponseDTO?> GetByIdAsync(long id);
        Task<VeiculoResponseDTO> CreateAsync(CreateVeiculoDTO request);
        Task<VeiculoResponseDTO?> UpdateAsync(long id, UpdateVeiculoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
