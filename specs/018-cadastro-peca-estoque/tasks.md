---
description: "Task list for feature 018-cadastro-peca-estoque"
---

# Tasks: US-018 - Criar formulário de cadastro de peça no estoque

**Input**: Design documents from `/specs/018-cadastro-peca-estoque/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21.

## Format: `[ID] [P?] [Story] Description`
- **[P]**: Executável em paralelo (arquivos distintos, sem dependência direta).
- **[Story]**: Mapeamento para as Histórias de Usuário da especificação (`[US1]`, `[US2]`, `[US3]`, `[US4]`).
- Caminhos exatos para cada arquivo modificado/criado.

---

## Phase 1: Setup (Serviço de Estoque)

**Objetivo**: Disponibilizar no `EstoqueService` o método para vincular o fornecedor selecionado à peça recém-cadastrada.

- [x] T001 [US1] [US2] Adicionar método `vincularFornecedor(body: any)` invocando `POST /api/v1/EstoquePecaFornecedores` em `oficina-motos-web/src/app/core/services/estoque.service.ts`

---

## Phase 2: User Story 1 & 2 - `EstoquePecaCadastroComponent` e Dropdowns de Fabricante, Categoria e Fornecedor (Prioridade: P1) 🎯 MVP (T-018.1, T-018.3)

**Objetivo**: Criar o componente standalone `EstoquePecaCadastroComponent` com Reactive Forms, Signals, dropdowns dinâmicos de Fabricante, Categoria, Fornecedor e Localização, e integração com `POST /api/v1/EstoquePecas` e `POST /api/v1/EstoquePecaFornecedores`.

**Teste Independente**: Acessar `/estoque/novo`, preencher código/descrição, selecionar fabricante, categoria e fornecedor nos dropdowns, definir preços e quantidades, e salvar criando o registro com sucesso.

- [x] T002 [US1] [US2] Criar componente `EstoquePecaCadastroComponent` com Angular 21 Standalone, `ChangeDetectionStrategy.OnPush`, Reactive Forms, carregamento de dropdowns (`fabricantes`, `categorias`, `fornecedores`, `localizacoes`) e submissão em `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-cadastro/estoque-peca-cadastro.ts`
- [x] T003 [US1] [US2] Criar template HTML em 4 cards temáticos (Identificação da Peça, Classificação & Fornecedor, Precificação & Níveis de Estoque e Observações) em `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-cadastro/estoque-peca-cadastro.html`
- [x] T004 [US1] Estilizar o formulário no padrão Dark Theme do projeto em `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-cadastro/estoque-peca-cadastro.scss`
- [x] T005 [P] [US1] [US2] Criar suíte de testes unitários para `EstoquePecaCadastroComponent` cobrindo inicialização, carregamento dos dropdowns, validação do formulário e submissão com vínculo de fornecedor em `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-cadastro/estoque-peca-cadastro.spec.ts`

---

## Phase 3: User Story 3 & 4 - Rota `/estoque/novo` e Badge de Estoque Crítico na Listagem (Prioridade: P1) (T-018.2, T-018.4)

**Objetivo**: Registrar a rota `/estoque/novo`, conectar o botão "+ Nova peça" na listagem e exibir o badge `.badge-critico` quando `quantidade < estoqueMinimo` (`qtd < qtd_minima`).

**Teste Independente**: Em `/estoque`, verificar que itens com `quantidade < estoqueMinimo` exibem o badge `"Crítico"` e que clicar em `"+ Nova peça"` navega para `/estoque/novo`.

- [x] T006 [US4] Registrar `{ path: 'estoque/novo', component: EstoquePecaCadastroComponent }` sob o `MainLayout` (`authGuard`) em `oficina-motos-web/src/app/app.routes.ts`
- [x] T007 [US3] [US4] Atualizar `EstoqueLista` (`EstoqueListaComponent`) adicionando `RouterLink`, ajustando `isCritico(item)` para `item.quantidade < item.estoqueMinimo` e exportando alias `EstoqueListaComponent` em `oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.ts`
- [x] T008 [US3] [US4] Atualizar template `estoque-lista.html` adicionando `routerLink="/estoque/novo"` no botão `"+ Nova peça"` e exibindo `<span class="badge-critico">Crítico</span>` quando `isCritico(item)` for verdadeiro em `oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.html`
- [x] T009 [US3] Estilizar `.badge-critico` no padrão Dark Theme em `oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.scss`
- [x] T010 [P] [US3] [US4] Criar suíte de testes unitários para `EstoqueListaComponent` cobrindo exibição do badge de estoque crítico quando `quantidade < estoqueMinimo` e link para `/estoque/novo` em `oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.spec.ts`

---

## Phase 4: Polish & Validação de Qualidade

**Objetivo**: Garantir que todos os testes unitários e o build de produção passem sem erros e atualizar a documentação de governança.

- [x] T011 Executar suíte de testes unitários do frontend com Vitest (`npm test -- --no-watch`)
- [x] T012 Executar compilação de produção (`npm run build`)
- [x] T013 Atualizar status das tarefas `T-018.1`, `T-018.2`, `T-018.3` e `T-018.4` em `governance/backlog.md`
