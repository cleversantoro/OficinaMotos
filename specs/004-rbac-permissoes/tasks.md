# Tasks: RBAC por permissões em controllers de negócio

**Input**: Design documents from `/specs/004-rbac-permissoes/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/, quickstart.md

**Tests**: Included because the feature spec defines independent test criteria for each story.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Align the feature with the current auth and UI structure before applying RBAC rules

- [X] T001 Review current JWT and fallback policy setup in c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Program.cs and identify the controllers that will receive role restrictions
- [X] T002 Review current frontend auth/session flow in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\core\auth\auth.service.ts and c:\Projetos\OficinaMotos\oficina-motos-web\src\app\core\auth\auth.model.ts to confirm where role data is already available
- [X] T003 [P] Review shared table action visibility hooks in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\shared\ui\data-table\data-table.models.ts and c:\Projetos\OficinaMotos\oficina-motos-web\src\app\shared\ui\data-table\data-table.ts

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish the common RBAC shape used by all stories before restricting any specific action

- [X] T004 Define the role-permission matrix for destructive business actions in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\data-model.md and map the canonical roles to edit/delete capabilities
- [X] T005 [P] Document the backend authorization contract for 403 responses in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\contracts\rbac-access-contract.md
- [X] T006 [P] Document the frontend visibility contract for destructive actions in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\contracts\rbac-access-contract.md
- [X] T007 Add a shared role-to-capability helper in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\core\auth\rbac-access.helper.ts, exposing the current user role in a form that UI guards can consume

**Checkpoint**: RBAC rules and shared role shape are defined and ready for per-story implementation

---

## Phase 3: User Story 1 - Acesso coerente por papel (Priority: P1)

**Goal**: Hide destructive actions from users whose role does not allow them, so the UI only exposes valid actions.

**Independent Test**: With a restricted role, the action buttons for edit/delete are not rendered in a business table or detail screen; with an authorized role, they are rendered normally.

### Tests for User Story 1

- [X] T008 [P] [US1] Add unit tests for action visibility rules in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\shared\ui\data-table\data-table.spec.ts
- [X] T009 [P] [US1] Add unit tests for role-based button visibility helpers in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\core\auth\rbac-access.helper.spec.ts

### Implementation for User Story 1

- [X] T010 [US1] Extend the table action visibility logic in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\shared\ui\data-table\data-table.models.ts and c:\Projetos\OficinaMotos\oficina-motos-web\src\app\shared\ui\data-table\data-table.ts to support role-aware hiding of destructive actions
- [X] T011 [P] [US1] Add a reusable role guard/helper for destructive action visibility in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\core\auth\
- [X] T012 [US1] Wire the role-aware visibility helper into c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\clientes\pages\cliente-lista\cliente-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\clientes\pages\cliente-detalhe\cliente-detalhe.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\motos\pages\veiculo-lista\veiculo-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\fornecedores\pages\fornecedor-lista\fornecedor-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\mecanicos\pages\mecanico-lista\mecanico-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\estoque\pages\estoque-lista\estoque-lista.ts, and c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\ordens-servico\pages\os-detalhe\os-detalhe.ts for edit/delete buttons
- [X] T013 [US1] Verify the login/session role value from c:\Projetos\OficinaMotos\oficina-motos-web\src\app\core\auth\auth.service.ts is used consistently by the new visibility logic

**Checkpoint**: Users without permission no longer see destructive buttons; authorized roles still see them

---

## Phase 4: User Story 2 - Bloqueio de operações destrutivas (Priority: P1)

**Goal**: Enforce authorization on destructive backend operations so restricted roles receive 403 even if the UI is bypassed.

**Independent Test**: Calling update/delete endpoints with an insufficient role returns 403; calling the same endpoints with an authorized role succeeds.

### Tests for User Story 2

- [ ] T014 [P] [US2] Add API authorization tests for destructive endpoints in c:\Projetos\OficinaMotos\oficina-motos-api\tests\OficinaMotos.API.Tests\OficinaMotos.API.Tests.csproj
- [ ] T015 [P] [US2] Add negative-path tests asserting 403 for restricted roles on update/delete operations in c:\Projetos\OficinaMotos\oficina-motos-api\tests\OficinaMotos.API.Tests\Authorization\DestructiveEndpointsTests.cs

### Implementation for User Story 2

- [X] T016 [P] [US2] Apply role-based authorization attributes to destructive endpoints in c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Cliente\ClientesController.cs
- [X] T017 [P] [US2] Apply role-based authorization attributes to destructive endpoints in c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\UsuariosController.cs
- [X] T018 [P] [US2] Apply role-based authorization attributes to destructive endpoints in c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Cliente\ClientesController.cs, c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Veiculo\VeiculosController.cs, c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Fornecedor\FornecedoresController.cs, c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Mecanico\MecanicosController.cs, c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\OrdemServico\OrdemServicosController.cs, c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Estoque\EstoquePecasController.cs, and c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Financeiro\FinanceiroContasPagarController.cs
- [X] T019 [US2] Ensure unauthorized requests return 403 consistently through the existing authorization pipeline in c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Program.cs

**Checkpoint**: Restricted roles are blocked at the API layer and authorized roles keep working

---

## Phase 5: User Story 3 - Matriz clara de permissões por papel (Priority: P2)

**Goal**: Make the role-permission matrix explicit and reusable so product, security, backend, and frontend follow the same access rules.

**Independent Test**: The matrix documents which roles can edit/delete, and the same matrix drives both the API restrictions and the frontend visibility rules.

### Tests for User Story 3

- [ ] T020 [P] [US3] Add coverage for the documented role-permission matrix in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\quickstart.md or a dedicated validation note if needed
- [ ] T021 [P] [US3] Add a consistency test or checklist entry for role-permission mapping in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\data-model.md

### Implementation for User Story 3

- [ ] T022 [US3] Consolidate the role-permission matrix documentation in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\data-model.md so each canonical role has explicit edit/delete expectations
- [ ] T023 [P] [US3] Add or update a shared frontend authorization helper in c:\Projetos\OficinaMotos\oficina-motos-web\src\app\core\auth\rbac-access.helper.ts to consume the documented role matrix
- [ ] T024 [US3] Update c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\clientes\pages\cliente-lista\cliente-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\clientes\pages\cliente-detalhe\cliente-detalhe.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\motos\pages\veiculo-lista\veiculo-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\fornecedores\pages\fornecedor-lista\fornecedor-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\mecanicos\pages\mecanico-lista\mecanico-lista.ts, c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\estoque\pages\estoque-lista\estoque-lista.ts, and c:\Projetos\OficinaMotos\oficina-motos-web\src\app\features\ordens-servico\pages\os-detalhe\os-detalhe.ts to use the shared authorization helper
- [ ] T025 [US3] Add a backend note or inline policy reference near c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Cliente\ClientesController.cs and c:\Projetos\OficinaMotos\oficina-motos-api\src\OficinaMotos.API\Controllers\Veiculo\VeiculosController.cs to tie the implementation back to the documented matrix

**Checkpoint**: The same matrix explains and enforces access rules across documentation, API, and UI

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Finish consistency, documentation, and validation across all stories

- [ ] T026 [P] Update any feature-facing docs or navigation references affected by RBAC visibility in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\quickstart.md and related notes
- [X] T027 Run a final walkthrough of the affected frontend and backend files to confirm no destructive action remains exposed without a role check
- [ ] T028 Validate the quickstart scenarios in c:\Projetos\OficinaMotos\specs\004-rbac-permissoes\quickstart.md against the implemented behavior

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1**: Can start immediately
- **Phase 2**: Depends on Phase 1 completion and blocks all user stories
- **Phase 3**: Depends on Phase 2 completion
- **Phase 4**: Depends on Phase 2 completion
- **Phase 5**: Depends on Phase 2 completion, with implementation integrating the results of Phases 3 and 4
- **Phase 6**: Depends on completion of the stories chosen for delivery

### User Story Dependencies

- **User Story 1 (P1)**: Can start after the foundational RBAC matrix is documented
- **User Story 2 (P1)**: Can start after the foundational RBAC matrix is documented
- **User Story 3 (P2)**: Depends on the matrix structure and benefits from the completed backend/frontend enforcement rules

### Within Each User Story

- Tests, when included, should be written before implementation and should fail before the code change
- Frontend helpers before UI wiring
- API authorization before end-to-end validation
- Documentation consistency before polishing

### Parallel Opportunities

- Setup review tasks T002 and T003 can run in parallel after T001
- Foundational documentation tasks T005 and T006 can run in parallel after T004
- User Story 1 test tasks T008 and T009 can run in parallel
- User Story 1 implementation tasks T011 and parts of T012 can run in parallel if they touch different files
- User Story 2 test tasks T014 and T015 can run in parallel
- User Story 2 controller updates T016, T017, and T018 can run in parallel if different controller files are edited
- User Story 3 helper and documentation tasks T022 and T023 can run in parallel

---

## Parallel Example: User Story 1

```bash
Task: "Add unit tests for action visibility rules in c:\\Projetos\\OficinaMotos\\oficina-motos-web\\src\\app\\shared\\ui\\data-table\\data-table.spec.ts"
Task: "Add unit tests for role-based button visibility helpers in a new frontend spec file under c:\\Projetos\\OficinaMotos\\oficina-motos-web\\src\\app\\core\\auth\\"
Task: "Add a reusable role guard/helper for destructive action visibility in c:\\Projetos\\OficinaMotos\\oficina-motos-web\\src\\app\\core\\auth\\"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Validate that restricted roles no longer see destructive actions
5. Demo the UI-level RBAC improvement

### Incremental Delivery

1. Complete Setup + Foundational → shared RBAC shape is ready
2. Add User Story 1 → UI hides destructive actions
3. Add User Story 2 → API blocks destructive operations with 403
4. Add User Story 3 → document and reuse the matrix consistently across layers
5. Complete Polish → run the quickstart scenarios and final walkthrough

### Parallel Team Strategy

With multiple developers:

1. Team completes Phase 1 and Phase 2 together
2. Once the foundation is ready:
   - Developer A: User Story 1 frontend visibility work
   - Developer B: User Story 2 backend authorization work
   - Developer C: User Story 3 matrix consolidation and shared helper work
3. Finish with shared validation and polishing

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Avoid same-file conflicts when assigning parallel work
- Keep the role matrix aligned with [CONSTITUTION.md](../../oficina-motos-docs/markdown/CONSTITUTION.md) and [SEGURANCA_USUARIOS.md](../../oficina-motos-docs/markdown/SEGURANCA_USUARIOS.md)
