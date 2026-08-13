# Modelo de Dados da Interface

Esta feature não altera entidades persistidas, migrations ou o contrato da API. O modelo abaixo descreve somente os dados necessários para a tela de lista.

## OrdemServico

Fonte: `oficina-motos-web/src/app/core/models/ordem-servico.ts` e resposta do endpoint `/api/v1/OrdemServicos`.

| Campo | Tipo | Obrigatório na lista | Uso |
| --- | --- | ---: | --- |
| `id` | `number` | Sim | Identifica a ordem e compõe a rota `/ordens/:id`. |
| `clienteId` | `number` | Sim | Exibe a referência do cliente. |
| `mecanicoId` | `number` | Sim | Exibe a referência do mecânico. |
| `veiculoId` | `number` | Não para a tabela, quando disponível | Mantém o modelo alinhado ao contrato atual da API. |
| `descricaoProblema` | `string` | Sim | Exibe o resumo do problema. |
| `status` | `string` | Sim | Exibe o estado atual da ordem. |
| `dataAbertura` | `string` | Sim | Formata a data de abertura para a apresentação. |
| `dataConclusao` | `string ou null` | Não | Não é necessária para a lista, mas pertence à resposta completa. |
| `itens` | `OrdemServicoItem[]` | Não | Permanece disponível para destinos de detalhe. |

## Lista de Ordens

- **Entrada**: coleção retornada por `OrdensService.list()`.
- **Filtro**: texto global aplicado pela infraestrutura `DataTable` aos campos configurados.
- **Paginação**: controlada pelo paginator do `DataTable`, inicialmente com 10 registros e opções de 5, 10, 25 e 50.
- **Carregamento**: sinalizado enquanto `OrdensService.list()` está pendente.
- **Vazio**: exibido quando a coleção não contém registros após o carregamento.
- **Erro**: exibido quando a consulta falha, sem apresentar a coleção como se estivesse atualizada.

## Regras de Integridade da Interface

- A ação de uma linha só deve navegar quando `id` for um identificador válido.
- O botão de criação deve navegar para `/ordens/novo` sem alterar a coleção da lista.
- A lista não deve criar, atualizar ou excluir dados nesta feature; a exclusão existente deve permanecer sujeita à autorização atual.
- A paginação e o filtro devem operar sobre os dados carregados sem alterar o endpoint ou seus parâmetros.
