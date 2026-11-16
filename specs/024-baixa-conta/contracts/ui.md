# Contratos de UI: Registrar Baixa de Conta (US-024)

**Branch**: `024-baixa-conta` | **Data**: 2026-11-16

## 1. Componente `BaixaPagamentoModalComponent` (`T-024.1`)
- **Seletor**: `app-baixa-pagamento-modal`
- **Caminho**: `src/app/features/financeiro/components/baixa-pagamento-modal/baixa-pagamento-modal.ts`
- **Paradigma**: Standalone Component (`standalone: true`), `ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule` + Angular Signals (`submitting`, `valorDigitado`, `saldoAtual`, `saldoResidual`, `isParcial`, `isIntegral`, `valorExcedente`).
- **Inputs / Outputs**:
  - `@Input() visible = false`: controla a abertura do modal.
  - `@Input() conta: BaixaContaAlvo | null = null`: dados da conta selecionada para baixa (`id`, `tipo: 'PAGAR' | 'RECEBER'`, `descricao`, `valor`, `vencimento`, `metodoId`, `parceiroNome`, `observacao`).
  - `@Input() metodos: FinanceiroMetodoPagamento[] = []`: lista de métodos de pagamento disponíveis (com fallback resiliente caso vazia).
  - `@Output() visibleChange = new EventEmitter<boolean>()`: notifica fechamento do modal.
  - `@Output() baixaRegistrada = new EventEmitter<ContaPagarPagamentoResponse | ContaReceberPagamentoResponse>()`: emite o resultado da baixa após confirmação da API.
- **Estrutura Visual do Modal (Dark Theme)**:
  1. **Cabeçalho do Modal**:
     - Título contextual: `"Registrar Baixa — Conta a Pagar"` ou `"Registrar Baixa — Conta a Receber"`.
     - Subtítulo com descrição da conta e nome do fornecedor/cliente.
  2. **Resumo Financeiro e Saldo Residual**:
     - **Saldo Atual em Aberto**: `R$ {{ saldoAtual() | number:'1.2-2' }}` + botão rápido `"Usar valor total"`.
     - **Saldo Residual Após Baixa**: `R$ {{ saldoResidual() | number:'1.2-2' }}`.
     - **Badge de Tipo de Baixa**:
       - Quando `isIntegral()`: `"Baixa Integral — Conta será liquidada"` (verde).
       - Quando `isParcial()`: `"Pagamento Parcial — Saldo residual de R$ X,XX permanecerá em aberto"` (âmbar/azul).
  3. **Campos do Formulário (`<form [formGroup]="form" (ngSubmit)="confirmarBaixa()">`)**:
     - **Data do Pagamento (`dataPagamento`)**: `<input type="date" formControlName="dataPagamento">` (obrigatório).
     - **Valor Pago (`valorPago`)**: `<input type="number" step="0.01" min="0.01" formControlName="valorPago">` (obrigatório, `> 0` e `<= saldoAtual`).
     - **Forma de Pagamento (`metodoId`)**: `<select formControlName="metodoId">` (obrigatório, opções vindas de `metodos` ou lista padrão de fallback).
     - **Observação (`observacao`)**: `<textarea formControlName="observacao" rows="2" maxlength="240">` (opcional).
  4. **Rodapé de Ações**:
     - Botão secundário `"Cancelar"` (`type="button"`, fecha o modal).
     - Botão primário `"Confirmar Baixa"` (`type="submit"`, desabilitado quando inválido ou `submitting() === true`, exibindo `"Registrando..."`).

## 2. Integração em `ContasPagarListaComponent` e `ContasReceberListaComponent`
- **Coluna "Ações" em `/financeiro/contas-pagar` e `/financeiro/contas-receber`**:
  - Para contas com `statusEfetivo === 'PENDENTE' || statusEfetivo === 'VENCIDO'`, exibe o botão `<button type="button" class="btn-baixa-conta" (click)="abrirModalBaixa(conta)">Baixar</button>` ao lado do link `"Editar"`.
  - Para contas com `statusEfetivo === 'PAGO' | 'RECEBIDO' | 'CANCELADO'`, exibe apenas o link `"Editar"`.
  - Ao receber `(baixaRegistrada)`, atualiza o item na coleção reativa `contas`, refletindo imediatamente o novo status (`PAGO` / `RECEBIDO`) ou o novo `valor` (`saldoResidual`) e os KPIs no topo da página.
