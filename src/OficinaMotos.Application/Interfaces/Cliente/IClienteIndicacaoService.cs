using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Cliente
{
    public interface IClienteIndicacaoService
    {
        Task<List<ClienteIndicacaoResponseDTO>> GetAllAsync();
        Task<ClienteIndicacaoResponseDTO?> GetByIdAsync(long id);
        Task<ClienteIndicacaoResponseDTO> CreateAsync(CreateClienteIndicacaoDTO request);
        Task<ClienteIndicacaoResponseDTO?> UpdateAsync(long id, UpdateClienteIndicacaoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
