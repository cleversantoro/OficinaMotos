# Contratos de UI: Criar e Editar Conta a Receber Manual (US-023)

**Branch**: `023-contas-receber-form` | **Data**: 2026-11-16

## 1. Rotas Registradas (`T-023.2`)
- **Criação**: `/financeiro/contas-receber/novo` → `ContasReceberFormComponent`
- **Edição**: `/financeiro/contas-receber/:id/editar` → `ContasReceberFormComponent`
- **Escopo**: Filhas de `MainLayout` (`canActivate: [authGuard]`) em [`src/app/app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts), posicionadas junto à rota `/financeiro/contas-receber`.

## 2. Componente `ContasReceberFormComponent` (`T-023.1`)
- **Seletor**: `app-contas-receber-form`
- **Caminho**: `src/app/features/financeiro/pages/contas-receber-form/contas-receber-form.ts`
- **Paradigma**: Standalone Component (`standalone: true`), `ChangeDetectionStrategy.OnPush`, `ReactiveFormsModule` + Angular Signals (`loading`, `submitting`, `error`, `isEditMode`, `contaId`, `clientes`, `metodos`).
- **Estrutura Visual (Dark Theme consistente com o módulo Financeiro)**:
  1. **Cabeçalho da Página**:
     - Link de retorno `← Voltar para Contas a Receber` (`routerLink="/financeiro/contas-receber"`).
     - Título dinâmico: `"Nova Conta a Receber"` (criação) ou `"Editar Conta a Receber #ID"` (edição).
     - Subtítulo orientativo.
  2. **Estados de Carregamento e Erro (Modo Edição)**:
     - Enquanto `loading() === true`: card de carregamento `"Carregando dados da conta a receber..."`.
     - Quando `error() !== null` (ex.: conta não encontrada 404 ou ID inválido): caixa de alerta com mensagem de erro, botão `"Tentar novamente"` (quando ID válido) e link `"Voltar para a listagem"`.
  3. **Card de Formulário (`<form [formGroup]="form" (ngSubmit)="salvar()">`)**:
     - **Descrição (`descricao`)**: `<input type="text" formControlName="descricao" maxlength="240">` (obrigatório, exibe erro quando tocado e vazio ou > 240 caracteres).
     - **Cliente (`clienteId`)**: `<select formControlName="clienteId">` com opção padrão `"Sem cliente vinculado"` (`[ngValue]="null"`) e lista de clientes (`nomeExibicao || nome`).
     - **Valor (`valor`)**: `<input type="number" step="0.01" min="0.01" formControlName="valor">` (obrigatório, exibe erro quando vazio ou `<= 0`).
     - **Data de Vencimento (`vencimento`)**: `<input type="date" formControlName="vencimento">` (obrigatório).
     - **Status (`status`)**: `<select formControlName="status" (change)="onStatusChange()">` com opções:
       - `PENDENTE` ("Pendente")
       - `VENCIDO` ("Vencido")
       - `RECEBIDO` ("Recebido")
       - `CANCELADO` ("Cancelado")
     - **Método de Pagamento (`metodoId`)**: `<select formControlName="metodoId">` com opção padrão `"Selecione um método (opcional)"` (`[ngValue]="null"`) e itens de `metodos()`.
     - **Data de Recebimento (`dataRecebimento`)**: `<input type="date" formControlName="dataRecebimento">` (opcional; preenchida automaticamente com a data de hoje quando o status é alterado para `RECEBIDO`).
     - **Observação (`observacao`)**: `<textarea formControlName="observacao" rows="3" maxlength="240">` (opcional, até 240 caracteres).
  4. **Rodapé de Ações**:
     - Botão secundário `"Cancelar"` (`type="button"`, navega para `/financeiro/contas-receber`).
     - Botão primário `"Salvar Conta"` (criação) / `"Salvar Alterações"` (edição) (`type="submit"`, desabilitado enquanto `submitting() === true`, exibindo `"Salvando..."`).

## 3. Pontos de Entrada em `ContasReceberListaComponent`
- **Cabeçalho de `/financeiro/contas-receber`**: Botão primário `+ Nova Conta` (`routerLink="/financeiro/contas-receber/novo"`).
- **Tabela de `/financeiro/contas-receber`**: Coluna `"Ações"` com link/botão `"Editar"` (`[routerLink]="['/financeiro/contas-receber', conta.id, 'editar']"`).
