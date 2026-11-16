# Especificação de Funcionalidade: Criar e Editar Conta a Pagar (US-021)

**Feature Branch**: `021-contas-pagar-form`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-021 — Criar e editar conta a pagar | Prioridade: 🟡 Should | Estimativa: M | Sprint: 4 | Depende: US-020 | Tasks: [ ] T-021.1 — Criar ContasPagarFormComponent, [ ] T-021.2 — Registrar rotas de criação e edição, [ ] T-021.3 — Integrar endpoints ContasPagar"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Cadastrar Nova Conta a Pagar em `/financeiro/contas-pagar/novo` (Priority: P1)

Como gestor, analista financeiro ou administrador da oficina, quero acessar o formulário de criação de conta a pagar em `/financeiro/contas-pagar/novo` (`ContasPagarFormComponent`) para registrar uma nova obrigação financeira (informando descrição, valor, data de vencimento, status, fornecedor opcional, método de pagamento opcional, data de pagamento opcional e observação), garantindo o controle completo dos compromissos financeiros da oficina.

**Why this priority**: A criação de novas obrigações financeiras (`T-021.1`, `T-021.2` e `T-021.3`) é o fluxo primário da história `US-021`, permitindo alimentar a listagem de contas a pagar entregue na `US-020`.

**Independent Test**: Pode ser testado navegando para `/financeiro/contas-pagar` e clicando em **"Nova Conta a Pagar"** (ou acessando diretamente `/financeiro/contas-pagar/novo`), preenchendo os campos obrigatórios (`descricao`, `valor`, `vencimento`, `status`), submetendo o formulário e verificando que `POST /api/v1/FinanceiroContasPagar` é invocado e o usuário é redirecionado para `/financeiro/contas-pagar` com feedback de sucesso.

**Acceptance Scenarios**:

1. **Given** que o usuário autenticado acessa `/financeiro/contas-pagar/novo`, **When** a tela é carregada, **Then** o sistema exibe o `ContasPagarFormComponent` em modo de criação com valores iniciais padrão (`status = 'PENDENTE'`, campos limpos) e carrega as listas auxiliares de fornecedores (`FornecedoresService.list()`) e métodos de pagamento (`FinanceiroService.metodos()`).
2. **Given** que o usuário preenche uma descrição válida (até 240 caracteres), um valor numérico maior que zero (`valor > 0`), uma data de vencimento válida e seleciona o status da conta, **When** aciona **"Salvar"**, **Then** o sistema envia `POST /api/v1/FinanceiroContasPagar`, exibe notificação de sucesso e redireciona para `/financeiro/contas-pagar`.
3. **Given** que o usuário tenta salvar o formulário sem preencher campos obrigatórios (`descricao`, `valor <= 0`, `vencimento` ou `status`) ou excedendo o limite de 240 caracteres em `descricao`/`observacao`, **When** submete o formulário, **Then** o sistema impede o envio à API e exibe mensagens de validação claras nos respectivos campos.

---

### User Story 2 - Editar Conta a Pagar Existente em `/financeiro/contas-pagar/:id/editar` (Priority: P1)

Como usuário com permissão de gestão financeira (`Administrador`, `Gerente` ou `Financeiro`), quero acessar `/financeiro/contas-pagar/:id/editar` a partir da listagem de contas a pagar para atualizar os dados de uma obrigação financeira existente (como corrigir valor, alterar vencimento, vincular fornecedor/método de pagamento, atualizar status ou observações).

**Why this priority**: A edição de conta a pagar existente compõe o núcleo da `US-021` (`T-021.1`, `T-021.2` e `T-021.3`) reutilizando o mesmo componente de formulário (`ContasPagarFormComponent`) e integrando os endpoints `GET /api/v1/FinanceiroContasPagar/{id}` e `PUT /api/v1/FinanceiroContasPagar/{id}`.

**Independent Test**: Na listagem `/financeiro/contas-pagar`, acionar o botão/link **"Editar"** de uma conta existente, verificar o carregamento dos dados atuais via `GET /api/v1/FinanceiroContasPagar/{id}` no formulário `/financeiro/contas-pagar/:id/editar`, alterar um campo (ex.: descrição ou valor), salvar e confirmar a chamada `PUT /api/v1/FinanceiroContasPagar/{id}` seguida do retorno para `/financeiro/contas-pagar`.

**Acceptance Scenarios**:

1. **Given** que uma conta a pagar de `id` válido existe, **When** o usuário acessa `/financeiro/contas-pagar/:id/editar`, **Then** o `ContasPagarFormComponent` entra em modo de edição, busca os dados da conta via `GET /api/v1/FinanceiroContasPagar/{id}`, formata as datas (`vencimento` e `dataPagamento`) para `yyyy-MM-dd` e preenche todos os campos do formulário.
2. **Given** que o usuário altera os dados válidos de uma conta em modo de edição, **When** aciona **"Salvar alterações"**, **Then** o sistema envia `PUT /api/v1/FinanceiroContasPagar/{id}` com o payload atualizado, exibe feedback de sucesso e navega de volta para `/financeiro/contas-pagar`.
3. **Given** que o usuário acessa `/financeiro/contas-pagar/:id/editar` com um `id` inexistente ou inválido, **When** a consulta `GET /api/v1/FinanceiroContasPagar/{id}` retorna erro/404, **Then** o sistema exibe estado de erro amigável com opção de tentar novamente ou voltar para `/financeiro/contas-pagar`.

---

### User Story 3 - Acessar Criação e Edição a partir da Lista de Contas a Pagar e do Painel Financeiro (Priority: P2)

Como usuário navegando na listagem `/financeiro/contas-pagar` ou no painel `/financeiro`, quero dispor de ações diretas para criar uma **"Nova Conta a Pagar"** e para **"Editar"** qualquer item listado na tabela de contas a pagar, além de poder cancelar a operação no formulário e retornar com segurança à listagem.

**Why this priority**: Conecta as novas rotas registradas em `T-021.2` com a listagem entregue na `US-020`, garantindo fluxo contínuo de navegação para o usuário final.

**Independent Test**: Acessar `/financeiro/contas-pagar`, verificar a presença do botão **"Nova Conta"** no cabeçalho (levando a `/financeiro/contas-pagar/novo`) e da ação **"Editar"** em cada linha da tabela (levando a `/financeiro/contas-pagar/:id/editar`), e testar o botão **"Cancelar / Voltar"** dentro do formulário.

**Acceptance Scenarios**:

1. **Given** que o usuário está em `/financeiro/contas-pagar`, **When** clica no botão **"Nova Conta a Pagar"**, **Then** o roteador navega para `/financeiro/contas-pagar/novo`.
2. **Given** que o usuário visualiza uma linha na tabela de `/financeiro/contas-pagar`, **When** clica na ação **"Editar"** daquela conta, **Then** o roteador navega para `/financeiro/contas-pagar/{id}/editar`.
3. **Given** que o usuário está no formulário de criação ou edição, **When** clica em **"Cancelar"** ou no link **"← Voltar para Contas a Pagar"**, **Then** o roteador retorna para `/financeiro/contas-pagar` sem persistir alterações.

---

### Edge Cases

- **Parâmetro de rota `:id` não numérico ou negativo**: Se a rota `/financeiro/contas-pagar/:id/editar` receber um parâmetro `:id` inválido (ex.: `abc` ou `0`), o componente não deve disparar requisição inválida para a API, exibindo imediatamente mensagem de identificador inválido com link de retorno para `/financeiro/contas-pagar`.
- **Campos opcionais vazios (`fornecedorId`, `metodoId`, `dataPagamento`, `observacao`)**: Quando não preenchidos pelo usuário no formulário, devem ser serializados como `null` no payload enviado para `POST` / `PUT /api/v1/FinanceiroContasPagar`, evitando envio de strings vazias `""` ou `0` para chaves estrangeiras opcionais.
- **Preenchimento de status `PAGO` sem data de pagamento**: Se o usuário selecionar o status `PAGO` e não informar `dataPagamento`, o formulário pode sugerir ou preencher a data atual como padrão (permitindo edição pelo usuário), enquanto para status `PENDENTE`, `VENCIDO` ou `CANCELADO` o campo `dataPagamento` permanece opcional.
- **Falha nas consultas auxiliares (`Fornecedores` ou `MetodosPagamento`)**: Caso a listagem de fornecedores ou métodos de pagamento falhe ao carregar, o formulário deve continuar funcional para preenchimento e salvamento dos campos principais da conta a pagar (`descricao`, `valor`, `vencimento`, `status`, `observacao`), sem bloquear o usuário.
- **Prevenção de duplo envio (double submit)**: Enquanto a requisição `POST` ou `PUT` estiver em andamento (`saving() === true`), o botão de submissão deve permanecer desabilitado com indicador visual de salvamento.
- **Permissão RBAC em operações de escrita**: Caso o usuário não possua permissão de escrita no módulo financeiro (ex.: perfil `Consulta` recebendo `403 Forbidden` em `PUT /api/v1/FinanceiroContasPagar/{id}`), o sistema deve preservar os dados na tela, encerrar o estado de salvamento e exibir feedback de erro apropriado.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE disponibilizar o componente standalone `ContasPagarFormComponent` em `src/app/features/financeiro/pages/contas-pagar-form/` utilizando Reactive Forms e Angular Signals (`T-021.1`).
- **FR-002**: O sistema DEVE registrar as rotas protegidas por `authGuard` em `src/app/app.routes.ts`: `/financeiro/contas-pagar/novo` (modo criação) e `/financeiro/contas-pagar/:id/editar` (modo edição), ambas associadas ao `ContasPagarFormComponent` (`T-021.2`).
- **FR-003**: O `FinanceiroService` (`src/app/core/services/financeiro.service.ts`) DEVE expor os métodos de integração com o endpoint centralizado `apiPaths.financeiro.pagar` (`/api/v1/FinanceiroContasPagar`): `getContaPagarById(id)`, `createContaPagar(payload)` e `updateContaPagar(id, payload)` (`T-021.3`).
- **FR-004**: O formulário `ContasPagarFormComponent` DEVE conter os controles:
  - `descricao` (texto obrigatório, máximo de 240 caracteres);
  - `fornecedorId` (seleção opcional a partir de `FornecedoresService.list()`);
  - `valor` (numérico obrigatório, maior que `0`);
  - `vencimento` (data obrigatória no formato `yyyy-MM-dd`);
  - `status` (seleção obrigatória entre `PENDENTE`, `PAGO`, `VENCIDO`, `CANCELADO`, com padrão `PENDENTE` na criação);
  - `metodoId` (seleção opcional a partir de `FinanceiroService.metodos()`);
  - `dataPagamento` (data opcional no formato `yyyy-MM-dd`);
  - `observacao` (texto opcional, máximo de 240 caracteres).
- **FR-005**: Em modo de criação (`/financeiro/contas-pagar/novo`), ao submeter um formulário válido, o componente DEVE chamar `FinanceiroService.createContaPagar(payload)` (`POST /api/v1/FinanceiroContasPagar`), exibir feedback de sucesso e redirecionar para `/financeiro/contas-pagar`.
- **FR-006**: Em modo de edição (`/financeiro/contas-pagar/:id/editar`), o componente DEVE carregar o registro existente via `FinanceiroService.getContaPagarById(id)` (`GET /api/v1/FinanceiroContasPagar/{id}`), preencher o formulário normalizando datas ISO para `yyyy-MM-dd` e status para caixa alta, e ao submeter um formulário válido DEVE chamar `FinanceiroService.updateContaPagar(id, payload)` (`PUT /api/v1/FinanceiroContasPagar/{id}`), exibir feedback de sucesso e redirecionar para `/financeiro/contas-pagar`.
- **FR-007**: A listagem `ContasPagarListaComponent` (`/financeiro/contas-pagar`) DEVE exibir o botão **"Nova Conta"** no cabeçalho apontando para `/financeiro/contas-pagar/novo` e uma coluna de **Ações** na tabela com link **"Editar"** apontando para `/financeiro/contas-pagar/:id/editar` (respeitando a visibilidade por perfil quando aplicável).

### Key Entities *(include if feature involves data)*

- **`FinanceiroContaPagar` / `CreateFinanceiroContaPagarRequest` / `UpdateFinanceiroContaPagarRequest`** (`src/app/core/models/financeiro.ts` e DTOs `CreateFinanceiroContaPagarDTO` / `UpdateFinanceiroContaPagarDTO` no backend): Representa os dados de criação, leitura e atualização de uma conta a pagar (`id`, `fornecedorId`, `descricao`, `valor`, `vencimento`, `status`, `dataPagamento`, `metodoId`, `observacao`).
- **`Fornecedor`** (`src/app/core/models/fornecedor.ts`): Entidade de referência para seleção opcional do credor vinculado à conta a pagar.
- **`FinanceiroMetodoPagamento`** (`src/app/core/models/financeiro.ts`): Entidade de referência para seleção opcional da forma/método de pagamento.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Em 100% dos acessos autenticados a `/financeiro/contas-pagar/novo` e `/financeiro/contas-pagar/:id/editar`, o componente `ContasPagarFormComponent` é renderizado no modo correspondente (criação ou edição) (`T-021.1`, `T-021.2`).
- **SC-002**: Em 100% das submissões válidas em modo de criação, o sistema executa `POST /api/v1/FinanceiroContasPagar` com payload tipado e redireciona o usuário para `/financeiro/contas-pagar` (`T-021.3`).
- **SC-003**: Em 100% das submissões válidas em modo de edição, o sistema carrega previamente os dados via `GET /api/v1/FinanceiroContasPagar/{id}`, executa `PUT /api/v1/FinanceiroContasPagar/{id}` com os dados atualizados e redireciona para `/financeiro/contas-pagar` (`T-021.3`).
- **SC-004**: 100% das tentativas de submissão com campos obrigatórios vazios (`descricao`, `valor <= 0`, `vencimento`, `status`) ou acima do limite de 240 caracteres são bloqueadas no cliente antes de qualquer chamada HTTP.
- **SC-005**: Todos os testes unitários do frontend (`npm test -- --no-watch`) — incluindo novas specs para `ContasPagarFormComponent` e `FinanceiroService` — e a compilação de produção (`npm run build`) passam com zero erros.

## Assumptions

- Os endpoints backend `GET /api/v1/FinanceiroContasPagar/{id}`, `POST /api/v1/FinanceiroContasPagar` e `PUT /api/v1/FinanceiroContasPagar/{id}` já existem e estão operacionais em `FinanceiroContasPagarController`, validados por `CreateFinanceiroContaPagarValidator` e `UpdateFinanceiroContaPagarValidator` (`Descricao` obrigatória máx. 240, `Status` obrigatório máx. 50, `Observacao` máx. 240), não sendo necessária alteração no banco de dados nem criação de migrations.
- Um único componente `ContasPagarFormComponent` atende tanto à rota de criação (`/financeiro/contas-pagar/novo`) quanto à rota de edição (`/financeiro/contas-pagar/:id/editar`), determinando o modo de operação pela presença do parâmetro `:id` no `ActivatedRoute`.
- O registro dedicado de baixa de pagamento (com suporte a pagamento parcial e modal rápido na listagem) será tratado na história específica `US-024 — Registrar baixa de conta`.
