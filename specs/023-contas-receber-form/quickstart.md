# Guia Rápido de Verificação: Criar e Editar Conta a Receber Manual (US-023)

**Branch**: `023-contas-receber-form` | **Data**: 2026-11-16

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
1. Autenticar na aplicação (`/login`) e navegar para `/financeiro/contas-receber`.
2. Clicar no botão **"+ Nova Conta"** no cabeçalho e confirmar a navegação para `/financeiro/contas-receber/novo` (`T-023.2`).
3. Submeter o formulário vazio em `ContasReceberFormComponent` (`T-023.1`) e verificar as mensagens de validação obrigatória em **Descrição**, **Valor** e **Data de Vencimento**.
4. Preencher os dados de uma nova conta a receber manual (descrição, cliente opcional, valor, vencimento, método de pagamento, status e observação) e clicar em **"Salvar Conta"**, confirmando o envio de `POST /api/v1/FinanceiroContasReceber`, a exibição de toast de sucesso e o redirecionamento para `/financeiro/contas-receber`.
5. Na listagem `/financeiro/contas-receber`, clicar em **"Editar"** em uma conta existente, confirmar a navegação para `/financeiro/contas-receber/:id/editar` (`T-023.2`), o preenchimento automático dos dados via `GET /api/v1/FinanceiroContasReceber/{id}` e a atualização via `PUT /api/v1/FinanceiroContasReceber/{id}`.
