# Contratos de UI: Movimentações de Entrada e Saída de Estoque (US-019)

**Branch**: `019-movimentacoes-estoque` | **Data**: 2026-11-04

## 1. Rota `/estoque/:id` (`EstoquePecaDetalheComponent` — `T-019.4`, `T-019.5`)
- **Seletor**: `app-estoque-peca-detalhe`
- **Caminho**: `src/app/features/estoque/pages/estoque-peca-detalhe/estoque-peca-detalhe.ts`
- **Seções da Página**:
  1. **Cabeçalho e Navegação**:
     - Link `← Voltar para Estoque` (`routerLink="/estoque"`).
     - Título da peça (`peca.descricao`), código SKU (`peca.codigo`), badge de status (`Ativo` / `Inativo`) e badge `Crítico` (`badge-critico`) quando `peca.quantidade < peca.estoqueMinimo`.
     - Botões de ação primários:
       - **"+ Registrar Entrada"** (`abrirModalEntrada()`).
       - **"- Registrar Saída"** (`abrirModalSaida()`, desabilitado quando `peca.quantidade <= 0`).
  2. **Cards de Indicadores e Dados Cadastrais**:
     - Saldo Atual (`quantidade` + `unidade`), Estoque Mínimo, Estoque Máximo, Preço Unitário (`R$`).
     - Fabricante, Categoria, Localização e Observações.
  3. **Tabela de Histórico de Movimentações**:
     - Colunas: **Data/Hora**, **Tipo** (badge `ENTRADA` verde / `SAIDA` vermelho), **Quantidade** (`+N` / `-N`), **Referência / Motivo**, **Usuário**.
     - Estado vazio: `"Nenhuma movimentação registrada para esta peça."`

## 2. Modal de Entrada (`EstoqueEntradaModalComponent` — `T-019.1`)
- **Seletor**: `app-estoque-entrada-modal`
- **Caminho**: `src/app/features/estoque/components/estoque-entrada-modal/estoque-entrada-modal.ts`
- **Inputs/Outputs**:
  - `@Input() visible = false`
  - `@Input({ required: true }) peca!: EstoquePeca`
  - `@Output() visibleChange = new EventEmitter<boolean>()`
  - `@Output() saved = new EventEmitter<EstoqueMovimentacao>()`
- **Campos do Formulário Reativo**:
  - `quantidade`: numérico inteiro (`Validators.required`, `Validators.min(1)`)
  - `referencia`: texto (`Validators.required`, `Validators.maxLength(200)`) — ex.: `"NF 12345"`, `"Compra de reposição"`
- **Resumo no Modal**: Exibe o Saldo Atual (`peca.quantidade`) e o Saldo Projetado (`peca.quantidade + quantidade`).

## 3. Modal de Saída (`EstoqueSaidaModalComponent` — `T-019.2`)
- **Seletor**: `app-estoque-saida-modal`
- **Caminho**: `src/app/features/estoque/components/estoque-saida-modal/estoque-saida-modal.ts`
- **Inputs/Outputs**:
  - `@Input() visible = false`
  - `@Input({ required: true }) peca!: EstoquePeca`
  - `@Output() visibleChange = new EventEmitter<boolean>()`
  - `@Output() saved = new EventEmitter<EstoqueMovimentacao>()`
- **Campos do Formulário Reativo**:
  - `quantidade`: numérico inteiro (`Validators.required`, `Validators.min(1)`, e validador customizado/dinâmico verificando `quantidade <= peca.quantidade`)
  - `referencia`: texto (`Validators.required`, `Validators.maxLength(200)`) — ex.: `"OS #102"`, `"Ajuste de inventário"`
- **Resumo no Modal**: Exibe o Saldo Disponível (`peca.quantidade`) e o Saldo Restante Projetado (`peca.quantidade - quantidade`).

