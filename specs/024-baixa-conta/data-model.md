# Modelo de Dados: Registrar Baixa de Conta (US-024)

**Branch**: `024-baixa-conta` | **Data**: 2026-11-16

## 1. Contratos Backend (DTOs em `OficinaMotos.Application`)

### 1.1 Requests (`OficinaMotos.Application.DTOs.Requests.Financeiro`)
```csharp
public class CreateContaPagarPagamentoDTO
{
    public long ContaPagarId { get; set; }
    public DateTime DataPagamento { get; set; }
    public decimal ValorPago { get; set; }
    public long MetodoId { get; set; }
    public string? Observacao { get; set; }
}

public class CreateContaReceberPagamentoDTO
{
    public long ContaReceberId { get; set; }
    public DateTime DataPagamento { get; set; }
    public decimal ValorPago { get; set; }
    public long MetodoId { get; set; }
    public string? Observacao { get; set; }
}
```

### 1.2 Responses (`OficinaMotos.Application.DTOs.Responses.Financeiro`)
```csharp
public class ContaPagarPagamentoResponseDTO
{
    public long ContaPagarId { get; set; }
    public long PagamentoId { get; set; }
    public decimal ValorPago { get; set; }
    public decimal SaldoResidual { get; set; }
    public bool BaixaIntegral { get; set; }
    public FinanceiroContaPagarResponseDTO Conta { get; set; } = new();
}

public class ContaReceberPagamentoResponseDTO
{
    public long ContaReceberId { get; set; }
    public long PagamentoId { get; set; }
    public decimal ValorPago { get; set; }
    public decimal SaldoResidual { get; set; }
    public bool BaixaIntegral { get; set; }
    public FinanceiroContaReceberResponseDTO Conta { get; set; } = new();
}
```

## 2. Interfaces Frontend (`src/app/core/models/financeiro.ts`)

```typescript
export type BaixaContaTipo = 'PAGAR' | 'RECEBER';

export interface BaixaContaAlvo {
  id: number;
  tipo: BaixaContaTipo;
  descricao: string;
  valor: number;
  vencimento: string;
  metodoId: number | null;
  parceiroNome?: string;
  observacao?: string | null;
}

export interface CreateContaPagarPagamentoRequest {
  contaPagarId: number;
  dataPagamento: string;
  valorPago: number;
  metodoId: number;
  observacao: string | null;
}

export interface ContaPagarPagamentoResponse {
  contaPagarId: number;
  pagamentoId: number;
  valorPago: number;
  saldoResidual: number;
  baixaIntegral: boolean;
  conta: FinanceiroContaPagar;
}

export interface CreateContaReceberPagamentoRequest {
  contaReceberId: number;
  dataPagamento: string;
  valorPago: number;
  metodoId: number;
  observacao: string | null;
}

export interface ContaReceberPagamentoResponse {
  contaReceberId: number;
  pagamentoId: number;
  valorPago: number;
  saldoResidual: number;
  baixaIntegral: boolean;
  conta: FinanceiroContaReceber;
}
```

## 3. Estrutura e Validações do Formulário (`BaixaPagamentoModalComponent`)

| Campo do Formulário | Tipo no FormControl | Obrigatório | Validações (Frontend + Backend) | Valor Inicial ao Abrir Modal |
| :--- | :--- | :---: | :--- | :--- |
| `dataPagamento` | `string` (`yyyy-MM-dd`) | Sim | `Validators.required` | Data atual (`yyyy-MM-dd`) |
| `valorPago` | `number \| null` | Sim | `Validators.required`, `Validators.min(0.01)`, `<= conta.valor` | `conta.valor` (saldo atual em aberto) |
| `metodoId` | `number \| null` | Sim | `Validators.required`, `> 0` | `conta.metodoId` ou 1º método da lista |
| `observacao` | `string` | Não | `Validators.maxLength(240)` | `''` |

## 4. Transições de Estado das Contas

| Tipo de Conta | Estado Inicial Elegível | Condição da Baixa | Novo Status da Conta | Novo `valor` da Conta | Data de Liquidação (`dataPagamento` / `dataRecebimento`) |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Conta a Pagar** | `PENDENTE` ou `VENCIDO` | `valorPago == conta.valor` (**Integral**) | `'PAGO'` | Mantém o valor quitado na operação | Preenchida com `request.dataPagamento` |
| **Conta a Pagar** | `PENDENTE` ou `VENCIDO` | `0 < valorPago < conta.valor` (**Parcial**) | Permanece `'PENDENTE'` ou `'VENCIDO'` | Atualizado para `saldoResidual` (`conta.valor - valorPago`) | Permanece `null` (conta segue em aberto pelo saldo residual) |
| **Conta a Receber** | `PENDENTE` ou `VENCIDO` | `valorPago == conta.valor` (**Integral**) | `'RECEBIDO'` | Mantém o valor recebido na operação | Preenchida com `request.dataPagamento` |
| **Conta a Receber** | `PENDENTE` ou `VENCIDO` | `0 < valorPago < conta.valor` (**Parcial**) | Permanece `'PENDENTE'` ou `'VENCIDO'` | Atualizado para `saldoResidual` (`conta.valor - valorPago`) | Permanece `null` (conta segue em aberto pelo saldo residual) |

## 5. Banco de Dados (MySQL 8.0)

Nenhuma alteração de schema ou migration é necessária. As tabelas existentes `fin_contas_pagar`, `fin_contas_receber`, `fin_pagamentos` e `fin_metodos_pagamento` suportam integralmente a operação.
