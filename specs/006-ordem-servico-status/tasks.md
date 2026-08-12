# Tasks: US-006 — Criar enum OrdemServicoStatus

**Input**: Design documents from `/specs/006-ordem-servico-status/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Organization**: Tasks are grouped by user story to enable incremental implementation and independent validation.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Confirm the project conventions and establish the implementation entry points for the status enum change.

- [X] T001 Create feature planning and implementation traceability in specs/006-ordem-servico-status/
- [X] T002 [P] Verify EF Core migration and entity configuration conventions in oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs and oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Align the domain model and persistence strategy before any story-specific implementation is started.

- [X] T003 Confirm `BaseEntity` and domain conventions in oficina-motos-api/src/OficinaMotos.Domain/Common/BaseEntity.cs and related domain entities
- [X] T004 [P] Review the current `OrdemServico` model and storage contract in oficina-motos-api/src/OficinaMotos.Domain/Entities/OrdemServico.cs and oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs

**Checkpoint**: Foundation ready - user story implementation can begin in parallel.

---

## Phase 3: User Story 1 - Acompanhamento do progresso da ordem de serviço (Priority: P1) 🎯 MVP

**Goal**: Representar o estado da ordem de serviço como enum de domínio, sem strings livres e com o valor inicial correpondendo ao ciclo operacional da oficina.

**Independent Test**: Validar que uma nova ordem inicia em `Aberta` e que o sistema aceita os demais estados esperados do enum.

### Implementation for User Story 1

- [X] T005 [P] [US1] Create enum definition in oficina-motos-api/src/OficinaMotos.Domain/Enums/OrdemServicoStatus.cs
- [X] T006 [US1] Update `Status` property and default value in oficina-motos-api/src/OficinaMotos.Domain/Entities/OrdemServico.cs
- [X] T007 [US1] Update `using`/namespace references in the domain model so `OrdemServico` consumes `OficinaMotos.Domain.Enums`

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently.

---

## Phase 4: User Story 2 - Integração entre API e banco de dados (Priority: P2)

**Goal**: Garantir que a persistência do enum seja compatível com o banco atual e com a configuração EF Core do projeto.

**Independent Test**: Validar que o EF Core converte o enum para string e que a migration não quebra a coluna atual do status da OS.

### Implementation for User Story 2

- [X] T008 [P] [US2] Configure enum conversion in oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs using `HasConversion<string>()` for `OrdemServico.Status`
- [X] T009 [US2] Add migration file `AddOrdemServicoStatusEnum` in oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/
- [X] T010 [US2] Update migration snapshot metadata if the project requires model snapshot regeneration in oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/OficinaContextModelSnapshot.cs
- [X] T011 [US2] Validate database compatibility and migration rollback path against the existing schema in oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/

**Checkpoint**: At this point, User Stories 1 and 2 should both work independently.

---

## Phase 5: User Story 3 - Evolução de dados históricos e migração (Priority: P3)

**Goal**: Ajustar os contratos da API para refletirem o enum e manter a consistência entre entrada, saída e domínio.

**Independent Test**: Validar que a API aceita e retorna os estados do enum sem quebrar payloads de criação e consulta da ordem de serviço.

### Implementation for User Story 3

- [X] T012 [P] [US3] Update request DTO in oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/OrdemServico/CreateOrdemServicoDTO.cs to use `OrdemServicoStatus`
- [X] T013 [P] [US3] Update request DTO in oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/OrdemServico/UpdateOrdemServicoDTO.cs to use `OrdemServicoStatus`
- [X] T014 [US3] Update response DTO in oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/OrdemServico/OrdemServicoResponseDTO.cs to expose `OrdemServicoStatus`
- [X] T015 [US3] Review other dependency contracts affected by the status type, including API serializers and endpoint payloads in oficina-motos-api/src/OficinaMotos.API/Controllers/OrdemServico/

**Checkpoint**: All user stories should now be independently functional.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final validation of the feature and documentation consistency.

- [X] T016 [P] Check final implementation against the feature checklist in specs/006-ordem-servico-status/checklists/requirements.md
- [X] T017 [P] Update validation documentation in specs/006-ordem-servico-status/quickstart.md with the actual verification steps used in this feature
- [X] T018 Run backend validation using `dotnet build` and migration verification for the changed files under oficina-motos-api/src/

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: no dependencies, can start immediately
- **Foundational (Phase 2)**: depends on Setup completion and blocks all user stories
- **User Stories (Phase 3+)**: all depend on Foundational completion; can proceed in priority order or in parallel when capacity exists
- **Polish (Phase 6)**: depends on all story work being complete

### User Story Dependencies

- **User Story 1 (P1)**: can begin after Foundational; no dependency on other stories
- **User Story 2 (P2)**: depends on User Story 1 domain state being established
- **User Story 3 (P3)**: depends on the enum and persistence model being available to API contracts

### Parallel Opportunities

- `T002` can run in parallel with `T003` and `T004`
- `T005` and `T008` can proceed in parallel after foundational work
- `T012` and `T013` can be implemented in parallel once the enum is ready
- `T016` and `T017` can run in parallel during the final validation phase

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 and Phase 2
2. Implement User Story 1 in `OrdemServico` and the status enum
3. Validate state transitions and defaults
4. Stop and confirm the domain is correct before moving to persistence

### Incremental Delivery

1. Complete the enum and entity update
2. Wire up EF Core conversion and migration
3. Update DTO contracts and API payloads
4. Finish with validation and documentation verification

### Parallel Team Strategy

With multiple developers:

1. Developer A: User Story 1 domain change
2. Developer B: User Story 2 EF Core conversion and migration
3. Developer C: User Story 3 DTO/API update
4. Shared validation in the final polish phase
