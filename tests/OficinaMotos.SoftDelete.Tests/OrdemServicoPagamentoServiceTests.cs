using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.Mappings;
using OficinaMotos.Application.Services.OrdemServicoRepo;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories.FinanceiroRepo;
using OficinaMotos.Infrastructure.Repositories.OrdemServicoRepo;
using Microsoft.Extensions.DependencyInjection;
using OficinaMotos.Application.IoC;
using Xunit;

namespace OficinaMotos.SoftDelete.Tests
{
    public class OrdemServicoPagamentoServiceTests
    {
        private static OficinaContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<OficinaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new OficinaContext(options);
        }

        private static IMapper CreateMapper()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddApplication();
            return services.BuildServiceProvider().GetRequiredService<IMapper>();
        }

        private static async Task<(Cliente cliente, Mecanico mecanico)> CreateSeedDataAsync(OficinaContext context)
        {
            var cliente = new Cliente { Nome = "Cliente Teste", Documento = "12345678901", Tipo = ClienteTipo.PessoaFisica };
            var mecanico = new Mecanico { Nome = "Mecanico Teste", Sobrenome = "Silva", DocumentoPrincipal = "12345678901", DataAdmissao = DateTime.UtcNow };
            await context.Clientes.AddAsync(cliente);
            await context.Mecanicos.AddAsync(mecanico);
            await context.SaveChangesAsync();
            return (cliente, mecanico);
        }

        [Fact]
        public async Task RegistrarPagamentoAsync_PagamentoIntegral_DeveAtualizarStatusParaConcluidaEDataConclusao()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var (cliente, mecanico) = await CreateSeedDataAsync(context);
            var mapper = CreateMapper();
            var osRepo = new OrdemServicoRepository(context);
            var pagRepo = new OrdemServicoPagamentoRepository(context);
            var crRepo = new FinanceiroContaReceberRepository(context);
            var service = new OrdemServicoService(osRepo, mapper, pagRepo, crRepo);

            var os = new OrdemServico
            {
                ClienteId = cliente.Id,
                VeiculoId = 1,
                MecanicoId = mecanico.Id,
                DescricaoProblema = "Revisão geral",
                Status = OrdemServicoStatus.EmAndamento,
                Itens = new List<OrdemServicoItem>
                {
                    new OrdemServicoItem { Descricao = "Óleo", Quantidade = 2, ValorUnitario = 50, Total = 100 },
                    new OrdemServicoItem { Descricao = "Mão de Obra", Quantidade = 1, ValorUnitario = 150, Total = 150 }
                }
            };
            await osRepo.AddAsync(os);
            context.ChangeTracker.Clear();

            var request = new CreateOrdemServicoPagamentoDTO
            {
                OrdemServicoId = os.Id,
                Valor = 250m,
                Metodo = "PIX",
                Status = "Liquidado",
                DataPagamento = DateTime.UtcNow,
                Observacao = "Pagamento integral PIX"
            };

            // Act
            var result = await service.RegistrarPagamentoAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(250m, result.Valor);
            Assert.Equal("PIX", result.Metodo);
            Assert.Equal(OrdemServicoStatus.Concluida.ToString(), result.StatusOrdemServico);

            var updatedOs = await osRepo.GetByIdAsync(os.Id);
            Assert.NotNull(updatedOs);
            Assert.Equal(OrdemServicoStatus.Concluida, updatedOs.Status);
            Assert.NotNull(updatedOs.DataConclusao);

            var contasReceber = await crRepo.GetAllAsync();
            Assert.Single(contasReceber);
            var cr = contasReceber.First();
            Assert.Equal(250m, cr.Valor);
            Assert.Equal(cliente.Id, cr.ClienteId);
            Assert.Equal("Recebido", cr.Status);
            Assert.Contains($"OS #{os.Id}", cr.Descricao);
        }

        [Fact]
        public async Task RegistrarPagamentoAsync_PagamentoParcial_DeveManterStatusOperacional()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var (cliente, mecanico) = await CreateSeedDataAsync(context);
            var mapper = CreateMapper();
            var osRepo = new OrdemServicoRepository(context);
            var pagRepo = new OrdemServicoPagamentoRepository(context);
            var crRepo = new FinanceiroContaReceberRepository(context);
            var service = new OrdemServicoService(osRepo, mapper, pagRepo, crRepo);

            var os = new OrdemServico
            {
                ClienteId = cliente.Id,
                VeiculoId = 1,
                MecanicoId = mecanico.Id,
                DescricaoProblema = "Troca de relação",
                Status = OrdemServicoStatus.EmAndamento,
                Itens = new List<OrdemServicoItem>
                {
                    new OrdemServicoItem { Descricao = "Kit Relação", Quantidade = 1, ValorUnitario = 400, Total = 400 }
                }
            };
            await osRepo.AddAsync(os);
            context.ChangeTracker.Clear();

            var request = new CreateOrdemServicoPagamentoDTO
            {
                OrdemServicoId = os.Id,
                Valor = 150m,
                Metodo = "Dinheiro",
                Status = "Liquidado",
                DataPagamento = DateTime.UtcNow
            };

            // Act
            var result = await service.RegistrarPagamentoAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(150m, result.Valor);
            Assert.Equal(OrdemServicoStatus.EmAndamento.ToString(), result.StatusOrdemServico);

            var updatedOs = await osRepo.GetByIdAsync(os.Id);
            Assert.NotNull(updatedOs);
            Assert.Equal(OrdemServicoStatus.EmAndamento, updatedOs.Status);
            Assert.Null(updatedOs.DataConclusao);
        }

        [Fact]
        public async Task RegistrarPagamentoAsync_OSCancelada_DeveLancarInvalidOperationException()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var (cliente, mecanico) = await CreateSeedDataAsync(context);
            var mapper = CreateMapper();
            var osRepo = new OrdemServicoRepository(context);
            var pagRepo = new OrdemServicoPagamentoRepository(context);
            var crRepo = new FinanceiroContaReceberRepository(context);
            var service = new OrdemServicoService(osRepo, mapper, pagRepo, crRepo);

            var os = new OrdemServico
            {
                ClienteId = cliente.Id,
                MecanicoId = mecanico.Id,
                DescricaoProblema = "Cancelada pelo cliente",
                Status = OrdemServicoStatus.Cancelada
            };
            await osRepo.AddAsync(os);
            context.ChangeTracker.Clear();

            var request = new CreateOrdemServicoPagamentoDTO
            {
                OrdemServicoId = os.Id,
                Valor = 100m,
                Metodo = "PIX"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegistrarPagamentoAsync(request));
        }

        [Fact]
        public async Task RegistrarPagamentoAsync_OSInexistente_DeveLancarKeyNotFoundException()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var mapper = CreateMapper();
            var osRepo = new OrdemServicoRepository(context);
            var pagRepo = new OrdemServicoPagamentoRepository(context);
            var crRepo = new FinanceiroContaReceberRepository(context);
            var service = new OrdemServicoService(osRepo, mapper, pagRepo, crRepo);

            var request = new CreateOrdemServicoPagamentoDTO
            {
                OrdemServicoId = 99999,
                Valor = 100m
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.RegistrarPagamentoAsync(request));
        }
    }
}
