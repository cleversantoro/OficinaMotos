# Contratos de UI: Listar Contas a Receber (US-022)

**Branch**: `022-contas-receber-lista` | **Data**: 2026-11-16

## 1. Rota `/financeiro/contas-receber` (`ContasReceberListaComponent` — `T-022.1`, `T-022.2`)
- **Seletor**: `app-contas-receber-lista`
- **Caminho**: `src/app/features/financeiro/pages/contas-receber-lista/contas-receber-lista.ts`
- **Proteção**: Filha de `MainLayout` (`canActivate: [authGuard]`) em [`src/app/app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).

## 2. Estrutura Visual da Página
1. **Cabeçalho da Página**:
   - Link de retorno `← Painel Financeiro` (`routerLink="/financeiro"`).
   - Título `"Contas a Receber"` e subtítulo descritivo.
2. **Cards de Resumo (KPIs)**:
   - **A receber (em aberto)**: soma de `valor` das contas com status efetivo `PENDENTE` ou `VENCIDO`.
   - **Recebíveis vencidos**: soma de `valor` e quantidade das contas com status efetivo `VENCIDO` (card `.alerta` quando `> 0`).
   - **Total recebido**: soma de `valor` das contas com status efetivo `RECEBIDO`.
3. **Barra de Filtros**:
   - **Busca textual** (`searchTerm`): filtra por descrição, observação, nome do cliente ou número da OS.
   - **Filtro de Status** (`statusFilter`): `TODOS`, `PENDENTE`, `VENCIDO`, `RECEBIDO`, `CANCELADO`.
   - **Período de Vencimento** (`dataInicio` e `dataFim`) e botão `"Limpar filtros"`.
4. **Tabela de Contas a Receber e Coluna "OS Origem" (`T-022.3`)**:
   - Colunas:
     1. **Vencimento** (`dd/MM/yyyy` + indicador `"⚠ Em atraso"` quando `isVencida(conta)`)
     2. **Cliente** (`nomeExibicao || nome` ou `Cliente #id` / `-`)
     3. **Descrição** (`descricao` + `observacao` secundária)
     4. **OS Origem** (`T-022.3`):
        - Se `getOrdemServicoId(conta)` retornar um ID válido `osId`: exibe `<a class="os-origin-link" [routerLink]="['/ordens', osId]">OS #{{ osId }}</a>`
        - Caso contrário: exibe `-`
     5. **Método** (nome do método de pagamento ou `-`)
     6. **Status** (badge `.status-pendente`, `.status-vencido`, `.status-recebido`, `.status-cancelado`)
     7. **Recebimento** (`dataRecebimento | date:'dd/MM/yyyy'` ou `-`)
     8. **Valor** (`R$ {{ conta.valor | number:'1.2-2' }}`)
5. **Atalho em `FinanceiroDashboard`**:
   - Na aba `"Contas a Receber"` de [`financeiro-dashboard.html`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/financeiro/pages/financeiro-dashboard/financeiro-dashboard.html), link `<a class="ghost" routerLink="/financeiro/contas-receber">Ver listagem completa</a>`.
