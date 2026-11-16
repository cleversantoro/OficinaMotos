# Plano de Implementação: Listar Contas a Pagar (US-020)

**Branch**: `020-contas-pagar-lista` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/020-contas-pagar-lista/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)
- [`governance/GEMINI.md`](file:///c:/Projetos/OficinaMotos/governance/GEMINI.md)

**Entrada**: US-020 — Listar contas a pagar
- **Prioridade**: 🟡 Should | **Estimativa**: S | **Sprint**: 4 | **Depende**: US-001
- **Tasks**:
  - [ ] T-020.1 — Criar `ContasPagarListaComponent`
  - [ ] T-020.2 — Registrar rota `/financeiro/contas-pagar`
  - [ ] T-020.3 — Filtros por status e período
  - [ ] T-020.4 — Destaque visual para contas vencidas

---

## Resumo

Esta funcionalidade implementa a página dedicada de listagem de Contas a Pagar (`/financeiro/contas-pagar`) no módulo Financeiro (`EPIC-04`), com filtragem reativa por status, período de vencimento e busca textual, além de destaque visual imediato para obrigações vencidas.

O plano técnico contempla:
1. **Componente `ContasPagarListaComponent` (`T-020.1`)**:
   - Componente Angular 21 Standalone (`ChangeDetectionStrategy.OnPush`) com estado gerenciado por Signals (`signal`, `computed`).
   - Consome `FinanceiroService.contasPagar()` (`GET /api/v1/FinanceiroContasPagar`), além de carregar de forma resiliente (`catchError(() => of([]))`) os fornecedores (`FornecedoresService.list()`) e métodos de pagamento (`FinanceiroService.metodos()`) para resolver nomes legíveis na listagem.
   - Exibe cards de KPIs resumidos (Total em Aberto, Total Vencido com alerta e Total Pago), tabela paginada ordenada por vencimento, e estados de carregamento, erro (com botão "Tentar novamente") e lista vazia.
2. **Rota `/financeiro/contas-pagar` e Navegação Integrada (`T-020.2`)**:
   - Registro da rota `{ path: 'financeiro/contas-pagar', component: ContasPagarListaComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts) dentro do layout autenticado (`MainLayout` com `authGuard`).
   - Adição de link/atalho direto no painel [`FinanceiroDashboard`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.html) para navegar até `/financeiro/contas-pagar`.
3. **Filtros por Status, Período de Vencimento e Busca Textual (`T-020.3`)**:
   - Filtro por **Status** (`Todos`, `PENDENTE`, `VENCIDO`, `PAGO`, `CANCELADO`), normalizando valores de banco (`'PENDENTE'`, `'PAGO'`, `'CANCELADO'`, `'vencido'`) de maneira *case-insensitive*.
   - Filtro por **Período de Vencimento** com campos de data inicial (`dataInicio`) e data final (`dataFim`) comparando inclusivamente o dia civil de `vencimento` (`YYYY-MM-DD`).
   - Busca textual por descrição ou nome/ID do fornecedor e botão **"Limpar filtros"** que restaura todos os filtros e reinicia a paginação (`first = 0`).
4. **Destaque Visual para Contas Vencidas (`T-020.4`)**:
   - Função utilitária/método `isVencida(conta)` que identifica contas não pagas e não canceladas cujo `status` seja `'VENCIDO'` ou cuja data de `vencimento` seja anterior à data local de hoje (`vencimentoDate < hoje`).
   - Aplicação de classe de alerta na linha da tabela (`row-vencida`), ícone/indicador de atraso junto à data de vencimento (`vencimento-atrasado`) e badge de status **"Vencido"** com estilização vermelha de alerta (`status-vencido`).
5. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de schema ou migration necessária (a tabela `fin_contas_pagar` e o controller `FinanceiroContasPagarController` já existem e atendem integralmente ao contrato).

---

## Contexto Técnico

**Language/Version**: TypeScript 5.9, Angular 21.0.0 (Standalone Components, Signals, Control Flow `@if`/`@for`)  
**Primary Dependencies**: `@angular/common`, `@angular/forms`, `@angular/router`, `rxjs` 7.8.0  
**Storage**: API REST ASP.NET Core 8 + MySQL 8.0 (`fin_contas_pagar`, `fin_metodos_pagamento`, `cad_fornecedores` via endpoints `/api/v1/FinanceiroContasPagar`, `/api/v1/FinanceiroMetodosPagamento`, `/api/v1/Fornecedores`)  
**Testing**: Vitest (`npm test -- --no-watch`) + build de produção (`npm run build`)  
**Target Platform**: Aplicação Web SPA (Desktop e Mobile responsivo em Dark Theme)  
**Project Type**: Web application (`oficina-motos-web` + `oficina-motos-api`)  
**Performance Goals**: Filtragem e paginação reativas instantâneas (< 50ms em cliente via `computed` Signals)  
**Constraints**: Sem criação de `NgModule`; uso obrigatório de `apiPaths` (`ADR-006`); estilização consistente com o Dark Theme de `FinanceiroDashboard`  
**Scale/Scope**: 1 nova página standalone (`ContasPagarListaComponent`), registro de rota em `app.routes.ts`, atalho em `FinanceiroDashboard` e suíte de testes unitários `.spec.ts`

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Respeita o Bounded Context `Financeiro`, realizando apenas leitura de referência de `Fornecedores` para exibição de nome na UI. |
| **II. API RESTful Versionada (`ADR-006`)** | ✅ Pass | Consome exclusivamente `/api/v1/FinanceiroContasPagar`, `/api/v1/FinanceiroMetodosPagamento` e `/api/v1/Fornecedores` via `apiPaths` em `api-paths.ts`. |
| **III. Segurança por Design (RBAC)** | ✅ Pass | Rota `/financeiro/contas-pagar` protegida por `authGuard` no `MainLayout` e controller backend protegido por `[Authorize]`. |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `ContasPagarListaComponent` é `standalone: true`, com `ChangeDetectionStrategy.OnPush` e estado em Angular Signals (`signal`, `computed`). |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Sem alterações no schema MySQL 8.0 (`fin_contas_pagar`); nenhuma migration SQL adicional necessária. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Inclui especificação de testes unitários `contas-pagar-lista.spec.ts` cobrindo listagem, filtros de status/período e destaque de contas vencidas. |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Alinhado com `governance/backlog.md` (`US-020`), `governance/arquitetura.md` e o protótipo `oficina-motos-docs/pages/financeiro.html`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/020-contas-pagar-lista/
├── spec.md              # Especificação de requisitos (US-020)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Pesquisa técnica de contratos, filtros e status
├── data-model.md        # Fase 1: Modelo de dados, normalização de status e regras de filtro
├── quickstart.md        # Fase 1: Guia rápido de execução e validação
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    ├── api.md           # Fase 1: Contratos REST consumidos pela listagem
    └── ui.md            # Fase 1: Contratos de UI, filtros e classes de destaque visual
```

### Source Code (repository root)

```text
oficina-motos-web/src/app/
├── app.routes.ts                                                                      # [MODIFICAR] Registrar rota financeiro/contas-pagar (T-020.2)
└── features/
    └── financeiro/
        └── pages/
            ├── financeiro-dashboard/
            │   ├── financeiro-dashboard.ts                                            # [MODIFICAR] Importar RouterLink para navegação a /financeiro/contas-pagar
            │   └── financeiro-dashboard.html                                          # [MODIFICAR] Adicionar link/botão "Ver listagem completa" para /financeiro/contas-pagar
            └── contas-pagar-lista/
                ├── contas-pagar-lista.ts                                              # [CRIAR] ContasPagarListaComponent com filtros e destaque de vencidas (T-020.1, T-020.3, T-020.4)
                ├── contas-pagar-lista.html                                            # [CRIAR] Template com KPIs, barra de filtros (status/período/busca) e tabela paginada
                ├── contas-pagar-lista.scss                                            # [CRIAR] Estilos Dark Theme e destaque visual (.row-vencida, .status-vencido)
                └── contas-pagar-lista.spec.ts                                         # [CRIAR] Testes unitários com Vitest para T-020.1, T-020.2, T-020.3 e T-020.4
```

**Structure Decision**: Segue a estrutura oficial de módulos de negócio (`src/app/features/financeiro/pages/contas-pagar-lista/`) definida na Constituição e em `governance/arquitetura.md`, mantendo `FinanceiroService` e `FornecedoresService` em `src/app/core/services/`.

---

## Fases de Execução

### Fase 1: Componente `ContasPagarListaComponent` e Destaque de Vencidas (`T-020.1`, `T-020.4`)
1. Criar `ContasPagarListaComponent` em `src/app/features/financeiro/pages/contas-pagar-lista/` (`contas-pagar-lista.ts`, `.html`, `.scss`).
2. Carregar contas a pagar via `FinanceiroService.contasPagar()`, fornecedores via `FornecedoresService.list()` e métodos via `FinanceiroService.metodos()`.
3. Implementar a detecção de conta vencida (`isVencida(conta)`) e normalização de status (`getEffectiveStatus(conta)`), aplicando destaque visual na linha (`row-vencida`), na data de vencimento e no badge de status (`status-vencido`).

### Fase 2: Filtros por Status, Período de Vencimento e Busca (`T-020.3`)
1. Implementar Signals de filtro em `ContasPagarListaComponent`:
   - `statusFilter`: `'TODOS' | 'PENDENTE' | 'VENCIDO' | 'PAGO' | 'CANCELADO'`
   - `dataInicio`: `string` (`YYYY-MM-DD`)
   - `dataFim`: `string` (`YYYY-MM-DD`)
   - `searchTerm`: `string`
2. Implementar `filteredContas = computed(...)` combinando status efetivo, intervalo `[dataInicio, dataFim]` sobre `conta.vencimento` e termo de busca (descrição ou nome do fornecedor), além de `limparFiltros()`.

### Fase 3: Registro da Rota `/financeiro/contas-pagar` e Atalho no Dashboard (`T-020.2`)
1. Registrar `{ path: 'financeiro/contas-pagar', component: ContasPagarListaComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).
2. Adicionar botão/link de navegação para `/financeiro/contas-pagar` em [`financeiro-dashboard.html`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.html).

### Fase 4: Testes Unitários, Build e Governança
1. Criar `contas-pagar-lista.spec.ts` validando renderização, filtros por status e período, destaque visual de contas vencidas e paginação.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
