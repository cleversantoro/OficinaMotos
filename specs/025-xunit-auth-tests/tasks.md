---
description: "Task list for feature 025-xunit-auth-tests"
---

# Tasks: US-025 & US-026 — Projeto xUnit OficinaMotos.Tests e Testes Unitários de AuthService

**Input**: Design documents from `/specs/025-xunit-auth-tests/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/test-contracts.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Suíte de testes xUnit em `oficina-motos-api/src/OficinaMotos.Tests/` utilizando `Moq`, `FluentAssertions`, `Microsoft.EntityFrameworkCore.InMemory` e `Microsoft.AspNetCore.Mvc.Testing`.
**Organization**: As tarefas são organizadas por História de Usuário (`US1` = `US-025`, `US2` = `US-026`) e mapeadas diretamente para as tasks oficiais do backlog (`T-025.1` a `T-025.4` e `T-026.1` a `T-026.4`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: User Story 1 — Criar e Configurar Projeto xUnit `OficinaMotos.Tests` (Priority: P1) 🎯 MVP (`US-025`: `T-025.1` a `T-025.4`)

**Goal**: Criar o projeto `OficinaMotos.Tests` em `oficina-motos-api/src/OficinaMotos.Tests`, adicioná-lo ao `OficinaMotos.slnx`, instalar os pacotes `Moq`, `FluentAssertions`, `Microsoft.EntityFrameworkCore.InMemory` e `Microsoft.AspNetCore.Mvc.Testing`, referenciar os projetos da solução e criar `Helpers/TestDbContextFactory.cs`.

**Independent Test**: Executar `dotnet build OficinaMotos.slnx` em `oficina-motos-api/` e validar que o projeto `OficinaMotos.Tests` e `TestDbContextFactory` compilam sem erros.

- [x] T001 [US1] Criar o projeto xUnit `OficinaMotos.Tests` com referências para `Domain`, `Application`, `Infrastructure` e `API` e pacotes `Moq`, `FluentAssertions`, `Microsoft.EntityFrameworkCore.InMemory` e `Microsoft.AspNetCore.Mvc.Testing` em `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` (`T-025.1`, `T-025.3`)
- [x] T002 [P] [US1] Adicionar o projeto `src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` ao arquivo de solução `oficina-motos-api/OficinaMotos.slnx` (`T-025.2`)
- [x] T003 [US1] Criar a classe utilitária `TestDbContextFactory` (instanciação isolada de `OficinaContext` com `UseInMemoryDatabase` e criação de `IMapper`) em `oficina-motos-api/src/OficinaMotos.Tests/Helpers/TestDbContextFactory.cs` (`T-025.4`)

**Checkpoint**: Projeto `OficinaMotos.Tests` configurado na solução `OficinaMotos.slnx` com `TestDbContextFactory` pronto para uso (`US-025` completa).

---

## Phase 2: User Story 2 — Adequação de `AuthService` e Implementação de `AuthServiceTests` (Priority: P1) (`US-026`: `T-026.1` a `T-026.4`)

**Goal**: Expor `GenerateToken` em `IAuthService` / `AuthService`, retornar o `Token` JWT em `LoginDataResult`, lançar exceção para conta bloqueada (`InvalidOperationException`, mantendo retorno `401` no `AuthController`) e implementar os 4 testes unitários obrigatórios em `AuthServiceTests`.

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` e verificar que `Login_ValidCredentials_ReturnsJwtToken`, `Login_InvalidPassword_ReturnsNull`, `Login_BlockedAccount_ThrowsException` e `GenerateToken_ContainsExpectedClaims` passam com 100% de sucesso.

- [x] T004 [US2] Atualizar `oficina-motos-api/src/OficinaMotos.Application/OficinaMotos.Application.csproj`, `oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/Auth/LoginDataResult.cs`, `oficina-motos-api/src/OficinaMotos.Application/Interfaces/Seguranca/IAuthService.cs`, `oficina-motos-api/src/OficinaMotos.Application/Services/Seguranca/AuthService.cs` e `oficina-motos-api/src/OficinaMotos.API/Controllers/Auth/AuthController.cs` para suportar `GenerateToken`, preenchimento de `Token` em `LoginAsync` e lançamento de `InvalidOperationException` em conta bloqueada (`T-026.1`, `T-026.2`, `T-026.3`, `T-026.4`)
- [x] T005 [US2] Implementar `Login_ValidCredentials_ReturnsJwtToken`, `Login_InvalidPassword_ReturnsNull`, `Login_BlockedAccount_ThrowsException` e `GenerateToken_ContainsExpectedClaims` usando `Moq` e `FluentAssertions` em `oficina-motos-api/src/OficinaMotos.Tests/Services/AuthServiceTests.cs` (`T-026.1`, `T-026.2`, `T-026.3`, `T-026.4`)

**Checkpoint**: Todos os 4 cenários de teste de `AuthService` implementados em `AuthServiceTests.cs` (`US-026` completa).

---

## Phase 3: Polish, Execução de Testes & Governança

**Purpose**: Validar a compilação completa da solução (`dotnet build`), a execução de todos os testes (`dotnet test`) e atualizar o status de `US-025` e `US-026` no backlog oficial.

- [x] T006 Executar `dotnet build OficinaMotos.slnx` e `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` em `oficina-motos-api/`
- [x] T007 Atualizar o status das tarefas `T-025.1`..`T-025.4` e `T-026.1`..`T-026.4` para `[x]` em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (`US-025` — Setup `OficinaMotos.Tests`)**: `T001` e `T002 [P]` → `T003`.
- **Phase 2 (`US-026` — `AuthService` & `AuthServiceTests`)**: `T004` pode rodar em paralelo com a Phase 1; `T005` depende de `T001`, `T003` e `T004`.
- **Phase 3 (Validação & Governança)**: `T006` → `T007`.

### Parallel Opportunities

- `T001` (`OficinaMotos.Tests.csproj`), `T002` (`OficinaMotos.slnx`) e `T004` (`AuthService` / `LoginDataResult`) atuam em arquivos distintos e podem ser executadas em paralelo.

---

## Parallel Example: Phase 1 & Phase 2

```bash
Task T001: "Criar OficinaMotos.Tests.csproj com pacotes xUnit, Moq, FluentAssertions, InMemory, Mvc.Testing"
Task T002: "Registrar OficinaMotos.Tests.csproj em OficinaMotos.slnx"
Task T004: "Atualizar AuthService, IAuthService, LoginDataResult e AuthController"
```

---

## Implementation Strategy

### MVP First (Phase 1 — US-025)

1. Concluir `T001` a `T003` criando `src/OficinaMotos.Tests`, registrando em `OficinaMotos.slnx` e implementando `TestDbContextFactory`.
2. Validar compilação do projeto de testes.

### Incremental Delivery (Phase 2 & Phase 3 — US-026)

1. Concluir `T004` e `T005` com a suíte completa `AuthServiceTests`.
2. Rodar `dotnet test` (`T006`) e atualizar `governance/backlog.md` (`T007`).
