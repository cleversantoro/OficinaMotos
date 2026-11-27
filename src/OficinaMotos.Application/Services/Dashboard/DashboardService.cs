using OficinaMotos.Application.DTOs.Responses.Dashboard;
using OficinaMotos.Application.Interfaces.Dashboard;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Domain.Interfaces.Repositories.Estoque;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private static readonly string[] MesesAbreviadosPtBr =
        {
            "Jan", "Fev", "Mar", "Abr", "Mai", "Jun",
            "Jul", "Ago", "Set", "Out", "Nov", "Dez"
        };

        private readonly IOrdemServicoRepository _ordemServicoRepository;
        private readonly IEstoquePecaRepository _estoquePecaRepository;

        public DashboardService(
            IOrdemServicoRepository ordemServicoRepository,
            IEstoquePecaRepository estoquePecaRepository)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _estoquePecaRepository = estoquePecaRepository;
        }

        public async Task<DashboardKpisResponseDTO> GetKpisAsync(string? periodo = null)
        {
            var dataReferencia = ResolverDataReferencia(periodo);
            var inicioMesReferencia = new DateTime(dataReferencia.Year, dataReferencia.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var hoje = DateTime.UtcNow.Date;

            var ordens = await _ordemServicoRepository.GetAllAsync() ?? new List<OrdemServico>();
            var pecas = await _estoquePecaRepository.GetAllAsync() ?? new List<EstoquePeca>();

            // T-032.3 — Query OS abertas e do dia + OS por status
            var osAbertas = ordens.Count(o =>
                o.Status != OrdemServicoEnum.Concluida &&
                o.Status != OrdemServicoEnum.Cancelada);

            var osDoDia = ordens.Count(o => o.DataAbertura.Date == hoje);

            var osPorStatus = Enum.GetValues<OrdemServicoEnum>()
                .Select(status => new DashboardOsStatusCountDTO
                {
                    Status = status.ToString(),
                    Label = ObterLabelStatus(status),
                    Quantidade = ordens.Count(o => o.Status == status)
                })
                .ToList();

            // T-032.4 — Query faturamento mensal e série dos últimos 6 meses
            var osConcluidasNoMes = ordens
                .Where(o => o.Status == OrdemServicoEnum.Concluida)
                .Where(o =>
                {
                    var dataComp = o.DataConclusao ?? o.DataAbertura;
                    return dataComp.Year == inicioMesReferencia.Year && dataComp.Month == inicioMesReferencia.Month;
                })
                .ToList();

            var faturamentoMensal = osConcluidasNoMes.Sum(CalcularValorOrdemServico);
            var ticketMedio = osConcluidasNoMes.Count > 0
                ? Math.Round(faturamentoMensal / osConcluidasNoMes.Count, 2)
                : 0m;

            var faturamentoUltimosSeisMeses = new List<DashboardFaturamentoMensalDTO>(6);
            for (var offset = 5; offset >= 0; offset--)
            {
                var mesAlvo = inicioMesReferencia.AddMonths(-offset);
                var osDoMes = ordens
                    .Where(o => o.Status == OrdemServicoEnum.Concluida)
                    .Where(o =>
                    {
                        var dataComp = o.DataConclusao ?? o.DataAbertura;
                        return dataComp.Year == mesAlvo.Year && dataComp.Month == mesAlvo.Month;
                    })
                    .ToList();

                var valorMes = osDoMes.Sum(CalcularValorOrdemServico);
                var nomeMes = MesesAbreviadosPtBr[mesAlvo.Month - 1];

                faturamentoUltimosSeisMeses.Add(new DashboardFaturamentoMensalDTO
                {
                    Mes = $"{mesAlvo:yyyy-MM}",
                    Rotulo = $"{nomeMes}/{mesAlvo:yy}",
                    Valor = valorMes,
                    QuantidadeOs = osDoMes.Count
                });
            }

            // T-032.5 & T-034.1 — Query peças com qtd_atual < qtd_minima (QuantidadeEstoque < EstoqueMinimo)
            var pecasCriticas = pecas
                .Where(p => p.QuantidadeEstoque < p.EstoqueMinimo)
                .OrderByDescending(p => p.EstoqueMinimo - p.QuantidadeEstoque)
                .ThenBy(p => p.Descricao ?? p.Nome)
                .Select(p => new DashboardPecaCriticaDTO
                {
                    Id = p.Id,
                    Codigo = p.Codigo,
                    Descricao = p.Descricao ?? p.Nome,
                    QuantidadeAtual = (int)p.QuantidadeEstoque,
                    EstoqueMinimo = (int)p.EstoqueMinimo,
                    Deficit = (int)(p.EstoqueMinimo - p.QuantidadeEstoque),
                    Unidade = string.IsNullOrWhiteSpace(p.Unidade) ? "UN" : p.Unidade
                })
                .ToList();

            return new DashboardKpisResponseDTO
            {
                Periodo = $"{inicioMesReferencia:yyyy-MM}",
                OsAbertas = osAbertas,
                OsDoDia = osDoDia,
                OsConcluidasMes = osConcluidasNoMes.Count,
                FaturamentoMensal = faturamentoMensal,
                TicketMedio = ticketMedio,
                PecasAbaixoMinimo = pecasCriticas.Count,
                OsPorStatus = osPorStatus,
                FaturamentoUltimosSeisMeses = faturamentoUltimosSeisMeses,
                PecasCriticas = pecasCriticas
            };
        }

        private static decimal CalcularValorOrdemServico(OrdemServico os)
        {
            if (os.Itens != null && os.Itens.Count > 0)
            {
                var somaItens = os.Itens.Sum(i => i.Total > 0 ? i.Total : i.Quantidade * i.ValorUnitario);
                if (somaItens > 0)
                {
                    return somaItens;
                }
            }

            if (os.Pagamentos != null && os.Pagamentos.Count > 0)
            {
                return os.Pagamentos.Sum(p => p.Valor);
            }

            return 0m;
        }

        private static DateTime ResolverDataReferencia(string? periodo)
        {
            var agora = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(periodo))
            {
                return agora;
            }

            var normalizado = periodo.Trim();
            if (DateTime.TryParseExact(
                    normalizado,
                    new[] { "yyyy-MM", "yyyy-MM-dd", "MM/yyyy" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var parsed))
            {
                return parsed;
            }

            return agora;
        }

        private static string ObterLabelStatus(OrdemServicoEnum status) => status switch
        {
            OrdemServicoEnum.Aberta => "Aberta",
            OrdemServicoEnum.EmAndamento => "Em Andamento",
            OrdemServicoEnum.AguardandoPeca => "Aguardando Peça",
            OrdemServicoEnum.Concluida => "Concluída",
            OrdemServicoEnum.Cancelada => "Cancelada",
            _ => status.ToString()
        };
    }
}
