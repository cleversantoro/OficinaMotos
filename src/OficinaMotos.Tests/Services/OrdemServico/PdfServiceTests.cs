using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OficinaMotos.API.Controllers.OrdemServico;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Application.Services.OrdemServicoRepo;
using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using Xunit;
using OsEntity = OficinaMotos.Domain.Entities.OrdemServico;

namespace OficinaMotos.Tests.Services.OrdemServicoPdf
{
    public class PdfServiceTests
    {
        private readonly Mock<IOrdemServicoRepository> _ordemRepoMock = new();
        private readonly Mock<IVeiculoRepository> _veiculoRepoMock = new();

        private static T WithId<T>(T entity, long id) where T : BaseEntity
        {
            typeof(BaseEntity).GetProperty(nameof(BaseEntity.Id))!.SetValue(entity, id);
            return entity;
        }

        [Fact]
        public async Task GerarOrdemServicoPdfAsync_DeveGerarPdfValido_QuandoOrdemExistir()
        {
            var ordem = WithId(new OsEntity
            {
                ClienteId = 7,
                VeiculoId = 15,
                MecanicoId = 3,
                Status = OrdemServicoEnum.EmAndamento,
                DataAbertura = new DateTime(2026, 11, 16, 10, 30, 0, DateTimeKind.Utc),
                DescricaoProblema = "Revisão completa de freios e troca de óleo.",
                Cliente = WithId(new Cliente { Nome = "João Silva", NomeExibicao = "João Silva", Documento = "123.456.789-00" }, 7),
                Mecanico = WithId(new Mecanico { Nome = "Carlos", Sobrenome = "Souza" }, 3),
                Itens = new List<OrdemServicoItem>
                {
                    WithId(new OrdemServicoItem { OrdemServicoId = 42, Descricao = "Óleo Motul 10W40", Quantidade = 2, ValorUnitario = 65m, Total = 130m }, 1),
                    WithId(new OrdemServicoItem { OrdemServicoId = 42, Descricao = "Mão de obra revisão", Quantidade = 1, ValorUnitario = 150m, Total = 150m }, 2),
                },
                Pagamentos = new List<OrdemServicoPagamento>
                {
                    WithId(new OrdemServicoPagamento { OrdemServicoId = 42, Metodo = "PIX", Status = "Liquidado", Valor = 100m, DataPagamento = new DateTime(2026, 11, 16) }, 1),
                },
                Observacoes = new List<OrdemServicoObservacao>
                {
                    WithId(new OrdemServicoObservacao { OrdemServicoId = 42, Usuario = "Carlos", Texto = "Pastilha traseira com 30% de vida útil." }, 1),
                }
            }, 42);

            _ordemRepoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(ordem);
            _veiculoRepoMock.Setup(r => r.GetByIdAsync(15)).ReturnsAsync(WithId(new Veiculo
            {
                Placa = "ABC1D23",
                Cor = "Preta",
                AnoMod = 2024
            }, 15));

            var service = new PdfService(_ordemRepoMock.Object, _veiculoRepoMock.Object);

            var pdfBytes = await service.GerarOrdemServicoPdfAsync(42);

            pdfBytes.Should().NotBeNull();
            pdfBytes!.Length.Should().BeGreaterThan(100);
            Encoding.ASCII.GetString(pdfBytes, 0, 5).Should().Be("%PDF-");
        }

        [Fact]
        public async Task GerarOrdemServicoPdfAsync_DeveRetornarNull_QuandoOrdemNaoExistir()
        {
            _ordemRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((OsEntity?)null);
            var service = new PdfService(_ordemRepoMock.Object, _veiculoRepoMock.Object);

            var result = await service.GerarOrdemServicoPdfAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public void GerarOrdemServicoPdf_Guid_DeveDelegarEGerarPdf_QuandoOrdemExistir()
        {
            var guidBytes = new byte[16];
            BitConverter.GetBytes(10L).CopyTo(guidBytes, 0);
            var guid = new Guid(guidBytes);

            _ordemRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(WithId(new OsEntity
            {
                ClienteId = 1,
                VeiculoId = 1,
                MecanicoId = 1,
                Status = OrdemServicoEnum.Aberta,
                DescricaoProblema = "Troca de relação"
            }, 10));

            var service = new PdfService(_ordemRepoMock.Object);

            var pdfBytes = service.GerarOrdemServicoPdf(guid);

            pdfBytes.Should().NotBeNull();
            Encoding.ASCII.GetString(pdfBytes!, 0, 5).Should().Be("%PDF-");
        }

        [Fact]
        public async Task OrdemServicosController_GetPdf_DeveRetornarFilePdf_QuandoExistir()
        {
            var osServiceMock = new Mock<IOrdemServicoService>();
            var pdfServiceMock = new Mock<IPdfService>();
            var fakePdf = Encoding.ASCII.GetBytes("%PDF-1.4 fake");
            pdfServiceMock.Setup(s => s.GerarOrdemServicoPdfAsync(42)).ReturnsAsync(fakePdf);

            var controller = new OrdemServicosController(osServiceMock.Object, pdfServiceMock.Object);

            var result = await controller.GetPdf(42);

            var fileResult = result.Should().BeOfType<FileContentResult>().Subject;
            fileResult.ContentType.Should().Be("application/pdf");
            fileResult.FileDownloadName.Should().Be("os-42.pdf");
            fileResult.FileContents.Should().Equal(fakePdf);
        }

        [Fact]
        public async Task OrdemServicosController_GetPdf_DeveRetornarNotFound_QuandoNaoExistir()
        {
            var osServiceMock = new Mock<IOrdemServicoService>();
            var pdfServiceMock = new Mock<IPdfService>();
            pdfServiceMock.Setup(s => s.GerarOrdemServicoPdfAsync(999)).ReturnsAsync((byte[]?)null);

            var controller = new OrdemServicosController(osServiceMock.Object, pdfServiceMock.Object);

            var result = await controller.GetPdf(999);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
