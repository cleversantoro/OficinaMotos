# Guia Rápido de Verificação: Criar e Editar Conta a Pagar (US-021)

**Branch**: `021-contas-pagar-form` | **Data**: 2026-11-16

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
1. Autenticar na aplicação (`/login`) e navegar para `/financeiro/contas-pagar`.
2. Clicar no botão **"+ Nova Conta"** no cabeçalho e confirmar a navegação para `/financeiro/contas-pagar/novo` (`T-021.2`).
3. Submeter o formulário vazio em `ContasPagarFormComponent` (`T-021.1`) e verificar as mensagens de validação obrigatória em **Descrição**, **Valor** e **Data de Vencimento**.
4. Preencher os dados de uma nova conta a pagar (descrição, fornecedor, valor, vencimento, método de pagamento, status e observação) e clicar em **"Salvar Conta"**, confirmando o envio de `POST /api/v1/FinanceiroContasPagar` (`T-021.3`), a exibição de toast de sucesso e o redirecionamento para `/financeiro/contas-pagar`.
5. Na listagem `/financeiro/contas-pagar`, clicar em **"Editar"** em uma conta existente, confirmar a navegação para `/financeiro/contas-pagar/:id/editar` (`T-021.2`), o preenchimento automático dos dados via `GET /api/v1/FinanceiroContasPagar/{id}` e a atualização via `PUT /api/v1/FinanceiroContasPagar/{id}` (`T-021.3`).
