using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using OficinaMotos.Application.Interfaces.OrdemServico;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OsEntity = OficinaMotos.Domain.Entities.OrdemServico;

namespace OficinaMotos.Application.Services.OrdemServicoRepo
{
    public class PdfService : IPdfService
    {
        private static readonly CultureInfo PtBr = new("pt-BR");
        private readonly IOrdemServicoRepository _ordemServicoRepository;
        private readonly IVeiculoRepository? _veiculoRepository;

        static PdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public PdfService(
            IOrdemServicoRepository ordemServicoRepository,
            IVeiculoRepository? veiculoRepository = null)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _veiculoRepository = veiculoRepository;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]?> GerarOrdemServicoPdfAsync(long id)
        {
            var ordem = await _ordemServicoRepository.GetByIdAsync(id);
            if (ordem == null)
            {
                return null;
            }

            if (ordem.Veiculo == null && ordem.VeiculoId.HasValue && ordem.VeiculoId.Value > 0 && _veiculoRepository != null)
            {
                ordem.Veiculo = await _veiculoRepository.GetByIdAsync(ordem.VeiculoId.Value);
            }

            return BuildOrdemServicoPdf(ordem);
        }

        public byte[]? GerarOrdemServicoPdf(long id)
        {
            return GerarOrdemServicoPdfAsync(id).GetAwaiter().GetResult();
        }

        public byte[]? GerarOrdemServicoPdf(Guid id)
        {
            var bytes = id.ToByteArray();
            var numericId = Math.Abs(BitConverter.ToInt64(bytes, 0));
            if (numericId == 0)
            {
                numericId = 1;
            }

            return GerarOrdemServicoPdf(numericId);
        }

        private static byte[] BuildOrdemServicoPdf(OsEntity ordem)
        {
            var itens = ordem.Itens?.ToList() ?? [];
            var pagamentos = ordem.Pagamentos?.ToList() ?? [];
            var observacoes = ordem.Observacoes?.ToList() ?? [];

            var totalItens = itens.Sum(i => i.Total > 0 ? i.Total : i.Quantidade * i.ValorUnitario);
            var totalPago = pagamentos.Sum(p => p.Valor);
            var saldoPendente = Math.Max(0m, totalItens - totalPago);

            var clienteNome = !string.IsNullOrWhiteSpace(ordem.Cliente?.NomeExibicao)
                ? ordem.Cliente!.NomeExibicao
                : (!string.IsNullOrWhiteSpace(ordem.Cliente?.Nome) ? ordem.Cliente!.Nome : $"Cliente #{ordem.ClienteId}");

            var prestadores = itens
                .Where(i => i.Prestador != null)
                .Select(i => i.Prestador!.Codigo)
                .Distinct()
                .ToList();
            var executoresNome = prestadores.Count > 0
                ? string.Join(", ", prestadores)
                : "Não informado";

            var veiculoTexto = ordem.Veiculo != null
                ? $"{ordem.Veiculo.Placa}" +
                  (!string.IsNullOrWhiteSpace(ordem.Veiculo.Cor) ? $" · {ordem.Veiculo.Cor}" : string.Empty) +
                  (ordem.Veiculo.AnoMod.HasValue ? $" · Ano {ordem.Veiculo.AnoMod}" : string.Empty)
                : "Não informado";

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(32);
                    page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken4));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().Text("Oficina MotoPro").FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                                left.Item().Text("Relatório Oficial de Ordem de Serviço").FontSize(10).FontColor(Colors.Grey.Darken1);
                            });

                            row.ConstantItem(180).AlignRight().Column(right =>
                            {
                                right.Item().Text($"OS #{ordem.Id}").FontSize(16).Bold();
                                right.Item().Text($"Status: {FormatStatus(ordem.Status)}").FontSize(10).SemiBold();
                            });
                        });

                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                    });

                    page.Content().PaddingVertical(12).Column(col =>
                    {
                        col.Spacing(14);

                        // Dados Gerais
                        col.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(box =>
                        {
                            box.Spacing(6);
                            box.Item().Text("Dados Gerais").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);

                            box.Item().Row(row =>
                            {
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("Cliente: ").SemiBold();
                                    t.Span(clienteNome);
                                });
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("Documento: ").SemiBold();
                                    t.Span(string.IsNullOrWhiteSpace(ordem.Cliente?.Documento) ? "-" : ordem.Cliente!.Documento);
                                });
                            });

                            box.Item().Row(row =>
                            {
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("Veículo: ").SemiBold();
                                    t.Span(veiculoTexto);
                                });
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("Prestadores Responsáveis: ").SemiBold();
                                    t.Span(executoresNome);
                                });
                            });

                            box.Item().Row(row =>
                            {
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("Data de Abertura: ").SemiBold();
                                    t.Span(ordem.DataAbertura.ToString("dd/MM/yyyy HH:mm", PtBr));
                                });
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("Data de Conclusão: ").SemiBold();
                                    t.Span(ordem.DataConclusao.HasValue ? ordem.DataConclusao.Value.ToString("dd/MM/yyyy HH:mm", PtBr) : "-");
                                });
                            });

                            box.Item().PaddingTop(4).Text(t =>
                            {
                                t.Span("Descrição do Problema: ").SemiBold();
                                t.Span(string.IsNullOrWhiteSpace(ordem.DescricaoProblema) ? "Não informada." : ordem.DescricaoProblema);
                            });
                        });

                        // Itens da OS
                        col.Item().Column(sec =>
                        {
                            sec.Spacing(6);
                            sec.Item().Text("Itens da Ordem de Serviço").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);

                            if (itens.Count == 0)
                            {
                                sec.Item().Text("Nenhum item registrado nesta ordem de serviço.").Italic().FontColor(Colors.Grey.Darken1);
                            }
                            else
                            {
                                sec.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(4);
                                        columns.ConstantColumn(50);
                                        columns.ConstantColumn(95);
                                        columns.ConstantColumn(95);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).Text("Descrição").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).AlignRight().Text("Qtd").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).AlignRight().Text("Vlr. Unitário").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten3).Padding(6).AlignRight().Text("Subtotal").SemiBold();
                                    });

                                    foreach (var item in itens)
                                    {
                                        var subtotal = item.Total > 0 ? item.Total : item.Quantidade * item.ValorUnitario;
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(item.Descricao);
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text(item.Quantidade.ToString(PtBr));
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text(FormatCurrency(item.ValorUnitario));
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text(FormatCurrency(subtotal));
                                    }
                                });
                            }
                        });

                        // Resumo Financeiro e Pagamentos
                        col.Item().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Column(fin =>
                        {
                            fin.Spacing(6);
                            fin.Item().Text("Resumo Financeiro").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);

                            fin.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Total da OS: {FormatCurrency(totalItens)}").SemiBold();
                                row.RelativeItem().Text($"Total Pago: {FormatCurrency(totalPago)}").SemiBold().FontColor(Colors.Green.Darken2);
                                row.RelativeItem().Text($"Saldo Pendente: {FormatCurrency(saldoPendente)}").SemiBold()
                                    .FontColor(saldoPendente > 0 ? Colors.Orange.Darken2 : Colors.Grey.Darken2);
                            });

                            if (pagamentos.Count > 0)
                            {
                                fin.Item().PaddingTop(4).Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(2);
                                        columns.RelativeColumn(2);
                                        columns.RelativeColumn(2);
                                        columns.ConstantColumn(95);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Background(Colors.Grey.Lighten4).Padding(4).Text("Método").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten4).Padding(4).Text("Status").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten4).Padding(4).Text("Data").SemiBold();
                                        header.Cell().Background(Colors.Grey.Lighten4).Padding(4).AlignRight().Text("Valor").SemiBold();
                                    });

                                    foreach (var pag in pagamentos)
                                    {
                                        table.Cell().Padding(4).Text(string.IsNullOrWhiteSpace(pag.Metodo) ? "Não informado" : pag.Metodo);
                                        table.Cell().Padding(4).Text(string.IsNullOrWhiteSpace(pag.Status) ? "Registrado" : pag.Status);
                                        table.Cell().Padding(4).Text(pag.DataPagamento.HasValue ? pag.DataPagamento.Value.ToString("dd/MM/yyyy", PtBr) : "-");
                                        table.Cell().Padding(4).AlignRight().Text(FormatCurrency(pag.Valor));
                                    }
                                });
                            }
                        });

                        // Observações
                        if (observacoes.Count > 0)
                        {
                            col.Item().Column(obsSec =>
                            {
                                obsSec.Spacing(4);
                                obsSec.Item().Text("Observações").FontSize(12).Bold().FontColor(Colors.Blue.Darken2);
                                foreach (var obs in observacoes)
                                {
                                    var autor = string.IsNullOrWhiteSpace(obs.Usuario) ? "Sistema" : obs.Usuario;
                                    obsSec.Item().Text($"• [{autor}] {obs.Texto}").FontSize(9);
                                }
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Oficina MotoPro · Documento gerado em ");
                        text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm", PtBr));
                        text.Span(" · Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }

        private static string FormatCurrency(decimal value) => value.ToString("C2", PtBr);

        private static string FormatStatus(OrdemServicoStatus status) => status switch
        {
            OrdemServicoStatus.Aberta => "Aberta",
            OrdemServicoStatus.EmAndamento => "Em Andamento",
            OrdemServicoStatus.AguardandoPeca => "Aguardando Peça",
            OrdemServicoStatus.Concluida => "Concluída",
            OrdemServicoStatus.Cancelada => "Cancelada",
            _ => status.ToString()
        };
    }
}
