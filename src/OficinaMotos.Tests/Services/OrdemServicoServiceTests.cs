using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.Services.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using OficinaMotos.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Services
{
    public class OrdemServicoServiceTests
    {
        private readonly Mock<IOrdemServicoRepository> _repositoryMock;
        private readonly Mock<IOrdemServicoPagamentoRepository> _pagamentoRepositoryMock;
        private readonly Mock<IFinanceiroContaReceberRepository> _contaReceberRepositoryMock;
        private readonly IMapper _mapper;
        private readonly OrdemServicoService _sut;

        public OrdemServicoServiceTests()
        {
            _repositoryMock = new Mock<IOrdemServicoRepository>();
            _pagamentoRepositoryMock = new Mock<IOrdemServicoPagamentoRepository>();
            _contaReceberRepositoryMock = new Mock<IFinanceiroContaReceberRepository>();
            _mapper = TestDbContextFactory.CreateMapper();

            _sut = new OrdemServicoService(
                _repositoryMock.Object,
                _mapper,
                _pagamentoRepositoryMock.Object,
                _contaReceberRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_WithoutVeiculoId_Succeeds()
        {
            // Arrange (T-028.1)
            var request = new CreateOrdemServicoDTO
            {
                ClienteId = 10,
                VeiculoId = null,
                DescricaoProblema = "Troca de pastilha de freio dianteiro",
                Status = OrdemServicoEnum.Aberta
            };
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<OrdemServico>()))
                .ReturnsAsync((OrdemServico o) => o);

            // Act
            var result = await _sut.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.VeiculoId.Should().BeNull();
            _repositoryMock.Verify(r => r.AddAsync(It.Is<OrdemServico>(o => o.VeiculoId == null)), Times.Once);
        }

        [Fact]
        public async Task ChangeStatus_ValidTransition_Succeeds()
        {
            // Arrange (T-028.2)
            var ordem = new OrdemServico
            {
                ClienteId = 10,
                MecanicoId = 5,
                VeiculoId = 25,
                DescricaoProblema = "RevisÃ£o de 10.000 km",
                Status = OrdemServicoEnum.Aberta
            }.WithId(101);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(101))
                .ReturnsAsync(ordem);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<OrdemServico>()))
                .Returns(Task.CompletedTask);

            // Act 1: Aberta -> EmAndamento
            var emAndamentoResult = await _sut.ChangeStatusAsync(101, OrdemServicoEnum.EmAndamento);

            // Assert 1
            emAndamentoResult.Should().NotBeNull();
            emAndamentoResult!.Status.Should().Be(OrdemServicoEnum.EmAndamento);
            ordem.Status.Should().Be(OrdemServicoEnum.EmAndamento);

            // Act 2: EmAndamento -> Concluida
            var concluidaResult = await _sut.ChangeStatusAsync(101, OrdemServicoEnum.Concluida);

            // Assert 2
            concluidaResult.Should().NotBeNull();
            concluidaResult!.Status.Should().Be(OrdemServicoEnum.Concluida);
            concluidaResult.DataConclusao.Should().NotBeNull();
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<OrdemServico>(o => o.Id == 101)), Times.Exactly(2));
        }

        [Fact]
        public async Task ChangeStatus_InvalidTransition_ThrowsException()
        {
            // Arrange (T-028.3)
            var ordemCancelada = new OrdemServico
            {
                ClienteId = 10,
                MecanicoId = 5,
                VeiculoId = 25,
                DescricaoProblema = "Ordem jÃ¡ cancelada",
                Status = OrdemServicoEnum.Cancelada
            }.WithId(202);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(202))
                .ReturnsAsync(ordemCancelada);

            // Act
            Func<Task> act = async () => await _sut.ChangeStatusAsync(202, OrdemServicoEnum.EmAndamento);

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*TransiÃ§Ã£o de status invÃ¡lida*");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<OrdemServico>()), Times.Never);
        }

        [Fact]
        public async Task CalculateTotal_WithPecasAndServicos_IsCorrect()
        {
            // Arrange (T-028.4)
            var ordem = new OrdemServico
            {
                ClienteId = 10,
                MecanicoId = 5,
                VeiculoId = 25,
                DescricaoProblema = "RevisÃ£o completa com troca de Ã³leo, filtro e mÃ£o de obra",
                Status = OrdemServicoEnum.EmAndamento,
                Itens = new List<OrdemServicoItem>
                {
                    // PeÃ§a 1: 2x Ã“leo 10W40 (R$ 45,00 un = R$ 90,00)
                    new OrdemServicoItem
                    {
                        OrdemServicoId = 303,
                        PecaId = 11,
                        Descricao = "Ã“leo Lubrificante 10W40",
                        Quantidade = 2,
                        ValorUnitario = 45.00m,
                        Total = 90.00m
                    },
                    // PeÃ§a 2: 1x Filtro de Ã“leo (R$ 35,50 un, testando cÃ¡lculo Quantidade * ValorUnitario)
                    new OrdemServicoItem
                    {
                        OrdemServicoId = 303,
                        PecaId = 12,
                        Descricao = "Filtro de Ã“leo Original",
                        Quantidade = 1,
                        ValorUnitario = 35.50m,
                        Total = 0m
                    },
                    // ServiÃ§o / MÃ£o de Obra: PecaId nulo (1x R$ 150,00)
                    new OrdemServicoItem
                    {
                        OrdemServicoId = 303,
                        PecaId = null,
                        Descricao = "MÃ£o de Obra - RevisÃ£o Geral",
                        Quantidade = 1,
                        ValorUnitario = 150.00m,
                        Total = 150.00m
                    }
                }
            }.WithId(303);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(303))
                .ReturnsAsync(ordem);

            // Act
            var totalSync = _sut.CalculateTotal(ordem);
            var totalAsync = await _sut.CalculateTotalAsync(303);

            // Assert (90.00 + 35.50 + 150.00 = 275.50)
            totalSync.Should().Be(275.50m);
            totalAsync.Should().Be(275.50m);
        }

        [Fact]
        public async Task GetAllAsync_MapsClienteMecanicoEVeiculoSummaryFields()
        {
            // Arrange
            var ordem = new OrdemServico
            {
                ClienteId = 1,
                Cliente = new Cliente { Nome = "JoÃ£o Silva", NomeExibicao = "JoÃ£o" }.WithId(1),
                MecanicoId = 2,
                Mecanico = new Mecanico { Codigo = "MEC-002", Nome = "Carlos", Sobrenome = "Souza" }.WithId(2),
                VeiculoId = 3,
                Veiculo = new Veiculo
                {
                    Placa = "ABC1D23",
                    Modelo = new VeiculoModelo { Nome = "CB 500F" }.WithId(10)
                }.WithId(3),
                DescricaoProblema = "RevisÃ£o completa",
                Status = OrdemServicoEnum.Aberta
            }.WithId(1);

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<OrdemServico> { ordem });

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().HaveCount(1);
            var dto = result[0];
            dto.ClienteNome.Should().Be("JoÃ£o Silva");
            dto.ClienteNomeExibicao.Should().Be("JoÃ£o");
            dto.MecanicoCodigo.Should().Be("MEC-002");
            dto.MecanicoNome.Should().Be("Carlos");
            dto.MecanicoSobrenome.Should().Be("Souza");
            dto.VeiculoPlaca.Should().Be("ABC1D23");
            dto.VeiculoModeloNome.Should().Be("CB 500F");
        }
    }
}
