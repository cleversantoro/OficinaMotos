using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Pessoas
{
    public interface IPessoaContatoService
    {
        Task<List<PessoaContatoResponseDTO>> GetAllAsync();
        Task<PessoaContatoResponseDTO?> GetByIdAsync(long id);
        Task<PessoaContatoResponseDTO> CreateAsync(CreatePessoaContatoDTO request);
        Task<PessoaContatoResponseDTO?> UpdateAsync(long id, UpdatePessoaContatoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
