# Modelo de Dados: Listar Contas a Pagar (US-020)

**Branch**: `020-contas-pagar-lista` | **Data**: 2026-11-16

## 1. Interfaces TypeScript Utilizadas

As interfaces existentes em [`src/app/core/models/financeiro.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/financeiro.ts) e [`src/app/core/models/fornecedor.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/fornecedor.ts) atendem integralmente à funcionalidade:

```typescript
export interface FinanceiroContaPagar {
  id: number;
  fornecedorId: number | null;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string; // 'PENDENTE' | 'VENCIDO' | 'PAGO' | 'CANCELADO'
  dataPagamento: string | null;
  metodoId: number | null;
  observacao: string | null;
}

export interface FinanceiroMetodoPagamento {
  id: number;
  nome: string;
  descricao: string | null;
}
```

### Tipos Auxiliares de UI (`ContasPagarListaComponent`)

```typescript
export type ContaPagarStatusEfetivo = 'PENDENTE' | 'VENCIDO' | 'PAGO' | 'CANCELADO';
export type ContaPagarStatusFiltro = 'TODOS' | ContaPagarStatusEfetivo;
```

## 2. Regras de Derivação de Status e Filtragem

| Regra | Condição | Resultado Esperado |
| :--- | :--- | :--- |
| **Status `PAGO`** | `conta.status.toUpperCase() === 'PAGO'` | Status efetivo `'PAGO'`, `isVencida = false`, badge verde `"Pago"` |
| **Status `CANCELADO`** | `conta.status.toUpperCase() === 'CANCELADO'` | Status efetivo `'CANCELADO'`, `isVencida = false`, badge neutro `"Cancelado"` |
| **Status `VENCIDO`** | `conta.status.toUpperCase() === 'VENCIDO'` **OU** (`status` em aberto e `vencimentoDate < hojeDate` sem `dataPagamento`) | Status efetivo `'VENCIDO'`, `isVencida = true`, linha `.row-vencida`, badge vermelho `"Vencido"` |
| **Status `PENDENTE`** | `status` em aberto (`'PENDENTE'` / `'ABERTO'`) e `vencimentoDate >= hojeDate` | Status efetivo `'PENDENTE'`, `isVencida = false`, badge azul/âmbar `"Pendente"` |
| **Filtro de Status** | `statusFilter() !== 'TODOS'` | Mantém apenas contas com `getEffectiveStatus(conta) === statusFilter()` |
| **Filtro Data Inicial** | `dataInicio() !== ''` | Mantém apenas contas com `toDateKey(conta.vencimento) >= dataInicio()` |
| **Filtro Data Final** | `dataFim() !== ''` | Mantém apenas contas com `toDateKey(conta.vencimento) <= dataFim()` |
| **Busca Textual** | `searchTerm().trim() !== ''` | Mantém contas cuja `descricao`, `observacao`, nome do fornecedor ou `#fornecedorId` contenha o termo |

## 3. Banco de Dados (MySQL 8.0)

Nenhuma modificação de schema é necessária. A tabela `fin_contas_pagar` já possui todas as colunas utilizadas (`Id`, `fornecedor_id`, `descricao`, `valor`, `vencimento`, `status`, `data_pagamento`, `metodo_id`, `observacao`).
