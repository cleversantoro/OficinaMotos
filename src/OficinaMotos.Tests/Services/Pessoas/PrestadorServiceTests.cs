using FluentAssertions;
using Moq;
using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.Services.Mecanico;
using OficinaMotos.Application.Services.Prestador;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;

namespace OficinaMotos.Tests.Services.Pessoas
{
    public class PrestadorServiceTests
    {
        [Fact]
        public async Task Mecânico_PodePossuirMultiplasAtividades()
        {
            var prestadorRepository = new Mock<IPrestadorRepository>();
            var atividadeRepository = new Mock<IAtividadeProfissionalRepository>();
            atividadeRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new AtividadeProfissional { Codigo = "MEC", Nome = "Mecânica" });
            atividadeRepository.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new AtividadeProfissional { Codigo = "ELE", Nome = "Elétrica" });
            prestadorRepository.Setup(x => x.GetByPessoaIdAsync(10)).ReturnsAsync((Prestador?)null);
            prestadorRepository.Setup(x => x.GetByCodigoAsync("PRE-10")).ReturnsAsync((Prestador?)null);
            prestadorRepository.Setup(x => x.AddAsync(It.IsAny<Prestador>()))
                .ReturnsAsync((Prestador x) => x);

            var sut = new PrestadorService(prestadorRepository.Object, atividadeRepository.Object);
            var result = await sut.CreateAsync(new CreatePrestadorDTO
            {
                PessoaId = 10,
                Codigo = "PRE-10",
                MecanicamenteEspecializado = true,
                Atividades = new List<PrestadorAtividadeDTO>
                {
                    new() { AtividadeProfissionalId = 1, Principal = true, Ativo = true },
                    new() { AtividadeProfissionalId = 2, Principal = false, Ativo = true }
                }
            });

            result.Atividades.Should().HaveCount(2);
            result.Atividades.Count(x => x.Principal && x.Ativo).Should().Be(1);
        }

        [Fact]
        public async Task DuasAtividadesPrincipaisAtivas_DeveFalhar()
        {
            var prestadorRepository = new Mock<IPrestadorRepository>();
            var atividadeRepository = new Mock<IAtividadeProfissionalRepository>();
            var sut = new PrestadorService(prestadorRepository.Object, atividadeRepository.Object);

            Func<Task> act = () => sut.CreateAsync(new CreatePrestadorDTO
            {
                PessoaId = 10,
                Codigo = "PRE-10",
                Atividades = new List<PrestadorAtividadeDTO>
                {
                    new() { AtividadeProfissionalId = 1, Principal = true, Ativo = true },
                    new() { AtividadeProfissionalId = 2, Principal = true, Ativo = true }
                }
            });

            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("*uma atividade principal ativa*");
        }
    }
}
