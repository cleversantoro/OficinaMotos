# Contratos de Interface (UI): Cadastro de Peça e Badge de Estoque Crítico

**Feature**: `018-cadastro-peca-estoque` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Formulário de Cadastro de Peça (`/estoque/novo`) — `EstoquePecaCadastroComponent`

### Card 1: Identificação da Peça
- **Código / SKU**: Input obrigatório com sugestão automática (`PEC-XXX`).
- **Descrição da Peça**: Input obrigatório (`min: 3`, `max: 200`).
- **Unidade de Medida**: Select (`UN`, `PAR`, `KIT`, `LT`, `MT`, `KG`).
- **Status**: Select (`Ativo`, `Inativo`).

### Card 2: Classificação, Fabricante & Fornecedor (`T-018.3`)
- **Categoria (`categoriaId`)**: Dropdown dinâmico (`EstoqueService.categorias()`).
- **Fabricante (`fabricanteId`)**: Dropdown dinâmico (`EstoqueService.fabricantes()`).
- **Fornecedor Principal (`fornecedorId`)**: Dropdown dinâmico (`FornecedoresService.list()`).
- **Localização Física (`localizacaoId`)**: Dropdown dinâmico (`EstoqueService.localizacoes()`).

### Card 3: Precificação & Níveis de Estoque
- **Preço Unitário de Venda (R$)**: Input numérico obrigatório (`min: 0.01`, `step: 0.01`).
- **Preço de Custo no Fornecedor (R$)**: Input numérico opcional (`min: 0`).
- **Prazo de Entrega do Fornecedor (dias)**: Input numérico opcional (`min: 0`).
- **Quantidade Inicial**: Input inteiro obrigatório (`min: 0`, default `0`).
- **Estoque Mínimo (`qtd_minima`)**: Input inteiro obrigatório (`min: 0`, default `5`).
- **Estoque Máximo**: Input inteiro obrigatório (`min: 0`, default `50`).

### Card 4: Observações & Aplicações
- Textarea (`max: 500`) para modelos de motos compatíveis e observações técnicas.

---

## 2. Listagem de Estoque (`/estoque`) — `EstoqueListaComponent` (`T-018.4`)

- **Botão "+ Nova peça"**: Navega para `routerLink="/estoque/novo"`.
- **Coluna Estoque**:
  - Exibe a quantidade (`{{ item.quantidade }} {{ item.unidade || 'un' }}`).
  - Quando `item.quantidade < item.estoqueMinimo` (`isCritico(item)`), exibe o badge `<span class="badge-critico">Crítico</span>` com estilização de alerta vermelho (`#ef4444` / `#fca5a5`).

