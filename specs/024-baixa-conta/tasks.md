---
description: "Task list for feature 024-baixa-conta"
---

# Tasks: US-024 — Registrar Baixa de Conta

**Input**: Design documents from `/specs/024-baixa-conta/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21 (`financeiro.service.spec.ts`, `baixa-pagamento-modal.spec.ts`, `contas-pagar-lista.spec.ts`, `contas-receber-lista.spec.ts`) e compilação `dotnet build` + `npm run build`.
**Organization**: As tarefas são organizadas por História de Usuário (`US1`, `US2`, `US3`) e mapeadas diretamente para as tasks oficiais do backlog (`T-024.1`, `T-024.2`, `T-024.3`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Foundational — Endpoints Backend `ContasPagarPagamentos` e `ContasReceberPagamentos` (`T-024.2`, `T-024.3`)

**Purpose**: Disponibilizar na API .NET 8 os contratos, validadores, regras de negócio (baixa integral e parcial com cálculo de `saldoResidual` e registro em `fin_pagamentos`) e os controllers `POST /api/v1/ContasPagarPagamentos` (`T-024.2`) e `POST /api/v1/ContasReceberPagamentos` (`T-024.3`).

- [x] T001 [P] Criar os DTOs de requisição `CreateContaPagarPagamentoDTO` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/Financeiro/CreateContaPagarPagamentoDTO.cs` e `CreateContaReceberPagamentoDTO` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/Financeiro/CreateContaReceberPagamentoDTO.cs` (`T-024.2`, `T-024.3`)
- [x] T002 [P] Criar os DTOs de resposta `ContaPagarPagamentoResponseDTO` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/Financeiro/ContaPagarPagamentoResponseDTO.cs` e `ContaReceberPagamentoResponseDTO` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/Financeiro/ContaReceberPagamentoResponseDTO.cs` (`T-024.2`, `T-024.3`)
- [x] T003 [P] Criar os validadores FluentValidation `CreateContaPagarPagamentoValidator` em `oficina-motos-api/src/OficinaMotos.Application/Validators/Financeiro/CreateContaPagarPagamentoValidator.cs` e `CreateContaReceberPagamentoValidator` em `oficina-motos-api/src/OficinaMotos.Application/Validators/Financeiro/CreateContaReceberPagamentoValidator.cs` (`T-024.2`, `T-024.3`)
- [x] T004 Implementar `RegistrarBaixaAsync(CreateContaPagarPagamentoDTO request)` em `oficina-motos-api/src/OficinaMotos.Application/Interfaces/Financeiro/IFinanceiroContaPagarService.cs` e `oficina-motos-api/src/OficinaMotos.Application/Services/Financeiro/FinanceiroContaPagarService.cs` e criar o controller `ContasPagarPagamentosController` (`POST /api/v1/ContasPagarPagamentos`) em `oficina-motos-api/src/OficinaMotos.API/Controllers/Financeiro/ContasPagarPagamentosController.cs` (`T-024.2`)
- [x] T005 Implementar `RegistrarBaixaAsync(CreateContaReceberPagamentoDTO request)` em `oficina-motos-api/src/OficinaMotos.Application/Interfaces/Financeiro/IFinanceiroContaReceberService.cs` e `oficina-motos-api/src/OficinaMotos.Application/Services/Financeiro/FinanceiroContaReceberService.cs` e criar o controller `ContasReceberPagamentosController` (`POST /api/v1/ContasReceberPagamentos`) em `oficina-motos-api/src/OficinaMotos.API/Controllers/Financeiro/ContasReceberPagamentosController.cs` (`T-024.3`)

**Checkpoint**: Endpoints `POST /api/v1/ContasPagarPagamentos` e `POST /api/v1/ContasReceberPagamentos` operacionais no backend com suporte a pagamento integral e parcial (`saldoResidual`).

---

## Phase 2: Foundational — Frontend Core (`api-paths.ts`, Models e `FinanceiroService`) (`T-024.2`, `T-024.3`)

**Purpose**: Registrar as rotas centralizadas em `api-paths.ts`, definir os tipos TypeScript de requisição/resposta e alvo de baixa em `financeiro.ts` e expor os métodos `registrarBaixaContaPagar` e `registrarBaixaContaReceber` em `FinanceiroService`.

- [x] T006 Registrar `pagarPagamentos: '/api/v1/ContasPagarPagamentos'` e `receberPagamentos: '/api/v1/ContasReceberPagamentos'` em `oficina-motos-web/src/app/core/services/api-paths.ts` e adicionar as interfaces `BaixaContaTipo`, `BaixaContaAlvo`, `CreateContaPagarPagamentoRequest`, `ContaPagarPagamentoResponse`, `CreateContaReceberPagamentoRequest` e `ContaReceberPagamentoResponse` em `oficina-motos-web/src/app/core/models/financeiro.ts` (`T-024.2`, `T-024.3`)
- [x] T007 Adicionar os métodos `registrarBaixaContaPagar(payload: CreateContaPagarPagamentoRequest)` e `registrarBaixaContaReceber(payload: CreateContaReceberPagamentoRequest)` em `oficina-motos-web/src/app/core/services/financeiro.service.ts` e atualizar os testes unitários em `oficina-motos-web/src/app/core/services/financeiro.service.spec.ts` (`T-024.2`, `T-024.3`)

**Checkpoint**: `FinanceiroService` integrado e testado contra `/api/v1/ContasPagarPagamentos` e `/api/v1/ContasReceberPagamentos`.

---

## Phase 3: User Story 1 & User Story 3 — Criar `BaixaPagamentoModalComponent` e Integrar Baixa em Contas a Pagar (Priority: P1) 🎯 MVP (`T-024.1`, `T-024.2`)

**Goal**: Construir o `BaixaPagamentoModalComponent` com campos de data de pagamento, valor pago, forma de pagamento, observação e cálculo em tempo real do **saldo residual** (baixa integral vs parcial), e integrá-lo à listagem `ContasPagarListaComponent` (`/financeiro/contas-pagar`).

**Independent Test**: Acessar `/financeiro/contas-pagar`, clicar em **"Baixar"** em uma conta `PENDENTE` ou `VENCIDO`, testar a validação de campos e o cálculo de saldo residual, confirmar uma baixa parcial (verificando que a conta permanece em aberto com o novo `saldoResidual`) e uma baixa integral (verificando que o status passa para `PAGO`).

- [x] T008 [US1] [US3] Criar o componente standalone `BaixaPagamentoModalComponent` (`ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule`, Signals `submitting`, `saldoAtual`, `saldoResidual`, `isParcial`, `isIntegral`, `valorExcedente`, validações de `dataPagamento`, `valorPago` e `metodoId`, e submissão para `registrarBaixaContaPagar` / `registrarBaixaContaReceber`) em `oficina-motos-web/src/app/features/financeiro/components/baixa-pagamento-modal/baixa-pagamento-modal.ts`, `oficina-motos-web/src/app/features/financeiro/components/baixa-pagamento-modal/baixa-pagamento-modal.html` e `oficina-motos-web/src/app/features/financeiro/components/baixa-pagamento-modal/baixa-pagamento-modal.scss` (`T-024.1`)
- [x] T009 [US1] [US3] Integrar o botão `"Baixar"` (exibido apenas para contas com status efetivo `PENDENTE` ou `VENCIDO`) e o `<app-baixa-pagamento-modal>` em `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.ts`, `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.html` e `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.scss`, atualizando reativamente a lista e os KPIs após cada baixa (`T-024.2`)

**Checkpoint**: Fluxo completo de baixa integral e parcial de Contas a Pagar operacional em `/financeiro/contas-pagar`.

---

## Phase 4: User Story 2 — Integrar Baixa em Contas a Receber (Priority: P1) (`T-024.3`)

**Goal**: Integrar o `BaixaPagamentoModalComponent` e o botão **"Baixar"** na listagem `ContasReceberListaComponent` (`/financeiro/contas-receber`), permitindo registrar baixas integrais (`status = RECEBIDO`) e parciais (`saldoResidual` em aberto) via `POST /api/v1/ContasReceberPagamentos`.

**Independent Test**: Acessar `/financeiro/contas-receber`, clicar em **"Baixar"** em um recebível `PENDENTE` ou `VENCIDO`, registrar uma baixa parcial e uma baixa integral, e verificar a atualização reativa do status (`RECEBIDO`), do saldo residual e dos KPIs.

- [x] T010 [US2] [US3] Integrar o botão `"Baixar"` (exibido apenas para recebíveis com status efetivo `PENDENTE` ou `VENCIDO`) e o `<app-baixa-pagamento-modal>` em `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.ts`, `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.html` e `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.scss`, atualizando reativamente a lista e os KPIs após cada baixa (`T-024.3`)

**Checkpoint**: Fluxo completo de baixa integral e parcial de Contas a Receber operacional em `/financeiro/contas-receber`.

---

## Phase 5: Polish, Testes Unitários, Build & Governança

**Purpose**: Garantir cobertura de testes unitários com Vitest para `BaixaPagamentoModalComponent` e para as listagens de Contas a Pagar e Contas a Receber, validar as compilações `.NET` e `Angular` e atualizar `governance/backlog.md`.

- [x] T011 [P] Criar suíte de testes unitários `baixa-pagamento-modal.spec.ts` cobrindo inicialização do modal, cálculo de `saldoResidual` (baixa integral e parcial), bloqueio de valores inválidos (`<= 0` ou `> saldoAtual`), submissão para `POST /api/v1/ContasPagarPagamentos` e `POST /api/v1/ContasReceberPagamentos` e tratamento de erro em `oficina-motos-web/src/app/features/financeiro/components/baixa-pagamento-modal/baixa-pagamento-modal.spec.ts` (`T-024.1`)
- [x] T012 [P] Atualizar `oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.spec.ts` e `oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.spec.ts` para validar a renderização condicional do botão `"Baixar"`, a abertura do modal e a atualização reativa da lista após baixa integral e parcial (`T-024.2`, `T-024.3`)
- [x] T013 Executar `dotnet build` em `oficina-motos-api/`, `npm test -- --no-watch` e `npm run build` em `oficina-motos-web/`
- [x] T014 Atualizar o status das tarefas `T-024.1`, `T-024.2` e `T-024.3` para `[x]` em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Backend API)**: `T001 [P]`, `T002 [P]` e `T003 [P]` iniciam imediatamente em paralelo → desbloqueiam `T004` e `T005`.
- **Phase 2 (Frontend Core)**: `T006` → `T007` (pode ser executada em paralelo à Phase 1).
- **Phase 3 (US1 & US3 — Modal + Contas a Pagar)**: Depende de `T006` e `T007`; `T008` cria o `BaixaPagamentoModalComponent` e `T009` integra em `ContasPagarListaComponent`.
- **Phase 4 (US2 — Contas a Receber)**: Depende de `T008`; `T010` integra o modal em `ContasReceberListaComponent` (pode rodar em paralelo a `T009`).
- **Phase 5 (Polish, Testes & Governança)**: `T011 [P]` e `T012 [P]` → `T013` → `T014`.

### Parallel Opportunities

- `T001`, `T002`, `T003` e `T006` manipulam arquivos independentes e podem rodar simultaneamente.
- `T004` (`ContasPagarPagamentos`) e `T005` (`ContasReceberPagamentos`) atuam em controllers e services distintos.
- `T009` (`ContasPagarListaComponent`) e `T010` (`ContasReceberListaComponent`) podem ser executadas em paralelo assim que `T008` (`BaixaPagamentoModalComponent`) estiver pronto.
- `T011` (`baixa-pagamento-modal.spec.ts`) e `T012` (`contas-pagar-lista.spec.ts` / `contas-receber-lista.spec.ts`) podem ser executadas em paralelo.

---

## Parallel Example: Phase 3 & Phase 4

```bash
# Após concluir T008 (BaixaPagamentoModalComponent):
Task T009: "Integrar botão Baixar e modal em ContasPagarListaComponent (T-024.2)"
Task T010: "Integrar botão Baixar e modal em ContasReceberListaComponent (T-024.3)"
```

---

## Implementation Strategy

### MVP First (Phase 1 + Phase 2 + Phase 3)

1. Concluir os endpoints backend (`T001`–`T005`) e os métodos de `FinanceiroService` (`T006`–`T007`).
2. Criar o `BaixaPagamentoModalComponent` (`T008`) e integrá-lo em `ContasPagarListaComponent` (`T009`).
3. Validar baixa integral e parcial com saldo residual em `/financeiro/contas-pagar`.

### Incremental Delivery

1. Estender a integração do modal para `ContasReceberListaComponent` (`T010`).
2. Executar a suíte completa de testes unitários (`T011`, `T012`), validar os builds (`T013`) e atualizar `governance/backlog.md` (`T014`).
