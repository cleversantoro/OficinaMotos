# Pesquisa Técnica: Registrar rota `/mecanicos/:id` e Link na Listagem

**Feature**: `016-rota-mecanicos-id` | **Data**: 2026-11-04
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

---

## 1. Contexto e Motivação

No módulo de Mecânicos (`features/mecanicos`), a visualização dos detalhes de um colaborador está atualmente acoplada a um modal `<p-dialog>` em `MecanicoDetalhe` acionado via `@Input() mecanico` e `@Input() visible` dentro de `MecanicoLista`. Contudo, as demais entidades principais do sistema (`ClienteDetalhe` em `/clientes/:id` e `VeiculoDetalhe` em `/motos/:id`) utilizam rotas dedicadas que permitem deep-linking, compartilhamento de URL, navegação histórica do navegador e melhor espaço visual.

A história **US-016** estabelece:
- **T-016.1**: Adicionar `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` em `app.routes.ts`.
- **T-016.2**: Adicionar coluna de link (e navegação por rota) em `MecanicoListaComponent`.

---

## 2. Diagnóstico Técnico dos Componentes Atuais

### 2.1 Roteamento (`src/app/app.routes.ts`)
Atualmente, as rotas de mecânicos registradas são:
```typescript
{
  path: 'mecanicos',
  component: MecanicoLista,
},
{
  path: 'mecanicos/novo',
  component: MecanicoCadastroComponent,
},
```
Para atender à T-016.1 sem conflitar com `mecanicos/novo`, a rota dinâmica `mecanicos/:id` deve ser posicionada imediatamente após `mecanicos/novo`:
```typescript
{
  path: 'mecanicos/:id',
  component: MecanicoDetalheComponent,
},
```

### 2.2 Componente de Detalhe (`src/app/features/mecanicos/pages/mecanico-detalhe/`)
- O componente atual exporta a classe `MecanicoDetalhe`. Para atender estritamente à nomenclatura `MecanicoDetalheComponent` (e manter retrocompatibilidade com imports existentes de `MecanicoDetalhe`), exportaremos `MecanicoDetalheComponent` e `MecanicoDetalhe` (via alias).
- Quando instanciado via rota (`/mecanicos/:id`):
  - `ActivatedRoute` fornece o parâmetro `id`.
  - O componente invoca `MecanicosService.get(id)` no `ngOnInit()`.
  - Também busca o catálogo de especialidades (`MecanicosService.especialidades()`) se necessário para exibir o nome amigável da especialidade além do ID.
  - Renderiza o conteúdo diretamente na página (com cabeçalho de navegação "← Voltar para Mecânicos"), sem depender de `<p-dialog>` se estiver em modo página (`isRoutedPage`), mantendo total compatibilidade caso ainda seja instanciado como modal via `@Input() visible`.

### 2.3 Componente de Listagem (`src/app/features/mecanicos/pages/mecanico-lista/`)
- O componente exporta `MecanicoLista` (e adicionaremos alias `MecanicoListaComponent`).
- Na tabela de mecânicos (`mecanico-lista.html`):
  - O nome do mecânico e/ou uma coluna/botão de link utilizará `[routerLink]="['/mecanicos', m.id]"` para permitir navegação direta para `/mecanicos/:id`.
  - O botão de ação "Visualizar" navegará para `['/mecanicos', m.id]`.

---

## 3. Análise de Backend e Banco de Dados (MySQL 8.0)

- **Endpoint existente**: `GET /api/v1/Mecanicos/{id}` em `MecanicosController.cs` já retorna `MecanicoResponseDTO` contendo dados cadastrais e todas as 7 coleções relacionais (`certificacoes`, `contatos`, `disponibilidades`, `documentos`, `enderecos`, `especialidades`, `experiencias`).
- **Mudanças de schema**: Nenhuma alteração de tabela, coluna ou índice é necessária no MySQL 8.0. Portanto, não há geração de migration ou script SQL para esta US.

