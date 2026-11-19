using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.DTOs.Responses.Mecanico;

namespace OficinaMotos.Application.Interfaces.Mecanico
{
    public interface IPrestadorService
    {
        Task<List<PrestadorResponseDTO>> GetAllAsync();
        Task<PrestadorResponseDTO?> GetByIdAsync(long id);
        Task<PrestadorResponseDTO> CreateAsync(CreatePrestadorDTO request);
        Task<PrestadorResponseDTO?> UpdateAsync(long id, UpdatePrestadorDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
