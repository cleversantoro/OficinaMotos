---
description: "Task list for feature 021-contas-pagar-form"
---

# Tasks: US-021 — Criar e Editar Conta a Pagar

**Input**: Design documents from `/specs/021-contas-pagar-form/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21 (`financeiro.service.spec.ts`, `contas-pagar-form.spec.ts`, `contas-pagar-lista.spec.ts`).
**Organization**: As tarefas são organizadas por História de Usuário (`US1`, `US2`, `US3`) e mapeadas diretamente para as tasks oficiais do backlog (`T-021.1`, `T-021.2`, `T-021.3`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Foundational — Integração de Endpoints `ContasPagar` no `FinanceiroService` (`T-021.3`)

**Purpose**: Estender o `FinanceiroService` com os métodos de leitura por ID (`GET /{id}`), criação (`POST`) e atualização (`PUT /{id}`) usando `apiPaths.financeiro.pagar` (`/api/v1/FinanceiroContasPagar`), habilitando tanto o modo de criação (`US1`) quanto o modo de edição (`US2`) em `ContasPagarFormComponent`.

- [x] T001 Adicionar os métodos tipados `getContaPagarById(id: number | string)`, `createContaPagar(payload: CreateFinanceiroContaPagarRequest)` e `updateContaPagar(id: number | string, payload: UpdateFinanceiroContaPagarRequest)` em `oficina-motos-web/src/app/core/services/financeiro.service.ts` (`T-021.3`)
- [x] T002 [P] Criar suíte de testes unitários para `FinanceiroService` validando `contasPagar`, `getContaPagarById`, `createContaPagar` e `updateContaPagar` contra `/api/v1/FinanceiroContasPagar` em `oficina-motos-web/src/app/core/services/financeiro.service.spec.ts` (`T-021.3`)

**Checkpoint**: `FinanceiroService` expõe e testa todos os endpoints necessários de `ContasPagar` (`T-021.3`).

---

## Phase 2: User Story 1 & User Story 2 — Criar `ContasPagarFormComponent` (Criação e Edição) e Registrar Rotas (Priority: P1) 🎯 MVP (`T-021.1`, `T-021.2`, `T-021.3`)

**Goal**: Criar o componente standalone unificado `ContasPagarFormComponent` com suporte completo aos modos de criação (`/financeiro/contas-pagar/novo`) e edição (`/financeiro/contas-pagar/:id/editar`), carregamento de fornecedores e métodos de pagamento, validações reativas, submissão para `POST` e `PUT /api/v1/FinanceiroContasPagar`, feedback via `Toast` e registro das rotas em `app.routes.ts`.

**Independent Test**:
- **Criação (`US1`)**: Navegar para `/financeiro/contas-pagar/novo`, validar bloqueio de campos obrigatórios vazios, preencher dados válidos e salvar, verificando chamada `POST /api/v1/FinanceiroContasPagar` e redirecionamento para `/financeiro/contas-pagar`.
- **Edição (`US2`)**: Navegar para `/financeiro/contas-pagar/:id/editar`, verificar preenchimento automático via `GET /api/v1/FinanceiroContasPagar/{id}`, alterar campos e salvar, verificando chamada `PUT /api/v1/FinanceiroContasPagar/{id}` e redirecionamento para `/financeiro/contas-pagar`.

- [x] T003 [US1] [US2] Criar a classe do componente standalone `ContasPagarFormComponent` (`ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule`, Signals `loading`, `submitting`, `error`, `isEditMode`, `contaId`, `fornecedores`, `metodos`, carregamento via `forkJoin`, normalização de datas `YYYY-MM-DD` e submissão para `createContaPagar` / `updateContaPagar`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-form/contas-pagar-form.ts` (`T-021.1`, `T-021.3`)
- [x] T004 [US1] [US2] Criar o template HTML de `ContasPagarFormComponent` com cabeçalho dinâmico (Nova Conta / Editar Conta), estados de carregamento e erro, controles reativos (`descricao`, `fornecedorId`, `valor`, `vencimento`, `status`, `metodoId`, `dataPagamento`, `observacao`), mensagens de validação e botões `"Cancelar"` / `"Salvar"` em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-form/contas-pagar-form.html` (`T-021.1`)
- [x] T005 [P] [US1] [US2] Criar os estilos Dark Theme responsivos do formulário em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-form/contas-pagar-form.scss` (`T-021.1`)
- [x] T006 [P] [US1] [US2] Registrar as rotas `{ path: 'financeiro/contas-pagar/novo', component: ContasPagarFormComponent }` e `{ path: 'financeiro/contas-pagar/:id/editar', component: ContasPagarFormComponent }` dentro do escopo protegido por `authGuard` (`MainLayout`) em `oficina-motos-web/src/app/app.routes.ts` (`T-021.2`)

**Checkpoint**: Rotas `/financeiro/contas-pagar/novo` e `/financeiro/contas-pagar/:id/editar` ativas e integradas aos endpoints `GET /{id}`, `POST` e `PUT /{id}` de `FinanceiroContasPagar`.

---

## Phase 3: User Story 3 — Ações de Criação e Edição na Listagem `/financeiro/contas-pagar` (Priority: P2) (`T-021.2`)

**Goal**: Adicionar o botão **"+ Nova Conta"** no cabeçalho e a coluna de ação **"Editar"** em cada linha da tabela em `ContasPagarListaComponent`, conectando a listagem da `US-020` às novas rotas de criação e edição da `US-021`.

**Independent Test**: Acessar `/financeiro/contas-pagar`, clicar em **"+ Nova Conta"** para ir a `/financeiro/contas-pagar/novo` e clicar em **"Editar"** em uma linha da tabela para ir a `/financeiro/contas-pagar/:id/editar`.

- [x] T007 [P] [US3] Adicionar botão `routerLink="/financeiro/contas-pagar/novo"` no cabeçalho e coluna `"Ações"` com link `[routerLink]="['/financeiro/contas-pagar', conta.id, 'editar']"` na tabela em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.html` e estilizar em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.scss` (`T-021.2`)

---

## Phase 4: Polish, Testes Unitários & Governança

**Purpose**: Garantir cobertura de testes unitários com Vitest para todos os fluxos de criação, edição, rotas e integração com `ContasPagar`, validar o build de produção e atualizar `governance/backlog.md`.

- [x] T008 [P] Criar suíte de testes unitários `contas-pagar-form.spec.ts` cobrindo modo de criação (`POST`), modo de edição (`GET /:id` + `PUT /:id`), validações de campos obrigatórios, tratamento de erro/ID inválido e registro das rotas em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-form/contas-pagar-form.spec.ts`
- [x] T009 [P] Atualizar `contas-pagar-lista.spec.ts` para verificar a renderização do botão `"+ Nova Conta"` (`/financeiro/contas-pagar/novo`) e dos links `"Editar"` (`/financeiro/contas-pagar/:id/editar`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.spec.ts`
- [x] T010 Executar suíte de testes unitários do frontend (`npm test -- --no-watch`) em `oficina-motos-web/`
- [x] T011 Executar compilação de produção (`npm run build`) em `oficina-motos-web/`
- [x] T012 Atualizar o status das tarefas `T-021.1`, `T-021.2` e `T-021.3` para `[x]` em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Foundational — `FinanceiroService`)**: `T001` e `T002 [P]` — iniciam imediatamente; `T001` é pré-requisito para `T003`.
- **Phase 2 (US1 & US2 — `ContasPagarFormComponent` e Rotas)**: `T003` → `T004`, com `T005 [P]` (estilos `.scss`) e `T006 [P]` (`app.routes.ts`) executáveis em paralelo.
- **Phase 3 (US3 — Atalhos em `ContasPagarListaComponent`)**: `T007 [P]` — modifica apenas `contas-pagar-lista.html/.scss`, podendo rodar em paralelo à Phase 2.
- **Phase 4 (Polish, Testes & Governança)**: `T008 [P]` e `T009 [P]` → `T010` → `T011` → `T012`.

### User Story Dependencies

- **US1 (Criar Conta a Pagar) & US2 (Editar Conta a Pagar)**: Compartilham o mesmo componente `ContasPagarFormComponent` (`T-021.1`), as rotas em `app.routes.ts` (`T-021.2`) e os métodos de `FinanceiroService` (`T-021.3`).
- **US3 (Navegação a partir da Listagem)**: Independente da lógica interna do formulário; adiciona os links de entrada na tabela de `ContasPagarListaComponent`.

### Parallel Opportunities

- `T002` (`financeiro.service.spec.ts`), `T005` (`contas-pagar-form.scss`), `T006` (`app.routes.ts`) e `T007` (`contas-pagar-lista.html/.scss`) manipulam arquivos distintos e podem ser executados em paralelo.
- `T008` (`contas-pagar-form.spec.ts`) e `T009` (`contas-pagar-lista.spec.ts`) também podem ser criados/atualizados em paralelo.

---

## Parallel Example: Phase 2 & Phase 3

```bash
# Após T001 (FinanceiroService):
Task T003/T004: "Criar ContasPagarFormComponent em contas-pagar-form.ts e contas-pagar-form.html"
Task T005: "Criar estilos Dark Theme em contas-pagar-form.scss"
Task T006: "Registrar rotas /financeiro/contas-pagar/novo e /financeiro/contas-pagar/:id/editar em app.routes.ts"
Task T007: "Adicionar botão '+ Nova Conta' e coluna 'Editar' em contas-pagar-lista.html/.scss"
```

---

## Implementation Strategy

### MVP First (Phase 1 + Phase 2)

1. Concluir `T001` e `T002` (integração e testes dos endpoints `ContasPagar` em `FinanceiroService`).
2. Concluir `T003` a `T006` (`ContasPagarFormComponent` completo para criação e edição + rotas registradas).
3. Validar criação (`/financeiro/contas-pagar/novo`) e edição (`/financeiro/contas-pagar/:id/editar`).

### Incremental Delivery

1. Conectar a listagem `/financeiro/contas-pagar` com botões de criação e edição (`T007`).
2. Adicionar/atualizar testes unitários Vitest (`T008`, `T009`), validar `npm test -- --no-watch` (`T010`), `npm run build` (`T011`) e marcar a `US-021` como concluída no backlog (`T012`).
