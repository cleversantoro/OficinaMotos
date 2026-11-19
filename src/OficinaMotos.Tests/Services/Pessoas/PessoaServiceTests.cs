using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.Services.Pessoas;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Infrastructure.Repositories;
using OficinaMotos.Tests.Helpers;

namespace OficinaMotos.Tests.Services.Pessoas
{
    public class PessoaServiceTests
    {
        [Fact]
        public async Task Create_PessoaFisica_WithValidCpf_Succeeds()
        {
            await using var context = TestDbContextFactory.Create();
            var sut = new PessoaService(new PessoaRepository(context));

            var result = await sut.CreateAsync(new CreatePessoaDTO
            {
                TipoPessoa = "PF",
                Documento = "529.982.247-25",
                Nome = "Carlos Eduardo",
                PessoaFisica = new PessoaPfDTO { Rg = "1234567" }
            });

            result.Id.Should().BeGreaterThan(0);
            result.Documento.Should().Be("52998224725");
            result.TipoPessoa.Should().Be("PF");
            result.PessoaFisica.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_DuplicateDocumento_ThrowsDomainException()
        {
            await using var context = TestDbContextFactory.Create();
            var sut = new PessoaService(new PessoaRepository(context));

            await sut.CreateAsync(new CreatePessoaDTO
            {
                TipoPessoa = "PF",
                Documento = "52998224725",
                Nome = "Pessoa Original"
            });

            Func<Task> act = () => sut.CreateAsync(new CreatePessoaDTO
            {
                TipoPessoa = "PF",
                Documento = "529.982.247-25",
                Nome = "Pessoa Duplicada"
            });

            await act.Should().ThrowAsync<DomainException>();
            context.Pessoas.Count().Should().Be(1);
        }

        [Fact]
        public async Task Pessoa_PodeExercerPapeisClienteEFornecedor()
        {
            await using var context = TestDbContextFactory.Create();
            var sut = new PessoaService(new PessoaRepository(context));
            var pessoa = await sut.CreateAsync(new CreatePessoaDTO
            {
                TipoPessoa = "PJ",
                Documento = "04.252.011/0001-10",
                Nome = "Empresa Com Dois Papeis",
                PessoaJuridica = new PessoaPjDTO { RazaoSocial = "Empresa Com Dois Papeis LTDA" }
            });

            context.Clientes.Add(new Cliente
            {
                PessoaId = pessoa.Id,
                Codigo = 1,
                Nome = pessoa.Nome,
                NomeExibicao = pessoa.NomeExibicao,
                Documento = pessoa.Documento,
                Tipo = ClienteTipo.PessoaJuridica
            });
            context.Fornecedores.Add(new Fornecedor
            {
                PessoaId = pessoa.Id,
                Codigo = "FOR-1",
                Tipo = "SERVICOS",
                RazaoSocial = pessoa.PessoaJuridica?.RazaoSocial ?? pessoa.Nome
            });
            await context.SaveChangesAsync();

            context.Clientes.Count(x => x.PessoaId == pessoa.Id).Should().Be(1);
            context.Fornecedores.Count(x => x.PessoaId == pessoa.Id).Should().Be(1);
        }

        [Fact]
        public async Task Delete_Pessoa_SetsDeletedAt()
        {
            await using var context = TestDbContextFactory.Create();
            var sut = new PessoaService(new PessoaRepository(context));
            var pessoa = await sut.CreateAsync(new CreatePessoaDTO
            {
                TipoPessoa = "PJ",
                Documento = "04.252.011/0001-10",
                Nome = "Empresa Teste",
                PessoaJuridica = new PessoaPjDTO { RazaoSocial = "Empresa Teste LTDA" }
            });

            var deleted = await sut.DeleteAsync(pessoa.Id);

            deleted.Should().BeTrue();
            var persisted = await context.Pessoas.IgnoreQueryFilters().SingleAsync(x => x.Id == pessoa.Id);
            persisted.DeletedAt.Should().NotBeNull();
        }
    }
}
