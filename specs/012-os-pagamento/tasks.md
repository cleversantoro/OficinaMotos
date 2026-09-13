# Tarefas: Registrar Pagamento de Ordem de Serviço

**Entrada**: Documentos de design em `/specs/012-os-pagamento/`
**Pré-requisitos**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/ui.md`, `quickstart.md`
**Organização**: Estruturadas por fases e histórias de usuário, com foco em entrega incremental, consistência transacional e reatividade sem reload.

---

## Fase 1: Backend — Orquestração de Pagamento e Lançamento Financeiro (T-012.3, T-012.4, US1, US2, US3)

**Objetivo**: Implementar o método de negócio no backend para registrar pagamentos, avaliar quitação integral com atualização automática de status da OS e gerar o lançamento em `ContasReceber` sob transação atômica.

- [x] T001 [US1] [US3] Atualizar interface `IOrdemServicoService` em `oficina-motos-api/src/OficinaMotos.Application/Interfaces/OrdemServico/IOrdemServicoService.cs` declarando `Task<OrdemServicoPagamentoResponseDTO> RegistrarPagamentoAsync(CreateOrdemServicoPagamentoDTO request)`
- [x] T002 [US1] [US2] [US3] Implementar método `RegistrarPagamentoAsync` em `oficina-motos-api/src/OficinaMotos.Application/Services/OrdemServico/OrdemServicoService.cs`:
  - Validar existência da OS e bloquear pagamentos caso `Status == OrdemServicoStatus.Cancelada`;
  - Executar o fluxo em transação atômica (`BeginTransactionAsync`);
  - Persistir o pagamento na coleção da OS (`OrdemServicoPagamento`);
  - Somar os pagamentos acumulados e comparar com a soma dos itens da OS;
  - Se total pago $\ge$ total de itens, atualizar `Status = OrdemServicoStatus.Concluida` e `DataConclusao = DateTime.UtcNow`;
  - Criar e persistir automaticamente registro em `FinanceiroContaReceber` vinculado ao cliente e referenciando a OS;
  - Retornar o `OrdemServicoPagamentoResponseDTO` correspondente.
- [x] T003 [US1] Atualizar `OrdemServicoPagamentoService.CreateAsync` em `oficina-motos-api/src/OficinaMotos.Application/Services/OrdemServico/OrdemServicoPagamentoService.cs` para delegar a criação para `_ordemServicoService.RegistrarPagamentoAsync(request)`
- [x] T004 [P] [US1] [US2] [US3] Criar testes unitários para `OrdemServicoService.RegistrarPagamentoAsync` em `oficina-motos-api/tests/OficinaMotos.Application.Tests/Services/OrdemServicoServicePagamentoTests.cs` (ou diretório de testes de aplicação) cobrindo pagamento integral, pagamento parcial, lançamento financeiro e bloqueio de OS cancelada

---

## Fase 2: Frontend — Serviços e Modelos Base (T-012.2, US1)

**Objetivo**: Disponibilizar método tipado em `OrdensService` e validar interfaces para envio de pagamentos à API.

- [x] T005 [P] [US1] Validar as interfaces `OrdemServicoPagamento` e `CreateOrdemServicoPagamentoRequest` em `oficina-motos-web/src/app/core/models/ordem-servico.ts`
- [x] T006 [P] [US1] Atualizar `OrdensService` em `oficina-motos-web/src/app/core/services/ordens.service.ts` adicionando o método `addPagamento<T = OrdemServicoPagamento, B = CreateOrdemServicoPagamentoRequest>(body: B)` consumindo a rota canônica `apiPaths.ordens.pagamentos`

---

## Fase 3: Componente Modal de Pagamento da OS (T-012.1, US1, US2, US4)

**Objetivo**: Criar o componente standalone `OsPagamentoModalComponent` para entrada de dados de pagamento com sugestão automática de saldo pendente e validações reativas.
**Critério de teste independente**: Abrir o modal, conferir saldo pendente preenchido, selecionar forma de pagamento, preencher data/observação e submeter para `OrdensService.addPagamento()`.

### Testes do Modal de Pagamento
- [x] T007 [P] [US1] [US4] Criar testes unitários para `OsPagamentoModalComponent` em `oficina-motos-web/src/app/features/ordens-servico/components/os-pagamento-modal/os-pagamento-modal.spec.ts` validando pré-preenchimento do valor, validação de campos obrigatórios, bloqueio de cliques múltiplos e emissão de eventos

### Implementação do Modal de Pagamento
- [x] T008 [US1] [US2] [US4] Implementar classe standalone `OsPagamentoModalComponent` em `oficina-motos-web/src/app/features/ordens-servico/components/os-pagamento-modal/os-pagamento-modal.ts` com Signals para valor, forma de pagamento, data, observação, cálculo de quitação e envio via `OrdensService.addPagamento()`
- [x] T009 [US1] [US4] Construir template `os-pagamento-modal.html` com `p-dialog`, campo de valor monetário com máscara/validação, dropdown de formas de pagamento homologadas, input de data, textarea de observações e badge dinâmico de quitação ("Pagamento Integral" vs "Pagamento Parcial")
- [x] T010 [US1] Estilizar `os-pagamento-modal.scss` com layout responsivo e compatibilidade estrita com o tema dark da aplicação

---

## Fase 4: Integração do Modal no Detalhe da OS (T-012.1, T-012.2, US1, US2, US5)

**Objetivo**: Conectar o modal de pagamento à página de detalhe da OS (`OsDetalheComponent`), adicionando botão acionador e atualizando reativamente pagamentos, totais e status da OS sem recarregamento de página.
**Critério de teste independente**: Clicar em "Registrar Pagamento" no detalhe da OS, preencher o formulário, confirmar e verificar inserção imediata na tabela de pagamentos, atualização de `totalPago` / `saldoPendente` e transição do status para `Concluida`.

- [x] T011 [US1] [US2] [US5] Atualizar `OsDetalheComponent` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.ts`:
  - Importar `OsPagamentoModalComponent`;
  - Adicionar signal `modalPagamentoAberto = signal(false)`;
  - Implementar método `abrirModalPagamento()` validando elegibilidade da OS;
  - Implementar método `onPagamentoRegistrado(novoPagamento)` que atualiza reativamente o signal `ordem` anexando o pagamento, recalculando totais e marcando status como `Concluida` caso o total tenha sido quitado.
- [x] T012 [US1] [US5] Adicionar botão "Registrar Pagamento" no cabeçalho da seção "Pagamentos" em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.html`, habilitado quando `!isTerminal() && saldoPendente() > 0 && totalItens() > 0`
- [x] T013 [US5] Adicionar indicador/badge de "OS Quitada" em `os-detalhe.html` quando `saldoPendente() === 0` e `totalItens() > 0`
- [x] T014 [US1] Incluir a tag `<app-os-pagamento-modal>` no rodapé de `os-detalhe.html` conectando signals de visibilidade e evento de pagamento registrado

---

## Fase 5: Testes Unitários de Integração no Detalhe da OS (US1, US2, US5)

**Objetivo**: Garantir cobertura de testes abrangente para os fluxos de pagamento e reflexo reativo no detalhe da OS.

- [x] T015 [US1] [US2] [US5] Atualizar testes unitários de `OsDetalheComponent` em `oficina-motos-web/src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.spec.ts` cobrindo abertura do modal de pagamento, recebimento de pagamento registrado, atualização de totais, transição de status para `Concluida` e bloqueio de ações quando quitada ou cancelada

---

## Fase 6: Validação Final, Build e Atualização de Governança

**Objetivo**: Assegurar integridade de compilação em todas as camadas, aprovação dos testes automatizados e atualização do backlog do projeto.

- [x] T016 [P] Executar compilação do backend `dotnet build` em `oficina-motos-api`
- [x] T017 [P] Executar testes unitários do módulo de ordens no frontend com Vitest (`npx ng test --no-watch --include src/app/features/ordens-servico/**/*.spec.ts`)
- [x] T018 [P] Executar compilação de produção (`npm run build`) em `oficina-motos-web`
- [x] T019 Atualizar `governance/backlog.md` marcando como concluídas as tarefas T-012.1 a T-012.4 da US-012

---

## Dependências e Ordem de Execução

```text
Fase 1 (Backend: Service + ContaReceber)  ──┐
                                            ├──►  Fase 4 (Integração no Detalhe da OS) ──► Fase 5 (Testes de Integração) ──► Fase 6 (Build & Governança)
Fase 2 (Frontend: OrdensService)          ──┤
                                            │
Fase 3 (Frontend: OsPagamentoModal)       ──┘
```

- **Tarefas Paralelizáveis**:
  - `T001` e `T004` no backend podem ser desenvolvidas em paralelo com `T005` e `T006` no frontend;
  - `T007` (testes do modal) e `T008`/`T009` (implementação do modal) podem rodar independentemente das alterações de backend enquanto os contratos de DTO forem respeitados;
  - `T016`, `T017` e `T018` de validação final executam de forma paralela.

---

## Estratégia de Implementação (MVP Incremental)

1. **Incremento 1 (Backend)**: Garantir que a chamada de API registra o pagamento, atualiza o status para `Concluida` quando quitado e gera a conta a receber de forma transacional.
2. **Incremento 2 (Modal de Pagamento)**: Construir o componente `OsPagamentoModalComponent` standalone e testá-lo isoladamente.
3. **Incremento 3 (Integração no Detalhe)**: Adicionar o botão no card de pagamentos de `OsDetalheComponent`, plugar o modal e certificar a reatividade instantânea dos signals sem reload.
4. **Incremento 4 (Validação Completa)**: Rodar os testes unitários em ambas as aplicações, validar builds e fechar os itens no backlog de governança.

