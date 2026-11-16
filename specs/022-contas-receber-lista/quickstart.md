# Guia Rápido de Verificação: Listar Contas a Receber (US-022)

**Branch**: `022-contas-receber-lista` | **Data**: 2026-11-16

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
1. Autenticar na aplicação (`/login`) e navegar para `/financeiro/contas-receber` (ou via aba "Contas a Receber" → "Ver listagem completa" em `/financeiro`).
2. Confirmar que os cards de resumo (**A receber (em aberto)**, **Recebíveis vencidos** e **Total recebido**) e a tabela de contas a receber são exibidos com os dados de `GET /api/v1/FinanceiroContasReceber` (`T-022.1`, `T-022.2`).
3. Verificar a coluna **"OS Origem"** (`T-022.3`): para recebíveis originados de Ordem de Serviço (ex.: `"Pagamento OS #12 - PIX"`), clicar no link `OS #12` e confirmar a navegação para `/ordens/12`.
4. Testar os filtros por status, período de vencimento e busca textual.
