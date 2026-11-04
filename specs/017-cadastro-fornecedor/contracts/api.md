# Contratos de API: Cadastro de Fornecedor

**Feature**: `017-cadastro-fornecedor` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Endpoints Utilizados

### 1.1 `GET /api/v1/FornecedorSegmentos`
Retorna a lista de segmentos cadastrados para alimentar o dropdown de Segmento Principal (`T-017.4`).

#### Response Body — HTTP 200 OK
```json
[
  { "id": 1, "codigo": "SEG-OEM", "nome": "Peças Originais (OEM)", "descricao": null, "ativo": true },
  { "id": 2, "codigo": "SEG-PNE", "nome": "Pneus & Câmaras", "descricao": null, "ativo": true },
  { "id": 3, "codigo": "SEG-LUB", "nome": "Lubrificantes & Fluidos", "descricao": null, "ativo": true },
  { "id": 4, "codigo": "SEG-ELC", "nome": "Elétrica & Baterias", "descricao": null, "ativo": true }
]
```

---

### 1.2 `POST /api/v1/Fornecedores`
Cria o registro principal do fornecedor (`CreateFornecedorDTO`).

#### Request Body
```json
{
  "codigo": "FOR-005",
  "tipo": "DISTRIBUIDOR",
  "razaoSocial": "Moto Peças Brasil Distribuidora Ltda",
  "nomeFantasia": "MotoBrasil Peças",
  "documento": "11222333000181",
  "inscricaoEstadual": "110042490114",
  "inscricaoMunicipal": null,
  "segmentoPrincipalId": 1,
  "website": "https://motobrasil.com.br",
  "email": "comercial@motobrasil.com.br",
  "telefonePrincipal": "(11) 98888-7777",
  "status": "ATIVO",
  "condicaoPagamentoPadrao": "28/56 dias",
  "prazoEntregaMedio": 3,
  "notaMedia": null,
  "observacoes": "Fornecedor homologado de peças Honda e Yamaha.",
  "prazoGarantiaPadrao": "90 dias",
  "termosNegociados": null,
  "atendimentoPersonalizado": true,
  "retiradaLocal": true,
  "ratingLogistica": null,
  "ratingQualidade": null
}
```

#### Response Body (`FornecedorResponseDTO`) — HTTP 201 Created
```json
{
  "id": 12,
  "codigo": "FOR-005",
  "tipo": "DISTRIBUIDOR",
  "razaoSocial": "Moto Peças Brasil Distribuidora Ltda",
  "nomeFantasia": "MotoBrasil Peças",
  "documento": "11222333000181",
  "segmentoPrincipalId": 1,
  "status": "ATIVO"
}
```

---

### 1.3 `POST /api/v1/FornecedorEnderecos`
Registra o endereço comercial principal do fornecedor quando preenchido (`CreateFornecedorEnderecoDTO`).

#### Request Body
```json
{
  "fornecedorId": 12,
  "tipo": "Comercial",
  "cep": "01001000",
  "logradouro": "Praça da Sé",
  "numero": "100",
  "bairro": "Sé",
  "cidade": "São Paulo",
  "estado": "SP",
  "pais": "Brasil",
  "complemento": "Conjunto 12",
  "principal": true,
  "observacao": null
}
```

---

### 1.4 `GET https://viacep.com.br/ws/{cep}/json/` (via `CepService`)
Consulta externa utilizada pelo `CepService.buscarCep(cep)` (`T-017.3`) para autopreenchimento de endereço.

