using AutoMapper;
using FluentAssertions;
using Moq;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.Services.OrdemServicoRepo;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.Mecanico;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using OficinaMotos.Tests.Helpers;

namespace OficinaMotos.Tests.Services.OrdemServicoItems
{
    public class OrdemServicoItemServiceTests
    {
        [Fact]
        public async Task ItemComAtividadeInvalida_DeveRejeitar()
        {
            var itemRepository = new Mock<IOrdemServicoItemRepository>();
            var prestadorRepository = new Mock<IPrestadorRepository>();
            var atividadeRepository = new Mock<IAtividadeProfissionalRepository>();
            prestadorRepository.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(new Prestador
            {
                Codigo = "PRE-10",
                PessoaId = 1,
                Atividades = new HashSet<PrestadorAtividade>()
            });
            atividadeRepository.Setup(x => x.GetByIdAsync(99)).ReturnsAsync(new AtividadeProfissional
            {
                Codigo = "ELE",
                Nome = "Elétrica"
            });
            var sut = new OrdemServicoItemService(
                itemRepository.Object,
                TestDbContextFactory.CreateMapper(),
                prestadorRepository.Object,
                atividadeRepository.Object);

            Func<Task> act = () => sut.CreateAsync(new CreateOrdemServicoItemDTO
            {
                OrdemServicoId = 1,
                PrestadorId = 10,
                AtividadeProfissionalId = 99,
                Descricao = "Reparo elétrico",
                Quantidade = 1,
                ValorUnitario = 100
            });

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*não está habilitado*");
            itemRepository.Verify(x => x.AddAsync(It.IsAny<OrdemServicoItem>()), Times.Never);
        }
    }
}
