# Contratos de Interface: Adicionar/Remover Peças e Serviços na OS

## 1. Modais de Gestão de Itens

### 1.1 `OsItemPecaModalComponent`
- **Seletor**: `app-os-item-peca-modal`
- **Inputs**:
  - `visible: boolean` (suporte a two-way binding `[(visible)]`)
  - `ordemServicoId: number`
- **Outputs**:
  - `itemAdded: EventEmitter<OrdemServicoItem>`
  - `visibleChange: EventEmitter<boolean>`
- **Elementos de Formulário**:
  - Busca de peça no estoque: campo de busca com dropdown de sugestões exibindo `Código`, `Descrição`, `Estoque Atual` e `Preço`;
  - Campo Descrição: preenchida automaticamente após seleção da peça;
  - Campo Quantidade: numérico inteiro $\ge 1$;
  - Campo Valor Unitário: numérico formatado em reais;
  - Badge de Subtotal: `quantidade * valorUnitario` formatado via `CurrencyPipe`;
  - Botão "Cancelar": fecha o modal sem salvar;
  - Botão "Adicionar Peça": submete `POST /api/v1/OrdemServicoItens`, desabilitado se inválido ou enviando.

### 1.2 `OsItemServicoModalComponent`
- **Seletor**: `app-os-item-servico-modal`
- **Inputs**:
  - `visible: boolean` (suporte a `[(visible)]`)
  - `ordemServicoId: number`
- **Outputs**:
  - `itemAdded: EventEmitter<OrdemServicoItem>`
  - `visibleChange: EventEmitter<boolean>`
- **Elementos de Formulário**:
  - Campo Descrição do Serviço: `textarea` ou `input` com contador de caracteres (máx. 240);
  - Campo Quantidade: numérico inteiro (padrão 1);
  - Campo Valor da Mão de Obra: numérico decimal;
  - Badge de Subtotal: exibição em destaque do valor total do serviço;
  - Botão "Cancelar": fecha o modal;
  - Botão "Adicionar Serviço": submete `POST /api/v1/OrdemServicoItens` com `pecaId: null`.

## 2. Alterações no `OsDetalheComponent`

### 2.1 Cabeçalho da Seção "Itens da OS"
- Adição de grupo de ações:
  - Botão "Adicionar Peça" (ícone `pi pi-plus`, estilo outlined);
  - Botão "Adicionar Serviço" (ícone `pi pi-plus`, estilo outlined);
  - Botões desabilitados quando `isTerminal() === true` (`Concluida` ou `Cancelada`).

### 2.2 Tabela de Itens da OS
- Nova coluna **Ações**:
  - Botão de excluir com ícone de lixeira (`pi pi-trash`, estilo perigo/texto);
  - Ao clicar, chama `confirmarExclusaoItem(item)`;
  - Coluna oculta quando `isTerminal() === true`.

### 2.3 Resumo Financeiro
- O card de "Total da OS" e o "Saldo Pendente" atualizam automaticamente conforme itens são inseridos ou removidos.
