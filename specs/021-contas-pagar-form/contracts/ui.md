# Contratos de UI: Criar e Editar Conta a Pagar (US-021)

**Branch**: `021-contas-pagar-form` | **Data**: 2026-11-16

## 1. Rotas Registradas (`T-021.2`)
- **Criação**: `/financeiro/contas-pagar/novo` → `ContasPagarFormComponent`
- **Edição**: `/financeiro/contas-pagar/:id/editar` → `ContasPagarFormComponent`
- **Escopo**: Filhas de `MainLayout` (`canActivate: [authGuard]`) em [`src/app/app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts), posicionadas junto à rota `/financeiro/contas-pagar`.

## 2. Componente `ContasPagarFormComponent` (`T-021.1`)
- **Seletor**: `app-contas-pagar-form`
- **Caminho**: `src/app/features/financeiro/pages/contas-pagar-form/contas-pagar-form.ts`
- **Paradigma**: Standalone Component (`standalone: true`), `ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule` + Angular Signals (`loading`, `submitting`, `error`, `isEditMode`, `contaId`, `fornecedores`, `metodos`).
- **Estrutura Visual (Dark Theme consistente com o módulo Financeiro)**:
  1. **Cabeçalho da Página**:
     - Link de retorno `← Voltar para Contas a Pagar` (`routerLink="/financeiro/contas-pagar"`).
     - Título dinâmico: `"Nova Conta a Pagar"` (criação) ou `"Editar Conta a Pagar #ID"` (edição).
     - Subtítulo orientativo.
  2. **Estados de Carregamento e Erro (Modo Edição)**:
     - Enquanto `loading() === true`: card de carregamento `"Carregando dados da conta a pagar..."`.
     - Quando `error() !== null` (ex.: conta não encontrada 404 ou ID inválido): caixa de alerta com mensagem de erro, botão `"Tentar novamente"` (quando ID válido) e link `"Voltar para a listagem"`.
  3. **Card de Formulário (`<form [formGroup]="form" (ngSubmit)="salvar()">`)**:
     - **Descrição (`descricao`)**: `<input type="text" formControlName="descricao" maxlength="240">` (obrigatório, exibe erro quando tocado e vazio ou > 240 caracteres).
     - **Fornecedor (`fornecedorId`)**: `<select formControlName="fornecedorId">` com opção padrão `"Sem fornecedor vinculado"` (`[ngValue]="null"`) e lista de fornecedores ativos (`nomeFantasia || razaoSocial`).
     - **Valor (`valor`)**: `<input type="number" step="0.01" min="0.01" formControlName="valor">` (obrigatório, exibe erro quando vazio ou `<= 0`).
     - **Data de Vencimento (`vencimento`)**: `<input type="date" formControlName="vencimento">` (obrigatório).
     - **Status (`status`)**: `<select formControlName="status">` com opções:
       - `PENDENTE` ("Pendente")
       - `VENCIDO` ("Vencido")
       - `PAGO` ("Pago")
       - `CANCELADO` ("Cancelado")
     - **Método de Pagamento (`metodoId`)**: `<select formControlName="metodoId">` com opção padrão `"Selecione um método (opcional)"` (`[ngValue]="null"`) e itens de `metodos()`.
     - **Data de Pagamento (`dataPagamento`)**: `<input type="date" formControlName="dataPagamento">` (opcional).
     - **Observação (`observacao`)**: `<textarea formControlName="observacao" rows="3" maxlength="240">` (opcional, até 240 caracteres).
  4. **Rodapé de Ações**:
     - Botão secundário `"Cancelar"` (`type="button"`, navega para `/financeiro/contas-pagar`).
     - Botão primário `"Salvar Conta"` (criação) / `"Salvar Alterações"` (edição) (`type="submit"`, desabilitado enquanto `submitting() === true`, exibindo `"Salvando..."`).

## 3. Pontos de Entrada em `ContasPagarListaComponent`
- **Cabeçalho de `/financeiro/contas-pagar`**: Botão primário `+ Nova Conta` (`routerLink="/financeiro/contas-pagar/novo"`).
- **Tabela de `/financeiro/contas-pagar`**: Coluna `"Ações"` com link/botão `"Editar"` (`[routerLink]="['/financeiro/contas-pagar', conta.id, 'editar']"`).
