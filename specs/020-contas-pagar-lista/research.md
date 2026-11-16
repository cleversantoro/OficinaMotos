# Pesquisa Técnica: Listar Contas a Pagar (US-020)

**Branch**: `020-contas-pagar-lista` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/020-contas-pagar-lista/spec.md)

## 1. Análise dos Endpoints e Contratos Existentes

### 1.1 Endpoint de Contas a Pagar (`GET /api/v1/FinanceiroContasPagar`)
- **Controller**: [`FinanceiroContasPagarController.cs`](file:///c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.API/Controllers/Financeiro/FinanceiroContasPagarController.cs)
- **Rota em `api-paths.ts`**: `apiPaths.financeiro.pagar` (`'/api/v1/FinanceiroContasPagar'`)
- **Método no `FinanceiroService`**: `contasPagar(params?: Record<string, any>)` em [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts)
- **DTO de Resposta (`FinanceiroContaPagarResponseDTO` / interface `FinanceiroContaPagar`)**:
  ```json
  {
    "id": 9201,
    "fornecedorId": 301,
    "descricao": "Compra de filtros de óleo Bosch",
    "valor": 580.00,
    "vencimento": "2025-11-18T02:00:02",
    "status": "PENDENTE",
    "dataPagamento": null,
    "metodoId": 904,
    "observacao": "Prazo 10 dias via PIX"
  }
  ```

### 1.2 Dados Auxiliares (Fornecedores e Métodos de Pagamento)
- **Decision**: Carregar em paralelo (`forkJoin` com `catchError` individual para listas auxiliares) `FinanceiroService.contasPagar()`, `FornecedoresService.list()` (`GET /api/v1/Fornecedores`) e `FinanceiroService.metodos()` (`GET /api/v1/FinanceiroMetodosPagamento`).
- **Rationale**: A tabela `fin_contas_pagar` retorna `fornecedorId` e `metodoId` numéricos. Resolver o `nomeFantasia || razaoSocial` do fornecedor e o `nome` do método de pagamento enriquece a tabela e permite buscar pelo nome do fornecedor, mantendo fallback seguro (`Fornecedor #301` ou `-`) caso a chamada auxiliar falhe ou o ID seja nulo.
- **Alternatives considered**: Exibir apenas `Fornecedor #ID` como no rascunho de `FinanceiroDashboard`. Rejeitado para a página dedicada `ContasPagarListaComponent` porque reduz a usabilidade quando os serviços `FornecedoresService` e `FinanceiroService.metodos()` já estão disponíveis no frontend.

## 2. Normalização de Status e Regra de Conta Vencida (`T-020.4`)

- **Contexto do Banco de Dados (`oficina_db_table_fin_contas_pagar.sql`)**:
  - Os registros no MySQL possuem `status` em caixa alta: `'PENDENTE'`, `'PAGO'`, `'CANCELADO'`.
  - Contas com `status = 'PENDENTE'` e `vencimento` anterior à data atual (`hoje`) estão efetivamente vencidas, mesmo que nenhum job em background tenha alterado a string `status` no banco para `'VENCIDO'`.
- **Decision**:
  - Implementar o método `getEffectiveStatus(conta: FinanceiroContaPagar): 'PENDENTE' | 'VENCIDO' | 'PAGO' | 'CANCELADO'`:
    1. Normalizar `raw = (conta.status ?? '').trim().toUpperCase()`.
    2. Se `raw === 'PAGO'` → retorna `'PAGO'`.
    3. Se `raw === 'CANCELADO'` → retorna `'CANCELADO'`.
    4. Se `raw === 'VENCIDO'` → retorna `'VENCIDO'`.
    5. Caso contrário (ex.: `'PENDENTE'`, `'ABERTO'`), comparar a data civil local de `conta.vencimento` (`YYYY-MM-DD`) com a data civil local de hoje (`YYYY-MM-DD`). Se `vencimentoDate < hojeDate` e `!conta.dataPagamento`, retorna `'VENCIDO'`; senão retorna `'PENDENTE'`.
  - O método `isVencida(conta)` retorna `this.getEffectiveStatus(conta) === 'VENCIDO'`.
- **Rationale**: Garante consistência total entre o destaque visual de contas vencidas (`T-020.4`), o KPI de "Total Vencido" e o filtro por status `Vencido` (`T-020.3`), independentemente de o status no banco estar gravado como `'PENDENTE'` com data expirada ou `'VENCIDO'`.
- **Alternatives considered**: Verificar apenas `conta.status.toLowerCase() === 'vencido'`. Rejeitado porque no seed SQL (`oficina_db_table_fin_contas_pagar.sql`) todas as contas em aberto possuem `status = 'PENDENTE'` com datas de vencimento passadas, o que faria com que nenhuma delas fosse destacada como vencida.

## 3. Filtragem por Status e Período (`T-020.3`)

- **Decision**:
  - Utilizar Signals para os controles de filtro (`statusFilter`, `dataInicio`, `dataFim`, `searchTerm`) e um `computed` signal `filteredContas` que filtra e ordena os itens em memória.
  - Comparação de período: extrair a parte de data `YYYY-MM-DD` de `conta.vencimento` e comparar lexicograficamente com `dataInicio` (`>= dataInicio`) e `dataFim` (`<= dataFim`).
- **Rationale**: Evita problemas de fuso horário (UTC vs America/Sao_Paulo) ao comparar datas selecionadas em `<input type="date">` (`YYYY-MM-DD`) com strings ISO vindas da API (`2025-11-18T02:00:02`).
- **Alternatives considered**: Converter ambos para `Date` UTC meia-noite. A extração de `YYYY-MM-DD` (com fallback para `Date` local) é mais simples, determinística e livre de desvios de timezone.
