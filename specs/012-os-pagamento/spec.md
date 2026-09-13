# Especificação de Funcionalidade: Registrar Pagamento de Ordem de Serviço

**Feature Branch**: `012-os-pagamento`

**Criado**: 2026-09-09

**Status**: Draft

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

**Dependências**: US-010 — Criar página de detalhe real da OS

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Registro de Pagamento Integral com Conclusão Automática da OS (Prioridade: P1)

Como atendente, recepcionista ou operador de caixa da oficina mecânica visualizando o detalhe da OS (`/ordens/:id`), quero abrir um modal para registrar o pagamento total do saldo devedor da ordem de serviço informando valor pago, forma de pagamento, data e observações opcionais, para que o valor seja quitado e o status da OS seja atualizado automaticamente para `Concluida`, liberando a entrega da motocicleta ao cliente sem necessidade de alteração manual de status.

**Por que esta prioridade**: É o fluxo principal e mais frequente de finalização de atendimento na oficina. Quando o cliente quita o valor devido no balcão, a conclusão da OS deve ser imediata e integrada, reduzindo cliques do operador e prevenindo que ordens pagas permaneçam esquecidas em aberto.

**Teste independente**: Acessar uma OS ativa com saldo devedor (ex.: R$ 350,00), clicar no botão "Registrar Pagamento", confirmar o formulário com o valor total pendente, selecionar uma forma de pagamento (ex.: "PIX"), submeter e verificar que o pagamento é gravado, o total pago passa para R$ 350,00, o saldo pendente zera e o badge de status da OS transiciona imediatamente para `Concluída`.

**Cenários de aceitação**:

1. **Dado** que o usuário está na tela `/ordens/:id` de uma OS ativa com saldo pendente > 0 e status não terminal (`Aberta`, `EmAndamento` ou `AguardandoPeca`), **quando** clicar no botão "Registrar Pagamento" na seção de Pagamentos, **então** o modal `OsPagamentoModalComponent` será aberto.
2. **Dado** que o modal foi aberto, **quando** os campos forem renderizados, **então** o campo "Valor Pago" virá pré-preenchido com o saldo pendente atual da OS e a "Data do Pagamento" virá pré-preenchida com a data do dia atual.
3. **Dado** que o usuário confirma o pagamento no valor igual ao saldo devedor restante (pagamento integral), **quando** o formulário for submetido, **então** o sistema enviará a requisição `POST /api/v1/OrdemServicoPagamentos`, registrará o pagamento com sucesso, atualizará o status da OS no banco para `Concluida` e registrará a data de conclusão.
4. **Dado** que a operação foi concluída com sucesso (HTTP 201 Created), **quando** a resposta for recebida pelo frontend, **então** o modal será fechado, uma notificação Toast de sucesso será exibida ("Pagamento registrado com sucesso — OS Concluída!"), a lista de pagamentos será atualizada no signal da OS e o badge de status mudará para `Concluída` sem recarregar a página.
5. **Dado** que o envio falhou por erro de comunicação ou validação da API, **quando** a falha ocorrer, **então** o modal permanecerá aberto, os dados preenchidos serão preservados e uma notificação amigável de erro via Toast alertará o usuário sobre o problema.

---

### História de Usuário 2 — Registro de Pagamento Parcial ou Adiantamento (Prioridade: P1)

Como operador da oficina mecânica, quero registrar um pagamento parcial de uma OS (como um adiantamento/sinal para compra de peças ou pagamento fracionado), informando um montante inferior ao saldo total, para que o valor seja abatido do saldo devedor mantendo a OS em andamento operacional até sua quitação definitiva.

**Por que esta prioridade**: Muitas oficinas exigem adiantamento de 30% a 50% antes de iniciar reparos ou encomendar peças caras. O sistema deve amortizar pagamentos intermediários com precisão matemática sem alterar o status operacional prematuramente.

**Teste independente**: Em uma OS com total de R$ 600,00 e sem pagamentos anteriores, registrar um pagamento parcial de R$ 200,00 via Dinheiro; verificar que o total pago passa a R$ 200,00, o saldo pendente passa a R$ 400,00 e o status da OS permanece inalterado (ex.: `EmAndamento`). Em seguida, lançar um segundo pagamento de R$ 400,00 e verificar que, agora sim, a OS transiciona para `Concluida`.

**Cenários de aceitação**:

1. **Dado** uma OS ativa com saldo devedor de R$ 600,00, **quando** o usuário abrir o modal de pagamento, alterar o valor para R$ 200,00 e confirmar a transação, **então** o pagamento é registrado com sucesso.
2. **Dado** que o pagamento parcial foi registrado, **quando** a tela for atualizada de forma reativa, **então** o total pago refletirá R$ 200,00, o saldo pendente passará a R$ 400,00 e o status da OS permanecerá o mesmo (`EmAndamento`).
3. **Dado** uma OS que já possua pagamentos parciais acumulados com saldo restante de R$ 150,00, **quando** o usuário registrar um novo pagamento de exatamente R$ 150,00, **então** o sistema somará todos os pagamentos e, detectando a quitação total, atualizará o status da OS para `Concluida`.

---

### História de Usuário 3 — Lançamento Automático no Módulo de Contas a Receber (Prioridade: P1)

Como gestor financeiro da oficina, quero que cada pagamento registrado em uma Ordem de Serviço gere automaticamente um registro correspondente na tabela de Contas a Receber (`FinanceiroContaReceber`), para manter a escrituração financeira, o fluxo de caixa e os relatórios de faturamento estritamente alinhados com as operações de oficina, sem necessidade de digitação dupla.

**Por que esta prioridade**: A integração automática entre Ordens de Serviço e o Módulo Financeiro é essencial para a integridade de dados e evita esquecimentos, erros humanos e retrabalho de conciliação financeira entre recepção e caixa.

**Teste independente**: Registrar um pagamento de OS de R$ 180,00 via PIX; acessar a listagem/consulta de contas a receber e confirmar que foi criado um lançamento contendo o cliente da OS, valor de R$ 180,00, data de recebimento igual à data do pagamento, método PIX, status "Recebido" e descrição contendo a referência da OS (ex.: `Pagamento OS #123`).

**Cenários de aceitação**:

1. **Dado** que uma requisição válida de pagamento de OS é enviada ao backend, **quando** o serviço de aplicação processar o registro, **então** ele criará automaticamente uma entidade `FinanceiroContaReceber` vinculada ao cliente da OS, com o mesmo valor, forma de pagamento, data de vencimento/recebimento e status `Recebido` (ou `Liquidado`).
2. **Dado** que o lançamento em Contas a Receber é gerado, **quando** for persistido no banco de dados, **então** a descrição e observação conterão a indicação da OS de origem (ex.: `Pagamento OS #{ordemId}`).
3. **Dado** que ocorra qualquer falha na persistência do pagamento da OS, na atualização de status ou no lançamento em Contas a Receber, **quando** a transação falhar, **então** todas as operações daquele escopo serão revertidas (rollback atômico), garantindo que não existam registros financeiros órfãos nem pagamentos sem lançamento contábil.

---

### História de Usuário 4 — Validação de Dados de Pagamento e Consistência de Valores (Prioridade: P2)

Como operador de caixa, quero que o formulário de pagamento valide os dados obrigatórios e previna digitações inválidas (valores zerados, negativos ou que excedam o saldo pendente), para que os registros financeiros reflitam estritamente a realidade da cobrança.

**Por que esta prioridade**: Digitações acidentais de valores exorbitantes ou submissões sem forma de pagamento desestabilizam o fechamento de caixa diário e exigem estornos manuais complicados.

**Teste independente**: Abrir o modal de pagamento e tentar submeter com valor zero, campo de valor em branco, valor superior ao saldo devedor ou sem selecionar forma de pagamento; validar que o sistema bloqueia a submissão e exibe mensagens orientativas claras.

**Cenários de aceitação**:

1. **Dado** que o modal de pagamento está visível, **quando** o usuário deixar o campo "Forma de Pagamento" não selecionado ou o "Valor Pago" zerado/negativo, **então** o botão de confirmação permanecerá desabilitado ou a tentativa de envio sinalizará os campos obrigatórios inválidos.
2. **Dado** que uma OS possui saldo pendente de R$ 120,00, **quando** o usuário digitar um valor de R$ 200,00 no campo Valor Pago, **então** o formulário exibirá uma advertência de validação indicando que o valor pago não pode exceder o saldo pendente de R$ 120,00.
3. **Dado** que o usuário preenche a data do pagamento, **quando** a data informada for uma data futura em relação ao dia corrente, **então** o sistema alertará que o pagamento realizado deve possuir data igual ou anterior ao dia atual (ou justificativa via observação).

---

### História de Usuário 5 — Proteção e Bloqueio de Pagamentos em Ordens Não Elegíveis (Prioridade: P2)

Como operador ou gestor da oficina, quero que a opção de registrar pagamentos fique desabilitada ou inacessível caso a OS esteja cancelada, sem itens lançados (total R$ 0,00) ou com saldo devedor já quitado, para prevenir lançamentos incorretos em ordens encerradas ou não precificadas.

**Por que esta prioridade**: Evita que pagamentos sejam vinculados a ordens que foram canceladas pelo cliente ou a ordens que ainda não possuem orçamento/itens definidos.

**Teste independente**: Carregar uma OS com status `Cancelada` ou com saldo pendente de R$ 0,00 e certificar que o botão "Registrar Pagamento" não está disponível para clique e um indicativo visual de quitação/cancelamento é exibido.

**Cenários de aceitação**:

1. **Dado** que a OS possui status `Cancelada`, **quando** a página for renderizada, **então** a ação "Registrar Pagamento" estará oculta ou desabilitada, exibindo aviso de que a ordem foi cancelada.
2. **Dado** que a OS possui saldo pendente de R$ 0,00 (100% quitada), **quando** o resumo financeiro for exibido, **então** a seção apresentará badge/indicativo "OS Quitada" e o botão "Registrar Pagamento" não será exibido.
3. **Dado** que a OS não possui nenhum item de peça ou serviço cadastrado (total dos itens = R$ 0,00), **quando** o usuário tentar registrar pagamento, **então** o sistema informará que é necessário adicionar itens à ordem antes de efetuar pagamentos.

---

## Casos de Borda

- **Pagamento com centavos e arredondamentos**: Cálculos e comparações entre a soma dos pagamentos e o total da OS devem usar precisão decimal em 2 casas decimais (tipo `decimal` no C# e `number` com arredondamento monetário no TypeScript) para evitar que diferenças infinitesimais de ponto flutuante (ex.: R$ 0,0000001) impeçam a conclusão da OS.
- **Prevenção de duplo clique (duplicação de pagamento)**: O botão de confirmação do modal deve entrar em estado de carregamento (`loading = true`), desabilitar novas interações e ignorar múltiplos cliques concorrentes durante o ciclo da requisição HTTP.
- **Fechamento involuntário do modal**: O usuário pode cancelar a operação pressionando ESC, clicando no botão "Cancelar" ou no botão de fechar (X), momento em que o modal deve fechar sem persistir dados e sem modificar o estado da página.
- **Falha de rede ou timeout na API**: Caso a conexão caia ou a API retorne HTTP 500, o modal deve permanecer aberto com as informações preenchidas para que o operador não perca os dados digitados e possa tentar novamente após restauração da conexão.
- **Múltiplos pagamentos parciais com diferentes formas**: O sistema deve permitir que um mesmo atendimento receba pagamentos distintos (ex.: R$ 100,00 em Dinheiro + R$ 250,00 em Cartão de Crédito) e liste cada um deles individualmente na tabela de pagamentos da OS com método, valor e data.
- **Cliente não informado na OS**: Caso a OS porventura não possua `ClienteId` preenchido (ou cliente genérico), o lançamento em `ContasReceber` deve lidar de forma segura com `ClienteId` nulo ou associar ao cliente cadastrado na OS.

---

## Requisitos *(obrigatório)*

### Requisitos Funcionais

- **FR-001**: O sistema DEVE disponibilizar o componente standalone `OsPagamentoModalComponent` para registro de pagamentos diretamente a partir da página de detalhe da Ordem de Serviço (`/ordens/:id`).
- **FR-002**: O modal DEVE conter os seguintes campos de entrada:
  - **Valor Pago** (`valor`): numérico decimal obrigatório, com valor maior que zero ($\le$ saldo pendente da OS), formatado com máscara monetária brasileira;
  - **Forma de Pagamento** (`metodo`): seleção suspensa obrigatória contendo os métodos aceitos (Dinheiro, PIX, Cartão de Crédito, Cartão de Débito, Transferência Bancária, Boleto);
  - **Data do Pagamento** (`dataPagamento`): campo de data obrigatório, pré-preenchido por padrão com a data corrente (hoje);
  - **Observação** (`observacao`): texto opcional com limite de 240 caracteres para anotações complementares (ex.: comprovante, número de autorização de maquininha).
- **FR-003**: Ao abrir o modal, o campo "Valor Pago" DEVE ser inicializado com o valor atual do saldo pendente (`saldoPendente`) da Ordem de Serviço, permitindo que o usuário altere o valor em caso de pagamento parcial.
- **FR-004**: O componente `OsDetalheComponent` DEVE exibir um botão "Registrar Pagamento" no cabeçalho da seção "Pagamentos", habilitado para ordens que possuam saldo devedor positivo e status operacional elegível.
- **FR-005**: O frontend DEVE persistir o pagamento através do serviço `OrdensService.addPagamento()` (ou método equivalente em `OrdensService`), enviando uma requisição `POST /api/v1/OrdemServicoPagamentos` conforme registrado em `apiPaths.ordens.pagamentos`.
- **FR-006**: O payload da requisição DEVE seguir a estrutura canônica:
  - `ordemServicoId`: identificador numérico da OS;
  - `valor`: valor pago numérico decimal;
  - `metodo`: texto/nome da forma de pagamento selecionada;
  - `dataPagamento`: string de data no formato ISO (`YYYY-MM-DD` ou `ISO 8601`);
  - `status`: indicador textual do pagamento (`"Liquidado"` ou `"Pago"`);
  - `observacao`: texto descritivo informado ou nulo.
- **FR-007**: O backend DEVE validar a existência da Ordem de Serviço associada e rejeitar o registro com erro semântico (HTTP 400 Bad Request) se a OS possuir status `Cancelada`.
- **FR-008**: O backend DEVE recuperar o somatório atual dos itens da OS e o somatório dos pagamentos já existentes mais o novo pagamento submetido.
- **FR-009**: Se a soma de todos os pagamentos for maior ou igual ao total dos itens da OS (pagamento integral), o backend DEVE atualizar o status da OS para `Concluida` e preencher `DataConclusao` com o timestamp atual (`DateTime.UtcNow`).
- **FR-010**: Se a soma dos pagamentos for inferior ao total da OS (pagamento parcial), o status da OS DEVE permanecer inalterado, permitindo novos lançamentos posteriores até a quitação.
- **FR-011**: A cada pagamento registrado, o backend DEVE criar automaticamente um lançamento em `ContasReceber` (`FinanceiroContaReceber`).
- **FR-012**: O lançamento gerado em `FinanceiroContaReceber` DEVE conter:
  - `ClienteId`: cliente associado à Ordem de Serviço;
  - `Descricao`: texto padronizado contendo a referência da OS e o método (ex.: `$"Pagamento OS #{ordem.Id} - {request.Metodo}"`);
  - `Valor`: valor informado no pagamento;
  - `Vencimento`: data informada no pagamento;
  - `DataRecebimento`: data informada no pagamento;
  - `Status`: status quitado (`"Recebido"` ou `"Liquidado"`);
  - `Observacao`: observação fornecida no pagamento complementada com a referência da OS.
- **FR-013**: A operação de registro do pagamento, a atualização de status da OS e a geração do lançamento financeiro em `ContasReceber` DEVEM ser executadas sob transação atômica no banco de dados (`IDbContextTransaction` / `BeginTransactionAsync`). Em caso de falha em qualquer etapa, todas as alterações DEVEM ser desfeitas.
- **FR-014**: Ao receber a confirmação de criação da API (HTTP 201 Created), o frontend DEVE:
  - Fechar o modal de pagamento;
  - Inserir o novo pagamento na coleção de pagamentos do signal `ordem`;
  - Atualizar o status do signal `ordem` para `Concluida` caso a resposta indique a conclusão integral da ordem;
  - Apresentar notificação de sucesso via serviço `Toast`.
- **FR-015**: A interface DEVE recalcular e apresentar os totais financeiros (`totalPago` e `saldoPendente`) em tempo real através dos signals reativos do Angular, sem recarregar a rota.
- **FR-016**: Quando uma OS for concluída após pagamento integral, as seções de edição e adição/remoção de itens DEVEM entrar em modo de visualização estrita (comportamento de status terminal).
- **FR-017**: Em ordens com status `Cancelada` ou com saldo pendente de R$ 0,00, a ação de registrar novo pagamento DEVE permanecer inacessível.
- **FR-018**: Em cenários de falha de validação ou de rede, o sistema DEVE emitir notificação de erro compreensível via serviço `Toast`, mantendo os dados no modal para nova submissão.

---

### Entidades Principais

- **Pagamento da Ordem de Serviço (`OrdemServicoPagamento` / `CreateOrdemServicoPagamentoRequest`)**:
  - `id`: Identificador único do pagamento;
  - `ordemServicoId`: Chave estrangeira para a OS;
  - `valor`: Valor financeiro registrado do pagamento;
  - `status`: Estado do pagamento (`Liquidado` / `Pago`);
  - `dataPagamento`: Data e hora em que a liquidação foi realizada;
  - `metodo`: Forma de pagamento empregada (Dinheiro, PIX, Cartão de Crédito, etc.);
  - `observacao`: Notas operacionais sobre o pagamento.
- **Conta a Receber (`FinanceiroContaReceber`)**: Registro contábil no módulo financeiro representando o direito de recebimento gerado e liquidado pela OS (`ClienteId`, `Descricao`, `Valor`, `Vencimento`, `Status`, `DataRecebimento`, `Observacao`).
- **Ordem de Serviço (`OrdemServico`)**: Entidade central do atendimento mecânico contendo status (`Aberta`, `EmAndamento`, `AguardandoPeca`, `Concluida`, `Cancelada`), data de conclusão e as coleções filhas de itens e pagamentos.
- **Resumo Financeiro da OS**:
  - `totalItens`: Somatório de todas as peças e serviços inclusos na OS;
  - `totalPago`: Somatório de todos os pagamentos vinculados e liquidados;
  - `saldoPendente`: Valor remanescente a liquidar ($\max(0, \text{totalItens} - \text{totalPago})$).

---

## Critérios de Sucesso *(obrigatório)*

### Resultados Mensuráveis

- **SC-001**: Em 100% dos pagamentos submetidos com dados válidos, o registro é gravado e os valores financeiros da OS são atualizados na tela em menos de 1,5 segundo em condições normais de rede.
- **SC-002**: Em 100% dos pagamentos que liquidem o saldo integral da OS, o status transiciona automaticamente para `Concluida` e a data de conclusão é gravada sem requisições manuais adicionais do operador.
- **SC-003**: 100% dos pagamentos confirmados com êxito criam um lançamento correspondente na tabela de Contas a Receber (`FinanceiroContaReceber`) com status de recebido e referência da OS.
- **SC-004**: 100% das falhas parciais em qualquer etapa do backend provocam rollback completo, garantindo zero inconsistências contábeis ou registros órfãos.
- **SC-005**: 100% das atualizações de pagamentos e status ocorrem de modo reativo via Signals sem provocar recarregamento da página (0 reloads).
- **SC-006**: 100% das chamadas de API utilizam o serviço centralizado `OrdensService` e o mapeamento canônico em `apiPaths.ordens.pagamentos`, com zero URLs hardcoded em componentes.
- **SC-007**: 100% das ordens quitadas (saldo R$ 0,00) ou canceladas impedem novos registros acidentais de pagamentos na interface.

---

## Premissas

- O backend `oficina-motos-api` já possui os repositórios `IOrdemServicoPagamentoRepository`, `IOrdemServicoRepository` e `IFinanceiroContaReceberRepository`, assim como as tabelas `os_pagamentos`, `os_ordens` e `fin_contas_receber`.
- O endpoint `POST /api/v1/OrdemServicoPagamentos` e a rota `apiPaths.ordens.pagamentos` já estão catalogados e disponíveis na infraestrutura da API e frontend.
- O componente `OsDetalheComponent` já calcula `totalItens`, `totalPago` e `saldoPendente`, necessitando apenas da integração com o novo modal `OsPagamentoModalComponent` e o botão acionador.
- As formas de pagamento padrão suportadas no modal são: `Dinheiro`, `PIX`, `Cartão de Crédito`, `Cartão de Débito`, `Transferência Bancária` e `Boleto`.
- O processamento de emissão fiscal (NF-e/NFS-e) e gestão de caixas múltiplos faz parte do escopo autônomo dos módulos de Financeiro/Fiscal e não bloqueia a conclusão da OS.
