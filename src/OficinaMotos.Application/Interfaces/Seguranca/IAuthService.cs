using OficinaMotos.Application.DTOs.Requests.Auth;
using OficinaMotos.Application.DTOs.Responses.Auth;
using OficinaMotos.Application.DTOs.Responses.Seguranca;

namespace OficinaMotos.Application.Interfaces.Seguranca
{
    public interface IAuthService
    {
        Task<LoginDataResult?> LoginAsync(LoginRequestDTO request, string? ipAddress, string? userAgent);
        Task<RefreshTokenResponseDTO?> RefreshAsync(RefreshTokenRequestDTO request, string? ipAddress, string? userAgent);
        Task<bool> LogoutAsync(LogoutRequestDTO request, long userId, string? ipAddress, string? userAgent);
        Task<SegAuditLogResponseDTO> RegistrarAuditAsync(
            long? usuarioId, string? login, string acao,
            string? modulo = null, string? tabela = null, string? registroId = null,
            string? descricao = null, string? dadosAntes = null, string? dadosDepois = null,
            string? ip = null, string? userAgent = null);
    }
}
