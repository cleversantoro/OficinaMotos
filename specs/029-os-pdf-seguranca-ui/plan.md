# Plano de Implementação: Relatório PDF de Ordem de Serviço e Interface de Gerenciamento de Usuários e Perfis (US-035 & US-036)

**Branch**: `029-os-pdf-seguranca-ui` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/029-os-pdf-seguranca-ui/spec.md)  
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

**Entrada**:
- **US-035 — Gerar relatório PDF de Ordem de Serviço** (`Prioridade: 🟢 Could | Estimativa: L | Sprint: 6 | Depende: US-010`)
  - [ ] T-035.1 — Instalar `QuestPDF` no projeto API
  - [ ] T-035.2 — Criar `PdfService.GerarOrdemServicoPdf(Guid id)`
  - [ ] T-035.3 — Endpoint `GET /api/v1/OrdemServicos/{id}/pdf`
  - [ ] T-035.4 — Botão `"Imprimir OS"` no `OsDetalheComponent`
- **US-036 — Interface de gerenciamento de usuários e perfis** (`Prioridade: 🟡 Should | Estimativa: M | Sprint: 6 | Depende: US-001, US-004`)
  - [ ] T-036.1 — Criar `UsuarioListaComponent` e `UsuarioFormComponent`
  - [ ] T-036.2 — Criar `PerfilListaComponent`
  - [ ] T-036.3 — Registrar rotas `/seguranca/usuarios` e `/seguranca/perfis`
  - [ ] T-036.4 — `RoleGuard` restringindo acesso a role `Admin`

---

## Summary

Este plano estrutura a implementação das histórias **US-035** e **US-036** do **EPIC-06 — Dashboard Real e Funcionalidades Avançadas** (Sprint 6):
1. **Relatório PDF de Ordem de Serviço (`US-035`)**:
   - Backend: Instalação do pacote NuGet `QuestPDF` (`T-035.1`), criação de `IPdfService` e `PdfService` com `GerarOrdemServicoPdfAsync(long id)`, `GerarOrdemServicoPdf(long id)` e sobrecarga `GerarOrdemServicoPdf(Guid id)` (`T-035.2`), e exposição do endpoint autenticado `GET /api/v1/OrdemServicos/{id}/pdf` em `OrdemServicosController` (`T-035.3`).
   - Frontend: Adição de `downloadPdf(id)` em `OrdensService` (via `apiPaths.ordens`) e inclusão do botão `"Imprimir OS"` no cabeçalho de `OsDetalheComponent` (`T-035.4`).
2. **Interface de Gerenciamento de Usuários e Perfis (`US-036`)**:
   - Criação de `roleGuard` (`RoleGuard`) em `core/auth/role.guard.ts` restringindo o acesso a usuários com role `Admin` / `Administrador` (`T-036.4`).
   - Registro de `apiPaths.seguranca`, criação dos models de Segurança (`core/models/seguranca.ts`) e de `SegurancaService` (`core/services/seguranca.service.ts`).
   - Criação de `UsuarioListaComponent` e `UsuarioFormComponent` (`T-036.1`) e `PerfilListaComponent` (`T-036.2`) como Standalone Components com Signals.
   - Registro das rotas `/seguranca/usuarios` (incluindo `/novo` e `/:id/editar`) e `/seguranca/perfis` em `app.routes.ts` protegidas por `roleGuard` (`T-036.3`).

---

## Technical Context

**Language/Version**: C# 12 / .NET 8.0 (`net8.0`) no backend; TypeScript 5.9 / Angular 21.0 no frontend  
**Primary Dependencies**:
- Backend: ASP.NET Core Web API, Entity Framework Core 8.0, `QuestPDF` (`LicenseType.Community`), xUnit 2.9.3, Moq 4.20.72, FluentAssertions 6.12.2
- Frontend: Angular 21 Standalone Components + Signals, ReactiveFormsModule, PrimeNG 21 (`ButtonModule`, `TagModule`), `@angular/router`, `@angular/common/http`, Vitest 4.0.8  
**Storage**: MySQL 8+ (produção/desenvolvimento) e EF Core InMemory (testes) via `IOrdemServicoRepository`, `ISegUsuarioRepository`, `ISegPerfilRepository` e `ISegPermissaoRepository`  
**Testing**: `dotnet test` (backend) e `npm test -- --watch=false` (frontend)  
**Target Platform**: Web Application (API RESTful .NET 8 + SPA Angular 21)  
**Project Type**: Full-stack Web Application (`oficina-motos-api` + `oficina-motos-web`)  
**Performance Goals**: Geração in-memory de PDF de OS em milissegundos via `QuestPDF` e carregamento reativo com Signals nas telas de Segurança  
**Constraints**: Manter compatibilidade com `OrdemServico.Id` (`long`) e sobrecarga `Guid`, exigir autenticação JWT (`[Authorize]`), centralizar URLs em `api-paths.ts` e preservar `strict: true` no Angular  
**Scale/Scope**: Pacote `QuestPDF` + `IPdfService`/`PdfService` + endpoint em `OrdemServicosController` + botão em `OsDetalheComponent` + `RoleGuard` + `SegurancaService` + 3 componentes de página (`UsuarioListaComponent`, `UsuarioFormComponent`, `PerfilListaComponent`) + testes automatizados em backend e frontend

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | `OrdemServicosController` delega a geração do PDF para `IPdfService` na camada Application; lógica de negócio fora dos Controllers e UI. |
| **II. API RESTful Versionada** | ✅ Pass | Endpoint `/api/v1/OrdemServicos/{id}/pdf` e endpoints `/api/v1/Usuarios`, `/api/v1/Perfis` e `/api/v1/Permissoes` centralizados em `api-paths.ts`. |
| **III. Segurança por Design (RBAC + LGPD)** | ✅ Pass | Endpoints protegidos por `[Authorize]` e rotas `/seguranca/usuarios` e `/seguranca/perfis` protegidas por `roleGuard` (`Admin` / `Administrador`). Senhas hasheadas com BCrypt custo 12. |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `UsuarioListaComponent`, `UsuarioFormComponent` e `PerfilListaComponent` são Standalone (`standalone: true`) com Signals e `ChangeDetectionStrategy.OnPush`. |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Respeita PKs `bigint` (`long`) e relacionamentos de `seg_usuarios`, `seg_perfis`, `seg_permissoes` e `os_ordens_servico`. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Inclui testes unitários no backend (`PdfServiceTests`) e no frontend (`os-detalhe.spec.ts`, `role.guard.spec.ts`, `seguranca.service.spec.ts`, `usuario-lista.spec.ts`, `usuario-form.spec.ts`, `perfil-lista.spec.ts`). |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Atende integralmente às tasks `T-035.1`..`T-035.4` e `T-036.1`..`T-036.4` de `governance/backlog.md`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/029-os-pdf-seguranca-ui/
├── spec.md              # Especificação de requisitos (US-035 e US-036)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Decisões técnicas (QuestPDF, Blob download, RoleGuard e UI Segurança)
├── data-model.md        # Fase 1: Contratos de dados C# e interfaces TypeScript
├── quickstart.md        # Fase 1: Guia rápido de validação e testes
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    └── os-pdf-seguranca-contract.md # Fase 1: Contratos HTTP, de rotas e de componentes
```

### Source Code (repository root)

```text
oficina-motos-api/
└── src/
    ├── OficinaMotos.API/
    │   ├── OficinaMotos.API.csproj                                            # [MODIFICAR] Referenciar QuestPDF (T-035.1)
    │   └── Controllers/
    │       └── OrdemServico/
    │           └── OrdemServicosController.cs                                 # [MODIFICAR] Injetar IPdfService e expor GET /api/v1/OrdemServicos/{id}/pdf (T-035.3)
    ├── OficinaMotos.Application/
    │   ├── OficinaMotos.Application.csproj                                    # [MODIFICAR] Instalar pacote QuestPDF (T-035.1)
    │   ├── DTOs/
    │   │   └── Requests/
    │   │       └── Seguranca/
    │   │           └── SegUsuarioRequestDTO.cs                                # [MODIFICAR] Incluir PerfilId opcional em UpdateSegUsuarioDTO
    │   ├── Interfaces/
    │   │   └── OrdemServico/
    │   │       └── IPdfService.cs                                             # [CRIAR] Interface de geração de PDF da OS (T-035.2)
    │   ├── IoC/
    │   │   └── DependencyInjection.cs                                         # [MODIFICAR] Configurar QuestPDF Community License e registrar IPdfService -> PdfService (T-035.1, T-035.2)
    │   └── Services/
    │       ├── OrdemServico/
    │       │   └── PdfService.cs                                              # [CRIAR] Implementação QuestPDF com GerarOrdemServicoPdfAsync(long id) e GerarOrdemServicoPdf(Guid/long id) (T-035.2)
    │       └── Seguranca/
    │           └── SegUsuarioService.cs                                       # [MODIFICAR] Suportar atualização de PerfilId em UpdateAsync
    └── OficinaMotos.Tests/
        └── Services/
            └── OrdemServico/
                └── PdfServiceTests.cs                                         # [CRIAR] Testes unitários do PdfService e endpoint de PDF (T-035.2, T-035.3)

oficina-motos-web/
└── src/
    └── app/
        ├── app.routes.ts                                                      # [MODIFICAR] Registrar /seguranca/usuarios, /seguranca/usuarios/novo, /seguranca/usuarios/:id/editar e /seguranca/perfis com roleGuard (T-036.3, T-036.4)
        ├── core/
        │   ├── auth/
        │   │   ├── role.guard.ts                                              # [CRIAR] RoleGuard restringindo acesso a role Admin / Administrador (T-036.4)
        │   │   ├── role.guard.spec.ts                                         # [CRIAR] Testes unitários do RoleGuard (T-036.4)
        │   │   └── index.ts                                                   # [MODIFICAR] Exportar roleGuard / RoleGuard
        │   ├── models/
        │   │   ├── seguranca.ts                                               # [CRIAR] Interfaces TypeScript de Usuários, Perfis e Permissões
        │   │   └── index.ts                                                   # [MODIFICAR] Exportar models de seguranca
        │   └── services/
        │       ├── api-paths.ts                                               # [MODIFICAR] Adicionar ordens.pdf e seguranca (usuarios, perfis, permissoes, modulos)
        │       ├── api-client.service.ts                                      # [MODIFICAR] Adicionar downloadBlob(path)
        │       ├── ordens.service.ts                                          # [MODIFICAR] Adicionar downloadPdf(id) (T-035.4)
        │       ├── seguranca.service.ts                                       # [CRIAR] Serviço HTTP para Usuários, Perfis e Permissões (T-036.1, T-036.2)
        │       └── seguranca.service.spec.ts                                  # [CRIAR] Testes unitários do SegurancaService
        ├── features/
        │   ├── ordens-servico/
        │   │   └── pages/
        │   │       └── os-detalhe/
        │   │           ├── os-detalhe.ts                                      # [MODIFICAR] Adicionar imprimirOs() e estado printingPdf (T-035.4)
        │   │           ├── os-detalhe.html                                    # [MODIFICAR] Adicionar botão "Imprimir OS" (T-035.4)
        │   │           └── os-detalhe.spec.ts                                 # [MODIFICAR] Testar botão "Imprimir OS" e download de PDF (T-035.4)
        │   └── seguranca/
        │       └── pages/
        │           ├── usuario-lista/
        │           │   ├── usuario-lista.ts                                   # [CRIAR] UsuarioListaComponent (T-036.1)
        │           │   ├── usuario-lista.html                                 # [CRIAR] Template de listagem de usuários
        │           │   ├── usuario-lista.scss                                 # [CRIAR] Estilos da listagem de usuários
        │           │   └── usuario-lista.spec.ts                              # [CRIAR] Testes unitários de UsuarioListaComponent
        │           ├── usuario-form/
        │           │   ├── usuario-form.ts                                    # [CRIAR] UsuarioFormComponent (T-036.1)
        │           │   ├── usuario-form.html                                  # [CRIAR] Template de criação/edição de usuário
        │           │   ├── usuario-form.scss                                  # [CRIAR] Estilos do formulário de usuário
        │           │   └── usuario-form.spec.ts                               # [CRIAR] Testes unitários de UsuarioFormComponent
        │           └── perfil-lista/
        │               ├── perfil-lista.ts                                    # [CRIAR] PerfilListaComponent (T-036.2)
        │               ├── perfil-lista.html                                  # [CRIAR] Template de listagem e gestão de perfis/permissões
        │               ├── perfil-lista.scss                                  # [CRIAR] Estilos da listagem de perfis
        │               └── perfil-lista.spec.ts                               # [CRIAR] Testes unitários de PerfilListaComponent
        └── layout/
            └── sidebar/
                └── sidebar.ts                                                 # [MODIFICAR] Adicionar item de navegação para Segurança (/seguranca/usuarios)
```

**Structure Decision**: Segue rigorosamente a arquitetura Clean Architecture do backend (`API/Controllers/OrdemServico`, `Application/Services/OrdemServico`) e a organização por feature com Standalone Components e Signals no frontend (`features/ordens-servico/pages/os-detalhe` e `features/seguranca/pages/`).

---

## Fases de Execução

### Fase 1: Relatório PDF de Ordem de Serviço no Backend (`US-035`: `T-035.1` a `T-035.3`)
1. Instalar o pacote `QuestPDF` em `OficinaMotos.Application` e `OficinaMotos.API` (`T-035.1`).
2. Criar `IPdfService.cs` e `PdfService.cs` configurando `QuestPDF.Settings.License = LicenseType.Community` e implementando `GerarOrdemServicoPdfAsync(long id)`, `GerarOrdemServicoPdf(long id)` e `GerarOrdemServicoPdf(Guid id)` (`T-035.2`).
3. Registrar `IPdfService` em `DependencyInjection.cs`.
4. Adicionar `[HttpGet("{id}/pdf")]` em `OrdemServicosController.cs` retornando `application/pdf` (`T-035.3`).
5. Criar testes automatizados em `PdfServiceTests.cs` cobrindo o `PdfService` e o endpoint `GetPdf` do `OrdemServicosController`.

### Fase 2: Botão "Imprimir OS" no Frontend (`US-035`: `T-035.4`)
1. Atualizar `api-paths.ts`, `api-client.service.ts` e `ordens.service.ts` com `downloadPdf(id)`.
2. Adicionar botão `"Imprimir OS"` e método `imprimirOs()` em `OsDetalheComponent` (`os-detalhe.ts` / `os-detalhe.html`) (`T-035.4`).
3. Atualizar `os-detalhe.spec.ts` com testes unitários para o botão `"Imprimir OS"`.

### Fase 3: `RoleGuard`, Rotas e Componentes de Segurança (`US-036`: `T-036.1` a `T-036.4`)
1. Criar `roleGuard` (`RoleGuard`) em `core/auth/role.guard.ts` e `role.guard.spec.ts` restringindo acesso a `Admin` / `Administrador` (`T-036.4`).
2. Criar `core/models/seguranca.ts`, atualizar `api-paths.ts` e criar `SegurancaService` (`core/services/seguranca.service.ts` + `.spec.ts`).
3. Criar `UsuarioListaComponent` e `UsuarioFormComponent` (+ specs) em `features/seguranca/pages/` (`T-036.1`).
4. Criar `PerfilListaComponent` (+ spec) em `features/seguranca/pages/` (`T-036.2`).
5. Registrar as rotas `/seguranca/usuarios` (e `/novo`, `/:id/editar`) e `/seguranca/perfis` com `canActivate: [roleGuard]` em `app.routes.ts` (`T-036.3`, `T-036.4`) e adicionar item de menu no `AppSidebar`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
