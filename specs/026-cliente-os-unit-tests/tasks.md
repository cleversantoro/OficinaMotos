---
description: "Task list for feature 026-cliente-os-unit-tests"
---

# Tasks: US-027 & US-028 — Testes Unitários de ClienteService e OrdemServicoService

**Input**: Design documents from `/specs/026-cliente-os-unit-tests/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/test-contracts.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Suítes de testes xUnit `ClienteServiceTests` (`US-027`) e `OrdemServicoServiceTests` (`US-028`) em `oficina-motos-api/src/OficinaMotos.Tests/Services/` utilizando `Moq`, `FluentAssertions` e `Microsoft.EntityFrameworkCore.InMemory` (`TestDbContextFactory`).
**Organization**: As tarefas são organizadas por fases (Foundational → `US1` = `US-027` → `US2` = `US-028` → Polish) e mapeadas diretamente para as tasks oficiais do backlog (`T-027.1` a `T-027.4` e `T-028.1` a `T-028.4`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Foundational — Exceção de Domínio e Tratamento Global de Erros

**Purpose**: Criar a exceção de domínio `DomainException` compartilhada entre `US-027` (CPF duplicado) e `US-028` (transições inválidas de status da OS) e mapear `DomainException` / `ValidationException` no handler global da API.

**⚠️ CRITICAL**: Necessário antes da implementação das regras de exceção de `US1` e `US2`.

- [x] T001 Criar a classe `DomainException` (derivada de `InvalidOperationException`) em `oficina-motos-api/src/OficinaMotos.Domain/Exceptions/DomainException.cs`
- [x] T002 [P] Atualizar `GlobalExceptionHandler` para retornar `400 BadRequest` quando a exceção for `DomainException` ou `FluentValidation.ValidationException` em `oficina-motos-api/src/OficinaMotos.API/Middlewares/GlobalExceptionHandler.cs`

**Checkpoint**: `DomainException` disponível na camada `Domain` para `ClienteService` e `OrdemServicoService`.

---

## Phase 2: User Story 1 — Testes Unitários de `ClienteService` (Priority: P1) 🎯 MVP (`US-027`: `T-027.1` a `T-027.4`)

**Goal**: Garantir que `ClienteService.CreateAsync` lance `DomainException` ao detectar CPF duplicado e implementar a suíte `ClienteServiceTests` cobrindo criação de cliente PF com CPF válido (`T-027.1`), rejeição de CPF duplicado (`T-027.2`), exclusão lógica sem remoção física (`T-027.3`) e omissão de cliente excluído logicamente em `GetByIdAsync` (`T-027.4`).

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~ClienteServiceTests"` em `oficina-motos-api/` e verificar a aprovação dos 4 testes (`Create_PF_WithValidCpf_Succeeds`, `Create_DuplicateCpf_ThrowsDomainException`, `Delete_SetsIsDeletedTrue_NotPhysicalDelete` e `GetById_DeletedClient_ReturnsNull`).

- [x] T003 [US1] Atualizar `ClienteService.CreateAsync` para lançar `DomainException` quando já existir cliente cadastrado com o mesmo CPF em `oficina-motos-api/src/OficinaMotos.Application/Services/Cliente/ClienteService.cs` (`T-027.2`)
- [x] T004 [US1] Criar a suíte `ClienteServiceTests` implementando `Create_PF_WithValidCpf_Succeeds` (`T-027.1`), `Create_DuplicateCpf_ThrowsDomainException` (`T-027.2`), `Delete_SetsIsDeletedTrue_NotPhysicalDelete` (`T-027.3`) e `GetById_DeletedClient_ReturnsNull` (`T-027.4`) em `oficina-motos-api/src/OficinaMotos.Tests/Services/ClienteServiceTests.cs`

**Checkpoint**: Todos os 4 cenários de teste de `ClienteService` implementados e passando (`US-027` completa).

---

## Phase 3: User Story 2 — Testes Unitários de `OrdemServicoService` (Priority: P1) (`US-028`: `T-028.1` a `T-028.4`)

**Goal**: Garantir a validação de `VeiculoId` obrigatório em `OrdemServicoService.CreateAsync` (`FluentValidation.ValidationException`), a máquina de estados para mudança de status (`ChangeStatusAsync` e validação em `UpdateAsync`) e o cálculo do total de peças e serviços (`CalculateTotal` / `CalculateTotalAsync`), implementando os 4 testes unitários obrigatórios em `OrdemServicoServiceTests`.

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~OrdemServicoServiceTests"` em `oficina-motos-api/` e verificar a aprovação dos 4 testes (`Create_WithoutVeiculoId_ThrowsValidationException`, `ChangeStatus_ValidTransition_Succeeds`, `ChangeStatus_InvalidTransition_ThrowsException` e `CalculateTotal_WithPecasAndServicos_IsCorrect`).

- [x] T005 [P] [US2] Atualizar a interface `IOrdemServicoService` adicionando os contratos `ChangeStatusAsync(long id, OrdemServicoStatus novoStatus)`, `CalculateTotal(OrdemServico ordem)` e `CalculateTotalAsync(long id)` em `oficina-motos-api/src/OficinaMotos.Application/Interfaces/OrdemServico/IOrdemServicoService.cs` (`T-028.2`, `T-028.3`, `T-028.4`)
- [x] T006 [US2] Atualizar `OrdemServicoService` para validar `CreateOrdemServicoDTO` com `CreateOrdemServicoValidator` em `CreateAsync` (lançando `ValidationException` quando `VeiculoId <= 0`), validar transições de `OrdemServicoStatus` em `ChangeStatusAsync` e `UpdateAsync` (lançando `DomainException` em transições inválidas) e implementar `CalculateTotal` / `CalculateTotalAsync` somando peças e serviços em `oficina-motos-api/src/OficinaMotos.Application/Services/OrdemServico/OrdemServicoService.cs` (`T-028.1`, `T-028.2`, `T-028.3`, `T-028.4`)
- [x] T007 [US2] Criar a suíte `OrdemServicoServiceTests` implementando `Create_WithoutVeiculoId_ThrowsValidationException` (`T-028.1`), `ChangeStatus_ValidTransition_Succeeds` (`T-028.2`), `ChangeStatus_InvalidTransition_ThrowsException` (`T-028.3`) e `CalculateTotal_WithPecasAndServicos_IsCorrect` (`T-028.4`) em `oficina-motos-api/src/OficinaMotos.Tests/Services/OrdemServicoServiceTests.cs`

**Checkpoint**: Todos os 4 cenários de teste de `OrdemServicoService` implementados e passando (`US-028` completa).

---

## Phase 4: Polish, Execução de Testes & Governança

**Purpose**: Validar a compilação completa da solução (`dotnet build`), a execução de toda a suíte de testes (`dotnet test`) sem regressões e atualizar o status de `US-027` e `US-028` no backlog de governança (se presente).

- [x] T008 Executar `dotnet build OficinaMotos.slnx` e `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` em `oficina-motos-api/` confirmando 100% de aprovação em todas as suítes
- [x] T009 Atualizar o status das tarefas `T-027.1`..`T-027.4` e `T-028.1`..`T-028.4` para `[x]` em `governance/backlog.md` (caso o arquivo exista no workspace)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Foundational)**: `T001` e `T002 [P]` — concluída antes de `Phase 2` e `Phase 3`.
- **Phase 2 (`US1` — `US-027` `ClienteServiceTests`)**: `T003` depende de `T001`; `T004` depende de `T003`. Pode rodar em paralelo com a `Phase 3`.
- **Phase 3 (`US2` — `US-028` `OrdemServicoServiceTests`)**: `T005 [P]` → `T006` (depende de `T001` e `T005`) → `T007` (depende de `T006`). Pode rodar em paralelo com a `Phase 2`.
- **Phase 4 (Polish & Validação)**: `T008` → `T009` (depende da conclusão das Phases 2 e 3).

### Parallel Opportunities

- `T001` (`DomainException.cs`), `T002` (`GlobalExceptionHandler.cs`) e `T005` (`IOrdemServicoService.cs`) editam arquivos independentes e podem ser executadas em paralelo.
- Após `T001`, toda a `Phase 2` (`ClienteService` + `ClienteServiceTests`) e a `Phase 3` (`OrdemServicoService` + `OrdemServicoServiceTests`) atuam em módulos distintos e podem ser desenvolvidas em paralelo.

---

## Parallel Example: Phase 2 & Phase 3

```bash
Task T003: "Atualizar ClienteService.CreateAsync para lançar DomainException em CPF duplicado"
Task T005: "Adicionar ChangeStatusAsync e CalculateTotal/CalculateTotalAsync em IOrdemServicoService"
Task T006: "Implementar validação de VeiculoId, ChangeStatusAsync e CalculateTotal em OrdemServicoService"
```

---

## Implementation Strategy

### MVP First (Phase 1 + Phase 2 — US-027)

1. Concluir `T001` e `T002` criando `DomainException`.
2. Concluir `T003` e `T004` implementando e validando `ClienteServiceTests` (`T-027.1` a `T-027.4`).

### Incremental Delivery (Phase 3 + Phase 4 — US-028)

1. Concluir `T005`, `T006` e `T007` implementando e validando `OrdemServicoServiceTests` (`T-028.1` a `T-028.4`).
2. Executar `dotnet build` e `dotnet test` na solução completa (`T008`) e atualizar o backlog (`T009`).
