# Contratos de Interface (UI): Formulário de Cadastro de Mecânico

**Feature**: `015-cadastro-mecanico` | **Data**: 2026-09-13
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Estrutura Visual da Página (`/mecanicos/novo`)

A interface é construída como SPA responsiva com tema escuro (Dark Theme), estruturada em 4 seções em cards:

### Card 1: Identificação Pessoal
- **Código Funcional**: Input de texto, gerado automaticamente como sugestão ou editável (ex.: `MEC-005`).
- **Nome**: Input de texto obrigatório (`min: 2`, `max: 160`).
- **Sobrenome**: Input de texto (`max: 160`).
- **Nome Social / Apelido**: Input de texto (`max: 160`).
- **CPF (Documento Principal)**: Input com máscara `000.000.000-00`, validação de dígitos verificadores e checagem assíncrona de unicidade.
- **Data de Nascimento**: Input tipo `date`.

### Card 2: Dados Profissionais & Contratuais
- **Data de Admissão**: Input tipo `date`, obrigatório (default: hoje).
- **Status**: Select (`Ativo`, `Inativo`, `Afastado`).
- **Nível de Senioridade**: Select (`Junior`, `Pleno`, `Senior`, `Especialista`).
- **Valor da Hora Técnica (R$)**: Input tipo `number` (`step="0.01"`, `min="0"`).
- **Carga Horária Semanal (horas)**: Input tipo `number` (default: `44`).

### Card 3: Especialidades Técnicas (Multi-Select)
- Componente interativo que lista todas as especialidades ativas carregadas do backend.
- Cada especialidade é representada por um card/chip clicável.
- Ao clicar em um card, a especialidade é marcada como selecionada.
- Uma das especialidades selecionadas pode ser eleita como **Principal** através de um botão/ícone com estrela (`⭐ Principal`).
- Se apenas 1 especialidade for selecionada, ela se torna automaticamente a principal.

### Card 4: Observações Gerais
- Textarea com limite de 500 caracteres para histórico, certificações prévias ou notas gerais.

---

## 2. Ações do Formulário
- **Botão Cancelar**: Volta para `/mecanicos` sem persistir dados.
- **Botão Salvar Mecânico**: Botão primário com gradiente laranja `#f97316`, desabilitado com spinner enquanto `submitting()` for verdadeiro.

