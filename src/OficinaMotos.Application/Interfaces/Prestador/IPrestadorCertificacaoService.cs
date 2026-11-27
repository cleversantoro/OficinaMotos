using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Prestador
{
    public interface IPrestadorCertificacaoService
    {
        Task<List<PrestadorCertificacaoResponseDTO>> GetAllAsync();
        Task<PrestadorCertificacaoResponseDTO?> GetByIdAsync(long id);
        Task<PrestadorCertificacaoResponseDTO> CreateAsync(CreatePrestadorCertificacaoDTO request);
        Task<PrestadorCertificacaoResponseDTO?> UpdateAsync(long id, UpdatePrestadorCertificacaoDTO request);
        Task<bool> DeleteAsync(long id);
    }
}
