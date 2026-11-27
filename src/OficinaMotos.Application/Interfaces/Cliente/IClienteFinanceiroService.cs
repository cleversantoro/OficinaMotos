using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Cliente
{
    public interface IClienteFinanceiroService
    {
        Task<List<ClienteFinanceiroResponseDTO>> GetAllAsync();
        Task<ClienteFinanceiroResponseDTO?> GetByIdAsync(long id);
        Task<ClienteFinanceiroResponseDTO> CreateAsync(CreateClienteFinanceiroDTO request);
        Task<ClienteFinanceiroResponseDTO?> UpdateAsync(long id, UpdateClienteFinanceiroDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
