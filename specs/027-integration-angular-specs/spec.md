# Especificação de Funcionalidade: Testes de Integração de Controllers Críticos e Specs de Services, Guards e Interceptors Angular (US-029, US-030 & US-031)

**Feature Branch**: `027-integration-angular-specs`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-029 — Testes de integração para controllers críticos | Prioridade: 🟢 Could | Estimativa: L | Sprint: 5 | Depende: US-025, US-001 | Tasks: [ ] T-029.1 — Criar OficinaMotosWebApplicationFactory, [ ] T-029.2 — AuthControllerIntegrationTests: login válido, login inválido, [ ] T-029.3 — ClienteControllerIntegrationTests: 401 sem token, CRUD autenticado || US-030 — Specs para services Angular críticos | Prioridade: 🟡 Should | Estimativa: M | Sprint: 5 | Tasks: [ ] T-030.1 — auth.service.spec.ts: login(), logout(), isAuthenticated(), signal, [ ] T-030.2 — clientes.service.spec.ts: CRUD completo, [ ] T-030.3 — ordens.service.spec.ts: getAll(), create(), changeStatus() || US-031 — Specs para guards e interceptors Angular | Prioridade: 🟡 Should | Estimativa: S | Sprint: 5 | Depende: US-030 | Tasks: [ ] T-031.1 — auth.guard.spec.ts: usuário não autenticado é redirecionado, [ ] T-031.2 — auth.interceptor.spec.ts: token injetado no header correto, [ ] T-031.3 — error.interceptor.spec.ts: 401 chama logout()"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Validar Endpoints Críticos da API (`AuthController` e `ClientesController`) com Testes de Integração HTTP (Priority: P1)

Como engenheiro de software do backend, quero dispor de uma infraestrutura de testes de integração baseada em `WebApplicationFactory<Program>` (`OficinaMotosWebApplicationFactory`) (`T-029.1`) e suítes de testes de integração para `AuthController` (`AuthControllerIntegrationTests`) (`T-029.2`) e `ClientesController` (`ClienteControllerIntegrationTests`) (`T-029.3`), validando o pipeline HTTP completo da API (roteamento `/api/v1/`, serialização JSON, autenticação/autorização JWT e persistência isolada em banco `InMemory`) tanto para credenciais válidas/inválidas de login quanto para proteção `401 Unauthorized` sem token e operações de CRUD autenticado de clientes.

**Why this priority**: Cumpre integralmente a `US-029` (`T-029.1` a `T-029.3`), assegurando que a camada de apresentação HTTP da API (`OficinaMotos.API`), a configuração de segurança JWT (`US-001`) e os contratos RESTful versionados (`/api/v1/Auth` e `/api/v1/Clientes`, Princípios II e III da Constituição) funcionem de ponta a ponta sem depender de instância MySQL externa.

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Integration"` a partir de `oficina-motos-api/` e verificar a aprovação de todos os testes de `AuthControllerIntegrationTests` e `ClienteControllerIntegrationTests`.

**Acceptance Scenarios**:

1. **Given** o projeto `OficinaMotos.API` com `public partial class Program` exposto para hospedagem em memória, **When** `OficinaMotosWebApplicationFactory` (`T-029.1`) inicializa o host de teste substituindo o `DbContextOptions<OficinaContext>` MySQL por um banco `InMemory` isolado, configurando `Jwt:Key` de teste e realizando o seed de usuário e perfil administrativo ativo, **Then** clientes HTTP criados pela factory executam requisições contra o pipeline real da API sem dependência de banco externo.
2. **Given** um usuário ativo previamente semeado no contexto da `OficinaMotosWebApplicationFactory`, **When** `AuthControllerIntegrationTests` (`T-029.2`) envia `POST /api/v1/Auth/login` com credenciais válidas, **Then** a API responde `200 OK` contendo `LoginResponseDTO` com `token` JWT não vazio, `refreshToken`, `email` e `roles`; e **When** envia `POST /api/v1/Auth/login` com senha incorreta ou usuário inexistente, **Then** a API responde `401 Unauthorized`.
3. **Given** um cliente HTTP sem cabeçalho `Authorization`, **When** `ClienteControllerIntegrationTests` (`T-029.3`) envia `GET /api/v1/Clientes` (ou demais operações protegidas), **Then** o pipeline de autenticação/autorização da API rejeita a requisição com `401 Unauthorized`.
4. **Given** um cliente HTTP autenticado com `Authorization: Bearer <token>` obtido para perfil autorizado (`Administrador`), **When** `ClienteControllerIntegrationTests` (`T-029.3`) executa o fluxo completo de CRUD em `/api/v1/Clientes` (`POST` criação, `GET` listagem e busca por ID, `PUT` atualização e `DELETE` exclusão lógica), **Then** as respostas retornam `201 Created`, `200 OK`, `200 OK`, `204 NoContent` e `404 NotFound` ao consultar o ID excluído.

---

### User Story 2 - Validar Services Angular Críticos (`AuthService`, `ClientesService`, `OrdensService`) com Testes Unitários (Priority: P1)

Como engenheiro de software do frontend, quero dispor de suítes completas de testes unitários em `auth.service.spec.ts` (`T-030.1`), `clientes.service.spec.ts` (`T-030.2`) e `ordens.service.spec.ts` (`T-030.3`), garantindo cobertura dos fluxos de sessão reativa via Signals (`login()`, `logout()`, `isAuthenticated()`, `currentUser`), das operações de CRUD de clientes e das operações principais de Ordens de Serviço (`getAll()`/`list()`, `create()` e `changeStatus()`/`update()`).

**Why this priority**: Cumpre integralmente a `US-030` (`T-030.1` a `T-030.3`) e atende aos Princípios II, IV e VI da Constituição (centralização em `apiPaths`, estado reativo com Signals e cobertura mínima de testes em serviços críticos do frontend).

**Independent Test**: Executar `npm test -- --watch=false` a partir de `oficina-motos-web/` e verificar a aprovação integral das suítes `auth.service.spec.ts`, `clientes.service.spec.ts` e `ordens.service.spec.ts`.

**Acceptance Scenarios**:

1. **Given** o `AuthService` configurado com `HttpTestingController` e `Router`, **When** `auth.service.spec.ts` (`T-030.1`) executa `login()`, **Then** o token, o refresh token e os dados do usuário são salvos no `localStorage`, o signal `currentUser()` passa a refletir o usuário autenticado e o computed signal `isAuthenticated()` retorna `true`; e **When** `logout()` é invocado, **Then** os itens do `localStorage` são removidos, `currentUser()` passa a `null`, `isAuthenticated()` passa a `false` e o `Router` navega para `['/login']`.
2. **Given** o `ClientesService` injetado nos testes unitários, **When** `clientes.service.spec.ts` (`T-030.2`) executa as operações de CRUD completo (`list()`, `get(id)`, `create(body)`, `update(id, body)` e `delete(id)`), **Then** cada método dispara a requisição HTTP correspondente (`GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`) para `apiPaths.clientes.base` com os payloads e parâmetros esperados.
3. **Given** o `OrdensService` injetado nos testes unitários, **When** `ordens.service.spec.ts` (`T-030.3`) executa `getAll()` (ou `list()`), `create()` e `changeStatus()` (atualização de status da OS via `apiPaths.ordens.base`), **Then** cada chamada realiza a requisição HTTP esperada (`GET /api/v1/OrdemServicos`, `POST /api/v1/OrdemServicos` e `PUT`/`PATCH` de mudança de status da OS) e propaga a resposta tipada ao assinante.

---

### User Story 3 - Validar Guards e Interceptors Angular (`authGuard`, `authInterceptor`, `errorInterceptor`) com Testes Unitários (Priority: P2)

Como engenheiro de software do frontend, quero dispor de suítes completas de testes unitários para o guard de autenticação (`auth.guard.spec.ts`) (`T-031.1`), o interceptor de injeção de token (`auth.interceptor.spec.ts`) (`T-031.2`) e o interceptor de tratamento global de erros (`error.interceptor.spec.ts` / `error-interceptor.spec.ts`) (`T-031.3`), garantindo que rotas protegidas redirecionem usuários não autenticados, que o cabeçalho `Authorization: Bearer <token>` seja anexado corretamente e que respostas `401 Unauthorized` acionem `logout()`.

**Why this priority**: Cumpre integralmente a `US-031` (`T-031.1` a `T-031.3`), assegurando por testes automatizados o cumprimento das regras inegociáveis dos Princípios III e VI da Constituição para proteção de rotas e interceptação HTTP no Angular.

**Independent Test**: Executar `npm test -- --watch=false` a partir de `oficina-motos-web/` e verificar a aprovação integral das suítes de `authGuard`, `authInterceptor` e `errorInterceptor`.

**Acceptance Scenarios**:

1. **Given** um usuário não autenticado (`authService.isAuthenticated()` retornando `false`), **When** `auth.guard.spec.ts` (`T-031.1`) executa `authGuard` dentro do contexto de injeção do Angular, **Then** o guard bloqueia a ativação da rota e retorna um `UrlTree` redirecionando para `/login` (e retorna `true` quando `isAuthenticated()` é `true`).
2. **Given** um token JWT presente no `AuthService` (`getToken()` retornando `'jwt-token-valido'`), **When** `auth.interceptor.spec.ts` (`T-031.2`) intercepta uma requisição HTTP, **Then** a requisição encaminhada contém o cabeçalho `Authorization: Bearer jwt-token-valido` e o serviço `Loading` é acionado e finalizado ao término da requisição (e, quando não há token, o cabeçalho `Authorization` não é adicionado).
3. **Given** uma requisição HTTP que recebe resposta `401 Unauthorized` sem possibilidade de renovação por refresh token (ou após falha de refresh), **When** `error.interceptor.spec.ts` / `error-interceptor.spec.ts` (`T-031.3`) processa o erro, **Then** o interceptor exibe notificação de sessão expirada via `Toast` e invoca `authService.logout()`.

---

### Edge Cases

- **Hospedagem do `Program` em `WebApplicationFactory` (`T-029.1`)**: Como `Program.cs` utiliza top-level statements, é necessário declarar `public partial class Program { }` ao final de `Program.cs` e substituir a configuração do `DbContextOptions<OficinaContext>` (originalmente registrada com `UseMySql` e `ServerVersion.AutoDetect(connectionString)`, que tentaria abrir conexão MySQL no startup) antes que o container construa o `OficinaContext`, ou condicionar o registro MySQL em `Program.cs` / remover os descritores de `DbContextOptions<OficinaContext>` na `OficinaMotosWebApplicationFactory`.
- **Métodos `getAll()` e `changeStatus()` em `OrdensService` (`T-030.3`)**: Atualmente `OrdensService` expõe `list()` e `update()`. Para atender de forma explícita e retrocompatível à task `T-030.3` (`getAll(), create(), changeStatus()`), `OrdensService` deve disponibilizar os métodos `getAll()` (delegando/equivalente a `list()`) e `changeStatus(id, status, ...)` (realizando a atualização de status da OS via `apiPaths.ordens.base`), mantendo `list()` e `update()` intactos para os componentes existentes.
- **Nomenclatura de `error-interceptor.spec.ts` vs `error.interceptor.spec.ts` (`T-031.3`)**: O repositório já possui `src/app/core/interceptors/error-interceptor.ts` e `src/app/core/interceptors/error-interceptor.spec.ts`. Os testes de `T-031.3` (`401 chama logout()`, além de `403` e `500` previstos na Constituição) devem ser implementados garantindo que a suíte de `errorInterceptor` seja executada pelo runner de testes do projeto sem arquivos duplicados conflitantes.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O arquivo `oficina-motos-api/src/OficinaMotos.API/Program.cs` DEVE expor `public partial class Program { }` e permitir que `OficinaMotosWebApplicationFactory` substitua o provedor MySQL pelo provedor `InMemory` sem tentar conectar a um servidor MySQL real durante a inicialização dos testes (`T-029.1`).
- **FR-002**: O projeto `OficinaMotos.Tests` DEVE conter a classe `OficinaMotosWebApplicationFactory` (em `oficina-motos-api/src/OficinaMotos.Tests/Integration/OficinaMotosWebApplicationFactory.cs`) herdando de `WebApplicationFactory<Program>`, configurando chave JWT de teste, banco de dados `InMemory` isolado e seed de dados mínimos para autenticação (`Usuario`, `Perfil`, `Permissao`) (`T-029.1`).
- **FR-003**: O projeto `OficinaMotos.Tests` DEVE conter a classe `AuthControllerIntegrationTests` (em `oficina-motos-api/src/OficinaMotos.Tests/Integration/AuthControllerIntegrationTests.cs`) validando via `HttpClient`:
  - Login válido em `POST /api/v1/Auth/login` retornando `200 OK` e token JWT válido (`T-029.2`);
  - Login inválido (credenciais incorretas ou ausentes) em `POST /api/v1/Auth/login` retornando `401 Unauthorized` / `400 BadRequest` (`T-029.2`).
- **FR-004**: O projeto `OficinaMotos.Tests` DEVE conter a classe `ClienteControllerIntegrationTests` (em `oficina-motos-api/src/OficinaMotos.Tests/Integration/ClienteControllerIntegrationTests.cs`) validando via `HttpClient`:
  - Requisição sem token JWT para `/api/v1/Clientes` retornando `401 Unauthorized` (`T-029.3`);
  - Fluxo completo de CRUD autenticado com Bearer token válido (`POST /api/v1/Clientes`, `GET /api/v1/Clientes`, `GET /api/v1/Clientes/{id}`, `PUT /api/v1/Clientes/{id}`, `DELETE /api/v1/Clientes/{id}`) (`T-029.3`).
- **FR-005**: O arquivo `oficina-motos-web/src/app/core/auth/auth.service.spec.ts` DEVE cobrir os cenários de `login()`, `logout()`, `isAuthenticated()` e atualização reativa do signal `currentUser()` (incluindo expiração de sessão e limpeza de `localStorage` + redirecionamento para `/login`) (`T-030.1`).
- **FR-006**: O arquivo `oficina-motos-web/src/app/core/services/clientes.service.spec.ts` DEVE cobrir o CRUD completo de `ClientesService` (`list`, `get`, `create`, `update`, `delete` e `search`) utilizando `apiPaths.clientes.base` (`T-030.2`).
- **FR-007**: O serviço `OrdensService` (`oficina-motos-web/src/app/core/services/ordens.service.ts`) DEVE expor `getAll()` e `changeStatus()`, e o arquivo `oficina-motos-web/src/app/core/services/ordens.service.spec.ts` DEVE cobrir `getAll()`, `create()` e `changeStatus()` utilizando `apiPaths.ordens.base` (`T-030.3`).
- **FR-008**: O projeto `oficina-motos-web` DEVE conter o arquivo `oficina-motos-web/src/app/core/auth/auth.guard.spec.ts` validando que um usuário não autenticado é redirecionado para `/login` (`UrlTree`) e que um usuário autenticado tem acesso permitido (`true`) (`T-031.1`).
- **FR-009**: O projeto `oficina-motos-web` DEVE conter o arquivo `oficina-motos-web/src/app/core/auth/auth.interceptor.spec.ts` validando que o token JWT retornado por `AuthService.getToken()` é injetado no header `Authorization: Bearer <token>` (e omitido quando ausente) (`T-031.2`).
- **FR-010**: A suíte de testes do interceptor de erro (`oficina-motos-web/src/app/core/interceptors/error-interceptor.spec.ts`) DEVE validar que uma resposta `401 Unauthorized` invoca `AuthService.logout()` (além de verificar o acionamento de `Toast` para erros HTTP) (`T-031.3`).

### Key Entities *(include if feature involves data)*

- **`OficinaMotosWebApplicationFactory`** (`OficinaMotos.Tests.Integration`): Fábrica de aplicação web em memória para testes de integração HTTP da API .NET (`T-029.1`).
- **`AuthController` / `ClientesController`** (`OficinaMotos.API.Controllers`): Controllers RESTful críticos sob teste de integração (`T-029.2`, `T-029.3`).
- **`AuthService` / `ClientesService` / `OrdensService`** (`oficina-motos-web/src/app/core`): Serviços Angular responsáveis por autenticação reativa com Signals e comunicação HTTP com os domínios de Clientes e Ordens de Serviço (`T-030.1` a `T-030.3`).
- **`authGuard` / `authInterceptor` / `errorInterceptor`** (`oficina-motos-web/src/app/core`): Guard e interceptors funcionais responsáveis pela proteção de rotas, injeção de token Bearer e tratamento global de erros HTTP (`T-031.1` a `T-031.3`).

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `dotnet test` em `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Integration"` executa e aprova 100% dos testes de integração de `AuthControllerIntegrationTests` e `ClienteControllerIntegrationTests` (`T-029.1` a `T-029.3`).
- **SC-002**: `dotnet test` em `oficina-motos-api/OficinaMotos.slnx` executa toda a suíte de testes do backend (unitários e de integração) com 100% de aprovação e zero regressões.
- **SC-003**: A execução da suíte de testes do frontend (`npm test -- --watch=false` em `oficina-motos-web/`) aprova 100% das specs de `auth.service.spec.ts`, `clientes.service.spec.ts`, `ordens.service.spec.ts`, `auth.guard.spec.ts`, `auth.interceptor.spec.ts` e `error-interceptor.spec.ts` (`T-030.1` a `T-030.3` e `T-031.1` a `T-031.3`).

## Assumptions

- No `Program.cs` da API, `ServerVersion.AutoDetect(connectionString)` abre uma conexão imediata com o MySQL durante a configuração do `AddDbContext` se for chamado diretamente fora do lambda ou durante a resolução das opções. A `OficinaMotosWebApplicationFactory` (em conjunto com um ajuste seguro em `Program.cs` para ambientes de teste ou substituição de `DbContextOptions<OficinaContext>`) garantirá que nenhuma conexão MySQL seja tentada nos testes de integração.
- No frontend Angular, `error-interceptor.spec.ts` já existe em `src/app/core/interceptors/error-interceptor.spec.ts` e será expandido para cobrir integralmente `T-031.3` (`401 chama logout()`).