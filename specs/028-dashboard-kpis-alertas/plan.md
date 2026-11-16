# Plano de Implementação: Endpoint Agregador de KPIs, Dashboard Real com Gráficos e Alertas de Estoque Mínimo (US-032, US-033 & US-034)

**Branch**: `028-dashboard-kpis-alertas` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///C:/Projetos/OficinaMotos/specs/028-dashboard-kpis-alertas/spec.md)  
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///C:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///C:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///C:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///C:/Projetos/OficinaMotos/governance/inventario.md)

**Entrada**:
- **US-032 — Criar endpoint agregador de KPIs** (`Prioridade: 🟢 Could | Estimativa: M | Sprint: 6 | Depende: US-001, US-006`)
  - [ ] T-032.1 — Criar `DashboardController` com `[Authorize]`
  - [ ] T-032.2 — Criar `DashboardService.GetKpisAsync(periodo)`
  - [ ] T-032.3 — Query OS abertas e do dia
  - [ ] T-032.4 — Query faturamento mensal
  - [ ] T-032.5 — Query peças com `qtd_atual < qtd_minima`
- **US-033 — Substituir dados mockados do dashboard por dados reais** (`Prioridade: 🟢 Could | Estimativa: M | Sprint: 6 | Depende: US-032`)
  - [ ] T-033.1 — `DashboardPrincipalComponent` consome `GET /api/v1/Dashboard/kpis`
  - [ ] T-033.2 — Gráfico de OS por status
  - [ ] T-033.3 — Gráfico de faturamento mensal (6 meses)
  - [ ] T-033.4 — Skeleton loading nos cards de KPI
- **US-034 — Alerta de estoque mínimo no dashboard** (`Prioridade: 🟢 Could | Estimativa: S | Sprint: 6 | Depende: US-032`)
  - [ ] T-034.1 — Incluir `pecasCriticas` no response de `/Dashboard/kpis`
  - [ ] T-034.2 — Card "Alertas de Estoque" com link para `/estoque/:id`

---

## Summary

Este plano estrutura a implementação das histórias **US-032**, **US-033** e **US-034** do **EPIC-06 — Dashboard Real e Funcionalidades Avançadas** (Sprint 6):
1. **Backend (`US-032` & `T-034.1`)**: Criação dos DTOs de resposta do Dashboard (`DashboardKpisResponseDTO`, `DashboardOsStatusCountDTO`, `DashboardFaturamentoMensalDTO`, `DashboardPecaCriticaDTO`), da interface `IDashboardService` e da implementação `DashboardService.GetKpisAsync(periodo)` (`T-032.2`) consolidando as queries de OS abertas, do dia e por status (`T-032.3`), faturamento mensal do período e série dos últimos 6 meses (`T-032.4`), e peças críticas com `Quantidade < EstoqueMinimo` (`T-032.5`, `T-034.1`), expostos por `DashboardController` (`GET /api/v1/Dashboard/kpis`) protegido com `[Authorize]` (`T-032.1`).
2. **Frontend (`US-033` & `T-034.2`)**: Registro de `apiPaths.dashboard.kpis` (`/api/v1/Dashboard/kpis`), criação de `DashboardService` (`core/services/dashboard.service.ts`) e atualização de `DashboardPrincipalComponent` (`DashboardPrincipal`) para consumir `GET /api/v1/Dashboard/kpis` (`T-033.1`), exibir skeleton loading nos cards de KPI (`T-033.4`), renderizar o gráfico de OS por status (`T-033.2`), o gráfico de faturamento mensal dos últimos 6 meses (`T-033.3`) e o card **"Alertas de Estoque"** com links diretos para `/estoque/:id` (`T-034.2`).

---

## Technical Context

**Language/Version**: C# 12 / .NET 8.0 (`net8.0`) no backend; TypeScript 5.9 / Angular 21.0 no frontend  
**Primary Dependencies**:
- Backend: ASP.NET Core Web API, Entity Framework Core 8.0, xUnit 2.9.3, Moq 4.20.72, FluentAssertions 6.12.2
- Frontend: Angular 21 Standalone Components + Signals, `@angular/router`, `@angular/common/http`, Vitest 4.0.8  
**Storage**: MySQL 8+ (produção/desenvolvimento) e EF Core InMemory (testes de integração) via `IOrdemServicoRepository` e `IEstoquePecaRepository`  
**Testing**: `dotnet test` (backend) e `npm test -- --watch=false` (frontend)  
**Target Platform**: Web Application (API RESTful .NET 8 + SPA Angular 21)  
**Project Type**: Full-stack Web Application (`oficina-motos-api` + `oficina-motos-web`)  
**Performance Goals**: Resposta de `GET /api/v1/Dashboard/kpis` em uma única viagem HTTP com renderização imediata de skeleton loading nos cards de KPI  
**Constraints**: Manter compatibilidade com `OrdemServicoStatus` (`US-006`), exigir autenticação JWT (`[Authorize]`, `US-001`), centralizar URL em `api-paths.ts` e preservar `strict: true` no Angular  
**Scale/Scope**: 1 novo Controller, 1 novo Service + Interface + DTOs na camada Application, 2 suítes de testes backend (unitário + integração), 1 novo model + service + spec no frontend e refatoração completa de `DashboardPrincipalComponent` + spec

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | `DashboardController` apenas delega para `IDashboardService.GetKpisAsync(periodo)`; toda agregação reside na camada `Application` consumindo repositórios de leitura. |
| **II. API RESTful Versionada** | ✅ Pass | Endpoint versionado em `/api/v1/Dashboard/kpis` e registrado centralmente em `apiPaths.dashboard.kpis` no arquivo `api-paths.ts`. |
| **III. Segurança por Design (RBAC + LGPD)** | ✅ Pass | `DashboardController` protegido por `[Authorize]` (`T-032.1`), rejeitando acessos não autenticados com `401 Unauthorized`. |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `DashboardPrincipalComponent` é Standalone (`standalone: true`) e gerencia seu estado reativo (`loading`, `kpisData`) com Angular Signals. |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | As queries respeitam os filtros globais de soft delete (`!IsDeleted`) e o enum fortemente tipado `OrdemServicoStatus` (`US-006`). |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Inclui testes unitários e de integração no backend (`DashboardServiceTests`, `DashboardControllerIntegrationTests`) e specs unitárias no frontend (`dashboard.service.spec.ts`, `dashboard-principal.spec.ts`). |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Atende integralmente às tasks `T-032.1`..`T-032.5`, `T-033.1`..`T-033.4` e `T-034.1`..`T-034.2` de `governance/backlog.md`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/028-dashboard-kpis-alertas/
├── spec.md              # Especificação de requisitos (US-032, US-033 e US-034)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Decisões técnicas de agregação de KPIs e UI do Dashboard
├── data-model.md        # Fase 1: DTOs C# e interfaces TypeScript do Dashboard
├── quickstart.md        # Fase 1: Guia rápido de validação e testes
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    └── dashboard-kpis-contract.md # Fase 1: Contrato HTTP e de componente
```

### Source Code (repository root)

```text
oficina-motos-api/
└── src/
    ├── OficinaMotos.API/
    │   └── Controllers/
    │       └── Dashboard/
    │           └── DashboardController.cs                                 # [CRIAR] Endpoint GET /api/v1/Dashboard/kpis com [Authorize] (T-032.1)
    ├── OficinaMotos.Application/
    │   ├── DTOs/
    │   │   └── Responses/
    │   │       └── Dashboard/
    │   │           └── DashboardKpisResponseDTO.cs                        # [CRIAR] DTOs agregadores incluindo pecasCriticas, osPorStatus e faturamentoUltimosSeisMeses (T-032.2..T-032.5, T-034.1)
    │   ├── Interfaces/
    │   │   └── Dashboard/
    │   │       └── IDashboardService.cs                                   # [CRIAR] Contrato GetKpisAsync(string? periodo = null) (T-032.2)
    │   ├── IoC/
    │   │   └── DependencyInjection.cs                                     # [MODIFICAR] Registrar IDashboardService -> DashboardService (T-032.2)
    │   └── Services/
    │       └── Dashboard/
    │           └── DashboardService.cs                                    # [CRIAR] Implementação das queries de OS abertas/do dia, faturamento mensal (6m) e peças críticas (T-032.2..T-032.5, T-034.1)
    └── OficinaMotos.Tests/
        ├── Integration/
        │   └── DashboardControllerIntegrationTests.cs                     # [CRIAR] Testes de integração HTTP para /api/v1/Dashboard/kpis (401 e 200 OK com KPIs)
        └── Services/
            └── Dashboard/
                └── DashboardServiceTests.cs                               # [CRIAR] Testes unitários de GetKpisAsync(periodo)

oficina-motos-web/
└── src/
    └── app/
        ├── core/
        │   ├── models/
        │   │   ├── dashboard.ts                                           # [CRIAR] Interfaces TypeScript de DashboardKpisResponse e PecasCriticas
        │   │   └── index.ts                                               # [MODIFICAR] Exportar models de dashboard
        │   └── services/
        │       ├── api-paths.ts                                           # [MODIFICAR] Adicionar dashboard.kpis: '/api/v1/Dashboard/kpis' (T-033.1)
        │       ├── dashboard.service.ts                                   # [CRIAR] Serviço Angular consumindo GET /api/v1/Dashboard/kpis (T-033.1)
        │       └── dashboard.service.spec.ts                              # [CRIAR] Testes unitários do DashboardService
        └── features/
            └── dashboard/
                └── pages/
                    └── dashboard-principal/
                        ├── dashboard-principal.ts                         # [MODIFICAR] DashboardPrincipalComponent consumindo /api/v1/Dashboard/kpis com Signals (T-033.1..T-033.4, T-034.2)
                        ├── dashboard-principal.html                       # [MODIFICAR] Skeleton loading nos cards, gráfico OS por status, gráfico faturamento 6m e card Alertas de Estoque com link /estoque/:id
                        ├── dashboard-principal.scss                       # [MODIFICAR] Estilos de shimmer skeleton, gráficos e card de alertas de estoque
                        └── dashboard-principal.spec.ts                    # [CRIAR] Testes unitários cobrindo T-033.1..T-033.4 e T-034.2
```

**Structure Decision**: Segue a separação de camadas existente em `oficina-motos-api` (`API/Controllers/Dashboard`, `Application/Services/Dashboard`, `Application/DTOs/Responses/Dashboard`) e a estrutura modular do Angular em `oficina-motos-web/src/app/` (`core/services/dashboard.service.ts` e `features/dashboard/pages/dashboard-principal/`).

---

## Fases de Execução

### Fase 1: Endpoint Agregador de KPIs no Backend (`US-032` & `T-034.1`)
1. Criar `DashboardKpisResponseDTO.cs` (incluindo `DashboardOsStatusCountDTO`, `DashboardFaturamentoMensalDTO` e `DashboardPecaCriticaDTO` para `pecasCriticas`).
2. Criar `IDashboardService.cs` e `DashboardService.cs` com `GetKpisAsync(string? periodo = null)` implementando:
   - Query de OS abertas, OS do dia e agrupamento de OS por status (`T-032.3`);
   - Query de faturamento mensal do período e histórico dos últimos 6 meses (`T-032.4`);
   - Query de peças com `Quantidade < EstoqueMinimo` (`qtd_atual < qtd_minima`) populando `pecasAbaixoMinimo` e `pecasCriticas` (`T-032.5`, `T-034.1`).
3. Registrar `IDashboardService` em `OficinaMotos.Application/IoC/DependencyInjection.cs`.
4. Criar `DashboardController.cs` com `[Authorize]` e `[HttpGet("kpis")]` (`T-032.1`).
5. Criar testes automatizados `DashboardServiceTests.cs` e `DashboardControllerIntegrationTests.cs`.

### Fase 2: Consumo Real, Gráficos, Skeleton Loading e Alertas de Estoque no Frontend (`US-033` & `US-034`)
1. Criar `src/app/core/models/dashboard.ts`, exportar em `src/app/core/models/index.ts` e adicionar `dashboard.kpis` em `src/app/core/services/api-paths.ts`.
2. Criar `src/app/core/services/dashboard.service.ts` e `dashboard.service.spec.ts`.
3. Atualizar `DashboardPrincipalComponent` (`dashboard-principal.ts`, `.html`, `.scss`) para:
   - Consumir `GET /api/v1/Dashboard/kpis` (`T-033.1`);
   - Exibir skeleton loading nos cards de KPI enquanto carrega (`T-033.4`);
   - Renderizar gráfico de OS por status (`T-033.2`);
   - Renderizar gráfico de faturamento mensal dos últimos 6 meses (`T-033.3`);
   - Renderizar card **"Alertas de Estoque"** listando `pecasCriticas` com link `[routerLink]="['/estoque', peca.id]"` (`T-034.2`).
4. Criar `dashboard-principal.spec.ts` validando todos os requisitos de UI e integração com `DashboardService`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
