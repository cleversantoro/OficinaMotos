---
description: "Task list for feature 029-os-pdf-seguranca-ui"
---

# Tasks: US-035 & US-036 — Relatório PDF de Ordem de Serviço e Interface de Gerenciamento de Usuários e Perfis

**Input**: Design documents from `/specs/029-os-pdf-seguranca-ui/`  
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/os-pdf-seguranca-contract.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`  
**Tests**: Suítes de testes unitários xUnit (`PdfServiceTests`) em `oficina-motos-api/src/OficinaMotos.Tests/` (`US-035`) e suítes Vitest (`os-detalhe.spec.ts`, `role.guard.spec.ts`, `seguranca.service.spec.ts`, `usuario-lista.spec.ts`, `usuario-form.spec.ts`, `perfil-lista.spec.ts`) em `oficina-motos-web/src/app/` (`US-035` & `US-036`).  
**Organization**: Tarefas organizadas por fase (`Setup` → `Foundational` → `US1` → `US2` → `US3` → `US4` → `Polish`), mapeadas às tasks oficiais do backlog (`T-035.1` a `T-035.4` e `T-036.1` a `T-036.4`).

## Format: `[ID] [P?] [Story?] Description`

- **[P]**: Pode ser executada em paralelo (arquivos distintos, sem dependência de tarefa incompleta)
- **[Story]**: História de usuário da `spec.md` à qual a tarefa pertence (`[US1]`, `[US2]`, `[US3]`, `[US4]`)
- Caminhos exatos de arquivo incluídos em todas as descrições

---

## Phase 1: Setup (Shared Dependencies)

**Purpose**: Instalar e configurar o pacote `QuestPDF` no backend (`T-035.1`) antes da implementação dos serviços de geração de PDF.

- [X] T001 Instalar o pacote NuGet `QuestPDF` em `oficina-motos-api/src/OficinaMotos.Application/OficinaMotos.Application.csproj` e referenciar em `oficina-motos-api/src/OficinaMotos.API/OficinaMotos.API.csproj` (`T-035.1`)

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Preparar contratos, DTOs, rotas de API e helpers HTTP compartilhados necessários para as histórias de PDF e Segurança.

**⚠️ CRITICAL**: Necessário antes da implementação das User Stories 1 a 4.

- [X] T002 [P] Criar a interface `IPdfService` com `GerarOrdemServicoPdfAsync(long id)`, `GerarOrdemServicoPdf(long id)` e sobrecarga `GerarOrdemServicoPdf(Guid id)` em `oficina-motos-api/src/OficinaMotos.Application/Interfaces/OrdemServico/IPdfService.cs` (`T-035.2`)
- [X] T003 [P] Adicionar `PerfilId` opcional em `UpdateSegUsuarioDTO` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/Seguranca/SegUsuarioRequestDTO.cs` e suportar atualização do vínculo `SegUsuarioPerfil` em `SegUsuarioService.UpdateAsync` em `oficina-motos-api/src/OficinaMotos.Application/Services/Seguranca/SegUsuarioService.cs` (`T-036.1`)
- [X] T004 [P] Registrar `ordens.pdf` e a seção `seguranca` (`usuarios`, `perfis`, `permissoes`, `modulos`) em `oficina-motos-web/src/app/core/services/api-paths.ts` (`T-035.4`, `T-036.1`, `T-036.2`)
- [X] T005 [P] Adicionar o método `downloadBlob(path: string)` com `responseType: 'blob'` em `oficina-motos-web/src/app/core/services/api-client.service.ts` (`T-035.4`)
- [X] T006 [P] Criar as interfaces TypeScript de usuários, perfis e permissões em `oficina-motos-web/src/app/core/models/seguranca.ts` e reexportar em `oficina-motos-web/src/app/core/models/index.ts` (`T-036.1`, `T-036.2`)

**Checkpoint**: Dependências e contratos base prontos para implementação paralela das histórias de usuário.

---

## Phase 3: User Story 1 — Gerar Relatório PDF da Ordem de Serviço no Backend (Priority: P1) 🎯 MVP (`US-035`: `T-035.1`, `T-035.2`, `T-035.3`)

**Goal**: Implementar `PdfService` usando `QuestPDF` (`LicenseType.Community`), registrar em DI e expor o endpoint autenticado `GET /api/v1/OrdemServicos/{id}/pdf` no `OrdemServicosController`.

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Pdf"` em `oficina-motos-api/` confirmando geração de bytes `%PDF-` para OS existente e `404 NotFound` / `null` para OS inexistente.

- [X] T007 [US1] Implementar `PdfService` com `QuestPDF.Settings.License = LicenseType.Community`, layout completo da OS (dados gerais, cliente, veículo, mecânico, itens, observações e resumo de pagamentos) e métodos `GerarOrdemServicoPdfAsync(long id)`, `GerarOrdemServicoPdf(long id)` e `GerarOrdemServicoPdf(Guid id)` em `oficina-motos-api/src/OficinaMotos.Application/Services/OrdemServico/PdfService.cs` (`T-035.2`)
- [X] T008 [US1] Configurar `QuestPDF.Settings.License = LicenseType.Community` e registrar `services.AddScoped<Interfaces.OrdemServico.IPdfService, Services.OrdemServico.PdfService>()` em `oficina-motos-api/src/OficinaMotos.Application/IoC/DependencyInjection.cs` (`T-035.1`, `T-035.2`)
- [X] T009 [US1] Injetar `IPdfService` e expor o endpoint `[HttpGet("{id:long}/pdf")]` retornando `File(pdfBytes, "application/pdf", $"os-{id}.pdf")` ou `404 NotFound` em `oficina-motos-api/src/OficinaMotos.API/Controllers/OrdemServico/OrdemServicosController.cs` (`T-035.3`)
- [X] T010 [P] [US1] Criar `PdfServiceTests` cobrindo `GerarOrdemServicoPdfAsync(long id)`, `GerarOrdemServicoPdf(Guid id)`, caso de OS inexistente e retorno HTTP do `OrdemServicosController.GetPdf` em `oficina-motos-api/src/OficinaMotos.Tests/Services/OrdemServico/PdfServiceTests.cs` (`T-035.2`, `T-035.3`)

**Checkpoint**: Endpoint `GET /api/v1/OrdemServicos/{id}/pdf` funcional e validado com `QuestPDF` (`T-035.1` a `T-035.3`).

---

## Phase 4: User Story 2 — Botão "Imprimir OS" no `OsDetalheComponent` (Priority: P2) (`US-035`: `T-035.4`)

**Goal**: Permitir que o usuário baixe/imprima o relatório PDF diretamente da tela de detalhes da Ordem de Serviço clicando no botão `"Imprimir OS"`.

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` e confirmar que `os-detalhe.spec.ts` valida a renderização do botão `"Imprimir OS"` e a chamada a `OrdensService.downloadPdf(id)`.

- [X] T011 [US2] Adicionar `downloadPdf(id: string | number)` utilizando `apiPaths.ordens.pdf(id)` e `api.downloadBlob` em `oficina-motos-web/src/app/core/services/ordens.service.ts` (`T-035.4`)
- [X] T012 [US2] Adicionar estado `printingPdf`, método `imprimirOs()` e botão `"Imprimir OS"` no cabeçalho de `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.ts` e `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.html` (`T-035.4`)
- [X] T013 [P] [US2] Atualizar testes unitários cobrindo exibição do botão `"Imprimir OS"`, disparo de `downloadPdf(id)` e tratamento de erro em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.spec.ts` (`T-035.4`)

**Checkpoint**: Fluxo ponta a ponta da `US-035` (`T-035.1` a `T-035.4`) concluído e testado.

---

## Phase 5: User Story 3 — Proteger Rotas com `RoleGuard` e Registrar `/seguranca/usuarios` e `/seguranca/perfis` (Priority: P1) (`US-036`: `T-036.3`, `T-036.4`)

**Goal**: Criar `roleGuard` (`RoleGuard`) restringindo o acesso exclusivamente ao perfil Administrador (`Admin` / `Administrador`) e registrar as rotas de segurança em `app.routes.ts` e no menu lateral.

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` validando `role.guard.spec.ts` para roles permitidas (`Admin`, `Administrador`) e bloqueadas (`Gerente`, `Mecânico`, `Recepcionista`, `null`).

- [X] T014 [US3] Criar `roleGuard` (e exportar alias `RoleGuard`) validando `Admin` / `Administrador` com redirecionamento para `/dashboard` e `toast.warn` em `oficina-motos-web/src/app/core/auth/role.guard.ts` e reexportar em `oficina-motos-web/src/app/core/auth/index.ts` (`T-036.4`)
- [X] T015 [P] [US3] Criar testes unitários `role.guard.spec.ts` cobrindo acesso permitido (`Admin`, `Administrador`, case-insensitive) e bloqueio para demais perfis em `oficina-motos-web/src/app/core/auth/role.guard.spec.ts` (`T-036.4`)
- [X] T016 [US3] Registrar rotas `/seguranca/usuarios`, `/seguranca/usuarios/novo`, `/seguranca/usuarios/:id/editar` e `/seguranca/perfis` protegidas por `roleGuard` (lazy loading) em `oficina-motos-web/src/app/app.routes.ts` e adicionar navegação de Segurança em `oficina-motos-web/src/app/layout/sidebar/sidebar.ts` (`T-036.3`, `T-036.4`)

**Checkpoint**: Rotas de segurança protegidas por `RoleGuard` e devidamente registradas (`T-036.3`, `T-036.4`).

---

## Phase 6: User Story 4 — Interface de Gerenciamento de Usuários e Perfis (Priority: P1) (`US-036`: `T-036.1`, `T-036.2`)

**Goal**: Implementar `SegurancaService`, `UsuarioListaComponent`, `UsuarioFormComponent` e `PerfilListaComponent` integrados às APIs `/api/v1/Usuarios`, `/api/v1/Perfis` e `/api/v1/Permissoes`.

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` confirmando 100% de aprovação em `seguranca.service.spec.ts`, `usuario-lista.spec.ts`, `usuario-form.spec.ts` e `perfil-lista.spec.ts`.

- [X] T017 [US4] Criar `SegurancaService` com métodos CRUD para usuários, perfis, permissões e associação perfil-permissões em `oficina-motos-web/src/app/core/services/seguranca.service.ts` (`T-036.1`, `T-036.2`)
- [X] T018 [P] [US4] Criar `seguranca.service.spec.ts` validando chamadas HTTP para `/api/v1/Usuarios`, `/api/v1/Perfis` e `/api/v1/Permissoes` em `oficina-motos-web/src/app/core/services/seguranca.service.spec.ts` (`T-036.1`, `T-036.2`)
- [X] T019 [P] [US4] Criar `UsuarioListaComponent` (listagem, busca, status, atalhos para `/seguranca/perfis` e `/seguranca/usuarios/novo`, edição e exclusão) em `oficina-motos-web/src/app/features/seguranca/pages/usuario-lista/usuario-lista.ts`, `.html` e `.scss` (`T-036.1`)
- [X] T020 [P] [US4] Criar `UsuarioFormComponent` (criação e edição de usuário com seleção de perfil e validações reativas) em `oficina-motos-web/src/app/features/seguranca/pages/usuario-form/usuario-form.ts`, `.html` e `.scss` (`T-036.1`)
- [X] T021 [P] [US4] Criar `PerfilListaComponent` (listagem de perfis, criação/edição de perfil e gestão de permissões vinculadas) em `oficina-motos-web/src/app/features/seguranca/pages/perfil-lista/perfil-lista.ts`, `.html` e `.scss` (`T-036.2`)
- [X] T022 [P] [US4] Criar testes unitários para `UsuarioListaComponent`, `UsuarioFormComponent` e `PerfilListaComponent` em `oficina-motos-web/src/app/features/seguranca/pages/usuario-lista/usuario-lista.spec.ts`, `usuario-form/usuario-form.spec.ts` e `perfil-lista/perfil-lista.spec.ts` (`T-036.1`, `T-036.2`)

**Checkpoint**: Interface completa de gerenciamento de usuários e perfis (`T-036.1`, `T-036.2`) funcional e coberta por testes unitários.

---

## Phase 7: Polish, Execução de Testes & Governança

**Purpose**: Validar builds e suítes completas de backend e frontend, e atualizar `governance/backlog.md`.

- [X] T023 Executar `dotnet build OficinaMotos.slnx` e `dotnet test OficinaMotos.slnx` em `oficina-motos-api/` confirmando 100% de aprovação nos testes unitários e de integração
- [X] T024 Executar `npm test -- --watch=false` e `npm run build` em `oficina-motos-web/` confirmando 100% de aprovação em todas as specs e build limpo
- [X] T025 Atualizar o status de `US-035` (`T-035.1`..`T-035.4`) e `US-036` (`T-036.1`..`T-036.4`) para concluído (`[x]`) em `governance/backlog.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: Sem dependências — instala `QuestPDF` (`T001`).
- **Foundational (Phase 2)**: Depende da Fase 1 para `IPdfService`/`QuestPDF`; `T002`–`T006` podem rodar em paralelo.
- **US1 (Phase 3 - `US-035` Backend)**: Depende de `T001` e `T002`.
- **US2 (Phase 4 - `US-035` Frontend)**: Depende de `T004`, `T005` e do contrato da US1 (`T009`).
- **US3 (Phase 5 - `US-036` Guard & Rotas)**: Depende da Fase 2 (`T004`) e dos componentes da US4 para carregamento nas rotas (ou criação conjunta com `T019`–`T021`).
- **US4 (Phase 6 - `US-036` UI Usuários & Perfis)**: Depende de `T003`, `T004` e `T006`.
- **Polish (Phase 7)**: Depende de todas as User Stories concluídas.

### Parallel Opportunities

```bash
# Fase 2 em paralelo:
T002 (IPdfService.cs) | T003 (UpdateSegUsuarioDTO/Service) | T004 (api-paths.ts) | T005 (api-client.service.ts) | T006 (seguranca.ts)

# Fase 6 em paralelo após T017 (SegurancaService):
T018 (seguranca.service.spec.ts) | T019 (UsuarioListaComponent) | T020 (UsuarioFormComponent) | T021 (PerfilListaComponent)
```

---

## Implementation Strategy

1. **MVP 1 (`US-035` Backend + Frontend)**: Executar Phases 1, 2, 3 e 4 para entregar a geração de PDF via `QuestPDF` e o botão `"Imprimir OS"` no detalhe da OS.
2. **MVP 2 (`US-036` Segurança UI + RoleGuard)**: Executar Phases 5 e 6 para entregar `RoleGuard`, rotas `/seguranca/usuarios` e `/seguranca/perfis`, e os componentes de gerenciamento de usuários e perfis.
3. **Validação Final**: Executar Phase 7 (`dotnet test`, `npm test -- --watch=false`, `npm run build` e atualização de `governance/backlog.md`).
