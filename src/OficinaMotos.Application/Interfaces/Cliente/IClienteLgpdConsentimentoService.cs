using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Cliente
{
    public interface IClienteLgpdConsentimentoService
    {
        Task<List<ClienteLgpdConsentimentoResponseDTO>> GetAllAsync();
        Task<ClienteLgpdConsentimentoResponseDTO?> GetByIdAsync(long id);
        Task<ClienteLgpdConsentimentoResponseDTO> CreateAsync(CreateClienteLgpdConsentimentoDTO request);
        Task<ClienteLgpdConsentimentoResponseDTO?> UpdateAsync(long id, UpdateClienteLgpdConsentimentoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
