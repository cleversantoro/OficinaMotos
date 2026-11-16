# Contratos de API: Listar Contas a Pagar (US-020)

**Branch**: `020-contas-pagar-lista` | **Data**: 2026-11-16

## 1. Listar Contas a Pagar (`T-020.1`)
- **Método / Rota**: `GET /api/v1/FinanceiroContasPagar`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.pagar`
- **Serviço Frontend**: `FinanceiroService.contasPagar(params?)`
- **Resposta (`200 OK`)**: `FinanceiroContaPagar[]`
  ```json
  [
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
  ]
  ```

## 2. Listar Fornecedores (Consulta de Referência para Nome do Credor)
- **Método / Rota**: `GET /api/v1/Fornecedores`
- **Constante em `api-paths.ts`**: `apiPaths.fornecedores.base`
- **Serviço Frontend**: `FornecedoresService.list()`
- **Resposta (`200 OK`)**: `Fornecedor[]` (utilizados `id`, `nomeFantasia`, `razaoSocial`)

## 3. Listar Métodos de Pagamento (Consulta de Referência)
- **Método / Rota**: `GET /api/v1/FinanceiroMetodosPagamento`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.metodos`
- **Serviço Frontend**: `FinanceiroService.metodos()`
- **Resposta (`200 OK`)**: `FinanceiroMetodoPagamento[]` (utilizados `id`, `nome`)
