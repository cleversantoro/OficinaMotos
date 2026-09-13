# Contratos de API: Campo `proximo_km_revisao`

**Feature**: `014-proximo-km-revisao` | **Data**: 2026-09-13

---

## 1. Endpoints Afetados

### 1.1 `POST /api/v1/Veiculos`
Cria um novo veículo/motocicleta.

#### Request Headers
- `Authorization: Bearer <jwt_token>`
- `Content-Type: application/json`

#### Request Body (`CreateVeiculoDTO`)
```json
{
  "clienteId": 12,
  "placa": "BRA2E19",
  "modeloId": 5,
  "anoFab": 2023,
  "anoMod": 2024,
  "cor": "Vermelho",
  "chassi": "9C2JD08107R000001",
  "renavam": "01234567890",
  "km": "10500",
  "proximoKmRevisao": 15000,
  "combustivel": "Gasolina",
  "observacao": "Revisão dos 15.000 km inclui troca de vela e filtro",
  "principal": true,
  "ativo": true
}
```

#### Response Body (`VeiculoResponseDTO`) — HTTP 201 Created
```json
{
  "id": 101,
  "clienteId": 12,
  "placa": "BRA2E19",
  "modeloId": 5,
  "anoFab": 2023,
  "anoMod": 2024,
  "cor": "Vermelho",
  "chassi": "9C2JD08107R000001",
  "renavam": "01234567890",
  "km": "10500",
  "proximoKmRevisao": 15000,
  "combustivel": "Gasolina",
  "observacao": "Revisão dos 15.000 km inclui troca de vela e filtro",
  "principal": true,
  "ativo": true
}
```

---

### 1.2 `PUT /api/v1/Veiculos/{id}`
Atualiza dados cadastrais de um veículo existente.

#### Request Body (`UpdateVeiculoDTO`)
```json
{
  "clienteId": 12,
  "placa": "BRA2E19",
  "modeloId": 5,
  "anoFab": 2023,
  "anoMod": 2024,
  "cor": "Vermelho",
  "chassi": "9C2JD08107R000001",
  "renavam": "01234567890",
  "km": "15120",
  "proximoKmRevisao": 20000,
  "combustivel": "Gasolina",
  "observacao": "Revisão dos 15.000 km realizada; agendada para 20.000 km",
  "principal": true,
  "ativo": true
}
```

#### Response Body — HTTP 200 OK
Retorna o `VeiculoResponseDTO` atualizado contendo o novo `proximoKmRevisao`.

---

### 1.3 `GET /api/v1/Veiculos/{id}` e `GET /api/v1/Veiculos`
Recuperação individual ou em lista de veículos.

#### Response Body
Todos os itens retornados conterão a propriedade `"proximoKmRevisao": 15000` (ou `null` se não informado).

