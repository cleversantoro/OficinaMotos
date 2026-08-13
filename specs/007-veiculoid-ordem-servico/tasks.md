# Tasks: US-007 — Adicionar VeiculoId à entidade OrdemServico

**Input**: Design documents from `/specs/007-veiculoid-ordem-servico/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Revisão do contexto de domínio e infraestrutura antes da implementação da feature

- [X] T001 [P] Revisar a estrutura atual de `OrdemServico` e `Veiculo` em `oficina-motos-api/src/OficinaMotos.Domain/Entities/OrdemServico.cs` e `oficina-motos-api/src/OficinaMotos.Domain/Entities/Veiculo.cs`
- [X] T002 [P] Confirmar o mapeamento do EF Core e a configuração global em `oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs` e `oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Preparar a base de domínio e persistência que bloqueia toda a implementação da feature

- [X] T003 Validar que a entidade `OrdemServico` pode receber a FK de veículo sem quebrar o contrato de domínio em `oficina-motos-api/src/OficinaMotos.Domain/Entities/OrdemServico.cs`
- [X] T004 Validar a convenção de relacionamento no EF Core em `oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs`
- [X] T005 Validar que o DTO de criação está alinhado com o modelo de domínio em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/OrdemServico/CreateOrdemServicoDTO.cs`
- [X] T006 [P] Confirmar que a configuração global do contexto aplica as entidades do domínio sem sobrescrita em `oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs`

**Checkpoint**: Foundation ready - user story implementation can now begin

---

## Phase 3: User Story 1 - Vinculação correta do veículo à ordem de serviço (Priority: P1) 🎯 MVP

**Goal**: Garantir que cada ordem de serviço fique associada a um veículo válido e rastreável

**Independent Test**: Pode ser validado criando uma ordem de serviço com `VeiculoId` válido e confirmando que a API aceita a criação; um payload sem veículo deve ser rejeitado.

### Implementation for User Story 1

- [X] T007 [P] [US1] Adicionar a propriedade `long VeiculoId` e a navegação `Veiculo? Veiculo` em `oficina-motos-api/src/OficinaMotos.Domain/Entities/OrdemServico.cs`
- [X] T008 [US1] Configurar a relação foreign key de `Veiculo` em `oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs`
- [X] T009 [US1] Ajustar o relacionamento para preservar a integridade referencial e evitar exclusão acidental em `oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs`

**Checkpoint**: User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - Validação e integridade de dados da API (Priority: P2)

**Goal**: Exigir a referência do veículo no contrato de criação e reforçar a validação do payload

**Independent Test**: Pode ser validado enviando uma requisição de criação sem `VeiculoId` e confirmando que a API responde com erro de validação; quando o `VeiculoId` está presente e válido, o registro é aceito.

### Implementation for User Story 2

- [X] T010 [P] [US2] Atualizar `CreateOrdemServicoDTO` com `[Required] long VeiculoId` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/OrdemServico/CreateOrdemServicoDTO.cs`
- [X] T011 [US2] Ajustar a validação e o mapeamento de entrada para refletir `VeiculoId` no fluxo de criação em `oficina-motos-api/src/OficinaMotos.Application/Services/OrdemServico/OrdemServicoService.cs`
- [X] T012 [US2] Revalidar o contrato da API em `specs/007-veiculoid-ordem-servico/contracts/ordem-servico-veiculo-contract.md` para manter o payload e a resposta consistentes com a regra de dados

**Checkpoint**: User Story 2 should work independently after validation of the API contract

---

## Phase 5: User Story 3 - Evolução segura do banco e reversão de migração (Priority: P3)

**Goal**: Publicar a alteração no banco de forma segura, com migration reversível e sem perda de integridade

**Independent Test**: Pode ser validado gerando e aplicando a migration, confirmando a criação da FK e verificando que o `Down` da migration remove a alteração de forma controlada.

### Implementation for User Story 3

- [X] T013 [P] [US3] Gerar a migration `AddVeiculoIdToOrdemServico` em `oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/`
- [X] T014 [US3] Incluir a coluna `veiculo_id` e a foreign key para `cad_veiculos` na migration, preservando a estrutura atual e o histórico de dados em `oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/`
- [X] T015 [US3] Validar o `Down` da migration e a reversibilidade em `oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/`

**Checkpoint**: All user stories should now be independently functional

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Revisão final de compatibilidade, documentação e validação de integração

- [X] T016 [P] Revisar a consistência do relacionamento em `oficina-motos-api/src/OficinaMotos.Domain/Entities/OrdemServico.cs`, `oficina-motos-api/src/OficinaMotos.Domain/Entities/Veiculo.cs` e `oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/OrdemServicoConfig/OrdemServicoConfigurations.cs`
- [X] T017 [P] Executar a validação final de compilação e integração do backend com `dotnet build` na pasta `oficina-motos-api` e confirmar que a feature segue a documentação em `specs/007-veiculoid-ordem-servico/quickstart.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User Story 1 (P1) is the MVP and can be validated independently
  - User Story 2 (P2) depends on US1 contracts and validation rules
  - User Story 3 (P3) depends on the domain and persistence changes being in place
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational - no dependency on other stories
- **User Story 2 (P2)**: Can start after US1 foundation and validation rules are in place
- **User Story 3 (P3)**: Can start after US1 and US2 are implemented and the relational model is confirmed

### Parallel Opportunities

- Setup tasks `T001` and `T002` can run in parallel
- Foundational tasks `T006` can run in parallel with review of domain/configuration
- User Story 1 tasks `T007` and `T009` can proceed in parallel if work is split across model and EF configuration
- User Story 2 tasks `T010` and `T012` can run in parallel
- User Story 3 tasks `T013` and `T015` can run in parallel

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate the model plus the creation flow with `VeiculoId`
5. Proceed to US2 and US3 only after the MVP is confirmed

### Incremental Delivery

1. Setup + Foundational → foundation ready
2. User Story 1 → add `VeiculoId` and relation model
3. User Story 2 → enforce API contract and validation
4. User Story 3 → add migration and rollback safety
5. Final polish and validation

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to a user story for traceability
- Each user story is independently completable and testable
- Avoid vague tasks and cross-story dependencies that compromise independence
