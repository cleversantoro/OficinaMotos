---
description: "Task list for feature 019-movimentacoes-estoque"
---

# Tasks: US-019 - Registrar movimentações de entrada e saída de estoque

**Input**: Design documents from `/specs/019-movimentacoes-estoque/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21.

## Format: `[ID] [P?] [Story] Description`
- **[P]**: Executável em paralelo (arquivos distintos, sem dependência direta).
- **[Story]**: Mapeamento para as Histórias de Usuário da especificação (`[US1]`, `[US2]`, `[US3]`, `[US4]`).
- Caminhos exatos para cada arquivo modificado/criado.

---

## Phase 1: Setup (Serviço de Estoque - T-019.3)

**Objetivo**: Disponibilizar no `EstoqueService` o método `listarMovimentacoes(params?)` (`GET /api/v1/EstoqueMovimentacoes`) mantendo `movimentar(body)` (`POST /api/v1/EstoqueMovimentacoes`) e `atualizarPeca(id, body)` (`PUT /api/v1/EstoquePecas/:id`).

- [x] T001 [US1] [US2] [US3] Adicionar método `listarMovimentacoes(params?: Record<string, any>)` invocando `GET /api/v1/EstoqueMovimentacoes` em `oficina-motos-web/src/app/core/services/estoque.service.ts`

---

## Phase 2: User Story 2 & 3 - Modais de Entrada e Saída de Estoque (Prioridade: P1) 🎯 MVP (T-019.1, T-019.2, T-019.3)

**Objetivo**: Criar os componentes standalone `EstoqueEntradaModalComponent` e `EstoqueSaidaModalComponent` com `<p-dialog>`, Signals, Reactive Forms e integração com `POST /api/v1/EstoqueMovimentacoes` (`EstoqueService.movimentar()`) e atualização de saldo da peça (`EstoqueService.atualizarPeca()`).

**Teste Independente**: Abrir `EstoqueEntradaModalComponent` ou `EstoqueSaidaModalComponent` com uma peça mockada, validar campos obrigatórios e limites de quantidade (`>= 1` para entrada; `1 <= qtd <= peca.quantidade` para saída) e submeter verificando a chamada a `EstoqueService.movimentar` e `EstoqueService.atualizarPeca`.

- [x] T002 [P] [US2] Criar componente `EstoqueEntradaModalComponent` com Angular 21 Standalone, `ChangeDetectionStrategy.OnPush`, `<p-dialog>`, Reactive Forms (`quantidade >= 1`, `referencia` obrigatória) e integração `movimentar()` + `atualizarPeca()` em `oficina-motos-web/src/app/features/estoque/components/estoque-entrada-modal/estoque-entrada-modal.ts`
- [x] T003 [P] [US2] Criar template HTML e estilos Dark Theme do modal de entrada em `oficina-motos-web/src/app/features/estoque/components/estoque-entrada-modal/estoque-entrada-modal.html` e `oficina-motos-web/src/app/features/estoque/components/estoque-entrada-modal/estoque-entrada-modal.scss`
- [x] T004 [P] [US2] Criar suíte de testes unitários para `EstoqueEntradaModalComponent` cobrindo validações do formulário, chamada a `POST /api/v1/EstoqueMovimentacoes` (`tipo: 'ENTRADA'`) e incremento do saldo da peça em `oficina-motos-web/src/app/features/estoque/components/estoque-entrada-modal/estoque-entrada-modal.spec.ts`
- [x] T005 [P] [US3] Criar componente `EstoqueSaidaModalComponent` com Angular 21 Standalone, `ChangeDetectionStrategy.OnPush`, `<p-dialog>`, Reactive Forms (`1 <= quantidade <= peca.quantidade`, `referencia` obrigatória) e integração `movimentar()` + `atualizarPeca()` em `oficina-motos-web/src/app/features/estoque/components/estoque-saida-modal/estoque-saida-modal.ts`
- [x] T006 [P] [US3] Criar template HTML e estilos Dark Theme do modal de saída em `oficina-motos-web/src/app/features/estoque/components/estoque-saida-modal/estoque-saida-modal.html` e `oficina-motos-web/src/app/features/estoque/components/estoque-saida-modal/estoque-saida-modal.scss`
- [x] T007 [P] [US3] Criar suíte de testes unitários para `EstoqueSaidaModalComponent` cobrindo bloqueio de quantidade acima do saldo disponível, chamada a `POST /api/v1/EstoqueMovimentacoes` (`tipo: 'SAIDA'`) e decremento do saldo da peça em `oficina-motos-web/src/app/features/estoque/components/estoque-saida-modal/estoque-saida-modal.spec.ts`

---

## Phase 3: User Story 1 & 4 - `EstoquePecaDetalheComponent` com Histórico e Rota `/estoque/:id` (Prioridade: P1) (T-019.4, T-019.5)

**Objetivo**: Criar a página `EstoquePecaDetalheComponent` exibindo os dados cadastrais da peça, indicadores de saldo (incluindo badge `Crítico`), botões para abrir os modais de Entrada e Saída, e tabela cronológica de histórico de movimentações; registrar a rota `/estoque/:id` e adicionar navegação a partir de `EstoqueListaComponent`.

**Teste Independente**: Acessar `/estoque/10` (ou clicar na peça em `/estoque`), visualizar os dados cadastrais e o histórico de movimentações, abrir os modais de Entrada e Saída e verificar a atualização da tela após salvar.

- [x] T008 [US1] Criar componente `EstoquePecaDetalheComponent` carregando dados da peça, metadados auxiliares e histórico de movimentações (`pecaId === id` ordenado por `dataMovimentacao` decrescente) e integrando `EstoqueEntradaModalComponent` e `EstoqueSaidaModalComponent` em `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-detalhe/estoque-peca-detalhe.ts`
- [x] T009 [US1] Criar template HTML e estilos Dark Theme da página de detalhe e histórico de movimentações em `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-detalhe/estoque-peca-detalhe.html` e `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-detalhe/estoque-peca-detalhe.scss`
- [x] T010 [P] [US1] Criar suíte de testes unitários para `EstoquePecaDetalheComponent` cobrindo carregamento da peça e histórico, badge de estoque crítico e recarregamento após movimentação em `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-detalhe/estoque-peca-detalhe.spec.ts`
- [x] T011 [US1] [US4] Registrar `{ path: 'estoque/:id', component: EstoquePecaDetalheComponent }` logo após `estoque/novo` em `oficina-motos-web/src/app/app.routes.ts`
- [x] T012 [US4] Atualizar `estoque-lista.html`, `estoque-lista.scss` e `estoque-lista.spec.ts` adicionando `[routerLink]="['/estoque', item.id]"` no nome da peça e no botão de ação `"Visualizar"` em `oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.html`

---

## Phase 4: Polish & Validação de Qualidade

**Objetivo**: Garantir que todos os testes unitários e o build de produção passem sem erros e atualizar a documentação de governança.

- [x] T013 Executar suíte de testes unitários do frontend com Vitest (`npm test -- --no-watch`)
- [x] T014 Executar compilação de produção (`npm run build`)
- [x] T015 Atualizar status das tarefas `T-019.1`, `T-019.2`, `T-019.3`, `T-019.4` e `T-019.5` em `governance/backlog.md`
