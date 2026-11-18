# Referência — Design System

## Antes de criar

1. Procure tokens CSS/SCSS e tema atual.
2. Identifique fontes, ícones, cores, raios, sombras e espaçamentos.
3. Localize componentes compartilhados e biblioteca visual.
4. Reutilize o padrão existente se estiver consistente.
5. Se não existir padrão, proponha uma base mínima e mantenha-a centralizada.

## Tokens semânticos sugeridos

- Cores: `--color-primary`, `--color-secondary`, `--color-success`, `--color-warning`, `--color-danger`.
- Superfícies: `--color-background`, `--color-surface`, `--color-surface-muted`.
- Texto: `--color-text`, `--color-text-muted`, `--color-text-inverse`.
- Bordas: `--color-border`, `--radius-sm`, `--radius-md`, `--radius-lg`.
- Espaçamento: escala consistente baseada em múltiplos.
- Tipografia: família, escala, peso e altura de linha.

Os nomes são exemplos; adapte à convenção existente.

## Componentes-base comuns

- Button: variantes, tamanhos, loading e disabled.
- FormField: label, hint, erro e estado.
- Input/Select/Textarea: foco, erro, disabled e acessibilidade.
- Modal/Dialog: foco, teclado, backdrop e viewport.
- Table: cabeçalho, ordenação, vazio, carregamento e paginação.
- Alert/Toast: severidade e mensagem acessível.
- Badge: status com texto, não apenas cor.
- EmptyState: explicação e próxima ação.
- Skeleton: dimensões próximas ao conteúdo final.

## Consistência

Não crie estilos isolados para resolver cada tela. Evite valores arbitrários repetidos e não introduza outra biblioteca visual sem necessidade.
