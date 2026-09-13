# Guia de Validação: Registrar Pagamento de Ordem de Serviço

## Pré-requisitos

1. **Backend**: Solução `.NET` compilada sem erros e banco de dados MySQL acessível.
2. **Frontend**: Aplicação Angular 21 com dependências instaladas.
3. **Autenticação**: Usuário autenticado com perfil com permissão para operar e visualizar ordens de serviço.

---

## Verificação Automatizada

### 1. Backend (.NET)
Executar compilação e suíte de testes:

```powershell
Set-Location C:\Projetos\OficinaMotos\oficina-motos-api
dotnet test
```

### 2. Frontend (Angular / Vitest)
Executar verificação de tipos, build e testes unitários do módulo de ordens:

```powershell
Set-Location C:\Projetos\OficinaMotos\oficina-motos-web
npx ng test --no-watch --include src/app/features/ordens-servico/**/*.spec.ts
```

---

## Roteiro de Validação Manual

### Cenário 1: Pagamento Integral com Conclusão Automática da OS
1. Acesse uma OS ativa não concluída em `/ordens/:id` (ex.: `/ordens/1`) que possua saldo pendente (ex.: R$ 250,00).
2. Na seção **"Pagamentos"**, verifique que o botão **"Registrar Pagamento"** está visível e habilitado.
3. Clique em **"Registrar Pagamento"**:
   - O modal `OsPagamentoModalComponent` deve abrir;
   - O campo "Valor Pago" deve vir preenchido com o saldo pendente (R$ 250,00);
   - A data deve indicar o dia atual;
   - O badge informativo deve indicar "Pagamento Integral — Concluirá a OS".
4. Selecione a forma de pagamento **"PIX"** e insira uma observação ("Pagamento via chave CNPJ").
5. Clique em **"Confirmar Pagamento"**.
6. **Resultado Esperado**:
   - O modal fecha imediatamente;
   - Um Toast verde de sucesso é exibido ("Pagamento registrado com sucesso!");
   - A tabela de pagamentos exibe a nova linha (PIX, R$ 250,00, data atual);
   - O indicador "Total Pago" incrementa em R$ 250,00 e o "Saldo Pendente" passa para R$ 0,00;
   - O badge de status da OS muda automaticamente para **"Concluída"**;
   - Os botões de adicionar peças/serviços e alterar status são desabilitados/ocultados.

---

### Cenário 2: Pagamento Parcial (Sinal / Entrada)
1. Acesse outra OS ativa com total de itens de R$ 500,00 e sem pagamentos anteriores.
2. Clique em **"Registrar Pagamento"**.
3. Altere o valor sugerido de R$ 500,00 para **R$ 200,00**.
4. Observe que o badge informativo atualiza em tempo real para "Pagamento Parcial — Saldo Restante: R$ 300,00".
5. Selecione a forma **"Dinheiro"** e confirme.
6. **Resultado Esperado**:
   - O pagamento de R$ 200,00 é listado na tabela;
   - O "Total Pago" passa para R$ 200,00 e o "Saldo Pendente" para R$ 300,00;
   - O status da OS permanece como **"Em Andamento"** (não é concluída prematuramente);
   - Os botões de edição continuam disponíveis.

---

### Cenário 3: Validação do Lançamento Automático em Contas a Receber
1. No backend, realize uma consulta à tabela `fin_contas_receber` ou chame `GET /api/v1/FinanceiroContasReceber`:
2. Localize o registro recém-criado correspondente ao pagamento realizado:
   - `ClienteId` deve ser o mesmo cliente da OS;
   - `Descricao` deve conter `Pagamento OS #{ordemId}`;
   - `Valor` deve ser exatamente o valor pago;
   - `Status` deve ser `Recebido`.

---

### Cenário 4: Bloqueio em OS Cancelada ou Já Quitada
1. Acesse uma OS com status `Cancelada`:
   - O botão "Registrar Pagamento" deve estar desabilitado ou oculto.
2. Acesse uma OS com saldo pendente de R$ 0,00:
   - A seção exibe o indicativo "OS Quitada" e impede novos pagamentos.

