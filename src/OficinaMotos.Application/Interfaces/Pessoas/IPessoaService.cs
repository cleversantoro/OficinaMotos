using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;

namespace OficinaMotos.Application.Interfaces.Pessoas
{
    public interface IPessoaService
    {
        Task<List<PessoaResponseDTO>> GetAllAsync();
        Task<PessoaResponseDTO?> GetByIdAsync(long id);
        Task<PessoaResponseDTO> CreateAsync(CreatePessoaDTO request);
        Task<PessoaResponseDTO?> UpdateAsync(long id, UpdatePessoaDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
