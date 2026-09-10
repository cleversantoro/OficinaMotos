# Modelo de Dados: Adicionar/Remover Peças e Serviços na OS

## Entidades e DTOs

### 1. CreateOrdemServicoItemRequest (Payload POST)

Payload enviado para `POST /api/v1/OrdemServicoItens`:

| Propriedade | Tipo | Obrigatório | Descrição |
| --- | --- | :---: | --- |
| `ordemServicoId` | `number` | Sim | Identificador numérico da OS |
| `pecaId` | `number | null` | Não | ID da peça no estoque (`null` para serviços) |
| `descricao` | `string` | Sim | Descrição da peça ou serviço (máx. 240 caracteres) |
| `quantidade` | `number` | Sim | Quantidade aplicada (inteiro $\ge 1$) |
| `valorUnitario` | `number` | Sim | Preço unitário em reais (decimal $\ge 0$) |

### 2. OrdemServicoItem (Entidade Retornada)

Representação do item na OS:

| Propriedade | Tipo | Descrição |
| --- | --- | --- |
| `id` | `number` | Identificador único do item cadastrado |
| `ordemServicoId` | `number` | ID da ordem de serviço vinculada |
| `pecaId` | `number | null` | ID da peça de estoque vinculada |
| `descricao` | `string` | Texto descritivo da peça ou serviço |
| `quantidade` | `number` | Quantidade registrada |
| `valorUnitario` | `number` | Valor por unidade cobrado |
| `total` | `number` | Subtotal computado (`quantidade * valorUnitario`) |

### 3. EstoquePeca (Consulta de Autocomplete)

Dados da peça recuperados do estoque para seleção:

| Propriedade | Tipo | Descrição |
| --- | --- | --- |
| `id` | `number` | Identificador da peça no estoque |
| `codigo` | `string` | Código interno ou SKU da peça |
| `descricao` | `string` | Descrição / nome da peça |
| `precoUnitario` | `number` | Preço de venda praticado |
| `quantidade` | `number` | Saldo físico atual em estoque |
| `status` | `string` | Situação cadastral (`Ativo`, etc.) |

## Estados dos Novos Componentes

### OsItemPecaModalComponent
- `visible`: `model<boolean>(false)` — controle bidirecional de visibilidade do modal;
- `ordemServicoId`: `input.required<number>()` — ID da OS alvo;
- `pecaSelecionada`: `signal<EstoquePeca | null>(null)`;
- `buscaTermo`: `signal<string>('')`;
- `quantidade`: `signal<number>(1)`;
- `valorUnitario`: `signal<number>(0)`;
- `pecasFiltradas`: `signal<EstoquePeca[]>([])`;
- `subtotal`: `computed(() => quantidade() * valorUnitario())`;
- `loading`: `signal<boolean>(false)`;
- `submitting`: `signal<boolean>(false)`.

### OsItemServicoModalComponent
- `visible`: `model<boolean>(false)` — visibilidade do modal;
- `ordemServicoId`: `input.required<number>()` — ID da OS alvo;
- `descricao`: `signal<string>('')`;
- `quantidade`: `signal<number>(1)`;
- `valorUnitario`: `signal<number>(0)`;
- `subtotal`: `computed(() => quantidade() * valorUnitario())`;
- `submitting`: `signal<boolean>(false)`.
