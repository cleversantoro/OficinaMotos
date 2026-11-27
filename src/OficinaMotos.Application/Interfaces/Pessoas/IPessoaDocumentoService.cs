using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Pessoas
{
    public interface IPessoaDocumentoService
    {
        Task<List<PessoaDocumentoResponseDTO>> GetAllAsync();
        Task<PessoaDocumentoResponseDTO?> GetByIdAsync(long id);
        Task<PessoaDocumentoResponseDTO> CreateAsync(CreatePessoaDocumentoDTO request);
        Task<PessoaDocumentoResponseDTO?> UpdateAsync(long id, UpdatePessoaDocumentoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
