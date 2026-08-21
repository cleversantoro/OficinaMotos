# Pesquisa: Formulário de Nova Ordem de Serviço

## Decisão 1: Formulário standalone com Reactive Forms

**Decisão**: Implementar `OsCadastroComponent` como componente standalone, usando `ReactiveFormsModule`, Signals para estado de carregamento/erro e os componentes PrimeNG já utilizados no frontend.

**Racional**: A constituição exige componentes standalone e Signals para estado novo. Reactive Forms fornece validação determinística dos quatro campos obrigatórios e facilita bloquear submissões duplicadas.

**Alternativas consideradas**:

- Usar `ngModel` para todo o formulário: rejeitado porque a tela possui dependências entre campos e validações de submissão.
- Criar um módulo Angular específico: rejeitado porque módulos não são permitidos em código novo.

## Decisão 2: Cliente com autocomplete e consulta controlada

**Decisão**: Expor no `ClientesService` um contrato `search(term)` que encapsule o endpoint `apiPaths.clientes.base` existente e seus parâmetros de consulta suportados pelo backend. O componente deve iniciar a busca somente após o termo atingir o limite mínimo definido pelo formulário e cancelar/ignorar respostas obsoletas.

**Racional**: O serviço atual possui `list(params)`, mas não `search(term)`. O requisito pede busca por termo; criar o método no service preserva a centralização de URLs e evita chamada HTTP no componente.

**Alternativas consideradas**:

- Filtrar todos os clientes já carregados no componente: rejeitado porque não escala e não atende a uma busca remota.
- Criar uma URL nova específica sem contrato: rejeitado por violar a centralização de `apiPaths`.

## Decisão 3: Veículos dependentes do cliente

**Decisão**: Expor no `VeiculosService` um método de consulta por `clienteId`, encapsulando `apiPaths.veiculos.base` e seus parâmetros existentes, sem criar nova URL HTTP. Ao trocar o cliente, limpar imediatamente `veiculoId`, desabilitar o controle durante a consulta e substituir as opções pelo resultado do novo cliente.

**Racional**: O veículo só é válido dentro do conjunto do cliente selecionado. Limpar o valor antes da resposta evita enviar uma relação antiga se a consulta falhar ou retornar vazio.

**Alternativas consideradas**:

- Carregar todos os veículos e filtrar na tela: rejeitado por custo e por permitir estado desatualizado.
- Manter o veículo ao trocar o cliente: rejeitado porque permite vínculo inválido.

## Decisão 4: Mecânicos carregados por service

**Decisão**: Expor `getAll()` em `MecanicosService` como contrato semântico da coleção existente, mantendo `apiPaths.mecanicos.base`, e carregar as opções na inicialização do formulário. O campo ficará em estado de carregamento até a resposta e exibirá erro sem permitir envio inválido.

**Racional**: O serviço atual possui `list()`, mas a US-009 define `MecanicosService.getAll()`. O alias explícito mantém a intenção do contrato sem criar endpoint novo.

**Alternativas consideradas**:

- Embutir uma lista fixa de mecânicos: rejeitado porque os dados são dinâmicos.
- Consultar diretamente `HttpClient` no componente: rejeitado porque viola a separação entre UI e service.

## Decisão 5: DTO de criação alinhado à US-007

**Decisão**: Atualizar o tipo `CreateOrdemServicoRequest` para incluir `veiculoId` e usar o DTO no método `OrdensService.create<T>(body)`. O formulário enviará `status: 'Aberta'`, `dataAbertura: null` e os quatro dados obrigatórios.

**Racional**: A US-007 adicionou `VeiculoId` ao backend e o modelo atual do frontend ainda não o representa no request. A atualização corrige o contrato local sem alterar a API.

**Alternativas consideradas**:

- Enviar payload anônimo no componente: rejeitado porque perde a garantia de tipo.
- Reabrir a implementação da US-007: rejeitado porque a dependência já está definida.

## Decisão 6: Navegação após criação

**Decisão**: Após `OrdensService.create()` retornar um `id` inteiro positivo, navegar com `Router.navigate(['/ordens', id])`. Se o identificador não existir ou for inválido, exibir erro e permanecer no formulário.

**Racional**: O destino já é o contrato da US-008 e o array de segmentos evita montar URL manualmente.

**Alternativas consideradas**:

- Redirecionar sempre para `/ordens`: rejeitado porque o aceite exige a ordem criada.
- Usar `window.location`: rejeitado porque quebra a navegação SPA.

## Decisão 7: Autorização e erros

**Decisão**: Reutilizar `ordensPermissionGuard` para proteger a rota e repetir a verificação de `ordens × criar` no envio. Acesso negado exibirá `Você não tem permissão para acessar esta área.` e navegará para `/dashboard`; erros de consulta/criação permanecerão no formulário com Toast de erro.

**Racional**: A verificação no submit cobre perda de permissão durante a sessão e o guard cobre acesso direto à URL.

**Alternativas consideradas**:

- Confiar somente no guard: rejeitado porque a sessão pode mudar antes do envio.
- Navegar para detalhe em erro: rejeitado porque não existe ordem criada válida.

## Incertezas resolvidas

- A aplicação usa Angular 21 standalone, Signals, PrimeNG e Vitest.
- A rota `/ordens/novo` já existe na implementação da US-008 e será substituída pelo formulário.
- `ClientesService`, `VeiculosService` e `MecanicosService` possuem listagens, mas precisarão expor métodos semânticos para os contratos da US-009.
- `OrdensService.create()` existe e precisa ser tipado com o request que inclui `veiculoId`.
