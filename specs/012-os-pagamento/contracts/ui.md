# Contratos de Interface: Registrar Pagamento de Ordem de Serviço

## 1. Componente Modal de Pagamento

### 1.1 `OsPagamentoModalComponent`
- **Seletor**: `app-os-pagamento-modal`
- **Localização**: `features/ordens-servico/components/os-pagamento-modal/`
- **Standalone**: `true`
- **Inputs**:
  - `visible: boolean` (two-way binding `[(visible)]`)
  - `ordemServicoId: number`
  - `saldoPendente: number` (valor sugerido para quitação integral)
- **Outputs**:
  - `visibleChange: EventEmitter<boolean>`
  - `pagamentoRegistrado: EventEmitter<{ pagamento: OrdemServicoPagamento; concluida: boolean }>`
- **Elementos do Formulário**:
  - **Campo Valor Pago**: input numérico monetário obrigatório (inicializado com `saldoPendente`, $\le$ `saldoPendente`);
  - **Campo Forma de Pagamento**: dropdown/select com opções (`Dinheiro`, `PIX`, `Cartão de Crédito`, `Cartão de Débito`, `Transferência Bancária`, `Boleto`);
  - **Campo Data do Pagamento**: input de data obrigatório (inicializado com data atual);
  - **Campo Observações**: textarea opcional com contador de caracteres (máx. 240 caracteres);
  - **Badge/Indicador de Quitação**: informativo visual dinâmico ("Pagamento Integral — Concluirá a OS" vs "Pagamento Parcial — Saldo Restante: R$ X,XX");
  - **Botão Cancelar**: fecha o modal sem persistir dados;
  - **Botão Confirmar Pagamento**: submete o formulário via serviço, desabilitado se inválido ou durante trânsito da requisição (`loading`).

---

## 2. Alterações no `OsDetalheComponent`

### 2.1 Seção "Pagamentos"
- Adição do botão **"Registrar Pagamento"** no cabeçalho da seção (`card-header-row`):
  - Ícone `pi pi-dollar` ou `pi pi-wallet`, classe `p-button-success` ou padrão do tema;
  - Habilitado quando: `!isTerminal() && saldoPendente() > 0 && totalItens() > 0`;
  - Desabilitado ou substituído por badge de quitação ("OS Quitada") quando `saldoPendente() === 0`.

### 2.2 Tabela e Resumo Financeiro
- A tabela de pagamentos existente recebe o novo item de pagamento registrado sem recarregamento da tela;
- Métricas financeiras recalculadas reativamente:
  - `totalPago`: soma imediata de todos os pagamentos;
  - `saldoPendente`: recalculado como $\max(0, \text{totalItens} - \text{totalPago})$;
- Status da OS:
  - Se a resposta indicar conclusão (`concluida: true` ou status retornado `Concluida`), atualiza o signal `ordem` com `status: 'Concluida'` e a data de conclusão correspondente.

