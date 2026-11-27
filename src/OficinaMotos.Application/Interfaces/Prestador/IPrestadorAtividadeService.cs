using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Prestador
{
    public interface IPrestadorAtividadeService
    {
        Task<List<PrestadorAtividadeResponseDTO>> GetAllAsync();
        Task<PrestadorAtividadeResponseDTO?> GetByIdAsync(long prestadorId, long atividadeProfissionalId);
        Task<PrestadorAtividadeResponseDTO> CreateAsync(CreatePrestadorAtividadeDTO request);
        Task<PrestadorAtividadeResponseDTO?> UpdateAsync(long prestadorId, long atividadeProfissionalId, UpdatePrestadorAtividadeDTO request);
        Task<bool> DeleteAsync(long prestadorId, long atividadeProfissionalId);
    }
}
