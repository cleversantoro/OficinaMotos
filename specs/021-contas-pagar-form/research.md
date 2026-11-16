# Pesquisa Técnica: Criar e Editar Conta a Pagar (US-021)

**Branch**: `021-contas-pagar-form` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/021-contas-pagar-form/spec.md)

## 1. Análise dos Endpoints e Contratos Backend (`T-021.3`)

### 1.1 Controller e Permissões (`FinanceiroContasPagarController.cs`)
- **Controller**: [`FinanceiroContasPagarController.cs`](file:///c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.API/Controllers/Financeiro/FinanceiroContasPagarController.cs)
- **Rota base**: `api/v1/FinanceiroContasPagar` (mapeada em `apiPaths.financeiro.pagar` em [`api-paths.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/api-paths.ts))
- **Operações disponíveis**:
  - `GET /api/v1/FinanceiroContasPagar/{id}` → `[Authorize]` → retorna `FinanceiroContaPagarResponseDTO` (`200 OK`) ou `404 NotFound`.
  - `POST /api/v1/FinanceiroContasPagar` → `[Authorize]` → recebe `CreateFinanceiroContaPagarDTO`, retorna `FinanceiroContaPagarResponseDTO` (`201 Created`).
  - `PUT /api/v1/FinanceiroContasPagar/{id}` → `[Authorize(Roles = "Administrador,Gerente,Financeiro")]` → recebe `UpdateFinanceiroContaPagarDTO`, retorna `FinanceiroContaPagarResponseDTO` (`200 OK`) ou `404 NotFound`.

### 1.2 Validações Backend (`CreateFinanceiroContaPagarValidator` / `UpdateFinanceiroContaPagarValidator`)
- `Descricao`: `NotEmpty()`, `MaximumLength(240)`
- `Status`: `NotEmpty()`, `MaximumLength(50)`
- `Observacao`: `MaximumLength(240)`
- `Valor`: `decimal` (no domínio financeiro e UI validado como numérico obrigatório `>= 0.01`)
- `Vencimento`: `DateTime` obrigatório

### 1.3 Integração no `FinanceiroService` (`T-021.3`)
- **Situação atual**: [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts) possui apenas `contasPagar(params?)` (listagem `GET`). As interfaces `CreateFinanceiroContaPagarRequest` e `UpdateFinanceiroContaPagarRequest` já existem em [`src/app/core/models/financeiro.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/financeiro.ts) e são exportadas pelo barrel `src/app/core/models/index.ts`.
- **Decision**: Adicionar ao `FinanceiroService` os métodos tipados:
  - `getContaPagarById(id: number | string)` → `this.api.getById<FinanceiroContaPagar>(apiPaths.financeiro.pagar, id)`
  - `createContaPagar(payload: CreateFinanceiroContaPagarRequest)` → `this.api.create<FinanceiroContaPagar, CreateFinanceiroContaPagarRequest>(apiPaths.financeiro.pagar, payload)`
  - `updateContaPagar(id: number | string, payload: UpdateFinanceiroContaPagarRequest)` → `this.api.update<FinanceiroContaPagar, UpdateFinanceiroContaPagarRequest>(apiPaths.financeiro.pagar, id, payload)`
- **Rationale**: Reutiliza integralmente os métodos genéricos `getById`, `create` e `update` de `ApiClientService` e a constante centralizada `apiPaths.financeiro.pagar`, respeitando `ADR-006`.
- **Alternatives considered**: Criar um serviço separado `ContasPagarService`. Rejeitado porque `FinanceiroService` já centraliza os recursos do contexto Financeiro e a especificação da arquitetura prevê um serviço por módulo de domínio.

## 2. Arquitetura do Componente `ContasPagarFormComponent` (`T-021.1`)

- **Decision**: Criar um único componente standalone `ContasPagarFormComponent` em `src/app/features/financeiro/pages/contas-pagar-form/` que opera tanto em modo de criação (`/financeiro/contas-pagar/novo`) quanto em modo de edição (`/financeiro/contas-pagar/:id/editar`).
- **Detecção de modo**:
  - Ler `this.route.snapshot.paramMap.get('id')`.
  - Se ausente → `isEditMode = false` (modo criação).
  - Se presente → validar se é um inteiro positivo (`Number.isInteger(num) && num > 0`). Caso inválido, definir `error` de ID inválido sem chamar a API; caso válido, definir `contaId = num`, `isEditMode = true` e buscar o registro via `financeiroService.getContaPagarById(num)`.
- **Carregamento resiliente de dados**:
  - Fornecedores (`FornecedoresService.list()`) e Métodos de Pagamento (`FinanceiroService.metodos()`) são carregados em paralelo com `catchError(() => of([]))`, garantindo que uma falha em listas auxiliares não bloqueie a criação ou edição da conta a pagar.
- **Rationale**: A tarefa `T-021.1` solicita explicitamente `ContasPagarFormComponent` unificado para as rotas de criação e edição (`T-021.2`), evitando duplicação de template, estilos e regras de validação.
- **Alternatives considered**: Dois componentes separados (`ContasPagarCadastroComponent` e `ContasPagarEditarComponent`). Rejeitado porque o backlog especifica explicitamente `T-021.1 — Criar ContasPagarFormComponent`.

## 3. Rotas e Integração com a Listagem (`T-021.2`)

- **Decision**:
  - Registrar em [`src/app/app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts) dentro de `MainLayout` (`canActivate: [authGuard]`):
    - `{ path: 'financeiro/contas-pagar/novo', component: ContasPagarFormComponent }`
    - `{ path: 'financeiro/contas-pagar/:id/editar', component: ContasPagarFormComponent }`
  - Atualizar [`ContasPagarListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.html) para incluir:
    - Botão primário **"+ Nova Conta a Pagar"** (`routerLink="/financeiro/contas-pagar/novo"`) no cabeçalho da página.
    - Coluna **Ações** na tabela com link **"Editar"** (`[routerLink]="['/financeiro/contas-pagar', conta.id, 'editar']"`) para cada conta listada.
- **Rationale**: Conecta diretamente a listagem construída na `US-020` aos fluxos de criação e edição da `US-021`, permitindo navegação completa sem digitação manual de URL.
