# Contratos de API: Cadastro de Mecânico

**Feature**: `015-cadastro-mecanico` | **Data**: 2026-09-13
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Endpoints Utilizados

### 1.1 `POST /api/v1/Mecanicos`
Cria o registro principal do colaborador mecânico.

#### Request Body (`CreateMecanicoDTO`)
```json
{
  "codigo": "MEC-005",
  "nome": "Ricardo",
  "sobrenome": "Mendes",
  "nomeSocial": "Ricardinho",
  "documentoPrincipal": "12345678901",
  "tipoDocumento": 1,
  "dataNascimento": "1990-05-15",
  "dataAdmissao": "2026-09-13",
  "dataDemissao": null,
  "status": "Ativo",
  "especialidadePrincipalId": 2,
  "nivel": "Pleno",
  "valorHora": 80.00,
  "cargaHorariaSemanal": 44,
  "observacoes": "Especialista em injeção eletrônica e motores 4 tempos."
}
```

#### Response Body (`MecanicoResponseDTO`) — HTTP 201 Created
```json
{
  "id": 14,
  "codigo": "MEC-005",
  "nome": "Ricardo",
  "sobrenome": "Mendes",
  "nomeSocial": "Ricardinho",
  "documentoPrincipal": "12345678901",
  "tipoDocumento": 1,
  "dataNascimento": "1990-05-15T00:00:00",
  "dataAdmissao": "2026-09-13T00:00:00",
  "dataDemissao": null,
  "status": "Ativo",
  "especialidadePrincipalId": 2,
  "nivel": "Pleno",
  "valorHora": 80.00,
  "cargaHorariaSemanal": 44,
  "observacoes": "Especialista em injeção eletrônica e motores 4 tempos."
}
```

---

### 1.2 `GET /api/v1/MecanicoEspecialidades`
Retorna a lista de especialidades cadastradas para alimentar o componente multi-select.

#### Response Body — HTTP 200 OK
```json
[
  { "id": 1, "codigo": "ESP-INJ", "nome": "Injeção Eletrônica", "ativo": true },
  { "id": 2, "codigo": "ESP-MOT", "nome": "Motor & Câmbio", "ativo": true },
  { "id": 3, "codigo": "ESP-SUS", "nome": "Freios & Suspensão", "ativo": true },
  { "id": 4, "codigo": "ESP-ELE", "nome": "Parte Elétrica", "ativo": true },
  { "id": 5, "codigo": "ESP-PIN", "nome": "Pintura & Carenagem", "ativo": true }
]
```

---

### 1.3 `POST /api/v1/MecanicoEspecialidadeRel`
Vincula especialidades adicionais ao mecânico recém-criado.

#### Request Body (`CreateMecanicoEspecialidadeRelDTO`)
```json
{
  "mecanicoId": 14,
  "especialidadeId": 3,
  "nivel": "Pleno",
  "principal": false,
  "anotacoes": null
}
```

#### Response Body — HTTP 201 Created
```json
{
  "id": 28,
  "mecanicoId": 14,
  "especialidadeId": 3,
  "nivel": "Pleno",
  "principal": false,
  "anotacoes": null
}
```

