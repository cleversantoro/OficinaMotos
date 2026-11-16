---
description: "Task list for feature 027-integration-angular-specs"
---

# Tasks: US-029, US-030 & US-031 — Testes de Integração de Controllers Críticos e Specs de Services, Guards e Interceptors Angular

**Input**: Design documents from `/specs/027-integration-angular-specs/`  
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/test-contracts.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`  
**Tests**: Suítes de testes de integração HTTP xUnit (`OficinaMotosWebApplicationFactory`, `AuthControllerIntegrationTests`, `ClienteControllerIntegrationTests`) em `oficina-motos-api/src/OficinaMotos.Tests/Integration/` (`US-029`) e suítes de testes unitários Angular/Vitest (`auth.service.spec.ts`, `clientes.service.spec.ts`, `ordens.service.spec.ts`, `auth.guard.spec.ts`, `auth.interceptor.spec.ts`, `error-interceptor.spec.ts`) em `oficina-motos-web/src/app/core/` (`US-030` & `US-031`).  
**Organization**: As tarefas estão organizadas por fases (`Foundational` → `US1` = `US-029` → `US2` = `US-030` → `US3` = `US-031` → `Polish`) e mapeadas diretamente para as tasks oficiais do backlog (`T-029.1` a `T-029.3`, `T-030.1` a `T-030.3` e `T-031.1` a `T-031.3`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Foundational — Suporte a Host de Testes em Memória na API e Contratos de `OrdensService`

**Purpose**: Expor o entry point `Program` da API .NET sem acionar `ServerVersion.AutoDetect` do MySQL durante testes de integração (`US1`) e expor os métodos `getAll()` e `changeStatus()` em `OrdensService` no Angular (`US2`).

**⚠️ CRITICAL**: Necessário antes da execução dos testes de integração (`US1`) e da spec `ordens.service.spec.ts` (`US2`).

- [x] T001 Atualizar `Program.cs` para condicionar o registro `UseMySql` / `ServerVersion.AutoDetect` fora do ambiente `"Testing"` e declarar `public partial class Program { }` ao final de `oficina-motos-api/src/OficinaMotos.API/Program.cs` (`T-029.1`)
- [x] T002 [P] Atualizar `DependencyInjection.AddInfrastructure` para registrar `UseMySql` / `ServerVersion.AutoDetect` apenas quando `UseInMemoryDatabase` não estiver ativo na configuração em `oficina-motos-api/src/OficinaMotos.Infrastructure/IoC/DependencyInjection.cs` (`T-029.1`)
- [x] T003 [P] Adicionar os métodos `getAll()` e `changeStatus()` em `OrdensService` mantendo compatibilidade com `list()` e `update()` em `oficina-motos-web/src/app/core/services/ordens.service.ts` (`T-030.3`)

**Checkpoint**: `Program` pronto para hospedagem via `WebApplicationFactory<Program>` sem conexão MySQL externa e `OrdensService` expondo `getAll()`, `create()` e `changeStatus()`.

---

## Phase 2: User Story 1 — Testes de Integração para Controllers Críticos (Priority: P1) 🎯 MVP (`US-029`: `T-029.1` a `T-029.3`)

**Goal**: Criar a `OficinaMotosWebApplicationFactory` com banco `EF Core InMemory`, configuração JWT de teste e seed de usuário `Administrador` (`T-029.1`), e implementar as suítes `AuthControllerIntegrationTests` (`T-029.2`) e `ClienteControllerIntegrationTests` (`T-029.3`).

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Integration"` em `oficina-motos-api/` e verificar 100% de aprovação em `AuthControllerIntegrationTests` e `ClienteControllerIntegrationTests`.

- [x] T004 [US1] Criar `OficinaMotosWebApplicationFactory` herdando de `WebApplicationFactory<Program>` com banco `InMemory` isolado, chave JWT de teste, seed de usuário/perfil `Administrador` e helper de cliente HTTP autenticado em `oficina-motos-api/src/OficinaMotos.Tests/Integration/OficinaMotosWebApplicationFactory.cs` (`T-029.1`)
- [x] T005 [P] [US1] Criar `AuthControllerIntegrationTests` cobrindo login válido (`200 OK` com token JWT e refresh token) e login inválido (`401 Unauthorized` com senha incorreta ou usuário inexistente) em `oficina-motos-api/src/OficinaMotos.Tests/Integration/AuthControllerIntegrationTests.cs` (`T-029.2`)
- [x] T006 [P] [US1] Criar `ClienteControllerIntegrationTests` cobrindo `401 Unauthorized` sem token e fluxo completo de CRUD autenticado (`POST`, `GET`, `PUT`, `DELETE`) em `oficina-motos-api/src/OficinaMotos.Tests/Integration/ClienteControllerIntegrationTests.cs` (`T-029.3`)

**Checkpoint**: Todos os testes de integração de `AuthController` e `ClientesController` implementados e aprovados (`US-029` completa).

---

## Phase 3: User Story 2 — Specs para Services Angular Críticos (Priority: P1) (`US-030`: `T-030.1` a `T-030.3`)

**Goal**: Garantir cobertura unitária completa para os três services críticos do frontend Angular: `AuthService` (`login()`, `logout()`, `isAuthenticated()`, signal `currentUser`) (`T-030.1`), `ClientesService` (CRUD completo) (`T-030.2`) e `OrdensService` (`getAll()`, `create()`, `changeStatus()`) (`T-030.3`).

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` e verificar a aprovação integral de `auth.service.spec.ts`, `clientes.service.spec.ts` e `ordens.service.spec.ts`.

- [x] T007 [P] [US2] Atualizar `auth.service.spec.ts` para testar `login()`, `logout()`, `isAuthenticated()` (sessão ativa, expirada e inexistente) e a reatividade do signal `currentUser()` em `oficina-motos-web/src/app/core/auth/auth.service.spec.ts` (`T-030.1`)
- [x] T008 [P] [US2] Atualizar `clientes.service.spec.ts` para testar o CRUD completo (`list()`, `get()`, `create()`, `update()`, `delete()`, `search()` e `listTable()`) em `oficina-motos-web/src/app/core/services/clientes.service.spec.ts` (`T-030.2`)
- [x] T009 [P] [US2] Atualizar `ordens.service.spec.ts` para testar `getAll()`, `create()` e `changeStatus()` em `oficina-motos-web/src/app/core/services/ordens.service.spec.ts` (`T-030.3`)

**Checkpoint**: Suítes unitárias de `AuthService`, `ClientesService` e `OrdensService` implementadas e aprovadas (`US-030` completa).

---

## Phase 4: User Story 3 — Specs para Guards e Interceptors Angular (Priority: P2) (`US-031`: `T-031.1` a `T-031.3`)

**Goal**: Garantir cobertura unitária para `authGuard` (redirecionamento de usuário não autenticado para `/login`) (`T-031.1`), `authInterceptor` (injeção do header `Authorization: Bearer <token>`) (`T-031.2`) e `errorInterceptor` (`401 Unauthorized` acionando `logout()`) (`T-031.3`).

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` e verificar a aprovação integral de `auth.guard.spec.ts`, `auth.interceptor.spec.ts` e `error-interceptor.spec.ts`.

- [x] T010 [P] [US3] Criar `auth.guard.spec.ts` testando que usuário não autenticado é redirecionado para `/login` (`UrlTree`) e que usuário autenticado recebe `true` em `oficina-motos-web/src/app/core/auth/auth.guard.spec.ts` (`T-031.1`)
- [x] T011 [P] [US3] Criar `auth.interceptor.spec.ts` testando que o token JWT é injetado no header `Authorization: Bearer <token>` (e omitido quando ausente) em `oficina-motos-web/src/app/core/auth/auth.interceptor.spec.ts` (`T-031.2`)
- [x] T012 [P] [US3] Atualizar `error-interceptor.spec.ts` testando que resposta HTTP `401 Unauthorized` chama `authService.logout()` e exibe toast de erro em `oficina-motos-web/src/app/core/interceptors/error-interceptor.spec.ts` (`T-031.3`)

**Checkpoint**: Suítes unitárias de `authGuard`, `authInterceptor` e `errorInterceptor` implementadas e aprovadas (`US-031` completa).

---

## Phase 5: Polish, Execução de Testes & Governança

**Purpose**: Validar a compilação e execução completa das suítes de testes do backend (`dotnet test`) e do frontend (`npm test`), e atualizar o backlog de governança (`governance/backlog.md`).

- [x] T013 Executar `dotnet build OficinaMotos.slnx` e `dotnet test OficinaMotos.slnx` em `oficina-motos-api/` confirmando 100% de aprovação nos testes unitários e de integração
- [x] T014 Executar `npm test -- --watch=false` em `oficina-motos-web/` confirmando 100% de aprovação em todas as specs Angular
- [x] T015 Atualizar o status de `US-029` (`T-029.1`..`T-029.3`), `US-030` (`T-030.1`..`T-030.3`) e `US-031` (`T-031.1`..`T-031.3`) para concluído (`[x]`) em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Foundational)**: `T001`, `T002 [P]` e `T003 [P]` — destravam os testes de integração (`Phase 2`) e a spec de `OrdensService` (`Phase 3`).
- **Phase 2 (`US1` — `US-029` Integration Tests)**: `T004` depende de `T001` e `T002`; `T005 [P]` e `T006 [P]` dependem de `T004`.
- **Phase 3 (`US2` — `US-030` Angular Services Specs)**: `T007 [P]` e `T008 [P]` podem iniciar imediatamente; `T009 [P]` depende de `T003`.
- **Phase 4 (`US3` — `US-031` Angular Guards & Interceptors Specs)**: `T010 [P]`, `T011 [P]` e `T012 [P]` atuam em arquivos independentes e podem rodar em paralelo com a `Phase 3`.
- **Phase 5 (Polish & Validação)**: `T013`, `T014` e `T015` dependem da conclusão das Phases 2, 3 e 4.

### Parallel Opportunities

- `T001`/`T002` (backend) e `T003` (frontend) são completamente independentes.
- Após `T004`, `T005` (`AuthControllerIntegrationTests.cs`) e `T006` (`ClienteControllerIntegrationTests.cs`) podem ser implementadas em paralelo.
- Todas as 6 suítes Angular (`T007`, `T008`, `T009`, `T010`, `T011`, `T012`) editam arquivos `.spec.ts` distintos e podem ser implementadas em paralelo.

---

## Parallel Example: User Stories 1, 2 & 3

```bash
# Backend Integration Tests (após T004):
Task T005: "Criar AuthControllerIntegrationTests em oficina-motos-api/src/OficinaMotos.Tests/Integration/AuthControllerIntegrationTests.cs"
Task T006: "Criar ClienteControllerIntegrationTests em oficina-motos-api/src/OficinaMotos.Tests/Integration/ClienteControllerIntegrationTests.cs"

# Frontend Angular Specs (em paralelo):
Task T007: "Atualizar auth.service.spec.ts em oficina-motos-web/src/app/core/auth/auth.service.spec.ts"
Task T008: "Atualizar clientes.service.spec.ts em oficina-motos-web/src/app/core/services/clientes.service.spec.ts"
Task T009: "Atualizar ordens.service.spec.ts em oficina-motos-web/src/app/core/services/ordens.service.spec.ts"
Task T010: "Criar auth.guard.spec.ts em oficina-motos-web/src/app/core/auth/auth.guard.spec.ts"
Task T011: "Criar auth.interceptor.spec.ts em oficina-motos-web/src/app/core/auth/auth.interceptor.spec.ts"
Task T012: "Atualizar error-interceptor.spec.ts em oficina-motos-web/src/app/core/interceptors/error-interceptor.spec.ts"
```

---

## Implementation Strategy

### MVP First (Phase 1 + Phase 2 — US-029)

1. Concluir `T001` e `T002` habilitando o host de testes da API sem dependência de MySQL.
2. Concluir `T004`, `T005` e `T006` entregando e validando `OficinaMotosWebApplicationFactory`, `AuthControllerIntegrationTests` e `ClienteControllerIntegrationTests`.

### Incremental Delivery (Phase 3 + Phase 4 + Phase 5 — US-030 & US-031)

1. Concluir `T003` e as specs de services (`T007`, `T008`, `T009`).
2. Concluir as specs de guard e interceptors (`T010`, `T011`, `T012`).
3. Executar `dotnet test` (`T013`) e `npm test` (`T014`) e atualizar `governance/backlog.md` (`T015`).
