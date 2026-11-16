# Modelo de Dados e Contratos de Teste: OficinaMotos.Tests e AuthService (US-025 & US-026)

**Branch**: `025-xunit-auth-tests` | **Data**: 2026-11-16

## 1. Utilitário `TestDbContextFactory` (`T-025.4`)

**Caminho**: `oficina-motos-api/src/OficinaMotos.Tests/Helpers/TestDbContextFactory.cs`

```csharp
namespace OficinaMotos.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static OficinaContext Create(string? databaseName = null);
        public static IMapper CreateMapper();
    }
}
```

- `Create(string? databaseName = null)`: Cria `DbContextOptionsBuilder<OficinaContext>().UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())`, instancia `OficinaContext` e garante inicialização limpa para cada teste.
- `CreateMapper()`: Configura `DomainToDTOMappingProfile` via `ServiceCollection` e retorna `IMapper` pronto para uso nos testes de serviços.

## 2. Contrato de `IAuthService` e `LoginDataResult` (`US-026`)

### 2.1 `LoginDataResult` (`OficinaMotos.Application.DTOs.Responses.Auth`)
| Propriedade | Tipo | Descrição |
| :--- | :--- | :--- |
| `Token` | `string` | Token JWT assinado gerado no login (`T-026.1`, `T-026.4`) |
| `ExpiresAt` | `DateTime` | Data/hora UTC de expiração do token JWT |
| `UserId` | `long` | Identificador do usuário (`SegUsuario.Id`) |
| `Nome` | `string` | Nome completo do usuário |
| `Email` | `string` | E-mail do usuário |
| `Login` | `string` | Login único do usuário |
| `Roles` | `List<string>` | Perfis ativos do usuário (`SegPerfil.Nome`) |
| `Permissions` | `List<string>` | Permissões no formato `{Modulo}:{Acao}` |
| `RefreshToken` | `string?` | Refresh token gerado na sessão |
| `RefreshTokenExpiresAt` | `DateTime?` | Data/hora UTC de expiração do refresh token |

### 2.2 Métodos de `AuthService` Testados em `AuthServiceTests` (`US-026`)
| Teste (`AuthServiceTests`) | Entrada / Estado Mockado | Comportamento Esperado |
| :--- | :--- | :--- |
| `Login_ValidCredentials_ReturnsJwtToken` (`T-026.1`) | `SegUsuario` ativo (`Status = 1`, `BloqueadoAte = null`), senha hash BCrypt correspondente, perfil `"Administrador"` e permissão `"Financeiro:visualizar"` | Retorna `LoginDataResult` não nulo com `Token` JWT válido (3 segmentos separados por `.`), `UserId`, `Email`, `Roles` e `Permissions` preenchidos |
| `Login_InvalidPassword_ReturnsNull` (`T-026.2`) | `SegUsuario` ativo com senha BCrypt diferente da senha informada no `LoginRequestDTO` | Retorna `null`, incrementa `usuario.TentativasLogin` para `1`, chama `_usuarioRepository.UpdateAsync` e registra `LOGIN_FAIL` |
| `Login_BlockedAccount_ThrowsException` (`T-026.3`) | `SegUsuario` com `BloqueadoAte = DateTime.UtcNow.AddMinutes(25)` (ou `Status = 3`) | Registra `LOGIN_FAIL` no audit log e lança `InvalidOperationException` |
| `GenerateToken_ContainsExpectedClaims` (`T-026.4`) | `LoginDataResult` com `UserId = 42`, `Email = "admin@oficina.com"`, `Login = "admin"`, `Nome = "Admin MotoPro"`, `Roles = ["Administrador", "Financeiro"]`, `Permissions = ["Financeiro:visualizar", "Financeiro:editar"]` | `GenerateToken` retorna JWT decodificável contendo `sub = "42"`, `email = "admin@oficina.com"`, `unique_name = "admin"`, `ClaimTypes.Name = "Admin MotoPro"`, `ClaimTypes.Role` (`Administrador`, `Financeiro`) e `permissao` (`Financeiro:visualizar`, `Financeiro:editar`) |
