using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Cliente
{
    public interface IClienteOrigemService
    {
        Task<List<ClienteOrigemResponseDTO>> GetAllAsync();
        Task<ClienteOrigemResponseDTO?> GetByIdAsync(long id);
        Task<ClienteOrigemResponseDTO> CreateAsync(CreateClienteOrigemDTO request);
        Task<ClienteOrigemResponseDTO?> UpdateAsync(long id, UpdateClienteOrigemDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
