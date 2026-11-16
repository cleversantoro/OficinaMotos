# Guia Rápido de Verificação: Registrar Baixa de Conta (US-024)

**Branch**: `024-baixa-conta` | **Data**: 2026-11-16

## 1. Compilar e Validar Backend (.NET 8)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet build
```

## 2. Executar Testes Unitários do Frontend (Vitest)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm test -- --no-watch
```

## 3. Executar Build de Produção do Frontend
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm run build
```

## 4. Verificação Funcional End-to-End
1. **Baixa Parcial e Integral em Contas a Pagar (`/financeiro/contas-pagar`)**:
   - Localizar uma conta com status **Pendente** ou **Vencido** (ex.: `R$ 580,00`) e clicar em **"Baixar"**.
   - Verificar no `BaixaPagamentoModalComponent` (`T-024.1`) que a data atual e o valor total vêm preenchidos.
   - Alterar o **Valor Pago** para `200.00`, selecionar a **Forma de Pagamento** e verificar a exibição do **Saldo Residual** de `R$ 380,00` (**Pagamento Parcial**).
   - Clicar em **"Confirmar Baixa"**, validando a chamada `POST /api/v1/ContasPagarPagamentos` (`T-024.2`) e que a conta permanece em aberto na listagem com o saldo residual de `R$ 380,00`.
   - Clicar novamente em **"Baixar"** na mesma conta, confirmar o valor residual de `R$ 380,00` (**Baixa Integral**) e verificar que o status passa para **Pago**.
2. **Baixa Parcial e Integral em Contas a Receber (`/financeiro/contas-receber`)**:
   - Localizar um recebível com status **Pendente** ou **Vencido** e clicar em **"Baixar"**.
   - Testar a baixa parcial e a baixa integral, validando a chamada `POST /api/v1/ContasReceberPagamentos` (`T-024.3`), a transição de status para **Recebido** quando o saldo residual zera e a preservação do link na coluna **OS Origem**.
