# Especificação de Funcionalidade: Registrar Baixa de Conta (US-024)

**Feature Branch**: `024-baixa-conta`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-024 — Registrar baixa de conta | Prioridade: 🟡 Should | Estimativa: M | Sprint: 4 | Depende: US-020, US-022 | Critério de aceite: Modal com: data pagamento, valor pago, forma de pagamento; Suporte a pagamento parcial (saldo residual) | Tasks: [ ] T-024.1 — Criar BaixaPagamentoModalComponent, [ ] T-024.2 — Integrar POST /api/v1/ContasPagarPagamentos, [ ] T-024.3 — Integrar POST /api/v1/ContasReceberPagamentos"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Registrar Baixa Integral ou Parcial em Contas a Pagar via Modal (Priority: P1)

Como gestor, analista financeiro ou administrador da oficina, quero acionar o botão **"Baixar"** em uma conta em aberto (`PENDENTE` ou `VENCIDO`) na listagem `/financeiro/contas-pagar` (`ContasPagarListaComponent`) para abrir o `BaixaPagamentoModalComponent`, informar a **data de pagamento**, o **valor pago** e a **forma de pagamento** (além de observação opcional), visualizando em tempo real o **saldo residual** e registrando a baixa via `POST /api/v1/ContasPagarPagamentos`.

**Why this priority**: Cumpre diretamente `T-024.1` e `T-024.2`, permitindo liquidar obrigações financeiras integralmente ou amortizar pagamentos parciais diretamente na listagem de Contas a Pagar entregue na `US-020`.

**Independent Test**: Acessar `/financeiro/contas-pagar`, localizar uma conta com status `PENDENTE` ou `VENCIDO` (ex.: `valor = R$ 500,00`), clicar em **"Baixar"**, verificar a abertura do `BaixaPagamentoModalComponent` com data atual e valor pré-preenchido (`R$ 500,00`), alterar para `R$ 200,00` (verificando a exibição do saldo residual de `R$ 300,00`), selecionar uma forma de pagamento, confirmar e validar o envio de `POST /api/v1/ContasPagarPagamentos` e a atualização imediata da listagem e dos KPIs.

**Acceptance Scenarios**:

1. **Given** que o usuário está em `/financeiro/contas-pagar` e visualiza uma conta com status efetivo `PENDENTE` ou `VENCIDO`, **When** clica na ação **"Baixar"** da linha correspondente, **Then** o sistema abre o `BaixaPagamentoModalComponent` configurado para conta a pagar (`tipo = 'PAGAR'`), exibindo a descrição e o saldo atual da conta, com `dataPagamento` inicializada na data atual (`yyyy-MM-dd`), `valorPago` inicializado com o saldo total da conta e `metodoId` pré-selecionado com o método atual da conta (ou o primeiro método disponível).
2. **Given** que o usuário mantém `valorPago` igual ao saldo total da conta a pagar (`saldoResidual = R$ 0,00`), preenche `dataPagamento` e seleciona a forma de pagamento (`metodoId`), **When** confirma a baixa no modal, **Then** o sistema envia `POST /api/v1/ContasPagarPagamentos`, a conta a pagar passa para o status `PAGO` com `dataPagamento` e `metodoId` atualizados, o modal é fechado, um toast de sucesso é exibido e a listagem `/financeiro/contas-pagar` reflete a atualização sem recarregar a página.
3. **Given** que o usuário informa um `valorPago` menor que o saldo atual da conta a pagar (`0 < valorPago < saldoAtual`), **When** edita o campo de valor e confirma a baixa, **Then** o modal exibe o indicador de **Pagamento Parcial** com o **Saldo Residual** calculado (`saldoAtual - valorPago`), envia `POST /api/v1/ContasPagarPagamentos`, mantém a conta em aberto (`PENDENTE` ou `VENCIDO`) atualizada com o `saldoResidual` remanescente e atualiza os KPIs da listagem.

---

### User Story 2 - Registrar Baixa Integral ou Parcial em Contas a Receber via Modal (Priority: P1)

Como gestor, analista financeiro ou recepcionista da oficina, quero acionar o botão **"Baixar"** em um recebível em aberto (`PENDENTE` ou `VENCIDO`) na listagem `/financeiro/contas-receber` (`ContasReceberListaComponent`) para abrir o `BaixaPagamentoModalComponent`, informar a **data de pagamento/recebimento**, o **valor pago** e a **forma de pagamento**, acompanhando o **saldo residual** e registrando o recebimento via `POST /api/v1/ContasReceberPagamentos`.

**Why this priority**: Cumpre diretamente `T-024.1` e `T-024.3`, permitindo dar baixa rápida (integral ou parcial) nos recebíveis listados na `US-022` utilizando o mesmo componente modal reutilizável.

**Independent Test**: Acessar `/financeiro/contas-receber`, localizar uma conta com status `PENDENTE` ou `VENCIDO` (ex.: `valor = R$ 600,00`), clicar em **"Baixar"**, testar tanto a baixa parcial (ex.: `R$ 250,00`, restando saldo residual de `R$ 350,00`) quanto a baixa integral do saldo remanescente (`R$ 350,00`), verificando a chamada a `POST /api/v1/ContasReceberPagamentos` e a transição de status para `RECEBIDO` quando o saldo residual chega a zero.

**Acceptance Scenarios**:

1. **Given** que o usuário está em `/financeiro/contas-receber` e visualiza uma conta com status efetivo `PENDENTE` ou `VENCIDO`, **When** clica na ação **"Baixar"** daquela linha, **Then** o sistema abre o `BaixaPagamentoModalComponent` configurado para conta a receber (`tipo = 'RECEBER'`), exibindo a descrição e o saldo atual da conta, com `dataPagamento`, `valorPago` e `metodoId` inicializados.
2. **Given** que o usuário confirma uma baixa integral (`valorPago == saldoAtual`, `saldoResidual == 0`) para a conta a receber, **When** submete o formulário do modal, **Then** o sistema envia `POST /api/v1/ContasReceberPagamentos`, atualiza o status da conta para `RECEBIDO` com `dataRecebimento` e `metodoId` preenchidos, exibe toast de sucesso e atualiza a listagem e os KPIs de Contas a Receber.
3. **Given** que o usuário informa um `valorPago` parcial (`0 < valorPago < saldoAtual`) para a conta a receber, **When** submete o formulário do modal, **Then** o sistema envia `POST /api/v1/ContasReceberPagamentos`, atualiza o valor em aberto da conta para o `saldoResidual` (`saldoAtual - valorPago`) preservando o status em aberto (`PENDENTE` / `VENCIDO`) para futura quitação do restante e exibe feedback de baixa parcial registrada.

---

### User Story 3 - Validação de Formulário, Cálculo de Saldo Residual e Bloqueio em Contas Já Quitadas/Canceladas (Priority: P2)

Como usuário operando as listagens financeiras, quero que o modal valide os campos obrigatórios (`dataPagamento`, `valorPago`, `metodoId`), impeça valores inválidos (menores ou iguais a zero ou superiores ao saldo em aberto da conta) e que o botão **"Baixar"** seja exibido apenas para contas elegíveis (em aberto / vencidas), evitando baixas duplicadas ou inconsistências contábeis.

**Why this priority**: Garante a integridade financeira dos critérios de aceite da `US-024` (campos obrigatórios do modal e precisão matemática do saldo residual).

**Independent Test**: Abrir o `BaixaPagamentoModalComponent` para uma conta de `R$ 200,00`, testar digitação de `R$ 0,00`, `-R$ 10,00` e `R$ 250,00`, bem como remoção da data ou da forma de pagamento, verificando que o envio é bloqueado com mensagens claras; na listagem, verificar que contas com status `PAGO`/`RECEBIDO` ou `CANCELADO` não exibem o botão **"Baixar"**.

**Acceptance Scenarios**:

1. **Given** que o modal de baixa está aberto para uma conta com saldo de `R$ 200,00`, **When** o usuário digita `valorPago = 0`, valor negativo ou `valorPago > 200,00`, ou deixa `dataPagamento` ou `metodoId` vazios, **Then** o formulário fica inválido, exibe a mensagem de erro correspondente e impede a chamada HTTP.
2. **Given** uma conta a pagar com status `PAGO` ou `CANCELADO` (ou uma conta a receber com status `RECEBIDO` ou `CANCELADO`), **When** a linha é renderizada na tabela da listagem, **Then** a ação **"Baixar"** não é exibida (mantendo apenas a ação **"Editar"**).

---

### Edge Cases

- **Precisão decimal no cálculo do saldo residual**: O cálculo `saldoResidual = Math.round((saldoAtual - valorPago) * 100) / 100` (no frontend) e `decimal.Round(conta.Valor - request.ValorPago, 2)` (no backend) deve trabalhar sempre com 2 casas decimais para evitar erros de ponto flutuante (ex.: `100.10 - 33.33 = 66.77`).
- **Múltiplas baixas parciais sequenciais**: Uma conta de `R$ 1.000,00` que sofre uma primeira baixa parcial de `R$ 400,00` passa a ter saldo em aberto de `R$ 600,00` (permanecendo `PENDENTE` ou `VENCIDO`); ao abrir novamente o modal para essa conta, o saldo exibido e sugerido será `R$ 600,00`, permitindo novas baixas parciais ou a baixa integral de `R$ 600,00` que finalmente mudará o status para `PAGO` ou `RECEBIDO`.
- **Contas sem métodos de pagamento pré-cadastrados ou falha ao carregar métodos**: Caso a lista de `FinanceiroMetodoPagamento` vinda da API esteja vazia, o modal deve exibir opções padrão de fallback ou permitir informar a forma de pagamento de forma resiliente, validando a seleção obrigatória.
- **Prevenção de duplo clique (double submit)**: Enquanto a requisição `POST /api/v1/ContasPagarPagamentos` ou `POST /api/v1/ContasReceberPagamentos` estiver em andamento (`submitting() === true`), os botões de confirmação e fechamento devem ficar desabilitados.
- **Preservação de rastreabilidade de OS em Contas a Receber**: Quando uma conta a receber vinculada a uma OS (`OS #123` na descrição/observação) receber baixa parcial ou integral, sua descrição original deve ser preservada e a anotação da baixa deve ser concatenada de forma segura na `observacao` (respeitando o limite de 240 caracteres), mantendo íntegro o link da coluna **"OS Origem"** (`US-022`).

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE disponibilizar o componente standalone reutilizável `BaixaPagamentoModalComponent` em `src/app/features/financeiro/components/baixa-pagamento-modal/` com `ChangeDetectionStrategy.OnPush`, Reactive Forms e Angular Signals (`T-024.1`).
- **FR-002**: O `BaixaPagamentoModalComponent` DEVE exibir e validar os campos obrigatórios definidos no critério de aceite:
  - **Data do pagamento** (`dataPagamento`: obrigatório, formato `yyyy-MM-dd`, padrão data atual);
  - **Valor pago** (`valorPago`: numérico obrigatório, `> 0` e `<= saldoAtual`, inicializado com o valor/saldo atual da conta);
  - **Forma de pagamento** (`metodoId`: seleção obrigatória a partir dos métodos de pagamento carregados);
  - **Observação** (`observacao`: texto opcional, máximo de 240 caracteres).
- **FR-003**: O `BaixaPagamentoModalComponent` DEVE calcular e exibir reativamente o **Saldo Residual** (`saldoResidual = max(0, round(saldoAtual - valorPago, 2))`), indicando visualmente se a operação resultará em **Baixa Integral** (`saldoResidual === 0`) ou **Baixa Parcial** (`saldoResidual > 0`), além de oferecer ação rápida para preencher o valor integral do saldo.
- **FR-004**: O registro centralizado `apiPaths` (`src/app/core/services/api-paths.ts`) DEVE declarar os endpoints `pagarPagamentos: '/api/v1/ContasPagarPagamentos'` e `receberPagamentos: '/api/v1/ContasReceberPagamentos'`, e o `FinanceiroService` (`src/app/core/services/financeiro.service.ts`) DEVE expor os métodos `registrarBaixaContaPagar(payload)` (`POST /api/v1/ContasPagarPagamentos`, `T-024.2`) e `registrarBaixaContaReceber(payload)` (`POST /api/v1/ContasReceberPagamentos`, `T-024.3`).
- **FR-005**: A API backend DEVE disponibilizar os endpoints autenticados `POST /api/v1/ContasPagarPagamentos` (`T-024.2`) e `POST /api/v1/ContasReceberPagamentos` (`T-024.3`), que validam a conta alvo (`contaPagarId` / `contaReceberId`), o `valorPago` (`0 < valorPago <= conta.Valor`), a `dataPagamento` e a forma de pagamento (`metodoId`):
  - Em caso de **baixa integral** (`valorPago == conta.Valor`), atualiza a conta para status `PAGO` (em `fin_contas_pagar`) ou `RECEBIDO` (em `fin_contas_receber`), preenche `dataPagamento` / `dataRecebimento` e `metodoId`, registra o pagamento em `fin_pagamentos` e retorna `saldoResidual = 0`;
  - Em caso de **baixa parcial** (`valorPago < conta.Valor`), calcula `saldoResidual = conta.Valor - valorPago`, atualiza o `Valor` em aberto da conta para `saldoResidual` (mantendo o status em aberto `PENDENTE` ou `VENCIDO`), atualiza `metodoId` e registra a anotação do pagamento parcial e o registro em `fin_pagamentos`, retornando a conta atualizada e o `saldoResidual`.
- **FR-006**: Os componentes `ContasPagarListaComponent` (`/financeiro/contas-pagar`) e `ContasReceberListaComponent` (`/financeiro/contas-receber`) DEVEM exibir o botão **"Baixar"** na coluna de **Ações** para todas as contas com status efetivo `PENDENTE` ou `VENCIDO`, abrindo o `BaixaPagamentoModalComponent` e atualizando a lista e os KPIs imediatamente após a conclusão da baixa.

### Key Entities *(include if feature involves data)*

- **`CreateContaPagarPagamentoRequest` / `ContaPagarPagamentoResponse`** (`src/app/core/models/financeiro.ts` e DTOs correspondentes no backend): Contrato do endpoint `POST /api/v1/ContasPagarPagamentos` contendo `contaPagarId`, `dataPagamento`, `valorPago`, `metodoId`, `observacao` e retornando os dados atualizados da conta, `valorPago`, `saldoResidual` e indicador `baixaIntegral`.
- **`CreateContaReceberPagamentoRequest` / `ContaReceberPagamentoResponse`** (`src/app/core/models/financeiro.ts` e DTOs correspondentes no backend): Contrato do endpoint `POST /api/v1/ContasReceberPagamentos` contendo `contaReceberId`, `dataPagamento`, `valorPago`, `metodoId`, `observacao` e retornando os dados atualizados da conta, `valorPago`, `saldoResidual` e indicador `baixaIntegral`.
- **`FinanceiroContaPagar` (`fin_contas_pagar`), `FinanceiroContaReceber` (`fin_contas_receber`) e `FinanceiroPagamento` (`fin_pagamentos`)**: Entidades de domínio financeiro existentes atualizadas/criadas durante o processamento da baixa.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Em 100% das contas com status efetivo `PENDENTE` ou `VENCIDO` em `/financeiro/contas-pagar` e `/financeiro/contas-receber`, o botão **"Baixar"** abre o `BaixaPagamentoModalComponent` com os campos de data de pagamento, valor pago e forma de pagamento (`T-024.1`).
- **SC-002**: Em 100% das baixas de Contas a Pagar (integrais ou parciais), o sistema executa `POST /api/v1/ContasPagarPagamentos`, atualiza o status para `PAGO` (quando integral) ou reduz o saldo em aberto para o `saldoResidual` (quando parcial) e reflete os novos valores na tela sem reload (`T-024.2`).
- **SC-003**: Em 100% das baixas de Contas a Receber (integrais ou parciais), o sistema executa `POST /api/v1/ContasReceberPagamentos`, atualiza o status para `RECEBIDO` (quando integral) ou reduz o saldo em aberto para o `saldoResidual` (quando parcial) e reflete os novos valores na tela sem reload (`T-024.3`).
- **SC-004**: 100% das tentativas de submissão com `valorPago <= 0`, `valorPago > saldoAtual`, `dataPagamento` vazia ou `metodoId` não selecionado são bloqueadas pela validação do formulário antes do envio HTTP.
- **SC-005**: Todos os testes unitários do frontend (`npm test -- --no-watch`) e a compilação da API e do frontend (`dotnet build` e `npm run build`) executam com zero erros.

## Assumptions

- O banco de dados já possui as tabelas `fin_contas_pagar`, `fin_contas_receber`, `fin_pagamentos` e `fin_metodos_pagamento`, não sendo necessária nenhuma alteração de schema SQL nem criação de migration EF Core para suportar a baixa integral e parcial.
- Na baixa parcial (`valorPago < conta.Valor`), o valor em aberto da conta (`fin_contas_pagar.valor` ou `fin_contas_receber.valor`) passa a representar o `saldoResidual` (`valorAnterior - valorPago`), enquanto o valor efetivamente pago na operação é registrado em `fin_pagamentos` e documentado na observação da conta, permitindo que os KPIs de "Em aberto" reflitam com exatidão o saldo residual pendente e que novas baixas parciais ou a quitação final possam ser realizadas sobre a mesma conta.
- Contas já liquidadas (`PAGO` / `RECEBIDO`) ou canceladas (`CANCELADO`) não podem receber nova baixa pelo modal e são rejeitadas com `400 Bad Request` caso enviadas diretamente aos endpoints da API.
