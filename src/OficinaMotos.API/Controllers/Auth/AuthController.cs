using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OficinaMotos.Application.DTOs.Requests.Auth;
using OficinaMotos.Application.DTOs.Responses.Auth;
using OficinaMotos.Application.Interfaces.Seguranca;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace OficinaMotos.API.Controllers.Auth
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// Autentica o usuário e retorna um token JWT.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Login e senha são obrigatórios." });

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();

            var userInfo = await _authService.LoginAsync(request, ip, userAgent);
            if (userInfo == null)
                return Unauthorized(new { message = "Credenciais inválidas ou usuário bloqueado." });

            var expiration = DateTime.UtcNow.AddHours(8);
            var token = CreateJwtToken(userInfo, expiration);

            return Ok(new LoginResponseDTO
            {
                Token = token,
                Email = userInfo.Email,
                Name = userInfo.Nome,
                Role = userInfo.Roles.FirstOrDefault() ?? string.Empty,
                ExpiresAt = expiration,
                UserId = userInfo.UserId,
                Login = userInfo.Login,
                Roles = userInfo.Roles,
                Permissions = userInfo.Permissions,
                RefreshToken = userInfo.RefreshToken,
                RefreshTokenExpiresAt = userInfo.RefreshTokenExpiresAt,
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(RefreshTokenResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();
            var response = await _authService.RefreshAsync(request, ip, userAgent);
            if (response == null)
                return Unauthorized(new { message = "Refresh token inválido ou expirado." });

            var expiration = DateTime.UtcNow.AddHours(8);
            var token = CreateJwtToken(new LoginDataResult
            {
                UserId = response.UserId,
                Nome = response.Name,
                Email = response.Email,
                Login = response.Login,
                Roles = response.Roles,
                Permissions = response.Permissions,
            }, expiration);

            response.Token = token;
            response.ExpiresAt = expiration;
            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO request)
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!long.TryParse(userIdClaim, out var userId))
                return Unauthorized(new { message = "Usuário não autenticado." });

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers.UserAgent.ToString();
            var removed = await _authService.LogoutAsync(request, userId, ip, userAgent);
            return removed ? NoContent() : Unauthorized(new { message = "Não foi possível encerrar a sessão." });
        }

        private string CreateJwtToken(LoginDataResult userInfo, DateTime expiration)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "chave_super_secreta_padrao_desenvolvimento_123";
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userInfo.UserId.ToString()),
                new(JwtRegisteredClaimNames.Email, userInfo.Email),
                new(JwtRegisteredClaimNames.UniqueName, userInfo.Login),
                new(ClaimTypes.Name, userInfo.Nome),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in userInfo.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            foreach (var permissao in userInfo.Permissions)
                claims.Add(new Claim("permissao", permissao));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
