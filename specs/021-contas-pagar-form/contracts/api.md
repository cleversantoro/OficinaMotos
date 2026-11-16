# Contratos de API: Criar e Editar Conta a Pagar (US-021)

**Branch**: `021-contas-pagar-form` | **Data**: 2026-11-16

## 1. Consultar Conta a Pagar por ID (`T-021.3` — Modo Edição)
- **Método / Rota**: `GET /api/v1/FinanceiroContasPagar/{id}`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.pagar`
- **Método em `FinanceiroService`**: `getContaPagarById(id: number | string): Observable<FinanceiroContaPagar>`
- **Resposta (`200 OK`)**:
  ```json
  {
    "id": 9201,
    "fornecedorId": 301,
    "descricao": "Compra de filtros de óleo Bosch",
    "valor": 580.00,
    "vencimento": "2026-11-25T00:00:00",
    "status": "PENDENTE",
    "dataPagamento": null,
    "metodoId": 904,
    "observacao": "Prazo 10 dias via PIX"
  }
  ```
- **Resposta (`404 Not Found`)**:
  ```json
  {
    "message": "Conta a pagar nao encontrada."
  }
  ```

## 2. Criar Nova Conta a Pagar (`T-021.3` — Modo Criação)
- **Método / Rota**: `POST /api/v1/FinanceiroContasPagar`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.pagar`
- **Método em `FinanceiroService`**: `createContaPagar(payload: CreateFinanceiroContaPagarRequest): Observable<FinanceiroContaPagar>`
- **Corpo da Requisição (`CreateFinanceiroContaPagarRequest`)**:
  ```json
  {
    "fornecedorId": 301,
    "descricao": "Compra de kit relação DID",
    "valor": 1250.50,
    "vencimento": "2026-11-30",
    "status": "PENDENTE",
    "dataPagamento": null,
    "metodoId": 904,
    "observacao": "NF 45892"
  }
  ```
- **Resposta (`201 Created`)**: `FinanceiroContaPagar` criada com `id` gerado.

## 3. Atualizar Conta a Pagar Existente (`T-021.3` — Modo Edição)
- **Método / Rota**: `PUT /api/v1/FinanceiroContasPagar/{id}`
- **Permissão no Backend**: `[Authorize(Roles = "Administrador,Gerente,Financeiro")]`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.pagar`
- **Método em `FinanceiroService`**: `updateContaPagar(id: number | string, payload: UpdateFinanceiroContaPagarRequest): Observable<FinanceiroContaPagar>`
- **Corpo da Requisição (`UpdateFinanceiroContaPagarRequest`)**:
  ```json
  {
    "fornecedorId": 301,
    "descricao": "Compra de kit relação DID - Lote extra",
    "valor": 1400.00,
    "vencimento": "2026-12-05",
    "status": "PAGO",
    "dataPagamento": "2026-11-16",
    "metodoId": 904,
    "observacao": "Quitado antecipadamente"
  }
  ```
- **Resposta (`200 OK`)**: `FinanceiroContaPagar` atualizada.

## 4. Endpoints Auxiliares de Seleção (Fornecedores e Métodos de Pagamento)
- `GET /api/v1/Fornecedores` via `FornecedoresService.list()` (`apiPaths.fornecedores.base`)
- `GET /api/v1/FinanceiroMetodosPagamento` via `FinanceiroService.metodos()` (`apiPaths.financeiro.metodos`)
