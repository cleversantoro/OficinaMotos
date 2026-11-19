Atue como um Engenheiro Frontend Sênior e Especialista em UI/UX. Preciso estabelecer uma base visual e arquitetural rigorosa e padronizada para este projeto antes de criarmos ou refatorarmos novas telas.

Analise a estrutura atual do projeto (stack, bibliotecas instaladas, CSS/Tailwind) e implemente/configure os seguintes padrões globais:

### 1. Design System e Componentização Padrão
- **Preferência por Angular Material com confirmação prévia**: Ao criar ou alterar páginas ou componentes, o uso do **Angular Material** (`@angular/material` / `@angular/cdk`) deve ser preferido. **O agente deve sempre perguntar primeiro ao usuário** antes de aplicar ou alterar componentes com Angular Material.
- Centralize elementos visuais customizados compartilhados em `src/app/shared/ui` (como `<app-datatable>` e `<app-confirm-modal>`).
- **Regra de Ouro:** Nenhuma tela pode criar inputs, botões, modais, badges ou cards "do zero" com estilos inline ou classes soltas sem antes priorizar o Angular Material (com confirmação do usuário) ou os componentes compartilhados padrão.

### 2. Padrão Único de Tabelas (`DataTable`)
- Crie um componente genérico e reutilizável `<DataTable />` que será obrigatório para qualquer listagem do sistema.
- Deve conter por padrão: cabeçalho fixo (`sticky header`), ordenação por coluna, filtro/busca integrada, paginação consistente, estado de carregamento (*Skeleton*), estado vazio (*Empty State* ilustrado/informativo) e alinhamento correto (textos à esquerda, números/valores/datas alinhados, ações à direita).

### 3. Sistema Global de Notificações (`Toasts`)
- Configure um provedor global de Toasts (ex: Sonner ou equivalente) no layout raiz da aplicação.
- Padronize variantes semânticas (`success`, `error`, `warning`, `info`), posição fixa na tela, tempo de duração e feedback claro para toda ação assíncrona (salvar, excluir, erro de API).

### 4. Controle Rigoroso de Viewport e Layout (Sem quebra vertical)
- O layout principal deve respeitar `h-screen` / `100dvh` com `overflow-hidden` no container raiz (`AppShell`), mantendo a Sidebar e o Header/Topbar fixos.
- Apenas a área principal de conteúdo (`<main>`) ou o interior de tabelas/modais pode ter rolagem controlada (`overflow-y-auto`), evitando barras de rolagem duplas ou quebra de layout vertical em diferentes resoluções.

### 5. Tema Claro e Escuro (Dark / Light Mode)
- Implemente suporte nativo a Tema Claro e Tema Escuro via variáveis CSS semânticas (tokens como `--background`, `--foreground`, `--card`, `--primary`, `--muted`, `--border`) e persistência da escolha do usuário (respeitando também a preferência do sistema operacional).
- Garanta contraste adequado (WCAG AA) em ambos os temas.

### 6. UX, Intuitividade e Affordance
- **Affordance clara:** Elementos clicáveis devem parecer clicáveis (`cursor-pointer`, estados claros de `:hover`, `:focus-visible`, `:active` e `:disabled`).
- **Prevenção de erros:** Ações destrutivas (excluir/cancelar) devem exigir confirmação via modal/dialog e usar cores de alerta (`destructive`).
- **Feedback imediato:** Botões de submissão devem exibir estado de `loading` (spinner + `disabled`) enquanto aguardam resposta, evitando cliques duplos.
- **Hierarquia visual:** Uso consistente de espaçamentos (escala de 4px/8px), tipografia e agrupamento lógico de informações.

### Tarefa Imediata:
1. Verifique o que já existe no projeto e o que falta instalar/configurar.
2. Crie um arquivo de documentação/regras internas (ex: `UI_GUIDELINES.md` na raiz) consolidando todas essas diretrizes para que qualquer código futuro gerado neste repositório siga esse padrão automaticamente.
3. Implemente a estrutura base (Tokens de Tema, AppShell sem quebra vertical, Toast Provider e o componente padrão de DataTable) e me apresente um resumo do que foi criado.