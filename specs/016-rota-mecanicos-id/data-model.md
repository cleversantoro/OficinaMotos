# Modelo de Dados: Detalhe de Mecânico (`/mecanicos/:id`)

**Feature**: `016-rota-mecanicos-id` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md) e [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

---

## 1. Diagrama Entidade-Relacionamento (ERD)

```mermaid
erDiagram
    cad_mecanicos ||--o{ cad_mecanicos_especialidades_rel : "especialidades"
    cad_mecanicos ||--o{ cad_mecanicos_disponibilidades : "disponibilidades"
    cad_mecanicos ||--o{ cad_mecanicos_certificacoes : "certificacoes"
    cad_mecanicos ||--o{ cad_mecanicos_experiencias : "experiencias"
    cad_mecanicos ||--o{ cad_mecanicos_documentos : "documentos"
    cad_mecanicos ||--o{ cad_mecanicos_enderecos : "enderecos"
    cad_mecanicos ||--o{ cad_mecanicos_contatos : "contatos"

    cad_mecanicos {
        bigint Id PK
        string Codigo
        string Nome
        string Sobrenome
        string NomeSocial
        string DocumentoPrincipal
        int TipoDocumento
        date DataNascimento
        date DataAdmissao
        date DataDemissao
        string Status
        bigint EspecialidadePrincipalId FK
        string Nivel
        decimal ValorHora
        int CargaHorariaSemanal
        string Observacoes
    }

    cad_mecanicos_especialidades_rel {
        bigint Id PK
        bigint MecanicoId FK
        bigint EspecialidadeId FK
        string Nivel
        boolean Principal
        string Anotacoes
    }

    cad_mecanicos_disponibilidades {
        bigint Id PK
        bigint MecanicoId FK
        int DiaSemana
        string HoraInicio
        string HoraFim
        int CapacidadeAtendimentos
    }
```

---

## 2. Interfaces TypeScript Consumidas (`src/app/core/models/mecanico.ts`)

| Propriedade (`Mecanico`) | Tipo TypeScript | Origem | Exibição na Página de Detalhe |
|---|---|---|---|
| `id` | `number` | Parâmetro `:id` da rota / API | Identificador interno e link da rota |
| `codigo` | `string` | `MecanicoResponseDTO` | Badge de código funcional (ex.: `MEC-001`) |
| `nome` / `sobrenome` | `string` / `string \| null` | `MecanicoResponseDTO` | Título principal e iniciais do avatar |
| `nivel` | `string` | `MecanicoResponseDTO` | Subtítulo de senioridade (`Junior`, `Pleno`, `Senior`, `Especialista`) |
| `status` | `string` | `MecanicoResponseDTO` | Badge de status (`Ativo` / `Inativo`) |
| `valorHora` | `number` | `MecanicoResponseDTO` | KPI de Valor/Hora (`R$ 0,00`) |
| `cargaHorariaSemanal` | `number` | `MecanicoResponseDTO` | KPI de Carga Horária (`44h`) |
| `documentoPrincipal` | `string` | `MecanicoResponseDTO` | CPF exibido no card lateral de perfil |
| `dataAdmissao` | `string` | `MecanicoResponseDTO` | Data formatada (`dd/mm/aaaa`) no perfil |
| `especialidades` | `MecanicoEspecialidadeRel[]` | `MecanicoResponseDTO` | Aba 0 — Especialidades Técnicas |
| `disponibilidades` | `MecanicoDisponibilidade[]` | `MecanicoResponseDTO` | Aba 1 — Disponibilidade Semanal |
| `certificacoes` | `MecanicoCertificacao[]` | `MecanicoResponseDTO` | Aba 2 — Certificações & Cursos |
| `experiencias` | `MecanicoExperiencia[]` | `MecanicoResponseDTO` | Aba 3 — Experiência Profissional |
| `documentos` | `MecanicoDocumento[]` | `MecanicoResponseDTO` | Aba 4 — Documentos |
| `enderecos` | `MecanicoEndereco[]` | `MecanicoResponseDTO` | Aba 5 — Endereços |
| `contatos` | `MecanicoContato[]` | `MecanicoResponseDTO` | Aba 6 — Contatos |

