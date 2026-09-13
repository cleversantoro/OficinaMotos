# Plano de Implementação: Registrar Pagamento de Ordem de Serviço

**Branch**: `012-os-pagamento` | **Data**: 2026-09-09 | **Spec**: [spec.md](spec.md)

**Entrada**: US-012 — Registrar pagamento de Ordem de Serviço
- **Prioridade**: 🔴 Must | **Estimativa**: M | **Sprint**: 2 | **Depende**: US-010
- **Critério de aceite**:
  - Modal com: valor pago, forma de pagamento, data, observação
  - OS atualizada para `Concluida` após pagamento integral
  - Lançamento automático em `ContasReceber`
- **Tasks**:
  - [ ] T-012.1 — Criar `OsPagamentoModalComponent`
  - [ ] T-012.2 — Integrar `POST /api/v1/OrdemServicoPagamentos`
  - [ ] T-012.3 — Backend: `OrdemServicoService.RegistrarPagamentoAsync` atualiza status
  - [ ] T-012.4 — Backend: gerar lançamento em `ContasReceber`

---

## Resumo

A funcionalidade viabiliza o registro de pagamentos diretamente a partir da página de detalhe da OS (`/ordens/:id`) através do componente standalone `OsPagamentoModalComponent`. O formulário permite selecionar a forma de pagamento, data, observações e sugere automaticamente o saldo devedor restante da ordem.

No backend, a rota `POST /api/v1/OrdemServicoPagamentos` aciona o método orquestrador `OrdemServicoService.RegistrarPagamentoAsync`. Sob transação atômica, o serviço:
1. Valida a elegibilidade da OS (não permitindo pagamentos em ordens canceladas);
2. Registra a entidade `OrdemServicoPagamento`;
3. Avalia se o total acumulado pago atinge ou ultrapassa o total de peças e serviços da OS. Em caso afirmativo, transiciona automaticamente o status da OS para `Concluida` e preenche a data de conclusão (`DataConclusao`);
4. Cria automaticamente o lançamento contábil equivalente no módulo financeiro (`FinanceiroContaReceber`), vinculando o cliente, valor, método, vencimento e descrição referenciando a OS de origem.

No frontend, a resposta é refletida reativamente no signal `ordem` do `OsDetalheComponent`, recalculando instantaneamente `totalPago`, `saldoPendente` e o badge de status sem recarregamento da página.

---

## Contexto Técnico

**Linguagem/Versão**:
- Backend: C# 12 / .NET 8 (ASP.NET Core Web API)
- Frontend: TypeScript 5.9.2 com Angular 21

**Dependências Principais**:
- Backend: Entity Framework Core 8, AutoMapper, FluentValidation, MySQL Connector (Pomelo)
- Frontend: Angular Signals (`signal`, `computed`, `effect`), Angular Forms (`FormsModule`), Angular Common (`CurrencyPipe`, `DatePipe`), PrimeNG 21 (`DialogModule`, `ButtonModule`, `InputTextModule`, `InputNumberModule`, `SelectModule`/`DropdownModule`), Serviços centrais (`Toast`), `OrdensService`

**Armazenamento**:
- Banco de dados relacional MySQL 8 (InnoDB, utf8mb4)
- Tabelas envolvidas: `os_pagamentos`, `os_ordens`, `fin_contas_receber`

**Testes**:
- Backend: xUnit para testes unitários do serviço `OrdemServicoService.RegistrarPagamentoAsync`
- Frontend: Vitest com Angular TestBed (`npx ng test --no-watch`) para `OsPagamentoModalComponent` e integração com `OsDetalheComponent`

**Plataforma Alvo**:
- Navegadores modernos (Desktop e Mobile) consumindo API RESTful

**Tipo de Projeto**:
- Web application com Clean Architecture no backend e SPA Standalone Components no frontend

**Metas de Desempenho**:
- Gravação e resposta do pagamento em < 1,5s
- Atualização em tempo real na interface através de reatividade por Signals
- Zero reloads de rota completa (0 reloads)

**Restrições**:
- Execução transacional atômica no banco de dados (`BeginTransactionAsync`);
- Bloqueio estrito de pagamentos em OS com status `Cancelada` ou já quitada;
- Uso obrigatório de endpoints centralizados em `apiPaths` (`apiPaths.ordens.pagamentos`).

---

## Verificação da Constituição

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [x] **I. Domínio Primeiro (DDD)**: A lógica de validação de elegibilidade, cálculo de quitação e transição de status reside na camada de aplicação do domínio de OS (`OrdemServicoService`). A integração com o BC Financeiro ocorre via geração atômica da entidade de recebíveis (`FinanceiroContaReceber`), sem expor lógicas em controllers ou componentes de UI.
- [x] **II. API RESTful Versionada**: Utiliza o endpoint canônico versionado `POST /api/v1/OrdemServicoPagamentos` mapeado em `api-paths.ts` (`apiPaths.ordens.pagamentos`).
- [x] **III. Segurança por Design (RBAC + LGPD)**: Endpoint protegido por JWT e autorização RBAC; formulário valida permissões do usuário e dados de auditoria (`Criado_Em`, `Criado_Por`) são mantidos.
- [x] **IV. Frontend Reativo com Componentes Standalone**: O novo modal `OsPagamentoModalComponent` é standalone (`standalone: true`); o estado do `OsDetalheComponent` é atualizado via Signals (`ordem.update`, `computed`) com detecção `OnPush`.
- [x] **V. Integridade e Rastreabilidade de Dados**: Operação sob transação atômica do Entity Framework; chaves estrangeiras e integridade referencial respeitadas; referência explícita da OS na descrição do lançamento financeiro.
- [x] **VI. Qualidade e Testabilidade**: Cobertura de testes unitários com Vitest para o modal e detalhe no frontend, e testes unitários para a lógica de negócio de quitação no backend.
- [x] **VII. Documentação como Fonte de Verdade**: Alinhado com o backlog (`FEAT-02.5`), inventário de tabelas (`os_pagamentos`, `fin_contas_receber`) e arquitetura documentada em `governance/`.

### Reavaliação Pós-Design
- [x] O schema do banco de dados já suporta as entidades `OrdemServicoPagamento` e `FinanceiroContaReceber` sem necessidade de novas migrations.
- [x] A delegação de `OrdemServicoPagamentoService.CreateAsync` para `OrdemServicoService.RegistrarPagamentoAsync` unifica a regra de negócio sem quebrar contratos existentes de API.
- [x] A experiência do usuário no detalhe da OS flui de forma imediata e transparente.

---

## Estrutura do Projeto

### Documentação desta funcionalidade

```text
specs/012-os-pagamento/
├── spec.md
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── ui.md
└── checklists/
    └── requirements.md
```

### Código-fonte

```text
oficina-motos-api/
├── src/OficinaMotos.API/Controllers/OrdemServico/
│   └── OrdemServicoPagamentosController.cs
├── src/OficinaMotos.Application/
│   ├── DTOs/Requests/OrdemServico/
│   │   └── CreateOrdemServicoPagamentoDTO.cs
│   ├── DTOs/Responses/OrdemServico/
│   │   └── OrdemServicoPagamentoResponseDTO.cs
│   ├── Interfaces/OrdemServico/
│   │   ├── IOrdemServicoService.cs
│   │   └── IOrdemServicoPagamentoService.cs
│   └── Services/OrdemServico/
│       ├── OrdemServicoService.cs
│       └── OrdemServicoPagamentoService.cs

oficina-motos-web/
├── src/app/core/
│   ├── models/
│   │   └── ordem-servico.ts
│   └── services/
│       ├── api-paths.ts
│       └── ordens.service.ts
└── src/app/features/ordens-servico/
    ├── components/
    │   └── os-pagamento-modal/
    │       ├── os-pagamento-modal.ts
    │       ├── os-pagamento-modal.html
    │       ├── os-pagamento-modal.scss
    │       └── os-pagamento-modal.spec.ts
    └── pages/os-detalhe/
        ├── os-detalhe.ts
        ├── os-detalhe.html
        ├── os-detalhe.scss
        └── os-detalhe.spec.ts
```

---

## Rastreamento de Complexidade

Não há violações arquiteturais ou desvios de constituição.
