---
description: "Task list for feature 020-contas-pagar-lista"
---

# Tasks: US-020 — Listar Contas a Pagar

**Input**: Design documents from `/specs/020-contas-pagar-lista/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21 (`contas-pagar-lista.spec.ts`).
**Organization**: As tarefas são organizadas por História de Usuário (`US1` a `US4`) e mapeadas diretamente para as tasks oficiais do backlog (`T-020.1`, `T-020.2`, `T-020.3`, `T-020.4`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`, `[US4]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Setup & Foundational (Tipagem do `FinanceiroService`)

**Purpose**: Garantir tipagem explícita em `FinanceiroService` (`FinanceiroContaPagar[]`, `FinanceiroMetodoPagamento[]`) mantendo compatibilidade total com `apiPaths.financeiro.pagar` e `apiPaths.financeiro.metodos`.

- [x] T001 Tipar os retornos de `contasPagar(params?: Record<string, any>)` e `metodos()` em `oficina-motos-web/src/app/core/services/financeiro.service.ts`

**Checkpoint**: `FinanceiroService` pronto para consumo tipado em `ContasPagarListaComponent`.

---

## Phase 2: User Story 1 & User Story 3 - `ContasPagarListaComponent`, Rota `/financeiro/contas-pagar` e Destaque de Contas Vencidas (Priority: P1) 🎯 MVP (`T-020.1`, `T-020.2`, `T-020.4`)

**Goal**: Criar o componente standalone `ContasPagarListaComponent` com carregamento de contas a pagar (`GET /api/v1/FinanceiroContasPagar`), fornecedores e métodos de pagamento, KPIs resumidos, paginação, registro da rota `/financeiro/contas-pagar` e destaque visual imediato para contas vencidas (`.row-vencida`, `.vencimento-atrasado`, `.status-vencido`).

**Independent Test**: Acessar `/financeiro/contas-pagar`, verificar que a lista de contas a pagar é carregada e paginada, que os KPIs (Em Aberto, Vencidas, Pago) são calculados e que contas em aberto com vencimento anterior a hoje ou status `VENCIDO` recebem a classe `.row-vencida` e o badge **"Vencido"**.

- [x] T002 [US1] [US3] Criar a classe do componente standalone `ContasPagarListaComponent` (`ChangeDetectionStrategy.OnPush`, Signals `contas`, `fornecedores`, `metodos`, `loading`, `error`, `kpis`, paginação e métodos `getEffectiveStatus(conta)`, `isVencida(conta)`, `getFornecedorNome(id)` e `getMetodoNome(id)`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.ts` (`T-020.1`, `T-020.4`)
- [x] T003 [US1] [US3] Criar o template HTML de `ContasPagarListaComponent` com cabeçalho, cards de KPIs, tabela paginada, estados de carregamento/erro/vazio e destaque visual para contas vencidas (`[class.row-vencida]="isVencida(c)"`, indicador `"Em atraso"` e badge `.status-vencido`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.html` (`T-020.1`, `T-020.4`)
- [x] T004 [P] [US1] [US3] Criar os estilos Dark Theme e classes de destaque visual de contas vencidas (`.row-vencida`, `.vencimento-atrasado`, `.status-vencido`, `.status-pendente`, `.status-pago`, `.status-cancelado`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.scss` (`T-020.1`, `T-020.4`)
- [x] T005 [P] [US1] Registrar a rota `{ path: 'financeiro/contas-pagar', component: ContasPagarListaComponent }` dentro do escopo protegido por `authGuard` (`MainLayout`) em `oficina-motos-web/src/app/app.routes.ts` (`T-020.2`)

**Checkpoint**: `/financeiro/contas-pagar` funcional, listando contas a pagar com KPIs, paginação e destaque visual de contas vencidas.

---

## Phase 3: User Story 2 - Filtros por Status, Período de Vencimento e Busca Textual (Priority: P1) (`T-020.3`)

**Goal**: Adicionar filtragem reativa por status (`TODOS`, `PENDENTE`, `VENCIDO`, `PAGO`, `CANCELADO`), por período de vencimento (`dataInicio` e `dataFim`) e por busca textual (descrição, observação ou fornecedor), com reinício automático de paginação e botão **"Limpar filtros"**.

**Independent Test**: Em `/financeiro/contas-pagar`, selecionar o status `VENCIDO` (ou `PENDENTE` / `PAGO`), informar `dataInicio` e `dataFim` e digitar um termo de busca; verificar que `filteredContas()` exibe apenas os registros compatíveis e que clicar em **"Limpar filtros"** restaura a listagem completa.

- [x] T006 [US2] Implementar os Signals de filtro (`statusFilter`, `dataInicio`, `dataFim`, `searchTerm`), o `computed` `filteredContas` (filtrando por status efetivo, intervalo `[dataInicio, dataFim]` em `YYYY-MM-DD` e termo de busca) e os métodos de atualização e `limparFiltros()` em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.ts` (`T-020.3`)
- [x] T007 [US2] Adicionar a barra de controles de filtro (busca textual, seletor de status, inputs de data inicial/final e botão `"Limpar filtros"`) e mensagem de estado vazio para filtros sem resultado em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.html` e `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.scss` (`T-020.3`)

**Checkpoint**: Filtros combinados por status, período de vencimento e busca funcionando reativamente em `/financeiro/contas-pagar`.

---

## Phase 4: User Story 4 - Navegação a partir do Painel Financeiro `/financeiro` (Priority: P2) (`T-020.2`)

**Goal**: Conectar o painel `/financeiro` (`FinanceiroDashboard`) à nova rota `/financeiro/contas-pagar` por meio de um atalho direto no cabeçalho da aba de Contas a Pagar.

**Independent Test**: Acessar `/financeiro`, clicar no botão/link **"Ver listagem completa"** na seção de Contas a Pagar e verificar a navegação para `/financeiro/contas-pagar`.

- [x] T008 [P] [US4] Importar `RouterLink` em `FinanceiroDashboard` e adicionar link/botão `routerLink="/financeiro/contas-pagar"` no cabeçalho de Contas a Pagar em `oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.ts` e `oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.html` (`T-020.2`)

---

## Phase 5: Polish, Testes Unitários & Governança

**Purpose**: Garantir cobertura de testes unitários com Vitest para todos os critérios de aceite da `US-020`, validar o build de produção e atualizar `governance/backlog.md`.

- [x] T009 [P] Criar suíte de testes unitários `contas-pagar-lista.spec.ts` validando listagem (`T-020.1`), registro da rota `/financeiro/contas-pagar` (`T-020.2`), filtros por status e período (`T-020.3`) e destaque visual de contas vencidas (`T-020.4`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.spec.ts`
- [x] T010 Executar suíte de testes unitários do frontend (`npm test -- --no-watch`) em `oficina-motos-web/`
- [x] T011 Executar compilação de produção (`npm run build`) em `oficina-motos-web/`
- [x] T012 Atualizar o status das tarefas `T-020.1`, `T-020.2`, `T-020.3` e `T-020.4` para `[x]` em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup/Foundational)**: `T001` — sem dependências externas; inicia imediatamente.
- **Phase 2 (US1 & US3 - Lista, Rota e Destaque de Vencidas)**: `T002` → `T003`, com `T004 [P]` e `T005 [P]` executáveis em paralelo.
- **Phase 3 (US2 - Filtros por Status e Período)**: `T006` → `T007` (estende `ContasPagarListaComponent` criado na Phase 2).
- **Phase 4 (US4 - Atalho no Painel Financeiro)**: `T008 [P]` — toca apenas `financeiro-dashboard.*`, podendo rodar em paralelo às Phases 2 e 3.
- **Phase 5 (Polish, Testes & Governança)**: `T009` → `T010` → `T011` → `T012`.

### User Story Dependencies

- **US1 (Lista e Rota `/financeiro/contas-pagar`) & US3 (Destaque de Contas Vencidas)**: Base da página (MVP).
- **US2 (Filtros por Status e Período)**: Conecta-se diretamente à listagem e à regra de status efetivo de US1/US3.
- **US4 (Navegação a partir de `/financeiro`)**: Independente; adiciona o link de entrada em `FinanceiroDashboard`.

### Parallel Opportunities

- `T004` (estilos `.scss`), `T005` (`app.routes.ts`) e `T008` (`financeiro-dashboard.*`) manipulam arquivos distintos e podem ser executadas em paralelo com `T002`/`T003`.

---

## Parallel Example: Phase 2 & Phase 4

```bash
# Enquanto ContasPagarListaComponent (.ts/.html) é construído:
Task T004: "Criar estilos Dark Theme e classes .row-vencida / .status-vencido em contas-pagar-lista.scss"
Task T005: "Registrar rota /financeiro/contas-pagar em src/app/app.routes.ts"
Task T008: "Adicionar atalho routerLink='/financeiro/contas-pagar' em financeiro-dashboard.ts/.html"
```

---

## Implementation Strategy

### MVP First (Phase 1 + Phase 2)

1. Concluir `T001` (`FinanceiroService` tipado).
2. Concluir `T002` a `T005` (`ContasPagarListaComponent`, destaque de vencidas e rota `/financeiro/contas-pagar`).
3. Validar renderização da lista e destaque de contas vencidas.

### Incremental Delivery

1. Adicionar filtros reativos por status, período e busca (`T006`, `T007`).
2. Adicionar navegação integrada no painel `/financeiro` (`T008`).
3. Executar suíte de testes unitários Vitest, build de produção e atualização do backlog (`T009` a `T012`).
