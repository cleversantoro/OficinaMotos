# Contratos de API: Criar e Editar Conta a Receber Manual (US-023)

**Branch**: `023-contas-receber-form` | **Data**: 2026-11-16

## 1. Consultar Conta a Receber por ID (Modo Edição)
- **Método / Rota**: `GET /api/v1/FinanceiroContasReceber/{id}`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.receber`
- **Método em `FinanceiroService`**: `getContaReceberById(id: number | string): Observable<FinanceiroContaReceber>`
- **Resposta (`200 OK`)**:
  ```json
  {
    "id": 9301,
    "clienteId": 101,
    "descricao": "Venda avulsa de capacete e luvas",
    "valor": 450.00,
    "vencimento": "2026-11-25T00:00:00",
    "status": "PENDENTE",
    "dataRecebimento": null,
    "metodoId": 904,
    "observacao": "Pagamento combinado para dia 25"
  }
  ```
- **Resposta (`404 Not Found`)**:
  ```json
  {
    "message": "Conta a receber nao encontrada."
  }
  ```

## 2. Criar Nova Conta a Receber Manual (Modo Criação)
- **Método / Rota**: `POST /api/v1/FinanceiroContasReceber`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.receber`
- **Método em `FinanceiroService`**: `createContaReceber(payload: CreateFinanceiroContaReceberRequest): Observable<FinanceiroContaReceber>`
- **Corpo da Requisição (`CreateFinanceiroContaReceberRequest`)**:
  ```json
  {
    "clienteId": 101,
    "descricao": "Venda avulsa de capacete e luvas",
    "valor": 450.00,
    "vencimento": "2026-11-25",
    "status": "PENDENTE",
    "dataRecebimento": null,
    "metodoId": 904,
    "observacao": "Pagamento combinado para dia 25"
  }
  ```
- **Resposta (`201 Created`)**: `FinanceiroContaReceber` criada com `id` gerado.

## 3. Atualizar Conta a Receber Existente (Modo Edição)
- **Método / Rota**: `PUT /api/v1/FinanceiroContasReceber/{id}`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.receber`
- **Método em `FinanceiroService`**: `updateContaReceber(id: number | string, payload: UpdateFinanceiroContaReceberRequest): Observable<FinanceiroContaReceber>`
- **Corpo da Requisição (`UpdateFinanceiroContaReceberRequest`)**:
  ```json
  {
    "clienteId": 101,
    "descricao": "Venda avulsa de capacete, luvas e viseira",
    "valor": 520.00,
    "vencimento": "2026-11-25",
    "status": "RECEBIDO",
    "dataRecebimento": "2026-11-16",
    "metodoId": 904,
    "observacao": "Recebido via PIX"
  }
  ```
- **Resposta (`200 OK`)**: `FinanceiroContaReceber` atualizada.

## 4. Endpoints Auxiliares de Seleção (Clientes e Métodos de Pagamento)
- `GET /api/v1/Clientes` via `ClientesService.list()` (`apiPaths.clientes.base`)
- `GET /api/v1/FinanceiroMetodosPagamento` via `FinanceiroService.metodos()` (`apiPaths.financeiro.metodos`)
