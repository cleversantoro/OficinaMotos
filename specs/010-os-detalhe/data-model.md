# Modelo de Dados: Página de Detalhe Real da Ordem de Serviço

Esta funcionalidade consome os modelos existentes e estrutura o estado de apresentação do detalhe da Ordem de Serviço.

## Entidade Principal de Apresentação

### OrdemServicoDetalheView

| Propriedade | Tipo | Descrição |
| --- | --- | --- |
| `id` | `number` | Identificador único da OS |
| `clienteId` | `number` | Identificador do cliente proprietário |
| `clienteNome` | `string` | Nome/razão social resolvido para exibição |
| `clienteContato` | `string` | Telefone ou e-mail de contato |
| `veiculoId` | `number` | Identificador do veículo vinculado |
| `veiculoDescricao` | `string` | Placa, modelo e marca resolvidos |
| `mecanicoId` | `number` | Identificador do mecânico responsável |
| `mecanicoNome` | `string` | Nome do profissional responsável |
| `descricaoProblema` | `string` | Relato do cliente / diagnóstico inicial |
| `status` | `string` | Status atual (`Aberta`, `EmAndamento`, etc.) |
| `dataAbertura` | `string` | Data e hora de abertura |
| `dataConclusao` | `string | null` | Data e hora de finalização do serviço |
| `itens` | `OrdemServicoItem[]` | Lista de peças e serviços aplicados |
| `observacoes` | `OrdemServicoObservacao[]` | Lista de apontamentos e notas |
| `pagamentos` | `OrdemServicoPagamento[]` | Lista de pagamentos efetuados |

## Sub-Recursos

### OrdemServicoItem

| Campo | Tipo | Descrição |
| --- | --- | --- |
| `id` | `number` | Identificador do item |
| `descricao` | `string` | Nome da peça ou serviço executado |
| `quantidade` | `number` | Quantidade aplicada (unidades ou horas) |
| `valorUnitario` | `number` | Valor por unidade em reais |
| `total` | `number` | Subtotal (`quantidade * valorUnitario`) |

### OrdemServicoObservacao

| Campo | Tipo | Descrição |
| --- | --- | --- |
| `id` | `number` | Identificador da observação |
| `texto` | `string` | Conteúdo do apontamento |
| `usuario` | `string | null` | Nome/identificação de quem registrou |

### OrdemServicoPagamento

| Campo | Tipo | Descrição |
| --- | --- | --- |
| `id` | `number` | Identificador do lançamento |
| `valor` | `number` | Valor pago em reais |
| `metodo` | `string | null` | Forma de pagamento (PIX, Cartão, Dinheiro) |
| `status` | `string` | Situação do pagamento (ex.: Aprovado, Pendente) |
| `dataPagamento` | `string | null` | Data em que o pagamento foi realizado |
| `observacao` | `string | null` | Informações adicionais do pagamento |

## Matriz de Transições de Status

| Status Atual | Transições Permitidas | Indicador / Severity | Terminal? |
| --- | --- | --- | :---: |
| `Aberta` | `EmAndamento`, `Cancelada` | `info` (azul) | Não |
| `EmAndamento` | `AguardandoPeca`, `Concluida`, `Cancelada` | `warn` (amarelo) | Não |
| `AguardandoPeca` | `EmAndamento`, `Cancelada` | `secondary` (roxo/cinza) | Não |
| `Concluida` | *Nenhuma* | `success` (verde) | **Sim** |
| `Cancelada` | *Nenhuma* | `danger` (vermelho) | **Sim** |

## Estados do Componente

- **`loading`** (`boolean`): `true` enquanto carrega a ordem de serviço da API.
- **`error`** (`string | null`): Mensagem de erro caso a OS não exista (404) ou o ID seja inválido.
- **`submittingStatus`** (`boolean`): `true` durante a requisição de atualização de status para desabilitar controles.
- **`statusSelecionado`** (`string | null`): Valor escolhido no dropdown de transição aguardando confirmação.
- **`totalItens`** (`computed<number>`): Soma consolidada de todos os subtotais dos itens.
- **`totalPago`** (`computed<number>`): Soma consolidada de todos os pagamentos realizados.
- **`saldoPendente`** (`computed<number>`): Diferença entre `totalItens` e `totalPago`.
