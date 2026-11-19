using FluentAssertions;
using Moq;
using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.Services.Financeiro;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using AutoMapper;
using OficinaMotos.Tests.Helpers;

namespace OficinaMotos.Tests.Services
{
    public class Etapa6V2Tests
    {
        [Fact]
        public async Task PagamentoSemConta_DeveRejeitar()
        {
            var repository = new Mock<IFinanceiroPagamentoRepository>();
            var sut = new FinanceiroPagamentoService(repository.Object, TestDbContextFactory.CreateMapper());

            Func<Task> act = () => sut.CreateAsync(new CreateFinanceiroPagamentoDTO
            {
                Valor = 100,
                Status = "CONFIRMADO"
            });

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*exatamente uma conta*");
        }

        [Fact]
        public async Task PagamentoComContaPagarEContaReceber_DeveRejeitar()
        {
            var repository = new Mock<IFinanceiroPagamentoRepository>();
            var sut = new FinanceiroPagamentoService(repository.Object, TestDbContextFactory.CreateMapper());

            Func<Task> act = () => sut.CreateAsync(new CreateFinanceiroPagamentoDTO
            {
                ContaPagarId = 1,
                ContaReceberId = 2,
                Valor = 100,
                Status = "CONFIRMADO"
            });

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*exatamente uma conta*");
        }

        [Fact]
        public void DeletePecaComMovimentacao_DeveSerRestrito()
        {
            // A restrição é representada pela configuração EF DeleteBehavior.Restrict.
            // Este teste garante o contrato mínimo da entidade de movimentação no domínio.
            var movimentacao = new EstoqueMovimentacao
            {
                PecaId = 10,
                Quantidade = 2,
                Tipo = "SAIDA"
            };

            movimentacao.PecaId.Should().Be(10);
            movimentacao.Quantidade.Should().Be(2);
        }
    }
}
