# Especificação de Funcionalidade: Listar Contas a Pagar (US-020)

**Feature Branch**: `020-contas-pagar-lista`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-020 — Listar contas a pagar | Prioridade: 🟡 Should | Estimativa: S | Sprint: 4 | Depende: US-001 | Tasks: [ ] T-020.1 — Criar ContasPagarListaComponent, [ ] T-020.2 — Registrar rota /financeiro/contas-pagar, [ ] T-020.3 — Filtros por status e período, [ ] T-020.4 — Destaque visual para contas vencidas"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Consultar Lista de Contas a Pagar em `/financeiro/contas-pagar` (Priority: P1)

Como gestor, analista financeiro ou administrador da oficina, quero acessar a página dedicada `/financeiro/contas-pagar` (`ContasPagarListaComponent`) para visualizar todas as obrigações financeiras (contas a pagar) cadastradas com seus respectivos dados (vencimento, fornecedor, descrição, método de pagamento, status, valor e data de pagamento), paginação e indicadores resumidos (total em aberto, total vencido e total pago no recorte listado).

**Why this priority**: A listagem dedicada (`T-020.1` e `T-020.2`) é a base do módulo de Contas a Pagar no épico Financeiro (`EPIC-04`), permitindo visibilidade completa sobre os compromissos financeiros da oficina e servindo de ponto de partida para criação, edição (`US-021`) e baixa de pagamentos (`US-024`).

**Independent Test**: Pode ser testado navegando diretamente para `/financeiro/contas-pagar` (ou a partir de um atalho no painel `/financeiro`), verificando que a página carrega os registros reais do endpoint `GET /api/v1/FinanceiroContasPagar`, resolve os nomes de fornecedores e métodos de pagamento quando disponíveis e exibe a tabela paginada com os totalizadores.

**Acceptance Scenarios**:

1. **Given** que o usuário autenticado acessa `/financeiro/contas-pagar`, **When** a tela é carregada, **Then** o sistema consulta as contas a pagar registradas (`GET /api/v1/FinanceiroContasPagar`), exibe os cards de resumo financeiro (Total em Aberto, Total Vencido e Total Pago) e apresenta a tabela paginada ordenada por data de vencimento.
2. **Given** que uma conta a pagar possui `fornecedorId` ou `metodoId` vinculado, **When** a linha é renderizada na listagem, **Then** o sistema apresenta o nome/razão social do fornecedor (ou identificador de fallback `#id` caso não encontrado) e o nome do método de pagamento (ou `-` quando ausente).
3. **Given** que não existem contas a pagar cadastradas no sistema, **When** a consulta retorna uma lista vazia, **Then** a tela exibe um estado vazio informativo ("Nenhuma conta a pagar cadastrada.") sem erros visuais.

---

### User Story 2 - Filtrar Contas a Pagar por Status, Período de Vencimento e Busca Textual (Priority: P1)

Como usuário do setor financeiro, quero filtrar a listagem de contas a pagar por **status** (Todos, Pendente/Em Aberto, Vencido, Pago, Cancelado), por **período de vencimento** (data inicial e data final) e por **termo de busca** (descrição ou fornecedor), para localizar rapidamente compromissos específicos ou analisar os desembolsos previstos em uma janela de datas.

**Why this priority**: O filtro por status e período (`T-020.3`) é requisito direto da história `US-020` e indispensável para a rotina diária de controle de caixa e programação de pagamentos.

**Independent Test**: Na tela `/financeiro/contas-pagar`, selecionar o filtro de status (ex.: `PENDENTE` ou `VENCIDO`), informar um intervalo de datas de vencimento (`dataInicio` e `dataFim`) e confirmar que apenas as contas que satisfazem simultaneamente os critérios selecionados permanecem visíveis, reiniciando a paginação para a primeira página.

**Acceptance Scenarios**:

1. **Given** que existem contas com diferentes status (`PENDENTE`, `PAGO`, `CANCELADO` e contas vencidas), **When** o usuário seleciona um status específico no filtro de status, **Then** a listagem exibe apenas os registros correspondentes ao status selecionado (considerando como "Vencido" tanto contas com status explícito `VENCIDO` quanto contas `PENDENTE`/`ABERTO` cuja data de vencimento já ultrapassou a data atual) e retorna para a primeira página.
2. **Given** que o usuário informa uma **Data Inicial** e/ou **Data Final** no filtro de período de vencimento, **When** os filtros são aplicados, **Then** a listagem exibe apenas as contas cuja data de `vencimento` esteja dentro do intervalo `[dataInicio, dataFim]` (comparação inclusiva por dia civil).
3. **Given** que existem filtros ativos (status, período ou busca textual), **When** o usuário aciona a opção **"Limpar filtros"**, **Then** todos os campos de filtro são restaurados ao estado padrão e a listagem volta a exibir todas as contas a pagar.

---

### User Story 3 - Identificar Contas Vencidas com Destaque Visual (Priority: P1)

Como responsável financeiro, quero que todas as contas a pagar vencidas e ainda não quitadas recebam um destaque visual imediato na tabela (badge de alerta e estilização diferenciada na linha/data de vencimento), para identificar rapidamente pendências em atraso e evitar juros ou bloqueios de fornecedores.

**Why this priority**: O destaque visual para contas vencidas (`T-020.4`) é requisito explícito da `US-020` e reduz o risco operacional de inadimplência com fornecedores.

**Independent Test**: Com uma conta a pagar cujo status seja `VENCIDO` ou cujo status seja `PENDENTE`/`ABERTO` e a data de `vencimento` seja anterior à data atual (`hoje`), acessar `/financeiro/contas-pagar` e verificar que a linha e o badge de status exibem o destaque visual de conta vencida (severidade de erro/alerta e indicador claro de atraso).

**Acceptance Scenarios**:

1. **Given** uma conta a pagar com status explícito `VENCIDO` ou com status em aberto (`PENDENTE` / `ABERTO`) e data de `vencimento` anterior ao dia atual (sem `dataPagamento`), **When** a conta é exibida na listagem, **Then** o sistema aplica classe de destaque na linha (`row-vencida`), destaca a data de vencimento e exibe o badge visual **"Vencido"** em vermelho/alerta.
2. **Given** uma conta a pagar com status `PAGO` ou `CANCELADO` cuja data de vencimento seja anterior ao dia atual, **When** a conta é exibida na listagem, **Then** ela **não** recebe destaque de conta vencida, exibindo o badge correspondente a **"Pago"** (verde) ou **"Cancelado"** (neutro/cinza).
3. **Given** uma conta a pagar em aberto (`PENDENTE` / `ABERTO`) com data de `vencimento` igual ou posterior ao dia atual, **When** a conta é exibida na listagem, **Then** ela exibe o badge **"Pendente"** (azul/aviso) sem o alerta visual de atraso.

---

### User Story 4 - Navegar entre o Painel Financeiro e a Lista de Contas a Pagar (Priority: P2)

Como usuário navegando pelo módulo Financeiro (`/financeiro`), quero acessar facilmente a tela completa `/financeiro/contas-pagar` e poder voltar ao painel principal do financeiro ou iniciar ações futuras (como nova conta a pagar).

**Why this priority**: Garante a descoberta e a fluidez de navegação para a nova rota `/financeiro/contas-pagar` (`T-020.2`) dentro da experiência do módulo Financeiro.

**Independent Test**: Acessar `/financeiro`, clicar no link/botão **"Ver todas / Gerenciar Contas a Pagar"** e confirmar o redirecionamento para `/financeiro/contas-pagar`.

**Acceptance Scenarios**:

1. **Given** que o usuário está no painel `/financeiro`, **When** aciona o atalho para a listagem completa de contas a pagar, **Then** o roteador navega para `/financeiro/contas-pagar`.

---

### Edge Cases

- **Inconsistência de caixa/capitalização no campo `status` vindo do banco/API**: O banco (`fin_contas_pagar`) armazena valores como `'PENDENTE'`, `'PAGO'`, `'CANCELADO'`, enquanto mocks ou cadastros podem usar `'pendente'`, `'aberto'`, `'vencido'`, `'pago'`. A normalização de status deve ser *case-insensitive* (`toUpperCase()` / `toLowerCase()`).
- **Conta com status `PENDENTE` no banco mas data de vencimento no passado**: Mesmo que o campo `status` persistido na tabela `fin_contas_pagar` ainda seja `'PENDENTE'`, se `vencimento < hoje` (considerando o início do dia local) e a conta não estiver paga/cancelada, o sistema deve tratá-la visualmente e no filtro como **Vencida**.
- **Intervalo de datas invertido (`dataInicio > dataFim`)**: Caso o usuário selecione uma data inicial posterior à data final, o sistema deve exibir mensagem orientativa ou estado vazio amigável informando que nenhum registro corresponde ao período selecionado, permitindo limpar os filtros com um clique.
- **Falha na comunicação com a API (`GET /api/v1/FinanceiroContasPagar`)**: Se a requisição principal falhar, o estado de carregamento (`loading`) deve ser encerrado e uma mensagem de erro clara com botão **"Tentar novamente"** deve ser apresentada. Falhas nas consultas auxiliares (fornecedores ou métodos de pagamento) não devem impedir a exibição das contas a pagar (usando fallback `#fornecedorId` ou `-`).
- **Acesso não autenticado**: A rota `/financeiro/contas-pagar` deve estar protegida sob o `authGuard` no `MainLayout`, redirecionando usuários não autenticados para `/login`.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE disponibilizar o componente standalone `ContasPagarListaComponent` em `src/app/features/financeiro/pages/contas-pagar-lista/` (`T-020.1`).
- **FR-002**: O sistema DEVE registrar a rota `/financeiro/contas-pagar` em `src/app/app.routes.ts` dentro do escopo protegido por `authGuard` (`MainLayout`), associada ao `ContasPagarListaComponent` (`T-020.2`).
- **FR-003**: O `ContasPagarListaComponent` DEVE consumir `FinanceiroService.contasPagar()` (`GET /api/v1/FinanceiroContasPagar`) para buscar a lista de contas a pagar, bem como carregar os cadastros auxiliares de fornecedores (`FornecedoresService.list()`) e métodos de pagamento (`FinanceiroService.metodos()`) para exibição legível dos nomes na tabela.
- **FR-004**: A listagem DEVE exibir para cada conta a pagar: Data de Vencimento (`dd/MM/yyyy`), Fornecedor (nome/razão social ou fallback `Fornecedor #id` / `-`), Descrição (`descricao`), Método de Pagamento (nome ou `-`), Data de Pagamento (`dd/MM/yyyy` ou `-`), Status normalizado com badge semântico e Valor formatado em moeda brasileira (`R$ 1.234,56`).
- **FR-005**: O componente DEVE disponibilizar filtro por **Status** (`T-020.3`) com as opções: `Todos`, `Pendente` (em aberto e dentro do prazo), `Vencido` (status `VENCIDO` ou em aberto com `vencimento < hoje`), `Pago` e `Cancelado`.
- **FR-006**: O componente DEVE disponibilizar filtro por **Período de Vencimento** (`T-020.3`) composto por Data Inicial (`dataInicio`) e Data Final (`dataFim`), filtrando inclusivamente as contas cuja data de `vencimento` esteja no intervalo informado, além de busca textual por descrição ou fornecedor e ação para limpar filtros.
- **FR-007**: O componente DEVE aplicar **destaque visual para contas vencidas** (`T-020.4`), identificando qualquer conta não paga e não cancelada cujo status seja `VENCIDO` ou cuja data de `vencimento` seja anterior ao dia corrente, aplicando estilização de alerta na linha da tabela, destaque na data de vencimento e badge visual **"Vencido"** com severidade de perigo/erro.
- **FR-008**: O componente DEVE apresentar indicadores resumidos (KPIs) calculados reativamente sobre os dados carregados: **Total em Aberto (a pagar)**, **Total Vencido** (com destaque de alerta quando maior que zero) e **Total Pago**.
- **FR-009**: O componente DEVE fornecer paginação e estados claros de carregamento (`loading`), erro de consulta (com opção de recarregar) e lista vazia (diferenciando quando não há cadastros de quando nenhum item atende aos filtros ativos).
- **FR-010**: O painel `/financeiro` (`FinanceiroDashboard`) DEVE oferecer navegação direta para `/financeiro/contas-pagar` para permitir o acesso integrado à nova tela a partir do menu/painel financeiro.

### Key Entities *(include if feature involves data)*

- **`FinanceiroContaPagar`** (`src/app/core/models/financeiro.ts` / tabela `fin_contas_pagar`): Representa uma obrigação financeira da oficina, contendo `id`, `fornecedorId`, `descricao`, `valor`, `vencimento`, `status` (`PENDENTE`, `PAGO`, `VENCIDO`, `CANCELADO`), `dataPagamento`, `metodoId` e `observacao`.
- **`Fornecedor`** (`src/app/core/models/fornecedor.ts` / tabela `cad_fornecedores`): Entidade de referência do contexto de Cadastro utilizada para exibir o nome/razão social do credor associado a `fornecedorId`.
- **`FinanceiroMetodoPagamento`** (`src/app/core/models/financeiro.ts` / tabela `fin_metodos_pagamento`): Método de pagamento associado a `metodoId` (ex.: PIX, Boleto, Cartão de Crédito, Dinheiro, TED).

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Em 100% dos acessos autenticados à rota `/financeiro/contas-pagar`, o componente `ContasPagarListaComponent` é exibido e lista os registros retornados por `GET /api/v1/FinanceiroContasPagar` (`T-020.1`, `T-020.2`).
- **SC-002**: Em 100% dos cenários de teste de filtragem, os filtros combinados de status e período de vencimento (`dataInicio` / `dataFim`) restringem imediatamente os itens exibidos na tabela sem necessidade de recarregar a página (`T-020.3`).
- **SC-003**: Em 100% dos registros cujo status seja `VENCIDO` ou cujo status seja `PENDENTE`/`ABERTO` com data de vencimento anterior à data atual, a linha e o badge de status exibem o destaque visual de conta vencida (`T-020.4`).
- **SC-004**: Todos os testes unitários do frontend (`npm test -- --no-watch`) e a compilação de produção (`npm run build`) executam com 100% de aprovação e zero erros.

## Assumptions

- O endpoint backend `GET /api/v1/FinanceiroContasPagar` (`FinanceiroContasPagarController`) e o método `FinanceiroService.contasPagar()` já existem e retornam a coleção de `FinanceiroContaPagar`, não sendo necessária nenhuma alteração no schema MySQL (`fin_contas_pagar`) nem criação de migration SQL para esta história.
- Como o volume de contas a pagar retornado por `GET /api/v1/FinanceiroContasPagar` segue o padrão atual do repositório genérico da API (listagem completa em memória no cliente, conforme ADR-004 em `governance/decisoes.md`), a filtragem por status, período e texto e a paginação são processadas reativamente no frontend via Angular Signals (`computed`).
- As ações de criação/edição de conta a pagar (`US-021`) e de baixa de pagamento (`US-024`) pertencem a histórias subsequentes da Sprint 4; portanto, nesta história (`US-020`) o foco é exclusivamente a visualização, filtragem, paginação e destaque visual de vencimento.
