# Modelo de Dados: Movimentações de Entrada e Saída de Estoque (US-019)

**Branch**: `019-movimentacoes-estoque` | **Data**: 2026-11-04

## 1. Interfaces TypeScript (`src/app/core/models/estoque.ts`)

As interfaces já existentes em [`estoque.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/models/estoque.ts) atendem integralmente à US-019:

```typescript
export interface EstoquePeca {
  id: number;
  codigo: string;
  descricao: string;
  fabricanteId: number | null;
  categoriaId: number | null;
  localizacaoId: number | null;
  precoUnitario: number;
  quantidade: number;
  estoqueMinimo: number;
  estoqueMaximo: number;
  unidade: string;
  status: string;
  observacoes: string | null;
}

export interface EstoqueMovimentacao {
  id: number;
  pecaId: number;
  quantidade: number;
  tipo: string; // 'ENTRADA' | 'SAIDA'
  referencia: string | null;
  dataMovimentacao: string;
  usuario: string | null;
}

export interface CreateEstoqueMovimentacaoRequest {
  pecaId: number;
  quantidade: number;
  tipo: string; // 'ENTRADA' | 'SAIDA'
  referencia: string | null;
  usuario: string | null;
}
```

## 2. Regras de Domínio e Validação

| Operação | Campo | Regra |
|---|---|---|
| Entrada (`ENTRADA`) | `quantidade` | Obrigatório, inteiro `>= 1` |
| Entrada (`ENTRADA`) | `referencia` | Obrigatório, texto não vazio (`maxLength(200)`) |
| Entrada (`ENTRADA`) | Novo Saldo | `peca.quantidade + quantidade` |
| Saída (`SAIDA`) | `quantidade` | Obrigatório, inteiro `>= 1` e `<= peca.quantidade` |
| Saída (`SAIDA`) | `referencia` | Obrigatório, texto não vazio (`maxLength(200)`) |
| Saída (`SAIDA`) | Novo Saldo | `peca.quantidade - quantidade` |
| Detalhe (`/estoque/:id`) | Estoque Crítico | `peca.quantidade < peca.estoqueMinimo` |

## 3. Banco de Dados (MySQL 8.0)

Nenhuma alteração de schema ou migration é necessária. As tabelas `est_pecas` e `est_movimentacoes` já existem no schema oficial.

