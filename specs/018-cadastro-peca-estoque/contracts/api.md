# Contratos de API: Cadastro de Peça no Estoque

**Feature**: `018-cadastro-peca-estoque` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Endpoints Utilizados

### 1.1 `GET /api/v1/EstoqueCategorias`, `GET /api/v1/EstoqueFabricantes`, `GET /api/v1/EstoqueLocalizacoes` e `GET /api/v1/Fornecedores`
Alimentam os dropdowns de classificação e fornecedor (`T-018.3`).

---

### 1.2 `POST /api/v1/EstoquePecas`
Cria o registro principal da peça no estoque (`CreateEstoquePecaDTO`).

#### Request Body
```json
{
  "codigo": "PEC-010",
  "descricao": "Pastilha de Freio Dianteira Cobreq Honda CB 300F",
  "fabricanteId": 2,
  "categoriaId": 1,
  "localizacaoId": 3,
  "precoUnitario": 68.50,
  "quantidade": 12,
  "estoqueMinimo": 5,
  "estoqueMaximo": 40,
  "unidade": "PAR",
  "status": "Ativo",
  "observacoes": "Compatível com CB 300F Twister 2023 em diante."
}
```

#### Response Body (`EstoquePecaResponseDTO`) — HTTP 201 Created
```json
{
  "id": 25,
  "codigo": "PEC-010",
  "descricao": "Pastilha de Freio Dianteira Cobreq Honda CB 300F",
  "fabricanteId": 2,
  "categoriaId": 1,
  "localizacaoId": 3,
  "precoUnitario": 68.50,
  "quantidade": 12,
  "estoqueMinimo": 5,
  "estoqueMaximo": 40,
  "unidade": "PAR",
  "status": "Ativo"
}
```

---

### 1.3 `POST /api/v1/EstoquePecaFornecedores`
Registra o vínculo entre a peça recém-criada e o fornecedor selecionado no dropdown (`CreateEstoquePecaFornecedorDTO`).

#### Request Body
```json
{
  "pecaId": 25,
  "fornecedorId": 1,
  "preco": 39.90,
  "prazoEntrega": 3,
  "observacao": "Fornecedor principal no cadastro inicial"
}
```

