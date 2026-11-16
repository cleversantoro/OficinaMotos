# Pesquisa Técnica: Criar e Editar Conta a Receber Manual (US-023)

**Branch**: `023-contas-receber-form` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/023-contas-receber-form/spec.md)

## 1. Análise dos Endpoints e Contratos Backend (`T-023.1`)

### 1.1 Controller e Rotas (`FinanceiroContasReceberController.cs`)
- **Controller**: [`FinanceiroContasReceberController.cs`](file:///c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.API/Controllers/Financeiro/FinanceiroContasReceberController.cs)
- **Rota base**: `api/v1/FinanceiroContasReceber` (mapeada em `apiPaths.financeiro.receber` em [`api-paths.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/api-paths.ts))
- **Operações disponíveis**:
  - `GET /api/v1/FinanceiroContasReceber/{id}` → retorna `FinanceiroContaReceberResponseDTO` (`200 OK`) ou `404 NotFound`.
  - `POST /api/v1/FinanceiroContasReceber` → recebe `CreateFinanceiroContaReceberDTO`, retorna `FinanceiroContaReceberResponseDTO` (`201 Created`).
  - `PUT /api/v1/FinanceiroContasReceber/{id}` → recebe `UpdateFinanceiroContaReceberDTO`, retorna `FinanceiroContaReceberResponseDTO` (`200 OK`) ou `404 NotFound`.

### 1.2 Validações Backend (`CreateFinanceiroContaReceberValidator` / `UpdateFinanceiroContaReceberValidator`)
- `Descricao`: `NotEmpty()`, `MaximumLength(240)`
- `Status`: `NotEmpty()`, `MaximumLength(50)`
- `Observacao`: `MaximumLength(240)`
- `Valor`: `decimal` (no domínio financeiro e UI validado como numérico obrigatório `>= 0.01`)
- `Vencimento`: `DateTime` obrigatório

### 1.3 Integração no `FinanceiroService`
- **Situação atual**: [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts) possui apenas `contasReceber(params?)` (listagem `GET`). As interfaces `CreateFinanceiroContaReceberRequest` e `UpdateFinanceiroContaReceberRequest` já existem em [`src/app/core/models/financeiro.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/financeiro.ts) e são exportadas pelo barrel `src/app/core/models/index.ts`.
- **Decision**: Adicionar ao `FinanceiroService` os métodos tipados:
  - `getContaReceberById(id: number | string)` → `this.api.getById<FinanceiroContaReceber>(apiPaths.financeiro.receber, id)`
  - `createContaReceber(payload: CreateFinanceiroContaReceberRequest)` → `this.api.create<FinanceiroContaReceber, CreateFinanceiroContaReceberRequest>(apiPaths.financeiro.receber, payload)`
  - `updateContaReceber(id: number | string, payload: UpdateFinanceiroContaReceberRequest)` → `this.api.update<FinanceiroContaReceber, UpdateFinanceiroContaReceberRequest>(apiPaths.financeiro.receber, id, payload)`
- **Rationale**: Reutiliza integralmente os métodos genéricos `getById`, `create` e `update` de `ApiClientService` e a constante centralizada `apiPaths.financeiro.receber`, respeitando `ADR-006` e mantendo simetria com `ContasPagar` (`US-021`).
- **Alternatives considered**: Criar um serviço separado `ContasReceberService`. Rejeitado porque `FinanceiroService` centraliza os recursos do Bounded Context Financeiro.

## 2. Arquitetura do Componente `ContasReceberFormComponent` (`T-023.1`)

- **Decision**: Criar um único componente standalone `ContasReceberFormComponent` em `src/app/features/financeiro/pages/contas-receber-form/` que opera tanto em modo de criação (`/financeiro/contas-receber/novo`) quanto em modo de edição (`/financeiro/contas-receber/:id/editar`).
- **Detecção de modo**:
  - Ler `this.route.snapshot.paramMap.get('id')`.
  - Se ausente → `isEditMode = false` (modo criação).
  - Se presente → validar se é um inteiro positivo (`Number.isInteger(num) && num > 0`). Caso inválido, definir `error` de ID inválido sem chamar a API; caso válido, definir `contaId = num`, `isEditMode = true` e buscar o registro via `financeiroService.getContaReceberById(num)`.
- **Normalização de Status**:
  - Opções do formulário: `'PENDENTE'`, `'VENCIDO'`, `'RECEBIDO'`, `'CANCELADO'`.
  - Ao carregar uma conta existente (ex.: gerada por pagamento de OS com status `'PAGO'` ou `'LIQUIDADO'`), normalizar `'RECEBIDO' | 'PAGO' | 'LIQUIDADO'` para `'RECEBIDO'`, `'VENCIDO'` para `'VENCIDO'`, `'CANCELADO'` para `'CANCELADO'` e demais (`'PENDENTE'`, `'ABERTO'`) para `'PENDENTE'`.
  - Quando o usuário altera `status` para `'RECEBIDO'` e `dataRecebimento` está vazio, preencher automaticamente `dataRecebimento` com a data atual (`yyyy-MM-dd`).
- **Carregamento resiliente de dados**:
  - Clientes (`ClientesService.list()`) e Métodos de Pagamento (`FinanceiroService.metodos()`) são carregados em paralelo via `forkJoin` com `catchError(() => of([]))`, garantindo que uma falha em listas auxiliares não bloqueie a criação ou edição da conta a receber.
- **Rationale**: A tarefa `T-023.1` solicita explicitamente `ContasReceberFormComponent` unificado para as rotas de criação e edição (`T-023.2`), mantendo paridade visual e técnica com `ContasPagarFormComponent` (`US-021`).
- **Alternatives considered**: Dois componentes separados (`ContasReceberCadastroComponent` e `ContasReceberEditarComponent`). Rejeitado porque o backlog especifica explicitamente `T-023.1 — Criar ContasReceberFormComponent`.

## 3. Rotas e Integração com a Listagem (`T-023.2`)

- **Decision**:
  - Registrar em [`src/app/app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts) dentro de `MainLayout` (`canActivate: [authGuard]`):
    - `{ path: 'financeiro/contas-receber/novo', component: ContasReceberFormComponent }`
    - `{ path: 'financeiro/contas-receber/:id/editar', component: ContasReceberFormComponent }`
  - Atualizar [`ContasReceberListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.html) para incluir:
    - Botão primário **"+ Nova Conta"** (`routerLink="/financeiro/contas-receber/novo"`) no cabeçalho da página.
    - Coluna **Ações** na tabela com link **"Editar"** (`[routerLink]="['/financeiro/contas-receber', conta.id, 'editar']"`) para cada conta listada (ajustando `colspan="9"` na linha vazia).
- **Rationale**: Conecta diretamente a listagem construída na `US-022` aos fluxos de criação e edição da `US-023`, permitindo navegação completa sem digitação manual de URL.
