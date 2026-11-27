using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Prestador
{
    public interface IAtividadeProfissionalService
    {
        Task<List<AtividadeProfissionalResponseDTO>> GetAllAsync();
        Task<AtividadeProfissionalResponseDTO?> GetByIdAsync(long id);
        Task<AtividadeProfissionalResponseDTO> CreateAsync(CreateAtividadeProfissionalDTO request);
        Task<AtividadeProfissionalResponseDTO?> UpdateAsync(long id, UpdateAtividadeProfissionalDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
