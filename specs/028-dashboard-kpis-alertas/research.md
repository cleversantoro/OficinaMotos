# Research & Technical Decisions: Endpoint Agregador de KPIs, Dashboard Real com Gráficos e Alertas de Estoque Mínimo (US-032, US-033 & US-034)

**Feature Branch**: `028-dashboard-kpis-alertas`  
**Date**: 2026-11-16

---

## 1. Estrutura do `DashboardController` e `DashboardService.GetKpisAsync(periodo)` (`T-032.1`, `T-032.2`)

- **Decision**:
  - Criar `IDashboardService` em `oficina-motos-api/src/OficinaMotos.Application/Interfaces/Dashboard/IDashboardService.cs` com a assinatura `Task<DashboardKpisResponseDTO> GetKpisAsync(string? periodo = null)`.
  - Criar `DashboardService` em `oficina-motos-api/src/OficinaMotos.Application/Services/Dashboard/DashboardService.cs` injetando `IOrdemServicoRepository` e `IEstoquePecaRepository` (além de `IFinanceiroContaPagarRepository` e `IFinanceiroContaReceberRepository` se necessário para enriquecimento, mantendo foco absoluto nas queries de `T-032.3`, `T-032.4` e `T-032.5`) e registrá-lo em `OficinaMotos.Application.IoC.DependencyInjection.AddApplication()`.
  - Criar `DashboardController` em `oficina-motos-api/src/OficinaMotos.API/Controllers/Dashboard/DashboardController.cs` com `[ApiController]`, `[Route("api/v1/[controller]")]` e `[Authorize]`, expondo `[HttpGet("kpis")]` (`GET /api/v1/Dashboard/kpis?periodo=...`).
- **Rationale**:
  - Respeita estritamente a Clean Architecture do projeto (Controllers na camada `API`, regras de agregação e DTOs na camada `Application`, acesso a dados via interfaces de repositório de `Domain` implementadas em `Infrastructure`).
  - O parâmetro `periodo` aceita formatos como `"YYYY-MM"` (ex.: `"2026-11"`), `"mes_atual"`, `"hoje"` ou `null` (default: mês corrente em UTC/local).
- **Alternatives considered**:
  - Consultar `OficinaContext` diretamente no Controller: rejeitado por violar o Princípio I da Constituição (nenhuma lógica de negócio ou query direta em Controllers).

---

## 2. Cálculo de KPIs: OS Abertas/Do Dia, Faturamento Mensal (6 Meses) e Peças Críticas (`T-032.3`, `T-032.4`, `T-032.5`, `T-034.1`)

- **Decision**:
  - **Query OS abertas e do dia (`T-032.3`)**:
    - `OsAbertas`: contagem de `OrdemServico` com `Status != OrdemServicoStatus.Concluida && Status != OrdemServicoStatus.Cancelada`.
    - `OsDoDia`: contagem de `OrdemServico` cuja `DataAbertura.Date == dataReferencia.Date`.
    - `OsPorStatus`: lista de `DashboardOsStatusCountDTO` (`status`, `label`, `quantidade`) cobrindo os status do enum `OrdemServicoStatus` (`Aberta`, `EmAndamento`, `AguardandoPeca`, `Concluida`, `Cancelada`).
  - **Query faturamento mensal e série de 6 meses (`T-032.4`, `T-033.3`)**:
    - Para cada OS com `Status == OrdemServicoStatus.Concluida`, o valor da OS é a soma de seus `Itens` (`item.Total > 0 ? item.Total : item.Quantidade * item.ValorUnitario`) ou, caso não haja itens mas haja `Pagamentos` pagos, a soma de `Pagamentos.Select(p => p.Valor)`. A data de competência do faturamento da OS é `(os.DataConclusao ?? os.DataAbertura)`.
    - `FaturamentoMensal`: soma do faturamento das OS concluídas no mês de referência (`periodo`).
    - `TicketMedio`: média do faturamento das OS concluídas no mês de referência (`FaturamentoMensal / OsConcluidasNoMes`, ou `0` quando não houver OS concluídas).
    - `FaturamentoMensalSerie` (`faturamentoUltimosSeisMeses`): lista ordenada cronologicamente com os últimos 6 meses (incluindo o mês de referência), contendo `mes` (ex.: `"2026-06"`), `rotulo` (ex.: `"Jun/2026"`), `valor` (`decimal`) e `quantidadeOs` (`int`). Meses sem faturamento retornam `valor = 0` para garantir sempre 6 barras/pontos no gráfico.
  - **Query peças críticas com `qtd_atual < qtd_minima` (`T-032.5`, `T-034.1`)**:
    - Filtra `EstoquePeca` onde `Quantidade < EstoqueMinimo`, ordenadas pelo maior déficit (`EstoqueMinimo - Quantidade` decrescente) e `Descricao`.
    - `PecasAbaixoMinimo`: contagem total de peças com `Quantidade < EstoqueMinimo`.
    - `PecasCriticas`: coleção de `DashboardPecaCriticaDTO` contendo `Id`, `Codigo`, `Descricao`, `QuantidadeAtual` (`Quantidade`), `EstoqueMinimo`, `Deficit` (`EstoqueMinimo - Quantidade`) e `Unidade`.
- **Rationale**:
  - `OrdemServicoRepository.GetAllAsync()` já carrega `.Include(o => o.Itens)` e `.Include(o => o.Pagamentos)` com `AsNoTracking()`, funcionando de forma idêntica tanto em MySQL quanto no `EF Core InMemory` dos testes unitários e de integração.
- **Alternatives considered**:
  - Retornar apenas a contagem de `pecasAbaixoMinimo` sem a lista: rejeitado porque `T-034.1` exige explicitamente incluir `pecasCriticas` no response de `/Dashboard/kpis` e `T-034.2` exige linkar cada item para `/estoque/:id`.

---

## 3. Frontend Angular: Consumo de `/api/v1/Dashboard/kpis`, Gráficos, Skeleton Loading e Card "Alertas de Estoque" (`US-033` & `US-034`)

- **Decision**:
  - Adicionar `dashboard: { kpis: '/api/v1/Dashboard/kpis' }` em `oficina-motos-web/src/app/core/services/api-paths.ts`.
  - Criar `DashboardService` em `oficina-motos-web/src/app/core/services/dashboard.service.ts` com método `getKpis(periodo?: string): Observable<DashboardKpisResponse>`.
  - Atualizar `DashboardPrincipalComponent` (mantendo o alias `DashboardPrincipal` para compatibilidade com `app.routes.ts`) em `oficina-motos-web/src/app/features/dashboard/pages/dashboard-principal/` utilizando Signals (`loading`, `kpisData`, `periodoSelecionado`, `cardsKpi`, `osPorStatus`, `faturamentoSeisMeses`, `pecasCriticas`):
    - **Skeleton Loading (`T-033.4`)**: Quando `loading()` for `true`, renderizar 4 cards de KPI com classe `.kpi-card.skeleton-card` e elementos `.skeleton-line` animados via shimmer CSS em vez de ocultar os cards.
    - **Gráfico de OS por Status (`T-033.2`)**: Renderizar gráfico visual de OS por status (`osPorStatus`) com barras proporcionais, cores semânticas por status, rótulos e contagens.
    - **Gráfico de Faturamento Mensal - 6 Meses (`T-033.3`)**: Renderizar gráfico de faturamento mensal dos últimos 6 meses (`faturamentoUltimosSeisMeses`) exibindo cada um dos 6 meses, barra proporcional ao maior faturamento do semestre e valor em `BRL`.
    - **Card "Alertas de Estoque" (`T-034.2`)**: Renderizar card intitulado `"Alertas de Estoque"` listando cada peça de `pecasCriticas` com código, descrição, `quantidadeAtual / estoqueMinimo` e link `[routerLink]="['/estoque', peca.id]"` (gerando `/estoque/:id`).
- **Rationale**:
  - Segue o Princípio IV da Constituição (Standalone Components + Signals), o Princípio II (`apiPaths.dashboard.kpis`), e garante testabilidade determinística em Vitest + JSDOM sem depender de canvas WebGL nativo para validar os dados renderizados nos gráficos.
- **Alternatives considered**:
  - Manter o `forkJoin` de 6 endpoints no `DashboardPrincipal`: rejeitado porque a `US-032` e a `T-033.1` determinam substituir o carregamento fragmentado/mockado pelo endpoint agregador `GET /api/v1/Dashboard/kpis`.
