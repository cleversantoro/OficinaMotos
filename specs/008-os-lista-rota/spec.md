# Especificação de Funcionalidade: Lista de Ordens e Rota Corrigida

**Feature Branch**: `008-os-lista-rota`

**Criado**: 2026-08-12

**Status**: Draft

**Entrada**: US-008 — Renomear OsDetalhe → OsLista e corrigir rota

## Cenários de Usuário e Testes

### História de Usuário 1 — Consultar lista de ordens (Prioridade: P1)

Como usuário autorizado da oficina, quero acessar `/ordens` e visualizar as ordens de serviço em uma lista paginada, para encontrar rapidamente os atendimentos cadastrados.

**Por que esta prioridade**: A consulta da lista é o fluxo principal da área de ordens e é necessária para localizar registros e iniciar ações sobre eles.

**Teste independente**: Com ordens de serviço disponíveis, acessar `/ordens` e confirmar que a lista é exibida, que a paginação funciona e que a troca de página atualiza os registros apresentados.

**Cenários de aceitação**:

1. **Dado** que o usuário está autenticado e possui acesso às ordens, **quando** acessar `/ordens`, **então** a tela de lista será exibida com os registros disponíveis e controle de paginação.
2. **Dado** que existem mais registros do que o limite da página atual, **quando** o usuário avançar ou retornar uma página, **então** a lista exibirá o conjunto correspondente à página selecionada sem perder o contexto da tela.
3. **Dado** que não existem ordens de serviço, **quando** o usuário acessar `/ordens`, **então** a tela exibirá um estado vazio compreensível e manterá disponível a ação de criar uma nova OS.

### História de Usuário 2 — Acessar ações de uma ordem (Prioridade: P1)

Como usuário autorizado, quero encontrar uma ação em cada item da lista para abrir os detalhes da ordem selecionada, para consultar um registro específico sem ambiguidade.

**Por que esta prioridade**: A lista precisa conectar a consulta geral à visualização individual de uma ordem.

**Teste independente**: Abrir `/ordens` com pelo menos uma ordem e selecionar a ação do item; confirmar que a navegação utiliza o identificador daquele registro.

**Cenários de aceitação**:

1. **Dado** que uma ordem está visível na lista, **quando** o usuário selecionar sua ação de consulta, **então** será direcionado para `/ordens/:id` usando o identificador da ordem selecionada.
2. **Dado** que existem várias ordens na lista, **quando** o usuário selecionar uma ação, **então** somente o identificador do item selecionado será utilizado na navegação.

### História de Usuário 3 — Iniciar nova ordem (Prioridade: P1)

Como usuário autorizado, quero usar o botão “Nova OS” no cabeçalho da lista, para iniciar o cadastro de uma nova ordem de serviço.

**Por que esta prioridade**: A criação é a principal ação de continuidade a partir da consulta das ordens.

**Teste independente**: Acessar `/ordens`, selecionar “Nova OS” e confirmar a navegação para a tela de criação.

**Cenários de aceitação**:

1. **Dado** que o usuário está na lista de ordens, **quando** selecionar o botão “Nova OS”, **então** será direcionado para `/ordens/novo`.
2. **Dado** que o usuário está em uma viewport menor, **quando** visualizar o cabeçalho, **então** o texto do botão continuará legível e a ação permanecerá acessível.

### Casos de Borda

- Se a consulta da lista falhar, a tela deve informar o erro de forma compreensível e não apresentar dados como se fossem válidos.
- Se a lista estiver carregando, a tela deve apresentar um estado de carregamento sem permitir que controles duplicados provoquem navegações concorrentes.
- Se o identificador de uma ordem estiver ausente ou inválido, a ação daquele item não deve gerar uma rota incorreta.
- Se o usuário não estiver autenticado ou não tiver autorização, o fluxo deve respeitar os guards e o tratamento de erros já existentes.
- Se o usuário estiver autenticado, mas não tiver a permissão canônica exigida para a rota, o guard deve exibir Toast de permissão negada e redirecioná-lo para `/dashboard` sem carregar a tela protegida.
- Se a página solicitada deixar de existir após uma alteração nos registros, a lista deve retornar a uma página válida ou exibir um estado vazio.

## Requisitos

### Requisitos Funcionais

- **FR-001**: O sistema DEVE disponibilizar a funcionalidade de lista de ordens por meio de um componente nomeado `OsListaComponent`.
- **FR-002**: O sistema DEVE exibir a lista de ordens na rota `/ordens`.
- **FR-003**: A lista DEVE oferecer paginação e indicar ao usuário a página ou intervalo de registros atualmente exibido.
- **FR-004**: A paginação DEVE permitir avançar e retornar entre páginas válidas sem sair da rota `/ordens`.
- **FR-005**: Cada ordem exibida DEVE apresentar uma ação de consulta que direcione para `/ordens/:id`, substituindo `:id` pelo identificador do registro selecionado.
- **FR-006**: O cabeçalho da lista DEVE apresentar um botão com o texto “Nova OS”.
- **FR-007**: Ao selecionar “Nova OS”, o sistema DEVE direcionar o usuário para `/ordens/novo`.
- **FR-008**: A rota anterior associada ao componente `OsDetalheComponent` não DEVE continuar sendo usada como rota principal da lista após a alteração.
- **FR-009**: A tela DEVE apresentar estados distinguíveis para carregamento, lista vazia e erro de consulta.
- **FR-010**: O fluxo DEVE preservar o `authGuard`, exigir a permissão canônica `ordens × visualizar` para consulta da lista/detalhe e `ordens × criar` para `/ordens/novo`, redirecionando usuários sem autorização para `/dashboard`.
- **FR-011**: A lista DEVE permanecer utilizável em telas menores, sem ocultar ou sobrepor o botão “Nova OS”, os controles de paginação ou a coluna de ações.

### Entidades Principais

- **Ordem de Serviço**: Atendimento da oficina exibido na lista, identificado por um `id` e associado às ações de consulta e criação.
- **Lista de Ordens**: Visão paginada dos registros de ordens de serviço, com estados de carregamento, vazio e erro.
- **Rota de Ordens**: Endereços de navegação da área, incluindo `/ordens`, `/ordens/novo` e `/ordens/:id`.

## Critérios de Sucesso

### Resultados Mensuráveis

- **SC-001**: Em 100% dos testes de navegação, acessar `/ordens` apresenta o componente `OsListaComponent` e a lista correspondente.
- **SC-002**: Em 100% dos testes com mais de uma página de dados, avançar e retornar na paginação exibe registros da página correta sem sair de `/ordens`.
- **SC-003**: Em 100% dos testes de item, a ação de uma ordem direciona para `/ordens/:id` com o identificador correto.
- **SC-004**: Em 100% dos testes do cabeçalho, selecionar “Nova OS” direciona para `/ordens/novo`.
- **SC-005**: Em 100% dos testes automatizados com perfil autorizado para criação, o botão “Nova OS” fica visível e direciona para `/ordens/novo`; em 100% dos testes com perfil sem autorização, o botão não permite iniciar a criação.
- **SC-006**: Em testes automatizados com resposta controlada, a lista apresenta carregamento, estado vazio e erro de consulta em até 2 segundos após cada situação ser identificada pelo sistema.

## Premissas

- O usuário já está autenticado; a autorização para consultar a lista e iniciar a criação é verificada pelas permissões da sessão. O perfil permanece disponível como contexto/fallback compatível.
- A consulta de ordens e seus identificadores já é fornecida pelos serviços existentes do frontend/backend.
- A rota `/ordens/:id` será o destino individual de consulta da ordem.
- A rota `/ordens/novo` será o destino de criação de uma nova ordem e faz parte do escopo desta feature caso ainda não exista.
- A alteração está limitada à organização e navegação da área de ordens; não altera o modelo de dados nem o contrato da API.
- A responsividade deve seguir os componentes e padrões visuais já adotados no frontend.
