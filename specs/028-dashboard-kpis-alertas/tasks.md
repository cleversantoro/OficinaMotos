---
description: "Task list for feature 028-dashboard-kpis-alertas"
---

# Tasks: US-032, US-033 & US-034 — Endpoint Agregador de KPIs, Dashboard Real com Gráficos e Alertas de Estoque Mínimo

**Input**: Design documents from `/specs/028-dashboard-kpis-alertas/`  
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/dashboard-kpis-contract.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`  
**Tests**: Suítes de testes unitários e de integração xUnit (`DashboardServiceTests`, `DashboardControllerIntegrationTests`) em `oficina-motos-api/src/OficinaMotos.Tests/` (`US-032` & `T-034.1`) e suítes de testes unitários Angular/Vitest (`dashboard.service.spec.ts`, `dashboard-principal.spec.ts`) em `oficina-motos-web/src/app/` (`US-033` & `T-034.2`).  
**Organization**: As tarefas estão organizadas por fases (`Foundational` → `US1` = `US-032` + `T-034.1` → `US2` = `US-033` → `US3` = `US-034` → `Polish`) e mapeadas diretamente para as tasks oficiais do backlog (`T-032.1` a `T-032.5`, `T-033.1` a `T-033.4` e `T-034.1` a `T-034.2`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Foundational — DTOs, Interfaces e Contratos de API do Dashboard

**Purpose**: Criar os contratos de dados do backend (`DashboardKpisResponseDTO`, `DashboardOsStatusCountDTO`, `DashboardFaturamentoMensalDTO`, `DashboardPecaCriticaDTO`), a interface `IDashboardService`, e os modelos/rotas de API do frontend (`dashboard.ts`, `api-paths.ts`).

**⚠️ CRITICAL**: Necessário antes da implementação do `DashboardService` / `DashboardController` (`US1`) e do consumo no Angular (`US2`, `US3`).

- [x] T001 [P] Criar os DTOs `DashboardKpisResponseDTO`, `DashboardOsStatusCountDTO`, `DashboardFaturamentoMensalDTO` e `DashboardPecaCriticaDTO` (incluindo `pecasCriticas`) em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/Dashboard/DashboardKpisResponseDTO.cs` (`T-032.2`, `T-034.1`)
- [x] T002 [P] Criar a interface `IDashboardService` com `Task<DashboardKpisResponseDTO> GetKpisAsync(string? periodo = null)` em `oficina-motos-api/src/OficinaMotos.Application/Interfaces/Dashboard/IDashboardService.cs` (`T-032.2`)
- [x] T003 [P] Criar as interfaces TypeScript `DashboardKpisResponse`, `DashboardOsStatusCount`, `DashboardFaturamentoMensal` e `DashboardPecaCritica` em `oficina-motos-web/src/app/core/models/dashboard.ts` e reexportá-las em `oficina-motos-web/src/app/core/models/index.ts` (`T-033.1`, `T-034.1`)
- [x] T004 [P] Registrar o endpoint `dashboard: { kpis: '/api/v1/Dashboard/kpis' }` em `oficina-motos-web/src/app/core/services/api-paths.ts` (`T-033.1`)

**Checkpoint**: Contratos C# e TypeScript prontos para implementação do backend e frontend em paralelo.

---

## Phase 2: User Story 1 — Criar Endpoint Agregador de KPIs na API (Priority: P1) 🎯 MVP (`US-032`: `T-032.1` a `T-032.5` & `T-034.1`)

**Goal**: Implementar `DashboardService.GetKpisAsync(periodo)` (`T-032.2`) com as queries de OS abertas e do dia (`T-032.3`), faturamento mensal e série dos últimos 6 meses (`T-032.4`), e peças com `Quantidade < EstoqueMinimo` (`qtd_atual < qtd_minima`) incluindo a lista `pecasCriticas` (`T-032.5`, `T-034.1`), expondo o endpoint autenticado `GET /api/v1/Dashboard/kpis` em `DashboardController` (`T-032.1`).

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Dashboard"` em `oficina-motos-api/` e verificar 100% de aprovação em `DashboardServiceTests` e `DashboardControllerIntegrationTests`.

- [x] T005 [US1] Implementar `DashboardService.GetKpisAsync(string? periodo = null)` com as queries de OS abertas, do dia e por status (`T-032.3`), faturamento mensal e histórico de 6 meses (`T-032.4`), e peças com `Quantidade < EstoqueMinimo` populando `PecasAbaixoMinimo` e `PecasCriticas` (`T-032.5`, `T-034.1`) em `oficina-motos-api/src/OficinaMotos.Application/Services/Dashboard/DashboardService.cs` (`T-032.2`)
- [x] T006 [US1] Registrar `services.AddScoped<Interfaces.Dashboard.IDashboardService, Services.Dashboard.DashboardService>()` em `oficina-motos-api/src/OficinaMotos.Application/IoC/DependencyInjection.cs` (`T-032.2`)
- [x] T007 [US1] Criar `DashboardController` decorado com `[ApiController]`, `[Route("api/v1/[controller]")]` e `[Authorize]` expondo `GET /api/v1/Dashboard/kpis` (`[HttpGet("kpis")]`) em `oficina-motos-api/src/OficinaMotos.API/Controllers/Dashboard/DashboardController.cs` (`T-032.1`)
- [x] T008 [P] [US1] Criar `DashboardServiceTests` validando cálculo de OS abertas/do dia/por status (`T-032.3`), faturamento mensal e série de 6 meses (`T-032.4`) e filtro de peças críticas com `Quantidade < EstoqueMinimo` (`T-032.5`, `T-034.1`) em `oficina-motos-api/src/OficinaMotos.Tests/Services/Dashboard/DashboardServiceTests.cs`
- [x] T009 [P] [US1] Criar `DashboardControllerIntegrationTests` validando `401 Unauthorized` sem token (`T-032.1`) e `200 OK` autenticado retornando KPIs e `pecasCriticas` em `oficina-motos-api/src/OficinaMotos.Tests/Integration/DashboardControllerIntegrationTests.cs`

**Checkpoint**: Endpoint `GET /api/v1/Dashboard/kpis` funcional, protegido por JWT e validado por testes unitários e de integração (`US-032` e `T-034.1` completas).

---

## Phase 3: User Story 2 — Substituir Dados do Dashboard por Dados Reais do Endpoint `/api/v1/Dashboard/kpis` (Priority: P1) (`US-033`: `T-033.1` a `T-033.4`)

**Goal**: Criar o `DashboardService` no Angular e atualizar o `DashboardPrincipalComponent` (`DashboardPrincipal`) para consumir `GET /api/v1/Dashboard/kpis` (`T-033.1`), exibir skeleton loading nos cards de KPI (`T-033.4`), renderizar o gráfico de OS por status (`T-033.2`) e o gráfico de faturamento mensal dos últimos 6 meses (`T-033.3`).

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` e verificar a aprovação de `dashboard.service.spec.ts` e `dashboard-principal.spec.ts`.

- [x] T010 [P] [US2] Criar `DashboardService` com método `getKpis(periodo?: string)` consumindo `apiPaths.dashboard.kpis` em `oficina-motos-web/src/app/core/services/dashboard.service.ts` (`T-033.1`)
- [x] T011 [P] [US2] Criar `dashboard.service.spec.ts` validando a chamada `GET /api/v1/Dashboard/kpis` (com e sem parâmetro `periodo`) em `oficina-motos-web/src/app/core/services/dashboard.service.spec.ts` (`T-033.1`)
- [x] T012 [US2] Atualizar `DashboardPrincipalComponent` (e manter alias `DashboardPrincipal`) usando Signals para consumir `DashboardService.getKpis()`, expor estado de `loading` para skeleton loading e estruturar os dados dos gráficos de OS por status e faturamento mensal (6 meses) em `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/dashboard-principal.ts` (`T-033.1`, `T-033.2`, `T-033.3`, `T-033.4`)
- [x] T013 [US2] Atualizar o template e estilos do dashboard para renderizar skeleton loading nos cards de KPI (`T-033.4`), o gráfico de OS por status (`T-033.2`) e o gráfico de faturamento mensal dos últimos 6 meses (`T-033.3`) em `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/dashboard-principal.html` e `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/dashboard-principal.scss`

**Checkpoint**: `DashboardPrincipalComponent` consumindo `GET /api/v1/Dashboard/kpis`, exibindo skeleton loading nos cards e renderizando os gráficos de OS por status e faturamento mensal de 6 meses (`US-033` completa).

---

## Phase 4: User Story 3 — Alerta de Estoque Mínimo no Dashboard (Priority: P2) (`US-034`: `T-034.1` & `T-034.2`)

**Goal**: Exibir no `DashboardPrincipalComponent` o card dedicado **"Alertas de Estoque"** alimentado pela lista `pecasCriticas` do endpoint `/api/v1/Dashboard/kpis` (`T-034.1`), apresentando cada peça abaixo do estoque mínimo com link direto para `/estoque/:id` (`T-034.2`), e validar todos os comportamentos do componente em `dashboard-principal.spec.ts`.

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` e confirmar que `dashboard-principal.spec.ts` valida os links `/estoque/:id` no card "Alertas de Estoque", o skeleton loading e os gráficos.

- [x] T014 [US3] Incluir o card **"Alertas de Estoque"** no `DashboardPrincipalComponent` listando `pecasCriticas` (código, descrição, quantidade atual vs. mínima e déficit) com link `[routerLink]="['/estoque', peca.id]"` (`/estoque/:id`) em `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/dashboard-principal.html` e `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/dashboard-principal.scss` (`T-034.2`)
- [x] T015 [US3] Criar `dashboard-principal.spec.ts` validando o consumo de `GET /api/v1/Dashboard/kpis` (`T-033.1`), o skeleton loading nos cards de KPI (`T-033.4`), a renderização do gráfico de OS por status (`T-033.2`), do gráfico de faturamento mensal de 6 meses (`T-033.3`) e do card "Alertas de Estoque" com links para `/estoque/:id` (`T-034.2`) em `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/dashboard-principal.spec.ts`

**Checkpoint**: Card "Alertas de Estoque" com links `/estoque/:id` funcional e suíte `dashboard-principal.spec.ts` 100% aprovada (`US-034` completa).

---

## Phase 5: Polish, Execução de Testes & Governança

**Purpose**: Validar a compilação e execução completa das suítes de testes do backend (`dotnet test`) e do frontend (`npm test` e `npm run build`), e atualizar o backlog de governança (`governance/backlog.md`).

- [x] T016 Executar `dotnet build OficinaMotos.slnx` e `dotnet test OficinaMotos.slnx` em `oficina-motos-api/` confirmando 100% de aprovação nos testes unitários e de integração
- [x] T017 Executar `npm test -- --watch=false` e `npm run build` em `oficina-motos-web/` confirmando 100% de aprovação em todas as specs e build limpo
- [x] T018 Atualizar o status de `US-032` (`T-032.1`..`T-032.5`), `US-033` (`T-033.1`..`T-033.4`) e `US-034` (`T-034.1`..`T-034.2`) para concluído (`[x]`) em `governance/backlog.md`
