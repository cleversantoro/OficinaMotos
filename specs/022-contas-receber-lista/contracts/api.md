# Contratos de API: Listar Contas a Receber (US-022)

**Branch**: `022-contas-receber-lista` | **Data**: 2026-11-16

## 1. Listar Contas a Receber (`T-022.1`)
- **Método / Rota**: `GET /api/v1/FinanceiroContasReceber`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.receber`
- **Serviço Frontend**: `FinanceiroService.contasReceber(params?)`
- **Resposta (`200 OK`)**: `FinanceiroContaReceber[]`
  ```json
  [
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
  ]
  ```

## 2. Listar Clientes (Consulta de Referência para Nome do Devedor/Cliente)
- **Método / Rota**: `GET /api/v1/Clientes`
- **Constante em `api-paths.ts`**: `apiPaths.clientes.base`
- **Serviço Frontend**: `ClientesService.list()`
- **Resposta (`200 OK`)**: `Cliente[]` (utilizados `id`, `nomeExibicao`, `nome`)

## 3. Listar Métodos de Pagamento (Consulta de Referência)
- **Método / Rota**: `GET /api/v1/FinanceiroMetodosPagamento`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.metodos`
- **Serviço Frontend**: `FinanceiroService.metodos()`
- **Resposta (`200 OK`)**: `FinanceiroMetodoPagamento[]` (utilizados `id`, `nome`)
