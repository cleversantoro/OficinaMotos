---
description: "Task list for feature 015-cadastro-mecanico"
---

# Tasks: US-015 - Formulário de Cadastro de Mecânico

**Input**: Design documents from `/specs/015-cadastro-mecanico/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`, `governance/arquitetura.md`, `governance/backlog.md`
**Tests**: Testes unitários com Vitest no Angular 21.

## Format: `[ID] [P?] [Story] Description`
- **[P]**: Executável em paralelo (arquivos distintos, sem dependência direta).
- **[Story]**: Mapeamento para as Histórias de Usuário da especificação (`[US1]`, `[US2]`, `[US3]`, `[US4]`).
- Caminhos absolutos/relativos exatos para cada arquivo.

---

## Phase 1: Setup (Infraestrutura Compartilhada e Validadores)

**Objetivo**: Preparação de utilitários e validações reutilizáveis no frontend.

- [x] T001 [P] [US2] Criar validador de CPF único síncrono e assíncrono e formatador em `oficina-motos-web/src/app/shared/validators/cpf-unico-mecanico.validator.ts`
- [x] T002 [P] [US2] Criar testes unitários completos do validador em `oficina-motos-web/src/app/shared/validators/cpf-unico-mecanico.validator.spec.ts`
- [x] T003 [US2] Exportar `cpfUnicoMecanicoValidator`, `cpfUnicoMecanicoAsyncValidator` e `formatCpf` no barrel `oficina-motos-web/src/app/shared/validators/index.ts`

---

## Phase 2: User Story 1 - Cadastro Básico de Mecânico (Prioridade: P1) 🎯 MVP

**Objetivo**: Criar o formulário reativo para cadastro de mecânico com campos de identificação, contratuais e integração com a API `POST /api/v1/Mecanicos`.

**Teste Independente**: Acessar `/mecanicos/novo`, preencher dados básicos e submeter o formulário gerando o registro com sucesso.

- [x] T004 [US1] Criar componente base `MecanicoCadastroComponent` com Angular 21 Standalone, OnPush e Reactive Forms em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-cadastro/mecanico-cadastro.ts`
- [x] T005 [US1] Implementar sugestão automática de código funcional (`MEC-XXX`) e carregamento de dados em `mecanico-cadastro.ts`
- [x] T006 [US1] Criar template HTML com seções estruturadas (Identificação Pessoal, Dados Profissionais e Observações) em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-cadastro/mecanico-cadastro.html`
- [x] T007 [US1] Estilizar o componente no padrão Dark Theme do projeto em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-cadastro/mecanico-cadastro.scss`
- [x] T008 [US1] Criar testes unitários para inicialização do formulário, validação de campos e submissão à API em `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-cadastro/mecanico-cadastro.spec.ts`

---

## Phase 3: User Story 2 - Validação de CPF Único e Formatação (Prioridade: P1)

**Objetivo**: Garantir que o CPF digitado seja válido e único em relação aos mecânicos já existentes no sistema.

**Teste Independente**: Tentar cadastrar um mecânico com CPF inválido ou duplicado e verificar as mensagens de erro em tempo real.

- [x] T009 [US2] Integrar o validador `cpfUnicoMecanicoValidator` no formulário e adicionar tratamento de máscara na digitação em `mecanico-cadastro.ts`
- [x] T010 [US2] Adicionar mensagens e estilos de erro específicos para CPF inválido e CPF duplicado no template `mecanico-cadastro.html`
- [x] T011 [US2] Cobrir validação e detecção de duplicidade de CPF nos testes de componente em `mecanico-cadastro.spec.ts`

---

## Phase 4: User Story 3 - Multi-Select de Especialidades Técnicas (Prioridade: P1)

**Objetivo**: Permitir a seleção de múltiplas especialidades técnicas para o mecânico e a eleição de uma especialidade principal.

**Teste Independente**: Selecionar várias especialidades na tela, definir a principal, salvar e verificar as chamadas para `MecanicosService.vincularEspecialidade()`.

- [x] T012 [US3] Implementar sinais e métodos de gestão de especialidades (`toggleEspecialidade`, `setPrincipal`) e chamadas de vínculo pós-criação em `mecanico-cadastro.ts`
- [x] T013 [US3] Criar interface com chips/cards interativos para especialidades e botão "Tornar Principal" em `mecanico-cadastro.html`
- [x] T014 [US3] Adicionar estilos responsivos para chips de especialidades e badges de destaque em `mecanico-cadastro.scss`
- [x] T015 [US3] Cobrir seleção múltipla e definição de especialidade principal nos testes de componente em `mecanico-cadastro.spec.ts`

---

## Phase 5: User Story 4 - Roteamento e Navegação (Prioridade: P1)

**Objetivo**: Expor o formulário na rota `/mecanicos/novo` e adicionar botão de navegação na listagem de mecânicos.

**Teste Independente**: Navegar a partir de `/mecanicos` para `/mecanicos/novo` clicando no botão "+ Novo Mecânico" e retornar ao cancelar ou salvar.

- [x] T016 [US4] Registrar a rota `/mecanicos/novo` com proteção `authGuard` em `oficina-motos-web/src/app/app.routes.ts`
- [x] T017 [US4] Atualizar o botão "+ Novo Mecânico" em `mecanico-lista.html` com `routerLink="/mecanicos/novo"` e checagem de permissão RBAC (`canCreateMecanico()`) em `mecanico-lista.ts`

---

## Phase 6: Polish & Validação de Qualidade

**Objetivo**: Garantir que toda a suíte de testes e o build de produção passem sem erros.

- [x] T018 Executar testes unitários do frontend com Vitest (`npm test -- --watch=false`)
- [x] T019 Executar compilação de produção (`npm run build`)
- [x] T020 Executar compilação da API backend com `dotnet test`
- [x] T021 Atualizar registros no backlog do projeto em `governance/backlog.md`
