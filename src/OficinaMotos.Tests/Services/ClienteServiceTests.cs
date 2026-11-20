using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.Services.Cliente;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Infrastructure.Repositories.Cliente;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Services
{
    public class ClienteServiceTests
    {
        private static async Task CreatePessoaAsync(OficinaContext context, long id, string documento)
        {
            if (await context.Pessoas.AnyAsync(p => p.Id == id)) return;
            var pessoa = new Pessoa
            {
                Codigo = id,
                TipoPessoa = "PF",
                Documento = documento,
                Nome = "Pessoa Cliente",
                NomeExibicao = "Pessoa Cliente"
            };
            pessoa.GetType().GetProperty("Id")?.SetValue(pessoa, id);
            context.Pessoas.Add(pessoa);
            await context.SaveChangesAsync();
        }
        [Fact]
        public async Task Create_PF_WithValidCpf_Succeeds()
        {
            // Arrange (T-027.1)
            await using var context = TestDbContextFactory.Create();
            await CreatePessoaAsync(context, 1, "52998224725");
            var repository = new ClienteRepository(context);
            var mapper = TestDbContextFactory.CreateMapper();
            var sut = new ClienteService(repository, mapper);

            var request = new CreateClienteDTO
            {
                PessoaId = 1,
                Nome = "Carlos Eduardo Oliveira",
                Cpf = "52998224725",
                Email = "carlos.oliveira@oficinamotos.com",
                Telefone = "11987654321"
            };

            // Act
            var result = await sut.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Nome.Should().Be("Carlos Eduardo Oliveira");
            result.NomeExibicao.Should().Be("Carlos Eduardo Oliveira");
            result.Documento.Should().Be("52998224725");
            result.Email.Should().Be("carlos.oliveira@oficinamotos.com");
            result.Telefone.Should().Be("11987654321");
            result.Tipo.Should().Be((int)ClienteTipo.PessoaFisica);
            result.Status.Should().Be((int)ClienteStatus.Ativo);

            var persisted = await context.Clientes.SingleOrDefaultAsync(c => c.Id == result.Id);
            persisted.Should().NotBeNull();
            persisted!.DeletedAt.Should().BeNull();
        }

        [Fact]
        public async Task Create_DuplicateCpf_ThrowsDomainException()
        {
            // Arrange (T-027.2)
            await using var context = TestDbContextFactory.Create();
            await CreatePessoaAsync(context, 1, "11144477735");
            await CreatePessoaAsync(context, 2, "11144477735");
            var repository = new ClienteRepository(context);
            var mapper = TestDbContextFactory.CreateMapper();
            var sut = new ClienteService(repository, mapper);

            var firstRequest = new CreateClienteDTO
            {
                PessoaId = 1,
                Nome = "Cliente Original",
                Cpf = "11144477735",
                Email = "original@oficinamotos.com",
                Telefone = "11911112222"
            };
            await sut.CreateAsync(firstRequest);

            var duplicateRequest = new CreateClienteDTO
            {
                PessoaId = 2,
                Nome = "Cliente Duplicado",
                Cpf = "11144477735",
                Email = "duplicado@oficinamotos.com",
                Telefone = "11933334444"
            };

            // Act
            Func<Task> act = async () => await sut.CreateAsync(duplicateRequest);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*CPF*");

            var count = await context.Clientes.CountAsync();
            count.Should().Be(1);
        }

        [Fact]
        public async Task Delete_SetsDeletedAt_NotPhysicalDelete()
        {
            // Arrange (T-027.3)
            await using var context = TestDbContextFactory.Create();
            await CreatePessoaAsync(context, 1, "98765432100");
            var repository = new ClienteRepository(context);
            var mapper = TestDbContextFactory.CreateMapper();
            var sut = new ClienteService(repository, mapper);

            var created = await sut.CreateAsync(new CreateClienteDTO
            {
                PessoaId = 1,
                Nome = "Cliente Para Exclusao Logica",
                Cpf = "98765432100",
                Email = "exclusao@oficinamotos.com",
                Telefone = "11955556666"
            });

            // Act
            var deleted = await sut.DeleteAsync(created.Id);

            // Assert
            deleted.Should().BeTrue();

            var physicalRecord = await context.Clientes
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(c => c.Id == created.Id);

            physicalRecord.Should().NotBeNull("a exclusão deve ser lógica (soft delete) e não remover fisicamente o registro");
            physicalRecord!.DeletedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_DeletedClient_ReturnsNull()
        {
            // Arrange (T-027.4)
            await using var context = TestDbContextFactory.Create();
            await CreatePessoaAsync(context, 1, "12312312387");
            var repository = new ClienteRepository(context);
            var mapper = TestDbContextFactory.CreateMapper();
            var sut = new ClienteService(repository, mapper);

            var created = await sut.CreateAsync(new CreateClienteDTO
            {
                PessoaId = 1,
                Nome = "Cliente Oculto Apos Delete",
                Cpf = "12312312387",
                Email = "oculto@oficinamotos.com",
                Telefone = "11977778888"
            });

            await sut.DeleteAsync(created.Id);
            context.ChangeTracker.Clear();

            // Act
            var result = await sut.GetByIdAsync(created.Id);

            // Assert
            result.Should().BeNull();
        }
    }
}
