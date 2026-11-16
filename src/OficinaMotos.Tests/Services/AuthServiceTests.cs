using AutoMapper;
using FluentAssertions;
using Moq;
using OficinaMotos.Application.DTOs.Requests.Auth;
using OficinaMotos.Application.DTOs.Responses.Auth;
using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Application.Services.Seguranca;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;
using OficinaMotos.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<ISegUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ISegAuditLogRepository> _auditLogRepositoryMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            _usuarioRepositoryMock = new Mock<ISegUsuarioRepository>();
            _auditLogRepositoryMock = new Mock<ISegAuditLogRepository>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _mapperMock = new Mock<IMapper>();

            _auditLogRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<SegAuditLog>()))
                .ReturnsAsync((SegAuditLog log) => log);

            _mapperMock
                .Setup(m => m.Map<SegAuditLogResponseDTO>(It.IsAny<SegAuditLog>()))
                .Returns(new SegAuditLogResponseDTO());

            _refreshTokenRepositoryMock
                .Setup(r => r.GetActiveByUserIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new List<RefreshToken>());

            _refreshTokenRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<RefreshToken>()))
                .ReturnsAsync((RefreshToken rt) => rt);

            _sut = new AuthService(
                _usuarioRepositoryMock.Object,
                _auditLogRepositoryMock.Object,
                _refreshTokenRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsJwtToken()
        {
            // Arrange (T-026.1)
            const string plainPassword = "SenhaForte@123";
            var moduloFinanceiro = new SegModulo { Nome = "Financeiro" }.WithId(1);
            var permissaoVisualizar = new SegPermissao
            {
                ModuloId = 1,
                Modulo = moduloFinanceiro,
                Acao = "visualizar"
            }.WithId(10);
            var perfilAdmin = new SegPerfil
            {
                Nome = "Administrador",
                PerfisPermissoes = new List<SegPerfilPermissao>
                {
                    new SegPerfilPermissao { PerfilId = 1, PermissaoId = 10, Permissao = permissaoVisualizar }
                }
            }.WithId(1);

            var usuario = new SegUsuario
            {
                Nome = "Administrador MotoPro",
                Email = "admin@oficinamotos.com",
                Login = "admin",
                Senha = BCrypt.Net.BCrypt.HashPassword(plainPassword),
                Status = 1,
                TentativasLogin = 0,
                BloqueadoAte = null,
                UsuariosPerfis = new List<SegUsuarioPerfil>
                {
                    new SegUsuarioPerfil
                    {
                        UsuarioId = 42,
                        PerfilId = 1,
                        Ativo = true,
                        Perfil = perfilAdmin
                    }
                }
            }.WithId(42);

            _usuarioRepositoryMock
                .Setup(r => r.GetByEmailOrLoginAsync("admin@oficinamotos.com"))
                .ReturnsAsync(usuario);

            _usuarioRepositoryMock
                .Setup(r => r.GetWithPerfisAsync(42))
                .ReturnsAsync(usuario);

            var request = new LoginRequestDTO
            {
                Email = "admin@oficinamotos.com",
                Password = plainPassword
            };

            // Act
            var result = await _sut.LoginAsync(request, "127.0.0.1", "xUnit-TestAgent");

            // Assert
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNullOrWhiteSpace();
            result.Token.Split('.').Should().HaveCount(3);
            result.UserId.Should().Be(42);
            result.Email.Should().Be("admin@oficinamotos.com");
            result.Login.Should().Be("admin");
            result.Roles.Should().Contain("Administrador");
            result.Permissions.Should().Contain("Financeiro:visualizar");
            result.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _usuarioRepositoryMock.Verify(r => r.UpdateAsync(It.Is<SegUsuario>(u => u.Id == 42 && u.TentativasLogin == 0)), Times.Once);
            _auditLogRepositoryMock.Verify(r => r.AddAsync(It.Is<SegAuditLog>(l => l.Acao == "LOGIN" && l.UsuarioId == 42)), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsNull()
        {
            // Arrange (T-026.2)
            var usuario = new SegUsuario
            {
                Nome = "Operador Teste",
                Email = "operador@oficinamotos.com",
                Login = "operador",
                Senha = BCrypt.Net.BCrypt.HashPassword("SenhaCorreta@2026"),
                Status = 1,
                TentativasLogin = 0,
                BloqueadoAte = null
            }.WithId(15);

            _usuarioRepositoryMock
                .Setup(r => r.GetByEmailOrLoginAsync("operador@oficinamotos.com"))
                .ReturnsAsync(usuario);

            var request = new LoginRequestDTO
            {
                Email = "operador@oficinamotos.com",
                Password = "SenhaErrada@999"
            };

            // Act
            var result = await _sut.LoginAsync(request, "127.0.0.1", "xUnit-TestAgent");

            // Assert
            result.Should().BeNull();
            usuario.TentativasLogin.Should().Be(1);
            _usuarioRepositoryMock.Verify(r => r.UpdateAsync(It.Is<SegUsuario>(u => u.Id == 15 && u.TentativasLogin == 1)), Times.Once);
            _auditLogRepositoryMock.Verify(r => r.AddAsync(It.Is<SegAuditLog>(l => l.Acao == "LOGIN_FAIL" && l.UsuarioId == 15)), Times.Once);
        }

        [Fact]
        public async Task Login_BlockedAccount_ThrowsException()
        {
            // Arrange (T-026.3)
            var usuarioBloqueado = new SegUsuario
            {
                Nome = "Usuário Bloqueado",
                Email = "bloqueado@oficinamotos.com",
                Login = "bloqueado",
                Senha = BCrypt.Net.BCrypt.HashPassword("QualquerSenha@123"),
                Status = 1,
                TentativasLogin = 5,
                BloqueadoAte = DateTime.UtcNow.AddMinutes(25)
            }.WithId(99);

            _usuarioRepositoryMock
                .Setup(r => r.GetByEmailOrLoginAsync("bloqueado@oficinamotos.com"))
                .ReturnsAsync(usuarioBloqueado);

            var request = new LoginRequestDTO
            {
                Email = "bloqueado@oficinamotos.com",
                Password = "QualquerSenha@123"
            };

            // Act
            Func<Task> act = async () => await _sut.LoginAsync(request, "127.0.0.1", "xUnit-TestAgent");

            // Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*bloqueada*");

            _auditLogRepositoryMock.Verify(
                r => r.AddAsync(It.Is<SegAuditLog>(l => l.Acao == "LOGIN_FAIL" && l.UsuarioId == 99)),
                Times.Once);
        }

        [Fact]
        public void GenerateToken_ContainsExpectedClaims()
        {
            // Arrange (T-026.4)
            var userInfo = new LoginDataResult
            {
                UserId = 77,
                Nome = "Gestor Financeiro",
                Email = "financeiro@oficinamotos.com",
                Login = "gestor.fin",
                Roles = new List<string> { "Gerente", "Financeiro" },
                Permissions = new List<string> { "Financeiro:visualizar", "Financeiro:editar" }
            };
            var expiration = DateTime.UtcNow.AddHours(8);

            // Act
            var tokenString = _sut.GenerateToken(userInfo, expiration);

            // Assert
            tokenString.Should().NotBeNullOrWhiteSpace();

            var handler = new JwtSecurityTokenHandler();
            handler.CanReadToken(tokenString).Should().BeTrue();

            var jwt = handler.ReadJwtToken(tokenString);
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == "77");
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == "financeiro@oficinamotos.com");
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.UniqueName && c.Value == "gestor.fin");
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "Gestor Financeiro");
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Jti && !string.IsNullOrWhiteSpace(c.Value));

            var roles = jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            roles.Should().BeEquivalentTo(new[] { "Gerente", "Financeiro" });

            var permissions = jwt.Claims.Where(c => c.Type == "permissao").Select(c => c.Value).ToList();
            permissions.Should().BeEquivalentTo(new[] { "Financeiro:visualizar", "Financeiro:editar" });
        }

        [Fact]
        public async Task TestDbContextFactory_Create_ShouldReturnIsolatedInMemoryContext()
        {
            // Arrange & Act (T-025.4)
            await using var context1 = TestDbContextFactory.Create();
            await using var context2 = TestDbContextFactory.Create();

            context1.ClienteOrigens.Add(new ClienteOrigem { Nome = "Indicação" });
            await context1.SaveChangesAsync();

            // Assert
            context1.ClienteOrigens.Count().Should().Be(1);
            context2.ClienteOrigens.Count().Should().Be(0);
        }
    }
}
