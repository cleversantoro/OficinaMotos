# Pesquisa Técnica: Listar Contas a Receber (US-022)

**Branch**: `022-contas-receber-lista` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/022-contas-receber-lista/spec.md)

## 1. Análise dos Endpoints e Contratos Existentes

### 1.1 Endpoint de Contas a Receber (`GET /api/v1/FinanceiroContasReceber`)
- **Controller**: [`FinanceiroContasReceberController.cs`](file:///c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.API/Controllers/Financeiro/FinanceiroContasReceberController.cs)
- **Rota em `api-paths.ts`**: `apiPaths.financeiro.receber` (`'/api/v1/FinanceiroContasReceber'`)
- **Método no `FinanceiroService`**: `contasReceber(params?: Record<string, any>)` em [`financeiro.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/financeiro.service.ts)
- **DTO de Resposta (`FinanceiroContaReceberResponseDTO` / interface `FinanceiroContaReceber`)**:
  ```json
  {
    "id": 9101,
    "clienteId": 101,
    "descricao": "Pagamento OS #12 - PIX",
    "valor": 450.00,
    "vencimento": "2026-11-16T00:00:00",
    "status": "Recebido",
    "dataRecebimento": "2026-11-16T00:00:00",
    "metodoId": 904,
    "observacao": "Referente à OS #12"
  }
  ```

### 1.2 Dados Auxiliares (Clientes e Métodos de Pagamento)
- **Decision**: Carregar em paralelo (`forkJoin` com `catchError(() => of([]))` nas listas auxiliares) `FinanceiroService.contasReceber()`, `ClientesService.list()` (`GET /api/v1/Clientes`) e `FinanceiroService.metodos()` (`GET /api/v1/FinanceiroMetodosPagamento`).
- **Rationale**: Resolve `cliente.nomeExibicao || cliente.nome` para `clienteId` e `metodo.nome` para `metodoId`, mantendo fallback seguro (`Cliente #101` ou `-`) caso as chamadas auxiliares falhem ou o ID seja nulo.

## 2. Resolução da Coluna "OS Origem" e Link para `/ordens/:id` (`T-022.3`)

- **Contexto Técnico**:
  - Na `US-012` (`OrdemServicoService.RegistrarPagamentoAsync` em [`OrdemServicoService.cs`](file:///c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.Application/Services/OrdemServico/OrdemServicoService.cs)), todo pagamento de Ordem de Serviço gera automaticamente uma `FinanceiroContaReceber` com:
    - `Descricao = $"Pagamento OS #{ordem.Id} - {request.Metodo ?? "Geral"}"`
    - `Observacao = string.IsNullOrWhiteSpace(request.Observacao) ? $"Referente à OS #{ordem.Id}" : $"{request.Observacao} (OS #{ordem.Id})"`
  - Adicionalmente, objetos/mocks podem fornecer opcionalmente `ordemServicoId?: number | null`.
- **Decision**:
  - Estender opcionalmente a interface `FinanceiroContaReceber` com `ordemServicoId?: number | null`.
  - Implementar o método `getOrdemServicoId(conta: FinanceiroContaReceber): number | null` em `ContasReceberListaComponent`:
    1. Se `conta.ordemServicoId` for numérico inteiro positivo (`> 0`), retorna `Number(conta.ordemServicoId)`.
    2. Caso contrário, busca o padrão `/\bOS\s*#?\s*(\d+)\b/i` em `conta.descricao` e, se não encontrado, em `conta.observacao`. Se encontrar um número inteiro positivo, retorna `Number(match[1])`.
    3. Caso não haja correspondência, retorna `null`.
  - Na coluna **"OS Origem"** da tabela:
    - Quando `osId !== null`: renderizar `<a class="os-origin-link" [routerLink]="['/ordens', osId]">OS #{{ osId }}</a>`.
    - Quando `osId === null`: renderizar `<span class="muted-cell">-</span>`.
- **Rationale**: Atende 100% ao requisito `T-022.3 — Coluna "OS Origem" com link para /ordens/:id` sem exigir alteração de schema ou breaking change no banco MySQL (`fin_contas_receber`), funcionando tanto com os registros reais gerados por `OrdemServicoService.RegistrarPagamentoAsync` quanto com objetos que informem `ordemServicoId`.

## 3. Normalização de Status e Filtros Reativos (`T-022.1`)

- **Decision**:
  - Implementar `getEffectiveStatus(conta: FinanceiroContaReceber): 'PENDENTE' | 'VENCIDO' | 'RECEBIDO' | 'CANCELADO'`:
    - `'RECEBIDO'`, `'PAGO'`, `'LIQUIDADO'` → `'RECEBIDO'`
    - `'CANCELADO'` → `'CANCELADO'`
    - `'VENCIDO'` ou (`!conta.dataRecebimento && toDateKey(conta.vencimento) < todayKey`) → `'VENCIDO'`
    - Demais casos (`'PENDENTE'`, `'ABERTO'`) → `'PENDENTE'`
  - Disponibilizar filtros reativos via Signals (`statusFilter`, `dataInicio`, `dataFim`, `searchTerm`), KPIs resumidos (Total a Receber em Aberto, Total Vencido e Total Recebido) e paginação em memória (`rows = 8`), espelhando a arquitetura consolidada de `ContasPagarListaComponent` (`US-020`).
