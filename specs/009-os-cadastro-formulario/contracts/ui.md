# Contrato de Interface: Cadastro de Ordem de Serviço

## Componente

- **Classe**: `OsCadastroComponent`
- **Rota**: `/ordens/novo`
- **Tipo**: componente Angular standalone com Reactive Forms
- **Proteção**: `authGuard` e `ordensPermissionGuard` com `ordens × criar`
- **Fonte de criação**: `OrdensService.create(dto)`

## Campos

| Campo | Controle | Obrigatório | Comportamento |
| --- | --- | ---: | --- |
| Cliente | Autocomplete | Sim | Busca por termo; armazena `clienteId`. |
| Veículo | Dropdown | Sim | Desabilitado até cliente válido; lista apenas veículos do cliente; armazena `veiculoId`. |
| Descrição do problema | Textarea | Sim | Texto não vazio; armazena `descricaoProblema`. |
| Mecânico | Dropdown | Sim | Opções carregadas de `MecanicosService.getAll()`; armazena `mecanicoId`. |

## Serviços e contratos

- `ClientesService.search(term)`: retorna clientes compatíveis com o termo informado.
- `VeiculosService.listByCliente(clienteId)` ou contrato equivalente: retorna somente veículos do cliente.
- `MecanicosService.getAll()`: retorna mecânicos disponíveis.
- `OrdensService.create<CreateOrdemServicoRequest>(dto)`: cria a ordem e retorna o registro com `id`.
- URLs HTTP devem permanecer em `apiPaths`; o componente não monta endpoints.

## DTO de criação

```json
{
  "clienteId": 1,
  "veiculoId": 1,
  "descricaoProblema": "Descrição informada pelo usuário",
  "mecanicoId": 1,
  "status": "Aberta",
  "dataAbertura": null,
  "dataConclusao": null
}
```

## Estados

- **Inicial**: formulário vazio; veículo desabilitado; mecânicos e sugestões podem estar carregando.
- **Buscando cliente**: autocomplete informa carregamento e evita seleção obsoleta.
- **Carregando veículos**: seleção de veículo fica bloqueada até a resposta do cliente atual.
- **Carregando mecânicos**: dropdown informa carregamento.
- **Inválido**: mensagens nos campos obrigatórios; envio bloqueado.
- **Enviando**: botão bloqueado e somente uma requisição ativa.
- **Erro**: Toast com mensagem compreensível; formulário permanece preenchido e editável.
- **Sucesso**: navegação para `/ordens/:id` com ID positivo retornado pela API.

## Autorização

- Sem autenticação: redirecionar para `/login` pelo `authGuard`.
- Sem `ordens × criar`: exibir `Você não tem permissão para acessar esta área.` e redirecionar para `/dashboard`.
- Se a permissão for perdida durante o preenchimento, bloquear o envio e aplicar o mesmo tratamento de acesso negado.
