# Modelo de Dados: Criar e Editar Conta a Receber Manual (US-023)

**Branch**: `023-contas-receber-form` | **Data**: 2026-11-16

## 1. Interfaces de Domínio e Payloads REST (`src/app/core/models/financeiro.ts`)

As interfaces já definidas em [`src/app/core/models/financeiro.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/financeiro.ts) atendem integralmente ao contrato do backend (`CreateFinanceiroContaReceberDTO`, `UpdateFinanceiroContaReceberDTO` e `FinanceiroContaReceberResponseDTO`):

```typescript
export interface FinanceiroContaReceber {
  id: number;
  clienteId: number | null;
  ordemServicoId?: number | null;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string;
  dataRecebimento: string | null;
  metodoId: number | null;
  observacao: string | null;
}

export interface CreateFinanceiroContaReceberRequest {
  clienteId: number | null;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string;
  dataRecebimento: string | null;
  metodoId: number | null;
  observacao: string | null;
}

export type UpdateFinanceiroContaReceberRequest = CreateFinanceiroContaReceberRequest;
```

## 2. Estrutura e Regras de Validação do Formulário (`ContasReceberFormComponent`)

| Campo do Formulário | Tipo no FormControl | Obrigatório | Validações (Frontend + Backend) | Valor Inicial (Criação) | Transformação no Payload (`POST` / `PUT`) |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `descricao` | `string` | Sim | `Validators.required`, `Validators.maxLength(240)` (após `trim()`) | `''` | `descricao.trim()` |
| `clienteId` | `number \| null` | Não | Opcional (`cad_clientes.Id`) | `null` | `parseNullableNumber(raw.clienteId)` (`number \| null`) |
| `valor` | `number \| null` | Sim | `Validators.required`, `Validators.min(0.01)` | `null` | `Number(raw.valor)` |
| `vencimento` | `string` (`yyyy-MM-dd`) | Sim | `Validators.required` | `''` | `raw.vencimento.trim()` (`yyyy-MM-dd`) |
| `status` | `string` | Sim | `Validators.required`, `Validators.maxLength(50)` (`'PENDENTE' \| 'RECEBIDO' \| 'VENCIDO' \| 'CANCELADO'`) | `'PENDENTE'` | `normalizeStatus(raw.status)` |
| `metodoId` | `number \| null` | Não | Opcional (`fin_metodos_pagamento.Id`) | `null` | `parseNullableNumber(raw.metodoId)` (`number \| null`) |
| `dataRecebimento` | `string` (`yyyy-MM-dd`) | Não | Opcional | `''` | `raw.dataRecebimento.trim() ? raw.dataRecebimento.trim() : null` |
| `observacao` | `string` | Não | `Validators.maxLength(240)` | `''` | `raw.observacao.trim() ? raw.observacao.trim() : null` |

## 3. Mapeamento de Entrada (Modo Edição: `GET /api/v1/FinanceiroContasReceber/{id}` → Form)

Ao carregar uma `FinanceiroContaReceber` existente no modo de edição:
- `descricao`: `conta.descricao ?? ''`
- `clienteId`: `conta.clienteId ?? null`
- `valor`: `conta.valor ?? null`
- `vencimento`: extração `YYYY-MM-DD` de `conta.vencimento` (via helper `toDateInputValue(conta.vencimento)`)
- `status`: normalização via `normalizeStatus(conta.status)` (`'RECEBIDO' | 'PAGO' | 'LIQUIDADO'` → `'RECEBIDO'`, `'VENCIDO'` → `'VENCIDO'`, `'CANCELADO'` → `'CANCELADO'`, demais → `'PENDENTE'`)
- `metodoId`: `conta.metodoId ?? null`
- `dataRecebimento`: extração `YYYY-MM-DD` de `conta.dataRecebimento` (ou `''` quando `null`)
- `observacao`: `conta.observacao ?? ''`

## 4. Banco de Dados (MySQL 8.0)

Nenhuma alteração de schema ou migration é necessária. A tabela `fin_contas_receber` já contempla todas as colunas mapeadas (`Id`, `cliente_id`, `descricao`, `valor`, `vencimento`, `status`, `data_recebimento`, `metodo_id`, `observacao`).
