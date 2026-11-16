---
description: "Task list for feature 022-contas-receber-lista"
---

# Tasks: US-022 — Listar Contas a Receber

**Input**: Design documents from `/specs/022-contas-receber-lista/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21 (`contas-receber-lista.spec.ts`).
**Organization**: As tarefas são organizadas por História de Usuário (`US1`, `US2`, `US3`) e mapeadas diretamente para as tasks oficiais do backlog (`T-022.1`, `T-022.2`, `T-022.3`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Foundational — Tipagem de `FinanceiroContaReceber`

**Purpose**: Incluir a propriedade opcional `ordemServicoId?: number | null` na interface `FinanceiroContaReceber` para suportar resolução direta e tipada da OS de origem na coluna `"OS Origem"`.

- [x] T001 Adicionar a propriedade opcional `ordemServicoId?: number | null` em `FinanceiroContaReceber` em `oficina-motos-web/src/app/core/models/financeiro.ts` (`T-022.3`)

**Checkpoint**: Modelo `FinanceiroContaReceber` pronto para consumo em `ContasReceberListaComponent`.

---

## Phase 2: User Story 1 & User Story 2 — Criar `ContasReceberListaComponent`, Coluna "OS Origem" (`/ordens/:id`) e Rota `/financeiro/contas-receber` (Priority: P1) 🎯 MVP (`T-022.1`, `T-022.2`, `T-022.3`)

**Goal**: Criar o componente standalone `ContasReceberListaComponent` com carregamento de contas a receber (`GET /api/v1/FinanceiroContasReceber`), clientes (`ClientesService.list()`) e métodos de pagamento (`FinanceiroService.metodos()`), KPIs resumidos, filtros reativos por status/período/busca, paginação, coluna **"OS Origem"** com link navegável para `/ordens/:id` (`T-022.3`) e registro da rota `/financeiro/contas-receber` (`T-022.2`).

**Independent Test**: Acessar `/financeiro/contas-receber`, verificar que a lista de contas a receber é carregada e paginada, que os KPIs (A receber em aberto, Recebíveis vencidos, Total recebido) são exibidos, que os filtros funcionam e que a coluna **"OS Origem"** renderiza o link `OS #:id` apontando para `/ordens/:id` quando houver OS vinculada (ou `-` para lançamentos avulsos).

- [x] T002 [US1] [US2] Criar a classe do componente standalone `ContasReceberListaComponent` (`ChangeDetectionStrategy.OnPush`, Signals `contas`, `clientes`, `metodos`, `loading`, `error`, `statusFilter`, `dataInicio`, `dataFim`, `searchTerm`, `filteredContas`, `kpis`, paginação e métodos `getOrdemServicoId(conta)`, `getEffectiveStatus(conta)`, `isVencida(conta)`, `getClienteNome(id)` e `getMetodoNome(id)`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.ts` (`T-022.1`, `T-022.3`)
- [x] T003 [US1] [US2] Criar o template HTML de `ContasReceberListaComponent` com cabeçalho, cards de KPIs, barra de filtros (busca, status, período de vencimento), tabela paginada e coluna `"OS Origem"` com link `[routerLink]="['/ordens', osId]"` (ou `-` quando ausente) em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.html` (`T-022.1`, `T-022.3`)
- [x] T004 [P] [US1] [US2] Criar os estilos Dark Theme da listagem de contas a receber e do badge/link `.os-origin-link` em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.scss` (`T-022.1`, `T-022.3`)
- [x] T005 [P] [US1] Registrar a rota `{ path: 'financeiro/contas-receber', component: ContasReceberListaComponent }` dentro do escopo protegido por `authGuard` (`MainLayout`) em `oficina-motos-web/src/app/app.routes.ts` (`T-022.2`)

**Checkpoint**: `/financeiro/contas-receber` ativa, listando contas a receber com filtros, KPIs e coluna `"OS Origem"` vinculada a `/ordens/:id`.

---

## Phase 3: User Story 3 — Navegação a partir do Painel Financeiro `/financeiro` (Priority: P2) (`T-022.2`)

**Goal**: Conectar a aba "Contas a Receber" no painel `/financeiro` (`FinanceiroDashboard`) à nova rota `/financeiro/contas-receber`.

**Independent Test**: Acessar `/financeiro`, selecionar a aba **"Contas a Receber"**, clicar em **"Ver listagem completa"** e confirmar a navegação para `/financeiro/contas-receber`.

- [x] T006 [P] [US3] Adicionar link `<a class="ghost" routerLink="/financeiro/contas-receber">Ver listagem completa</a>` na seção de Contas a Receber em `oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.html` (`T-022.2`)

---

## Phase 4: Polish, Testes Unitários & Governança

**Purpose**: Garantir cobertura de testes unitários com Vitest para todos os critérios de aceite da `US-022`, validar o build de produção e atualizar `governance/backlog.md`.

- [x] T007 [P] Criar suíte de testes unitários `contas-receber-lista.spec.ts` validando listagem (`T-022.1`), registro da rota `/financeiro/contas-receber` (`T-022.2`), coluna `"OS Origem"` com link para `/ordens/:id` (`T-022.3`), filtros e estados de erro em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.spec.ts`
- [x] T008 Executar suíte de testes unitários do frontend (`npm test -- --no-watch`) em `oficina-motos-web/`
- [x] T009 Executar compilação de produção (`npm run build`) em `oficina-motos-web/`
- [x] T010 Atualizar o status das tarefas `T-022.1`, `T-022.2` e `T-022.3` para `[x]` em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Foundational)**: `T001` — inicia imediatamente.
- **Phase 2 (US1 & US2 — `ContasReceberListaComponent`, Coluna "OS Origem" e Rota)**: `T002` → `T003`, com `T004 [P]` (estilos `.scss`) e `T005 [P]` (`app.routes.ts`) executáveis em paralelo.
- **Phase 3 (US3 — Atalho em `FinanceiroDashboard`)**: `T006 [P]` — modifica apenas `financeiro-dashboard.html`, podendo rodar em paralelo à Phase 2.
- **Phase 4 (Polish, Testes & Governança)**: `T007 [P]` → `T008` → `T009` → `T010`.

### User Story Dependencies

- **US1 (Lista e Rota `/financeiro/contas-receber`) & US2 (Coluna "OS Origem" → `/ordens/:id`)**: Compõem a página principal `ContasReceberListaComponent` (MVP).
- **US3 (Navegação a partir de `/financeiro`)**: Independente; adiciona o link de entrada em `FinanceiroDashboard`.

### Parallel Opportunities

- `T004` (`contas-receber-lista.scss`), `T005` (`app.routes.ts`) e `T006` (`financeiro-dashboard.html`) atuam em arquivos distintos e podem ser executadas em paralelo com `T002`/`T003`.

---

## Parallel Example: Phase 2 & Phase 3

```bash
# Enquanto ContasReceberListaComponent (.ts/.html) é construído:
Task T004: "Criar estilos Dark Theme e classe .os-origin-link em contas-receber-lista.scss"
Task T005: "Registrar rota /financeiro/contas-receber em src/app/app.routes.ts"
Task T006: "Adicionar atalho routerLink='/financeiro/contas-receber' em financeiro-dashboard.html"
```

---

## Implementation Strategy

### MVP First (Phase 1 + Phase 2)

1. Concluir `T001` (tipagem de `FinanceiroContaReceber`).
2. Concluir `T002` a `T005` (`ContasReceberListaComponent`, coluna `"OS Origem"` com link `/ordens/:id` e rota `/financeiro/contas-receber`).
3. Validar renderização da lista e funcionamento dos links `/ordens/:id`.

### Incremental Delivery

1. Adicionar atalho na aba Contas a Receber do painel `/financeiro` (`T006`).
2. Executar suíte de testes unitários Vitest (`T007`, `T008`), build de produção (`T009`) e atualização do backlog (`T010`).
