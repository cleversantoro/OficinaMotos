# Tarefas: Formulário de Cadastro de Veículo (Motocicleta)

**Entrada**: Documentos de design em `/specs/013-cadastro-veiculo/`
**Pré-requisitos**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/ui.md`, `quickstart.md`
**Organização**: Estruturadas por fases e histórias de usuário, com foco em entrega incremental, validação estrita e reatividade sem reload.

---

## Fase 1: Validador de Placa Veicular (T-013.3, US2)

**Objetivo**: Criar e testar o validador reutilizável de placas nos padrões Mercosul e tradicional brasileiro.

- [x] T001 [P] [US2] Criar validador `placaValidator` e funções utilitárias `isPlacaValid` e `cleanPlaca` em `oficina-motos-web/src/app/shared/validators/placa-validator.ts`
- [x] T002 [P] [US2] Criar testes unitários para `placaValidator` em `oficina-motos-web/src/app/shared/validators/placa-validator.spec.ts` cobrindo padrões Mercosul (`BRA2E19`), tradicionais (`ABC-1234` e `ABC1234`), conversão para maiúsculas e rejeição de formatos inválidos
- [x] T003 [US2] Exportar `placaValidator` e `isPlacaValid` no barrel `oficina-motos-web/src/app/shared/validators/index.ts`

---

## Fase 2: Componente `VeiculoCadastroComponent` (T-013.1, T-013.4, T-013.5, US1, US3, US4, US5)

**Objetivo**: Implementar a lógica, template e estilização do formulário reativo de cadastro de motocicleta.

- [x] T004 [US1] [US3] [US4] [US5] Implementar a classe do componente `VeiculoCadastroComponent` em `oficina-motos-web/src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.ts`:
  - Componente standalone com `ChangeDetectionStrategy.OnPush`;
  - Inicialização do formulário reativo com validações obrigatórias (`clienteId`, `placa` com `placaValidator`, `marcaId`, `modeloId`) e metadados opcionais (`anoFab`, `anoMod`, `cor`, `chassi`, `km`, `combustivel`, `observacao`, `principal`, `ativo`);
  - Signals para estados de carregamento (`loadingMarcas`, `loadingModelos`, `loadingClientes`, `submitting`) e coleções de opções;
  - Autocomplete de busca de clientes via `ClientesService.search()` com controle sequencial de requisições;
  - Cascata marca -> modelo: carregamento de marcas via `VeiculosService.marcas()`, carregamento/filtragem de modelos via `VeiculosService.modelos()` e reset de modelo ao alterar a marca;
  - Submissão via `VeiculosService.create()` com feedback via Toast e navegação para `/motos`.
- [x] T005 [US1] [US3] [US4] [US5] Criar o template `veiculo-cadastro.html` em `oficina-motos-web/src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.html`:
  - Seções em cards: Proprietário (autocomplete com dropdown), Identificação (Placa com auto-uppercase, Chassi, Cor), Especificações (Marca, Modelo, Anos, KM, Combustível) e Observações;
  - Exibição de mensagens de erro inline para campos inválidos ou não preenchidos;
  - Botão Cancelar (volta para `/motos`) e botão Salvar Veículo com estado `loading`.
- [x] T006 [US1] Criar estilos `veiculo-cadastro.scss` em `oficina-motos-web/src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.scss` com layout responsivo e compatibilidade total com o tema dark da aplicação
- [x] T007 [P] [US1] [US3] [US4] [US5] Criar testes unitários do componente `VeiculoCadastroComponent` em `oficina-motos-web/src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.spec.ts`

---

## Fase 3: Registro de Rotas e Navegação (T-013.2, US1)

**Objetivo**: Configurar a rota `/motos/novo` no roteador do Angular com precedência correta e adicionar o botão de acesso na listagem de veículos.

- [x] T008 [US1] Registrar a rota `/motos/novo` em `oficina-motos-web/src/app/app.routes.ts` com proteção do `authGuard`, posicionada estritamente antes da rota `/motos/:id`
- [x] T009 [US1] Adicionar botão `+ Nova Moto` / `Novo Veículo` no cabeçalho de `oficina-motos-web/src/app/features/motos/pages/veiculo-lista/veiculo-lista.html` apontando para `/motos/novo`
- [x] T010 [US1] Ajustar estilos em `oficina-motos-web/src/app/features/motos/pages/veiculo-lista/veiculo-lista.scss` para posicionamento e destaque do botão de ação

---

## Fase 4: Testes e Verificação da Suíte Frontend

**Objetivo**: Assegurar cobertura total e ausência de regressões nos testes automatizados.

- [x] T011 [P] Executar testes unitários do validador `placaValidator` e do componente `VeiculoCadastroComponent` com Vitest
- [x] T012 [P] Executar a suíte completa de testes do frontend com `npx ng test --no-watch`

---

## Fase 5: Validação Final, Build e Atualização de Governança

**Objetivo**: Validar a compilação de produção e atualizar o backlog de governança do projeto.

- [x] T013 [P] Executar `npm run build` em `oficina-motos-web`
- [x] T014 Atualizar `governance/backlog.md` marcando como concluídas as tarefas T-013.1 a T-013.5 da US-013
- [x] T015 Atualizar status de conclusão em `specs/013-cadastro-veiculo/tasks.md`

