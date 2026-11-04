# Contratos de API: Detalhe de Mecânico (`/mecanicos/:id`)

**Feature**: `016-rota-mecanicos-id` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Endpoints Utilizados

### 1.1 `GET /api/v1/Mecanicos/{id}`
Retorna os dados cadastrais completos de um mecânico específico pelo seu identificador, incluindo todas as suas coleções filhas.

#### Parâmetros de Rota
- `id` (`long`, obrigatório): Identificador único do mecânico.

#### Response Body (`MecanicoResponseDTO`) — HTTP 200 OK
```json
{
  "id": 1,
  "codigo": "MEC-001",
  "nome": "Carlos",
  "sobrenome": "Silva",
  "nomeSocial": null,
  "documentoPrincipal": "12345678909",
  "tipoDocumento": 1,
  "dataNascimento": "1988-03-20T00:00:00",
  "dataAdmissao": "2024-01-10T00:00:00",
  "dataDemissao": null,
  "status": "Ativo",
  "especialidadePrincipalId": 1,
  "nivel": "Senior",
  "valorHora": 95.00,
  "cargaHorariaSemanal": 44,
  "observacoes": "Especialista em motores de alta cilindrada.",
  "especialidades": [
    {
      "id": 10,
      "mecanicoId": 1,
      "especialidadeId": 1,
      "nivel": "Senior",
      "principal": true,
      "anotacoes": "Certificado pela fábrica"
    }
  ],
  "disponibilidades": [
    {
      "id": 5,
      "mecanicoId": 1,
      "diaSemana": 1,
      "horaInicio": "08:00",
      "horaFim": "18:00",
      "capacidadeAtendimentos": 4
    }
  ],
  "certificacoes": [],
  "experiencias": [],
  "documentos": [],
  "enderecos": [],
  "contatos": [
    {
      "id": 3,
      "mecanicoId": 1,
      "tipo": "Celular",
      "valor": "(11) 98888-7777",
      "principal": true,
      "observacao": "WhatsApp"
    }
  ]
}
```

#### Respostas de Erro
- **HTTP 404 Not Found**: Retornado quando não existe mecânico com o `id` informado.
- **HTTP 401 Unauthorized**: Usuário não autenticado (interceptado pelo `authGuard` / `authInterceptor`).

