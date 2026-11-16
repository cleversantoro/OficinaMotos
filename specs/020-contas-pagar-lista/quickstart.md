# Guia Rápido de Verificação: Listar Contas a Pagar (US-020)

**Branch**: `020-contas-pagar-lista` | **Data**: 2026-11-16

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

## 3. Verificação Funcional End-to-End
1. Autenticar na aplicação (`/login`) e navegar para `/financeiro` ou diretamente para `/financeiro/contas-pagar`.
2. Confirmar que os cards de resumo (**Total em Aberto**, **Contas Vencidas** e **Total Pago**) e a tabela de contas a pagar são exibidos com os dados de `GET /api/v1/FinanceiroContasPagar`.
3. Verificar que contas não pagas com vencimento anterior à data atual (ou com status `VENCIDO`) apresentam destaque visual na linha (`.row-vencida`), indicador de atraso na data de vencimento e badge **"Vencido"** (`T-020.4`).
4. Selecionar o filtro **Status = Vencido** (ou **Pendente** / **Pago**) e informar um intervalo em **Vencimento de** / **Até** (`T-020.3`), verificando que a tabela atualiza imediatamente os registros listados.
5. Clicar em **"Limpar filtros"** e confirmar que todos os registros voltam a ser exibidos.
