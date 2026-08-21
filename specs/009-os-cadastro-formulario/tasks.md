# Tarefas: Formulário de Nova Ordem de Serviço

**Entrada**: Documentos de design em `/specs/009-os-cadastro-formulario/`

**Pré-requisitos**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/ui.md` e `quickstart.md`

**Organização**: As tarefas estão agrupadas por história de usuário e seguem a ordem de execução.

## Fase 1: Preparação

**Objetivo**: Alinhar contratos locais e serviços sem alterar banco ou endpoint.

- [X] T001 [P] Atualizar `CreateOrdemServicoRequest` em `oficina-motos-web/src/app/core/models/ordem-servico.ts` para incluir `veiculoId`, `status`, `dataAbertura` e `dataConclusao`
- [X] T002 [P] Tipar `OrdensService.create<T, B>()` em `oficina-motos-web/src/app/core/services/ordens.service.ts` para aceitar o DTO de criação e retornar a ordem criada
- [X] T003 [P] Adicionar `ClientesService.search(term)` em `oficina-motos-web/src/app/core/services/clientes.service.ts`, reutilizando `apiPaths.clientes.base` e parâmetros de consulta existentes
- [X] T004 [P] Adicionar `VeiculosService.listByCliente(clienteId)` em `oficina-motos-web/src/app/core/services/veiculos.service.ts`, reutilizando `apiPaths.veiculos.base` e o filtro de cliente suportado
- [X] T005 [P] Adicionar `MecanicosService.getAll()` em `oficina-motos-web/src/app/core/services/mecanicos.service.ts` como contrato semântico da listagem existente

## Fase 2: Fundamentos Compartilhados

**Objetivo**: Preparar o formulário standalone, autorização e destino da criação.

- [X] T006 Criar testes dos contratos de `ClientesService.search`, `VeiculosService.listByCliente`, `MecanicosService.getAll` e `OrdensService.create` em seus respectivos arquivos `.spec.ts`, verificando os `apiPaths` usados e escrevendo-os antes das implementações T001-T005
- [X] T007 Criar a estrutura standalone `OsCadastroComponent` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`, `os-novo.html` e `os-novo.scss`, com Reactive Forms, Signals e imports PrimeNG necessários
- [X] T008 Atualizar `oficina-motos-web/src/app/app.routes.ts` para carregar `OsCadastroComponent` em `/ordens/novo` com `loadComponent`, antes de `/ordens/:id`, preservando `authGuard` e `ordensPermissionGuard`
- [X] T009 Confirmar em `oficina-motos-web/src/app/core/auth/ordens-permission.guard.ts` que `ordens × criar` continua sendo exigida para `/ordens/novo` e que o Toast/redirecionamento permanecem consistentes

**Marco**: O formulário standalone, seus contratos de serviço e a rota protegida estão prontos.

## Fase 3: História de Usuário 1 - Abrir Formulário Autorizado (Prioridade: P1) MVP

**Objetivo**: Exibir o formulário correto na rota `/ordens/novo` para a sessão autorizada.

**Teste independente**: Acessar a rota com e sem `ordens × criar` e confirmar formulário ou redirecionamento.

### Testes da História de Usuário 1

- [X] T010 [P] [US1] Criar teste do componente para renderização de `OsCadastroComponent`, quatro campos obrigatórios, estado inicial e acesso autorizado em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [ ] T011 [P] [US1] Criar teste de acesso negado, mensagem `Você não tem permissão para acessar esta área.` e redirecionamento para `/dashboard` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`

### Implementação da História de Usuário 1

- [X] T012 [US1] Implementar os controles obrigatórios `clienteId`, `veiculoId`, `descricaoProblema` e `mecanicoId` com validators em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`
- [X] T013 [US1] Criar o template responsivo com autocomplete, dropdowns, textarea, mensagens de validação, botão de envio e estado de carregamento em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.html`
- [X] T014 [US1] Estilizar o formulário para desktop e viewport menor em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.scss`

## Fase 4: História de Usuário 2 - Selecionar Cliente e Veículo (Prioridade: P1)

**Objetivo**: Buscar clientes e limitar veículos ao cliente selecionado.

**Teste independente**: Buscar cliente, selecionar dois clientes diferentes e confirmar que o veículo anterior é limpo e as opções são substituídas.

- [ ] T015 [P] [US2] Criar teste de busca por termo, resultados e estado vazio em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [ ] T016 [P] [US2] Criar teste de carregamento de veículos por cliente, limpeza ao trocar cliente e ausência de opções em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [X] T017 [US2] Implementar autocomplete com `ClientesService.search(term)`, limite mínimo de termo e controle de respostas obsoletas em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`
- [X] T018 [US2] Implementar carregamento dependente de veículos com `VeiculosService.listByCliente(clienteId)`, limpeza de `veiculoId` e estados de loading/erro em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`
- [X] T019 [US2] Conectar a seleção de cliente e veículo ao template com opções pertencentes ao cliente atual em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.html`

## Fase 5: História de Usuário 3 - Preencher e Criar Ordem (Prioridade: P1)

**Objetivo**: Carregar mecânicos, validar o formulário, criar a OS e navegar para o detalhe.

**Teste independente**: Preencher os quatro campos com dados válidos, enviar e confirmar DTO, bloqueio de duplicidade e navegação pelo ID retornado.

- [ ] T020 [P] [US3] Criar teste de carregamento de mecânicos e estado de erro em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [X] T021 [P] [US3] Criar teste de formulário inválido sem chamada de criação em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [ ] T022 [P] [US3] Criar teste de criação válida, DTO com `veiculoId`, navegação para `/ordens/:id` e ID inválido retornado em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [ ] T023 [P] [US3] Criar teste de submissão duplicada e erro da API preservando os dados em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [ ] T024 [US3] Criar teste de perda da permissão `ordens × criar` durante o preenchimento, confirmando Toast, redirecionamento para `/dashboard` e ausência de chamada de criação em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts`
- [X] T025 [US3] Implementar carregamento de mecânicos via `MecanicosService.getAll()` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`
- [X] T026 [US3] Implementar validação de permissão, estado `submitting` e bloqueio de submissão duplicada em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`
- [X] T027 [US3] Montar o DTO padrão e chamar `OrdensService.create()` com cliente, veículo, descrição, mecânico, status `Aberta` e data de abertura nula em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`
- [X] T028 [US3] Navegar para `/ordens/:id` após criação com ID positivo e exibir Toast sem navegar quando houver erro ou ID inválido em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts`
- [X] T029 [US3] Exibir estados de loading, erro e sucesso no template em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.html`

## Fase 6: Polimento e Validação

**Objetivo**: Validar a feature completa e sua documentação.

- [X] T030 [P] Adicionar/atualizar testes E2E em `oficina-motos-web/e2e/os-cadastro.spec.ts`; se não houver infraestrutura E2E, documentar a limitação em `oficina-motos-web/docs/TESTES_E2E.md`
- [X] T031 Executar `npm run build` em `oficina-motos-web` e corrigir erros de TypeScript, template e rotas
- [X] T032 Executar `npm test -- --watch=false` em `oficina-motos-web` e registrar separadamente falhas legadas fora da US-009
- [ ] T033 Executar os cenários de `specs/009-os-cadastro-formulario/quickstart.md` com dados reais ou mocks controlados
- [X] T034 Revisar `oficina-motos-docs/markdown/PASSOS_IMPLEMENTACAO.md` para registrar o formulário de nova OS e o vínculo obrigatório de veículo

## Dependências e Paralelismo

- O checkpoint de contratos começa antes da Fase 1: T006 deve ser escrito primeiro e falhar antes de T001-T005; depois da falha esperada, T001-T005 podem ser implementadas em paralelo por arquivo.
- A Fase 2 depende da Fase 1; T008 depende de T007.
- US1 depende da Fase 2 e entrega o MVP visual do formulário.
- US2 depende de T012/T013 e pode evoluir em paralelo com parte de US3 após a estrutura do formulário.
- US3 depende dos controles de US1 e da seleção cliente/veículo de US2.
- T015/T016, T020/T021/T022/T023/T024 podem ser escritos em paralelo antes das implementações correspondentes.
- A Fase 6 depende das três histórias concluídas.

## Estratégia de Entrega

1. Alinhar DTOs e services e criar a tela standalone protegida.
2. Entregar US1 com os quatro campos obrigatórios.
3. Entregar US2 com autocomplete e veículo dependente.
4. Entregar US3 com mecânicos, criação, bloqueio de duplicidade e navegação.
5. Executar build, testes e quickstart.

## Observações

- A API, o banco e as migrations não serão alterados nesta feature.
- URLs HTTP permanecem centralizadas em `apiPaths`.
- `veiculoId` é obrigatório por causa da dependência da US-007.
- A permissão final é `ordens × criar`; o guard existente da US-008 deve ser reutilizado.

## Fase 7: Convergência

- [X] T035 Controlar respostas obsoletas em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.ts` para que buscas anteriores de cliente não substituam resultados do termo mais recente, conforme FR-004 (partial)
- [X] T036 Criar testes dos contratos HTTP de `ClientesService.search`, `VeiculosService.listByCliente`, `MecanicosService.getAll` e `OrdensService.create`, verificando paths e parâmetros de `apiPaths`, conforme FR-013 (missing)
- [ ] T037 Criar testes do formulário em `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts` cobrindo acesso negado, busca de cliente, troca/limpeza de veículo, erro de consulta, criação válida, ID inválido, submissão duplicada e perda de `ordens × criar`, conforme FR-003 a FR-012 e SC-002 a SC-008 (partial)
- [X] T038 Adicionar teste E2E do fluxo autorizado e negado de `/ordens/novo` em `oficina-motos-web/e2e/os-cadastro.spec.ts` ou documentar a ausência de infraestrutura em `oficina-motos-web/docs/TESTES_E2E.md`, conforme Constituição VI e FR-015 (missing)
- [X] T039 Atualizar `oficina-motos-docs/markdown/PASSOS_IMPLEMENTACAO.md` com o formulário de nova OS e o vínculo obrigatório entre cliente e veículo, conforme Constituição VII (missing)

## Fase 8: Convergência

- [X] T040 Completar os testes de `oficina-motos-web/src/app/features/ordens-servico/pages/os-novo/os-novo.spec.ts` para acesso negado, busca/erro de cliente, carregamento/erro de mecânicos, troca de cliente, criação válida, ID inválido, submissão duplicada e perda de `ordens × criar`, conforme FR-003 a FR-012 e SC-001 a SC-008 (partial)
- [X] T041 Executar `npm test -- --watch=false` em `oficina-motos-web` após registrar ou corrigir os erros legados de `error-interceptor.spec.ts`, conforme Constituição VI (missing)
- [ ] T042 Executar os cenários de `specs/009-os-cadastro-formulario/quickstart.md` com mocks controlados ou dados reais e registrar os resultados, conforme SC-001 a SC-008 (missing)
