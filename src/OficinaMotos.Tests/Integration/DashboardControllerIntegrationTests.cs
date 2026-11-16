using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OficinaMotos.Application.DTOs.Responses.Dashboard;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Integration
{
    public class DashboardControllerIntegrationTests : IClassFixture<OficinaMotosWebApplicationFactory>
    {
        private readonly OficinaMotosWebApplicationFactory _factory;

        public DashboardControllerIntegrationTests(OficinaMotosWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetKpis_SemToken_DeveRetornar401Unauthorized()
        {
            // Arrange
            using var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/Dashboard/kpis");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetKpis_Autenticado_DeveRetornar200OkComKpisEPecasCriticas()
        {
            // Arrange
            using var client = await _factory.CreateAuthenticatedClientAsync();
            var hoje = DateTime.UtcNow;

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OficinaContext>();

                var cliente = new Cliente
                {
                    Codigo = 9901,
                    Tipo = ClienteTipo.PessoaFisica,
                    Nome = "Cliente Dashboard Teste",
                    NomeExibicao = "Cliente Dashboard Teste",
                    Documento = "12345678901",
                    Email = "dashboard.cliente@oficinamotos.com",
                    Telefone = "11999990000",
                    OrigemId = 1
                };
                var mecanico = new Mecanico
                {
                    Codigo = "MEC-DASH-01",
                    Nome = "Mecânico Dashboard",
                    DocumentoPrincipal = "12345678901",
                    Status = "Ativo"
                };
                var veiculo = new Veiculo
                {
                    Cliente = cliente,
                    Placa = "DSH1A23",
                    AnoFab = 2024,
                    AnoMod = 2024,
                    Km = "5000"
                };

                db.Clientes.Add(cliente);
                db.Mecanicos.Add(mecanico);
                db.Veiculos.Add(veiculo);

                db.EstoquePecas.AddRange(
                    new EstoquePeca
                    {
                        Codigo = "CRIT-001",
                        Descricao = "Corrente de Transmissão",
                        Quantidade = 1,
                        EstoqueMinimo = 4,
                        EstoqueMaximo = 20,
                        PrecoUnitario = 220m,
                        Unidade = "UN"
                    },
                    new EstoquePeca
                    {
                        Codigo = "OK-002",
                        Descricao = "Vela de Ignição",
                        Quantidade = 10,
                        EstoqueMinimo = 3,
                        EstoqueMaximo = 30,
                        PrecoUnitario = 45m,
                        Unidade = "UN"
                    });

                db.OrdensServico.AddRange(
                    new OrdemServico
                    {
                        Cliente = cliente,
                        Mecanico = mecanico,
                        Veiculo = veiculo,
                        DescricaoProblema = "Troca de relação",
                        Status = OrdemServicoStatus.Aberta,
                        DataAbertura = hoje
                    },
                    new OrdemServico
                    {
                        Cliente = cliente,
                        Mecanico = mecanico,
                        Veiculo = veiculo,
                        DescricaoProblema = "Revisão completa",
                        Status = OrdemServicoStatus.Concluida,
                        DataAbertura = hoje,
                        DataConclusao = hoje,
                        Itens = new List<OrdemServicoItem>
                        {
                            new OrdemServicoItem
                            {
                                Descricao = "Revisão geral",
                                Quantidade = 1,
                                ValorUnitario = 450m,
                                Total = 450m
                            }
                        }
                    });

                await db.SaveChangesAsync();
            }

            // Act
            var response = await client.GetAsync($"/api/v1/Dashboard/kpis?periodo={hoje:yyyy-MM}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var payload = await response.Content.ReadFromJsonAsync<DashboardKpisResponseDTO>();
            payload.Should().NotBeNull();
            payload!.OsAbertas.Should().BeGreaterThanOrEqualTo(1);
            payload.OsDoDia.Should().BeGreaterThanOrEqualTo(2);
            payload.FaturamentoMensal.Should().BeGreaterThanOrEqualTo(450m);
            payload.FaturamentoUltimosSeisMeses.Should().HaveCount(6);
            payload.OsPorStatus.Should().NotBeEmpty();
            payload.PecasAbaixoMinimo.Should().BeGreaterThanOrEqualTo(1);
            payload.PecasCriticas.Should().Contain(p => p.Codigo == "CRIT-001" && p.QuantidadeAtual == 1 && p.EstoqueMinimo == 4);
        }
    }
}
