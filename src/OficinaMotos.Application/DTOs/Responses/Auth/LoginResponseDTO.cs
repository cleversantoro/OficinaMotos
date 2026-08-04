namespace OficinaMotos.Application.DTOs.Responses.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        /// <summary>Primeiro perfil do usuário (compatibilidade).</summary>
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }

        // Campos estendidos
        public long UserId { get; set; }
        public string Login { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}
