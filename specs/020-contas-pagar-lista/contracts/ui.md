# Contratos de UI: Listar Contas a Pagar (US-020)

**Branch**: `020-contas-pagar-lista` | **Data**: 2026-11-16

## 1. Rota `/financeiro/contas-pagar` (`ContasPagarListaComponent` — `T-020.1`, `T-020.2`)
- **Seletor**: `app-contas-pagar-lista`
- **Caminho**: `src/app/features/financeiro/pages/contas-pagar-lista/contas-pagar-lista.ts`
- **Proteção**: Filha de `MainLayout` (`canActivate: [authGuard]`) em `src/app/app.routes.ts`.

## 2. Estrutura Visual da Página
1. **Cabeçalho da Página**:
   - Link de retorno `← Painel Financeiro` (`routerLink="/financeiro"`).
   - Título `"Contas a Pagar"` e subtítulo descritivo.
2. **Cards de Resumo (KPIs)**:
   - **A Pagar (Em aberto)**: soma de `valor` das contas com status efetivo `PENDENTE` ou `VENCIDO`.
   - **Contas Vencidas**: soma de `valor` e quantidade das contas com status efetivo `VENCIDO` (card com classe `.alerta` quando `> 0`).
   - **Total Pago**: soma de `valor` das contas com status efetivo `PAGO`.
3. **Barra de Filtros (`T-020.3`)**:
   - **Busca textual** (`searchTerm`): input de texto para filtrar por descrição, observação ou fornecedor.
   - **Filtro de Status** (`statusFilter`): `<select>` com opções:
     - `TODOS` ("Todos os status")
     - `PENDENTE` ("Pendente")
     - `VENCIDO` ("Vencido")
     - `PAGO` ("Pago")
     - `CANCELADO` ("Cancelado")
   - **Filtro de Período de Vencimento** (`dataInicio` e `dataFim`): dois campos `<input type="date">` ("Vencimento de" / "Até").
   - **Botão `"Limpar filtros"`**: visível/habilitado quando qualquer filtro estiver ativo, redefinindo `statusFilter = 'TODOS'`, `dataInicio = ''`, `dataFim = ''`, `searchTerm = ''` e `first = 0`.
4. **Tabela de Contas a Pagar e Destaque de Vencidas (`T-020.4`)**:
   - Colunas:
     1. **Vencimento** (`dd/MM/yyyy` + tag/indicador `"Em atraso"` quando `isVencida(conta)`)
     2. **Fornecedor** (nome fantasia / razão social ou `Fornecedor #id` / `-`)
     3. **Descrição** (`descricao` + `observacao` secundária quando presente)
     4. **Método** (nome do método de pagamento ou `-`)
     5. **Status** (badge com classes `.status-pendente`, `.status-vencido`, `.status-pago`, `.status-cancelado`)
     6. **Pagamento** (`dataPagamento | date:'dd/MM/yyyy'` ou `-`)
     7. **Valor** (`R$ {{ conta.valor | number:'1.2-2' }}`)
   - **Destaque Visual de Conta Vencida (`T-020.4`)**:
     - A linha `<tr>` recebe a classe `.row-vencida` quando `isVencida(conta) === true` (fundo avermelhado sutil e borda lateral de alerta).
     - A célula de vencimento recebe `.vencimento-atrasado` e exibe o indicador `"Vencida"`.
     - O badge de status recebe `.status-vencido`.
5. **Paginação e Estados**:
   - Controles `← Anterior` e `Próximo →` com indicador `X–Y de Z`.
   - Estado de carregamento (`loading()`), estado de erro (`error()`) com botão `"Tentar novamente"` e estado vazio diferenciando lista sem dados vs filtro sem resultados.
