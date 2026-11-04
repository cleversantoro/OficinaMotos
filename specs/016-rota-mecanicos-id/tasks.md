---
description: "Task list for feature 016-rota-mecanicos-id"
---

# Tasks: US-016 - Registrar rota `/mecanicos/:id`

**Input**: Design documents from `/specs/016-rota-mecanicos-id/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21.

## Format: `[ID] [P?] [Story] Description`
- **[P]**: Executável em paralelo (arquivos distintos, sem dependência direta).
- **[Story]**: Mapeamento para as Histórias de Usuário da especificação (`[US1]`, `[US2]`, `[US3]`).
- Caminhos exatos para cada arquivo modificado/criado.

---

## Phase 1: User Story 1 & 3 - Adaptação de `MecanicoDetalheComponent` e Registro da Rota `/mecanicos/:id` (Prioridade: P1 / P2) 🎯 MVP (T-016.1)

**Objetivo**: Adaptar `MecanicoDetalheComponent` para operar como página roteada independente com carregamento via `ActivatedRoute` e `MecanicosService.get(id)`, tratamento de erros (404 / ID inválido) e registrar a rota `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` em `app.routes.ts`.

**Teste Independente**: Acessar `/mecanicos/1` diretamente e verificar o carregamento dos dados completos do mecânico e das abas de detalhes; acessar `/mecanicos/invalido` ou `/mecanicos/999999` e verificar o card de erro com botão de retorno para `/mecanicos`.

- [x] T001 [US1] [US3] Adaptar `MecanicoDetalhe` (`MecanicoDetalheComponent`) com `OnInit`, injeção de `ActivatedRoute`, `Router` e `MecanicosService`, busca de dados por `:id`, estados `loading`/`error` e exportação de alias `MecanicoDetalheComponent` em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-detalhe/mecanico-detalhe.ts`
- [x] T002 [US1] [US3] Atualizar template HTML para exibir layout de página completa (cabeçalho com botão "← Voltar para Mecânicos", estados de `loading` e `error`, card de perfil/KPIs e 7 abas de detalhes) mantendo compatibilidade modal em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-detalhe/mecanico-detalhe.html`
- [x] T003 [US1] [US3] Adicionar estilos de página roteada, cabeçalho, estados de loading/erro e responsividade Dark Theme em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-detalhe/mecanico-detalhe.scss`
- [x] T004 [US1] Registrar a rota `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` logo após `mecanicos/novo` em `oficina-motos-web/src/app/app.routes.ts`
- [x] T005 [P] [US1] [US3] Criar suíte de testes unitários para `MecanicoDetalheComponent` cobrindo carregamento por rota `:id`, formatação de dados, tratamento de ID inválido/erro 404 e navegação de retorno em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-detalhe/mecanico-detalhe.spec.ts`

---

## Phase 2: User Story 2 - Coluna de Link e Navegação em `MecanicoListaComponent` (Prioridade: P1) (T-016.2)

**Objetivo**: Adicionar coluna/link de navegação direta para `/mecanicos/:id` na listagem de mecânicos e atualizar a ação de visualização para navegar via roteador Angular.

**Teste Independente**: Na tela `/mecanicos`, clicar no link do nome do mecânico ou no botão "Visualizar" e verificar o redirecionamento para `/mecanicos/:id`.

- [x] T006 [US2] Atualizar `MecanicoLista` (`MecanicoListaComponent`) injetando `Router`, atualizando `openDetails()` para navegar para `['/mecanicos', item.id]` e exportando alias `MecanicoListaComponent` em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-lista/mecanico-lista.ts`
- [x] T007 [US2] Adicionar link `[routerLink]="['/mecanicos', m.id]"` na célula de nome do mecânico e atualizar o botão de ação "Visualizar" em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-lista/mecanico-lista.html`
- [x] T008 [US2] Estilizar o link de nome/detalhe com destaque hover no padrão Dark Theme (`#f97316`) em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-lista/mecanico-lista.scss`
- [x] T009 [P] [US2] Criar suíte de testes unitários para `MecanicoListaComponent` cobrindo renderização dos links `['/mecanicos', m.id]` e navegação ao acionar `openDetails()` em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-lista/mecanico-lista.spec.ts`

---

## Phase 3: Polish & Validação de Qualidade

**Objetivo**: Garantir que todos os testes unitários e o build de produção passem sem erros e atualizar a documentação de governança.

- [x] T010 Executar suíte de testes unitários do frontend com Vitest (`npm test -- --no-watch`)
- [x] T011 Executar compilação de produção (`npm run build`)
- [x] T012 Atualizar status das tarefas `T-016.1` e `T-016.2` em `governance/backlog.md`
