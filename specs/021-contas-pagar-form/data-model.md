# Modelo de Dados: Criar e Editar Conta a Pagar (US-021)

**Branch**: `021-contas-pagar-form` | **Data**: 2026-11-16

## 1. Interfaces de Domínio e Payloads REST (`src/app/core/models/financeiro.ts`)

As interfaces já definidas em [`src/app/core/models/financeiro.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/financeiro.ts) atendem integralmente ao contrato do backend (`CreateFinanceiroContaPagarDTO`, `UpdateFinanceiroContaPagarDTO` e `FinanceiroContaPagarResponseDTO`):

```typescript
export interface FinanceiroContaPagar {
  id: number;
  fornecedorId: number | null;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string;
  dataPagamento: string | null;
  metodoId: number | null;
  observacao: string | null;
}

export interface CreateFinanceiroContaPagarRequest {
  fornecedorId: number | null;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string;
  dataPagamento: string | null;
  metodoId: number | null;
  observacao: string | null;
}

export type UpdateFinanceiroContaPagarRequest = CreateFinanceiroContaPagarRequest;
```

## 2. Estrutura e Regras de Validação do Formulário (`ContasPagarFormComponent`)

| Campo do Formulário | Tipo no FormControl | Obrigatório | Validações (Frontend + Backend) | Valor Inicial (Criação) | Transformação no Payload (`POST` / `PUT`) |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `descricao` | `string` | Sim | `Validators.required`, `Validators.maxLength(240)` (após `trim()`) | `''` | `descricao.trim()` |
| `fornecedorId` | `number \| null` | Não | Opcional (`cad_fornecedores.Id`) | `null` | `parseNullableNumber(raw.fornecedorId)` (`number \| null`) |
| `valor` | `number \| null` | Sim | `Validators.required`, `Validators.min(0.01)` | `null` | `Number(raw.valor)` |
| `vencimento` | `string` (`yyyy-MM-dd`) | Sim | `Validators.required` | `''` | `raw.vencimento.trim()` (`yyyy-MM-dd`) |
| `status` | `string` | Sim | `Validators.required`, `Validators.maxLength(50)` (`'PENDENTE' \| 'PAGO' \| 'VENCIDO' \| 'CANCELADO'`) | `'PENDENTE'` | `raw.status.trim().toUpperCase()` |
| `metodoId` | `number \| null` | Não | Opcional (`fin_metodos_pagamento.Id`) | `null` | `parseNullableNumber(raw.metodoId)` (`number \| null`) |
| `dataPagamento` | `string` (`yyyy-MM-dd`) | Não | Opcional | `''` | `raw.dataPagamento.trim() ? raw.dataPagamento.trim() : null` |
| `observacao` | `string` | Não | `Validators.maxLength(240)` | `''` | `raw.observacao.trim() ? raw.observacao.trim() : null` |

## 3. Mapeamento de Entrada (Modo Edição: `GET /api/v1/FinanceiroContasPagar/{id}` → Form)

Ao carregar uma `FinanceiroContaPagar` existente no modo de edição:
- `descricao`: `conta.descricao ?? ''`
- `fornecedorId`: `conta.fornecedorId ?? null`
- `valor`: `conta.valor ?? null`
- `vencimento`: extração `YYYY-MM-DD` de `conta.vencimento` (via helper `toDateInputValue(conta.vencimento)`)
- `status`: normalização `(conta.status ?? 'PENDENTE').trim().toUpperCase()` (garantindo correspondência com as opções `'PENDENTE'`, `'PAGO'`, `'VENCIDO'`, `'CANCELADO'`)
- `metodoId`: `conta.metodoId ?? null`
- `dataPagamento`: extração `YYYY-MM-DD` de `conta.dataPagamento` (ou `''` quando `null`)
- `observacao`: `conta.observacao ?? ''`

## 4. Banco de Dados (MySQL 8.0)

Nenhuma alteração de schema ou migration é necessária. A tabela `fin_contas_pagar` já contempla todas as colunas mapeadas (`Id`, `fornecedor_id`, `descricao`, `valor`, `vencimento`, `status`, `data_pagamento`, `metodo_id`, `observacao`).
