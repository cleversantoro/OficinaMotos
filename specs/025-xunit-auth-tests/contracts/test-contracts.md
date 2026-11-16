# Contratos de Teste: OficinaMotos.Tests e AuthServiceTests (US-025 & US-026)

**Branch**: `025-xunit-auth-tests` | **Data**: 2026-11-16

## 1. Contrato do Projeto `OficinaMotos.Tests` (`US-025`)
- **Caminho do Projeto**: `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj`
- **Registro na Solução**: `oficina-motos-api/OficinaMotos.slnx` → `<Project Path="src/OficinaMotos.Tests/OficinaMotos.Tests.csproj" />`
- **Pacotes Obrigatórios (`T-025.3`)**:
  - `xunit`
  - `xunit.runner.visualstudio`
  - `Microsoft.NET.Test.Sdk`
  - `Moq`
  - `FluentAssertions`
  - `Microsoft.EntityFrameworkCore.InMemory`
  - `Microsoft.AspNetCore.Mvc.Testing`
- **Helper de Banco em Memória (`T-025.4`)**:
  - `oficina-motos-api/src/OficinaMotos.Tests/Helpers/TestDbContextFactory.cs`

## 2. Contrato da Suíte `AuthServiceTests` (`US-026`)
- **Caminho**: `oficina-motos-api/src/OficinaMotos.Tests/Services/AuthServiceTests.cs`
- **Métodos de Teste Obrigatórios**:
  1. `[Fact] public async Task Login_ValidCredentials_ReturnsJwtToken()` (`T-026.1`)
  2. `[Fact] public async Task Login_InvalidPassword_ReturnsNull()` (`T-026.2`)
  3. `[Fact] public async Task Login_BlockedAccount_ThrowsException()` (`T-026.3`)
  4. `[Fact] public void GenerateToken_ContainsExpectedClaims()` (`T-026.4`)
