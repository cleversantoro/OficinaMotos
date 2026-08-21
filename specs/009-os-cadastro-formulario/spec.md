# Especificação de Funcionalidade: Formulário de Nova Ordem de Serviço

**Feature Branch**: `009-os-cadastro-formulario`

**Criado**: 2026-08-12

**Status**: Draft

**Entrada**: US-009 — Criar formulário de nova Ordem de Serviço

**Dependências**: US-007 — Veículo associado à Ordem de Serviço; US-008 — Lista e rotas de Ordens de Serviço

## Cenários de Usuário e Testes

### História de Usuário 1 — Abrir o formulário de nova OS (Prioridade: P1)

Como usuário autorizado da oficina, quero acessar `/ordens/novo` e visualizar um formulário de criação, para registrar um novo atendimento.

**Por que esta prioridade**: Sem o formulário, o botão “Nova OS” da lista não conclui o fluxo principal de criação de ordens.

**Teste independente**: Acessar `/ordens/novo` com uma sessão que possua a permissão `ordens × criar` e confirmar que o formulário e seus campos obrigatórios são exibidos.

**Cenários de aceitação**:

1. **Dado** que o usuário possui a permissão `ordens × criar`, **quando** acessar `/ordens/novo`, **então** o componente `OsCadastroComponent` será exibido.
2. **Dado** que o usuário não possui a permissão `ordens × criar`, **quando** tentar acessar `/ordens/novo`, **então** receberá a mensagem de permissão negada e será redirecionado para `/dashboard`.
3. **Dado** que o formulário está sendo carregado, **quando** as opções de clientes, veículos e mecânicos ainda não estiverem disponíveis, **então** os controles dependentes permanecerão em estado de carregamento e não permitirão seleção inválida.

### História de Usuário 2 — Selecionar cliente e veículo (Prioridade: P1)

Como usuário autorizado, quero buscar um cliente e selecionar um veículo pertencente a ele, para garantir que a ordem seja vinculada ao veículo correto.

**Por que esta prioridade**: A relação entre cliente e veículo é essencial para a integridade da nova ordem e depende da mudança da US-007.

**Teste independente**: Informar um termo de cliente, selecionar um resultado e confirmar que o dropdown de veículos mostra somente veículos daquele cliente.

**Cenários de aceitação**:

1. **Dado** que o usuário informou um termo de busca, **quando** a busca de clientes retornar resultados, **então** o autocomplete exibirá os clientes encontrados para seleção.
2. **Dado** que um cliente foi selecionado, **quando** seus veículos forem carregados, **então** o campo de veículo exibirá somente veículos associados ao cliente selecionado.
3. **Dado** que o cliente selecionado foi alterado, **quando** o novo cliente não possuir veículo previamente selecionado, **então** o veículo anterior será limpo e não poderá ser enviado no formulário.
4. **Dado** que o cliente não possui veículos, **quando** o carregamento terminar, **então** o campo de veículo ficará vazio, desabilitado ou exibirá mensagem orientando o usuário.

### História de Usuário 3 — Preencher e criar a ordem (Prioridade: P1)

Como usuário autorizado, quero preencher descrição e mecânico, enviar o formulário e ser direcionado para a ordem criada, para concluir o registro do atendimento.

**Por que esta prioridade**: Este é o resultado de negócio da funcionalidade e deve conectar os dados selecionados ao contrato de criação da OS.

**Teste independente**: Preencher cliente, veículo, descrição e mecânico com dados válidos, enviar o formulário e confirmar a chamada de criação e a navegação para `/ordens/:id`.

**Cenários de aceitação**:

1. **Dado** que cliente, veículo, descrição e mecânico foram preenchidos, **quando** o usuário enviar o formulário, **então** o sistema enviará os identificadores e a descrição ao serviço de criação.
2. **Dado** que a criação foi concluída com sucesso e retornou um identificador, **quando** a resposta for recebida, **então** o usuário será direcionado para `/ordens/:id` usando o identificador retornado.
3. **Dado** que a criação está em andamento, **quando** o usuário tentar enviar novamente, **então** o sistema impedirá submissões duplicadas e manterá um estado de processamento.
4. **Dado** que a API rejeitou a criação, **quando** o erro for recebido, **então** o formulário permanecerá disponível, exibirá uma mensagem compreensível e não navegará para uma ordem inexistente.

### Casos de Borda

- O usuário tenta enviar sem cliente, veículo, descrição ou mecânico.
- O termo de busca de cliente é vazio ou curto demais para iniciar uma consulta útil.
- A busca de clientes falha ou retorna uma coleção vazia.
- O cliente é selecionado, mas a consulta de veículos falha.
- O cliente é alterado depois de um veículo ter sido escolhido.
- O mecânico não está disponível ou a consulta de mecânicos falha.
- O usuário perde a permissão durante a sessão antes do envio.
- O serviço de criação retorna erro de validação, autenticação, autorização ou indisponibilidade.
- A resposta de criação não contém um identificador válido.
- O usuário recarrega a rota `/ordens/novo` durante uma submissão.

## Requisitos

### Requisitos Funcionais

- **FR-001**: O sistema DEVE disponibilizar a rota protegida `/ordens/novo` para usuários com a permissão `ordens × criar`.
- **FR-002**: A rota `/ordens/novo` DEVE exibir um componente nomeado `OsCadastroComponent`.
- **FR-003**: O formulário DEVE possuir campos obrigatórios para cliente, veículo, descrição do problema e mecânico.
- **FR-004**: O campo de cliente DEVE permitir busca por termo e apresentar resultados selecionáveis por meio do contrato de consulta de clientes.
- **FR-005**: Após a seleção de um cliente, o campo de veículo DEVE carregar e exibir somente veículos vinculados ao cliente selecionado.
- **FR-006**: Quando o cliente for alterado, o sistema DEVE limpar o veículo selecionado e recarregar as opções correspondentes ao novo cliente.
- **FR-007**: O campo de mecânico DEVE carregar opções disponíveis pelo serviço de mecânicos.
- **FR-008**: O sistema DEVE impedir o envio enquanto qualquer campo obrigatório estiver vazio, inválido ou enquanto uma submissão estiver em andamento.
- **FR-009**: Ao enviar dados válidos, o sistema DEVE chamar `OrdensService.create()` com `clienteId`, `veiculoId`, `descricaoProblema`, `mecanicoId` e os valores padrão definidos para status e abertura.
- **FR-010**: Após uma criação bem-sucedida, o sistema DEVE direcionar o usuário para `/ordens/:id` usando o identificador retornado pela criação.
- **FR-011**: Em caso de erro de consulta ou criação, o sistema DEVE manter o formulário disponível, exibir mensagem compreensível e evitar navegação incorreta.
- **FR-012**: O sistema DEVE impedir submissões duplicadas enquanto a criação estiver em andamento.
- **FR-013**: O formulário DEVE usar os serviços existentes e os caminhos centralizados em `apiPaths`, sem hardcodar URLs HTTP no componente.
- **FR-014**: O fluxo DEVE preservar o `authGuard` e o guard de permissão da US-008; usuários sem `ordens × criar` devem receber `Você não tem permissão para acessar esta área.` e retornar para `/dashboard`.
- **FR-015**: O formulário DEVE permanecer utilizável em telas menores, mantendo campos, mensagens, botão de envio e estados de carregamento legíveis.

### Entidades Principais

- **Ordem de Serviço**: registro criado a partir do formulário, vinculando cliente, veículo, mecânico, descrição, status e data de abertura.
- **Cliente**: pessoa ou organização selecionável no autocomplete e proprietária do veículo escolhido.
- **Veículo**: veículo pertencente ao cliente selecionado e obrigatório para a nova ordem.
- **Mecânico**: profissional selecionável responsável pelo atendimento.
- **Sessão de Autorização**: sessão autenticada que deve possuir a permissão `ordens × criar`.

## Critérios de Sucesso

### Resultados Mensuráveis

- **SC-001**: Em 100% dos testes com permissão `ordens × criar`, `/ordens/novo` exibe `OsCadastroComponent` e os quatro campos obrigatórios.
- **SC-002**: Em 100% dos testes de seleção de cliente, o conjunto de veículos exibido corresponde somente ao cliente selecionado.
- **SC-003**: Em 100% dos testes de formulário incompleto, o envio é bloqueado e nenhuma chamada de criação é realizada.
- **SC-004**: Em 100% dos testes com dados válidos, `OrdensService.create()` recebe os quatro identificadores/dados obrigatórios e os valores padrão esperados.
- **SC-005**: Em 100% dos testes de criação bem-sucedida, a aplicação navega para `/ordens/:id` com o identificador retornado.
- **SC-006**: Em 100% dos testes de erro, o formulário permanece visível, apresenta mensagem ao usuário e não navega para uma rota inválida.
- **SC-007**: Em 100% dos testes de submissão duplicada, somente uma criação é enviada enquanto a primeira permanece em andamento.
- **SC-008**: Em 100% dos testes automatizados com cliente, veículo pertencente ao cliente, descrição e mecânico válidos, o formulário permite concluir o envio sem erro de vínculo entre cliente e veículo.

## Premissas

- A US-007 já disponibiliza `veiculoId` no contrato de criação de Ordem de Serviço.
- A US-008 já registra `/ordens/novo`, `OsCadastroComponent` como destino previsto e o guard de `ordens × criar`.
- Os serviços de clientes, veículos e mecânicos existentes continuarão sendo usados; métodos de busca/listagem podem ser adaptados para expor os contratos necessários sem criar URLs fora de `apiPaths`.
- O backend aceita os campos `clienteId`, `veiculoId`, `descricaoProblema` e `mecanicoId` no contrato atual de criação.
- O status inicial será `Aberta` e a data de abertura será definida pelo fluxo padrão da aplicação quando não for informada.
- O usuário possui conexão ativa e está autenticado.
- O escopo não inclui criação de cliente, veículo ou mecânico dentro do formulário.
