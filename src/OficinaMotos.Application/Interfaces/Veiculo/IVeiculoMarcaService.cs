using OficinaMotos.Application.DTOs.Requests.Veiculo;
using OficinaMotos.Application.DTOs.Responses.Veiculo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Veiculo
{
    public interface IVeiculoMarcaService
    {
        Task<List<VeiculoMarcaResponseDTO>> GetAllAsync();
        Task<VeiculoMarcaResponseDTO?> GetByIdAsync(long id);
        Task<VeiculoMarcaResponseDTO> CreateAsync(CreateVeiculoMarcaDTO request);
        Task<VeiculoMarcaResponseDTO?> UpdateAsync(long id, UpdateVeiculoMarcaDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
