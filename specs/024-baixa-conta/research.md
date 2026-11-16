# Pesquisa Técnica: Registrar Baixa de Conta (US-024)

**Branch**: `024-baixa-conta` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/024-baixa-conta/spec.md)

## 1. Análise dos Endpoints de Baixa (`T-024.2` e `T-024.3`)

### 1.1 Requisitos do Backlog
- `T-024.2 — Integrar POST /api/v1/ContasPagarPagamentos`
- `T-024.3 — Integrar POST /api/v1/ContasReceberPagamentos`
- Critério de aceite:
  - Modal com: data pagamento, valor pago, forma de pagamento
  - Suporte a pagamento parcial (saldo residual)

### 1.2 Arquitetura Backend para `/api/v1/ContasPagarPagamentos` e `/api/v1/ContasReceberPagamentos`
- **Situação atual na API (`oficina-motos-api`)**:
  - Os controllers existentes no módulo Financeiro são `FinanceiroContasPagarController`, `FinanceiroContasReceberController` e `FinanceiroPagamentosController`.
  - As tabelas MySQL existentes são `fin_contas_pagar`, `fin_contas_receber`, `fin_pagamentos` e `fin_metodos_pagamento`.
- **Decision**:
  - Criar os controllers dedicados `ContasPagarPagamentosController` (`[Route("api/v1/[controller]")]` → `/api/v1/ContasPagarPagamentos`) e `ContasReceberPagamentosController` (`[Route("api/v1/[controller]")]` → `/api/v1/ContasReceberPagamentos`) protegidos por `[Authorize]`.
  - Adicionar os métodos de negócio `RegistrarBaixaAsync(CreateContaPagarPagamentoDTO request)` em `IFinanceiroContaPagarService` / `FinanceiroContaPagarService` e `RegistrarBaixaAsync(CreateContaReceberPagamentoDTO request)` em `IFinanceiroContaReceberService` / `FinanceiroContaReceberService`.
  - Persistir cada baixa na tabela `fin_pagamentos` (`IFinanceiroPagamentoRepository`) e atualizar a respectiva conta (`fin_contas_pagar` ou `fin_contas_receber`):
    - **Baixa Integral (`valorPago == conta.Valor`)**:
      - `SaldoResidual = 0`;
      - Em `FinanceiroContaPagar`: `Status = "PAGO"`, `DataPagamento = request.DataPagamento`, `MetodoId = request.MetodoId`;
      - Em `FinanceiroContaReceber`: `Status = "RECEBIDO"`, `DataRecebimento = request.DataPagamento`, `MetodoId = request.MetodoId`.
    - **Baixa Parcial (`0 < valorPago < conta.Valor`)**:
      - `SaldoResidual = decimal.Round(conta.Valor - request.ValorPago, 2)`;
      - Atualiza `conta.Valor = SaldoResidual` e `conta.MetodoId = request.MetodoId`;
      - Mantém a conta em aberto (`Status` permanece `PENDENTE` ou `VENCIDO`, com `DataPagamento = null` / `DataRecebimento = null`) para que o saldo residual continue contabilizado nos KPIs "Em aberto" e possa receber novas baixas até a liquidação total;
      - Registra a anotação de baixa parcial na `Observacao` (respeitando o limite de 240 caracteres e preservando eventual referência `OS #<id>` em Contas a Receber).
- **Rationale**:
  - Atende exatamente às rotas especificadas em `T-024.2` e `T-024.3` sem necessidade de alterar o schema MySQL 8.0 ou criar migrations EF Core, mantendo compatibilidade total com `ContasPagarListaComponent` (`US-020`) e `ContasReceberListaComponent` (`US-022`).
- **Alternatives considered**:
  - Criar novas tabelas `fin_contas_pagar_pagamentos` e `fin_contas_receber_pagamentos` via migration. Rejeitado porque a tabela `fin_pagamentos` já cumpre o papel de razão de pagamentos no schema canônico (`oficina-motos-docs/oficina_db_sql/financeiro/oficina_db_table_fin_pagamentos.sql`) e a atualização de `Valor` para o `SaldoResidual` resolve o pagamento parcial sem mutação estrutural do banco.

## 2. Arquitetura do Componente `BaixaPagamentoModalComponent` (`T-024.1`)

- **Decision**:
  - Criar `BaixaPagamentoModalComponent` em `src/app/features/financeiro/components/baixa-pagamento-modal/` como componente Angular 21 Standalone (`ChangeDetectionStrategy.OnPush`), utilizando `ReactiveFormsModule` e Angular Signals.
  - Suportar ambos os contextos (`tipo: 'PAGAR' | 'RECEBER'`) por meio de um `@Input() conta: BaixaContaAlvo | null` e `@Input() visible = false`, emitindo `@Output() visibleChange` e `@Output() baixaRegistrada`.
  - Calcular reativamente o `saldoResidual` à medida que o usuário digita `valorPago`, exibindo um card de resumo com:
    - **Saldo Atual da Conta**
    - **Valor a Baixar**
    - **Saldo Residual após a baixa** + badge dinâmico (**"Baixa Integral — Conta será liquidada"** quando `saldoResidual === 0` ou **"Baixa Parcial — Saldo residual permanecerá em aberto"** quando `saldoResidual > 0`).
- **Rationale**:
  - Um único componente modal reutilizável evita duplicação entre Contas a Pagar e Contas a Receber e cumpre estritamente `T-024.1 — Criar BaixaPagamentoModalComponent`.
- **Alternatives considered**:
  - Criar dois modais separados (`BaixaContaPagarModal` e `BaixaContaReceberModal`). Rejeitado porque `T-024.1` define um componente único `BaixaPagamentoModalComponent`.

## 3. Integração nas Listagens (`US-020` e `US-022`)

- **Decision**:
  - Em `ContasPagarListaComponent` (`/financeiro/contas-pagar`) e `ContasReceberListaComponent` (`/financeiro/contas-receber`), exibir o botão **"Baixar"** na coluna de **Ações** ao lado do link **"Editar"** sempre que o status efetivo da conta for `'PENDENTE'` ou `'VENCIDO'`.
  - Ao receber o evento `(baixaRegistrada)` do modal, atualizar o item correspondente no signal `contas` (com a conta retornada pela API), recalculando instantaneamente os KPIs (`A pagar / A receber (em aberto)`, `Vencidas` e `Total pago / recebido`) sem recarregar a página.
- **Rationale**:
  - Proporciona experiência fluida e reativa com Signals conforme o Princípio IV da Constituição do Projeto.
