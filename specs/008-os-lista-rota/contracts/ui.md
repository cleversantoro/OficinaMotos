# Contrato de Interface: Lista de Ordens

## Componente

- **Nome da classe**: `OsListaComponent`
- **Seletor**: `app-os-lista`
- **Tipo**: componente Angular standalone
- **Responsabilidade**: apresentar a coleção de ordens, estados de consulta, paginação e ações de navegação.
- **Fonte de dados**: `OrdensService.list()` usando `apiPaths.ordens.base`.

## Rotas protegidas

Todas as rotas abaixo permanecem filhas de `MainLayout`, usam lazy loading por `loadComponent` e combinam o `authGuard` com o guard de perfil da área de ordens:

| Caminho | Destino | Regra |
| --- | --- | --- |
| `/ordens` | `OsListaComponent` | Lista paginada; requer consulta de ordens. |
| `/ordens/novo` | Componente de criação da área de ordens | Deve ser registrado antes da rota dinâmica `:id` e requer criação de ordens. |
| `/ordens/:id` | Componente de consulta/detalhe da ordem | `id` deve ser o identificador da linha selecionada e requer consulta de ordens. |

## Ações da tabela

| Ação | Ícone/identificação | Entrada | Resultado |
| --- | --- | --- | --- |
| Visualizar | Ação de consulta da tabela | `OrdemServico` com `id` válido | Navegar para `/ordens/:id`. |
| Excluir | Ação já existente, condicionada ao RBAC | `OrdemServico` com `id` válido | Preservar o comportamento de exclusão autorizado. |

## Cabeçalho

- Título: `Ordens de Serviço`.
- Ação principal: `Nova OS`.
- Destino da ação principal: `/ordens/novo`.
- A ação deve permanecer acessível em viewport menor e somente ser exibida para perfil autorizado a criar ordens.

## Estados

- **Carregando**: `DataTable` recebe `loading=true`.
- **Com dados**: linhas tipadas como `OrdemServico` são exibidas.
- **Sem dados**: mensagem `Nenhuma ordem de serviço cadastrada.` ou mensagem equivalente configurada na tabela.
- **Erro**: mensagem visível de falha na consulta; não tratar erro como lista vazia silenciosa.

## Autorização

- Acesso sem autenticação: redirecionar para `/login` pelo `authGuard`.
- Acesso autenticado sem a permissão `ordens × visualizar`: exibir o Toast `Você não tem permissão para acessar esta área.` e redirecionar para `/dashboard` sem carregar a lista/detalhe.
- Acesso autenticado sem a permissão `ordens × criar`: exibir o mesmo Toast, redirecionar `/ordens/novo` para `/dashboard` e ocultar/desabilitar `Nova OS`.
- A sessão deve transportar a coleção de permissões recebida no login para que o guard não dependa apenas do papel.

## Paginação e filtragem

- Componente responsável: `DataTable` de `shared/ui/data-table`.
- Paginação: local, sem alteração nos parâmetros de `OrdensService.list()`.
- Configuração inicial: 10 linhas por página, com opções 5, 10, 25 e 50.
- Filtro global: habilitado para os campos relevantes da lista.
