using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Prestador
{
    public interface IPrestadorExperienciaService
    {
        Task<List<PrestadorExperienciaResponseDTO>> GetAllAsync();
        Task<PrestadorExperienciaResponseDTO?> GetByIdAsync(long id);
        Task<PrestadorExperienciaResponseDTO> CreateAsync(CreatePrestadorExperienciaDTO request);
        Task<PrestadorExperienciaResponseDTO?> UpdateAsync(long id, UpdatePrestadorExperienciaDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
