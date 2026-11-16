# Especificação de Funcionalidade: Criar e Editar Conta a Receber Manual (US-023)

**Feature Branch**: `023-contas-receber-form`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-023 — Criar e editar conta a receber manual | Prioridade: 🟡 Should | Estimativa: M | Sprint: 4 | Depende: US-022 | Tasks: [ ] T-023.1 — Criar ContasReceberFormComponent, [ ] T-023.2 — Registrar rotas de criação e edição"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Cadastrar Nova Conta a Receber Manual em `/financeiro/contas-receber/novo` (Priority: P1)

Como gestor, analista financeiro ou administrador da oficina, quero acessar o formulário de criação de conta a receber manual em `/financeiro/contas-receber/novo` (`ContasReceberFormComponent`) para registrar uma nova entrada financeira avulsa (informando descrição, valor, data de vencimento, status, cliente opcional, método de pagamento opcional, data de recebimento opcional e observação), permitindo o controle de recebíveis que não vieram automaticamente de uma Ordem de Serviço (como venda avulsa no balcão, serviços externos ou acordos financeiros).

**Why this priority**: A criação manual de contas a receber (`T-023.1` e `T-023.2`) é o fluxo principal da história `US-023`, complementando os recebíveis gerados automaticamente pelas Ordens de Serviço (`US-012`) e alimentando a listagem entregue na `US-022`.

**Independent Test**: Pode ser testado navegando para `/financeiro/contas-receber` e clicando em **"Nova Conta a Receber"** (ou acessando diretamente `/financeiro/contas-receber/novo`), preenchendo os campos obrigatórios (`descricao`, `valor`, `vencimento`, `status`), submetendo o formulário e verificando que `POST /api/v1/FinanceiroContasReceber` é invocado e o usuário é redirecionado para `/financeiro/contas-receber` com feedback de sucesso.

**Acceptance Scenarios**:

1. **Given** que o usuário autenticado acessa `/financeiro/contas-receber/novo`, **When** a tela é carregada, **Then** o sistema exibe o `ContasReceberFormComponent` em modo de criação com valores iniciais padrão (`status = 'PENDENTE'`, campos limpos) e carrega de forma resiliente as listas auxiliares de clientes (`ClientesService.list()`) e métodos de pagamento (`FinanceiroService.metodos()`).
2. **Given** que o usuário preenche uma descrição válida (até 240 caracteres), um valor numérico maior que zero (`valor > 0`), uma data de vencimento válida e seleciona o status da conta (`PENDENTE`, `VENCIDO`, `RECEBIDO` ou `CANCELADO`), **When** aciona **"Salvar"**, **Then** o sistema envia `POST /api/v1/FinanceiroContasReceber`, exibe notificação de sucesso e redireciona para `/financeiro/contas-receber`.
3. **Given** que o usuário tenta salvar o formulário sem preencher campos obrigatórios (`descricao`, `valor <= 0`, `vencimento` ou `status`) ou excedendo o limite de 240 caracteres em `descricao`/`observacao`, **When** submete o formulário, **Then** o sistema impede o envio à API e exibe mensagens de validação claras nos respectivos campos.

---

### User Story 2 - Editar Conta a Receber Existente em `/financeiro/contas-receber/:id/editar` (Priority: P1)

Como usuário com permissão de gestão financeira, quero acessar `/financeiro/contas-receber/:id/editar` a partir da listagem de contas a receber para atualizar os dados de um recebível existente (como ajustar descrição, corrigir valor, alterar data de vencimento, vincular cliente/método de pagamento, atualizar status, data de recebimento ou observações).

**Why this priority**: A edição de conta a receber existente compõe o núcleo da `US-023` (`T-023.1` e `T-023.2`), reutilizando o mesmo componente `ContasReceberFormComponent` e integrando os endpoints `GET /api/v1/FinanceiroContasReceber/{id}` e `PUT /api/v1/FinanceiroContasReceber/{id}`.

**Independent Test**: Na listagem `/financeiro/contas-receber`, acionar o botão/link **"Editar"** de uma conta existente, verificar o carregamento dos dados atuais via `GET /api/v1/FinanceiroContasReceber/{id}` no formulário `/financeiro/contas-receber/:id/editar`, alterar um campo (ex.: descrição ou valor), salvar e confirmar a chamada `PUT /api/v1/FinanceiroContasReceber/{id}` seguida do retorno para `/financeiro/contas-receber`.

**Acceptance Scenarios**:

1. **Given** que uma conta a receber de `id` válido existe, **When** o usuário acessa `/financeiro/contas-receber/:id/editar`, **Then** o `ContasReceberFormComponent` entra em modo de edição, busca os dados da conta via `GET /api/v1/FinanceiroContasReceber/{id}`, formata as datas (`vencimento` e `dataRecebimento`) para `yyyy-MM-dd`, normaliza o status (`RECEBIDO`/`PAGO`/`LIQUIDADO` → `RECEBIDO`) e preenche todos os campos do formulário.
2. **Given** que o usuário altera os dados válidos de uma conta em modo de edição, **When** aciona **"Salvar alterações"**, **Then** o sistema envia `PUT /api/v1/FinanceiroContasReceber/{id}` com o payload atualizado, exibe feedback de sucesso e navega de volta para `/financeiro/contas-receber`.
3. **Given** que o usuário acessa `/financeiro/contas-receber/:id/editar` com um `id` inexistente ou inválido, **When** a consulta `GET /api/v1/FinanceiroContasReceber/{id}` retorna erro/404, **Then** o sistema exibe estado de erro amigável com opção de tentar novamente ou voltar para `/financeiro/contas-receber`.

---

### User Story 3 - Acessar Criação e Edição a partir da Lista de Contas a Receber e do Painel Financeiro (Priority: P2)

Como usuário navegando na listagem `/financeiro/contas-receber` ou no painel `/financeiro`, quero dispor de ações diretas para criar uma **"Nova Conta a Receber"** e para **"Editar"** qualquer item listado na tabela de contas a receber, além de poder cancelar a operação no formulário e retornar com segurança à listagem.

**Why this priority**: Conecta as novas rotas registradas em `T-023.2` com a listagem entregue na `US-022` e o painel financeiro, garantindo fluxo contínuo de navegação para o usuário final.

**Independent Test**: Acessar `/financeiro/contas-receber`, verificar a presença do botão **"Nova Conta"** no cabeçalho (levando a `/financeiro/contas-receber/novo`) e da ação **"Editar"** em cada linha da tabela (levando a `/financeiro/contas-receber/:id/editar`), e testar o botão **"Cancelar / Voltar"** dentro do formulário.

**Acceptance Scenarios**:

1. **Given** que o usuário está em `/financeiro/contas-receber`, **When** clica no botão **"Nova Conta"** (ou **"Cadastrar Primeira Conta"** no estado vazio), **Then** o roteador navega para `/financeiro/contas-receber/novo`.
2. **Given** que o usuário visualiza uma linha na tabela de `/financeiro/contas-receber`, **When** clica na ação **"Editar"** daquela conta, **Then** o roteador navega para `/financeiro/contas-receber/{id}/editar`.
3. **Given** que o usuário está no formulário de criação ou edição, **When** clica em **"Cancelar"** ou no link **"← Voltar para Contas a Receber"**, **Then** o roteador retorna para `/financeiro/contas-receber` sem persistir alterações.

---

### Edge Cases

- **Parâmetro de rota `:id` não numérico ou negativo**: Se a rota `/financeiro/contas-receber/:id/editar` receber um parâmetro `:id` inválido (ex.: `abc` ou `0`), o componente não deve disparar requisição inválida para a API, exibindo imediatamente mensagem de identificador inválido com link de retorno para `/financeiro/contas-receber`.
- **Campos opcionais vazios (`clienteId`, `metodoId`, `dataRecebimento`, `observacao`)**: Quando não preenchidos pelo usuário no formulário, devem ser serializados como `null` no payload enviado para `POST` / `PUT /api/v1/FinanceiroContasReceber`, evitando envio de strings vazias `""` ou `0` para chaves estrangeiras opcionais.
- **Preenchimento de status `RECEBIDO` sem data de recebimento**: Se o usuário selecionar o status `RECEBIDO` e não informar `dataRecebimento`, o formulário preenche automaticamente a data atual (`yyyy-MM-dd`) como padrão (permitindo edição pelo usuário), enquanto para status `PENDENTE`, `VENCIDO` ou `CANCELADO` o campo `dataRecebimento` permanece opcional.
- **Normalização de status legado/originado de OS em modo de edição**: Contas originadas de pagamento de OS ou registros existentes podem retornar status como `'PAGO'`, `'LIQUIDADO'` ou `'ABERTO'`; ao carregar para edição, `'PAGO'` e `'LIQUIDADO'` devem ser normalizados para `'RECEBIDO'` e `'ABERTO'` para `'PENDENTE'`.
- **Falha nas consultas auxiliares (`Clientes` ou `MetodosPagamento`)**: Caso a listagem de clientes ou métodos de pagamento falhe ao carregar, o formulário deve continuar funcional para preenchimento e salvamento dos campos principais da conta a receber (`descricao`, `valor`, `vencimento`, `status`, `observacao`), sem bloquear o usuário.
- **Prevenção de duplo envio (double submit)**: Enquanto a requisição `POST` ou `PUT` estiver em andamento (`submitting() === true`), o botão de submissão deve permanecer desabilitado com indicador visual de salvamento.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE disponibilizar o componente standalone `ContasReceberFormComponent` em `src/app/features/financeiro/pages/contas-receber-form/` utilizando Reactive Forms, `ChangeDetectionStrategy.OnPush` e Angular Signals (`T-023.1`).
- **FR-002**: O sistema DEVE registrar as rotas protegidas por `authGuard` em `src/app/app.routes.ts`: `/financeiro/contas-receber/novo` (modo criação) e `/financeiro/contas-receber/:id/editar` (modo edição), ambas associadas ao `ContasReceberFormComponent` (`T-023.2`).
- **FR-003**: O `FinanceiroService` (`src/app/core/services/financeiro.service.ts`) DEVE expor os métodos de integração com o endpoint centralizado `apiPaths.financeiro.receber` (`/api/v1/FinanceiroContasReceber`): `getContaReceberById(id)`, `createContaReceber(payload)` e `updateContaReceber(id, payload)`.
- **FR-004**: O formulário `ContasReceberFormComponent` DEVE conter os controles:
  - `descricao` (texto obrigatório, máximo de 240 caracteres);
  - `clienteId` (seleção opcional a partir de `ClientesService.list()`);
  - `valor` (numérico obrigatório, maior que `0`);
  - `vencimento` (data obrigatória no formato `yyyy-MM-dd`);
  - `status` (seleção obrigatória entre `PENDENTE`, `RECEBIDO`, `VENCIDO`, `CANCELADO`, com padrão `PENDENTE` na criação);
  - `metodoId` (seleção opcional a partir de `FinanceiroService.metodos()`);
  - `dataRecebimento` (data opcional no formato `yyyy-MM-dd`);
  - `observacao` (texto opcional, máximo de 240 caracteres).
- **FR-005**: Em modo de criação (`/financeiro/contas-receber/novo`), ao submeter um formulário válido, o componente DEVE chamar `FinanceiroService.createContaReceber(payload)` (`POST /api/v1/FinanceiroContasReceber`), exibir feedback de sucesso via `Toast` e redirecionar para `/financeiro/contas-receber`.
- **FR-006**: Em modo de edição (`/financeiro/contas-receber/:id/editar`), o componente DEVE carregar o registro existente via `FinanceiroService.getContaReceberById(id)` (`GET /api/v1/FinanceiroContasReceber/{id}`), preencher o formulário normalizando datas ISO para `yyyy-MM-dd` e status para o domínio (`PENDENTE`, `RECEBIDO`, `VENCIDO`, `CANCELADO`), e ao submeter um formulário válido DEVE chamar `FinanceiroService.updateContaReceber(id, payload)` (`PUT /api/v1/FinanceiroContasReceber/{id}`), exibir feedback de sucesso e redirecionar para `/financeiro/contas-receber`.
- **FR-007**: A listagem `ContasReceberListaComponent` (`/financeiro/contas-receber`) e a aba "Contas a Receber" no painel `/financeiro` (`FinanceiroDashboard`) DEVEM exibir o botão **"Nova Conta"** apontando para `/financeiro/contas-receber/novo` e uma coluna de **Ações** na tabela de `ContasReceberListaComponent` com link **"Editar"** apontando para `/financeiro/contas-receber/:id/editar`.

### Key Entities *(include if feature involves data)*

- **`FinanceiroContaReceber` / `CreateFinanceiroContaReceberRequest` / `UpdateFinanceiroContaReceberRequest`** (`src/app/core/models/financeiro.ts` e DTOs `CreateFinanceiroContaReceberDTO` / `UpdateFinanceiroContaReceberDTO` no backend): Representa os dados de criação, leitura e atualização de uma conta a receber (`id`, `clienteId`, `descricao`, `valor`, `vencimento`, `status`, `dataRecebimento`, `metodoId`, `observacao`).
- **`Cliente`** (`src/app/core/models/cliente.ts`): Entidade de referência para seleção opcional do pagador/cliente vinculado à conta a receber.
- **`FinanceiroMetodoPagamento`** (`src/app/core/models/financeiro.ts`): Entidade de referência para seleção opcional da forma/método de pagamento.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Em 100% dos acessos autenticados a `/financeiro/contas-receber/novo` e `/financeiro/contas-receber/:id/editar`, o componente `ContasReceberFormComponent` é renderizado no modo correspondente (criação ou edição) (`T-023.1`, `T-023.2`).
- **SC-002**: Em 100% das submissões válidas em modo de criação, o sistema executa `POST /api/v1/FinanceiroContasReceber` com payload tipado e redireciona o usuário para `/financeiro/contas-receber`.
- **SC-003**: Em 100% das submissões válidas em modo de edição, o sistema carrega previamente os dados via `GET /api/v1/FinanceiroContasReceber/{id}`, executa `PUT /api/v1/FinanceiroContasReceber/{id}` com os dados atualizados e redireciona para `/financeiro/contas-receber`.
- **SC-004**: 100% das tentativas de submissão com campos obrigatórios vazios (`descricao`, `valor <= 0`, `vencimento`, `status`) ou acima do limite de 240 caracteres são bloqueadas no cliente antes de qualquer chamada HTTP.
- **SC-005**: Todos os testes unitários do frontend (`npm test -- --no-watch`) — incluindo novas specs para `ContasReceberFormComponent`, `ContasReceberListaComponent` e `FinanceiroService` — e a compilação de produção (`npm run build`) passam com zero erros.

## Assumptions

- Os endpoints backend `GET /api/v1/FinanceiroContasReceber/{id}`, `POST /api/v1/FinanceiroContasReceber` e `PUT /api/v1/FinanceiroContasReceber/{id}` já existem e estão operacionais em `FinanceiroContasReceberController`, validados por `CreateFinanceiroContaReceberValidator` e `UpdateFinanceiroContaReceberValidator` (`Descricao` obrigatória máx. 240, `Status` obrigatório máx. 50, `Observacao` máx. 240), não sendo necessária alteração no banco de dados nem criação de migrations.
- Um único componente `ContasReceberFormComponent` atende tanto à rota de criação (`/financeiro/contas-receber/novo`) quanto à rota de edição (`/financeiro/contas-receber/:id/editar`), determinando o modo de operação pela presença do parâmetro `:id` no `ActivatedRoute`, seguindo exatamente o padrão arquitetural de `ContasPagarFormComponent` (`US-021`).
- O registro dedicado de baixa de recebimento em modal rápido (com suporte a pagamento parcial) será tratado na história subsequente `US-024 — Registrar baixa de conta`.
