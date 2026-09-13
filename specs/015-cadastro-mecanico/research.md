# Pesquisa Técnica: Formulário de Cadastro de Mecânico

**Feature**: `015-cadastro-mecanico` | **Data**: 2026-09-13
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

---

## 1. Contexto e Motivação

O Bounded Context de Cadastro de Colaboradores / Mecânicos é responsável por gerenciar a equipe técnica da Oficina MotoPro. Na Sprint 3, a história **US-015** prevê a criação da tela `/mecanicos/novo`, preenchendo a lacuna existente onde apenas a listagem (`MecanicoLista`) estava implementada no frontend, enquanto o backend (.NET 8 Clean Architecture) já disponibiliza todos os endpoints necessários:
- `POST /api/v1/Mecanicos`: Criação da entidade `Mecanico` (`CreateMecanicoDTO`).
- `GET /api/v1/Mecanicos`: Listagem de mecânicos (usada para consulta de unicidade de CPF e tabela).
- `GET /api/v1/MecanicoEspecialidades`: Consulta das especialidades técnicas cadastradas.
- `POST /api/v1/MecanicoEspecialidadeRel`: Associação N:N entre Mecânico e Especialidade técnica.

---

## 2. Análise do Modelo de Dados e Endpoints

### 2.1 Entidade `Mecanico` (`cad_mecanicos`)
- `Id`: Identificador único (long)
- `Codigo`: Código funcional (ex.: `MEC-001`)
- `Nome`: Primeiro nome (obrigatório, max 160)
- `Sobrenome`: Sobrenome (opcional, max 160)
- `NomeSocial`: Nome social / apelido (opcional, max 160)
- `DocumentoPrincipal`: CPF do mecânico (obrigatório, 11 dígitos numéricos, único)
- `TipoDocumento`: 1 (CPF)
- `DataNascimento`: DateTime? (opcional)
- `DataAdmissao`: DateTime (obrigatório, default data corrente)
- `DataDemissao`: DateTime? (opcional)
- `Status`: "Ativo" / "Inativo" / "Afastado" (default: "Ativo")
- `EspecialidadePrincipalId`: long? (FK para `cad_mecanicos_especialidades`)
- `Nivel`: "Junior" / "Pleno" / "Senior" / "Especialista"
- `ValorHora`: decimal (obrigatório, >= 0)
- `CargaHorariaSemanal`: int (default: 44)
- `Observacoes`: string? (max 500)

### 2.2 Entidade `MecanicoEspecialidade` (`cad_mecanicos_especialidades`)
Especialidades pré-cadastradas no sistema:
- Injeção Eletrônica
- Motor & Transmissão
- Freios & Suspensão
- Parte Elétrica & Iluminação
- Pintura & Carenagem
- Pneus, Rodas & Alinhamento

### 2.3 Relação N:N `MecanicoEspecialidadeRel` (`cad_mecanicos_especialidades_rel`)
- `MecanicoId`: long (FK)
- `EspecialidadeId`: long (FK)
- `Nivel`: string (ex.: "Pleno")
- `Principal`: bool (se é a especialidade principal)

---

## 3. Validador de CPF Único (T-015.3)

### 3.1 Desafios Técnicos
1. **Validação de formato e dígitos verificadores**: O algoritmo já existe em `shared/validators/document-validator.ts` (`cpfValidator()`).
2. **Validação de unicidade**: O validador deve receber a lista de mecânicos (ou o `MecanicosService`) para checar se o CPF digitado (sem máscara) já existe para outro mecânico cadastrado no sistema.
3. **Máscara visual**: Formatação `000.000.000-00` no input para melhor experiência do usuário (UX), mas persistência limpa de 11 dígitos numéricos.

---

## 4. Multi-Select de Especialidades (T-015.4)

### 4.1 Requisitos de UX
- Carregar as especialidades via `MecanicosService.especialidades()`.
- Exibir cards ou badges com checkbox permitindo marcar múltiplas especialidades.
- Destacar uma especialidade como a **Principal** (com ícone de estrela / badge "Principal").
- Ao submeter o formulário:
  1. Cria o mecânico com `especialidadePrincipalId`.
  2. Executa requisições de vínculo para as demais especialidades selecionadas através de `vincularEspecialidade()`.

---

## 5. Roteamento e Acesso (T-015.2)

- Adicionar rota em `app.routes.ts`:
  ```typescript
  { path: 'mecanicos/novo', component: MecanicoCadastroComponent },
  ```
  posicionada antes de rotas dinâmicas como `mecanicos/:id`.
- Atualizar o botão "+ Novo Mecânico" em `mecanico-lista.html` com `routerLink="/mecanicos/novo"`.

