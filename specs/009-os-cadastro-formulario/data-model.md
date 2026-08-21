# Modelo de Dados: Formulário de Nova Ordem de Serviço

Esta feature não cria tabelas nem altera migrations. O modelo descreve o estado do formulário e o contrato de criação usado pela interface.

## OrdemServicoCreate

| Campo | Tipo | Obrigatório | Regra |
| --- | --- | ---: | --- |
| `clienteId` | `number` | Sim | Deve referenciar um cliente selecionado. |
| `veiculoId` | `number` | Sim | Deve pertencer ao cliente selecionado. |
| `descricaoProblema` | `string` | Sim | Não pode ser vazia; respeita o limite do contrato da API. |
| `mecanicoId` | `number` | Sim | Deve referenciar um mecânico selecionado. |
| `status` | `string` | Sim | Valor inicial `Aberta`. |
| `dataAbertura` | string ou nulo | Não | `null` permite que a API aplique o valor padrão. |
| `dataConclusao` | string ou nulo | Não | Deve iniciar como `null`. |

## Estado do Formulário

- **Cliente**: termo de busca, resultados, seleção atual e estado de consulta.
- **Veículo**: coleção dependente do cliente, seleção atual e estado de consulta.
- **Mecânico**: coleção disponível, seleção atual e estado de consulta.
- **Descrição**: texto informado pelo usuário.
- **Submissão**: `idle`, `submitting`, `success` ou `error`.
- **Mensagem de erro**: mensagem inline/Toast sem limpar os dados já preenchidos.

## Relacionamentos

- Um `Cliente` possui zero ou mais `Veiculo`.
- Um `Veiculo` pertence a exatamente um `Cliente` no contexto do formulário.
- Uma `OrdemServicoCreate` referencia um `Cliente`, um `Veiculo` desse cliente e um `Mecanico`.

## Transições

1. Cliente selecionado: limpar veículo, carregar veículos do cliente e habilitar o campo quando houver opções.
2. Cliente alterado: invalidar o veículo anterior antes de qualquer nova consulta.
3. Formulário válido enviado: bloquear controles e enviar uma única requisição.
4. Criação bem-sucedida com ID válido: navegar para `/ordens/:id`.
5. Erro ou ID inválido: liberar o formulário, preservar entradas e exibir mensagem.
