using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Pessoas
{
    public interface IPessoaPjService
    {
        Task<List<PessoaPjResponseDTO>> GetAllAsync();
        Task<PessoaPjResponseDTO?> GetByPessoaIdAsync(long pessoaId);
        Task<PessoaPjResponseDTO> CreateAsync(CreatePessoaPjDTO request);
        Task<PessoaPjResponseDTO?> UpdateAsync(long pessoaId, UpdatePessoaPjDTO request);
        Task<bool> DeleteAsync(long pessoaId);
    }
}
