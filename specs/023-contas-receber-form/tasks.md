---
description: "Task list for feature 023-contas-receber-form"
---

# Tasks: US-023 — Criar e Editar Conta a Receber Manual

**Input**: Design documents from `/specs/023-contas-receber-form/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21 (`financeiro.service.spec.ts`, `contas-receber-form.spec.ts`, `contas-receber-lista.spec.ts`).
**Organization**: As tarefas são organizadas por História de Usuário (`US1`, `US2`, `US3`) e mapeadas diretamente para as tasks oficiais do backlog (`T-023.1`, `T-023.2`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Foundational — Integração de Endpoints `ContasReceber` no `FinanceiroService` (`T-023.1`)

**Purpose**: Estender o `FinanceiroService` com os métodos de leitura por ID (`GET /{id}`), criação (`POST`) e atualização (`PUT /{id}`) usando `apiPaths.financeiro.receber` (`/api/v1/FinanceiroContasReceber`), habilitando tanto o modo de criação (`US1`) quanto o modo de edição (`US2`) em `ContasReceberFormComponent`.

- [x] T001 Adicionar os métodos tipados `getContaReceberById(id: number | string)`, `createContaReceber(payload: CreateFinanceiroContaReceberRequest)` e `updateContaReceber(id: number | string, payload: UpdateFinanceiroContaReceberRequest)` em `oficina-motos-web/src/app/core/services/financeiro.service.ts` (`T-023.1`)
- [x] T002 [P] Atualizar a suíte de testes unitários de `FinanceiroService` validando `contasReceber`, `getContaReceberById`, `createContaReceber` e `updateContaReceber` contra `/api/v1/FinanceiroContasReceber` em `oficina-motos-web/src/app/core/services/financeiro.service.spec.ts` (`T-023.1`)

**Checkpoint**: `FinanceiroService` expõe e testa todos os endpoints necessários de `ContasReceber` (`T-023.1`).

---

## Phase 2: User Story 1 & User Story 2 — Criar `ContasReceberFormComponent` (Criação e Edição) e Registrar Rotas (Priority: P1) 🎯 MVP (`T-023.1`, `T-023.2`)

**Goal**: Criar o componente standalone unificado `ContasReceberFormComponent` com suporte completo aos modos de criação (`/financeiro/contas-receber/novo`) e edição (`/financeiro/contas-receber/:id/editar`), carregamento de clientes e métodos de pagamento, validações reativas, submissão para `POST` e `PUT /api/v1/FinanceiroContasReceber`, feedback via `Toast` e registro das rotas em `app.routes.ts`.

**Independent Test**:
- **Criação (`US1`)**: Navegar para `/financeiro/contas-receber/novo`, validar bloqueio de campos obrigatórios vazios, preencher dados válidos e salvar, verificando chamada `POST /api/v1/FinanceiroContasReceber` e redirecionamento para `/financeiro/contas-receber`.
- **Edição (`US2`)**: Navegar para `/financeiro/contas-receber/:id/editar`, verificar preenchimento automático via `GET /api/v1/FinanceiroContasReceber/{id}`, alterar campos e salvar, verificando chamada `PUT /api/v1/FinanceiroContasReceber/{id}` e redirecionamento para `/financeiro/contas-receber`.

- [x] T003 [US1] [US2] Criar a classe do componente standalone `ContasReceberFormComponent` (`ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule`, Signals `loading`, `submitting`, `error`, `isEditMode`, `contaId`, `clientes`, `metodos`, carregamento via `forkJoin`, normalização de datas `YYYY-MM-DD`, normalização de status `RECEBIDO`/`PAGO`/`LIQUIDADO` → `RECEBIDO` e submissão para `createContaReceber` / `updateContaReceber`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-form/contas-receber-form.ts` (`T-023.1`)
- [x] T004 [US1] [US2] Criar o template HTML de `ContasReceberFormComponent` com cabeçalho dinâmico (Nova Conta a Receber / Editar Conta a Receber), estados de carregamento e erro, controles reativos (`descricao`, `clienteId`, `valor`, `vencimento`, `status`, `metodoId`, `dataRecebimento`, `observacao`), mensagens de validação e botões `"Cancelar"` / `"Salvar"` em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-form/contas-receber-form.html` (`T-023.1`)
- [x] T005 [P] [US1] [US2] Criar os estilos Dark Theme responsivos do formulário em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-form/contas-receber-form.scss` (`T-023.1`)
- [x] T006 [P] [US1] [US2] Registrar as rotas `{ path: 'financeiro/contas-receber/novo', component: ContasReceberFormComponent }` e `{ path: 'financeiro/contas-receber/:id/editar', component: ContasReceberFormComponent }` dentro do escopo protegido por `authGuard` (`MainLayout`) em `oficina-motos-web/src/app/app.routes.ts` (`T-023.2`)

**Checkpoint**: Rotas `/financeiro/contas-receber/novo` e `/financeiro/contas-receber/:id/editar` ativas e integradas aos endpoints `GET /{id}`, `POST` e `PUT /{id}` de `FinanceiroContasReceber`.

---

## Phase 3: User Story 3 — Ações de Criação e Edição na Listagem `/financeiro/contas-receber` (Priority: P2) (`T-023.2`)

**Goal**: Adicionar o botão **"+ Nova Conta"** no cabeçalho e a coluna de ação **"Editar"** em cada linha da tabela em `ContasReceberListaComponent`, conectando a listagem da `US-022` às novas rotas de criação e edição da `US-023`.

**Independent Test**: Acessar `/financeiro/contas-receber`, clicar em **"+ Nova Conta"** para ir a `/financeiro/contas-receber/novo` e clicar em **"Editar"** em uma linha da tabela para ir a `/financeiro/contas-receber/:id/editar`.

- [x] T007 [P] [US3] Adicionar botão `routerLink="/financeiro/contas-receber/novo"` no cabeçalho e coluna `"Ações"` com link `[routerLink]="['/financeiro/contas-receber', conta.id, 'editar']"` na tabela em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.html` e estilizar em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.scss` (`T-023.2`)

---

## Phase 4: Polish, Testes Unitários & Governança

**Purpose**: Garantir cobertura de testes unitários com Vitest para todos os fluxos de criação, edição, rotas e integração com `ContasReceber`, validar o build de produção e atualizar `governance/backlog.md`.

- [x] T008 [P] Criar suíte de testes unitários `contas-receber-form.spec.ts` cobrindo modo de criação (`POST`), modo de edição (`GET /:id` + `PUT /:id`), normalização de status (`PAGO`/`LIQUIDADO` → `RECEBIDO`), validações de campos obrigatórios, tratamento de erro/ID inválido e registro das rotas em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-form/contas-receber-form.spec.ts`
- [x] T009 [P] Atualizar `contas-receber-lista.spec.ts` para verificar a renderização do botão `"+ Nova Conta"` (`/financeiro/contas-receber/novo`) e dos links `"Editar"` (`/financeiro/contas-receber/:id/editar`) em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.spec.ts`
- [x] T010 Executar suíte de testes unitários do frontend (`npm test -- --no-watch`) em `oficina-motos-web/`
- [x] T011 Executar compilação de produção (`npm run build`) em `oficina-motos-web/`
- [x] T012 Atualizar o status das tarefas `T-023.1` e `T-023.2` para `[x]` em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Foundational — `FinanceiroService`)**: `T001` e `T002 [P]` — iniciam imediatamente; `T001` é pré-requisito para `T003`.
- **Phase 2 (US1 & US2 — `ContasReceberFormComponent` e Rotas)**: `T003` → `T004`, com `T005 [P]` (estilos `.scss`) e `T006 [P]` (`app.routes.ts`) executáveis em paralelo.
- **Phase 3 (US3 — Atalhos em `ContasReceberListaComponent`)**: `T007 [P]` — modifica apenas `contas-receber-lista.html/.scss`, podendo rodar em paralelo à Phase 2.
- **Phase 4 (Polish, Testes & Governança)**: `T008 [P]` e `T009 [P]` → `T010` → `T011` → `T012`.

### User Story Dependencies

- **US1 (Criar Conta a Receber Manual) & US2 (Editar Conta a Receber)**: Compartilham o mesmo componente `ContasReceberFormComponent` (`T-023.1`), as rotas em `app.routes.ts` (`T-023.2`) e os métodos de `FinanceiroService`.
- **US3 (Navegação a partir da Listagem)**: Independente da lógica interna do formulário; adiciona os links de entrada na tabela de `ContasReceberListaComponent`.

### Parallel Opportunities

- `T002` (`financeiro.service.spec.ts`), `T005` (`contas-receber-form.scss`), `T006` (`app.routes.ts`) e `T007` (`contas-receber-lista.html/.scss`) manipulam arquivos distintos e podem ser executados em paralelo.
- `T008` (`contas-receber-form.spec.ts`) e `T009` (`contas-receber-lista.spec.ts`) também podem ser criados/atualizados em paralelo.

---

## Parallel Example: Phase 2 & Phase 3

```bash
# Após T001 (FinanceiroService):
Task T003/T004: "Criar ContasReceberFormComponent em contas-receber-form.ts e contas-receber-form.html"
Task T005: "Criar estilos Dark Theme em contas-receber-form.scss"
Task T006: "Registrar rotas /financeiro/contas-receber/novo e /financeiro/contas-receber/:id/editar em app.routes.ts"
Task T007: "Adicionar botão '+ Nova Conta' e coluna 'Editar' em contas-receber-lista.html/.scss"
```

---

## Implementation Strategy

### MVP First (Phase 1 + Phase 2)

1. Concluir `T001` e `T002` (integração e testes dos endpoints `ContasReceber` em `FinanceiroService`).
2. Concluir `T003` a `T006` (`ContasReceberFormComponent` completo para criação e edição + rotas registradas).
3. Validar criação (`/financeiro/contas-receber/novo`) e edição (`/financeiro/contas-receber/:id/editar`).

### Incremental Delivery

1. Conectar a listagem `/financeiro/contas-receber` com botões de criação e edição (`T007`).
2. Adicionar/atualizar testes unitários Vitest (`T008`, `T009`), validar `npm test -- --no-watch` (`T010`), `npm run build` (`T011`) e marcar a `US-023` como concluída no backlog (`T012`).
