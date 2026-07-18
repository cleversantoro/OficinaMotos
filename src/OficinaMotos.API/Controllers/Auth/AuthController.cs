using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using OficinaMotos.Application.DTOs.Requests.Auth;
using OficinaMotos.Application.DTOs.Responses.Auth;
using OficinaMotos.Application.Interfaces.Seguranca;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            var jwtKey = _configuration["Jwt:Key"] ?? "chave_super_secreta_padrao_desenvolvimento_123";
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(8);

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

            return Ok(new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = userInfo.Email,
                Name = userInfo.Nome,
                Role = userInfo.Roles.FirstOrDefault() ?? string.Empty,
                ExpiresAt = expiration,
                UserId = userInfo.UserId,
                Login = userInfo.Login,
                Roles = userInfo.Roles,
                Permissions = userInfo.Permissions,
            });
        }
    }
}
