using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Pessoas
{
    public interface IPessoaAnexoService
    {
        Task<List<PessoaAnexoResponseDTO>> GetAllAsync();
        Task<PessoaAnexoResponseDTO?> GetByIdAsync(long id);
        Task<PessoaAnexoResponseDTO> CreateAsync(CreatePessoaAnexoDTO request);
        Task<PessoaAnexoResponseDTO?> UpdateAsync(long id, UpdatePessoaAnexoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
