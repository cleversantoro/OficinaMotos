# Plano de Implementação: Testes de Integração de Controllers Críticos e Specs de Services, Guards e Interceptors Angular (US-029, US-030 & US-031)

**Branch**: `027-integration-angular-specs` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/027-integration-angular-specs/spec.md)  
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

**Entrada**:
- **US-029 — Testes de integração para controllers críticos** (`Prioridade: 🟢 Could | Estimativa: L | Sprint: 5 | Depende: US-025, US-001`)
  - [ ] T-029.1 — Criar `OficinaMotosWebApplicationFactory`
  - [ ] T-029.2 — `AuthControllerIntegrationTests`: login válido, login inválido
  - [ ] T-029.3 — `ClienteControllerIntegrationTests`: 401 sem token, CRUD autenticado
- **US-030 — Specs para services Angular críticos** (`Prioridade: 🟡 Should | Estimativa: M | Sprint: 5`)
  - [ ] T-030.1 — `auth.service.spec.ts`: `login()`, `logout()`, `isAuthenticated()`, signal
  - [ ] T-030.2 — `clientes.service.spec.ts`: CRUD completo
  - [ ] T-030.3 — `ordens.service.spec.ts`: `getAll()`, `create()`, `changeStatus()`
- **US-031 — Specs para guards e interceptors Angular** (`Prioridade: 🟡 Should | Estimativa: S | Sprint: 5 | Depende: US-030`)
  - [ ] T-031.1 — `auth.guard.spec.ts`: usuário não autenticado é redirecionado
  - [ ] T-031.2 — `auth.interceptor.spec.ts`: token injetado no header correto
  - [ ] T-031.3 — `error.interceptor.spec.ts` (`error-interceptor.spec.ts`): 401 chama `logout()`

---

## Summary

Este plano estrutura a entrega final do **EPIC-05 — Testes e Qualidade** (Sprint 5), abrangendo:
1. **Backend (`US-029`)**: Exposição de `public partial class Program` na API, suporte a banco `EF Core InMemory` no ambiente `"Testing"` sem acionar `ServerVersion.AutoDetect` do MySQL, criação da `OficinaMotosWebApplicationFactory` (`T-029.1`) e implementação dos testes de integração HTTP `AuthControllerIntegrationTests` (`T-029.2`) e `ClienteControllerIntegrationTests` (`T-029.3`).
2. **Frontend Services (`US-030`)**: Ampliação das suítes `auth.service.spec.ts` (`T-030.1`), `clientes.service.spec.ts` (`T-030.2`) e `ordens.service.spec.ts` (`T-030.3`, incluindo os métodos `getAll()` e `changeStatus()` em `OrdensService`).
3. **Frontend Guards & Interceptors (`US-031`)**: Criação de `auth.guard.spec.ts` (`T-031.1`), `auth.interceptor.spec.ts` (`T-031.2`) e ampliação de `error-interceptor.spec.ts` (`T-031.3`).

---

## Technical Context

**Language/Version**: C# 12 / .NET 8.0 (`net8.0`) no backend; TypeScript 5.9 / Angular 21.0 no frontend  
**Primary Dependencies**:
- Backend: `Microsoft.AspNetCore.Mvc.Testing` 8.0.22, `Microsoft.EntityFrameworkCore.InMemory` 8.0.22, `xunit` 2.9.3, `FluentAssertions` 6.12.2
- Frontend: `@angular/core/testing` 21.0, `@angular/common/http/testing` 21.0, `@angular/router/testing` 21.0, `vitest` 4.0.8, `jsdom` 27.1.0  
**Storage**: `EF Core InMemory` isolado por instância de `OficinaMotosWebApplicationFactory` nos testes de integração; `localStorage` em memória (`jsdom`) nos testes Angular  
**Testing**: `dotnet test` (backend) e `npm test -- --watch=false` / `ng test` (frontend)  
**Target Platform**: .NET 8 TestServer (`Microsoft.AspNetCore.TestHost`) e Angular TestBed (Vitest + JSDOM)  
**Project Type**: Full-stack Web Application (`oficina-motos-api` + `oficina-motos-web`)  
**Performance Goals**: Execução dos testes de integração em < 5s e das specs Angular em < 10s  
**Constraints**: Zero dependência de servidor MySQL em execução para rodar `dotnet test`; manutenção de `strict: true` no TypeScript e uso centralizado de `apiPaths`  
**Scale/Scope**: 3 novos arquivos em `OficinaMotos.Tests/Integration/`, ajustes de inicialização em `Program.cs` e `DependencyInjection.cs`, evolução de `ordens.service.ts` e 6 arquivos `.spec.ts` no frontend Angular

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Nenhuma lógica de negócio é movida para Controllers ou UI; os testes validam o isolamento e os contratos de cada contexto. |
| **II. API RESTful Versionada** | ✅ Pass | Os testes de integração e as specs Angular utilizam estritamente `/api/v1/Auth`, `/api/v1/Clientes` e `/api/v1/OrdemServicos` via `apiPaths`. |
| **III. Segurança por Design (RBAC + LGPD)** | ✅ Pass | Valida diretamente a exigência de JWT (`401 Unauthorized` sem token), autenticação via BCrypt/JWT, `authGuard`, `authInterceptor` e `errorInterceptor`. |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `auth.service.spec.ts` valida diretamente os Signals reativos (`currentUser`, `isAuthenticated`, `currentRole`, `permissions`). |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | `ClienteControllerIntegrationTests` valida o ciclo completo de CRUD com exclusão lógica (`IsDeleted`) via pipeline HTTP. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Implementa integralmente os testes de integração e as specs de services, guards e interceptors exigidos pelo Princípio VI da Constituição. |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Atende integralmente às tasks `T-029.1`..`T-029.3`, `T-030.1`..`T-030.3` e `T-031.1`..`T-031.3` de `governance/backlog.md`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/027-integration-angular-specs/
├── spec.md              # Especificação de requisitos (US-029, US-030 e US-031)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Decisões técnicas de WebApplicationFactory e Angular TestBed
├── data-model.md        # Fase 1: Modelo de dados de seed e contratos de serviços/interceptors
├── quickstart.md        # Fase 1: Guia rápido de execução de dotnet test e npm test
├── checklists/
│   └── requirements.md  # Checklist de validação da especificação
└── contracts/
    └── test-contracts.md # Fase 1: Contratos detalhados de cada suíte de teste
```

### Source Code (repository root)

```text
oficina-motos-api/
└── src/
    ├── OficinaMotos.API/
    │   └── Program.cs                                                     # [MODIFICAR] Expor public partial class Program e pular UseMySql quando em ambiente Testing (T-029.1)
    ├── OficinaMotos.Infrastructure/
    │   └── IoC/
    │       └── DependencyInjection.cs                                     # [MODIFICAR] Registrar UseMySql apenas quando não houver DbContextOptions<OficinaContext> pré-configurado / fora do ambiente Testing (T-029.1)
    └── OficinaMotos.Tests/
        └── Integration/
            ├── OficinaMotosWebApplicationFactory.cs                       # [CRIAR] WebApplicationFactory<Program> com EF Core InMemory, JWT de teste e seed de usuário Admin (T-029.1)
            ├── AuthControllerIntegrationTests.cs                          # [CRIAR] Testes de integração de login válido e login inválido (T-029.2)
            └── ClienteControllerIntegrationTests.cs                       # [CRIAR] Testes de integração de 401 sem token e CRUD autenticado (T-029.3)

oficina-motos-web/
└── src/
    └── app/
        └── core/
            ├── auth/
            │   ├── auth.service.spec.ts                                   # [MODIFICAR] Testar login(), logout(), isAuthenticated() e signal currentUser() (T-030.1)
            │   ├── auth.guard.spec.ts                                     # [CRIAR] Testar redirecionamento de usuário não autenticado e acesso autenticado (T-031.1)
            │   └── auth.interceptor.spec.ts                               # [CRIAR] Testar injeção do header Authorization: Bearer <token> (T-031.2)
            ├── interceptors/
            │   └── error-interceptor.spec.ts                              # [MODIFICAR] Testar que erro 401 chama logout() (T-031.3)
            └── services/
                ├── clientes.service.spec.ts                               # [MODIFICAR] Testar CRUD completo de ClientesService (T-030.2)
                ├── ordens.service.ts                                      # [MODIFICAR] Expor getAll() e changeStatus() (T-030.3)
                └── ordens.service.spec.ts                                 # [MODIFICAR] Testar getAll(), create() e changeStatus() (T-030.3)
```

**Structure Decision**: Preserva a estrutura Clean Architecture em `oficina-motos-api/src/` (adicionando a pasta `Integration/` em `OficinaMotos.Tests`) e mantém os arquivos `.spec.ts` colocalizados com seus respectivos services, guards e interceptors em `oficina-motos-web/src/app/core/`.

---

## Fases de Execução

### Fase 1: Infraestrutura e Testes de Integração Backend (`US-029`)
1. Atualizar `oficina-motos-api/src/OficinaMotos.API/Program.cs` e `oficina-motos-api/src/OficinaMotos.Infrastructure/IoC/DependencyInjection.cs` para suportar substituição limpa do `OficinaContext` em ambiente `"Testing"` e expor `public partial class Program`.
2. Criar `oficina-motos-api/src/OficinaMotos.Tests/Integration/OficinaMotosWebApplicationFactory.cs` (`T-029.1`) configurando `EF Core InMemory`, chave JWT de teste e seed de usuário `Administrador`.
3. Criar `oficina-motos-api/src/OficinaMotos.Tests/Integration/AuthControllerIntegrationTests.cs` (`T-029.2`) cobrindo login válido (`200 OK` + JWT) e login inválido (`401 Unauthorized`).
4. Criar `oficina-motos-api/src/OficinaMotos.Tests/Integration/ClienteControllerIntegrationTests.cs` (`T-029.3`) cobrindo `401 Unauthorized` sem token e CRUD completo autenticado (`POST`, `GET`, `PUT`, `DELETE`).

### Fase 2: Specs de Services Angular Críticos (`US-030`)
1. Atualizar `oficina-motos-web/src/app/core/auth/auth.service.spec.ts` (`T-030.1`) cobrindo `login()`, `logout()`, `isAuthenticated()` e reatividade do signal `currentUser()`.
2. Atualizar `oficina-motos-web/src/app/core/services/clientes.service.spec.ts` (`T-030.2`) cobrindo `list()`, `get()`, `create()`, `update()`, `delete()` e `search()`.
3. Adicionar `getAll()` e `changeStatus()` em `oficina-motos-web/src/app/core/services/ordens.service.ts` e atualizar `oficina-motos-web/src/app/core/services/ordens.service.spec.ts` (`T-030.3`) cobrindo `getAll()`, `create()` e `changeStatus()`.

### Fase 3: Specs de Guards e Interceptors Angular (`US-031`)
1. Criar `oficina-motos-web/src/app/core/auth/auth.guard.spec.ts` (`T-031.1`) cobrindo redirecionamento para `/login` quando não autenticado e liberação (`true`) quando autenticado.
2. Criar `oficina-motos-web/src/app/core/auth/auth.interceptor.spec.ts` (`T-031.2`) cobrindo injeção do header `Authorization: Bearer <token>` e omissão quando não há token.
3. Atualizar `oficina-motos-web/src/app/core/interceptors/error-interceptor.spec.ts` (`T-031.3`) cobrindo que resposta `401 Unauthorized` chama `authService.logout()`.

### Fase 4: Validação End-to-End e Governança
1. Executar `dotnet test OficinaMotos.slnx` em `oficina-motos-api/` e `npm test -- --watch=false` em `oficina-motos-web/`.
2. Atualizar `governance/backlog.md` marcando `US-029`, `US-030` e `US-031` como concluídas na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
