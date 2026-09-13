# Modelo de Dados: Registrar Pagamento de Ordem de Serviço

## Entidades e DTOs

### 1. CreateOrdemServicoPagamentoDTO / CreateOrdemServicoPagamentoRequest

Payload enviado para `POST /api/v1/OrdemServicoPagamentos`:

| Propriedade | Tipo | Obrigatório | Descrição |
| --- | --- | :---: | --- |
| `ordemServicoId` | `number` / `long` | Sim | Identificador numérico da Ordem de Serviço |
| `valor` | `number` / `decimal` | Sim | Valor pago registrado ($\gt 0$) |
| `metodo` | `string?` | Sim | Forma de pagamento selecionada (ex.: Dinheiro, PIX, Cartão) |
| `dataPagamento` | `string?` / `DateTime?` | Sim | Data e hora em que o pagamento foi liquidado |
| `status` | `string` | Sim | Status do pagamento (padrão: `"Liquidado"` ou `"Pago"`) |
| `observacao` | `string?` | Não | Observações e notas adicionais (máx. 240 caracteres) |

---

### 2. OrdemServicoPagamentoResponseDTO / OrdemServicoPagamento

Representação do pagamento retornado pela API e anexado à coleção da OS:

| Propriedade | Tipo | Descrição |
| --- | --- | --- |
| `id` | `number` / `long` | Identificador único do registro de pagamento |
| `ordemServicoId` | `number` / `long` | ID da ordem de serviço vinculada |
| `valor` | `number` / `decimal` | Valor monetário pago |
| `status` | `string` | Situação do pagamento (`Liquidado` / `Pago`) |
| `dataPagamento` | `string?` / `DateTime?` | Data em que o pagamento foi registrado |
| `metodo` | `string?` | Forma de liquidação utilizada |
| `observacao` | `string?` | Notas informativas associadas |

---

### 3. FinanceiroContaReceber (Lançamento Automático no Financeiro)

Entidade gerada no banco de dados na tabela `fin_contas_receber`:

| Campo | Tipo | Mapeamento da OS |
| --- | --- | --- |
| `ClienteId` | `bigint?` | `ordem.ClienteId` |
| `Descricao` | `varchar(240)` | `$"Pagamento OS #{ordem.Id} - {request.Metodo ?? "Geral"}"` |
| `Valor` | `decimal(18,2)` | `request.Valor` |
| `Vencimento` | `datetime` | `request.DataPagamento ?? DateTime.UtcNow` |
| `Status` | `varchar(50)` | `"Recebido"` |
| `DataRecebimento` | `datetime?` | `request.DataPagamento ?? DateTime.UtcNow` |
| `Observacao` | `varchar(240)` | `$"Referente à OS #{ordem.Id}. {request.Observacao}"` |
| `Criado_Em` | `datetime` | `DateTime.UtcNow` |

---

## Estrutura de Estado do Frontend

### 1. `OsPagamentoModalComponent`

```typescript
// Inputs
visible = model<boolean>(false);
ordemServicoId = input.required<number>();
saldoPendente = input<number>(0);

// Outputs
pagamentoRegistrado = output<OrdemServicoPagamento>();

// Signals de Formulário
valor = signal<number>(0);
metodo = signal<string>('Dinheiro');
dataPagamento = signal<string>(new Date().toISOString().substring(0, 10));
observacao = signal<string>('');

// Signals de Controle e Validação
submitting = signal<boolean>(false);
isIntegral = computed(() => this.valor() >= this.saldoPendente());
valorValido = computed(() => this.valor() > 0 && this.valor() <= (this.saldoPendente() + 0.001));
formularioValido = computed(() => this.valorValido() && !!this.metodo() && !!this.dataPagamento());
```

---

### 2. Formas de Pagamento Homologadas

```typescript
export const FORMAS_PAGAMENTO = [
  'Dinheiro',
  'PIX',
  'Cartão de Crédito',
  'Cartão de Débito',
  'Transferência Bancária',
  'Boleto',
] as const;
```

---

### 3. Estado Reativo no `OsDetalheComponent`

```typescript
// Signals Existentes no Detalhe da OS
readonly ordem = signal<OrdemServico | null>(null);
readonly totalItens = computed(() => ...);
readonly totalPago = computed(() => {
  const pagamentos = this.ordem()?.pagamentos;
  if (!pagamentos || pagamentos.length === 0) return 0;
  return pagamentos.reduce((acc, p) => acc + (Number(p.valor) || 0), 0);
});
readonly saldoPendente = computed(() => {
  return Math.max(0, this.totalItens() - this.totalPago());
});
readonly isTerminal = computed(() => {
  const status = this.ordem()?.status;
  return status === 'Concluida' || status === 'Cancelada';
});

// Novo Signal de Controle do Modal de Pagamento
readonly modalPagamentoAberto = signal<boolean>(false);
```

