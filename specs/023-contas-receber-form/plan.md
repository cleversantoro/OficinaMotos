# Plano de Implementação: Criar e Editar Conta a Receber Manual (US-023)

**Branch**: `023-contas-receber-form` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/023-contas-receber-form/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)
- [`governance/GEMINI.md`](file:///c:/Projetos/OficinaMotos/governance/GEMINI.md)

**Entrada**: US-023 — Criar e editar conta a receber manual
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 4 | **Depende**: US-022
- **Tasks**:
  - [ ] T-023.1 — Criar `ContasReceberFormComponent`
  - [ ] T-023.2 — Registrar rotas de criação e edição

---

## Resumo

Esta funcionalidade implementa o fluxo completo de criação e edição manual de direitos de recebimento (Contas a Receber) no módulo Financeiro (`EPIC-04`), conectando-se diretamente à listagem entregue na `US-022`.

O plano técnico contempla:
1. **Integração dos Endpoints `ContasReceber` no `FinanceiroService`**:
   - Adição dos métodos `getContaReceberById(id)`, `createContaReceber(payload)` e `updateContaReceber(id, payload)` em [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts), consumindo `apiPaths.financeiro.receber` (`/api/v1/FinanceiroContasReceber`) via `ApiClientService` (`GET /{id}`, `POST`, `PUT /{id}`).
2. **Componente Unificado `ContasReceberFormComponent` (`T-023.1`)**:
   - Componente Angular 21 Standalone (`ChangeDetectionStrategy.OnPush`) em `src/app/features/financeiro/pages/contas-receber-form/`, baseado em `ReactiveFormsModule` e Angular Signals (`loading`, `submitting`, `error`, `isEditMode`, `contaId`, `clientes`, `metodos`).
   - Suporte aos modos de **criação** (sem parâmetro `:id`) e **edição** (com parâmetro `:id` carregado via `getContaReceberById`), validando `descricao` (obrigatória, máx. 240), `valor` (obrigatório, `>= 0.01`), `vencimento` (obrigatório), `status` (obrigatório: `PENDENTE`, `VENCIDO`, `RECEBIDO`, `CANCELADO`) e `observacao` (opcional, máx. 240), além de seleção opcional de cliente (`ClientesService.list()`) e método de pagamento (`FinanceiroService.metodos()`).
   - Autopreenchimento de `dataRecebimento` com a data atual (`yyyy-MM-dd`) ao selecionar status `RECEBIDO` quando o campo estiver vazio, e normalização de status legados/originados de OS (`PAGO`/`LIQUIDADO` → `RECEBIDO`, `ABERTO` → `PENDENTE`).
   - Feedback visual via serviço `Toast` (`shared/services/toast.ts`) e redirecionamento para `/financeiro/contas-receber` após salvar.
3. **Registro de Rotas de Criação e Edição e Navegação na Listagem (`T-023.2`)**:
   - Registro das rotas protegidas `{ path: 'financeiro/contas-receber/novo', component: ContasReceberFormComponent }` e `{ path: 'financeiro/contas-receber/:id/editar', component: ContasReceberFormComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).
   - Adição do botão **"+ Nova Conta"** no cabeçalho e da coluna de ação **"Editar"** na tabela de [`ContasReceberListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.html).
4. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de schema ou migration necessária (a tabela `fin_contas_receber`, os DTOs, os validadores FluentValidation e o controller `FinanceiroContasReceberController` já existem e suportam integralmente `GET /{id}`, `POST` e `PUT /{id}`).

---

## Contexto Técnico

**Language/Version**: TypeScript 5.9, Angular 21.0.0 (Standalone Components, Signals, ReactiveFormsModule, Control Flow `@if`/`@for`)  
**Primary Dependencies**: `@angular/common`, `@angular/forms`, `@angular/router`, `rxjs` 7.8.0  
**Storage**: API REST ASP.NET Core 8 + MySQL 8.0 (`fin_contas_receber`, `fin_metodos_pagamento`, `cad_clientes` via `/api/v1/FinanceiroContasReceber`, `/api/v1/FinanceiroMetodosPagamento`, `/api/v1/Clientes`)  
**Testing**: Vitest (`npm test -- --no-watch`) + build de produção (`npm run build`)  
**Target Platform**: Aplicação Web SPA (Desktop e Mobile responsivo em Dark Theme)  
**Project Type**: Web application (`oficina-motos-web` + `oficina-motos-api`)  
**Performance Goals**: Validação instantânea no cliente e carregamento paralelo de dados auxiliares via `forkJoin`  
**Constraints**: Sem criação de `NgModule`; uso obrigatório de `apiPaths` (`ADR-006`); reutilização de `Toast` em `shared/services/toast.ts`  
**Scale/Scope**: 1 novo componente de formulário (`ContasReceberFormComponent`), 3 métodos em `FinanceiroService`, 2 rotas em `app.routes.ts`, atalhos de criação/edição em `ContasReceberListaComponent` e testes unitários `.spec.ts`

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Respeita o Bounded Context `Financeiro`, realizando apenas leitura de referência de `Clientes` para vinculação opcional de `clienteId`. |
| **II. API RESTful Versionada (`ADR-006`)** | ✅ Pass | Consome exclusivamente `/api/v1/FinanceiroContasReceber` via constante centralizada `apiPaths.financeiro.receber` em `api-paths.ts`. |
| **III. Segurança por Design (RBAC)** | ✅ Pass | Rotas protegidas por `authGuard` no `MainLayout` e endpoints protegidos por JWT Bearer no backend (`FallbackPolicy`). |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `ContasReceberFormComponent` é `standalone: true`, com `ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule`, Angular Signals e reutilização do serviço `Toast`. |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Sem alterações no schema MySQL 8.0 (`fin_contas_receber`); validações frontend espelham `CreateFinanceiroContaReceberValidator` e `UpdateFinanceiroContaReceberValidator`. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Inclui testes unitários dedicados para `ContasReceberFormComponent` (modos criação e edição, validações e erros), `ContasReceberListaComponent` e `FinanceiroService` (`getContaReceberById`, `createContaReceber`, `updateContaReceber`). |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Alinhado com `governance/backlog.md` (`US-023`), `governance/arquitetura.md` e `specs/022-contas-receber-lista/`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/023-contas-receber-form/
├── spec.md              # Especificação de requisitos (US-023)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Pesquisa técnica de endpoints, validações e arquitetura do form
├── data-model.md        # Fase 1: Modelo de dados, FormControls e mapeamento de payloads
├── quickstart.md        # Fase 1: Guia rápido de execução e validação
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    ├── api.md           # Fase 1: Contratos REST (GET/:id, POST, PUT/:id)
    └── ui.md            # Fase 1: Contratos de UI, rotas e controles do formulário
```

### Source Code (repository root)

```text
oficina-motos-web/src/app/
├── app.routes.ts                                                                      # [MODIFICAR] Registrar rotas financeiro/contas-receber/novo e financeiro/contas-receber/:id/editar (T-023.2)
├── core/
│   └── services/
│       ├── financeiro.service.ts                                                      # [MODIFICAR] Adicionar getContaReceberById, createContaReceber e updateContaReceber (T-023.1)
│       └── financeiro.service.spec.ts                                                 # [MODIFICAR] Testes unitários dos métodos ContasReceber no FinanceiroService (T-023.1)
└── features/
    └── financeiro/
        └── pages/
            ├── contas-receber-lista/
            │   ├── contas-receber-lista.html                                          # [MODIFICAR] Adicionar botão "+ Nova Conta" e coluna/link "Editar"
            │   ├── contas-receber-lista.scss                                          # [MODIFICAR] Estilizar botão de cabeçalho e ação de edição na tabela
            │   └── contas-receber-lista.spec.ts                                       # [MODIFICAR] Validar links de criação e edição na listagem
            └── contas-receber-form/
                ├── contas-receber-form.ts                                             # [CRIAR] ContasReceberFormComponent unificado para criação e edição (T-023.1)
                ├── contas-receber-form.html                                           # [CRIAR] Template reativo com validações e estados de loading/erro (T-023.1)
                ├── contas-receber-form.scss                                           # [CRIAR] Estilos Dark Theme consistentes com ContasReceberListaComponent (T-023.1)
                └── contas-receber-form.spec.ts                                        # [CRIAR] Testes unitários cobrindo criação, edição, validações e tratamento de erro
```

**Structure Decision**: Mantém a organização por feature (`src/app/features/financeiro/pages/contas-receber-form/`) espelhando o padrão de `contas-pagar-form/` (`US-021`) e centraliza as chamadas HTTP em `src/app/core/services/financeiro.service.ts`, conforme definido na Constituição e em `governance/arquitetura.md`.

---

## Fases de Execução

### Fase 1: Integração de Endpoints no `FinanceiroService` (`T-023.1`)
1. Atualizar [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts) adicionando:
   - `getContaReceberById(id: number | string)`
   - `createContaReceber(payload: CreateFinanceiroContaReceberRequest)`
   - `updateContaReceber(id: number | string, payload: UpdateFinanceiroContaReceberRequest)`
2. Atualizar `financeiro.service.spec.ts` validando as chamadas HTTP para `/api/v1/FinanceiroContasReceber`.

### Fase 2: Componente `ContasReceberFormComponent` (`T-023.1`)
1. Criar `ContasReceberFormComponent` em `src/app/features/financeiro/pages/contas-receber-form/` (`contas-receber-form.ts`, `.html`, `.scss`).
2. Implementar `FormGroup` reativo com validações (`descricao`, `clienteId`, `valor`, `vencimento`, `status`, `metodoId`, `dataRecebimento`, `observacao`), detecção automática de modo criação vs edição via `ActivatedRoute`, carregamento de clientes e métodos de pagamento via `forkJoin`, submissão para `createContaReceber` ou `updateContaReceber`, feedback via `Toast` e navegação para `/financeiro/contas-receber`.

### Fase 3: Registro de Rotas e Atalhos na Listagem (`T-023.2`)
1. Registrar `{ path: 'financeiro/contas-receber/novo', component: ContasReceberFormComponent }` e `{ path: 'financeiro/contas-receber/:id/editar', component: ContasReceberFormComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).
2. Atualizar [`ContasReceberListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.html) com o botão **"+ Nova Conta"** no cabeçalho e link **"Editar"** nas linhas da tabela.

### Fase 4: Testes Unitários, Build e Governança
1. Criar `contas-receber-form.spec.ts` e atualizar `contas-receber-lista.spec.ts`.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
