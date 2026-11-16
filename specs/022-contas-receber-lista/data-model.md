# Modelo de Dados: Listar Contas a Receber (US-022)

**Branch**: `022-contas-receber-lista` | **Data**: 2026-11-16

## 1. Interfaces TypeScript Utilizadas (`src/app/core/models/financeiro.ts` e `cliente.ts`)

```typescript
export interface FinanceiroContaReceber {
  id: number;
  clienteId: number | null;
  ordemServicoId?: number | null;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string; // 'PENDENTE' | 'RECEBIDO' | 'PAGO' | 'LIQUIDADO' | 'VENCIDO' | 'CANCELADO'
  dataRecebimento: string | null;
  metodoId: number | null;
  observacao: string | null;
}
```

### Tipos Auxiliares de UI (`ContasReceberListaComponent`)

```typescript
export type ContaReceberStatusEfetivo = 'PENDENTE' | 'VENCIDO' | 'RECEBIDO' | 'CANCELADO';
export type ContaReceberStatusFiltro = 'TODOS' | ContaReceberStatusEfetivo;

export interface ContaReceberKpi {
  label: string;
  valor: number;
  quantidade: number;
  tipo: 'entrada' | 'alerta' | 'recebido';
}
```

## 2. Regras de Derivação de "OS Origem" e Status

| Regra | Condição | Resultado Esperado |
| :--- | :--- | :--- |
| **OS Origem via propriedade** | `Number.isInteger(conta.ordemServicoId) && conta.ordemServicoId > 0` | Retorna `conta.ordemServicoId` e exibe link `OS #{{ osId }}` para `/ordens/{{ osId }}` |
| **OS Origem via texto (`US-012`)** | Match de `/\bOS\s*#?\s*(\d+)\b/i` em `conta.descricao` ou `conta.observacao` | Retorna `Number(match[1])` e exibe link `OS #{{ osId }}` para `/ordens/{{ osId }}` |
| **Sem OS Origem (Conta manual)** | Nenhuma referência encontrada | Retorna `null` e exibe `-` na coluna `"OS Origem"` |
| **Status `RECEBIDO`** | `rawStatus` em `['RECEBIDO', 'PAGO', 'LIQUIDADO']` | Status efetivo `'RECEBIDO'`, badge verde `"Recebido"` |
| **Status `CANCELADO`** | `rawStatus === 'CANCELADO'` | Status efetivo `'CANCELADO'`, badge neutro `"Cancelado"` |
| **Status `VENCIDO`** | `rawStatus === 'VENCIDO'` **OU** (`status` em aberto e `vencimento < hoje` sem `dataRecebimento`) | Status efetivo `'VENCIDO'`, linha `.row-vencida`, badge vermelho `"Vencido"` |
| **Status `PENDENTE`** | `status` em aberto e `vencimento >= hoje` | Status efetivo `'PENDENTE'`, badge azul `"Pendente"` |

## 3. Banco de Dados (MySQL 8.0)

Nenhuma alteração de schema ou migration é necessária. A tabela `fin_contas_receber` e o `FinanceiroContasReceberController` já existem e atendem integralmente à consulta.
