# Contratos de API e Componentes: Dashboard KPIs e Alertas de Estoque (US-032, US-033 & US-034)

**Feature Branch**: `028-dashboard-kpis-alertas`  
**Date**: 2026-11-16

---

## 1. Contrato HTTP — `GET /api/v1/Dashboard/kpis` (`US-032` & `T-034.1`)

- **Controller**: `DashboardController` (`oficina-motos-api/src/OficinaMotos.API/Controllers/Dashboard/DashboardController.cs`)
- **Autorização**: `[Authorize]` (requer Bearer token JWT válido; retorna `401 Unauthorized` quando ausente/inválido)
- **Método e Rota**: `GET /api/v1/Dashboard/kpis`
- **Query Parameters**:
  - `periodo` (`string?`, opcional): Competência no formato `YYYY-MM` (ex.: `2026-11`) ou marcador de período (`mes_atual`). Quando omitido, assume o mês corrente.
- **Response `200 OK` (`application/json`)**:
  ```json
  {
    "periodo": "2026-11",
    "osAbertas": 5,
    "osDoDia": 2,
    "osConcluidasMes": 8,
    "faturamentoMensal": 4850.00,
    "ticketMedio": 606.25,
    "pecasAbaixoMinimo": 2,
    "osPorStatus": [
      { "status": "Aberta", "label": "Aberta", "quantidade": 2 },
      { "status": "EmAndamento", "label": "Em Andamento", "quantidade": 2 },
      { "status": "AguardandoPeca", "label": "Aguardando Peça", "quantidade": 1 },
      { "status": "Concluida", "label": "Concluída", "quantidade": 8 },
      { "status": "Cancelada", "label": "Cancelada", "quantidade": 1 }
    ],
    "faturamentoUltimosSeisMeses": [
      { "mes": "2026-06", "rotulo": "Jun/26", "valor": 3200.00, "quantidadeOs": 5 },
      { "mes": "2026-07", "rotulo": "Jul/26", "valor": 4100.00, "quantidadeOs": 7 },
      { "mes": "2026-08", "rotulo": "Ago/26", "valor": 2950.00, "quantidadeOs": 4 },
      { "mes": "2026-09", "rotulo": "Set/26", "valor": 5120.00, "quantidadeOs": 9 },
      { "mes": "2026-10", "rotulo": "Out/26", "valor": 4400.00, "quantidadeOs": 7 },
      { "mes": "2026-11", "rotulo": "Nov/26", "valor": 4850.00, "quantidadeOs": 8 }
    ],
    "pecasCriticas": [
      {
        "id": 14,
        "codigo": "PAST-FREIO-CB300",
        "descricao": "Pastilha de Freio Dianteira CB 300",
        "quantidadeAtual": 2,
        "estoqueMinimo": 5,
        "deficit": 3,
        "unidade": "UN"
      }
    ]
  }
  ```

---

## 2. Contrato de Serviço e Componente Frontend (`US-033` & `US-034`)

### 2.1 `apiPaths.dashboard` (`src/app/core/services/api-paths.ts`)
```typescript
dashboard: {
  kpis: '/api/v1/Dashboard/kpis',
}
```

### 2.2 `DashboardService` (`src/app/core/services/dashboard.service.ts`)
- `getKpis(periodo?: string): Observable<DashboardKpisResponse>`
  - Realiza `GET` em `apiPaths.dashboard.kpis` repassando `{ periodo }` quando fornecido.

### 2.3 `DashboardPrincipalComponent` (`src/app/features/dashboard/pages/dashboard-principal/dashboard-principal.ts`)
- **Consumo da API (`T-033.1`)**: Invoca `this.dashboardService.getKpis()` no `ngOnInit()`.
- **Skeleton Loading (`T-033.4`)**: Enquanto `loading()` é `true`, renderiza os cards de KPI no estado `.kpi-card.skeleton-card` (`data-testid="kpi-skeleton"`).
- **Gráfico de OS por Status (`T-033.2`)**: Exibe a seção `"Gráfico de OS por Status"` (`data-testid="chart-os-status"`) iterando sobre `osPorStatus` com barras proporcionais e contagens.
- **Gráfico de Faturamento Mensal - 6 Meses (`T-033.3`)**: Exibe a seção `"Faturamento Mensal (Últimos 6 Meses)"` (`data-testid="chart-faturamento-mensal"`) iterando sobre os 6 meses de `faturamentoUltimosSeisMeses`.
- **Card "Alertas de Estoque" (`T-034.2`)**: Exibe o card `"Alertas de Estoque"` (`data-testid="card-alertas-estoque"`) listando `pecasCriticas`, onde cada item possui um link `[routerLink]="['/estoque', peca.id]"` (apontando para `/estoque/:id`).
