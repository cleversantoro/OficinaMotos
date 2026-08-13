# Contract: Ordem de Serviço com Veículo

## Objetivo

Documentar o contrato de criação e leitura da ordem de serviço quando a referência ao veículo passa a ser obrigatória.

## Requisição de criação

### Endpoint

`POST /api/v1/OrdemServico`

### Payload

```json
{
  "clienteId": 123,
  "mecanicoId": 456,
  "veiculoId": 101,
  "descricaoProblema": "Motor em falha ao dar partida",
  "status": "Aberta",
  "dataAbertura": "2026-08-12T10:00:00Z"
}
```

### Regras

- `veiculoId` é obrigatório
- o valor deve ser um `long` válido
- o veículo deve existir em `cad_veiculos` antes da persistência

## Resposta de sucesso

```json
{
  "id": 1001,
  "clienteId": 123,
  "mecanicoId": 456,
  "veiculoId": 101,
  "descricaoProblema": "Motor em falha ao dar partida",
  "status": "Aberta",
  "dataAbertura": "2026-08-12T10:00:00Z",
  "dataConclusao": null
}
```

## Erros esperados

- `400 Bad Request`: payload sem `veiculoId` ou com `long` inválido
- `404 Not Found`: veículo informado não existe
- `500 Internal Server Error`: falha inesperada de persistência ou banco de dados

## Observações

- a referência ao veículo deve ser persistida em todas as ordens criadas no sistema
- o contrato deve continuar consistente com o modelo de domínio e o banco de dados
