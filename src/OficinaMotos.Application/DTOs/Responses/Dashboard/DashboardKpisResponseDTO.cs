using System.Collections.Generic;

namespace OficinaMotos.Application.DTOs.Responses.Dashboard
{
    public class DashboardKpisResponseDTO
    {
        public string Periodo { get; set; } = string.Empty;
        public int OsAbertas { get; set; }
        public int OsDoDia { get; set; }
        public int OsConcluidasMes { get; set; }
        public decimal FaturamentoMensal { get; set; }
        public decimal TicketMedio { get; set; }
        public int PecasAbaixoMinimo { get; set; }
        public List<DashboardOsStatusCountDTO> OsPorStatus { get; set; } = new();
        public List<DashboardFaturamentoMensalDTO> FaturamentoUltimosSeisMeses { get; set; } = new();
        public List<DashboardPecaCriticaDTO> PecasCriticas { get; set; } = new();
    }

    public class DashboardOsStatusCountDTO
    {
        public string Status { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }

    public class DashboardFaturamentoMensalDTO
    {
        public string Mes { get; set; } = string.Empty;
        public string Rotulo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int QuantidadeOs { get; set; }
    }

    public class DashboardPecaCriticaDTO
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int QuantidadeAtual { get; set; }
        public int EstoqueMinimo { get; set; }
        public int Deficit { get; set; }
        public string Unidade { get; set; } = "UN";
    }
}
