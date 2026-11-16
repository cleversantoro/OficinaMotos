# Plano de Implementação: Criar e Editar Conta a Pagar (US-021)

**Branch**: `021-contas-pagar-form` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/021-contas-pagar-form/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)
- [`governance/GEMINI.md`](file:///c:/Projetos/OficinaMotos/governance/GEMINI.md)

**Entrada**: US-021 — Criar e editar conta a pagar
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 4 | **Depende**: US-020
- **Tasks**:
  - [ ] T-021.1 — Criar `ContasPagarFormComponent`
  - [ ] T-021.2 — Registrar rotas de criação e edição
  - [ ] T-021.3 — Integrar endpoints `ContasPagar`

---

## Resumo

Esta funcionalidade implementa o fluxo completo de criação e edição de obrigações financeiras (Contas a Pagar) no módulo Financeiro (`EPIC-04`), conectando-se diretamente à listagem entregue na `US-020`.

O plano técnico contempla:
1. **Integração dos Endpoints `ContasPagar` no `FinanceiroService` (`T-021.3`)**:
   - Adição dos métodos `getContaPagarById(id)`, `createContaPagar(payload)` e `updateContaPagar(id, payload)` em [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts), consumindo `apiPaths.financeiro.pagar` (`/api/v1/FinanceiroContasPagar`) via `ApiClientService` (`GET /{id}`, `POST`, `PUT /{id}`).
2. **Componente Unificado `ContasPagarFormComponent` (`T-021.1`)**:
   - Componente Angular 21 Standalone (`ChangeDetectionStrategy.OnPush`) em `src/app/features/financeiro/pages/contas-pagar-form/`, baseado em `ReactiveFormsModule` e Angular Signals (`loading`, `submitting`, `error`, `isEditMode`, `contaId`, `fornecedores`, `metodos`).
   - Suporte aos modos de **criação** (sem parâmetro `:id`) e **edição** (com parâmetro `:id` carregado via `getContaPagarById`), validando `descricao` (obrigatória, máx. 240), `valor` (obrigatório, `>= 0.01`), `vencimento` (obrigatório), `status` (obrigatório: `PENDENTE`, `VENCIDO`, `PAGO`, `CANCELADO`) e `observacao` (opcional, máx. 240), além de seleção opcional de fornecedor e método de pagamento.
   - Feedback visual via serviço `Toast` (`shared/services/toast.ts`) e redirecionamento para `/financeiro/contas-pagar` após salvar.
3. **Registro de Rotas de Criação e Edição e Navegação na Listagem (`T-021.2`)**:
   - Registro das rotas protegidas `{ path: 'financeiro/contas-pagar/novo', component: ContasPagarFormComponent }` e `{ path: 'financeiro/contas-pagar/:id/editar', component: ContasPagarFormComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).
   - Adição do botão **"+ Nova Conta"** no cabeçalho e da coluna de ação **"Editar"** na tabela de [`ContasPagarListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.html).
4. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de schema ou migration necessária (a tabela `fin_contas_pagar`, os DTOs, os validadores FluentValidation e o controller `FinanceiroContasPagarController` já existem e suportam integralmente `GET /{id}`, `POST` e `PUT /{id}`).

---

## Contexto Técnico

**Language/Version**: TypeScript 5.9, Angular 21.0.0 (Standalone Components, Signals, ReactiveFormsModule, Control Flow `@if`/`@for`)  
**Primary Dependencies**: `@angular/common`, `@angular/forms`, `@angular/router`, `rxjs` 7.8.0  
**Storage**: API REST ASP.NET Core 8 + MySQL 8.0 (`fin_contas_pagar`, `fin_metodos_pagamento`, `cad_fornecedores` via `/api/v1/FinanceiroContasPagar`, `/api/v1/FinanceiroMetodosPagamento`, `/api/v1/Fornecedores`)  
**Testing**: Vitest (`npm test -- --no-watch`) + build de produção (`npm run build`)  
**Target Platform**: Aplicação Web SPA (Desktop e Mobile responsivo em Dark Theme)  
**Project Type**: Web application (`oficina-motos-web` + `oficina-motos-api`)  
**Performance Goals**: Validação instantânea no cliente e carregamento paralelo de dados auxiliares via `forkJoin`  
**Constraints**: Sem criação de `NgModule`; uso obrigatório de `apiPaths` (`ADR-006`); reutilização de `Toast` em `shared/services/toast.ts`  
**Scale/Scope**: 1 novo componente de formulário (`ContasPagarFormComponent`), 3 métodos em `FinanceiroService`, 2 rotas em `app.routes.ts`, atalhos de criação/edição em `ContasPagarListaComponent` e testes unitários `.spec.ts`

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Respeita o Bounded Context `Financeiro`, realizando apenas leitura de referência de `Fornecedores` para vinculação de `fornecedorId`. |
| **II. API RESTful Versionada (`ADR-006`)** | ✅ Pass | Consome exclusivamente `/api/v1/FinanceiroContasPagar` via constante centralizada `apiPaths.financeiro.pagar` em `api-paths.ts`. |
| **III. Segurança por Design (RBAC)** | ✅ Pass | Rotas protegidas por `authGuard` no `MainLayout`; endpoint `PUT` protegido por `[Authorize(Roles = "Administrador,Gerente,Financeiro")]` e visibilidade de ações alinhada ao perfil. |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `ContasPagarFormComponent` é `standalone: true`, com `ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule`, Angular Signals e reutilização do serviço `Toast`. |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Sem alterações no schema MySQL 8.0 (`fin_contas_pagar`); validações frontend espelham `CreateFinanceiroContaPagarValidator` e `UpdateFinanceiroContaPagarValidator`. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Inclui testes unitários dedicados para `ContasPagarFormComponent` (modos criação e edição, validações e erros) e `FinanceiroService` (`getContaPagarById`, `createContaPagar`, `updateContaPagar`). |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Alinhado com `governance/backlog.md` (`US-021`), `governance/arquitetura.md` e `specs/020-contas-pagar-lista/`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/021-contas-pagar-form/
├── spec.md              # Especificação de requisitos (US-021)
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
├── app.routes.ts                                                                      # [MODIFICAR] Registrar rotas financeiro/contas-pagar/novo e financeiro/contas-pagar/:id/editar (T-021.2)
├── core/
│   └── services/
│       ├── financeiro.service.ts                                                      # [MODIFICAR] Adicionar getContaPagarById, createContaPagar e updateContaPagar (T-021.3)
│       └── financeiro.service.spec.ts                                                 # [CRIAR] Testes unitários dos métodos ContasPagar no FinanceiroService (T-021.3)
└── features/
    └── financeiro/
        └── pages/
            ├── contas-pagar-lista/
            │   ├── contas-pagar-lista.html                                            # [MODIFICAR] Adicionar botão "+ Nova Conta" e coluna/link "Editar"
            │   ├── contas-pagar-lista.scss                                            # [MODIFICAR] Estilizar botão de cabeçalho e ação de edição na tabela
            │   └── contas-pagar-lista.spec.ts                                         # [MODIFICAR] Validar links de criação e edição na listagem
            └── contas-pagar-form/
                ├── contas-pagar-form.ts                                               # [CRIAR] ContasPagarFormComponent unificado para criação e edição (T-021.1, T-021.3)
                ├── contas-pagar-form.html                                             # [CRIAR] Template reativo com validações e estados de loading/erro (T-021.1)
                ├── contas-pagar-form.scss                                             # [CRIAR] Estilos Dark Theme consistentes com ContasPagarListaComponent (T-021.1)
                └── contas-pagar-form.spec.ts                                          # [CRIAR] Testes unitários cobrindo criação, edição, validações e tratamento de erro
```

**Structure Decision**: Mantém a organização por feature (`src/app/features/financeiro/pages/contas-pagar-form/`) e centraliza as chamadas HTTP em `src/app/core/services/financeiro.service.ts`, conforme definido na Constituição e em `governance/arquitetura.md`.

---

## Fases de Execução

### Fase 1: Integração de Endpoints no `FinanceiroService` (`T-021.3`)
1. Atualizar [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts) adicionando:
   - `getContaPagarById(id: number | string)`
   - `createContaPagar(payload: CreateFinanceiroContaPagarRequest)`
   - `updateContaPagar(id: number | string, payload: UpdateFinanceiroContaPagarRequest)`
2. Criar `financeiro.service.spec.ts` validando as chamadas HTTP para `/api/v1/FinanceiroContasPagar`.

### Fase 2: Componente `ContasPagarFormComponent` (`T-021.1`)
1. Criar `ContasPagarFormComponent` em `src/app/features/financeiro/pages/contas-pagar-form/` (`contas-pagar-form.ts`, `.html`, `.scss`).
2. Implementar `FormGroup` reativo com validações (`descricao`, `fornecedorId`, `valor`, `vencimento`, `status`, `metodoId`, `dataPagamento`, `observacao`), detecção automática de modo criação vs edição via `ActivatedRoute`, carregamento de fornecedores e métodos de pagamento via `forkJoin`, submissão para `createContaPagar` ou `updateContaPagar`, feedback via `Toast` e navegação para `/financeiro/contas-pagar`.

### Fase 3: Registro de Rotas e Atalhos na Listagem (`T-021.2`)
1. Registrar `{ path: 'financeiro/contas-pagar/novo', component: ContasPagarFormComponent }` e `{ path: 'financeiro/contas-pagar/:id/editar', component: ContasPagarFormComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).
2. Atualizar [`ContasPagarListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.html) com o botão **"+ Nova Conta"** no cabeçalho e link **"Editar"** nas linhas da tabela.

### Fase 4: Testes Unitários, Build e Governança
1. Criar `contas-pagar-form.spec.ts` e atualizar `contas-pagar-lista.spec.ts`.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
