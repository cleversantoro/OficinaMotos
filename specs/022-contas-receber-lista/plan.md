# Plano de Implementação: Listar Contas a Receber (US-022)

**Branch**: `022-contas-receber-lista` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/022-contas-receber-lista/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)
- [`governance/GEMINI.md`](file:///c:/Projetos/OficinaMotos/governance/GEMINI.md)

**Entrada**: US-022 — Listar contas a receber
- **Prioridade**: 🟡 Should | **Estimativa**: S | **Sprint**: 4 | **Depende**: US-001
- **Tasks**:
  - [ ] T-022.1 — Criar `ContasReceberListaComponent`
  - [ ] T-022.2 — Registrar rota `/financeiro/contas-receber`
  - [ ] T-022.3 — Coluna "OS Origem" com link para `/ordens/:id`

---

## Resumo

Esta funcionalidade implementa a página dedicada de listagem de Contas a Receber (`/financeiro/contas-receber`) no módulo Financeiro (`EPIC-04`), incluindo a coluna **"OS Origem"** com link navegável para `/ordens/:id`, filtros reativos por status, período de vencimento e busca textual, além de KPIs resumidos e paginação.

O plano técnico contempla:
1. **Componente `ContasReceberListaComponent` (`T-022.1`)**:
   - Componente Angular 21 Standalone (`ChangeDetectionStrategy.OnPush`) em `src/app/features/financeiro/pages/contas-receber-lista/` com estado gerenciado por Signals (`signal`, `computed`).
   - Consome `FinanceiroService.contasReceber()` (`GET /api/v1/FinanceiroContasReceber`), além de carregar de forma resiliente (`catchError(() => of([]))`) os clientes (`ClientesService.list()`) e métodos de pagamento (`FinanceiroService.metodos()`).
   - Exibe cards de KPIs resumidos (A receber em aberto, Recebíveis vencidos e Total recebido), filtros por status/período/busca, tabela paginada ordenada por vencimento e destaque visual para recebíveis em atraso.
2. **Rota `/financeiro/contas-receber` e Atalho no Painel Financeiro (`T-022.2`)**:
   - Registro da rota `{ path: 'financeiro/contas-receber', component: ContasReceberListaComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts) dentro do layout autenticado (`MainLayout` com `authGuard`).
   - Adição do link `"Ver listagem completa"` (`routerLink="/financeiro/contas-receber"`) na aba "Contas a Receber" de [`financeiro-dashboard.html`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.html).
3. **Coluna "OS Origem" com Link para `/ordens/:id` (`T-022.3`)**:
   - Implementação do método `getOrdemServicoId(conta)` que resolve o ID da OS a partir de `conta.ordemServicoId` (opcional) ou da expressão regular `/\bOS\s*#?\s*(\d+)\b/i` aplicada sobre `conta.descricao` e `conta.observacao` (compatível com os lançamentos automáticos gerados por `OrdemServicoService.RegistrarPagamentoAsync` na `US-012`).
   - Renderização do link `<a class="os-origin-link" [routerLink]="['/ordens', osId]">OS #{{ osId }}</a>` na coluna **"OS Origem"** (ou `-` para contas manuais sem OS vinculada).
4. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de schema ou migration necessária.

---

## Contexto Técnico

**Language/Version**: TypeScript 5.9, Angular 21.0.0 (Standalone Components, Signals, Control Flow `@if`/`@for`)  
**Primary Dependencies**: `@angular/common`, `@angular/forms`, `@angular/router`, `rxjs` 7.8.0  
**Storage**: API REST ASP.NET Core 8 + MySQL 8.0 (`fin_contas_receber`, `fin_metodos_pagamento`, `cad_clientes` via `/api/v1/FinanceiroContasReceber`, `/api/v1/FinanceiroMetodosPagamento`, `/api/v1/Clientes`)  
**Testing**: Vitest (`npm test -- --no-watch`) + build de produção (`npm run build`)  
**Target Platform**: Aplicação Web SPA (Desktop e Mobile responsivo em Dark Theme)  
**Project Type**: Web application (`oficina-motos-web` + `oficina-motos-api`)  
**Performance Goals**: Filtragem e paginação reativas instantâneas (< 50ms em cliente via `computed` Signals)  
**Constraints**: Sem criação de `NgModule`; uso obrigatório de `apiPaths` (`ADR-006`); estilização consistente com `ContasPagarListaComponent`  
**Scale/Scope**: 1 nova página standalone (`ContasReceberListaComponent`), registro de rota em `app.routes.ts`, atalho em `FinanceiroDashboard` e suíte de testes unitários `.spec.ts`

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Respeita o Bounded Context `Financeiro`, realizando apenas leitura de referência de `Clientes` e navegação por rota para `Ordens de Serviço` (`/ordens/:id`). |
| **II. API RESTful Versionada (`ADR-006`)** | ✅ Pass | Consome exclusivamente `/api/v1/FinanceiroContasReceber`, `/api/v1/FinanceiroMetodosPagamento` e `/api/v1/Clientes` via `apiPaths`. |
| **III. Segurança por Design (RBAC)** | ✅ Pass | Rota `/financeiro/contas-receber` protegida por `authGuard` no `MainLayout` e API protegida pela `FallbackPolicy` (`US-001`). |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `ContasReceberListaComponent` é `standalone: true`, com `ChangeDetectionStrategy.OnPush` e estado em Angular Signals (`signal`, `computed`). |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Sem alterações no schema MySQL 8.0 (`fin_contas_receber`); preserva rastreabilidade direta com a OS de origem (`T-022.3`). |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Inclui especificação de testes unitários `contas-receber-lista.spec.ts` cobrindo listagem, rota, coluna "OS Origem" (`/ordens/:id`) e filtros. |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Alinhado com `governance/backlog.md` (`US-022`), `governance/arquitetura.md` e `specs/012-os-pagamento/`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/022-contas-receber-lista/
├── spec.md              # Especificação de requisitos (US-022)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Pesquisa técnica de contratos e resolução de OS Origem
├── data-model.md        # Fase 1: Modelo de dados, extração de OS e regras de status
├── quickstart.md        # Fase 1: Guia rápido de execução e validação
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    ├── api.md           # Fase 1: Contratos REST consumidos pela listagem
    └── ui.md            # Fase 1: Contratos de UI, coluna OS Origem e filtros
```

### Source Code (repository root)

```text
oficina-motos-web/src/app/
├── app.routes.ts                                                                      # [MODIFICAR] Registrar rota financeiro/contas-receber (T-022.2)
├── core/
│   └── models/
│       └── financeiro.ts                                                              # [MODIFICAR] Adicionar ordemServicoId?: number | null opcional em FinanceiroContaReceber
└── features/
    └── financeiro/
        └── pages/
            ├── financeiro-dashboard/
            │   └── financeiro-dashboard.html                                          # [MODIFICAR] Adicionar link "Ver listagem completa" para /financeiro/contas-receber
            └── contas-receber-lista/
                ├── contas-receber-lista.ts                                            # [CRIAR] ContasReceberListaComponent com getOrdemServicoId, filtros e KPIs (T-022.1, T-022.3)
                ├── contas-receber-lista.html                                          # [CRIAR] Template com coluna "OS Origem" -> /ordens/:id, filtros e paginação (T-022.1, T-022.3)
                ├── contas-receber-lista.scss                                          # [CRIAR] Estilos Dark Theme e estilização do badge/link OS Origem
                └── contas-receber-lista.spec.ts                                       # [CRIAR] Testes unitários com Vitest para T-022.1, T-022.2 e T-022.3
```

**Structure Decision**: Segue a estrutura oficial de módulos de negócio (`src/app/features/financeiro/pages/contas-receber-lista/`), espelhando `contas-pagar-lista`.

---

## Fases de Execução

### Fase 1: Modelo e Componente `ContasReceberListaComponent` com Coluna "OS Origem" (`T-022.1`, `T-022.3`)
1. Adicionar `ordemServicoId?: number | null` opcional em `FinanceiroContaReceber` (`src/app/core/models/financeiro.ts`).
2. Criar `ContasReceberListaComponent` em `src/app/features/financeiro/pages/contas-receber-lista/` (`contas-receber-lista.ts`, `.html`, `.scss`).
3. Implementar carregamento via `forkJoin` (`FinanceiroService.contasReceber()`, `ClientesService.list()`, `FinanceiroService.metodos()`), método `getOrdemServicoId(conta)` e renderização da coluna **"OS Origem"** com `[routerLink]="['/ordens', osId]"`.

### Fase 2: Registro da Rota `/financeiro/contas-receber` e Atalho no Painel (`T-022.2`)
1. Registrar `{ path: 'financeiro/contas-receber', component: ContasReceberListaComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).
2. Adicionar link `routerLink="/financeiro/contas-receber"` na aba "Contas a Receber" de [`financeiro-dashboard.html`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.html).

### Fase 3: Testes Unitários, Build e Governança
1. Criar `contas-receber-lista.spec.ts` validando listagem (`T-022.1`), rota `/financeiro/contas-receber` (`T-022.2`), coluna "OS Origem" com link para `/ordens/:id` (`T-022.3`) e filtros.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
