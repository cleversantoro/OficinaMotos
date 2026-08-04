# Tasks: Reativar Soft Delete em BaseEntity

**Input**: Design documents from `/specs/005-reativar-soft-delete-baseentity/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/, quickstart.md

**Tests**: Incluídos para garantir aderência à constituição (qualidade e testabilidade) em mudanças transversais de domínio e persistência.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Confirmar baseline técnico e preparar execução segura da alteração transversal

- [X] T001 Revisar o estado atual de exclusão em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Common/BaseEntity.cs e registrar diferenças esperadas no fluxo desta feature
- [X] T002 [P] Revisar contratos e implementação do repositório base em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Interfaces/Repositories/IRepository.cs e c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs
- [X] T003 [P] Revisar configuração do contexto EF Core em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs para aplicação de query filter global

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Implementar fundações de soft delete que bloqueiam todas as user stories

**⚠️ CRITICAL**: Nenhuma user story pode iniciar antes desta fase

- [X] T004 Atualizar a entidade base com `IsDeleted` e `DeletedAt` em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Common/BaseEntity.cs
- [X] T005 [P] Adicionar operação explícita de soft delete no contrato base em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Interfaces/Repositories/IRepository.cs
- [X] T006 Implementar a operação de soft delete e alinhar `DeleteAsync` para exclusão lógica em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs
- [X] T007 Implementar query filter global `!IsDeleted` no `OnModelCreating` em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs
- [X] T008 Ajustar busca por ID para respeitar soft delete (evitando bypass de filtro global) em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs

**Checkpoint**: Base técnica pronta para exclusão lógica transversal e leitura filtrada

---

## Phase 3: User Story 1 - Exclusão lógica padronizada (Priority: P1) 🎯 MVP

**Goal**: Garantir que toda entidade derivada de BaseEntity possua e utilize estado de exclusão lógica consistente.

**Independent Test**: Excluir um registro por fluxo de serviço e confirmar persistência com `IsDeleted = true` e `DeletedAt` preenchido, sem remoção física.

### Implementation for User Story 1

- [X] T009 [US1] Validar que o método de domínio de exclusão lógica atualiza `IsDeleted`, `DeletedAt` e `UpdatedAt` após T004 em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Common/BaseEntity.cs
- [X] T010 [US1] Validar o contrato público com `SoftDeleteAsync(long id)` e compatibilidade de `DeleteAsync(long id)` após T005 em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Interfaces/Repositories/IRepository.cs
- [X] T011 [US1] Validar execução de `SoftDeleteAsync` e `DeleteAsync` sem hard delete após T006 em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs
- [X] T012 [US1] Validar compatibilidade dos serviços existentes por `dotnet build` + execução de pelo menos 3 fluxos DELETE sem alteração de assinatura pública em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Application/

### Tests for User Story 1

- [X] T025 [P] [US1] Criar testes unitários do `BaseEntity` cobrindo transição para exclusão lógica em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Common/BaseEntity.cs
- [X] T026 [P] [US1] Criar testes de repositório para `SoftDeleteAsync` e `DeleteAsync` em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs

**Checkpoint**: Exclusão lógica padronizada e utilizável em todos os fluxos de domínio que usam repositório base

---

## Phase 4: User Story 2 - Leitura padrão sem registros excluídos (Priority: P1)

**Goal**: Fazer com que consultas padrão da API ignorem automaticamente registros excluídos logicamente.

**Independent Test**: Criar registros, executar soft delete em parte deles e validar que GET/listagens padrão não retornam excluídos.

### Implementation for User Story 2

- [X] T013 [US2] Validar que o query filter global definido em T007 está aplicado para todos os tipos que herdam `BaseEntity` em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs
- [X] T014 [US2] Ajustar `GetByIdAsync` para consulta com filtro global em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs
- [X] T015 [P] [US2] Validar `GetAllAsync` e `FindAsync` com cenários contendo registros ativos e excluídos logicamente em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs
- [X] T016 [US2] Atualizar guia de verificação funcional de leitura filtrada em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/quickstart.md
- [X] T028 [US2] Validar default de `IsDeleted = false` em novas inserções e em base existente após migração em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/quickstart.md

### Tests for User Story 2

- [X] T027 [P] [US2] Criar testes de integração para confirmar que consultas padrão não retornam `IsDeleted = true` em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs

**Checkpoint**: Consultas padrão não expõem mais registros excluídos logicamente

---

## Phase 5: User Story 3 - Exclusão via API sem hard delete (Priority: P2)

**Goal**: Garantir que endpoints DELETE mantenham contrato REST e executem apenas exclusão lógica no backend.

**Independent Test**: Chamar DELETE de recurso de negócio e confirmar resposta HTTP esperada, ausência em leituras padrão e permanência física no banco.

### Implementation for User Story 3

- [X] T017 [US3] Criar migration `AddSoftDeleteToBaseEntity` em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/
- [X] T018 [US3] Validar e ajustar arquivos de snapshot gerados em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/
- [X] T019 [US3] Confirmar cobertura dos endpoints DELETE dos controladores `ClientesController`, `VeiculosController`, `FornecedoresController`, `MecanicosController`, `OrdemServicosController`, `EstoquePecasController` e `FinanceiroContasPagarController`, validando ausência de hard delete em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.API/Controllers/
- [X] T020 [US3] Atualizar contrato de comportamento do DELETE com semântica final em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/contracts/soft-delete-contract.md

**Checkpoint**: DELETE preserva contrato HTTP e não remove fisicamente dados de negócio

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Finalizar validações de build/migration e coerência documental

- [X] T021 [P] Executar `dotnet build` e registrar resultado da feature em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/quickstart.md
- [X] T022 Executar fluxo de validação da migration (apply + rollback) e registrar evidências em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/quickstart.md
- [X] T023 [P] Revisar consistência final entre spec, plan, research, data-model e contrato em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/
- [X] T024 Validar manualmente os critérios de aceite da US-005 e consolidar checklist final em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/checklists/requirements.md

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1**: Pode iniciar imediatamente
- **Phase 2**: Depende da Phase 1 e bloqueia todas as user stories
- **Phase 3 (US1)**: Depende da conclusão da Phase 2
- **Phase 4 (US2)**: Depende da conclusão da Phase 2
- **Phase 5 (US3)**: Depende da conclusão das Phases 3 e 4
- **Phase 6**: Depende da conclusão das histórias implementadas

### User Story Dependencies

- **User Story 1 (P1)**: Primeiro incremento do MVP após fundação
- **User Story 2 (P1)**: Depende da fundação e complementa o MVP de leitura
- **User Story 3 (P2)**: Depende de US1+US2 para consolidar contrato API + migration

### Within Each User Story

- Alterações de domínio e contrato antes de ajustes de repositório
- Ajustes de repositório antes de validações de comportamento
- Migração após estabilização de modelo e contexto
- Validação final após build + apply/rollback

### Parallel Opportunities

- T002 e T003 em paralelo na Setup
- T005 e T007 em paralelo na Foundational
- T025 e T026 em paralelo na US1
- T015 e T016 em paralelo na US2
- T027 e T028 em paralelo na US2
- T021 e T023 em paralelo na fase de Polish

---

## Parallel Example: User Story 1

```bash
Task: "Atualizar BaseEntity com IsDeleted e DeletedAt em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Common/BaseEntity.cs"
Task: "Adicionar SoftDeleteAsync no contrato base em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Domain/Interfaces/Repositories/IRepository.cs"
```

## Parallel Example: User Story 2

```bash
Task: "Validar GetAllAsync e FindAsync com query filter global em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/Repository.cs"
Task: "Atualizar quickstart de leitura filtrada em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/quickstart.md"
```

## Parallel Example: User Story 3

```bash
Task: "Confirmar controladores DELETE sem hard delete em c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.API/Controllers/"
Task: "Atualizar contrato final de DELETE em c:/Projetos/OficinaMotos/specs/005-reativar-soft-delete-baseentity/contracts/soft-delete-contract.md"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Completar Phase 1 (Setup)
2. Completar Phase 2 (Foundational)
3. Completar Phase 3 (US1)
4. Validar exclusão lógica persistida sem hard delete

### Incremental Delivery

1. Foundation pronta (Phases 1 e 2)
2. Entregar US1 (base de soft delete)
3. Entregar US2 (leitura filtrada)
4. Entregar US3 (migration + contrato API)
5. Finalizar com validações transversais

### Parallel Team Strategy

1. Time fecha Setup e Foundational em conjunto
2. Depois da fundação:
   - Dev A: US1
   - Dev B: US2
   - Dev C: US3
3. Consolidar na fase de Polish com build e rollback

---

## Notes

- [P] tasks = diferentes arquivos, sem dependência direta
- [Story] label mapeia tarefa à user story
- Cada user story permanece testável de forma independente
- Evitar alteração de assinatura pública já consumida sem necessidade
- Manter alinhamento com a constituição e com os artefatos da feature
