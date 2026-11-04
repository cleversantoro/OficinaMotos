# Especificação de Funcionalidade: Registrar Movimentações de Entrada e Saída de Estoque (US-019)

**Branch da Funcionalidade**: `019-movimentacoes-estoque`  
**Criado em**: 2026-11-04  
**Status**: Rascunho  
**Entrada**: Descrição do usuário: "US-019 — Registrar movimentações de entrada e saída de estoque | Prioridade: 🟡 Should | Estimativa: M | Sprint: 3 | Depende: US-018 | Tasks: T-019.1 Criar EstoqueEntradaModalComponent, T-019.2 Criar EstoqueSaidaModalComponent, T-019.3 Integrar POST /api/v1/EstoqueMovimentacoes, T-019.4 Criar EstoquePecaDetalheComponent com histórico, T-019.5 Registrar rota /estoque/:id"

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 - Visualizar Detalhes e Histórico da Peça em `/estoque/:id` (Prioridade: P1)

Como gestor, estoquista ou atendente da oficina, quero acessar a página de detalhes de uma peça no estoque em `/estoque/:id` para consultar os dados cadastrais da peça (código, descrição, fabricante, categoria, localização, preço unitário, quantidade atual, estoque mínimo/máximo e status), visualizar o histórico cronológico de movimentações de entrada e saída e acionar os modais de movimentação.

**Por que esta prioridade**: A página de detalhes da peça (`EstoquePecaDetalheComponent`, `T-019.4` e `T-019.5`) é o ponto central de consulta de saldo, auditoria de movimentações e acionamento das operações de entrada e saída de estoque.

**Teste Independente**: Pode ser testado navegando para `/estoque/10` (ou clicando na peça na listagem `/estoque`), verificando a renderização dos dados da peça, do badge de estoque crítico quando `quantidade < estoqueMinimo`, da tabela de histórico de movimentações e dos botões **"Registrar Entrada"** e **"Registrar Saída"**.

**Cenários de Aceitação**:

1. **Dado** que o usuário está autenticado e acessa `/estoque/:id` com um ID numérico válido, **Quando** a página carrega, **Então** o sistema busca os dados da peça (`GET /api/v1/EstoquePecas/:id`), a lista de movimentações filtrada por `pecaId` (`GET /api/v1/EstoqueMovimentacoes`) e exibe as informações cadastrais, os indicadores de saldo e a tabela de histórico ordenada da movimentação mais recente para a mais antiga.
2. **Dado** que a peça possui `quantidade < estoqueMinimo`, **Quando** o detalhe da peça é exibido, **Então** o sistema destaca o saldo com o badge visual **"Crítico"** (`badge-critico`).
3. **Dado** que o usuário acessa `/estoque/:id` com um ID inexistente ou inválido, **Quando** a API retorna erro ou o parâmetro não é numérico, **Então** o sistema exibe mensagem de erro amigável com botão para retornar à listagem `/estoque`.

---

### História de Usuário 2 - Registrar Entrada de Estoque via Modal (Prioridade: P1)

Como estoquista ou gestor da oficina, quero registrar uma entrada de estoque para uma peça através de um modal dedicado (`EstoqueEntradaModalComponent`), informando a quantidade recebida e a referência/observação (ex.: Nota Fiscal, Compra, Ajuste de Inventário), para incrementar o saldo disponível da peça e manter a rastreabilidade das entradas.

**Por que esta prioridade**: O abastecimento do estoque (`T-019.1` e `T-019.3`) é indispensável para repor peças e atualizar os saldos disponíveis para ordens de serviço.

**Teste Independente**: Na tela `/estoque/:id`, clicar em **"Registrar Entrada"**, preencher `quantidade = 10` e `referencia = "NF 12345"` no `EstoqueEntradaModalComponent`, confirmar o envio e verificar que `POST /api/v1/EstoqueMovimentacoes` é chamado com `tipo: "ENTRADA"`, o saldo da peça é atualizado (`quantidadeAtual + 10`) e a listagem de histórico exibe o novo registro.

**Cenários de Aceitação**:

1. **Dado** que o usuário está na tela `/estoque/:id` e clica em **"Registrar Entrada"**, **Quando** o modal `EstoqueEntradaModalComponent` é aberto, **Então** são exibidos o nome/código da peça, o saldo atual e um formulário reativo com os campos **Quantidade** (inteiro obrigatório `>= 1`) e **Referência / Motivo** (texto obrigatório, máx. 200 caracteres).
2. **Dado** que o usuário preenche uma quantidade válida (`>= 1`) e a referência no modal de entrada, **Quando** clica em **"Confirmar Entrada"**, **Então** o sistema envia `POST /api/v1/EstoqueMovimentacoes` com `{ pecaId, quantidade, tipo: "ENTRADA", referencia, usuario }`, atualiza o saldo da peça (`PUT /api/v1/EstoquePecas/:id` com `quantidade: saldoAtual + quantidade`), exibe notificação de sucesso, fecha o modal e recarrega os dados e o histórico da peça.
3. **Dado** que o usuário tenta confirmar a entrada com quantidade vazia, zero ou negativa, ou sem referência, **Quando** submete o formulário do modal, **Então** o formulário marca os campos como tocados, exibe mensagens de validação e bloqueia a requisição.

---

### História de Usuário 3 - Registrar Saída de Estoque via Modal (Prioridade: P1)

Como estoquista ou mecânico da oficina, quero registrar uma saída/baixa de estoque para uma peça através de um modal dedicado (`EstoqueSaidaModalComponent`), informando a quantidade retirada e a referência/motivo (ex.: OS #102, Uso interno, Avaria), validando que a quantidade retirada não ultrapasse o saldo disponível em estoque.

**Por que esta prioridade**: O controle de baixas de estoque (`T-019.2` e `T-019.3`) evita furos de inventário e impede que o estoque fique com saldo negativo.

**Teste Independente**: Na tela `/estoque/:id`, clicar em **"Registrar Saída"**, preencher `quantidade = 3` (menor ou igual ao saldo atual) e `referencia = "OS #102"` no `EstoqueSaidaModalComponent`, confirmar o envio e verificar que `POST /api/v1/EstoqueMovimentacoes` é chamado com `tipo: "SAIDA"`, o saldo da peça é reduzido (`quantidadeAtual - 3`) e o histórico é atualizado.

**Cenários de Aceitação**:

1. **Dado** que o usuário está na tela `/estoque/:id` e clica em **"Registrar Saída"**, **Quando** o modal `EstoqueSaidaModalComponent` é aberto, **Então** são exibidos o nome/código da peça, o saldo disponível atual e o formulário reativo com **Quantidade** (inteiro obrigatório entre `1` e `saldoAtual`) e **Referência / Motivo** (texto obrigatório, máx. 200 caracteres).
2. **Dado** que o usuário informa uma quantidade superior ao saldo disponível da peça (`quantidade > saldoAtual`), **Quando** tenta confirmar a saída, **Então** o formulário exibe erro de validação informando que a quantidade excede o saldo disponível e impede o envio.
3. **Dado** que o usuário preenche uma quantidade válida (`1 <= quantidade <= saldoAtual`) e a referência, **Quando** clica em **"Confirmar Saída"**, **Então** o sistema envia `POST /api/v1/EstoqueMovimentacoes` com `{ pecaId, quantidade, tipo: "SAIDA", referencia, usuario }`, atualiza o saldo da peça (`PUT /api/v1/EstoquePecas/:id` com `quantidade: saldoAtual - quantidade`), exibe notificação de sucesso, fecha o modal e recarrega os dados e o histórico da peça.

---

### História de Usuário 4 - Navegar da Listagem de Estoque para o Detalhe da Peça (Prioridade: P2)

Como usuário na tela de listagem de estoque (`/estoque`), quero clicar no nome da peça ou no botão de ação **"Visualizar"** na linha da tabela para abrir a página `/estoque/:id`.

**Por que esta prioridade**: Conecta a listagem existente (`EstoqueListaComponent`) à nova rota `/estoque/:id` (`T-019.5`).

**Teste Independente**: Em `/estoque`, clicar no nome de uma peça ou no ícone de visualização da linha e verificar a navegação para `/estoque/:id`.

**Cenários de Aceitação**:

1. **Dado** que o usuário visualiza a tabela em `/estoque`, **Quando** clica no nome da peça ou no botão **"Visualizar"** (`pi-eye`), **Então** o roteador navega para `/estoque/:id`.

---

### Casos de Borda

- **Peça com saldo zero (`quantidade === 0`)**: O botão **"Registrar Saída"** na página de detalhes pode ser desabilitado ou o modal `EstoqueSaidaModalComponent` deve bloquear qualquer saída informando que não há saldo disponível em estoque.
- **Cliques duplos durante submissão do modal**: Os botões **"Confirmar Entrada"** e **"Confirmar Saída"** devem ficar desabilitados (`saving() === true`) enquanto a requisição estiver em andamento.
- **Falha de comunicação com a API**: Caso `POST /api/v1/EstoqueMovimentacoes` ou `PUT /api/v1/EstoquePecas/:id` retorne erro HTTP, o modal permanece aberto, restaura o estado `saving = false` e exibe toast de erro via `NotificationService`.
- **Ordem das rotas em `app.routes.ts`**: A rota `estoque/novo` deve permanecer declarada **antes** de `estoque/:id` para evitar que `"novo"` seja interpretado como `:id`.

## Requisitos *(obrigatório)*

### Requisitos Funcionais

- **FR-001**: O sistema DEVE registrar a rota `/estoque/:id` em `src/app/app.routes.ts` (protegida por `authGuard` e declarada após `estoque/novo`) apontando para `EstoquePecaDetalheComponent` (`T-019.5`).
- **FR-002**: O componente `EstoquePecaDetalheComponent` (`T-019.4`) DEVE carregar os dados da peça via `EstoqueService.getPeca(id)` (`GET /api/v1/EstoquePecas/:id`), metadados auxiliares (fabricantes, categorias e localizações para exibição legível) e o histórico de movimentações via `EstoqueService.listarMovimentacoes()` (`GET /api/v1/EstoqueMovimentacoes`), filtrando pelas movimentações de `pecaId === id` e ordenando por `dataMovimentacao` decrescente.
- **FR-003**: O componente `EstoquePecaDetalheComponent` DEVE exibir os dados principais da peça (código, descrição, fabricante, categoria, localização, preço unitário, quantidade atual, estoque mínimo, estoque máximo, status e badge **"Crítico"** quando `quantidade < estoqueMinimo`), além de botões para **"Voltar"**, **"Registrar Entrada"** e **"Registrar Saída"**.
- **FR-004**: O componente `EstoquePecaDetalheComponent` DEVE exibir uma tabela de histórico de movimentações contendo: Data/Hora (`dataMovimentacao`), Tipo (`ENTRADA` com badge verde / `SAIDA` com badge vermelho/laranja), Quantidade (`+N` para entrada, `-N` para saída), Referência/Motivo (`referencia`) e Usuário responsável (`usuario`).
- **FR-005**: O componente `EstoqueEntradaModalComponent` (`T-019.1`) DEVE ser um componente standalone baseado em `<p-dialog>` do PrimeNG com formulário reativo contendo `quantidade` (`Validators.required`, `Validators.min(1)`) e `referencia` (`Validators.required`, `Validators.maxLength(200)`).
- **FR-006**: O componente `EstoqueSaidaModalComponent` (`T-019.2`) DEVE ser um componente standalone baseado em `<p-dialog>` do PrimeNG com formulário reativo contendo `quantidade` (`Validators.required`, `Validators.min(1)`, `Validators.max(peca.quantidade)`) e `referencia` (`Validators.required`, `Validators.maxLength(200)`).
- **FR-007**: Ao confirmar uma entrada ou saída (`T-019.3`), o sistema DEVE enviar `POST /api/v1/EstoqueMovimentacoes` através de `EstoqueService.movimentar(payload)` com `pecaId`, `quantidade`, `tipo` (`"ENTRADA"` ou `"SAIDA"`), `referencia` e `usuario` (obtido de `AuthService.currentUser()`), e atualizar o saldo da peça via `EstoqueService.atualizarPeca(peca.id, ...)` com a nova quantidade calculada (`saldoAtual + quantidade` na entrada; `saldoAtual - quantidade` na saída).
- **FR-008**: Após o registro bem-sucedido da movimentação e atualização do saldo, o modal DEVE emitir o evento de conclusão (`saved`), exibir mensagem de sucesso via `NotificationService.success(...)` e atualizar os dados e o histórico exibidos em `EstoquePecaDetalheComponent`.
- **FR-009**: O componente `EstoqueListaComponent` (`/estoque`) DEVE permitir navegar para `/estoque/:id` ao clicar no nome da peça ou em um botão de ação **"Visualizar"** na tabela de estoque.

### Requisitos Não Funcionais

- **NFR-001**: Os componentes `EstoquePecaDetalheComponent`, `EstoqueEntradaModalComponent` e `EstoqueSaidaModalComponent` DEVEM utilizar Angular 21 standalone components, Signals (`signal`, `computed`, `input`, `output`) e Reactive Forms (`FormBuilder`), seguindo o padrão arquitetural de `governance/arquitetura.md`.
- **NFR-002**: Nenhuma alteração de schema de banco de dados (migrations) deve ser realizada, utilizando exclusivamente os endpoints e tabelas existentes (`est_pecas` e `est_movimentacoes`).
- **NFR-003**: A funcionalidade DEVE possuir cobertura de testes unitários com Vitest (`.spec.ts`) validando o carregamento de detalhes e histórico, o registro de entrada, o registro de saída e o bloqueio de saída acima do saldo disponível.

### Entidades Envolvidas

- **`EstoqueItem`** (`src/app/core/models/estoque.ts`): Representa a peça de estoque (`id`, `codigo`, `descricao`, `fabricanteId`, `categoriaId`, `localizacaoId`, `precoUnitario`, `quantidade`, `estoqueMinimo`, `estoqueMaximo`, `status`).
- **`EstoqueMovimentacaoPayload`** (`src/app/core/models/estoque.ts`): Payload enviado para `POST /api/v1/EstoqueMovimentacoes` (`pecaId`, `quantidade`, `tipo: 'ENTRADA' | 'SAIDA'`, `referencia`, `usuario`).
- **`EstoqueMovimentacao`** (`src/app/core/models/estoque.ts`): Interface de resposta de `GET /api/v1/EstoqueMovimentacoes` (`id`, `pecaId`, `quantidade`, `tipo`, `referencia`, `dataMovimentacao`, `usuario`).

## Critérios de Sucesso *(obrigatório)*

### Resultados Mensuráveis

- **SC-001**: A rota `/estoque/:id` carrega e exibe corretamente os detalhes da peça e a tabela de histórico de movimentações (`T-019.4`, `T-019.5`).
- **SC-002**: O modal `EstoqueEntradaModalComponent` (`T-019.1`) registra entradas via `POST /api/v1/EstoqueMovimentacoes` (`T-019.3`) e incrementa o saldo da peça.
- **SC-003**: O modal `EstoqueSaidaModalComponent` (`T-019.2`) valida que `quantidade <= saldoAtual`, registra saídas via `POST /api/v1/EstoqueMovimentacoes` (`T-019.3`) e decrementa o saldo da peça.
- **SC-004**: A listagem `/estoque` permite navegar diretamente para `/estoque/:id` a partir do nome da peça ou do botão de ação da linha.
- **SC-005**: Todos os testes unitários (`npm test -- --no-watch`) e o build de produção (`npm run build`) executam com 100% de sucesso e zero erros de compilação.
