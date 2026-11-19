using FluentAssertions;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Integration
{
    public class ClienteControllerIntegrationTests
    {
        [Fact]
        public async Task GetAll_WithoutToken_ReturnsUnauthorized()
        {
            // Arrange (T-029.3)
            await using var factory = new OficinaMotosWebApplicationFactory();
            using var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/Clientes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Crud_WithAuthenticatedClient_Succeeds()
        {
            // Arrange (T-029.3)
            await using var factory = new OficinaMotosWebApplicationFactory();
            using var client = await factory.CreateAuthenticatedClientAsync();

            var createDto = new CreateClienteDTO
            {
                PessoaId = 1,
                Nome = "Carlos Eduardo Moto",
                Cpf = "52998224725",
                Email = "carlos.moto@oficina.com",
                Telefone = "11987654321"
            };

            // 1. POST /api/v1/Clientes (Create)
            var createResponse = await client.PostAsJsonAsync("/api/v1/Clientes", createDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await createResponse.Content.ReadFromJsonAsync<ClienteResponseDTO>();
            created.Should().NotBeNull();
            created!.Id.Should().BeGreaterThan(0);
            created.Nome.Should().Be("Carlos Eduardo Moto");
            created.Documento.Should().Be("52998224725");

            var clienteId = created.Id;

            // 2. GET /api/v1/Clientes/{id} (Read by ID)
            var getByIdResponse = await client.GetAsync($"/api/v1/Clientes/{clienteId}");
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var fetched = await getByIdResponse.Content.ReadFromJsonAsync<ClienteResponseDTO>();
            fetched.Should().NotBeNull();
            fetched!.Id.Should().Be(clienteId);
            fetched.Email.Should().Be("carlos.moto@oficina.com");

            // 3. GET /api/v1/Clientes (Read All)
            var getAllResponse = await client.GetAsync("/api/v1/Clientes");
            getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var allClientes = await getAllResponse.Content.ReadFromJsonAsync<List<ClienteResponseDTO>>();
            allClientes.Should().NotBeNull();
            allClientes!.Should().Contain(c => c.Id == clienteId && c.Documento == "52998224725");

            // 4. PUT /api/v1/Clientes/{id} (Update)
            var updateDto = new UpdateClienteDTO
            {
                PessoaId = 1,
                Nome = "Carlos Eduardo Moto Atualizado",
                NomeExibicao = "Carlos Moto VIP",
                Documento = "52998224725",
                Tipo = 1,
                Status = 1,
                Vip = true,
                Observacoes = "Cliente recorrente",
                OrigemCadastroId = 1,
                Telefone = "11999998888",
                Email = "carlos.vip@oficina.com",
                OrigemId = 1
            };

            var updateResponse = await client.PutAsJsonAsync($"/api/v1/Clientes/{clienteId}", updateDto);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updated = await updateResponse.Content.ReadFromJsonAsync<ClienteResponseDTO>();
            updated.Should().NotBeNull();
            updated!.Nome.Should().Be("Carlos Eduardo Moto Atualizado");
            updated.Vip.Should().BeTrue();
            updated.Email.Should().Be("carlos.vip@oficina.com");

            // 5. DELETE /api/v1/Clientes/{id} (Soft Delete)
            var deleteResponse = await client.DeleteAsync($"/api/v1/Clientes/{clienteId}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // 6. GET /api/v1/Clientes/{id} after delete -> 404 NotFound
            var getDeletedResponse = await client.GetAsync($"/api/v1/Clientes/{clienteId}");
            getDeletedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
