# Quickstart Validation Guide: Endpoint Agregador de KPIs, Dashboard Real e Alertas de Estoque (US-032, US-033 & US-034)

**Feature Branch**: `028-dashboard-kpis-alertas`  
**Date**: 2026-11-16

---

## Pré-requisitos

- **.NET SDK 8.0+** instalado (`dotnet --version`)
- **Node.js 20+ / npm 10+** instalado (`npm --version`)

---

## 1. Validar Backend (`US-032` & `T-034.1`)

A partir do diretório `oficina-motos-api/`:

```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet build OficinaMotos.slnx
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Dashboard"
```

**Resultado esperado**:
- Compilação limpa sem erros.
- Aprovação de todos os testes unitários de `DashboardServiceTests` e testes de integração de `DashboardControllerIntegrationTests`:
  - `GET /api/v1/Dashboard/kpis` sem token retorna `401 Unauthorized` (`T-032.1`);
  - `DashboardService.GetKpisAsync(periodo)` retorna `osAbertas`, `osDoDia` e `osPorStatus` (`T-032.2`, `T-032.3`);
  - `DashboardService.GetKpisAsync(periodo)` retorna `faturamentoMensal` e `faturamentoUltimosSeisMeses` com 6 meses (`T-032.4`);
  - `DashboardService.GetKpisAsync(periodo)` retorna `pecasAbaixoMinimo` e `pecasCriticas` para peças com `Quantidade < EstoqueMinimo` (`T-032.5`, `T-034.1`).

---

## 2. Validar Frontend (`US-033` & `US-034`)

A partir do diretório `oficina-motos-web/`:

```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm test -- --watch=false
npm run build
```

**Resultado esperado**:
- Todas as suítes Vitest passam com 100% de sucesso, incluindo `dashboard.service.spec.ts` e `dashboard-principal.spec.ts`:
  - `DashboardPrincipalComponent` consome `GET /api/v1/Dashboard/kpis` (`T-033.1`);
  - Exibe skeleton loading nos cards de KPI enquanto carrega (`T-033.4`);
  - Renderiza o gráfico de OS por status (`T-033.2`) e o gráfico de faturamento mensal de 6 meses (`T-033.3`);
  - Renderiza o card "Alertas de Estoque" com links `/estoque/:id` para cada peça crítica (`T-034.2`).
- `npm run build` conclui sem erros de compilação TypeScript ou template Angular.
