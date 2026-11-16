# Especificação de Funcionalidade: Criar Projeto xUnit OficinaMotos.Tests e Testes Unitários de AuthService (US-025 & US-026)

**Feature Branch**: `025-xunit-auth-tests`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-025 — Criar projeto xUnit OficinaMotos.Tests | Prioridade: 🟡 Should | Estimativa: S | Sprint: 5 | Tasks: [ ] T-025.1 — dotnet new xunit -n OficinaMotos.Tests em src/, [ ] T-025.2 — Adicionar ao OficinaMotos.slnx, [ ] T-025.3 — Instalar: Moq, FluentAssertions, InMemory, Mvc.Testing, [ ] T-025.4 — Criar Helpers/TestDbContextFactory.cs || US-026 — Testes unitários de AuthService | Prioridade: 🟡 Should | Estimativa: M | Sprint: 5 | Depende: US-025 | Tasks: [ ] T-026.1 — Login_ValidCredentials_ReturnsJwtToken, [ ] T-026.2 — Login_InvalidPassword_ReturnsNull, [ ] T-026.3 — Login_BlockedAccount_ThrowsException, [ ] T-026.4 — GenerateToken_ContainsExpectedClaims"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Estruturar o Projeto de Testes `OficinaMotos.Tests` na Solução .NET (Priority: P1)

Como engenheiro de software ou arquiteto do projeto, quero criar e configurar o projeto oficial de testes `OficinaMotos.Tests` em `oficina-motos-api/src/OficinaMotos.Tests`, registrá-lo na solução `OficinaMotos.slnx`, instalar os pacotes de teste padronizados (`Moq`, `FluentAssertions`, `Microsoft.EntityFrameworkCore.InMemory`, `Microsoft.AspNetCore.Mvc.Testing`) e disponibilizar a fábrica utilitária `Helpers/TestDbContextFactory.cs`, estabelecendo a infraestrutura base do `EPIC-05 — Testes e Qualidade`.

**Why this priority**: A criação do projeto `OficinaMotos.Tests` (`US-025`: `T-025.1` a `T-025.4`) é pré-requisito estrutural e bloqueante para todas as histórias de testes automatizados de backend da Sprint 5 (`US-026`, `US-027`, `US-028` e `US-029`).

**Independent Test**: Executar `dotnet build` e `dotnet test` sobre a solução `OficinaMotos.slnx` a partir de `oficina-motos-api/`, verificando que o projeto `src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` é descoberto, compilado com todas as referências de pacotes e projetos (`Domain`, `Application`, `Infrastructure`, `API`) e que `TestDbContextFactory.Create()` instancia um `OficinaContext` em memória isolado e funcional.

**Acceptance Scenarios**:

1. **Given** a árvore de código em `oficina-motos-api/`, **When** o projeto `OficinaMotos.Tests` é criado em `src/OficinaMotos.Tests` (`T-025.1`) e adicionado a `OficinaMotos.slnx` (`T-025.2`), **Then** `dotnet build OficinaMotos.slnx` compila todos os 5 projetos da solução (`Domain`, `Application`, `Infrastructure`, `API` e `Tests`) sem erros.
2. **Given** o arquivo `src/OficinaMotos.Tests/OficinaMotos.Tests.csproj`, **When** as dependências do projeto são inspecionadas (`T-025.3`), **Then** constam instaladas e funcionais as bibliotecas `xunit`, `Moq`, `FluentAssertions`, `Microsoft.EntityFrameworkCore.InMemory` e `Microsoft.AspNetCore.Mvc.Testing`, além das referências aos projetos da solução.
3. **Given** um teste unitário que precisa de persistência isolada em memória, **When** invoca `TestDbContextFactory.Create()` (`T-025.4`), **Then** recebe uma nova instância de `OficinaContext` configurada com banco `InMemory` exclusivo (nomeado por `Guid` único ou identificador informado) e com o modelo e filtros globais de `BaseEntity` devidamente inicializados.

---

### User Story 2 - Validar Autenticação e Geração de Token JWT no `AuthService` com Testes Unitários (Priority: P1)

Como engenheiro de segurança e desenvolvedor backend, quero dispor de uma suíte completa de testes unitários para `AuthService` (`AuthServiceTests`) cobrindo credenciais válidas com retorno de token JWT (`T-026.1`), senha inválida retornando `null` (`T-026.2`), conta bloqueada lançando exceção (`T-026.3`) e geração de token JWT contendo todas as claims esperadas (`T-026.4`), garantindo que o fluxo crítico de autenticação e RBAC não sofra regressões.

**Why this priority**: Cumpre integralmente a `US-026` (`T-026.1` a `T-026.4`), protegendo o serviço mais sensível de segurança (`AuthService`) com testes rápidos e determinísticos usando `Moq` e `FluentAssertions`.

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~AuthServiceTests"` e verificar a aprovação dos 4 testes canônicos (`Login_ValidCredentials_ReturnsJwtToken`, `Login_InvalidPassword_ReturnsNull`, `Login_BlockedAccount_ThrowsException` e `GenerateToken_ContainsExpectedClaims`).

**Acceptance Scenarios**:

1. **Given** um usuário ativo (`Status = 1`, não bloqueado) com senha hash BCrypt válida, perfis e permissões associadas, **When** `Login_ValidCredentials_ReturnsJwtToken` (`T-026.1`) executa `AuthService.LoginAsync`, **Then** o resultado não é nulo, contém um token JWT válido e não vazio (`Token`), além dos dados do usuário (`UserId`, `Email`, `Login`, `Roles`, `Permissions`, `RefreshToken`), e registra auditoria de sucesso (`LOGIN`).
2. **Given** um usuário ativo existente no repositório, **When** `Login_InvalidPassword_ReturnsNull` (`T-026.2`) executa `AuthService.LoginAsync` informando uma senha incorreta, **Then** o método retorna `null`, incrementa `TentativasLogin` do usuário, persiste a atualização no repositório e registra auditoria de falha (`LOGIN_FAIL`).
3. **Given** um usuário cuja conta está bloqueada (`BloqueadoAte > DateTime.UtcNow` ou `Status == 3`), **When** `Login_BlockedAccount_ThrowsException` (`T-026.3`) executa `AuthService.LoginAsync`, **Then** o método registra a tentativa em `SegAuditLog` (`LOGIN_FAIL`) e lança uma exceção de operação inválida (`InvalidOperationException`) indicando que a conta está bloqueada.
4. **Given** os dados de autenticação de um usuário (`LoginDataResult` com `UserId`, `Email`, `Login`, `Nome`, `Roles` e `Permissions`), **When** `GenerateToken_ContainsExpectedClaims` (`T-026.4`) invoca `AuthService.GenerateToken`, **Then** o token JWT gerado pode ser lido por `JwtSecurityTokenHandler` e contém as claims esperadas (`sub`, `email`, `unique_name`, `ClaimTypes.Name`, `jti`, `ClaimTypes.Role` para cada perfil e `permissao` para cada permissão).

---

### Edge Cases

- **Isolamento entre testes usando `TestDbContextFactory`**: Cada chamada a `TestDbContextFactory.Create()` sem nome explícito deve gerar um `databaseName` único (`Guid.NewGuid().ToString()`) para evitar colisão de dados em execuções paralelas do xUnit.
- **Compatibilidade HTTP no `AuthController` ao tratar conta bloqueada**: Quando `AuthService.LoginAsync` lançar `InvalidOperationException` para conta bloqueada (`T-026.3`), o `AuthController.Login` deve capturar `InvalidOperationException` e continuar retornando `401 Unauthorized` com mensagem amigável, mantendo total compatibilidade com o contrato REST e o frontend.
- **Chave JWT para `GenerateToken` em ambiente de teste**: Quando `IConfiguration["Jwt:Key"]` não for injetada ou estiver ausente em testes unitários isolados de `AuthService`, `GenerateToken` deve utilizar uma chave de teste/fallback segura de pelo menos 32 bytes para permitir a assinatura HMAC-SHA256 sem exigir configuração externa nos testes unitários, enquanto `Program.cs` mantém a validação fail-fast em startup da API (`US-002`).

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE possuir o projeto xUnit `OficinaMotos.Tests` em `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` configurado para `.NET 8.0` (`net8.0`) (`T-025.1`).
- **FR-002**: O arquivo de solução `oficina-motos-api/OficinaMotos.slnx` DEVE incluir a referência ao projeto `src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` (`T-025.2`).
- **FR-003**: O projeto `OficinaMotos.Tests` DEVE declarar os pacotes NuGet `Moq`, `FluentAssertions`, `Microsoft.EntityFrameworkCore.InMemory` e `Microsoft.AspNetCore.Mvc.Testing` (além de `xunit`, `xunit.runner.visualstudio` e `Microsoft.NET.Test.Sdk`) e referenciar os projetos `OficinaMotos.Domain`, `OficinaMotos.Application`, `OficinaMotos.Infrastructure` e `OficinaMotos.API` (`T-025.3`).
- **FR-004**: O projeto `OficinaMotos.Tests` DEVE disponibilizar a classe auxiliar `TestDbContextFactory` em `oficina-motos-api/src/OficinaMotos.Tests/Helpers/TestDbContextFactory.cs` com método estático para criar instâncias de `OficinaContext` baseadas em `UseInMemoryDatabase` com banco isolado por teste (`T-025.4`).
- **FR-005**: O `AuthService` (`OficinaMotos.Application.Services.Seguranca.AuthService`) e a interface `IAuthService` DEVEM:
  - Expor o método `GenerateToken(LoginDataResult userInfo, DateTime? expiration = null)` que gera o JWT assinado contendo as claims `sub`, `email`, `unique_name`, `ClaimTypes.Name`, `jti`, `ClaimTypes.Role` e `permissao` (`T-026.4`);
  - Preencher a propriedade `Token` em `LoginDataResult` durante `LoginAsync` quando as credenciais forem válidas (`T-026.1`);
  - Retornar `null` quando a senha for inválida (`T-026.2`);
  - Lançar `InvalidOperationException` quando a conta do usuário estiver bloqueada (`usuario.EstaBloqueado()` ou `usuario.Status == 3`), após registrar o evento `LOGIN_FAIL` no audit log (`T-026.3`).
- **FR-006**: O projeto `OficinaMotos.Tests` DEVE conter a classe de testes `AuthServiceTests` implementando os 4 métodos de teste especificados na `US-026`:
  - `Login_ValidCredentials_ReturnsJwtToken` (`T-026.1`);
  - `Login_InvalidPassword_ReturnsNull` (`T-026.2`);
  - `Login_BlockedAccount_ThrowsException` (`T-026.3`);
  - `GenerateToken_ContainsExpectedClaims` (`T-026.4`).

### Key Entities *(include if feature involves data)*

- **`TestDbContextFactory`** (`src/OficinaMotos.Tests/Helpers/TestDbContextFactory.cs`): Fábrica utilitária para criação de `OficinaContext` em memória (`EF Core InMemory`) e suporte aos testes de repositório/serviço.
- **`AuthService` / `LoginDataResult`** (`OficinaMotos.Application`): Serviço de autenticação e resultado de login contendo `Token`, `UserId`, `Nome`, `Email`, `Login`, `Roles`, `Permissions`, `RefreshToken` e `RefreshTokenExpiresAt`.
- **`SegUsuario`** (`OficinaMotos.Domain.Entities`): Entidade de usuário contendo credenciais (`Email`, `Login`, `Senha` hash BCrypt), controle de bloqueio (`TentativasLogin`, `BloqueadoAte`, `Status`) e relacionamentos com perfis e permissões.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `dotnet build` em `oficina-motos-api/OficinaMotos.slnx` compila 100% dos projetos da solução, incluindo `src/OficinaMotos.Tests/OficinaMotos.Tests.csproj`, com zero erros (`T-025.1`, `T-025.2`, `T-025.3`).
- **SC-002**: `TestDbContextFactory` (`src/OficinaMotos.Tests/Helpers/TestDbContextFactory.cs`) cria instâncias funcionais e isoladas de `OficinaContext` em memória (`T-025.4`).
- **SC-003**: `dotnet test` em `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj` executa e aprova 100% dos testes unitários de `AuthService` (`Login_ValidCredentials_ReturnsJwtToken`, `Login_InvalidPassword_ReturnsNull`, `Login_BlockedAccount_ThrowsException` e `GenerateToken_ContainsExpectedClaims`) (`T-026.1` a `T-026.4`).

## Assumptions

- Nenhuma alteração no banco de dados MySQL ou criação de migration é necessária para `US-025` e `US-026`.
- A extração/disponibilização de `GenerateToken` em `AuthService` também resolve na prática o débito técnico documentado em `ADR-003` (`governance/decisoes.md`), centralizando a geração de claims JWT na camada de aplicação e permitindo seu teste unitário direto sem depender de `HttpContext` do controller.
