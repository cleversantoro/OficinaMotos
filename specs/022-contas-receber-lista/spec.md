# Especificação de Funcionalidade: Listar Contas a Receber (US-022)

**Feature Branch**: `022-contas-receber-lista`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-022 — Listar contas a receber | Prioridade: 🟡 Should | Estimativa: S | Sprint: 4 | Depende: US-001 | Tasks: [ ] T-022.1 — Criar ContasReceberListaComponent, [ ] T-022.2 — Registrar rota /financeiro/contas-receber, [ ] T-022.3 — Coluna 'OS Origem' com link para /ordens/:id"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Consultar Lista de Contas a Receber em `/financeiro/contas-receber` (Priority: P1)

Como gestor, analista financeiro ou administrador da oficina, quero acessar a página dedicada `/financeiro/contas-receber` (`ContasReceberListaComponent`) para visualizar todos os recebíveis cadastrados (tanto gerados automaticamente por pagamentos de Ordens de Serviço quanto lançados manualmente), incluindo vencimento, cliente, descrição, OS de origem, método de pagamento, status, data de recebimento, valor, paginação, filtros e indicadores resumidos (Total a Receber em Aberto, Total em Atraso e Total Recebido).

**Why this priority**: A listagem dedicada (`T-022.1` e `T-022.2`) é a base do fluxo de Contas a Receber no épico Financeiro (`EPIC-04`), dando visibilidade às entradas de caixa e preparando o terreno para criação/edição manual (`US-023`) e baixa de recebimentos (`US-024`).

**Independent Test**: Pode ser testado navegando para `/financeiro/contas-receber` (ou clicando no atalho na aba Contas a Receber do painel `/financeiro`), verificando que a página consome `GET /api/v1/FinanceiroContasReceber`, resolve os nomes dos clientes e métodos de pagamento quando disponíveis e exibe os KPIs e a tabela paginada.

**Acceptance Scenarios**:

1. **Given** que o usuário autenticado acessa `/financeiro/contas-receber`, **When** a tela é carregada, **Then** o sistema consulta `GET /api/v1/FinanceiroContasReceber`, exibe os cards de resumo financeiro (A Receber em Aberto, Recebíveis Vencidos e Total Recebido) e apresenta a tabela paginada ordenada por data de vencimento.
2. **Given** que uma conta a receber possui `clienteId` ou `metodoId` vinculado, **When** a linha é renderizada na tabela, **Then** o sistema apresenta o nome de exibição do cliente (ou fallback `Cliente #id` / `-`) e o nome do método de pagamento (ou `-` quando ausente).
3. **Given** que existem filtros por status (Todos, Pendente, Vencido, Recebido, Cancelado), período de vencimento (`dataInicio` e `dataFim`) e busca textual (descrição, observação, cliente ou número da OS), **When** o usuário aplica ou limpa os filtros, **Then** a tabela e a paginação são atualizadas reativamente sem recarregar a página.

---

### User Story 2 - Visualizar Coluna "OS Origem" com Link Direto para `/ordens/:id` (Priority: P1)

Como responsável financeiro ou recepcionista conferindo os recebíveis da oficina, quero visualizar na tabela de Contas a Receber uma coluna específica **"OS Origem"** que identifique a Ordem de Serviço geradora do recebível e disponibilize um link clicável para `/ordens/:id`, permitindo auditar rapidamente os serviços, peças e pagamentos da OS correspondente.

**Why this priority**: A coluna **"OS Origem"** com link para `/ordens/:id` (`T-022.3`) é requisito explícito da `US-022` e conecta o módulo Financeiro (`EPIC-04`) ao módulo de Ordens de Serviço (`EPIC-02`, integrado na `US-012`).

**Independent Test**: Com uma conta a receber originada de uma OS (seja via `ordemServicoId` explícito ou via referência textual `OS #123` gerada por `OrdemServicoService.RegistrarPagamentoAsync` na descrição/observação), acessar `/financeiro/contas-receber`, localizar a coluna **"OS Origem"** e verificar que o link exibe `OS #123` apontando para `/ordens/123`; para contas manuais sem vínculo com OS, verificar que a coluna exibe `-`.

**Acceptance Scenarios**:

1. **Given** uma conta a receber associada a uma Ordem de Serviço (via propriedade `ordemServicoId` ou padrão textual `OS #<id>` em `descricao`/`observacao`, como `"Pagamento OS #45 - PIX"` ou `"Referente à OS #45"`), **When** a linha é renderizada em `/financeiro/contas-receber`, **Then** a coluna **"OS Origem"** exibe um link estilizado `OS #45` apontando para `/ordens/45`.
2. **Given** que o usuário clica no link `OS #:id` na coluna **"OS Origem"**, **When** a navegação é acionada, **Then** o roteador direciona o usuário para a página de detalhe da Ordem de Serviço (`/ordens/:id`).
3. **Given** uma conta a receber avulsa/manual que não possui `ordemServicoId` nem referência `OS #<id>` na descrição ou observação, **When** a linha é renderizada na tabela, **Then** a coluna **"OS Origem"** exibe `-` sem link quebrado.

---

### User Story 3 - Navegar entre o Painel Financeiro e a Lista de Contas a Receber (Priority: P2)

Como usuário navegando pelo módulo Financeiro (`/financeiro`), quero acessar diretamente a listagem completa `/financeiro/contas-receber` a partir da aba "Contas a Receber" no painel financeiro e poder retornar ao painel principal a qualquer momento.

**Why this priority**: Garante a descoberta e a fluidez de navegação para a nova rota `/financeiro/contas-receber` (`T-022.2`) dentro do módulo Financeiro, espelhando a experiência de Contas a Pagar (`US-020`).

**Independent Test**: Acessar `/financeiro`, selecionar a aba **"Contas a Receber"**, clicar em **"Ver listagem completa"** e confirmar o redirecionamento para `/financeiro/contas-receber`.

**Acceptance Scenarios**:

1. **Given** que o usuário está na aba "Contas a Receber" em `/financeiro`, **When** clica em **"Ver listagem completa"**, **Then** o roteador navega para `/financeiro/contas-receber`.

---

### Edge Cases

- **Resolução do identificador da OS de origem (`T-022.3`)**: Como a tabela `fin_contas_receber` e o `OrdemServicoService.RegistrarPagamentoAsync` (`US-012`) registram o vínculo da OS na descrição (`"Pagamento OS #{id} - ..."`) e na observação (`"Referente à OS #{id}"` ou `"(OS #{id})"`), o método `getOrdemServicoId(conta)` deve priorizar `conta.ordemServicoId` (caso presente e `> 0`) e, em seguida, extrair o número da OS via expressão regular `/\bOS\s*#?\s*(\d+)\b/i` a partir de `conta.descricao` ou `conta.observacao`.
- **Normalização de status de recebíveis**: O banco de dados e o serviço de OS utilizam variações como `'PENDENTE'`, `'ABERTO'`, `'RECEBIDO'`, `'PAGO'`, `'LIQUIDADO'`, `'VENCIDO'`, `'CANCELADO'`. A normalização deve ser *case-insensitive*, mapeando `'RECEBIDO'`, `'PAGO'` e `'LIQUIDADO'` para o status efetivo `RECEBIDO`, e tratando contas em aberto com `vencimento < hoje` (sem `dataRecebimento`) como `VENCIDO`.
- **Falha em consultas auxiliares (`Clientes` ou `MetodosPagamento`)**: Caso `ClientesService.list()` ou `FinanceiroService.metodos()` falhem, a listagem principal de contas a receber deve ser exibida normalmente utilizando o fallback `Cliente #id` ou `-`.
- **Intervalo de datas invertido (`dataInicio > dataFim`)**: Exibe mensagem orientativa informando que a data inicial não pode ser posterior à data final e permite limpar os filtros.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE disponibilizar o componente standalone `ContasReceberListaComponent` em `src/app/features/financeiro/pages/contas-receber-lista/` com `ChangeDetectionStrategy.OnPush` e estado reativo em Angular Signals (`T-022.1`).
- **FR-002**: O sistema DEVE registrar a rota `/financeiro/contas-receber` em `src/app/app.routes.ts` dentro do escopo protegido por `authGuard` (`MainLayout`), associada ao `ContasReceberListaComponent` (`T-022.2`).
- **FR-003**: O `ContasReceberListaComponent` DEVE consumir `FinanceiroService.contasReceber()` (`GET /api/v1/FinanceiroContasReceber`) e carregar de forma resiliente os clientes (`ClientesService.list()`) e métodos de pagamento (`FinanceiroService.metodos()`).
- **FR-004**: A tabela de `ContasReceberListaComponent` DEVE exibir as colunas:
  1. **Vencimento** (`dd/MM/yyyy` + indicador de atraso quando vencida);
  2. **Cliente** (nome de exibição do cliente ou fallback `Cliente #id` / `-`);
  3. **Descrição** (`descricao` e `observacao` secundária);
  4. **OS Origem** (`T-022.3`): link `[routerLink]="['/ordens', osId]"` exibindo `OS #{{ osId }}` quando houver OS vinculada, ou `-` quando não houver;
  5. **Método** (nome do método de pagamento ou `-`);
  6. **Status** (badge semântico para `Pendente`, `Vencido`, `Recebido`, `Cancelado`);
  7. **Recebimento** (`dataRecebimento | date:'dd/MM/yyyy'` ou `-`);
  8. **Valor** (`R$ 1.234,56`).
- **FR-005**: O componente DEVE extrair o identificador da OS de origem (`getOrdemServicoId(conta)`) verificando `conta.ordemServicoId` (opcional) ou o padrão `/\bOS\s*#?\s*(\d+)\b/i` em `conta.descricao` e `conta.observacao`, gerando o link navegável para `/ordens/:id` (`T-022.3`).
- **FR-006**: O componente DEVE disponibilizar filtros reativos por **Status** (`TODOS`, `PENDENTE`, `VENCIDO`, `RECEBIDO`, `CANCELADO`), **Período de Vencimento** (`dataInicio` e `dataFim`) e **Busca textual** (descrição, observação, cliente ou número da OS), além de cards de KPIs resumidos e paginação.
- **FR-007**: O painel `/financeiro` (`FinanceiroDashboard`) DEVE disponibilizar na aba "Contas a Receber" o atalho `"Ver listagem completa"` apontando para `/financeiro/contas-receber`.

### Key Entities *(include if feature involves data)*

- **`FinanceiroContaReceber`** (`src/app/core/models/financeiro.ts` / tabela `fin_contas_receber`): Representa um direito de recebimento da oficina (`id`, `clienteId`, `descricao`, `valor`, `vencimento`, `status`, `dataRecebimento`, `metodoId`, `observacao`, além de referência opcional/extraída para a Ordem de Serviço de origem).
- **`Cliente`** (`src/app/core/models/cliente.ts` / tabela `cad_clientes`): Entidade de referência utilizada para resolver o nome do cliente associado a `clienteId`.
- **`OrdemServico`** (`src/app/core/models/ordem-servico.ts` / tabela `os_ordens_servico`): Entidade de destino do link `/ordens/:id` na coluna **"OS Origem"**.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Em 100% dos acessos autenticados à rota `/financeiro/contas-receber`, o componente `ContasReceberListaComponent` é exibido e lista os registros retornados por `GET /api/v1/FinanceiroContasReceber` (`T-022.1`, `T-022.2`).
- **SC-002**: Em 100% das contas a receber que possuam vínculo com Ordem de Serviço (`ordemServicoId` ou referência `OS #<id>` em `descricao`/`observacao`), a coluna **"OS Origem"** renderiza um link funcional apontando para `/ordens/:id` (`T-022.3`).
- **SC-003**: Em 100% das contas a receber manuais sem vínculo com OS, a coluna **"OS Origem"** renderiza `-` sem gerar links inválidos.
- **SC-004**: Todos os testes unitários do frontend (`npm test -- --no-watch`) e a compilação de produção (`npm run build`) executam com 100% de aprovação e zero erros.

## Assumptions

- O endpoint backend `GET /api/v1/FinanceiroContasReceber` (`FinanceiroContasReceberController`) e o método `FinanceiroService.contasReceber()` já existem e retornam a coleção de `FinanceiroContaReceber`, não sendo necessária alteração no schema MySQL (`fin_contas_receber`) nem criação de migration SQL para esta história.
- A criação e edição manual de contas a receber será tratada na história subsequente `US-023 — Criar e editar conta a receber manual`, e a baixa de recebimento na `US-024 — Registrar baixa de conta`.
