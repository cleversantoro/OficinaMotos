# Auditoria Técnica — Oficina MotoPro

**Data da Auditoria:** 2026-07-18  
**Auditor:** GitHub Copilot (Oficina — Auditoria)  
**Revisão da Documentação Base:** `oficina-motos-docs/markdown/RESUMO_EXECUTIVO.md` (08/04/2026)  

---

# 1 Estado Geral

**Projeto: ~53% concluído**

| Camada | Completude | Observação |
|--------|-----------|------------|
| Backend API (.NET 9) | ~72% | CRUD implementado; autorização ausente na maioria dos endpoints |
| Frontend Angular 21 | ~48% | Infraestrutura shared completa; vários módulos só têm lista |
| Banco de Dados SQL | ~85% | Schema bem definido; lacunas em campos de KM de revisão |
| Testes | ~2% | 2 specs triviais no frontend; 0 projetos de teste no backend |
| **Geral** | **~53%** | |

---

# 2 Funcionalidades Concluídas

### Backend — Implementado e funcional

**Módulo Auth**
- `POST /api/v1/Auth/login` — JWT com BCrypt, bloqueio de conta, audit log de falhas

**Módulo Clientes** (11 controllers, 11 services, 11 repositories)
- CRUD completo: `ClientesController`, `ClientePfsController`, `ClientePjsController`
- Sub-entidades: Endereços, Contatos, Documentos, Anexos, Indicações, Financeiro, LGPD, Origens
- Endpoint especial `GET /api/v1/Clientes/table` para tabelas paginadas
- Validação de CPF duplicado no `ClienteService.CreateAsync`

**Módulo Veículos** (3 controllers, 3 services)
- CRUD: `VeiculosController`, `VeiculoMarcasController`, `VeiculoModelosController`

**Módulo Mecânicos** (9 controllers)
- CRUD: Mecânicos + Certificações, Contatos, Disponibilidades, Documentos, Endereços, Especialidades, Especialidades-Rel, Experiências

**Módulo Fornecedores** (10 controllers)
- CRUD: Fornecedores + Avaliações, Bancos, Certificações, Contatos, Documentos, Endereços, Representantes, Segmentos, Segmentos-Rel

**Módulo Ordens de Serviço** (8 controllers)
- CRUD: OrdemServicos + Anexos, Avaliações, Checklists, Históricos, Itens, Observações, Pagamentos

**Módulo Estoque** (8 controllers)
- CRUD: EstoquePecas + Categorias, Fabricantes, Localizações, Movimentações, Anexos, Fornecedores, Históricos

**Módulo Financeiro** (7 controllers)
- CRUD: ContasPagar, ContasReceber, Pagamentos, Lançamentos, Históricos, Anexos, MetodosPagamento

**Módulo Segurança** (5 controllers, `[Authorize]` aplicado)
- CRUD: Usuários, Perfis, Permissões, Módulos, AuditLog
- RBAC completo: 6 perfis, 46 permissões, modelo `seg_` bem estruturado

**Infrastructure**
- `OficinaContext` com todos os `DbSet<>` mapeados
- Repositórios em todas as pastas (ClienteRepo, EstoqueRepo, FinanceiroRepo, FornecedorRepo, MecanicoRepo, OrdemServicoRepo, SegurancaRepo, VeiculoRepo)
- Migration inicial criada (`20251214160118_InitialCreate`)

### Frontend — Implementado e funcional

**Infraestrutura Shared (100% dos itens do PASSOS_IMPLEMENTACAO.md Fase 1)**
- `Toast` service + componente UI
- `Confirmation` service (ConfirmDialog)
- `Loading` service + spinner
- `Cep` service (integração ViaCEP)
- Validators: CPF, CNPJ, email, telefone, CEP (`shared/validators/`)
- `FileUpload` componente
- `DataTable` componente reutilizável
- `PlaceholderPage` componente

**Auth**
- Login page funcional com JWT
- `AuthService` com Signals
- `AuthGuard` protegendo rotas
- `AuthInterceptor` injetando Bearer token
- `ErrorInterceptor` registrado

**Clientes (módulo mais completo do frontend)**
- `ClienteLista` — lista com busca
- `ClienteCadastro` — formulário reativo com tabs (perfil, contato, financeiro, legal, anexos)
- `ClienteDetalhe` — visualização completa
- `ClienteEditar` — edição com FormArray para endereços/contatos, delete de sub-entidades, upload de arquivos

**Outros módulos com visualização básica**
- `VeiculoLista`, `VeiculoDetalhe`
- `FornecedorLista`, `FornecedorDetalhe`
- `MecanicoLista`, `MecanicoDetalhe`
- `EstoqueLista`
- `OsDetalhe` (funciona como lista de OS — ver Seção 7.9)
- `FinanceiroDashboard`
- `DashboardPrincipal` com KPIs

**Serviços Core (todos implementados)**
- `ApiClientService`, `ClientesService`, `VeiculosService`, `EstoqueService`, `FornecedoresService`, `MecanicosService`, `OrdensService`, `FinanceiroService`
- `api-paths.ts` centraliza todos os 50+ endpoints

---

# 3 Funcionalidades Parcialmente Implementadas

| Módulo | Backend | Frontend | Lacuna Principal |
|--------|---------|----------|-----------------|
| Clientes | 90% ✅ | 75% 🟡 | Sem busca por CNPJ no create; frontend não exibe subentidades de Indicações e LGPD |
| Veículos | 80% 🟡 | 40% 🔴 | Sem formulário de cadastro; sem vínculo direto com OS; campo KM revisão ausente |
| Mecânicos | 85% 🟡 | 40% 🔴 | Sem formulário de cadastro/edição; rota `/mecanicos/:id` inexistente em `app.routes.ts` |
| Fornecedores | 85% 🟡 | 40% 🔴 | Sem formulário de cadastro; detalhe parcial |
| Ordens de Serviço | 75% 🟡 | 20% 🔴 | Sem VeiculoId na entidade; sem formulário create/edit; componente OsDetalhe faz papel de lista |
| Estoque | 85% 🟡 | 25% 🔴 | Sem formulário create/edit de peça; sem detalhe de peça |
| Financeiro | 80% 🟡 | 20% 🔴 | Sem listas de contas a pagar/receber; sem formulários; dashboard exibe dados mockados |
| Auth/Segurança | 85% 🟡 | 70% 🟡 | `[Authorize]` ausente nos controllers de negócio; sem gestão de usuários no frontend |
| Dashboard | 50% 🔴 | 60% 🟡 | Endpoint `GET /api/v1/PedidosCompra` inexistente; campo `proximo_km_revisao` inexistente |

---

# 4 Funcionalidades Documentadas mas Inexistentes

| Funcionalidade | Referência | Status |
|----------------|-----------|--------|
| `GET /api/v1/PedidosCompra` | `docs/dashboard.txt` | **Não existe** — controller e entidade ausentes |
| Campo `proximo_km_revisao` / `ultimo_km_registrado` em Veículo | `docs/dashboard.txt` | **Não existe** — ausente na entidade `Veiculo.cs` e no schema SQL |
| Gráficos interativos no Dashboard | `RESUMO_EXECUTIVO.md` | Backend sem aggregations; frontend com dados estáticos |
| Relatórios PDF/Excel | `RESUMO_EXECUTIVO.md` | **Não iniciado** |
| Notificações por E-mail | `RESUMO_EXECUTIVO.md` | **Não iniciado** |
| Refresh Token JWT | `RESUMO_EXECUTIVO.md` | **Não iniciado** — token fixo de 8h |
| Rota `/mecanicos/:id` | `ANALISE_PROJETO.md` | Componente existe mas rota **ausente** em `app.routes.ts` |
| Rota `/ordens/novo` e `/ordens/:id` | `ANALISE_PROJETO.md` | **Não existe** |
| Rota `/estoque/novo` e `/estoque/:id` | `ANALISE_PROJETO.md` | **Não existe** |
| Rota `/motos/novo` | `ANALISE_PROJETO.md` | **Não existe** |
| Rota `/fornecedores/novo` | `ANALISE_PROJETO.md` | **Não existe** |
| Rota `/mecanicos/novo` | `ANALISE_PROJETO.md` | **Não existe** |
| RBAC enforcement nos controllers de negócio | `CONSTITUTION.md §III` | **Não implementado** |
| Soft Delete (`IsDeleted`) | `BaseEntity.cs` | **Comentado** — deletes são físicos |
| VeiculoId na Ordem de Serviço | Modelo de domínio esperado | **Ausente** na entidade e no DTO |
| PWA / Responsividade Mobile | `RESUMO_EXECUTIVO.md` | **Não iniciado** |

---

# 5 Funcionalidades Implementadas mas Não Documentadas

| Funcionalidade | Localização | Observação |
|----------------|------------|------------|
| `ClienteEditar` completo com tabs, FormArray e upload | `features/clientes/pages/cliente-editar/` | ANALISE_PROJETO.md ainda lista como "falta" |
| `ClienteDetalhe` completo | `features/clientes/pages/cliente-detalhe/` | ANALISE_PROJETO.md ainda lista como "falta" |
| Todos os validators de shared | `shared/validators/` | Listados como pendentes no PASSOS_IMPLEMENTACAO.md |
| CEP service com ViaCEP | `shared/services/cep.ts` | Listado como pendente |
| FileUpload component | `shared/ui/file-upload/` | Não documentado como concluído |
| DataTable component reutilizável | `shared/ui/data-table/` | Não documentado como concluído |
| Gestão completa de Segurança via API | `Controllers/Seguranca/` | RESUMO_EXECUTIVO.md não menciona estes endpoints |
| `GET /api/v1/Clientes/table` | `ClientesController.cs:43` | Não mencionado na documentação |
| Bloqueio automático de conta por tentativas de login | `AuthService.cs`, `SegUsuario.EstaBloqueado()` | Não documentado |
| Audit log automático de login | `AuthService.cs` | Não documentado |

---

# 6 Divergências

### 6.1 Documentação vs. Código

| Divergência | Doc diz | Código real |
|-------------|---------|-------------|
| Estado de `ClienteEditar` | ANALISE_PROJETO.md: "falta implementar" | Implementado e funcional |
| Estado de `ClienteDetalhe` | ANALISE_PROJETO.md: "falta implementar" | Implementado e funcional |
| RBAC enforcement | CONSTITUTION.md §III: "obrigatório em todos os endpoints exceto /login" | 40+ controllers de negócio sem `[Authorize]` |
| Bounded Contexts | CONSTITUTION.md §I | Único `OficinaContext` com todos os DbSets; `FinanceiroPagamento` referencia `OrdemServico` diretamente |
| `OsDetalhe` como lista | ANALISE_PROJETO.md: página de detalhe | Componente funciona como lista de OS (`ordens: any[] = []`) |
| Campo `proximo_km_revisao` | `docs/dashboard.txt` pressupõe o campo | Ausente em `Veiculo.cs` e nas migrations |
| Endpoint `PedidosCompra` | `docs/dashboard.txt` documenta `GET /api/v1/PedidosCompra` | Não existe nenhum controller ou entidade |
| Mecanico detalhe rota | ANALISE_PROJETO.md: "rota existe mas precisa receber ID" | Rota **completamente ausente** em `app.routes.ts` |

### 6.2 Schema SQL vs. Domain Entities

| Tabela SQL | Entidade .NET | Status |
|-----------|--------------|--------|
| `cad_clientes` | `Cliente.cs` | Alinhados ✅ |
| `cad_mecanicos` | `Mecanico.cs` | Alinhados ✅ |
| `os_ordens_servico` (esperado com `veiculo_id`) | `OrdemServico.cs` (sem `VeiculoId`) | **Divergente** |
| `cad_veiculos` (sem `proximo_km_revisao`) | `Veiculo.cs` (sem campo KM) | Consistentes na ausência, mas dashboard requer o campo |
| `seg_*` (7 tabelas) | `Seguranca.cs` (7 classes) | Alinhados ✅ |

### 6.3 api-paths.ts vs. Controllers Existentes

| Path em api-paths.ts | Controller real | Status |
|---------------------|----------------|--------|
| `/api/v1/OrdemServicos` | `OrdemServicosController` | ✅ |
| `/api/v1/EstoquePecas` | `EstoquePecasController` | ✅ |
| `/api/v1/FinanceiroContasPagar` | `FinanceiroContasPagarController` | ✅ |
| Todos os 50+ paths listados | Controllers correspondentes | ✅ — alinhamento completo |
| `GET /api/v1/PedidosCompra` (dashboard.txt) | — | **Ausente** |

---

# 7 Dívida Técnica

### 7.1 🔴 CRÍTICO — Segurança (OWASP A01: Broken Access Control)

**Problema:** O atributo `[Authorize]` está **ausente** em todos os controllers de negócio. Apenas os 5 controllers do módulo `Seguranca/` têm a anotação. Os ~40 controllers restantes são **publicamente acessíveis** sem autenticação.

| Controller sem `[Authorize]` (amostra) | Arquivo |
|----------------------------------------|---------|
| `ClientesController` — comentado | `Controllers/Cliente/ClientesController.cs:11` |
| `OrdemServicosController` | `Controllers/OrdemServico/OrdemServicosController.cs` |
| `EstoquePecasController` | `Controllers/Estoque/EstoquePecasController.cs` |
| `FinanceiroContasPagarController` | `Controllers/Financeiro/FinanceiroContasPagarController.cs` |

**Impacto:** Qualquer usuário sem autenticação pode ler, criar, atualizar ou deletar clientes, OS, peças, dados financeiros e veículos.

### 7.2 🔴 CRÍTICO — Chave JWT hardcoded (OWASP A02)

**Problema:** Fallback `"chave_super_secreta_padrao_desenvolvimento_123"` no `AuthController.cs:58`.

```csharp
var jwtKey = _configuration["Jwt:Key"] ?? "chave_super_secreta_padrao_desenvolvimento_123";
```

**Impacto:** Se `Jwt:Key` não for configurada em produção, tokens são assinados com uma chave pública e previsível.

### 7.3 🟡 Violações DDD — Bounded Context

**Problema:** A entidade `FinanceiroPagamento.cs` referencia diretamente `OrdemServico`, `Cliente` e `Fornecedor` — cruzando Bounded Contexts.

```csharp
public OrdemServico? OrdemServico { get; set; }    // violação BC
public Cliente? Cliente { get; set; }               // violação BC
public Fornecedor? Fornecedor { get; set; }         // violação BC
```

**Regra violada:** CONSTITUTION.md §I

### 7.4 🟡 Violações DDD — Status como string livre

```csharp
public string Status { get; set; } = "ABERTA";  // OrdemServico.cs:13
```

Sem type-safety; strings arbitrárias podem ser persistidas.

### 7.5 🟡 Soft Delete comentado

```csharp
//public bool IsDeleted { get; private set; }
```

Todos os deletes são físicos e irreversíveis.

### 7.6 🟡 Geração de JWT no Controller (Clean Architecture)

Lógica de criação de token em `AuthController.cs:55-75` (camada API). Deveria estar em `AuthService` na Application Layer.

### 7.7 🟡 Namespace incorreto em OrdemServicoService

O `OrdemServicoService` está no namespace `OficinaMotos.Application.Services.OrdemServicoRepo` (sufixo `Repo`).

### 7.8 🟡 Uso massivo de `any` no Frontend

| Arquivo | Ocorrências de `any` |
|---------|---------------------|
| `clientes.service.ts` | 16+ |
| `estoque.service.ts` | 3+ |
| `ordens.service.ts` | 2+ |
| `veiculos.service.ts` | 2+ |
| `os-detalhe.ts` | `ordens: any[] = []` |

Modelos TypeScript completos existem em `core/models/` mas não são usados nos services.

### 7.9 🟡 Componente `OsDetalhe` com responsabilidade errada

O componente `features/ordens-servico/pages/os-detalhe/os-detalhe.ts` funciona como lista de OS mas está nomeado e roteado como "detalhe" (`/ordens`). Viola Single Responsibility Principle.

### 7.10 🟡 Ausência total de testes

| Camada | Arquivos de teste | Cobertura estimada |
|--------|------------------|--------------------|
| Backend (.NET) | 0 projetos de teste | 0% |
| Frontend (Angular) | 2 spec files triviais | ~2% |

### 7.11 🟢 Entidade `OrdemServico` sem `VeiculoId`

Não é possível vincular uma Ordem de Serviço a um veículo específico.

### 7.12 🟢 Rota `/mecanicos/:id` ausente

Componente `MecanicoDetalhe` existe mas nenhuma rota está registrada em `app.routes.ts`.

---

# 8 Melhorias Sugeridas

### 8.1 Segurança (Prioridade Máxima)

1. Adicionar `[Authorize]` globalmente via `Program.cs` com `AddAuthorization` + política default.
2. Mover `Jwt:Key` para variável de ambiente ou `dotnet user-secrets` — nunca fallback hardcoded.
3. Implementar `[Authorize(Roles = "...")]` por módulo conforme o modelo `seg_perfis`.

### 8.2 Domínio e Arquitetura

4. Criar enum `OrdemServicoStatus` no Domain.
5. Adicionar `VeiculoId` à entidade `OrdemServico` e ao `CreateOrdemServicoDTO`.
6. Adicionar campos `ProximoKmRevisao` e `UltimoKmRegistrado` em `Veiculo`.
7. Reativar Soft Delete em `BaseEntity` e ajustar repositórios.
8. Mover geração de JWT para `AuthService` (Application Layer).
9. Corrigir namespace de `OrdemServicoService`.
10. Introduzir Anti-Corruption Layer entre BC Financeiro e BC OS/Cadastro.

### 8.3 Frontend

11. Renomear `OsDetalhe` para `OsLista` e criar componentes separados `OsCadastro`, `OsDetalhe`, `OsEditar`.
12. Registrar rota `/mecanicos/:id` em `app.routes.ts`.
13. Substituir `any` pelos modelos TypeScript em `core/models/`.
14. Implementar formulários de cadastro: OS, Mecânico, Fornecedor, Estoque, Veículo.
15. Implementar páginas de lista de Contas a Pagar/Receber.

### 8.4 Testes

16. Criar projeto `OficinaMotos.Tests` com testes unitários para services críticos.
17. Criar testes de integração para controllers com banco em memória.
18. Escrever specs Angular para os services de `core/services/` e componentes de `shared/`.

---

# 9 Roadmap de Estabilização

## Sprint 1 — Segurança e Estabilização (1-2 semanas) 🔴

| Tarefa | Prioridade |
|--------|-----------|
| Aplicar `[Authorize]` globalmente ou em todos os controllers de negócio | 🔴 BLOQUEANTE |
| Remover fallback hardcoded da chave JWT | 🔴 BLOQUEANTE |
| Adicionar `VeiculoId` à entidade e DTO de OS | 🔴 |
| Reativar Soft Delete em BaseEntity | 🟡 |
| Criar enum `OrdemServicoStatus` | 🟡 |

## Sprint 2 — Frontend: Formulários Críticos (2-3 semanas) 🟡

| Tarefa |
|--------|
| Renomear `OsDetalhe` → `OsLista` e criar `OsCadastro` |
| Criar `VeiculoCadastro` e rota `/motos/novo` |
| Registrar rota `/mecanicos/:id` em `app.routes.ts` |
| Criar `MecanicoCadastro` e rota `/mecanicos/novo` |
| Criar `FornecedorCadastro` e rota `/fornecedores/novo` |
| Criar `EstoquePecaCadastro` e rota `/estoque/novo` |
| Substituir `any` pelos modelos em `core/models/` |

## Sprint 3 — Financeiro e Dashboard Real (2-3 semanas) 🟡

| Tarefa |
|--------|
| Implementar lista + forms de Contas a Pagar e Contas a Receber |
| Conectar KPIs do `DashboardPrincipal` a endpoints reais da API |
| Adicionar campo `ProximoKmRevisao` em `Veiculo` |
| Criar aggregation endpoints para o dashboard |

## Sprint 4 — Cobertura de Testes (2-3 semanas) 🟡

| Tarefa |
|--------|
| Criar projeto `OficinaMotos.Tests` com testes de ClienteService, AuthService |
| Testes de integração para os controllers mais usados |
| Specs Angular para `ClientesService`, `OrdensService`, `AuthService` |

## Sprint 5 — Funcionalidades Avançadas (4-6 semanas) 🟢

| Tarefa |
|--------|
| Refresh Token |
| RBAC por permissão nos controllers |
| Gestão de Usuários no frontend |
| Relatórios PDF/Excel |
| Responsividade Mobile / PWA |
| Notificações por E-mail |
| Soft Delete completo |
| Anti-Corruption Layer entre BCs |

---

# 10 Resumo de Riscos

| Risco | Severidade | Mitigação |
|-------|-----------|-----------|
| API sem autenticação (~40 controllers) | 🔴 CRÍTICO | Aplicar `[Authorize]` imediatamente |
| Chave JWT hardcoded | 🔴 CRÍTICO | Usar variável de ambiente / secrets |
| OS sem vínculo com Veículo | 🟡 ALTO | Adicionar `VeiculoId` e nova migration |
| 0% de cobertura de testes no backend | 🟡 ALTO | Criar projeto de testes |
| Deletes físicos sem soft delete | 🟡 MÉDIO | Reativar `IsDeleted` |
| Violação de BCs (Financeiro → OS) | 🟡 MÉDIO | Refatorar para ACL |
| Frontend sem formulários de OS e Estoque | 🟡 MÉDIO | Sprints 2-3 |

oficina-motos-web
Frontend Angular 20

oficina-motos-docs
Documentação oficial do projeto.

