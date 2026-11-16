# Data Model: Endpoint Agregador de KPIs, Dashboard Real com Gráficos e Alertas de Estoque Mínimo (US-032, US-033 & US-034)

**Feature Branch**: `028-dashboard-kpis-alertas`  
**Date**: 2026-11-16

---

## 1. DTOs de Resposta do Backend (`OficinaMotos.Application.DTOs.Responses.Dashboard`)

### 1.1 `DashboardKpisResponseDTO`
| Propriedade | Tipo C# | JSON Property | Descrição | Origem / Regra |
| :--- | :--- | :--- | :--- | :--- |
| `Periodo` | `string` | `periodo` | Mês/período de referência no formato `YYYY-MM` | Query param `periodo` ou `DateTime.UtcNow.ToString("yyyy-MM")` |
| `OsAbertas` | `int` | `osAbertas` | Total de OS não concluídas e não canceladas | `Status != Concluida && Status != Cancelada` (`T-032.3`) |
| `OsDoDia` | `int` | `osDoDia` | Total de OS abertas na data atual | `DataAbertura.Date == hoje.Date` (`T-032.3`) |
| `OsConcluidasMes` | `int` | `osConcluidasMes` | Total de OS concluídas no mês de referência | `Status == Concluida` no mês de `Periodo` |
| `FaturamentoMensal` | `decimal` | `faturamentoMensal` | Soma do valor faturado no mês de referência | Soma dos itens/pagamentos das OS concluídas no mês (`T-032.4`) |
| `TicketMedio` | `decimal` | `ticketMedio` | Valor médio por OS concluída no mês | `OsConcluidasMes > 0 ? FaturamentoMensal / OsConcluidasMes : 0` |
| `PecasAbaixoMinimo` | `int` | `pecasAbaixoMinimo` | Quantidade de peças com `Quantidade < EstoqueMinimo` | `EstoquePeca.Where(p => p.Quantidade < p.EstoqueMinimo).Count()` (`T-032.5`) |
| `OsPorStatus` | `List<DashboardOsStatusCountDTO>` | `osPorStatus` | Contagem de OS agrupadas por status | Agrupamento por `OrdemServicoStatus` (`T-032.3`, `T-033.2`) |
| `FaturamentoUltimosSeisMeses` | `List<DashboardFaturamentoMensalDTO>` | `faturamentoUltimosSeisMeses` | Série histórica de faturamento dos últimos 6 meses | 6 meses consecutivos até o mês de referência (`T-032.4`, `T-033.3`) |
| `PecasCriticas` | `List<DashboardPecaCriticaDTO>` | `pecasCriticas` | Lista de peças com `Quantidade < EstoqueMinimo` | Ordenada por maior déficit (`T-032.5`, `T-034.1`, `T-034.2`) |

### 1.2 `DashboardOsStatusCountDTO`
| Propriedade | Tipo C# | JSON Property | Descrição |
| :--- | :--- | :--- | :--- |
| `Status` | `string` | `status` | Nome canônico do enum (`Aberta`, `EmAndamento`, `AguardandoPeca`, `Concluida`, `Cancelada`) |
| `Label` | `string` | `label` | Rótulo amigável (`Aberta`, `Em Andamento`, `Aguardando Peça`, `Concluída`, `Cancelada`) |
| `Quantidade` | `int` | `quantidade` | Total de OS naquele status |

### 1.3 `DashboardFaturamentoMensalDTO`
| Propriedade | Tipo C# | JSON Property | Descrição |
| :--- | :--- | :--- | :--- |
| `Mes` | `string` | `mes` | Competência no formato `YYYY-MM` (ex.: `"2026-11"`) |
| `Rotulo` | `string` | `rotulo` | Rótulo curto para o gráfico (ex.: `"Nov/26"`) |
| `Valor` | `decimal` | `valor` | Valor total faturado no mês |
| `QuantidadeOs` | `int` | `quantidadeOs` | Quantidade de OS concluídas no mês |

### 1.4 `DashboardPecaCriticaDTO`
| Propriedade | Tipo C# | JSON Property | Descrição |
| :--- | :--- | :--- | :--- |
| `Id` | `long` | `id` | Identificador da peça (`EstoquePeca.Id`) usado na rota `/estoque/:id` |
| `Codigo` | `string` | `codigo` | Código SKU da peça (`EstoquePeca.Codigo`) |
| `Descricao` | `string` | `descricao` | Descrição da peça (`EstoquePeca.Descricao`) |
| `QuantidadeAtual` | `int` | `quantidadeAtual` | Quantidade disponível em estoque (`EstoquePeca.Quantidade`) |
| `EstoqueMinimo` | `int` | `estoqueMinimo` | Quantidade mínima configurada (`EstoquePeca.EstoqueMinimo`) |
| `Deficit` | `int` | `deficit` | Diferença `EstoqueMinimo - QuantidadeAtual` |
| `Unidade` | `string` | `unidade` | Unidade de medida (`UN`, `LT`, etc.) |

---

## 2. Modelos TypeScript do Frontend (`oficina-motos-web/src/app/core/models/dashboard.ts`)

```typescript
export interface DashboardOsStatusCount {
  status: string;
  label: string;
  quantidade: number;
}

export interface DashboardFaturamentoMensal {
  mes: string;
  rotulo: string;
  valor: number;
  quantidadeOs: number;
}

export interface DashboardPecaCritica {
  id: number;
  codigo: string;
  descricao: string;
  quantidadeAtual: number;
  estoqueMinimo: number;
  deficit: number;
  unidade: string;
}

export interface DashboardKpisResponse {
  periodo: string;
  osAbertas: number;
  osDoDia: number;
  osConcluidasMes: number;
  faturamentoMensal: number;
  ticketMedio: number;
  pecasAbaixoMinimo: number;
  osPorStatus: DashboardOsStatusCount[];
  faturamentoUltimosSeisMeses: DashboardFaturamentoMensal[];
  pecasCriticas: DashboardPecaCritica[];
}
```
