# Pesquisa Técnica: Criar Projeto xUnit OficinaMotos.Tests e Testes Unitários de AuthService (US-025 & US-026)

**Branch**: `025-xunit-auth-tests` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/025-xunit-auth-tests/spec.md)

## 1. Estruturação do Projeto `OficinaMotos.Tests` (`US-025`)

- **Decision**:
  - Criar `OficinaMotos.Tests` em `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` (`T-025.1`) e registrá-lo em `oficina-motos-api/OficinaMotos.slnx` (`T-025.2`).
  - Instalar/configurar no `.csproj` (`T-025.3`):
    - `Moq` (`4.20.72`)
    - `FluentAssertions` (`7.0.0` / `6.12.2`)
    - `Microsoft.EntityFrameworkCore.InMemory` (`8.0.22`)
    - `Microsoft.AspNetCore.Mvc.Testing` (`8.0.22`)
    - `xunit` (`2.9.3`), `xunit.runner.visualstudio` (`3.1.4`), `Microsoft.NET.Test.Sdk` (`17.14.1`), `coverlet.collector` (`6.0.4`)
  - Referenciar os 4 projetos da solução: `OficinaMotos.Domain`, `OficinaMotos.Application`, `OficinaMotos.Infrastructure` e `OficinaMotos.API`.
  - Criar `Helpers/TestDbContextFactory.cs` (`T-025.4`) com `Create(string? databaseName = null)` retornando uma instância limpa de `OficinaContext` configurada com `UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())` e `CreateMapper()` para instanciar o `IMapper` com `DomainToDTOMappingProfile`.
- **Rationale**:
  - Segue literalmente as 4 tasks de `US-025` no `governance/backlog.md` e já deixa o projeto pronto também para `US-027` (`ClienteService`), `US-028` (`OrdemServicoService`) e `US-029` (`WebApplicationFactory`).
- **Alternatives considered**:
  - Reaproveitar `tests/OficinaMotos.SoftDelete.Tests` sem criar `src/OficinaMotos.Tests`. Rejeitado porque `T-025.1` e `T-025.2` exigem explicitamente `src/OficinaMotos.Tests` adicionado ao `OficinaMotos.slnx`.

## 2. Alinhamento de `AuthService` para `US-026` (`T-026.1` a `T-026.4`)

- **Análise dos 4 cenários de `US-026`**:
  1. `T-026.1 — Login_ValidCredentials_ReturnsJwtToken`: `AuthService.LoginAsync` com credenciais válidas deve retornar resultado contendo o token JWT assinado (`Token` preenchido e `ExpiresAt` definido), além dos metadados do usuário, roles, permissões e refresh token.
  2. `T-026.2 — Login_InvalidPassword_ReturnsNull`: `AuthService.LoginAsync` com senha incorreta deve incrementar `TentativasLogin`, atualizar o usuário no repositório, gravar `LOGIN_FAIL` no audit log e retornar `null`.
  3. `T-026.3 — Login_BlockedAccount_ThrowsException`: `AuthService.LoginAsync` quando `usuario.EstaBloqueado()` ou `usuario.Status == 3` deve gravar `LOGIN_FAIL` no audit log e lançar `InvalidOperationException` indicando que a conta está bloqueada. No `AuthController.Login`, capturar `InvalidOperationException` e retornar `401 Unauthorized` para preservar o contrato HTTP existente.
  4. `T-026.4 — GenerateToken_ContainsExpectedClaims`: `AuthService.GenerateToken(LoginDataResult userInfo, DateTime? expiration = null)` deve gerar o JWT HMAC-SHA256 com as claims:
     - `JwtRegisteredClaimNames.Sub` (`UserId`)
     - `JwtRegisteredClaimNames.Email` (`Email`)
     - `JwtRegisteredClaimNames.UniqueName` (`Login`)
     - `ClaimTypes.Name` (`Nome`)
     - `JwtRegisteredClaimNames.Jti` (GUID único)
     - `ClaimTypes.Role` (uma claim para cada perfil em `Roles`)
     - `"permissao"` (uma claim para cada permissão em `Permissions`)
- **Decision**:
  - Adicionar `Token` (`string`) e `ExpiresAt` (`DateTime`) em `LoginDataResult`.
  - Adicionar `string GenerateToken(LoginDataResult userInfo, DateTime? expiration = null)` em `IAuthService` e `AuthService` (recebendo `IConfiguration? configuration = null` opcional no construtor de `AuthService` para ler `Jwt:Key` quando disponível ou usar chave padrão de 32+ caracteres em testes unitários isolados).
  - Atualizar `AuthController` para delegar a geração do JWT a `_authService.GenerateToken(...)` e tratar `InvalidOperationException` em `Login` retornando `401 Unauthorized`.
- **Rationale**:
  - Cumpre 100% dos requisitos de `US-026` (`T-026.1`..`T-026.4`), resolve o débito `ADR-003` (`JWT gerado no Controller`) e mantém retrocompatibilidade total nos endpoints `/api/v1/Auth/login` e `/api/v1/Auth/refresh`.
