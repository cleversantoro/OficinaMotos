# Especificação de Funcionalidade: Relatório PDF de Ordem de Serviço e Interface de Gerenciamento de Usuários e Perfis (US-035 & US-036)

**Feature Branch**: `029-os-pdf-seguranca-ui`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-035 — Gerar relatório PDF de Ordem de Serviço | Prioridade: 🟢 Could | Estimativa: L | Sprint: 6 | Depende: US-010 | Tasks: [ ] T-035.1 — Instalar QuestPDF no projeto API, [ ] T-035.2 — Criar PdfService.GerarOrdemServicoPdf(Guid id), [ ] T-035.3 — Endpoint GET /api/v1/OrdemServicos/{id}/pdf, [ ] T-035.4 — Botão \"Imprimir OS\" no OsDetalheComponent || US-036 — Interface de gerenciamento de usuários e perfis | Prioridade: 🟡 Should | Estimativa: M | Sprint: 6 | Depende: US-001, US-004 | Tasks: [ ] T-036.1 — Criar UsuarioListaComponent e UsuarioFormComponent, [ ] T-036.2 — Criar PerfilListaComponent, [ ] T-036.3 — Registrar rotas /seguranca/usuarios e /seguranca/perfis, [ ] T-036.4 — RoleGuard restringindo acesso a role Admin"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Gerar Relatório PDF da Ordem de Serviço no Backend (`QuestPDF`, `PdfService` e `OrdemServicosController`) (Priority: P1)

Como colaborador autenticado da oficina, quero gerar e baixar o documento PDF oficial de uma Ordem de Serviço através do endpoint `GET /api/v1/OrdemServicos/{id}/pdf` (`T-035.3`), apoiado pelo pacote `QuestPDF` (`T-035.1`) e pelo serviço `PdfService` (`T-035.2`), contendo os dados gerais da OS (número, status, datas, cliente, veículo, mecânico, descrição do problema), a tabela de itens (peças e serviços com quantidades, valores unitários e subtotais) e o resumo financeiro (pagamentos registrados, total da OS, total pago e saldo pendente).

**Why this priority**: Cumpre as tarefas de backend da `US-035` (`T-035.1`, `T-035.2` e `T-035.3`), expondo o contrato RESTful versionado (`/api/v1/OrdemServicos/{id}/pdf`, Princípios I, II e III da Constituição) necessário para a funcionalidade de impressão no detalhe da OS.

**Independent Test**: Executar `dotnet test` em `oficina-motos-api/OficinaMotos.slnx` validando `PdfService` (geração de bytes PDF válidos com cabeçalho `%PDF-` para uma OS existente e tratamento quando inexistente) e `OrdemServicosController.GetPdf` (`200 OK` com `application/pdf` e `404 NotFound` quando a OS não existir).

**Acceptance Scenarios**:

1. **Given** uma Ordem de Serviço cadastrada no sistema com cliente, veículo, mecânico, itens e pagamentos, **When** `PdfService.GerarOrdemServicoPdfAsync(long id)` / `PdfService.GerarOrdemServicoPdf(...)` (`T-035.2`) é invocado utilizando `QuestPDF` (`T-035.1`) com licença `LicenseType.Community`, **Then** o serviço retorna um array de bytes (`byte[]`) não vazio representando um documento PDF válido (`%PDF`) contendo os dados da OS.
2. **Given** um usuário autenticado solicitando `GET /api/v1/OrdemServicos/{id}/pdf` (`T-035.3`) para um `id` existente, **When** `OrdemServicosController` processa a requisição, **Then** retorna `200 OK` com `Content-Type: application/pdf` e nome de arquivo `os-{id}.pdf`.
3. **Given** uma requisição para `GET /api/v1/OrdemServicos/{id}/pdf` com um `id` inexistente, **When** `OrdemServicosController` consulta o serviço, **Then** retorna `404 NotFound` com mensagem descritiva.

---

### User Story 2 - Botão "Imprimir OS" no `OsDetalheComponent` (Priority: P2)

Como atendente, mecânico ou gestor visualizando os detalhes de uma Ordem de Serviço no frontend Angular, quero clicar no botão `"Imprimir OS"` no `OsDetalheComponent` (`T-035.4`) para requisitar o PDF em `GET /api/v1/OrdemServicos/{id}/pdf` (`Blob`) e abrir/baixar o relatório PDF da OS com feedback visual de carregamento e tratamento de erro via toast.

**Why this priority**: Completa a entrega ponta a ponta da `US-035` (`T-035.4`) integrando o `OsDetalheComponent` (implementado na `US-010`) ao endpoint de PDF sem violar o Princípio II da Constituição (uso centralizado de `apiPaths.ordens` e `OrdensService`).

**Independent Test**: Executar `npm test -- --watch=false` em `oficina-motos-web/` validando que `OsDetalheComponent` exibe o botão `"Imprimir OS"`, invoca `OrdensService.downloadPdf(id)` (que chama `/api/v1/OrdemServicos/{id}/pdf` com `responseType: 'blob'`), e aciona a abertura/download do arquivo PDF.

**Acceptance Scenarios**:

1. **Given** que o usuário está na tela de detalhe de uma Ordem de Serviço (`/ordens/:id`) com dados carregados, **When** visualiza o cabeçalho da página em `OsDetalheComponent`, **Then** o botão `"Imprimir OS"` (`T-035.4`) está visível e habilitado.
2. **Given** que o usuário clica no botão `"Imprimir OS"`, **When** `OrdensService.downloadPdf(id)` retorna o `Blob` (`application/pdf`) de `GET /api/v1/OrdemServicos/{id}/pdf`, **Then** o sistema gera a URL do objeto (`URL.createObjectURL`) para download/impressão do PDF (`os-{id}.pdf`) e exibe feedback adequado, ou apresenta `toast.error` caso a requisição falhe.

---

### User Story 3 - Proteger Rotas Administrativas com `RoleGuard` e Registrar Rotas `/seguranca/usuarios` e `/seguranca/perfis` (Priority: P1)

Como administrador do sistema, quero que as rotas `/seguranca/usuarios` (incluindo criação/edição) e `/seguranca/perfis` estejam registradas em `app.routes.ts` (`T-036.3`) e protegidas por `roleGuard` (`RoleGuard`) restringindo o acesso exclusivamente ao perfil de Administrador (`Admin` / `Administrador`) (`T-036.4`), redirecionando usuários sem esse perfil para `/dashboard` com aviso de acesso negado.

**Why this priority**: Cumpre `T-036.3` e `T-036.4` e assegura o Princípio III da Constituição (Segurança por Design / RBAC no frontend com guards de rota baseados no perfil do usuário autenticado).

**Independent Test**: Executar testes unitários em `role.guard.spec.ts` verificando que usuários com role `Administrador` (ou `Admin` / configurada via `route.data['roles']`) têm acesso permitido (`true`), enquanto outros perfis (`Gerente`, `Recepcionista`, `Mecânico`, `Financeiro`, `Consulta` ou não autenticado) são bloqueados, recebem `toast.warn` e são redirecionados para `/dashboard`.

**Acceptance Scenarios**:

1. **Given** um usuário autenticado com perfil `Administrador` (ou `Admin`), **When** navega para `/seguranca/usuarios`, `/seguranca/usuarios/novo`, `/seguranca/usuarios/:id/editar` ou `/seguranca/perfis` (`T-036.3`), **Then** o `roleGuard` (`T-036.4`) permite a ativação da rota (`true`).
2. **Given** um usuário autenticado com perfil diferente de Administrador (ex.: `Gerente`, `Mecânico`, `Recepcionista`), **When** tenta acessar qualquer rota sob `/seguranca/*`, **Then** o `roleGuard` bloqueia a navegação, exibe alerta de acesso negado via `Toast` e redireciona para `/dashboard`.

---

### User Story 4 - Gerenciar Usuários e Perfis na Interface de Segurança (`UsuarioListaComponent`, `UsuarioFormComponent` e `PerfilListaComponent`) (Priority: P1)

Como Administrador autenticado no sistema, quero listar usuários com seus perfis e status em `UsuarioListaComponent` (`T-036.1`), cadastrar ou editar usuários (incluindo seleção de perfil e definição/atualização de dados) em `UsuarioFormComponent` (`T-036.1`), e consultar/gerenciar os perfis de acesso e suas permissões associadas em `PerfilListaComponent` (`T-036.2`).

**Why this priority**: Cumpre `T-036.1` e `T-036.2`, entregando a interface administrativa de segurança conectada aos endpoints existentes `/api/v1/Usuarios`, `/api/v1/Perfis` e `/api/v1/Permissoes` (implementados nas `US-001` e `US-004`).

**Independent Test**: Executar `npm test -- --watch=false` e `npm run build` em `oficina-motos-web/` validando `SegurancaService`, `UsuarioListaComponent`, `UsuarioFormComponent` e `PerfilListaComponent`.

**Acceptance Scenarios**:

1. **Given** que o Administrador acessa `/seguranca/usuarios`, **When** `UsuarioListaComponent` (`T-036.1`) é inicializado, **Then** consulta `GET /api/v1/Usuarios`, exibe a listagem de usuários (nome, login, e-mail, perfis, status e último login) e oferece ações para criar novo usuário (`/seguranca/usuarios/novo`), editar (`/seguranca/usuarios/:id/editar`) e excluir usuário.
2. **Given** que o Administrador acessa `/seguranca/usuarios/novo` ou `/seguranca/usuarios/:id/editar`, **When** interage com `UsuarioFormComponent` (`T-036.1`), **Then** o formulário valida os campos obrigatórios (nome, login, e-mail, perfil de acesso carregado de `GET /api/v1/Perfis`, e senha obrigatória na criação / status na edição) e envia `POST /api/v1/Usuarios` ou `PUT /api/v1/Usuarios/{id}`.
3. **Given** que o Administrador acessa `/seguranca/perfis`, **When** `PerfilListaComponent` (`T-036.2`) é inicializado, **Then** consulta `GET /api/v1/Perfis`, exibe os perfis cadastrados (nome, descrição, nível, status e quantidade/lista de permissões) e permite visualizar/atualizar as permissões ou criar/editar perfis.

---

### Edge Cases

- **Compatibilidade de tipo do identificador em `PdfService.GerarOrdemServicoPdf` (`T-035.2` vs modelo de domínio `BaseEntity.Id: long`)**: No banco de dados e na API (`OrdemServico : BaseEntity` e `OrdemServicosController`), o identificador das entidades é `long` (`bigint`, conforme Princípio V da Constituição), enquanto a descrição textual da task `T-035.2` menciona `PdfService.GerarOrdemServicoPdf(Guid id)`. Para garantir compatibilidade total tanto com o domínio real (`long id`) quanto com a assinatura nominal da task (`Guid id`), `IPdfService` / `PdfService` deve prover `Task<byte[]?> GerarOrdemServicoPdfAsync(long id)` (e `byte[]? GerarOrdemServicoPdf(long id)`) além da sobrecarga `byte[]? GerarOrdemServicoPdf(Guid id)` (que converte/resolve ou delega adequadamente), e o endpoint `GET /api/v1/OrdemServicos/{id}/pdf` (`T-035.3`) deve aceitar `{id:long}` consistente com os demais endpoints de `OrdemServicosController`.
- **Configuração de licença do `QuestPDF` (`T-035.1`)**: O `QuestPDF` versão 2022.12+ exige que `QuestPDF.Settings.License = LicenseType.Community;` seja configurado antes da geração de qualquer documento para não lançar exceção em runtime ou em testes automatizados. Essa configuração deve ser garantida tanto na inicialização do `PdfService` (construtor estático/instância) quanto no registro de DI.
- **Equivalência de nomenclatura da role de Administrador no `RoleGuard` (`T-036.4`)**: No backend e na Constituição, o perfil de nível 1 chama-se `"Administrador"`, enquanto a task `T-036.4` refere-se a `"Admin"`. O `RoleGuard` (`roleGuard`) deve utilizar normalização case-insensitive e tratar `"Admin"` e `"Administrador"` como equivalentes (aceitando usuários cujo `currentRole()` seja `"Administrador"` ou `"Admin"` quando a rota exigir `['Admin', 'Administrador']`).
- **Atualização de Perfil ao editar Usuário em `UsuarioFormComponent` (`T-036.1`)**: Verificar se `UpdateSegUsuarioDTO` e `SegUsuarioService.UpdateAsync` no backend permitem atualizar o `PerfilId` opcionalmente além de `Nome`, `Email`, `Login`, `Telefone` e `Status`, suportando `PerfilId` em `UpdateSegUsuarioDTO` caso informado pelo `UsuarioFormComponent`.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O projeto backend (`OficinaMotos.Application` e/ou `OficinaMotos.API`) DEVE ter o pacote NuGet `QuestPDF` instalado e configurado com `QuestPDF.Settings.License = LicenseType.Community` (`T-035.1`).
- **FR-002**: A camada Application DEVE disponibilizar `IPdfService` e `PdfService` (registrado via DI em `AddApplication()`) implementando a geração do relatório PDF da Ordem de Serviço (`GerarOrdemServicoPdfAsync(long id)`, `GerarOrdemServicoPdf(long id)` e sobrecarga compatível `GerarOrdemServicoPdf(Guid id)`) contendo cabeçalho da oficina/OS, dados do cliente, veículo e mecânico, descrição do problema, tabela de itens e resumo de pagamentos (`T-035.2`).
- **FR-003**: O `OrdemServicosController` DEVE expor o endpoint autenticado `GET /api/v1/OrdemServicos/{id}/pdf` retornando `File(pdfBytes, "application/pdf", $"os-{id}.pdf")` (`200 OK`) quando a OS existir, ou `404 NotFound` quando não existir (`T-035.3`).
- **FR-004**: O `api-paths.ts` e o `OrdensService` no frontend DEVEM expor o método para requisitar o PDF de `GET /api/v1/OrdemServicos/{id}/pdf` com `responseType: 'blob'`, e o `OsDetalheComponent` DEVE exibir o botão `"Imprimir OS"` no cabeçalho da OS para baixar/abrir o PDF gerado (`T-035.4`).
- **FR-005**: O `api-paths.ts` DEVE centralizar os caminhos de segurança (`seguranca: { usuarios: '/api/v1/Usuarios', perfis: '/api/v1/Perfis', permissoes: '/api/v1/Permissoes', modulos: '/api/v1/Modulos' }`) consumidos por `SegurancaService` (`T-036.1`, `T-036.2`).
- **FR-006**: O frontend DEVE implementar `UsuarioListaComponent` e `UsuarioFormComponent` (standalone components com Signals) em `features/seguranca/pages/` permitindo listar, criar, editar e excluir usuários integrados com `/api/v1/Usuarios` e `/api/v1/Perfis` (`T-036.1`).
- **FR-007**: O frontend DEVE implementar `PerfilListaComponent` (standalone component com Signals) em `features/seguranca/pages/` permitindo listar os perfis de acesso, visualizar suas permissões e gerenciar perfis/permissões integrado com `/api/v1/Perfis` e `/api/v1/Permissoes` (`T-036.2`).
- **FR-008**: O frontend DEVE implementar `roleGuard` (`RoleGuard`) em `core/auth/role.guard.ts` restringindo o acesso às rotas de segurança exclusivamente ao perfil Administrador (`Admin` / `Administrador`), redirecionando usuários não autorizados para `/dashboard` com notificação via `Toast` (`T-036.4`).
- **FR-009**: O arquivo `app.routes.ts` DEVE registrar as rotas `/seguranca/usuarios` (bem como `/seguranca/usuarios/novo` e `/seguranca/usuarios/:id/editar`) e `/seguranca/perfis` protegidas por `roleGuard`, e o menu lateral (`AppSidebar`) DEVE disponibilizar a navegação correspondente para administradores (`T-036.3`, `T-036.4`).

### Key Entities *(include if feature involves data)*

- **`IPdfService` & `PdfService`** (`OficinaMotos.Application.Interfaces.OrdemServico` / `OficinaMotos.Application.Services.OrdemServicoRepo`): Serviço gerador do documento PDF de Ordem de Serviço utilizando `QuestPDF`.
- **`SegUsuario` / `SegUsuarioTableDTO` / `SegUsuarioResponseDTO` / `CreateSegUsuarioDTO` / `UpdateSegUsuarioDTO`**: Entidade e DTOs de usuários do módulo de Segurança (`seg_usuarios`).
- **`SegPerfil` / `SegPerfilResponseDTO` / `SegPermissaoResponseDTO`**: Entidade e DTOs de perfis e permissões do módulo de Segurança (`seg_perfis`, `seg_permissoes`).
- **`RoleGuard` (`roleGuard`)**: Guard funcional e/ou classe Angular em `core/auth/role.guard.ts` que valida se o usuário autenticado possui role `Admin` / `Administrador`.
- **`UsuarioListaComponent`, `UsuarioFormComponent` & `PerfilListaComponent`**: Componentes standalone Angular em `features/seguranca/pages/` para gestão administrativa de usuários e perfis.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `dotnet build` e `dotnet test` em `oficina-motos-api/OficinaMotos.slnx` compilam sem erros e aprovam 100% dos testes unitários e de integração, incluindo testes dedicados para `PdfService` e para o endpoint `GET /api/v1/OrdemServicos/{id}/pdf` (`T-035.1` a `T-035.3`).
- **SC-002**: `npm run build` e `npm test -- --watch=false` em `oficina-motos-web/` compilam sem erros e aprovam 100% das specs, incluindo testes unitários para o botão `"Imprimir OS"` no `OsDetalheComponent` (`T-035.4`), `roleGuard` (`T-036.4`), `SegurancaService`, `UsuarioListaComponent`, `UsuarioFormComponent` (`T-036.1`), `PerfilListaComponent` (`T-036.2`) e registro das rotas `/seguranca/usuarios` e `/seguranca/perfis` (`T-036.3`).

## Assumptions

- O layout do relatório PDF da Ordem de Serviço (`T-035.2`) utiliza a licença gratuita `QuestPDF.Infrastructure.LicenseType.Community` e formata valores monetários no padrão brasileiro (`R$`) e datas em `dd/MM/yyyy HH:mm`.
- O `RoleGuard` (`T-036.4`) aceita tanto a nomenclatura abreviada `"Admin"` quanto a nomenclatura canônica `"Administrador"` emitida no JWT pelo `AuthService` do backend.
