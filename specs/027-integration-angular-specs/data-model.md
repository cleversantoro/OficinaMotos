# Data Model & Contracts: Testes de Integração e Specs Angular (US-029, US-030 & US-031)

**Feature**: `027-integration-angular-specs`  
**Date**: 2026-11-16

---

## 1. Infraestrutura de Testes de Integração Backend (`US-029`)

### `OficinaMotosWebApplicationFactory` (`OficinaMotos.Tests.Integration`)

| Membro / Configuração | Tipo | Descrição |
| :--- | :--- | :--- |
| `AdminEmail` | `const string` | `"admin@oficinamotos.com"` (usuário semeado para testes autenticados) |
| `AdminLogin` | `const string` | `"admin"` |
| `AdminPassword` | `const string` | `"Admin@123456"` (hasheada com BCrypt no seed do `OficinaContext`) |
| `TestJwtKey` | `const string` | `"ChaveSecretaIntegracaoOficinaMotos2026_Min32Bytes!"` |
| `ConfigureWebHost(IWebHostBuilder)` | `override void` | Define `UseEnvironment("Testing")`, injeta configurações em memória (`Jwt:Key`, `UseInMemoryDatabase = true`), substitui `DbContextOptions<OficinaContext>` por `UseInMemoryDatabase` isolado e executa `SeedTestData(OficinaContext)` |
| `CreateAuthenticatedClientAsync()` | `Task<HttpClient>` | Realiza `POST /api/v1/Auth/login` com `AdminEmail`/`AdminPassword` e retorna um `HttpClient` com `DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token)` |

### Entidades Semeadas no `OficinaContext` (`InMemory`)

- **`SegModulo`**: `Id = 1`, `Nome = "Clientes"`
- **`SegPermissao`**: `Id = 1`, `ModuloId = 1`, `Acao = "visualizar"`
- **`SegPerfil`**: `Id = 1`, `Nome = "Administrador"`
- **`SegPerfilPermissao`**: vínculo `PerfilId = 1`, `PermissaoId = 1`
- **`SegUsuario`**: `Id = 1`, `Nome = "Administrador Teste"`, `Email = "admin@oficinamotos.com"`, `Login = "admin"`, `Senha = BCrypt.HashPassword("Admin@123456")`, `Status = 1`
- **`SegUsuarioPerfil`**: vínculo `UsuarioId = 1`, `PerfilId = 1`, `Ativo = true`

---

## 2. Contratos de Serviços, Guards e Interceptors Angular (`US-030` & `US-031`)

### `OrdensService` (`oficina-motos-web/src/app/core/services/ordens.service.ts`)

| Método | Assinatura | Comportamento HTTP |
| :--- | :--- | :--- |
| `getAll<T = OrdemServico>` | `getAll<T = OrdemServico>(params?: Record<string, any>): Observable<T[]>` | Delega para `this.list<T>(params)` → `GET /api/v1/OrdemServicos` (`T-030.3`) |
| `create<T, B>` | `create<T = OrdemServico, B = CreateOrdemServicoRequest>(body: B): Observable<T>` | `POST /api/v1/OrdemServicos` com `body` (`T-030.3`) |
| `changeStatus<T>` | `changeStatus<T = OrdemServico>(id: string \| number, status: string, body?: Partial<UpdateOrdemServicoRequest>): Observable<T>` | `PUT /api/v1/OrdemServicos/{id}` enviando `{ ...body, status }` via `ApiClientService.update` (`T-030.3`) |

### `AuthService` (`oficina-motos-web/src/app/core/auth/auth.service.ts`)

| Membro | Tipo | Contrato Testado em `T-030.1` |
| :--- | :--- | :--- |
| `currentUser` | `Signal<CurrentUser \| null>` | Signal somente leitura atualizado em `login()`, `refreshToken()` e `logout()` |
| `isAuthenticated` | `Signal<boolean>` | Computed signal que retorna `true` sse `currentUser()` não for nulo e `expiresAt > new Date()` |
| `login(credentials)` | `Observable<LoginResponse>` | `POST /api/v1/Auth/login`, persiste `oficina_token`, `oficina_refresh_token` e `oficina_user`, e atualiza `currentUser` |
| `logout()` | `void` | Remove chaves do `localStorage`, define `currentUser` como `null` e chama `router.navigate(['/login'])` |

### `authGuard`, `authInterceptor` e `errorInterceptor` (`US-031`)

| Artefato | Arquivo | Contrato Testado |
| :--- | :--- | :--- |
| `authGuard` | `src/app/core/auth/auth.guard.ts` | Retorna `true` quando `authService.isAuthenticated()` é `true`; retorna `UrlTree('/login')` quando `false` (`T-031.1`) |
| `authInterceptor` | `src/app/core/auth/auth.interceptor.ts` | Clona a requisição adicionando `Authorization: Bearer <token>` quando `authService.getToken()` retorna valor; não adiciona header quando `null`; aciona e finaliza `Loading.show()` (`T-031.2`) |
| `errorInterceptor` | `src/app/core/interceptors/error-interceptor.ts` | Em erro HTTP `401` sem refresh token (ou se o refresh falhar), invoca `authService.logout()` e exibe `toast.error` (`T-031.3`) |
