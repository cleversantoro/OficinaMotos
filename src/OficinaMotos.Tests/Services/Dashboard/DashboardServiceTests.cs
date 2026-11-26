using FluentAssertions;
using Moq;
using OficinaMotos.Application.Services.Dashboard;
using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Domain.Interfaces.Repositories.Estoque;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Services.Dashboard
{
    public class DashboardServiceTests
    {
        private readonly Mock<IOrdemServicoRepository> _ordemServicoRepoMock = new();
        private readonly Mock<IEstoquePecaRepository> _estoquePecaRepoMock = new();
        private readonly DashboardService _sut;

        public DashboardServiceTests()
        {
            _sut = new DashboardService(_ordemServicoRepoMock.Object, _estoquePecaRepoMock.Object);
        }

        private static T WithId<T>(T entity, long id) where T : BaseEntity
        {
            typeof(BaseEntity)
                .GetProperty(nameof(BaseEntity.Id), BindingFlags.Public | BindingFlags.Instance)!
                .SetValue(entity, id);
            return entity;
        }

        [Fact]
        public async Task GetKpisAsync_DeveCalcularOsAbertasDoDiaFaturamentoMensalSeisMesesEPecasCriticas()
        {
            // Arrange
            var hoje = DateTime.UtcNow;
            var periodoAtual = $"{hoje:yyyy-MM}";
            var mesAnterior = hoje.AddMonths(-1);

            var ordens = new List<OrdemServico>
            {
                WithId(new OrdemServico
                {
                    Status = OrdemServicoEnum.Aberta,
                    DataAbertura = hoje
                }, 1),
                WithId(new OrdemServico
                {
                    Status = OrdemServicoEnum.EmAndamento,
                    DataAbertura = hoje.AddDays(-2)
                }, 2),
                WithId(new OrdemServico
                {
                    Status = OrdemServicoEnum.AguardandoPeca,
                    DataAbertura = hoje
                }, 3),
                WithId(new OrdemServico
                {
                    Status = OrdemServicoEnum.Concluida,
                    DataAbertura = hoje.AddDays(-3),
                    DataConclusao = hoje,
                    Itens = new List<OrdemServicoItem>
                    {
                        new OrdemServicoItem { Quantidade = 2, ValorUnitario = 150m, Total = 300m },
                        new OrdemServicoItem { Quantidade = 1, ValorUnitario = 200m, Total = 200m }
                    }
                }, 4),
                WithId(new OrdemServico
                {
                    Status = OrdemServicoEnum.Concluida,
                    DataAbertura = mesAnterior,
                    DataConclusao = mesAnterior,
                    Itens = new List<OrdemServicoItem>
                    {
                        new OrdemServicoItem { Quantidade = 1, ValorUnitario = 800m, Total = 800m }
                    }
                }, 5),
                WithId(new OrdemServico
                {
                    Status = OrdemServicoEnum.Cancelada,
                    DataAbertura = hoje.AddDays(-5)
                }, 6)
            };

            var pecas = new List<EstoquePeca>
            {
                WithId(new EstoquePeca
                {
                    Codigo = "PAST-01",
                    Descricao = "Pastilha de Freio",
                    Quantidade = 2,
                    EstoqueMinimo = 5,
                    Unidade = "UN"
                }, 10),
                WithId(new EstoquePeca
                {
                    Codigo = "OLEO-10W30",
                    Descricao = "Óleo Motor 10W30",
                    Quantidade = 1,
                    EstoqueMinimo = 10,
                    Unidade = "LT"
                }, 11),
                WithId(new EstoquePeca
                {
                    Codigo = "FILTRO-AR",
                    Descricao = "Filtro de Ar",
                    Quantidade = 8,
                    EstoqueMinimo = 5,
                    Unidade = "UN"
                }, 12)
            };

            _ordemServicoRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(ordens);
            _estoquePecaRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(pecas);

            // Act
            var result = await _sut.GetKpisAsync(periodoAtual);

            // Assert
            result.Should().NotBeNull();
            result.Periodo.Should().Be(periodoAtual);

            // T-032.3: OS abertas (Aberta, EmAndamento, AguardandoPeca) e OS do dia (IDs 1 e 3)
            result.OsAbertas.Should().Be(3);
            result.OsDoDia.Should().Be(2);
            result.OsPorStatus.Should().HaveCount(5);
            result.OsPorStatus.Should().Contain(s => s.Status == "Aberta" && s.Quantidade == 1);
            result.OsPorStatus.Should().Contain(s => s.Status == "EmAndamento" && s.Quantidade == 1);
            result.OsPorStatus.Should().Contain(s => s.Status == "AguardandoPeca" && s.Quantidade == 1);
            result.OsPorStatus.Should().Contain(s => s.Status == "Concluida" && s.Quantidade == 2);
            result.OsPorStatus.Should().Contain(s => s.Status == "Cancelada" && s.Quantidade == 1);

            // T-032.4: Faturamento mensal e série dos últimos 6 meses
            result.OsConcluidasMes.Should().Be(1);
            result.FaturamentoMensal.Should().Be(500m);
            result.TicketMedio.Should().Be(500m);
            result.FaturamentoUltimosSeisMeses.Should().HaveCount(6);
            result.FaturamentoUltimosSeisMeses[5].Mes.Should().Be(periodoAtual);
            result.FaturamentoUltimosSeisMeses[5].Valor.Should().Be(500m);
            result.FaturamentoUltimosSeisMeses[4].Mes.Should().Be($"{mesAnterior:yyyy-MM}");
            result.FaturamentoUltimosSeisMeses[4].Valor.Should().Be(800m);

            // T-032.5 & T-034.1: Peças com qtd_atual < qtd_minima
            result.PecasAbaixoMinimo.Should().Be(2);
            result.PecasCriticas.Should().HaveCount(2);
            result.PecasCriticas[0].Id.Should().Be(11);
            result.PecasCriticas[0].Deficit.Should().Be(9);
            result.PecasCriticas[1].Id.Should().Be(10);
            result.PecasCriticas[1].Deficit.Should().Be(3);
        }
    }
}
