# Tarefas: Adicionar/Remover Peças e Serviços na OS

**Entrada**: Documentos de design em `/specs/011-os-itens/`
**Pré-requisitos**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/ui.md`, `quickstart.md`
**Organização**: Estruturadas por fases e histórias de usuário, com foco em entrega incremental e reatividade sem reload.

## Fase 1: Serviços e Modelos Base (T-011.3, T-011.4)

**Objetivo**: Disponibilizar métodos tipados em `OrdensService` para inclusão e exclusão de itens da OS.

- [X] T001 [P] Atualizar `OrdensService` em `oficina-motos-web/src/app/core/services/ordens.service.ts` adicionando os métodos `addItem<T, B>(body: B)` e `deleteItem(itemId)`
- [X] T002 [P] Validar interfaces `OrdemServicoItem` e `CreateOrdemServicoItemRequest` em `oficina-motos-web/src/app/core/models/ordem-servico.ts`

## Fase 2: Componente Modal de Peça do Estoque (T-011.1, T-011.3, HU1)

**Objetivo**: Criar o componente `OsItemPecaModalComponent` com autocomplete de peças do estoque, seleção de quantidade, valor unitário e submissão à API.
**Critério de teste independente**: Abrir o modal, pesquisar uma peça cadastrada no estoque, selecionar, conferir preenchimento dos campos e submeter para `POST /api/v1/OrdemServicoItens`.

### Testes do Modal de Peça
- [X] T003 [P] [US1] Criar testes unitários para `OsItemPecaModalComponent` em `oficina-motos-web/src/app/features/ordens-servico/components/os-item-peca-modal/os-item-peca-modal.spec.ts` cobrindo busca no estoque, seleção, validações e envio

### Implementação do Modal de Peça
- [X] T004 [US1] Implementar classe standalone `OsItemPecaModalComponent` em `oficina-motos-web/src/app/features/ordens-servico/components/os-item-peca-modal/os-item-peca-modal.ts` com Signals para busca, peça selecionada, quantidade, valor unitário e cálculo de subtotal
- [X] T005 [US1] Construir template `os-item-peca-modal.html` com `p-dialog`, campo de busca com lista de sugestões, inputs de quantidade e valor, badge de subtotal e botões de ação
- [X] T006 [US1] Estilizar `os-item-peca-modal.scss` com layout responsivo e compatibilidade com o tema dark do sistema

## Fase 3: Componente Modal de Serviço / Mão de Obra (T-011.2, T-011.3, HU2)

**Objetivo**: Criar o componente `OsItemServicoModalComponent` para inclusão de mão de obra e serviços avulsos.
**Critério de teste independente**: Abrir o modal, digitar a descrição do serviço e valor da mão de obra, submeter e confirmar o envio com `pecaId: null`.

### Testes do Modal de Serviço
- [X] T007 [P] [US2] Criar testes unitários para `OsItemServicoModalComponent` em `oficina-motos-web/src/app/features/ordens-servico/components/os-item-servico-modal/os-item-servico-modal.spec.ts` validando descrição obrigatória (máx 240), valor e submissão

### Implementação do Modal de Serviço
- [X] T008 [US2] Implementar classe standalone `OsItemServicoModalComponent` em `oficina-motos-web/src/app/features/ordens-servico/components/os-item-servico-modal/os-item-servico-modal.ts` com Signals, validações e envio via `OrdensService.addItem()`
- [X] T009 [US2] Construir template `os-item-servico-modal.html` com `p-dialog`, textarea com contador de caracteres, campo de valor monetário e subtotal
- [X] T010 [US2] Estilizar `os-item-servico-modal.scss`

## Fase 4: Integração dos Modais e Adição de Itens no Detalhe da OS (T-011.1, T-011.2, T-011.5, HU1, HU2, HU4)

**Objetivo**: Conectar os modais de peça e serviço à página de detalhe da OS e garantir a adição de itens com atualização reativa instantânea.
**Critério de teste independente**: Clicar em "Adicionar Peça" e "Adicionar Serviço" no detalhe da OS e verificar inclusão imediata na tabela e recálculo do total sem reload da página.

- [X] T011 [US1] [US2] Atualizar `OsDetalheComponent` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.ts` importando os dois modais e gerenciando signals de visibilidade (`modalPecaAberto`, `modalServicoAberto`)
- [X] T012 [US1] [US2] Adicionar botões "Adicionar Peça" e "Adicionar Serviço" no cabeçalho da seção "Itens da OS" em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.html`
- [X] T013 [US4] Implementar método `onItemAdicionado(novoItem)` em `os-detalhe.ts` atualizando o signal `ordem` com `update()` e emitindo Toast de sucesso

## Fase 5: Exclusão de Itens com Confirmação e Recálculo Reativo (T-011.4, T-011.5, HU3, HU4)

**Objetivo**: Permitir a exclusão segura de itens com diálogo de confirmação e recálculo imediato dos totais da OS.
**Critério de teste independente**: Clicar na lixeira de uma linha de item, cancelar e verificar permanência; confirmar e verificar exclusão na API, remoção da linha e decréscimo imediato do total.

- [X] T014 [US3] Adicionar coluna de Ações na tabela de itens em `os-detalhe.html` com botão de exclusão (`pi pi-trash`)
- [X] T015 [US3] Implementar método `confirmarExclusaoItem(item)` em `os-detalhe.ts` utilizando `Confirmation.confirmDelete()` e disparando `OrdensService.deleteItem(item.id)`
- [X] T016 [US4] Atualizar a lista de itens no signal `ordem` após exclusão com sucesso, garantindo recálculo automático de `totalItens` e `saldoPendente` sem reload

## Fase 6: Proteção de Imutabilidade em Ordens Finalizadas (HU5)

**Objetivo**: Bloquear mutações em ordens de serviço já concluídas ou canceladas.
**Critério de teste independente**: Acessar OS com status `Concluida` ou `Cancelada` e certificar que os botões de adicionar peças/serviços e de excluir itens estão inativos ou ausentes.

- [X] T017 [US5] Aplicar condições no template `os-detalhe.html` para ocultar ou desabilitar botões de adicionar itens e coluna de exclusão quando `isTerminal() === true`

## Fase 7: Testes Unitários de Integração no Detalhe da OS

**Objetivo**: Garantir cobertura de testes abrangente para os novos fluxos de itens na página de detalhe.

- [X] T018 Criar testes unitários em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.spec.ts` cobrindo abertura de modais, recebimento de `itemAdded`, exclusão com confirmação e recálculo reativo dos computed signals

## Fase 8: Validação Final, Build e Atualização de Backlog

**Objetivo**: Assegurar integridade de compilação, aprovação de testes e conformidade com a governança.

- [X] T019 Executar testes unitários do módulo com o Angular test builder (`npx ng test --no-watch`)
- [X] T020 Executar compilação de produção (`npm run build`) em `oficina-motos-web`
- [X] T021 Atualizar `governance/backlog.md` marcando como concluídas as tasks T-011.1 a T-011.5
