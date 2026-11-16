# Research: Testes de Integração de Controllers Críticos e Specs de Services, Guards e Interceptors Angular (US-029, US-030 & US-031)

**Feature**: `027-integration-angular-specs`  
**Date**: 2026-11-16

---

## 1. Hospedagem em Memória com `OficinaMotosWebApplicationFactory` (`T-029.1`)

- **Decision**:
  1. Adicionar `public partial class Program { }` ao final de `oficina-motos-api/src/OficinaMotos.API/Program.cs` para tornar o entry point visível para `WebApplicationFactory<Program>`.
  2. Ajustar o registro de `OficinaContext` em `Program.cs` e `OficinaMotos.Infrastructure/IoC/DependencyInjection.cs` para não registrar o provedor MySQL (`ServerVersion.AutoDetect(connectionString)`) quando a aplicação estiver em ambiente de teste (`UseInMemoryDatabase == true` ou `EnvironmentName == "Testing"`) e/ou remover os descritores `DbContextOptions<OficinaContext>` e `IDbContextOptionsConfiguration<OficinaContext>` dentro de `OficinaMotosWebApplicationFactory.ConfigureWebHost`.
  3. Na `OficinaMotosWebApplicationFactory`, configurar um banco `EF Core InMemory` exclusivo por instância da factory (`Guid.NewGuid().ToString()`), definir `Jwt:Key` com pelo menos 32 bytes e realizar o seed inicial de um usuário ativo (`SegUsuario`) com senha hasheada via `BCrypt.Net.BCrypt.HashPassword`, perfil `Administrador` (`SegPerfil`, `SegUsuarioPerfil`) e permissões (`SegModulo`, `SegPermissao`, `SegPerfilPermissao`).
- **Rationale**:
  - Em `Program.cs` (linha 71) e `OficinaMotos.Infrastructure/IoC/DependencyInjection.cs` (linha 31), `AddDbContext<OficinaContext>` é chamado duas vezes com `ServerVersion.AutoDetect(connectionString)`. No EF Core 8, cada chamada a `AddDbContext` registra um `IDbContextOptionsConfiguration<OficinaContext>`. Se a `WebApplicationFactory` apenas chamar `services.AddDbContext<OficinaContext>(o => o.UseInMemoryDatabase(...))` sem remover `IDbContextOptionsConfiguration<OficinaContext>` (ou sem pular o registro MySQL no ambiente `Testing`), o EF Core 8 tenta executar `ServerVersion.AutoDetect` (falhando sem MySQL local) ou lança `InvalidOperationException: Only a single database provider can be registered`.
  - Condicionar o registro MySQL em `Program.cs` e `DependencyInjection.cs` quando o ambiente for `"Testing"` (além de limpar descritores na factory) torna o boot de `OficinaMotosWebApplicationFactory` instantâneo, determinístico e 100% livre de rede.
- **Alternatives considered**:
  - *Subir container MySQL via Testcontainers*: descartado pois `US-025` e `US-029` padronizaram `Microsoft.EntityFrameworkCore.InMemory` + `Microsoft.AspNetCore.Mvc.Testing` já instalados em `OficinaMotos.Tests.csproj`.

---

## 2. Testes de Integração de `AuthController` e `ClientesController` (`T-029.2`, `T-029.3`)

- **Decision**:
  - Criar `AuthControllerIntegrationTests` em `oficina-motos-api/src/OficinaMotos.Tests/Integration/AuthControllerIntegrationTests.cs` testando:
    - `POST /api/v1/Auth/login` com credenciais válidas do usuário semeado → retorna `200 OK` e payload `LoginResponseDTO` com `Token` JWT válido, `RefreshToken`, `Email` e `Roles` contendo `"Administrador"`.
    - `POST /api/v1/Auth/login` com senha incorreta ou e-mail inexistente → retorna `401 Unauthorized`.
  - Criar `ClienteControllerIntegrationTests` em `oficina-motos-api/src/OficinaMotos.Tests/Integration/ClienteControllerIntegrationTests.cs` testando:
    - Requisição `GET /api/v1/Clientes` sem header `Authorization` → retorna `401 Unauthorized`.
    - Fluxo CRUD autenticado com `Authorization: Bearer <token>` (obtido via login na própria API de teste):
      1. `POST /api/v1/Clientes` com `CreateClienteDTO` válido → retorna `201 Created` e `ClienteResponseDTO` com `Id > 0`;
      2. `GET /api/v1/Clientes/{id}` e `GET /api/v1/Clientes` → retornam `200 OK` contendo o cliente criado;
      3. `PUT /api/v1/Clientes/{id}` com `UpdateClienteDTO` (requer role `Administrador,Gerente,Recepcionista`, atendida pelo token do usuário admin semeado) → retorna `200 OK` com dados atualizados;
      4. `DELETE /api/v1/Clientes/{id}` → retorna `204 NoContent`, e `GET /api/v1/Clientes/{id}` subsequente retorna `404 NotFound`.
- **Rationale**:
  - Exercita o pipeline HTTP real (`Program.cs`, autenticação JWT Bearer, política `[Authorize]` e `[Authorize(Roles = "Administrador,Gerente,Recepcionista")]`, serialização JSON com `JsonStringEnumConverter`, AutoMapper, `ClienteService` e `ClienteRepository` com Soft Delete).
- **Alternatives considered**:
  - *Instanciar controllers diretamente com `new AuthController(...)`*: descartado porque não testaria o pipeline HTTP, filtros de autorização `[Authorize]` nem serialização JSON exigidos em `US-029`.

---

## 3. Specs para Services Angular Críticos (`US-030`: `T-030.1`, `T-030.2`, `T-030.3`)

- **Decision**:
  - **`auth.service.spec.ts` (`T-030.1`)**: Expandir a suíte existente em `src/app/core/auth/auth.service.spec.ts` para validar explicitamente:
    - `login()`: envia `POST` para `apiPaths.auth.login`, persiste `oficina_token`, `oficina_refresh_token` e `oficina_user` no `localStorage`, atualiza o signal `currentUser()` e faz `isAuthenticated()` retornar `true`.
    - `logout()`: envia `POST` para `apiPaths.auth.logout` (quando há refresh token), limpa `oficina_token`, `oficina_refresh_token` e `oficina_user` do `localStorage`, zera o signal `currentUser()` (`null`), faz `isAuthenticated()` retornar `false` e navega para `['/login']` via `Router.navigate`.
    - `isAuthenticated()` e `currentUser` signal: retorna `false` antes do login ou quando `expiresAt` está no passado, e reage imediatamente às mudanças do signal `_currentUser`.
  - **`clientes.service.spec.ts` (`T-030.2`)**: Expandir `src/app/core/services/clientes.service.spec.ts` para cobrir o CRUD completo (`list()`, `get(id)`, `create(body)`, `update(id, body)`, `delete(id)`) além de `search(term)` e `listTable()`.
  - **`ordens.service.ts` e `ordens.service.spec.ts` (`T-030.3`)**: Adicionar os métodos `getAll<T = OrdemServico>(params?: Record<string, any>)` (delegando para `this.list<T>(params)`) e `changeStatus<T = OrdemServico>(id: string | number, status: string, body?: Partial<UpdateOrdemServicoRequest>)` em `OrdensService`, e expandir `src/app/core/services/ordens.service.spec.ts` para testar `getAll()`, `create()` e `changeStatus()`.
- **Rationale**:
  - O backlog especifica literalmente `T-030.3 — ordens.service.spec.ts: getAll(), create(), changeStatus()`. Expor `getAll()` e `changeStatus()` em `OrdensService` mantendo `list()` e `update()` garante aderência literal à task sem quebrar nenhum componente existente.
- **Alternatives considered**:
  - *Testar apenas `list()` e `update()` sem criar `getAll()` e `changeStatus()`*: descartado para não divergir da nomenclatura explícita da task `T-030.3`.

---

## 4. Specs para Guards e Interceptors Angular (`US-031`: `T-031.1`, `T-031.2`, `T-031.3`)

- **Decision**:
  - **`auth.guard.spec.ts` (`T-031.1`)**: Criar `src/app/core/auth/auth.guard.spec.ts` executando `authGuard` via `TestBed.runInInjectionContext`:
    - Quando `authService.isAuthenticated()` é `false`, retorna `UrlTree` apontando para `/login` (`router.serializeUrl(result as UrlTree)` igual a `'/login'`).
    - Quando `authService.isAuthenticated()` é `true`, retorna `true`.
  - **`auth.interceptor.spec.ts` (`T-031.2`)**: Criar `src/app/core/auth/auth.interceptor.spec.ts` testando `authInterceptor` com `provideHttpClient(withInterceptors([authInterceptor]))` e `provideHttpClientTesting()`:
    - Quando `authService.getToken()` retorna um token válido, a requisição HTTP recebe o header `Authorization: Bearer <token>` e o serviço `Loading.show()` é chamado e finalizado no término da requisição.
    - Quando `authService.getToken()` retorna `null`, a requisição segue sem header `Authorization`.
  - **`error-interceptor.spec.ts` (`T-031.3`)**: Expandir `src/app/core/interceptors/error-interceptor.spec.ts` (que já existe no projeto) para testar:
    - Resposta `401 Unauthorized` sem refresh token (ou com falha no refresh) → chama `authService.logout()` e `toast.error(...)`.
    - Resposta `403 Forbidden` → chama `toast.error('Acesso negado', ...)` sem deslogar.
    - Resposta `500 Internal Server Error` → chama `toast.error('Erro no servidor', ...)`.
- **Rationale**:
  - Reutiliza o arquivo existente `src/app/core/interceptors/error-interceptor.spec.ts` (evitando duplicidade) e adiciona os cenários exigidos por `T-031.3` e pelo Princípio VI da Constituição.
- **Alternatives considered**:
  - *Criar um segundo arquivo `error.interceptor.spec.ts` duplicado*: descartado pois `error-interceptor.spec.ts` já existe junto a `error-interceptor.ts` em `src/app/core/interceptors/`.
