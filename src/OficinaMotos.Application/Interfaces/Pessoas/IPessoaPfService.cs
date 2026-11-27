using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Pessoas
{
    public interface IPessoaPfService
    {
        Task<List<PessoaPfResponseDTO>> GetAllAsync();
        Task<PessoaPfResponseDTO?> GetByPessoaIdAsync(long pessoaId);
        Task<PessoaPfResponseDTO> CreateAsync(CreatePessoaPfDTO request);
        Task<PessoaPfResponseDTO?> UpdateAsync(long pessoaId, UpdatePessoaPfDTO request);
        Task<bool> DeleteAsync(long pessoaId);
    }
}
