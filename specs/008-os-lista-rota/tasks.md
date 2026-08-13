# Tarefas: Lista de Ordens e Rota Corrigida

**Entrada**: Documentos de design em `/specs/008-os-lista-rota/`

**Pré-requisitos**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/ui.md` e `quickstart.md`

**Organização**: As tarefas estão agrupadas por história de usuário e seguem a ordem de execução.

## Fase 1: Preparação

**Objetivo**: Preparar a estrutura do frontend sem alterar a API ou o banco.

- [X] T001 [P] Renomear a pasta `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/` para `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/`
- [X] T002 Atualizar os nomes dos arquivos para `os-lista.ts`, `os-lista.html` e `os-lista.scss` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/`, após T001
- [X] T003 [P] Atualizar `oficina-motos-web/src/app/core/auth/auth.model.ts` e `oficina-motos-web/src/app/core/auth/auth.service.ts` para transportar e persistir as permissões recebidas no login
- [ ] T004 Criar testes de login, persistência, restauração e expiração das permissões em `oficina-motos-web/src/app/core/auth/auth.service.spec.ts`

## Fase 2: Fundamentos Compartilhados

**Objetivo**: Preparar autorização, destinos de rota e componentes base antes das histórias.

- [X] T005 Renomear a classe e o seletor para `OsListaComponent` e `app-os-lista` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`
- [X] T006 Configurar `ChangeDetectionStrategy.OnPush`, `OrdemServico[]`, Signals para estado novo e `OrdensService` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`
- [X] T007 Criar testes do helper para permissões presentes, ausentes e normalização de `ordens × visualizar` e `ordens × criar` em `oficina-motos-web/src/app/core/auth/rbac-access.helper.spec.ts`; estes testes devem falhar antes da implementação do helper
- [X] T008 Estender `oficina-motos-web/src/app/core/auth/rbac-access.helper.ts` para validar permissões canônicas `visualizar` e `criar` por módulo, mantendo fallback de papel somente por compatibilidade explícita
- [X] T009 Criar `oficina-motos-web/src/app/core/auth/ordens-permission.guard.ts` para exigir `ordens × visualizar` em `/ordens` e `/ordens/:id`, `ordens × criar` em `/ordens/novo`, exibir o Toast `Você não tem permissão para acessar esta área.` e redirecionar para `/dashboard`
- [X] T010 [P] Criar testes do guard para acesso autorizado, permissão ausente, Toast e redirecionamento em `oficina-motos-web/src/app/core/auth/ordens-permission.guard.spec.ts`
- [X] T011 [P] Criar o componente standalone de criação em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`, `os-novo.html` e `os-novo.scss`
- [X] T012 [P] Criar o componente standalone de detalhe em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.ts`, `os-detalhe.html` e `os-detalhe.scss`, carregando o `id` da rota
- [X] T013 Registrar `/ordens`, `/ordens/novo` e `/ordens/:id` em `oficina-motos-web/src/app/app.routes.ts` com `loadComponent`, guards e ordem específica antes do wildcard

**Marco**: A base de sessão, autorização, lazy loading e destinos de navegação está pronta.

## Fase 3: História de Usuário 1 - Consultar Lista de Ordens (Prioridade: P1) MVP

**Objetivo**: Exibir `/ordens` como lista paginada com estados de carregamento, vazio e erro.

**Teste independente**: Consultar a rota com zero, um e vários registros e confirmar tabela, estados e paginação.

### Testes da História de Usuário 1

- [ ] T014 [P] [US1] Criar testes de carregamento, estado vazio e erro usando fake timers; medir desde o início de `OrdensService.list()` até o estado final renderizado e limitar a 2 segundos em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.spec.ts`
- [ ] T015 [P] [US1] Criar teste de paginação e opções de linhas em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.spec.ts`

### Implementação da História de Usuário 1

- [X] T016 [US1] Adaptar `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.html` para usar `DataTable` com colunas de ID, status, abertura, cliente, mecânico, descrição e ações
- [X] T017 [US1] Configurar colunas tipadas, filtro global, paginação 10/5/10/25/50 e estados no componente `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`
- [X] T018 [US1] Atualizar `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.scss` para manter cabeçalho, paginação e tabela legíveis em viewport menor
- [X] T019 [US1] Remover diálogo de detalhe e paginação manual baseada em `slice`, `first` e `rows` do componente de lista

## Fase 4: História de Usuário 2 - Acessar Ações de uma Ordem (Prioridade: P1)

**Objetivo**: Navegar da linha selecionada para o detalhe correto.

**Teste independente**: Acionar ações de duas linhas e confirmar `/ordens/1` e `/ordens/2`.

- [ ] T020 [P] [US2] Criar teste da ação de visualização e navegação por ID em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.spec.ts`
- [X] T021 [P] [US2] Criar teste que rejeita ID ausente ou inválido em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.spec.ts`
- [X] T022 [US2] Configurar a ação de visualização do `DataTable` com ícone, tooltip e callback por linha em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`
- [X] T023 [US2] Implementar `Router.navigate(['/ordens', id])` com validação de ID em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`
- [X] T024 [US2] Implementar carregamento pelo `ActivatedRoute` e `OrdensService.get` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.ts`
- [X] T025 [US2] Preservar a ação de exclusão condicionada à permissão existente em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`

## Fase 5: História de Usuário 3 - Iniciar Nova Ordem (Prioridade: P1)

**Objetivo**: Permitir iniciar uma OS somente com a permissão `ordens × criar`.

**Teste independente**: Testar a ação com sessões que possuem e não possuem a permissão de criação.

- [ ] T026 [P] [US3] Criar teste do botão `Nova OS`, navegação e visibilidade conforme `ordens × criar` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.spec.ts`
- [ ] T027 [P] [US3] Criar teste de ordem das rotas, garantindo `ordens/novo` antes de `ordens/:id`, em `oficina-motos-web/src/app/app.routes.spec.ts`
- [X] T028 [P] [US3] Criar teste do guard para `ordens × criar`, Toast e redirecionamento para `/dashboard` em `oficina-motos-web/src/app/core/auth/ordens-permission.guard.spec.ts`
- [X] T029 [US3] Adicionar o botão `Nova OS` com ícone, texto acessível e ação no cabeçalho de `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.html`
- [X] T030 [US3] Implementar a navegação para `/ordens/novo` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`
- [X] T031 [US3] Ocultar ou desabilitar `Nova OS` quando faltar `ordens × criar` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-lista/os-lista.ts`

## Fase 6: Polimento, E2E e Validação

**Objetivo**: Validar consistência funcional, documental e constitucional.

- [ ] T032 Atualizar referências residuais a `OsDetalhe` e `os-detalhe` em `oficina-motos-web/src/app/`, preservando o componente legítimo de detalhe individual
- [ ] T033 [P] Atualizar `oficina-motos-docs/markdown/PASSOS_IMPLEMENTACAO.md` para registrar `/ordens/novo` como rota vigente e a autorização por permissões
- [ ] T034 [P] Adicionar teste E2E do fluxo autorizado/negado e da navegação para `/ordens/novo` em `oficina-motos-web/e2e/ordens-lista.spec.ts`; se a infraestrutura E2E não existir, documentar a limitação em `oficina-motos-web/docs/TESTES_E2E.md`
- [X] T035 Executar `npm run build` em `oficina-motos-web` e corrigir erros de compilação
- [ ] T036 Executar `npm test -- --watch=false` em `oficina-motos-web` e confirmar os critérios das três histórias
- [ ] T037 Executar os cenários de `specs/008-os-lista-rota/quickstart.md`, incluindo o diagnóstico completo de `npm start`, e registrar limitações de ambiente

## Dependências e Paralelismo

- A Fase 1 não depende de outra fase; T002 depende de T001; T004 depende de T003.
- A Fase 2 depende da Fase 1; T007 deve falhar antes de T008; T010 depende de T009; T013 depende de T009, T011 e T012.
- As histórias dependem da Fase 2; US1 entrega o MVP, US2 depende da tabela de US1 e US3 depende do cabeçalho e das rotas.
- T014/T015, T020/T021 e T026/T027/T028 podem ser paralelas quando suas dependências estiverem concluídas.
- A Fase 6 depende das três histórias.

## Estratégia de Entrega

1. Concluir preparação, sessão de permissões, helper, guard e rotas.
2. Entregar US1 como MVP e validar lista, estados e paginação.
3. Entregar US2 com navegação por ID.
4. Entregar US3 com `Nova OS` condicionado a `ordens × criar`.
5. Executar testes unitários, E2E quando disponível, build e quickstart.

## Observações

- A API, o banco e as migrations ficam fora do escopo.
- `visualizar` e `criar` são os nomes canônicos das permissões; a autorização final não deve depender apenas do papel.
- Acesso autenticado sem permissão exibe o Toast `Você não tem permissão para acessar esta área.` e redireciona para `/dashboard`.
- O limite de 2 segundos mede o intervalo entre o início de `OrdensService.list()` e o estado final renderizado.
- `loadComponent` é obrigatório nas rotas da feature.
