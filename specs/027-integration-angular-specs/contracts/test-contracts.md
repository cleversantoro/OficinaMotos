# Contratos de Testes: Integração Backend (US-029) e Specs Frontend Angular (US-030 & US-031)

**Feature**: `027-integration-angular-specs`  
**Date**: 2026-11-16

---

## 1. Backend — Testes de Integração (`OficinaMotos.Tests/Integration/`)

### 1.1 `OficinaMotosWebApplicationFactory` (`T-029.1`)
- **Arquivo**: `oficina-motos-api/src/OficinaMotos.Tests/Integration/OficinaMotosWebApplicationFactory.cs`
- **Responsabilidades**:
  - Herdar de `WebApplicationFactory<Program>`.
  - Substituir a configuração de `OficinaContext` para usar `UseInMemoryDatabase` isolado por instância.
  - Configurar `Jwt:Key` e desabilitar exportadores externos de telemetria durante os testes.
  - Semear usuário ativo com perfil `Administrador` para permitir testes de login e endpoints protegidos por `[Authorize]` e `[Authorize(Roles = ...)]`.

### 1.2 `AuthControllerIntegrationTests` (`T-029.2`)
- **Arquivo**: `oficina-motos-api/src/OficinaMotos.Tests/Integration/AuthControllerIntegrationTests.cs`
- **Cenários obrigatórios**:
  1. `Login_WithValidCredentials_ReturnsOkAndJwtToken`: envia `POST /api/v1/Auth/login` com credenciais válidas e verifica `StatusCode == 200 OK`, `Token` JWT válido (3 segmentos), `RefreshToken` preenchido e `Email` correspondente.
  2. `Login_WithInvalidCredentials_ReturnsUnauthorized`: envia `POST /api/v1/Auth/login` com senha inválida (e/ou usuário inexistente) e verifica `StatusCode == 401 Unauthorized`.

### 1.3 `ClienteControllerIntegrationTests` (`T-029.3`)
- **Arquivo**: `oficina-motos-api/src/OficinaMotos.Tests/Integration/ClienteControllerIntegrationTests.cs`
- **Cenários obrigatórios**:
  1. `GetAll_WithoutToken_ReturnsUnauthorized`: envia `GET /api/v1/Clientes` sem header `Authorization` e verifica `StatusCode == 401 Unauthorized`.
  2. `Crud_WithAuthenticatedClient_Succeeds`: autentica via `POST /api/v1/Auth/login`, injeta `Bearer <token>` e executa:
     - `POST /api/v1/Clientes` → `201 Created` (`ClienteResponseDTO` com `Id > 0`);
     - `GET /api/v1/Clientes/{id}` → `200 OK`;
     - `PUT /api/v1/Clientes/{id}` → `200 OK` com nome/telefone atualizados;
     - `DELETE /api/v1/Clientes/{id}` → `204 NoContent`;
     - `GET /api/v1/Clientes/{id}` após exclusão → `404 NotFound`.

---

## 2. Frontend — Specs de Services Angular (`US-030`)

### 2.1 `auth.service.spec.ts` (`T-030.1`)
- **Arquivo**: `oficina-motos-web/src/app/core/auth/auth.service.spec.ts`
- **Cenários obrigatórios**:
  1. `login()` envia `POST` para `apiPaths.auth.login`, salva tokens/usuário no `localStorage`, atualiza o signal `currentUser()` e faz `isAuthenticated()` retornar `true`.
  2. `logout()` limpa `localStorage`, atualiza o signal `currentUser()` para `null`, faz `isAuthenticated()` retornar `false` e redireciona para `['/login']`.
  3. `isAuthenticated()` retorna `false` quando não há sessão ou quando `expiresAt` está expirado, e `true` quando a sessão é válida.

### 2.2 `clientes.service.spec.ts` (`T-030.2`)
- **Arquivo**: `oficina-motos-web/src/app/core/services/clientes.service.spec.ts`
- **Cenários obrigatórios**:
  1. `list()` realiza `GET` em `apiPaths.clientes.base`.
  2. `get(id)` realiza `GET` em `${apiPaths.clientes.base}/${id}`.
  3. `create(body)` realiza `POST` em `apiPaths.clientes.base` com `body`.
  4. `update(id, body)` realiza `PUT` em `${apiPaths.clientes.base}/${id}` com `body`.
  5. `delete(id)` realiza `DELETE` em `${apiPaths.clientes.base}/${id}`.

### 2.3 `ordens.service.spec.ts` (`T-030.3`)
- **Arquivo**: `oficina-motos-web/src/app/core/services/ordens.service.spec.ts`
- **Cenários obrigatórios**:
  1. `getAll()` realiza `GET` em `apiPaths.ordens.base` e retorna a lista de ordens de serviço.
  2. `create(dto)` realiza `POST` em `apiPaths.ordens.base` com o payload de nova OS.
  3. `changeStatus(id, status)` realiza `PUT` em `${apiPaths.ordens.base}/${id}` enviando o novo status da OS.

---

## 3. Frontend — Specs de Guards e Interceptors Angular (`US-031`)

### 3.1 `auth.guard.spec.ts` (`T-031.1`)
- **Arquivo**: `oficina-motos-web/src/app/core/auth/auth.guard.spec.ts`
- **Cenários obrigatórios**:
  1. Usuário não autenticado (`isAuthenticated() === false`) é redirecionado para `/login` (`UrlTree`).
  2. Usuário autenticado (`isAuthenticated() === true`) recebe permissão de ativação (`true`).

### 3.2 `auth.interceptor.spec.ts` (`T-031.2`)
- **Arquivo**: `oficina-motos-web/src/app/core/auth/auth.interceptor.spec.ts`
- **Cenários obrigatórios**:
  1. Injeta o header `Authorization: Bearer <token>` quando `authService.getToken()` retorna um token.
  2. Não injeta o header `Authorization` quando `authService.getToken()` retorna `null`.

### 3.3 `error-interceptor.spec.ts` (`T-031.3`)
- **Arquivo**: `oficina-motos-web/src/app/core/interceptors/error-interceptor.spec.ts`
- **Cenários obrigatórios**:
  1. Resposta HTTP `401 Unauthorized` (sem refresh token disponível ou quando a renovação falha) chama `authService.logout()` e exibe toast de sessão expirada.
  2. Respostas HTTP `403` e `500` exibem as mensagens de erro apropriadas no `Toast`.
