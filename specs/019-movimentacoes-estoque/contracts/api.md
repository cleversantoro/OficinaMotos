# Contratos de API: Movimentações de Entrada e Saída de Estoque (US-019)

**Branch**: `019-movimentacoes-estoque` | **Data**: 2026-11-04

## 1. Consultar Peça por ID
- **Método / Rota**: `GET /api/v1/EstoquePecas/{id}`
- **Serviço Frontend**: `EstoqueService.getPeca(id)`
- **Resposta (`200 OK`)**: `EstoquePeca`

## 2. Listar Movimentações de Estoque
- **Método / Rota**: `GET /api/v1/EstoqueMovimentacoes`
- **Serviço Frontend**: `EstoqueService.listarMovimentacoes(params?)`
- **Resposta (`200 OK`)**: `EstoqueMovimentacao[]`
  ```json
  [
    {
      "id": 1,
      "pecaId": 10,
      "quantidade": 5,
      "tipo": "ENTRADA",
      "referencia": "NF 12345",
      "dataMovimentacao": "2026-11-04T22:50:00Z",
      "usuario": "Administrador"
    }
  ]
  ```

## 3. Registrar Movimentação de Estoque (`T-019.3`)
- **Método / Rota**: `POST /api/v1/EstoqueMovimentacoes`
- **Serviço Frontend**: `EstoqueService.movimentar(payload)`
- **Request Body (`CreateEstoqueMovimentacaoRequest`)**:
  ```json
  {
    "pecaId": 10,
    "quantidade": 5,
    "tipo": "ENTRADA",
    "referencia": "NF 12345",
    "usuario": "Administrador"
  }
  ```
- **Resposta (`201 Created`)**: `EstoqueMovimentacao`

## 4. Atualizar Saldo da Peça
- **Método / Rota**: `PUT /api/v1/EstoquePecas/{id}`
- **Serviço Frontend**: `EstoqueService.atualizarPeca(id, payload)`
- **Request Body (`UpdateEstoquePecaRequest`)**: Objeto `EstoquePeca` com `quantidade` atualizada (`peca.quantidade + qtd` para `ENTRADA`, `peca.quantidade - qtd` para `SAIDA`).

