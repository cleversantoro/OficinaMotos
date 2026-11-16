# Plano de Implementação: Criar Projeto xUnit OficinaMotos.Tests e Testes Unitários de AuthService (US-025 & US-026)

**Branch**: `025-xunit-auth-tests` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/025-xunit-auth-tests/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

**Entrada**:
- **US-025 — Criar projeto xUnit OficinaMotos.Tests** (`Prioridade: 🟡 Should | Estimativa: S | Sprint: 5`)
  - [ ] T-025.1 — `dotnet new xunit -n OficinaMotos.Tests` em `src/`
  - [ ] T-025.2 — Adicionar ao `OficinaMotos.slnx`
  - [ ] T-025.3 — Instalar: `Moq`, `FluentAssertions`, `InMemory`, `Mvc.Testing`
  - [ ] T-025.4 — Criar `Helpers/TestDbContextFactory.cs`
- **US-026 — Testes unitários de AuthService** (`Prioridade: 🟡 Should | Estimativa: M | Sprint: 5 | Depende: US-025`)
  - [ ] T-026.1 — `Login_ValidCredentials_ReturnsJwtToken`
  - [ ] T-026.2 — `Login_InvalidPassword_ReturnsNull`
  - [ ] T-026.3 — `Login_BlockedAccount_ThrowsException`
  - [ ] T-026.4 — `GenerateToken_ContainsExpectedClaims`

---

## Resumo

Este plano implementa a fundação da suíte oficial de testes automatizados de backend (`EPIC-05 — Testes e Qualidade`) criando o projeto xUnit `OficinaMotos.Tests` em `oficina-motos-api/src/OficinaMotos.Tests`, integrando-o ao `OficinaMotos.slnx`, configurando `Moq`, `FluentAssertions`, `EF Core InMemory` e `Mvc.Testing`, criando o helper `TestDbContextFactory` (`US-025`) e implementando a bateria de testes unitários de `AuthService` (`US-026`).

---

## Contexto Técnico

**Language/Version**: C# 12 / .NET 8.0 (`net8.0`)  
**Primary Dependencies**: `xunit` 2.9.3, `Moq` 4.20.72, `FluentAssertions` 6.12.2, `Microsoft.EntityFrameworkCore.InMemory` 8.0.22, `Microsoft.AspNetCore.Mvc.Testing` 8.0.22, `System.IdentityModel.Tokens.Jwt` 8.0.1, `BCrypt.Net-Next` 4.0.3  
**Storage**: `EF Core InMemory` (`TestDbContextFactory`) + Repositórios mockados com `Moq`  
**Testing**: `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj`  
**Target Platform**: .NET 8 SDK / Test Host  
**Project Type**: Backend Web API + xUnit Test Project (`oficina-motos-api`)  
**Performance Goals**: Execução completa da suíte unitária em < 3 segundos sem dependência de banco MySQL externo  
**Constraints**: Nenhuma alteração de schema MySQL; manter compatibilidade integral do `AuthController` (`401 Unauthorized` para credenciais inválidas ou conta bloqueada)  
**Scale/Scope**: 1 novo projeto xUnit em `src/OficinaMotos.Tests`, atualização de `OficinaMotos.slnx`, `Helpers/TestDbContextFactory.cs`, método `GenerateToken` e validação de bloqueio em `AuthService`, e suíte `AuthServiceTests`

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Centraliza a geração do token JWT e a regra de bloqueio de conta na camada `Application` (`AuthService`), resolvendo o débito `ADR-003`. |
| **II. API RESTful Versionada** | ✅ Pass | Mantém inalterados os contratos e status HTTP de `/api/v1/Auth/login`, `/api/v1/Auth/refresh` e `/api/v1/Auth/logout`. |
| **III. Segurança por Design (RBAC + LGPD)** | ✅ Pass | Valida diretamente hash BCrypt, bloqueio de conta (`BloqueadoAte` / `Status == 3`), registro de auditoria (`LOGIN` / `LOGIN_FAIL`) e claims de RBAC (`Role` e `permissao`). |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Sem alterações no schema MySQL; `TestDbContextFactory` preserva todos os filtros globais (`IsDeleted`) do `OficinaContext`. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Cumpre diretamente o Princípio VI ao estabelecer o projeto `OficinaMotos.Tests` e cobrir o `AuthService` com testes unitários determinísticos. |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Segue todas as tasks de `US-025` e `US-026` definidas em `governance/backlog.md`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/025-xunit-auth-tests/
├── spec.md              # Especificação de requisitos (US-025 e US-026)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Pesquisa técnica de pacotes, TestDbContextFactory e AuthService
├── data-model.md        # Fase 1: Contratos de TestDbContextFactory, LoginDataResult e AuthServiceTests
├── quickstart.md        # Fase 1: Guia rápido de execução de dotnet build e dotnet test
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    └── test-contracts.md # Fase 1: Contratos do projeto de teste e métodos de AuthServiceTests
```

### Source Code (repository root)

```text
oficina-motos-api/
├── OficinaMotos.slnx                                                                  # [MODIFICAR] Adicionar src/OficinaMotos.Tests/OficinaMotos.Tests.csproj (T-025.2)
└── src/
    ├── OficinaMotos.Application/
    │   ├── OficinaMotos.Application.csproj                                            # [MODIFICAR] Adicionar System.IdentityModel.Tokens.Jwt e Microsoft.Extensions.Configuration.Abstractions
    │   ├── DTOs/Responses/Auth/
    │   │   └── LoginDataResult.cs                                                     # [MODIFICAR] Incluir Token e ExpiresAt para retorno direto em LoginAsync (T-026.1)
    │   ├── Interfaces/Seguranca/
    │   │   └── IAuthService.cs                                                        # [MODIFICAR] Expor GenerateToken(LoginDataResult userInfo, DateTime? expiration = null) (T-026.4)
    │   └── Services/Seguranca/
    │       └── AuthService.cs                                                         # [MODIFICAR] Implementar GenerateToken, preencher Token no LoginAsync e lançar exceção para conta bloqueada (T-026.1..T-026.4)
    ├── OficinaMotos.API/
    │   └── Controllers/Auth/
    │       └── AuthController.cs                                                      # [MODIFICAR] Delegar geração de token ao AuthService e tratar InvalidOperationException retornando 401
    └── OficinaMotos.Tests/
        ├── OficinaMotos.Tests.csproj                                                  # [CRIAR] Projeto xUnit com Moq, FluentAssertions, InMemory e Mvc.Testing (T-025.1, T-025.3)
        ├── Helpers/
        │   └── TestDbContextFactory.cs                                                # [CRIAR] Fábrica de OficinaContext InMemory e IMapper para testes (T-025.4)
        └── Services/
            └── AuthServiceTests.cs                                                    # [CRIAR] Testes unitários de AuthService (T-026.1, T-026.2, T-026.3, T-026.4)
```

**Structure Decision**: Segue exatamente a estrutura solicitada no backlog (`src/OficinaMotos.Tests` registrado em `OficinaMotos.slnx`, com `Helpers/TestDbContextFactory.cs` e `Services/AuthServiceTests.cs`).

---

## Fases de Execução

### Fase 1: Criação e Configuração do Projeto `OficinaMotos.Tests` (`US-025`)
1. Criar `src/OficinaMotos.Tests` (`T-025.1`) e adicioná-lo ao `OficinaMotos.slnx` (`T-025.2`).
2. Configurar as referências aos projetos (`Domain`, `Application`, `Infrastructure`, `API`) e instalar os pacotes `Moq`, `FluentAssertions`, `Microsoft.EntityFrameworkCore.InMemory` e `Microsoft.AspNetCore.Mvc.Testing` (`T-025.3`).
3. Criar `Helpers/TestDbContextFactory.cs` (`T-025.4`).

### Fase 2: Adequação de `AuthService` e Implementação de `AuthServiceTests` (`US-026`)
1. Atualizar `LoginDataResult`, `IAuthService`, `AuthService` e `AuthController` para suportar `GenerateToken`, retorno de `Token` em `LoginAsync` e lançamento de exceção para conta bloqueada (`T-026.1`..`T-026.4`).
2. Criar `Services/AuthServiceTests.cs` implementando:
   - `Login_ValidCredentials_ReturnsJwtToken` (`T-026.1`)
   - `Login_InvalidPassword_ReturnsNull` (`T-026.2`)
   - `Login_BlockedAccount_ThrowsException` (`T-026.3`)
   - `GenerateToken_ContainsExpectedClaims` (`T-026.4`)

### Fase 3: Validação e Governança
1. Executar `dotnet build OficinaMotos.slnx` e `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj`.
2. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
