using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Prestador
{
    public interface IPrestadorDisponibilidadeService
    {
        Task<List<PrestadorDisponibilidadeResponseDTO>> GetAllAsync();
        Task<PrestadorDisponibilidadeResponseDTO?> GetByIdAsync(long id);
        Task<PrestadorDisponibilidadeResponseDTO> CreateAsync(CreatePrestadorDisponibilidadeDTO request);
        Task<PrestadorDisponibilidadeResponseDTO?> UpdateAsync(long id, UpdatePrestadorDisponibilidadeDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
