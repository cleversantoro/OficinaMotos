using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Pessoas
{
    public interface IPessoaEnderecoService
    {
        Task<List<PessoaEnderecoResponseDTO>> GetAllAsync();
        Task<PessoaEnderecoResponseDTO?> GetByIdAsync(long id);
        Task<PessoaEnderecoResponseDTO> CreateAsync(CreatePessoaEnderecoDTO request);
        Task<PessoaEnderecoResponseDTO?> UpdateAsync(long id, UpdatePessoaEnderecoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
