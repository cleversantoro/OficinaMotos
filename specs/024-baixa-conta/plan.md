# Plano de Implementação: Registrar Baixa de Conta (US-024)

**Branch**: `024-baixa-conta` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/024-baixa-conta/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

**Entrada**: US-024 — Registrar baixa de conta
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 4 | **Depende**: US-020, US-022
- **Critério de aceite**:
  - Modal com: data pagamento, valor pago, forma de pagamento
  - Suporte a pagamento parcial (saldo residual)
- **Tasks**:
  - [ ] T-024.1 — Criar `BaixaPagamentoModalComponent`
  - [ ] T-024.2 — Integrar `POST /api/v1/ContasPagarPagamentos`
  - [ ] T-024.3 — Integrar `POST /api/v1/ContasReceberPagamentos`

---

## Resumo

Esta funcionalidade entrega o fluxo de registro de baixa (liquidação integral ou amortização parcial com cálculo de saldo residual) para **Contas a Pagar** (`US-020`) e **Contas a Receber** (`US-022`) no módulo Financeiro (`EPIC-04`).

O plano técnico contempla:
1. **Endpoints Backend `POST /api/v1/ContasPagarPagamentos` (`T-024.2`) e `POST /api/v1/ContasReceberPagamentos` (`T-024.3`)**:
   - Criação dos DTOs `CreateContaPagarPagamentoDTO`, `ContaPagarPagamentoResponseDTO`, `CreateContaReceberPagamentoDTO` e `ContaReceberPagamentoResponseDTO`, e respectivos validadores FluentValidation (`CreateContaPagarPagamentoValidator`, `CreateContaReceberPagamentoValidator`).
   - Implementação de `RegistrarBaixaAsync` em `FinanceiroContaPagarService` e `FinanceiroContaReceberService`, registrando o pagamento em `fin_pagamentos` (`IFinanceiroPagamentoRepository`) e atualizando a conta:
     - **Baixa Integral (`valorPago == conta.Valor`)**: define `Status = "PAGO"` (Contas a Pagar) ou `Status = "RECEBIDO"` (Contas a Receber), preenche `DataPagamento` / `DataRecebimento` e `MetodoId`, e retorna `SaldoResidual = 0`.
     - **Baixa Parcial (`0 < valorPago < conta.Valor`)**: calcula `SaldoResidual = conta.Valor - valorPago`, atualiza o `Valor` em aberto da conta para o `SaldoResidual` mantendo o status em aberto (`PENDENTE` / `VENCIDO`), registra a observação da amortização parcial e retorna `SaldoResidual > 0`.
   - Exposição dos controllers autenticados `ContasPagarPagamentosController` (`/api/v1/ContasPagarPagamentos`) e `ContasReceberPagamentosController` (`/api/v1/ContasReceberPagamentos`).
2. **Integração Frontend no `FinanceiroService` e `api-paths.ts` (`T-024.2`, `T-024.3`)**:
   - Adição de `pagarPagamentos: '/api/v1/ContasPagarPagamentos'` e `receberPagamentos: '/api/v1/ContasReceberPagamentos'` em [`api-paths.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/api-paths.ts).
   - Adição das interfaces tipadas em [`financeiro.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/financeiro.ts) e dos métodos `registrarBaixaContaPagar(payload)` e `registrarBaixaContaReceber(payload)` em [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts).
3. **Componente `BaixaPagamentoModalComponent` e Integração nas Listagens (`T-024.1`)**:
   - Criação do componente standalone `BaixaPagamentoModalComponent` em `src/app/features/financeiro/components/baixa-pagamento-modal/` com `ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule` e Angular Signals.
   - Campos obrigatórios: **Data do Pagamento** (`dataPagamento`), **Valor Pago** (`valorPago`) e **Forma de Pagamento** (`metodoId`), mais **Observação** opcional (`observacao`), com exibição reativa do **Saldo Residual** (`saldoAtual - valorPago`) e badge indicando **Baixa Integral** ou **Pagamento Parcial**.
   - Adição do botão **"Baixar"** nas tabelas de [`ContasPagarListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.ts) e [`ContasReceberListaComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.ts) para contas com status efetivo `PENDENTE` ou `VENCIDO`, atualizando reativamente a lista e os KPIs após cada baixa.

---

## Contexto Técnico

**Language/Version**: C# 12 / .NET 8 (Backend API) + TypeScript 5.9 / Angular 21.0.0 (Frontend SPA)  
**Primary Dependencies**: ASP.NET Core Web API, AutoMapper, FluentValidation, EF Core 8 (Backend); `@angular/common`, `@angular/forms`, `rxjs` 7.8.0 (Frontend)  
**Storage**: MySQL 8.0 — tabelas existentes `fin_contas_pagar`, `fin_contas_receber`, `fin_pagamentos` e `fin_metodos_pagamento` (nenhuma migration ou alteração de schema necessária)  
**Testing**: Vitest (`npm test -- --no-watch`), `npm run build` e `dotnet build`  
**Target Platform**: Aplicação Web SPA (Desktop e Mobile responsivo em Dark Theme) + REST API v1  
**Project Type**: Web application (`oficina-motos-web` + `oficina-motos-api`)  
**Performance Goals**: Cálculo instantâneo do saldo residual via Angular Signals e atualização em memória da listagem sem reload de página  
**Constraints**: Sem criação de `NgModule`; uso obrigatório de `apiPaths` (`ADR-006`); reutilização de `Toast` em `shared/services/toast.ts`; sem novas tabelas SQL  
**Scale/Scope**: 1 novo componente modal (`BaixaPagamentoModalComponent`), 2 métodos em `FinanceiroService`, integração em 2 componentes de listagem (`ContasPagarListaComponent`, `ContasReceberListaComponent`), 2 endpoints `POST` na API e testes unitários `.spec.ts`

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Toda a regra de baixa integral/parcial e geração de registro em `fin_pagamentos` permanece encapsulada no Bounded Context `Financeiro`. |
| **II. API RESTful Versionada (`ADR-006`)** | ✅ Pass | Endpoints `/api/v1/ContasPagarPagamentos` e `/api/v1/ContasReceberPagamentos` seguem o versionamento `/api/v1/` e são registrados centralmente em `api-paths.ts`. |
| **III. Segurança por Design (RBAC)** | ✅ Pass | Controllers decorados com `[Authorize]` (além da `FallbackPolicy` global) e rotas frontend protegidas por `authGuard`. |
| **IV. Frontend Reativo com Componentes Standalone** | ✅ Pass | `BaixaPagamentoModalComponent` é `standalone: true`, com `ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule`, Angular Signals e uso de `Toast`. |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Opera sobre as tabelas existentes `fin_contas_pagar`, `fin_contas_receber` e `fin_pagamentos` sem necessidade de alterar o schema MySQL 8.0. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Inclui specs unitárias para `BaixaPagamentoModalComponent`, `FinanceiroService`, `ContasPagarListaComponent` e `ContasReceberListaComponent`. |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Estritamente alinhado com `governance/backlog.md` (`US-024`, `T-024.1`, `T-024.2`, `T-024.3`). |

---

## Project Structure

### Documentation (this feature)

```text
specs/024-baixa-conta/
├── spec.md              # Especificação de requisitos (US-024)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Pesquisa técnica de endpoints, saldo residual e modal
├── data-model.md        # Fase 1: Modelo de dados, DTOs, interfaces e transições de status
├── quickstart.md        # Fase 1: Guia rápido de compilação, testes e verificação funcional
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    ├── api.md           # Fase 1: Contratos REST (POST /api/v1/ContasPagarPagamentos e ContasReceberPagamentos)
    └── ui.md            # Fase 1: Contratos de UI do BaixaPagamentoModalComponent e listagens
```

### Source Code (repository root)

```text
oficina-motos-api/src/
├── OficinaMotos.Application/
│   ├── DTOs/
│   │   ├── Requests/Financeiro/
│   │   │   ├── CreateContaPagarPagamentoDTO.cs                                        # [CRIAR] Request DTO de baixa de conta a pagar (T-024.2)
│   │   │   └── CreateContaReceberPagamentoDTO.cs                                      # [CRIAR] Request DTO de baixa de conta a receber (T-024.3)
│   │   └── Responses/Financeiro/
│   │       ├── ContaPagarPagamentoResponseDTO.cs                                      # [CRIAR] Response DTO com saldoResidual e conta atualizada (T-024.2)
│   │       └── ContaReceberPagamentoResponseDTO.cs                                    # [CRIAR] Response DTO com saldoResidual e conta atualizada (T-024.3)
│   ├── Interfaces/Financeiro/
│   │   ├── IFinanceiroContaPagarService.cs                                            # [MODIFICAR] Adicionar RegistrarBaixaAsync (T-024.2)
│   │   └── IFinanceiroContaReceberService.cs                                          # [MODIFICAR] Adicionar RegistrarBaixaAsync (T-024.3)
│   ├── Services/Financeiro/
│   │   ├── FinanceiroContaPagarService.cs                                             # [MODIFICAR] Implementar baixa integral/parcial e registro em fin_pagamentos (T-024.2)
│   │   └── FinanceiroContaReceberService.cs                                           # [MODIFICAR] Implementar baixa integral/parcial e registro em fin_pagamentos (T-024.3)
│   └── Validators/Financeiro/
│       ├── CreateContaPagarPagamentoValidator.cs                                      # [CRIAR] Validação FluentValidation (T-024.2)
│       └── CreateContaReceberPagamentoValidator.cs                                    # [CRIAR] Validação FluentValidation (T-024.3)
└── OficinaMotos.API/
    └── Controllers/Financeiro/
        ├── ContasPagarPagamentosController.cs                                         # [CRIAR] POST /api/v1/ContasPagarPagamentos (T-024.2)
        └── ContasReceberPagamentosController.cs                                       # [CRIAR] POST /api/v1/ContasReceberPagamentos (T-024.3)

oficina-motos-web/src/app/
├── core/
│   ├── models/
│   │   └── financeiro.ts                                                              # [MODIFICAR] Adicionar interfaces de baixa e saldoResidual (T-024.1, T-024.2, T-024.3)
│   └── services/
│       ├── api-paths.ts                                                               # [MODIFICAR] Adicionar pagarPagamentos e receberPagamentos (T-024.2, T-024.3)
│       ├── financeiro.service.ts                                                      # [MODIFICAR] Adicionar registrarBaixaContaPagar e registrarBaixaContaReceber (T-024.2, T-024.3)
│       └── financeiro.service.spec.ts                                                 # [MODIFICAR] Testes unitários dos novos métodos no FinanceiroService
└── features/
    └── financeiro/
        ├── components/
        │   └── baixa-pagamento-modal/
        │       ├── baixa-pagamento-modal.ts                                           # [CRIAR] BaixaPagamentoModalComponent (T-024.1)
        │       ├── baixa-pagamento-modal.html                                         # [CRIAR] Template do modal com cálculo de saldo residual (T-024.1)
        │       ├── baixa-pagamento-modal.scss                                         # [CRIAR] Estilos Dark Theme do modal (T-024.1)
        │       └── baixa-pagamento-modal.spec.ts                                      # [CRIAR] Testes unitários do modal (integral, parcial, validações)
        └── pages/
            ├── contas-pagar-lista/
            │   ├── contas-pagar-lista.ts                                              # [MODIFICAR] Controle de abertura do modal e atualização pós-baixa (T-024.2)
            │   ├── contas-pagar-lista.html                                            # [MODIFICAR] Botão "Baixar" e instância de <app-baixa-pagamento-modal> (T-024.2)
            │   ├── contas-pagar-lista.scss                                            # [MODIFICAR] Estilo do botão "Baixar"
            │   └── contas-pagar-lista.spec.ts                                         # [MODIFICAR] Testes de acionamento e atualização de baixa
            └── contas-receber-lista/
                ├── contas-receber-lista.ts                                            # [MODIFICAR] Controle de abertura do modal e atualização pós-baixa (T-024.3)
                ├── contas-receber-lista.html                                          # [MODIFICAR] Botão "Baixar" e instância de <app-baixa-pagamento-modal> (T-024.3)
                ├── contas-receber-lista.scss                                          # [MODIFICAR] Estilo do botão "Baixar"
                └── contas-receber-lista.spec.ts                                       # [MODIFICAR] Testes de acionamento e atualização de baixa
```

**Structure Decision**: Segue a arquitetura Clean Architecture na API (`OficinaMotos.Application` + `OficinaMotos.API`) e a organização modular de features no Angular (`src/app/features/financeiro/components/baixa-pagamento-modal/`), mantendo `apiPaths` e `FinanceiroService` centralizados em `src/app/core/services/`.

---

## Fases de Execução

### Fase 1: Backend — Endpoints `ContasPagarPagamentos` e `ContasReceberPagamentos` (`T-024.2`, `T-024.3`)
1. Criar DTOs de requisição/resposta e validadores FluentValidation para baixa de Contas a Pagar e Contas a Receber.
2. Implementar `RegistrarBaixaAsync` em `FinanceiroContaPagarService` e `FinanceiroContaReceberService` com suporte a baixa integral (`status = PAGO/RECEBIDO`, `saldoResidual = 0`) e baixa parcial (`valor = saldoResidual`, conta mantida em aberto + registro em `fin_pagamentos`).
3. Criar `ContasPagarPagamentosController` (`POST /api/v1/ContasPagarPagamentos`) e `ContasReceberPagamentosController` (`POST /api/v1/ContasReceberPagamentos`).

### Fase 2: Frontend Core — `api-paths.ts`, Models e `FinanceiroService` (`T-024.2`, `T-024.3`)
1. Registrar `pagarPagamentos` e `receberPagamentos` em `api-paths.ts`.
2. Adicionar interfaces de request/response e `BaixaContaAlvo` em `src/app/core/models/financeiro.ts`.
3. Implementar `registrarBaixaContaPagar` e `registrarBaixaContaReceber` em `FinanceiroService` e cobrir com testes em `financeiro.service.spec.ts`.

### Fase 3: Componente `BaixaPagamentoModalComponent` e Integração nas Listagens (`T-024.1`, `T-024.2`, `T-024.3`)
1. Criar `BaixaPagamentoModalComponent` (`ts`, `html`, `scss`, `spec.ts`) em `src/app/features/financeiro/components/baixa-pagamento-modal/`.
2. Integrar o botão **"Baixar"** e o modal em `ContasPagarListaComponent` (`/financeiro/contas-pagar`) e `ContasReceberListaComponent` (`/financeiro/contas-receber`), atualizando os signals e KPIs após cada baixa.

### Fase 4: Validação de Build, Testes e Governança
1. Executar `dotnet build` em `oficina-motos-api`.
2. Executar `npm test -- --no-watch` e `npm run build` em `oficina-motos-web`.
3. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
