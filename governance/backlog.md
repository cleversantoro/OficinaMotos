# Product Backlog — Oficina MotoPro

**Gerado em:** 2026-07-18
**Versão:** 1.0
**Base:** Auditoria 2026-07-18 · Cobertura 2026-07-18
**Estado do projeto:** Backend ~72% · Frontend ~48% · Banco ~85% · Testes ~2%

---

## Legenda

| Símbolo | Significado |
|---------|-------------|
| 🔴 Must | Imprescindível — bloqueia MVP ou viola segurança |
| 🟡 Should | Importante — deve estar no MVP, não bloqueia |
| 🟢 Could | Desejável — agrega valor, pode ficar pós-MVP |
| S | Small ≤ 1 dia |
| M | Medium 2–3 dias |
| L | Large 4–5 dias |

---

## Visão Geral dos Epics

| Epic | Título | Stories | Must | Should | Could | Esforço |
|------|--------|---------|------|--------|-------|---------|
| EPIC-01 | Segurança e RBAC | 7 | 5 | 2 | 0 | ~11d |
| EPIC-02 | Ordens de Serviço | 5 | 5 | 0 | 0 | ~12d |
| EPIC-03 | Cadastros Pendentes | 7 | 1 | 5 | 1 | ~15d |
| EPIC-04 | Financeiro | 5 | 0 | 5 | 0 | ~10d |
| EPIC-05 | Testes e Qualidade | 7 | 0 | 5 | 2 | ~14d |
| EPIC-06 | Dashboard e Avançados | 5 | 0 | 1 | 4 | ~10d |
| **Total** | | **36** | **11** | **18** | **7** | **~72d** |

---

# EPIC-01 — Segurança e RBAC

> 56 de 62 controllers sem `[Authorize]` (OWASP A01+A02). Bloqueante para qualquer deploy.

## FEAT-01.1 — Autorização Global

### US-001 — Aplicar `[Authorize]` globalmente via FallbackPolicy

**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 1

**Critério de aceite:**
- `Program.cs` configura `FallbackPolicy = RequireAuthenticatedUser`
- Todos os 56 controllers de negócio retornam 401 sem token JWT válido
- Endpoints públicos (`/auth/login`) decorados com `[AllowAnonymous]`

**Tasks:**
- [x] T-001.1 — Adicionar em `Program.cs`: `options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()`
- [x] T-001.2 — Adicionar `[AllowAnonymous]` em `AuthController.Login`
- [x] T-001.3 — Testar com curl que 401 é retornado sem token em `ClientesController`

---

### US-002 — Remover chave JWT hardcoded

**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 1

**Critério de aceite:**
- Nenhum segredo JWT presente em `appsettings.json` versionado
- Aplicação lança exceção clara em startup se variável não configurada (fail-fast)
- `README.md` atualizado com instruções

**Tasks:**
- [ ] T-002.1 — Substituir fallback hardcoded por leitura de configuração sem fallback
- [ ] T-002.2 — Adicionar validação: `if (string.IsNullOrEmpty(jwtKey)) throw new InvalidOperationException(...)`
- [ ] T-002.3 — `appsettings.json` com placeholder vazio `"SecretKey": ""`
- [ ] T-002.4 — Documentar variáveis de ambiente no `README.md`

---

### US-003 — Implementar Refresh Token

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 6

**Critério de aceite:**
- Endpoint `POST /api/v1/Auth/refresh` aceita refresh token e retorna novo access token
- Logout invalida o refresh token
- Frontend: interceptor para auto-refresh em respostas 401

**Tasks:**
- [ ] T-003.1 — Criar entidade `RefreshToken` e migration
- [ ] T-003.2 — `AuthService.LoginAsync` gera e persiste refresh token
- [ ] T-003.3 — Endpoint `POST /api/v1/Auth/refresh`
- [ ] T-003.4 — Endpoint `POST /api/v1/Auth/logout` (revogação)
- [ ] T-003.5 — Frontend: atualizar `ErrorInterceptor` para chamar `/auth/refresh` em 401

---

### US-004 — RBAC por permissão nos controllers de negócio

**Prioridade:** 🟡 Should | **Estimativa:** L | **Sprint:** 6

**Critério de aceite:**
- Endpoints destrutivos (DELETE, PUT) restringidos por role
- Usuário com role insuficiente recebe 403
- Frontend: oculta botões de ação para roles sem permissão

**Tasks:**
- [ ] T-004.1 — Criar matriz de permissões por role
- [ ] T-004.2 — Aplicar `[Authorize(Roles = "...")]` nos endpoints sensíveis
- [ ] T-004.3 — Frontend: `RoleGuard` verifica role antes de exibir botões destrutivos

---

## FEAT-01.2 — Integridade de Domínio

### US-005 — Reativar Soft Delete em BaseEntity

**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 1

**Critério de aceite:**
- `BaseEntity` possui `bool IsDeleted` e `DateTime? DeletedAt`
- `OficinaContext` configura `HasQueryFilter(e => !e.IsDeleted)` em todas as entidades
- DELETE na API executa soft delete
- Migration criada e reversível

**Tasks:**
- [ ] T-005.1 — Adicionar `bool IsDeleted = false` e `DateTime? DeletedAt` em `BaseEntity`
- [ ] T-005.2 — Configurar query filter global no `OnModelCreating`
- [ ] T-005.3 — Criar método `SoftDelete()` nos repositórios base
- [ ] T-005.4 — Criar migration `AddSoftDeleteToBaseEntity`

---

### US-006 — Criar enum OrdemServicoStatus

**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 1

**Critério de aceite:**
- Enum `OrdemServicoStatus`: `Aberta=1`, `EmAndamento=2`, `AguardandoPeca=3`, `Concluida=4`, `Cancelada=5`
- Entidade `OrdemServico` usa o enum (substituindo `string Status`)
- DTOs refletem o enum
- Migration criada

**Tasks:**
- [ ] T-006.1 — Criar `OrdemServicoStatus.cs` em `OficinaMotos.Domain/Enums/`
- [ ] T-006.2 — Atualizar propriedade `Status` em `OrdemServico.cs`
- [ ] T-006.3 — Configurar `HasConversion<string>()` no DbContext
- [ ] T-006.4 — Criar migration `AddOrdemServicoStatusEnum`
- [ ] T-006.5 — Atualizar DTOs de OrdemServico

---

### US-007 — Adicionar VeiculoId à entidade OrdemServico

**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 1

**Critério de aceite:**
- `OrdemServico` possui `Guid VeiculoId` (FK para `cad_veiculos`)
- `CreateOrdemServicoDto` exige `VeiculoId`
- Migration criada e reversível

**Tasks:**
- [ ] T-007.1 — Adicionar `Guid VeiculoId` e `Veiculo? Veiculo` em `OrdemServico.cs`
- [ ] T-007.2 — Configurar FK no `OnModelCreating`
- [ ] T-007.3 — Criar migration `AddVeiculoIdToOrdemServico`
- [ ] T-007.4 — Atualizar `CreateOrdemServicoDto` com `[Required] Guid VeiculoId`

---

# EPIC-02 — Ordens de Serviço

> Core do negócio. Backend 75%. Frontend 20% — sem formulário de criação nem detalhe real.

## FEAT-02.1 — Listagem de OS

### US-008 — Renomear OsDetalhe → OsLista e corrigir rota

**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 2

**Critério de aceite:**
- Componente nomeado `OsListaComponent`
- Rota `/ordens` exibe a lista com paginação
- Botão "Nova OS" → `/ordens/novo`

**Tasks:**
- [ ] T-008.1 — Renomear pasta `os-detalhe` → `os-lista` e classe para `OsListaComponent`
- [ ] T-008.2 — Atualizar `app.routes.ts`
- [ ] T-008.3 — Adicionar coluna de ações com link para `/ordens/:id`
- [ ] T-008.4 — Adicionar botão "Nova OS" no header

---

## FEAT-02.2 — Cadastro de OS

### US-009 — Criar formulário de nova Ordem de Serviço

**Prioridade:** 🔴 Must | **Estimativa:** M | **Sprint:** 2 | **Depende:** US-007, US-008

**Critério de aceite:**
- Rota `/ordens/novo` exibe formulário de criação
- Campos obrigatórios: cliente (autocomplete), veículo do cliente, descrição, mecânico
- Redirect para `/ordens/:id` após criação

**Tasks:**
- [ ] T-009.1 — Criar `OsCadastroComponent`
- [ ] T-009.2 — Registrar rota `/ordens/novo` antes de `/ordens/:id`
- [ ] T-009.3 — Autocomplete de clientes via `ClientesService.search(term)`
- [ ] T-009.4 — Dropdown dinâmico de veículos filtrado pelo cliente
- [ ] T-009.5 — Dropdown de mecânicos via `MecanicosService.getAll()`
- [ ] T-009.6 — Integrar `OrdensService.create(dto)`

---

## FEAT-02.3 — Detalhe de OS

### US-010 — Criar página de detalhe real da OS

**Prioridade:** 🔴 Must | **Estimativa:** M | **Sprint:** 2 | **Depende:** US-006, US-009

**Critério de aceite:**
- Rota `/ordens/:id` exibe detalhe completo
- Seções: dados gerais, status atual, itens, observações, pagamentos
- Botão de alterar status com transições válidas

**Tasks:**
- [ ] T-010.1 — Criar `OsDetalheComponent`
- [ ] T-010.2 — Registrar rota `/ordens/:id`
- [ ] T-010.3 — Card "Dados Gerais" com cliente e veículo
- [ ] T-010.4 — Seção "Itens da OS"
- [ ] T-010.5 — Controle de status (dropdown de transição)
- [ ] T-010.6 — Seção "Observações"
- [ ] T-010.7 — Seção "Pagamentos"

---

## FEAT-02.4 — Gestão de Itens da OS

### US-011 — Adicionar/remover peças e serviços na OS

**Prioridade:** 🔴 Must | **Estimativa:** M | **Sprint:** 2 | **Depende:** US-010

**Critério de aceite:**
- Modal "Adicionar Peça": busca no estoque, quantidade, valor unitário
- Modal "Adicionar Serviço": descrição livre, valor de mão de obra
- Total da OS recalculado após cada alteração

**Tasks:**
- [ ] T-011.1 — Criar `OsItemPecaModalComponent` com autocomplete de estoque
- [ ] T-011.2 — Criar `OsItemServicoModalComponent`
- [ ] T-011.3 — Integrar `POST /api/v1/OrdemServicoItens`
- [ ] T-011.4 — Integrar `DELETE /api/v1/OrdemServicoItens/{itemId}`
- [ ] T-011.5 — Calcular e exibir totais sem reload completo

---

## FEAT-02.5 — Pagamento de OS

### US-012 — Registrar pagamento de Ordem de Serviço

**Prioridade:** 🔴 Must | **Estimativa:** M | **Sprint:** 2 | **Depende:** US-010

**Critério de aceite:**
- Modal com: valor pago, forma de pagamento, data, observação
- OS atualizada para `Concluida` após pagamento integral
- Lançamento automático em `ContasReceber`

**Tasks:**
- [ ] T-012.1 — Criar `OsPagamentoModalComponent`
- [ ] T-012.2 — Integrar `POST /api/v1/OrdemServicoPagamentos`
- [ ] T-012.3 — Backend: `OrdemServicoService.RegistrarPagamentoAsync` atualiza status
- [ ] T-012.4 — Backend: gerar lançamento em `ContasReceber`

---

# EPIC-03 — Cadastros Pendentes

> Veículos, Mecânicos, Fornecedores e Estoque têm apenas listagem no frontend. Backend ~85%.

### US-013 — Criar formulário de cadastro de veículo

**Prioridade:** 🔴 Must | **Estimativa:** M | **Sprint:** 3 | **Depende:** US-001

**Critério de aceite:**
- Rota `/motos/novo` exibe formulário
- Campos: placa (validação Mercosul), marca, modelo, ano, cor, chassis, KM, proprietário
- Vínculo obrigatório com cliente existente

**Tasks:**
- [ ] T-013.1 — Criar `VeiculoCadastroComponent`
- [ ] T-013.2 — Registrar rota `/motos/novo`
- [ ] T-013.3 — Validator de placa em `shared/validators/`
- [ ] T-013.4 — Autocomplete de marcas e modelos em cascata
- [ ] T-013.5 — Autocomplete de cliente proprietário

---

### US-014 — Adicionar campo `proximo_km_revisao` ao veículo

**Prioridade:** 🟡 Should | **Estimativa:** S | **Sprint:** 3 | **Depende:** US-013

**Tasks:**
- [ ] T-014.1 — Adicionar `int? ProximoKmRevisao` em `Veiculo.cs`
- [ ] T-014.2 — Criar migration `AddProximoKmRevisaoToVeiculo`
- [ ] T-014.3 — Incluir campo no formulário

---

### US-015 — Criar formulário de cadastro de mecânico

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 3 | **Depende:** US-001

**Tasks:**
- [ ] T-015.1 — Criar `MecanicoCadastroComponent`
- [ ] T-015.2 — Registrar rota `/mecanicos/novo`
- [ ] T-015.3 — Validator de CPF único
- [ ] T-015.4 — Multi-select de especialidades

---

### US-016 — Registrar rota `/mecanicos/:id`

**Prioridade:** 🟡 Should | **Estimativa:** S | **Sprint:** 3 | **Depende:** US-001

**Tasks:**
- [ ] T-016.1 — Adicionar `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` em `app.routes.ts`
- [ ] T-016.2 — Adicionar coluna de link em `MecanicoListaComponent`

---

### US-017 — Criar formulário de cadastro de fornecedor

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 3 | **Depende:** US-001

**Tasks:**
- [ ] T-017.1 — Criar `FornecedorCadastroComponent`
- [ ] T-017.2 — Registrar rota `/fornecedores/novo`
- [ ] T-017.3 — Validator de CNPJ e integração `CepService`
- [ ] T-017.4 — Dropdown de segmentos

---

### US-018 — Criar formulário de cadastro de peça no estoque

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 3 | **Depende:** US-001

**Tasks:**
- [ ] T-018.1 — Criar `EstoquePecaCadastroComponent`
- [ ] T-018.2 — Registrar rota `/estoque/novo`
- [ ] T-018.3 — Dropdowns de fabricante, categoria, fornecedor
- [ ] T-018.4 — Badge de estoque crítico na lista quando `qtd < qtd_minima`

---

### US-019 — Registrar movimentações de entrada e saída de estoque

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 3 | **Depende:** US-018

**Tasks:**
- [ ] T-019.1 — Criar `EstoqueEntradaModalComponent`
- [ ] T-019.2 — Criar `EstoqueSaidaModalComponent`
- [ ] T-019.3 — Integrar `POST /api/v1/EstoqueMovimentacoes`
- [ ] T-019.4 — Criar `EstoquePecaDetalheComponent` com histórico
- [ ] T-019.5 — Registrar rota `/estoque/:id`

---

# EPIC-04 — Financeiro

> Backend 80% completo. Frontend 20% — apenas dashboard com dados mockados.

### US-020 — Listar contas a pagar

**Prioridade:** 🟡 Should | **Estimativa:** S | **Sprint:** 4 | **Depende:** US-001

**Tasks:**
- [ ] T-020.1 — Criar `ContasPagarListaComponent`
- [ ] T-020.2 — Registrar rota `/financeiro/contas-pagar`
- [ ] T-020.3 — Filtros por status e período
- [ ] T-020.4 — Destaque visual para contas vencidas

---

### US-021 — Criar e editar conta a pagar

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 4 | **Depende:** US-020

**Tasks:**
- [ ] T-021.1 — Criar `ContasPagarFormComponent`
- [ ] T-021.2 — Registrar rotas de criação e edição
- [ ] T-021.3 — Integrar endpoints `ContasPagar`

---

### US-022 — Listar contas a receber

**Prioridade:** 🟡 Should | **Estimativa:** S | **Sprint:** 4 | **Depende:** US-001

**Tasks:**
- [ ] T-022.1 — Criar `ContasReceberListaComponent`
- [ ] T-022.2 — Registrar rota `/financeiro/contas-receber`
- [ ] T-022.3 — Coluna "OS Origem" com link para `/ordens/:id`

---

### US-023 — Criar e editar conta a receber manual

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 4 | **Depende:** US-022

**Tasks:**
- [ ] T-023.1 — Criar `ContasReceberFormComponent`
- [ ] T-023.2 — Registrar rotas de criação e edição

---

### US-024 — Registrar baixa de conta

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 4 | **Depende:** US-020, US-022

**Critério de aceite:**
- Modal com: data pagamento, valor pago, forma de pagamento
- Suporte a pagamento parcial (saldo residual)

**Tasks:**
- [ ] T-024.1 — Criar `BaixaPagamentoModalComponent`
- [ ] T-024.2 — Integrar `POST /api/v1/ContasPagarPagamentos`
- [ ] T-024.3 — Integrar `POST /api/v1/ContasReceberPagamentos`

---

# EPIC-05 — Testes e Qualidade

> Backend: 0 projetos de teste. Frontend: 2 specs triviais (~1%).

### US-025 — Criar projeto xUnit OficinaMotos.Tests

**Prioridade:** 🟡 Should | **Estimativa:** S | **Sprint:** 5

**Tasks:**
- [ ] T-025.1 — `dotnet new xunit -n OficinaMotos.Tests` em `src/`
- [ ] T-025.2 — Adicionar ao `OficinaMotos.slnx`
- [ ] T-025.3 — Instalar: Moq, FluentAssertions, InMemory, Mvc.Testing
- [ ] T-025.4 — Criar `Helpers/TestDbContextFactory.cs`

---

### US-026 — Testes unitários de AuthService

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 5 | **Depende:** US-025

**Tasks:**
- [ ] T-026.1 — `Login_ValidCredentials_ReturnsJwtToken`
- [ ] T-026.2 — `Login_InvalidPassword_ReturnsNull`
- [ ] T-026.3 — `Login_BlockedAccount_ThrowsException`
- [ ] T-026.4 — `GenerateToken_ContainsExpectedClaims`

---

### US-027 — Testes unitários de ClienteService

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 5 | **Depende:** US-025, US-005

**Tasks:**
- [ ] T-027.1 — `Create_PF_WithValidCpf_Succeeds`
- [ ] T-027.2 — `Create_DuplicateCpf_ThrowsDomainException`
- [ ] T-027.3 — `Delete_SetsIsDeletedTrue_NotPhysicalDelete`
- [ ] T-027.4 — `GetById_DeletedClient_ReturnsNull`

---

### US-028 — Testes unitários de OrdemServicoService

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 5 | **Depende:** US-025, US-006, US-007

**Tasks:**
- [ ] T-028.1 — `Create_WithoutVeiculoId_ThrowsValidationException`
- [ ] T-028.2 — `ChangeStatus_ValidTransition_Succeeds`
- [ ] T-028.3 — `ChangeStatus_InvalidTransition_ThrowsException`
- [ ] T-028.4 — `CalculateTotal_WithPecasAndServicos_IsCorrect`

---

### US-029 — Testes de integração para controllers críticos

**Prioridade:** 🟢 Could | **Estimativa:** L | **Sprint:** 5 | **Depende:** US-025, US-001

**Tasks:**
- [ ] T-029.1 — Criar `OficinaMotosWebApplicationFactory`
- [ ] T-029.2 — `AuthControllerIntegrationTests`: login válido, login inválido
- [ ] T-029.3 — `ClienteControllerIntegrationTests`: 401 sem token, CRUD autenticado

---

### US-030 — Specs para services Angular críticos

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 5

**Tasks:**
- [ ] T-030.1 — `auth.service.spec.ts`: login(), logout(), isAuthenticated(), signal
- [ ] T-030.2 — `clientes.service.spec.ts`: CRUD completo
- [ ] T-030.3 — `ordens.service.spec.ts`: getAll(), create(), changeStatus()

---

### US-031 — Specs para guards e interceptors Angular

**Prioridade:** 🟡 Should | **Estimativa:** S | **Sprint:** 5 | **Depende:** US-030

**Tasks:**
- [ ] T-031.1 — `auth.guard.spec.ts`: usuário não autenticado é redirecionado
- [ ] T-031.2 — `auth.interceptor.spec.ts`: token injetado no header correto
- [ ] T-031.3 — `error.interceptor.spec.ts`: 401 chama logout()

---

# EPIC-06 — Dashboard Real e Funcionalidades Avançadas

### US-032 — Criar endpoint agregador de KPIs

**Prioridade:** 🟢 Could | **Estimativa:** M | **Sprint:** 6 | **Depende:** US-001, US-006

**Tasks:**
- [ ] T-032.1 — Criar `DashboardController` com `[Authorize]`
- [ ] T-032.2 — Criar `DashboardService.GetKpisAsync(periodo)`
- [ ] T-032.3 — Query OS abertas e do dia
- [ ] T-032.4 — Query faturamento mensal
- [ ] T-032.5 — Query peças com `qtd_atual < qtd_minima`

---

### US-033 — Substituir dados mockados do dashboard por dados reais

**Prioridade:** 🟢 Could | **Estimativa:** M | **Sprint:** 6 | **Depende:** US-032

**Tasks:**
- [ ] T-033.1 — `DashboardPrincipalComponent` consome `GET /api/v1/Dashboard/kpis`
- [ ] T-033.2 — Gráfico de OS por status
- [ ] T-033.3 — Gráfico de faturamento mensal (6 meses)
- [ ] T-033.4 — Skeleton loading nos cards de KPI

---

### US-034 — Alerta de estoque mínimo no dashboard

**Prioridade:** 🟢 Could | **Estimativa:** S | **Sprint:** 6 | **Depende:** US-032

**Tasks:**
- [ ] T-034.1 — Incluir `pecasCriticas` no response de `/Dashboard/kpis`
- [ ] T-034.2 — Card "Alertas de Estoque" com link para `/estoque/:id`

---

### US-035 — Gerar relatório PDF de Ordem de Serviço

**Prioridade:** 🟢 Could | **Estimativa:** L | **Sprint:** 6 | **Depende:** US-010

**Tasks:**
- [ ] T-035.1 — Instalar `QuestPDF` no projeto API
- [ ] T-035.2 — Criar `PdfService.GerarOrdemServicoPdf(Guid id)`
- [ ] T-035.3 — Endpoint `GET /api/v1/OrdemServicos/{id}/pdf`
- [ ] T-035.4 — Botão "Imprimir OS" no `OsDetalheComponent`

---

### US-036 — Interface de gerenciamento de usuários e perfis

**Prioridade:** 🟡 Should | **Estimativa:** M | **Sprint:** 6 | **Depende:** US-001, US-004

**Tasks:**
- [ ] T-036.1 — Criar `UsuarioListaComponent` e `UsuarioFormComponent`
- [ ] T-036.2 — Criar `PerfilListaComponent`
- [ ] T-036.3 — Registrar rotas `/seguranca/usuarios` e `/seguranca/perfis`
- [ ] T-036.4 — `RoleGuard` restringindo acesso a role `Admin`

---

# Resumo Executivo do Backlog

| Epic | Stories | Must | Should | Could | Esforço Estimado |
|------|---------|------|--------|-------|-----------------|
| EPIC-01 — Segurança e RBAC | 7 | 5 | 2 | 0 | ~11 dias |
| EPIC-02 — Ordens de Serviço | 5 | 5 | 0 | 0 | ~12 dias |
| EPIC-03 — Cadastros Pendentes | 7 | 1 | 5 | 1 | ~15 dias |
| EPIC-04 — Financeiro | 5 | 0 | 5 | 0 | ~10 dias |
| EPIC-05 — Testes e Qualidade | 7 | 0 | 5 | 2 | ~14 dias |
| EPIC-06 — Dashboard e Avançados | 5 | 0 | 1 | 4 | ~10 dias |
| **Total** | **36** | **11** | **18** | **7** | **~72 dias** |

### Distribuição de Estimativas

| Tamanho | Qtd Stories |
|---------|-------------|
| S (menos ou igual a 1 dia) | 12 |
| M (2–3 dias) | 19 |
| L (4–5 dias) | 5 |
