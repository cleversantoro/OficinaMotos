# Guia Rápido de Verificação: Movimentações de Entrada e Saída de Estoque (US-019)

**Branch**: `019-movimentacoes-estoque` | **Data**: 2026-11-04

## 1. Executar Testes Unitários (Vitest)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm test -- --no-watch
```

## 2. Executar Build de Produção
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm run build
```

## 3. Verificação Funcional no Navegador
1. Acessar `/estoque` e clicar no nome de uma peça ou no botão **"Visualizar"** na coluna de ações.
2. Verificar que a página `/estoque/:id` exibe os dados da peça, os indicadores de saldo (com badge `Crítico` se `quantidade < estoqueMinimo`) e a tabela de histórico de movimentações.
3. Clicar em **"+ Registrar Entrada"**, informar `Quantidade = 10` e `Referência = "NF 1020"`, confirmar e verificar que o saldo aumenta em 10 unidades e a entrada aparece no topo do histórico.
4. Clicar em **"- Registrar Saída"**, tentar informar uma quantidade maior que o saldo disponível (deve bloquear e exibir erro de validação), corrigir para `Quantidade = 2` e `Referência = "OS #45"`, confirmar e verificar que o saldo diminui em 2 unidades e a saída aparece no histórico.

