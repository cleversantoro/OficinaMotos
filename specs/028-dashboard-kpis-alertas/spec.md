# Especificação de Funcionalidade: Endpoint Agregador de KPIs, Dashboard Real com Gráficos e Alertas de Estoque Mínimo (US-032, US-033 & US-034)

**Feature Branch**: `028-dashboard-kpis-alertas`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-032 — Criar endpoint agregador de KPIs | Prioridade: 🟢 Could | Estimativa: M | Sprint: 6 | Depende: US-001, US-006 | Tasks: [ ] T-032.1 — Criar DashboardController com [Authorize], [ ] T-032.2 — Criar DashboardService.GetKpisAsync(periodo), [ ] T-032.3 — Query OS abertas e do dia, [ ] T-032.4 — Query faturamento mensal, [ ] T-032.5 — Query peças com qtd_atual < qtd_minima || US-033 — Substituir dados mockados do dashboard por dados reais | Prioridade: 🟢 Could | Estimativa: M | Sprint: 6 | Depende: US-032 | Tasks: [ ] T-033.1 — DashboardPrincipalComponent consome GET /api/v1/Dashboard/kpis, [ ] T-033.2 — Gráfico de OS por status, [ ] T-033.3 — Gráfico de faturamento mensal (6 meses), [ ] T-033.4 — Skeleton loading nos cards de KPI || US-034 — Alerta de estoque mínimo no dashboard | Prioridade: 🟢 Could | Estimativa: S | Sprint: 6 | Depende: US-032 | Tasks: [ ] T-034.1 — Incluir pecasCriticas no response de /Dashboard/kpis, [ ] T-034.2 — Card \"Alertas de Estoque\" com link para /estoque/:id"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Expor Endpoint Agregador de KPIs do Dashboard na API (`DashboardController` & `DashboardService`) (Priority: P1)

Como gestor ou colaborador autenticado da oficina, quero consultar um endpoint agregador autenticado (`GET /api/v1/Dashboard/kpis?periodo=...`) (`T-032.1`) apoiado por `DashboardService.GetKpisAsync(periodo)` (`T-032.2`) que consolide em uma única resposta as métricas de Ordens de Serviço abertas, do dia e agrupadas por status (`T-032.3`), o faturamento mensal atual e histórico dos últimos 6 meses (`T-032.4`), e a contagem e listagem de peças críticas com quantidade atual abaixo do estoque mínimo (`qtd_atual < qtd_minima`) (`T-032.5`, `T-034.1`), eliminando a necessidade de múltiplas chamadas de listagem bruta no frontend.

**Why this priority**: Cumpre integralmente a `US-032` (`T-032.1` a `T-032.5`) e a parte de backend da `US-034` (`T-034.1`), fornecendo o contrato RESTful versionado (`/api/v1/Dashboard/kpis`, Princípios I, II e III da Constituição) exigido como dependência direta para `US-033` e `US-034`.

**Independent Test**: Executar `dotnet test` em `oficina-motos-api/OficinaMotos.slnx` validando `DashboardService` e `DashboardController` (incluindo proteção `[Authorize]`, filtro de `periodo`, cálculos de OS abertas/do dia/por status, faturamento mensal de 6 meses e peças com `Quantidade < EstoqueMinimo`).

**Acceptance Scenarios**:

1. **Given** uma requisição HTTP sem token JWT para `GET /api/v1/Dashboard/kpis`, **When** `DashboardController` (`T-032.1`, decorado com `[Authorize]` e rota `api/v1/[controller]`) é acionado, **Then** a API retorna `401 Unauthorized`.
2. **Given** um usuário autenticado e ordens de serviço registradas no sistema, **When** `DashboardService.GetKpisAsync(periodo)` (`T-032.2`, `T-032.3`) é executado, **Then** o DTO retornado inclui o total de OS abertas (status diferente de `Concluida` e `Cancelada`), o total de OS abertas na data de hoje (`osDoDia`), o ticket médio das OS concluídas no período e a distribuição de OS agrupadas por `OrdemServicoStatus` (`osPorStatus`).
3. **Given** pagamentos e ordens de serviço concluídas/recebidas nos últimos 6 meses, **When** `DashboardService.GetKpisAsync(periodo)` (`T-032.4`) calcula o faturamento, **Then** o DTO retorna o valor do faturamento do mês/período selecionado (`faturamentoMensal`) e a série histórica dos últimos 6 meses (`faturamentoUltimosSeisMeses`, contendo mês/ano e valor total faturado em cada mês).
4. **Given** peças cadastradas no estoque (`EstoquePeca`), **When** `DashboardService.GetKpisAsync(periodo)` (`T-032.5`, `T-034.1`) consulta o estoque, **Then** filtra todas as peças ativas onde `Quantidade < EstoqueMinimo`, preenchendo a contagem total (`pecasAbaixoMinimo`) e a coleção `pecasCriticas` (contendo `id`, `codigo`, `descricao`, `quantidadeAtual`, `estoqueMinimo` e `deficit`).

---

### User Story 2 - Consumir KPIs Reais, Exibir Gráficos e Skeleton Loading no Dashboard Angular (Priority: P1)

Como usuário autenticado no frontend Angular, quero que o `DashboardPrincipalComponent` (`DashboardPrincipal`) consuma os dados consolidados de `GET /api/v1/Dashboard/kpis` (`T-033.1`), apresente estados de carregamento com skeleton loading nos cards de KPI enquanto a requisição está em andamento (`T-033.4`), e renderize o gráfico de OS por status (`T-033.2`) e o gráfico de faturamento mensal dos últimos 6 meses (`T-033.3`).

**Why this priority**: Cumpre integralmente a `US-033` (`T-033.1` a `T-033.4`), alinhando o dashboard aos Princípios II e IV da Constituição (centralização de `apiPaths.dashboard.kpis`, Standalone Component reativo com Signals e visualização clara de indicadores operacionais e financeiros).

**Independent Test**: Executar `npm test -- --watch=false` e `npm run build` em `oficina-motos-web/`, verificando que `DashboardPrincipal` realiza chamada a `apiPaths.dashboard.kpis`, exibe os placeholders `.skeleton` nos cards durante `loading()`, e renderiza os gráficos/dados de OS por status e faturamento mensal (6 meses) após a resposta.

**Acceptance Scenarios**:

1. **Given** que o usuário acessa `/dashboard` e a chamada para `GET /api/v1/Dashboard/kpis` ainda não retornou, **When** `DashboardPrincipal` está em estado de carregamento (`loading` ativo), **Then** os cards de KPI exibem elementos visuais de skeleton loading (`T-033.4`) em vez de tela vazia ou dados estáticos.
2. **Given** que `GET /api/v1/Dashboard/kpis` retorna o payload de KPIs (`T-033.1`), **When** `DashboardPrincipal` processa a resposta, **Then** os cards de KPI exibem os indicadores reais (OS abertas e do dia, faturamento mensal, ticket médio e total de peças críticas), o gráfico de OS por status (`T-033.2`) apresenta a distribuição por status (`Aberta`, `EmAndamento`, `AguardandoPeca`, `Concluida`, `Cancelada`) e o gráfico de faturamento mensal (`T-033.3`) apresenta a evolução dos últimos 6 meses.

---

### User Story 3 - Exibir Card "Alertas de Estoque" com Navegação Direta para a Peça Crítica (Priority: P2)

Como responsável pelo estoque ou gerente da oficina, quero visualizar no dashboard um card dedicado "Alertas de Estoque" alimentado pela lista `pecasCriticas` do endpoint `/api/v1/Dashboard/kpis` (`T-034.1`), exibindo cada peça com estoque abaixo do mínimo e um link direto para `/estoque/:id` (`T-034.2`) para rápida reposição ou inspeção.

**Why this priority**: Cumpre integralmente a `US-034` (`T-034.1` e `T-034.2`), permitindo ação imediata sobre rupturas de estoque diretamente a partir da tela inicial do sistema.

**Independent Test**: Verificar via teste unitário do componente `DashboardPrincipal` (`dashboard-principal.spec.ts`) que, quando `pecasCriticas` contém itens no retorno de `/api/v1/Dashboard/kpis`, o card "Alertas de Estoque" renderiza a lista de peças críticas com atributos `routerLink` apontando para `['/estoque', peca.id]` (ou `/estoque/:id`).

**Acceptance Scenarios**:

1. **Given** que o response de `GET /api/v1/Dashboard/kpis` inclui `pecasCriticas` com peças onde `quantidadeAtual < estoqueMinimo` (`T-034.1`), **When** o dashboard é renderizado, **Then** o card "Alertas de Estoque" (`T-034.2`) lista as peças críticas informando descrição/código, quantidade atual vs. mínima e disponibiliza em cada item um link clicável para `/estoque/:id`.
2. **Given** que nenhuma peça está abaixo do estoque mínimo (`pecasCriticas` vazio), **When** o dashboard é renderizado, **Then** o card "Alertas de Estoque" exibe estado informativo indicando que o estoque está regularizado, mantendo atalho geral para `/estoque`.

---

### Edge Cases

- **Parâmetro `periodo` opcional ou não informado em `GET /api/v1/Dashboard/kpis`**: Quando `periodo` não for informado na query string (ou for vazio), `DashboardService.GetKpisAsync(periodo)` deve assumir o mês corrente (`"mes_atual"` / `"30d"`, ou `YYYY-MM` se informado nesse formato) como padrão sem lançar erro `400 BadRequest`.
- **Cálculo de faturamento mensal (`T-032.4`) com múltiplas fontes no domínio**: O faturamento mensal e dos últimos 6 meses deve considerar os valores faturados no mês (a partir dos itens de `OrdemServico` com status `Concluida` na data de conclusão/abertura, ou `OrdemServicoPagamento` / `FinanceiroContaReceber` pagos quando aplicável, evitando dupla contagem e tratando meses sem movimentação com valor `0` para garantir sempre os 6 pontos no gráfico de 6 meses).
- **Compatibilidade retroativa do seletor/classe `DashboardPrincipal`**: O arquivo `app.routes.ts` importa `DashboardPrincipal` de `./features/dashboard/pages/dashboard-principal/dashboard-principal`. Podemos exportar `DashboardPrincipalComponent` e manter `export { DashboardPrincipalComponent as DashboardPrincipal }` para total compatibilidade com as rotas existentes e com a nomenclatura da task `T-033.1`.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O backend DEVE conter `DashboardController` em `oficina-motos-api/src/OficinaMotos.API/Controllers/Dashboard/DashboardController.cs` decorado com `[ApiController]`, `[Route("api/v1/[controller]")]` e `[Authorize]`, expondo `GET /api/v1/Dashboard/kpis` com parâmetro opcional de query `[FromQuery] string? periodo = null` (`T-032.1`).
- **FR-002**: A camada Application DEVE definir `IDashboardService` e `DashboardService` (registrado via DI em `AddApplication()`) implementando `Task<DashboardKpisResponseDTO> GetKpisAsync(string? periodo = null)` (`T-032.2`).
- **FR-003**: `DashboardService.GetKpisAsync(periodo)` DEVE consultar Ordens de Serviço (`OrdemServico`) calculando: total de OS abertas (status != `Concluida` e != `Cancelada`), total de OS abertas no dia atual (`osDoDia`), ticket médio e agrupamento de contagem de OS por status (`osPorStatus`) (`T-032.3`).
- **FR-004**: `DashboardService.GetKpisAsync(periodo)` DEVE calcular o faturamento mensal do período (`faturamentoMensal`) e a série histórica dos últimos 6 meses (`faturamentoMensalHistorico` / `faturamentoUltimosSeisMeses`, sempre com 6 meses consecutivos até o mês de referência) (`T-032.4`).
- **FR-005**: `DashboardService.GetKpisAsync(periodo)` DEVE consultar as peças de estoque (`EstoquePeca`) onde `Quantidade < EstoqueMinimo` (`qtd_atual < qtd_minima`), retornando a contagem (`pecasAbaixoMinimo`) e a coleção `pecasCriticas` no `DashboardKpisResponseDTO` (`T-032.5`, `T-034.1`).
- **FR-006**: O arquivo `oficina-motos-web/src/app/core/services/api-paths.ts` DEVE registrar `dashboard: { kpis: '/api/v1/Dashboard/kpis' }` e um serviço Angular (`DashboardService` em `core/services/dashboard.service.ts`) DEVE encapsular a chamada `getKpis(periodo?: string)` (`T-033.1`).
- **FR-007**: O componente `DashboardPrincipalComponent` (`DashboardPrincipal`) em `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/` DEVE consumir `GET /api/v1/Dashboard/kpis` via `DashboardService` (`T-033.1`) e exibir skeleton loading nos cards de KPI enquanto os dados estão sendo carregados (`T-033.4`).
- **FR-008**: O `DashboardPrincipalComponent` DEVE exibir o gráfico de OS por status (`T-033.2`) e o gráfico de faturamento mensal dos últimos 6 meses (`T-033.3`) alimentados pelos dados reais retornados pelo endpoint `/api/v1/Dashboard/kpis`.
- **FR-009**: O `DashboardPrincipalComponent` DEVE exibir um card "Alertas de Estoque" listando os itens de `pecasCriticas` retornados pelo endpoint, onde cada item possui um link de navegação (`routerLink`) para `/estoque/:id` (`T-034.2`).

### Key Entities *(include if feature involves data)*

- **`DashboardKpisResponseDTO`** (`OficinaMotos.Application.DTOs.Responses.Dashboard`): DTO agregador contendo `osAbertas`, `osDoDia`, `faturamentoMensal`, `ticketMedio`, `pecasAbaixoMinimo`, `osPorStatus`, `faturamentoUltimosSeisMeses` (6 meses) e `pecasCriticas`.
- **`DashboardPecaCriticaDTO`** (`OficinaMotos.Application.DTOs.Responses.Dashboard`): Item de alerta de estoque contendo `id`, `codigo`, `descricao`, `quantidadeAtual`, `estoqueMinimo` e `unidade`.
- **`DashboardController` & `DashboardService`** (`OficinaMotos.API` / `OficinaMotos.Application`): Endpoint e serviço de aplicação responsáveis pela agregação de KPIs.
- **`DashboardPrincipalComponent` (`DashboardPrincipal`) & `DashboardService` (Angular)**: Componente standalone e serviço HTTP do frontend responsáveis por exibir KPIs com skeleton loading, gráficos de OS por status e faturamento (6 meses), e card "Alertas de Estoque".

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `dotnet build` e `dotnet test` em `oficina-motos-api/OficinaMotos.slnx` compilam sem erros e aprovam 100% dos testes unitários e de integração, incluindo testes dedicados para `DashboardService.GetKpisAsync` e `DashboardController` (`T-032.1` a `T-032.5`, `T-034.1`).
- **SC-002**: `npm run build` e `npm test -- --watch=false` em `oficina-motos-web/` compilam sem erros e aprovam 100% das specs, incluindo testes unitários para `DashboardService` e `DashboardPrincipalComponent` validando consumo de `/api/v1/Dashboard/kpis`, skeleton loading nos cards de KPI, gráfico de OS por status, gráfico de faturamento mensal (6 meses) e card "Alertas de Estoque" com links `/estoque/:id` (`T-033.1` a `T-033.4`, `T-034.2`).

## Assumptions

- O cálculo de faturamento mensal (`T-032.4`) considera a soma do valor total dos itens (`OrdemServicoItem.Total`, ou `Quantidade * ValorUnitario` caso `Total` seja zero) das Ordens de Serviço com `Status == OrdemServicoStatus.Concluida` cuja data de referência (`DataConclusao ?? DataAbertura`) pertença ao mês avaliado, somado a eventuais pagamentos avulsos ou mantendo consistência direta com o modelo de OS do domínio.
- A query de peças críticas (`T-032.5` e `T-034.1`) utiliza estritamente a regra `Quantidade < EstoqueMinimo` (`qtd_atual < qtd_minima`) conforme especificado na task `T-032.5`.
