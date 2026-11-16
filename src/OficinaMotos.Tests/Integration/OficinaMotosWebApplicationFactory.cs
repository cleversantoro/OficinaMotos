using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OficinaMotos.Application.DTOs.Requests.Auth;
using OficinaMotos.Application.DTOs.Responses.Auth;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OficinaMotos.Tests.Integration
{
    public class OficinaMotosWebApplicationFactory : WebApplicationFactory<Program>
    {
        public const string AdminEmail = "admin@oficinamotos.com";
        public const string AdminLogin = "admin";
        public const string AdminPassword = "SenhaForte@123";
        public const string TestJwtKey = "ChaveSecretaIntegracaoOficinaMotos2026_Min32Bytes!";

        private readonly string _databaseName = $"OficinaMotosIntegrationDb_{Guid.NewGuid():N}";
        private readonly InMemoryDatabaseRoot _databaseRoot = new();
        private bool _seeded;
        private readonly object _seedLock = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.UseSetting("UseInMemoryDatabase", "true");
            builder.UseSetting("Jwt:Key", TestJwtKey);
            builder.UseSetting("OpenTelemetry:OtlpEndpoint", string.Empty);
            builder.UseSetting("OpenTelemetry:EnablePrometheus", "false");

            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["UseInMemoryDatabase"] = "true",
                    ["Jwt:Key"] = TestJwtKey,
                    ["OpenTelemetry:OtlpEndpoint"] = string.Empty,
                    ["OpenTelemetry:EnablePrometheus"] = "false",
                });
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<OficinaContext>));
                services.RemoveAll(typeof(OficinaContext));

                services.AddDbContext<OficinaContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName, _databaseRoot);
                });
            });
        }

        public new HttpClient CreateClient()
        {
            var client = base.CreateClient();
            EnsureSeeded();
            return client;
        }

        public async Task<HttpClient> CreateAuthenticatedClientAsync()
        {
            var client = CreateClient();

            var response = await client.PostAsJsonAsync("/api/v1/Auth/login", new LoginRequestDTO
            {
                Email = AdminEmail,
                Password = AdminPassword
            });

            response.EnsureSuccessStatusCode();
            var loginResult = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", loginResult!.Token);

            return client;
        }

        private void EnsureSeeded()
        {
            if (_seeded) return;

            lock (_seedLock)
            {
                if (_seeded) return;

                using var scope = Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<OficinaContext>();
                db.Database.EnsureCreated();

                if (!db.SegUsuarios.Any(u => u.Email == AdminEmail))
                {
                    var moduloClientes = new SegModulo
                    {
                        Nome = "Clientes",
                        Descricao = "Módulo de Clientes"
                    };
                    db.SegModulos.Add(moduloClientes);
                    db.SaveChanges();

                    var permissaoVisualizar = new SegPermissao
                    {
                        ModuloId = moduloClientes.Id,
                        Acao = "visualizar",
                        Descricao = "Visualizar clientes"
                    };
                    var permissaoEditar = new SegPermissao
                    {
                        ModuloId = moduloClientes.Id,
                        Acao = "editar",
                        Descricao = "Editar clientes"
                    };
                    db.SegPermissoes.AddRange(permissaoVisualizar, permissaoEditar);
                    db.SaveChanges();

                    var perfilAdmin = new SegPerfil
                    {
                        Nome = "Administrador",
                        Descricao = "Acesso administrativo total"
                    };
                    db.SegPerfis.Add(perfilAdmin);
                    db.SaveChanges();

                    db.SegPerfisPermissoes.AddRange(
                        new SegPerfilPermissao
                        {
                            PerfilId = perfilAdmin.Id,
                            PermissaoId = permissaoVisualizar.Id
                        },
                        new SegPerfilPermissao
                        {
                            PerfilId = perfilAdmin.Id,
                            PermissaoId = permissaoEditar.Id
                        });

                    var adminUser = new SegUsuario
                    {
                        Nome = "Administrador Integração",
                        Email = AdminEmail,
                        Login = AdminLogin,
                        Senha = BCrypt.Net.BCrypt.HashPassword(AdminPassword),
                        Status = 1,
                        TentativasLogin = 0,
                        BloqueadoAte = null
                    };
                    db.SegUsuarios.Add(adminUser);
                    db.SaveChanges();

                    db.SegUsuariosPerfis.Add(new SegUsuarioPerfil
                    {
                        UsuarioId = adminUser.Id,
                        PerfilId = perfilAdmin.Id,
                        Ativo = true
                    });

                    db.ClienteOrigens.Add(new ClienteOrigem
                    {
                        Nome = "Balcão",
                        Descricao = "Atendimento presencial"
                    });

                    db.SaveChanges();
                }

                _seeded = true;
            }
        }
    }
}
