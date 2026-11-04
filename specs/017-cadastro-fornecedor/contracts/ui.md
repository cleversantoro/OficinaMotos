# Contratos de Interface (UI): Formulário de Cadastro de Fornecedor

**Feature**: `017-cadastro-fornecedor` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Estrutura Visual da Página (`/fornecedores/novo`)

A interface segue o padrão Dark Theme (`#0f172a` / `#1e293b` / destaque `#f97316`), organizada em cabeçalho + 4 cards de formulário:

### Cabeçalho
- Link de retorno **"← Voltar para Fornecedores"** (`routerLink="/fornecedores"`).
- Título **"Novo Fornecedor"** e subtítulo descritivo.

### Card 1: Identificação Jurídica & Fiscal
- **Código**: Input de texto com sugestão automática (`FOR-XXX`).
- **Tipo de Fornecedor**: Select (`DISTRIBUIDOR`, `FABRICANTE`, `IMPORTADOR`, `VAREJISTA`, `PRESTADOR_SERVICO`).
- **Status**: Select (`ATIVO`, `INATIVO`, `SUSPENSO`).
- **Razão Social**: Input de texto obrigatório (`min: 3`, `max: 200`).
- **Nome Fantasia**: Input de texto obrigatório (`min: 2`, `max: 160`).
- **CNPJ (`documento`)**: Input obrigatório com máscara `00.000.000/0000-00` e validação por `cnpjValidator()` (`T-017.3`).
- **Inscrição Estadual** e **Inscrição Municipal**: Inputs opcionais.

### Card 2: Segmento de Atuação & Contato
- **Segmento Principal (`segmentoPrincipalId`)**: Dropdown dinâmico alimentado por `FornecedoresService.segmentos()` (`T-017.4`).
- **E-mail Comercial**: Input com validação `Validators.email`.
- **Telefone / WhatsApp Principal**: Input com máscara `(00) 00000-0000`.
- **Website**: Input de URL opcional.

### Card 3: Endereço Comercial (Integração `CepService` — `T-017.3`)
- **CEP**: Input com máscara `00000-000`, validação `cepValidator()` e botão/gatilho automático para `CepService.buscarCep()`.
- **Logradouro**, **Número**, **Complemento**, **Bairro**, **Cidade** e **Estado (UF)**: Autopreenchidos pelo `CepService` e editáveis manualmente.

### Card 4: Condições Comerciais & Observações
- **Condição de Pagamento Padrão**: Input de texto (ex.: `28/56 dias`).
- **Prazo Médio de Entrega (dias)**: Input numérico (`min="0"`).
- **Prazo de Garantia Padrão**: Input de texto (ex.: `90 dias`).
- **Checkboxes**: `Atendimento Personalizado` e `Permite Retirada Local`.
- **Observações**: Textarea (`max: 500`).

---

## 2. Barra de Ações do Formulário
- **Botão Cancelar**: Retorna para `/fornecedores`.
- **Botão Salvar Fornecedor**: Botão CTA laranja (`#f97316`), exibe estado `"Salvando..."` enquanto `submitting()` estiver ativo.

