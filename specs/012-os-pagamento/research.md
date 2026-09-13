# Pesquisa Técnica: Registrar Pagamento de Ordem de Serviço

## Decisão 1: Arquitetura de Registro de Pagamento no Backend

**Decisão**: Implementar a lógica de orquestração do pagamento dentro de `OrdemServicoService.RegistrarPagamentoAsync`, sendo chamado a partir de `OrdemServicoPagamentoService.CreateAsync` acionado pelo controller `OrdemServicoPagamentosController` (`POST /api/v1/OrdemServicoPagamentos`).

**Racional**:
- O controller `OrdemServicoPagamentosController` já existe e expõe a rota REST canônica `/api/v1/OrdemServicoPagamentos`, consumindo `CreateOrdemServicoPagamentoDTO`.
- Manter o controller como ponto de entrada HTTP preserva a convenção RESTful do projeto e a rota já cadastrada em `apiPaths.ordens.pagamentos`.
- A regra de negócio que avalia os totais da OS, atualiza seu status para `Concluida` e dispara o lançamento em `ContasReceber` pertence à camada de aplicação de Ordem de Serviço (`OrdemServicoService`), atendendo ao Princípio I (DDD) e à tarefa do backlog `T-012.3`.

**Alternativas consideradas**:
- Lógica no Controller: rejeitada veementemente pela Constituição (Princípio I: nenhuma regra de negócio em controllers).
- Lógica apenas no Frontend (frontend chamando criar pagamento, depois alterar status, depois criar conta a receber): rejeitada por violar a integridade transacional e permitir que falhas de rede deixem o sistema em estado inconsistente.

---

## Decisão 2: Atualização Automática de Status da OS (Integral vs Parcial)

**Decisão**:
- O serviço backend recupera a OS pelo ID incluindo suas coleções de `Itens` e `Pagamentos`.
- Calcula:
  - Total dos itens: $\sum (\text{Total} > 0 \; ? \; \text{Total} : \text{Quantidade} \times \text{ValorUnitario})$
  - Total pago acumulado: $\sum \text{Valor} \text{ (dos pagamentos anteriores)} + \text{request.Valor}$
- Se $\text{Total Pago Acumulado} \ge \text{Total dos Itens}$:
  - `ordem.Status = OrdemServicoStatus.Concluida;`
  - `ordem.DataConclusao = DateTime.UtcNow;`
  - Atualiza a OS no repositório.
- Se $\text{Total Pago Acumulado} < \text{Total dos Itens}$:
  - A OS mantém seu status atual (ex.: `EmAndamento`).

**Racional**:
- Automatiza a transição operacional mais crítica da oficina (entrega do veículo após quitação).
- Suporta múltiplos pagamentos parciais sem conclusão prematura da ordem.

---

## Decisão 3: Integração com Contas a Receber (`FinanceiroContaReceber`)

**Decisão**:
Ao persistir o pagamento, injetar `IFinanceiroContaReceberRepository` (ou `IFinanceiroContaReceberService`) e criar uma nova instância de `FinanceiroContaReceber`:
- `ClienteId`: `ordem.ClienteId`;
- `Descricao`: `$"Pagamento OS #{ordem.Id} - {request.Metodo ?? "Geral"}"`;
- `Valor`: `request.Valor`;
- `Vencimento`: `request.DataPagamento ?? DateTime.UtcNow`;
- `DataRecebimento`: `request.DataPagamento ?? DateTime.UtcNow`;
- `Status`: `"Recebido"`;
- `Observacao`: `string.IsNullOrWhiteSpace(request.Observacao) ? $"Quitação OS #{ordem.Id}" : $"{request.Observacao} (OS #{ordem.Id})"`.

Toda a operação (criação do `OrdemServicoPagamento`, atualização do status da `OrdemServico` e criação do `FinanceiroContaReceber`) deve ser envolvida em uma transação do Entity Framework (`BeginTransactionAsync`), garantindo atomicidade estrita.

**Racional**:
- Elimina divergências entre o livro de OS e a contabilidade do módulo financeiro.
- Garante rastreabilidade total (audit trail e auditoria da Constituição).

---

## Decisão 4: Componente Frontend `OsPagamentoModalComponent`

**Decisão**:
Criar componente standalone `OsPagamentoModalComponent` em `src/app/features/ordens-servico/components/os-pagamento-modal/`.
- Reutiliza `DialogModule` do PrimeNG 21 com estilização dark nativa do sistema.
- Inputs:
  - `visible`: two-way binding `model<boolean>(false)`;
  - `ordemServicoId`: `input.required<number>()`;
  - `saldoPendente`: `input<number>(0)` (pré-preenche o valor do pagamento).
- Formas de pagamento suportadas:
  - `Dinheiro`, `PIX`, `Cartão de Crédito`, `Cartão de Débito`, `Transferência Bancária`, `Boleto`.
- Ao submeter com sucesso:
  - Emite evento `pagamentoRegistrado` com os dados do pagamento e indicador de conclusão;
  - Fecha o modal e notifica com Toast de sucesso.

**Racional**:
- Alinha-se ao padrão dos modais já desenvolvidos em `011-os-itens` (`OsItemPecaModalComponent` e `OsItemServicoModalComponent`).
- O operador não precisa digitar o valor na quitação integral, pois o saldo pendente já vem pré-preenchido.

---

## Decisão 5: Reatividade no `OsDetalheComponent` sem Reload Completo

**Decisão**:
- O `OsDetalheComponent` adiciona método `onPagamentoRegistrado(novoPagamento)`:
  - Atualiza o signal `ordem` anexando o novo pagamento à lista `pagamentos`;
  - Se o somatório atingir o total dos itens, atualiza o campo `status` do signal para `'Concluida'` e preenche `dataConclusao`;
  - Como `totalPago` e `saldoPendente` são `computed()`, as métricas do card de resumo financeiro atualizam instantaneamente;
  - Ao transicionar para `Concluida`, o getter `isTerminal()` passa a retornar `true`, bloqueando imediatamente botões de adição de itens e alteração manual de status.

**Racional**:
- Respeita o Princípio IV da Constituição (Frontend Reativo com Signals).
- Zero reloads de página inteira, oferecendo experiência rápida e consistente ao atendente.

