using FluentAssertions;
using OficinaMotos.Application.DTOs.Requests.Auth;
using OficinaMotos.Application.DTOs.Responses.Auth;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Integration
{
    public class AuthControllerIntegrationTests
    {
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkAndJwtToken()
        {
            // Arrange (T-029.2)
            await using var factory = new OficinaMotosWebApplicationFactory();
            using var client = factory.CreateClient();

            var request = new LoginRequestDTO
            {
                Email = OficinaMotosWebApplicationFactory.AdminEmail,
                Password = OficinaMotosWebApplicationFactory.AdminPassword
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/Auth/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            payload.Should().NotBeNull();
            payload!.Token.Should().NotBeNullOrWhiteSpace();
            payload.Token.Split('.').Should().HaveCount(3);
            payload.RefreshToken.Should().NotBeNullOrWhiteSpace();
            payload.Email.Should().Be(OficinaMotosWebApplicationFactory.AdminEmail);
            payload.Roles.Should().Contain("Administrador");
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange (T-029.2)
            await using var factory = new OficinaMotosWebApplicationFactory();
            using var client = factory.CreateClient();

            var request = new LoginRequestDTO
            {
                Email = OficinaMotosWebApplicationFactory.AdminEmail,
                Password = "SenhaIncorreta@999"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/Auth/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
        {
            // Arrange (T-029.2)
            await using var factory = new OficinaMotosWebApplicationFactory();
            using var client = factory.CreateClient();

            var request = new LoginRequestDTO
            {
                Email = "inexistente@oficinamotos.com",
                Password = "QualquerSenha@123"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/Auth/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
