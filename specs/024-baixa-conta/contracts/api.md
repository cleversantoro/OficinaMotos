# Contratos de API: Registrar Baixa de Conta (US-024)

**Branch**: `024-baixa-conta` | **Data**: 2026-11-16

## 1. Registrar Baixa de Conta a Pagar (`T-024.2`)
- **Método / Rota**: `POST /api/v1/ContasPagarPagamentos`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.pagarPagamentos`
- **Método em `FinanceiroService`**: `registrarBaixaContaPagar(payload: CreateContaPagarPagamentoRequest): Observable<ContaPagarPagamentoResponse>`
- **Corpo da Requisição (`CreateContaPagarPagamentoRequest`)**:
  ```json
  {
    "contaPagarId": 9201,
    "dataPagamento": "2026-11-16",
    "valorPago": 200.00,
    "metodoId": 904,
    "observacao": "Adiantamento via PIX"
  }
  ```
- **Resposta (`201 Created` - Exemplo de Baixa Parcial)**:
  ```json
  {
    "contaPagarId": 9201,
    "pagamentoId": 9501,
    "valorPago": 200.00,
    "saldoResidual": 380.00,
    "baixaIntegral": false,
    "conta": {
      "id": 9201,
      "fornecedorId": 301,
      "descricao": "Compra de filtros de óleo Bosch",
      "valor": 380.00,
      "vencimento": "2026-11-18T00:00:00",
      "status": "PENDENTE",
      "dataPagamento": null,
      "metodoId": 904,
      "observacao": "Prazo 10 dias via PIX | Baixa parcial R$ 200,00 (2026-11-16)"
    }
  }
  ```
- **Respostas de Erro**:
  - `400 Bad Request`: Valor pago inválido (`<= 0` ou `> conta.valor`), conta já paga/cancelada ou campos obrigatórios ausentes.
  - `404 Not Found`: `contaPagarId` não encontrado.

## 2. Registrar Baixa de Conta a Receber (`T-024.3`)
- **Método / Rota**: `POST /api/v1/ContasReceberPagamentos`
- **Constante em `api-paths.ts`**: `apiPaths.financeiro.receberPagamentos`
- **Método em `FinanceiroService`**: `registrarBaixaContaReceber(payload: CreateContaReceberPagamentoRequest): Observable<ContaReceberPagamentoResponse>`
- **Corpo da Requisição (`CreateContaReceberPagamentoRequest`)**:
  ```json
  {
    "contaReceberId": 9302,
    "dataPagamento": "2026-11-16",
    "valorPago": 698.00,
    "metodoId": 904,
    "observacao": "Recebido integralmente no balcão"
  }
  ```
- **Resposta (`201 Created` - Exemplo de Baixa Integral)**:
  ```json
  {
    "contaReceberId": 9302,
    "pagamentoId": 9502,
    "valorPago": 698.00,
    "saldoResidual": 0.00,
    "baixaIntegral": true,
    "conta": {
      "id": 9302,
      "clienteId": 202,
      "descricao": "Ordem #10002 - Suspensão dianteira",
      "valor": 698.00,
      "vencimento": "2026-11-10T00:00:00",
      "status": "RECEBIDO",
      "dataRecebimento": "2026-11-16T00:00:00",
      "metodoId": 904,
      "observacao": "Recebido integralmente no balcão"
    }
  }
  ```
- **Respostas de Erro**:
  - `400 Bad Request`: Valor pago inválido (`<= 0` ou `> conta.valor`), conta já recebida/cancelada ou campos obrigatórios ausentes.
  - `404 Not Found`: `contaReceberId` não encontrado.
