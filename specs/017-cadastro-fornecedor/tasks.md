---
description: "Task list for feature 017-cadastro-fornecedor"
---

# Tasks: US-017 - Criar formulário de cadastro de fornecedor

**Input**: Design documents from `/specs/017-cadastro-fornecedor/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21.

## Format: `[ID] [P?] [Story] Description`
- **[P]**: Executável em paralelo (arquivos distintos, sem dependência direta).
- **[Story]**: Mapeamento para as Histórias de Usuário da especificação (`[US1]`, `[US2]`, `[US3]`, `[US4]`).
- Caminhos exatos para cada arquivo modificado/criado.

---

## Phase 1: Setup (Serviço de Fornecedores)

**Objetivo**: Disponibilizar no `FornecedoresService` o método de persistência de endereço comercial do fornecedor.

- [x] T001 [US1] [US2] Adicionar método `criarEndereco(body: any)` invocando `POST /api/v1/FornecedorEnderecos` em `oficina-motos-web/src/app/core/services/fornecedores.service.ts`

---

## Phase 2: User Story 1, 2 & 3 - `FornecedorCadastroComponent`, CNPJ + `CepService` e Dropdown de Segmentos (Prioridade: P1) 🎯 MVP (T-017.1, T-017.3, T-017.4)

**Objetivo**: Criar o componente standalone `FornecedorCadastroComponent` com Reactive Forms, Signals, `cnpjValidator()`, integração com `CepService` e dropdown dinâmico de segmentos.

**Teste Independente**: Acessar `/fornecedores/novo`, validar CNPJ, selecionar segmento no dropdown, buscar CEP preenchendo endereço automaticamente e submeter o formulário criando o fornecedor com sucesso.

- [x] T002 [US1] [US2] [US3] Criar componente `FornecedorCadastroComponent` com Angular 21 Standalone, `ChangeDetectionStrategy.OnPush`, Reactive Forms, `cnpjValidator()`, `cepValidator()`, busca de CEP via `CepService`, carregamento de segmentos via `FornecedoresService.segmentos()` e submissão em `oficina-motos-web/src/app/features/fornecedores/pages/fornecedor-cadastro/fornecedor-cadastro.ts`
- [x] T003 [US1] [US2] [US3] Criar template HTML com os 4 cards temáticos (Identificação & Dados Fiscais, Segmento & Contato, Endereço Comercial com busca de CEP e Condições Comerciais & Observações) em `oficina-motos-web/src/app/features/fornecedores/pages/fornecedor-cadastro/fornecedor-cadastro.html`
- [x] T004 [US1] Estilizar o formulário no padrão Dark Theme do projeto em `oficina-motos-web/src/app/features/fornecedores/pages/fornecedor-cadastro/fornecedor-cadastro.scss`
- [x] T005 [P] [US1] [US2] [US3] Criar suíte de testes unitários para `FornecedorCadastroComponent` cobrindo inicialização, dropdown de segmentos, validação/máscara de CNPJ, autopreenchimento via `CepService` e submissão com sucesso/erro em `oficina-motos-web/src/app/features/fornecedores/pages/fornecedor-cadastro/fornecedor-cadastro.spec.ts`

---

## Phase 3: User Story 4 - Registro da Rota `/fornecedores/novo` (Prioridade: P1) (T-017.2)

**Objetivo**: Registrar a rota `/fornecedores/novo` no roteador Angular antes de `/fornecedores/:id` com proteção `authGuard`.

**Teste Independente**: Clicar em "+ Novo fornecedor" em `/fornecedores` e verificar a abertura de `/fornecedores/novo`.

- [x] T006 [US4] Registrar `{ path: 'fornecedores/novo', component: FornecedorCadastroComponent }` antes de `fornecedores/:id` em `oficina-motos-web/src/app/app.routes.ts`

---

## Phase 4: Polish & Validação de Qualidade

**Objetivo**: Garantir que todos os testes unitários e o build de produção passem sem erros e atualizar a documentação de governança.

- [x] T007 Executar suíte de testes unitários do frontend com Vitest (`npm test -- --no-watch`)
- [x] T008 Executar compilação de produção (`npm run build`)
- [x] T009 Atualizar status das tarefas `T-017.1`, `T-017.2`, `T-017.3` e `T-017.4` em `governance/backlog.md`
