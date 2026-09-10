# Guia de Validação: Adicionar/Remover Peças e Serviços na OS

## Pré-requisitos
- Repositório clonado e dependências instaladas em `oficina-motos-web`.
- Usuário logado com perfil operacional no sistema.

## Verificação Automatizada

Executar os testes unitários do módulo de ordens:

```powershell
Set-Location C:\Projetos\OficinaMotos\oficina-motos-web
npm run build
npx ng test --no-watch --include src/app/features/ordens-servico/**/*.spec.ts
```

## Roteiro de Validação Manual

### Cenário 1: Adicionar Peça do Estoque
1. Acessar uma OS ativa em `/ordens/:id` (ex.: `/ordens/1`).
2. Clicar no botão "Adicionar Peça".
3. No campo de busca, digitar ao menos 2 caracteres (ex.: "filtro" ou "óleo").
4. Selecionar a peça desejada na lista de sugestões.
5. Conferir o preenchimento automático da descrição e do preço unitário.
6. Alterar a quantidade para 2 e conferir o subtotal atualizado.
7. Clicar em "Adicionar Peça".
8. **Verificar**:
   - Modal fecha;
   - Nova linha inserida na tabela de itens;
   - Total dos Itens e Saldo Pendente incrementados corretamente;
   - Toast de sucesso exibido;
   - Nenhuma recarga da página inteira ocorreu.

### Cenário 2: Adicionar Serviço / Mão de Obra
1. Clicar no botão "Adicionar Serviço".
2. Preencher a descrição: "Revisão e limpeza de carburador".
3. Informar o valor de mão de obra: R$ 130,00.
4. Clicar em "Adicionar Serviço".
5. **Verificar**:
   - Modal fecha e linha de serviço é adicionada à tabela;
   - Total da OS atualizado com mais R$ 130,00;
   - Toast de sucesso exibido.

### Cenário 3: Excluir Item com Confirmação
1. Localizar um item adicionado na tabela de itens.
2. Clicar no botão de lixeira correspondente.
3. No diálogo de confirmação exibido, clicar em "Cancelar" e conferir que o item permanece.
4. Clicar novamente na lixeira e confirmar no diálogo.
5. **Verificar**:
   - Chamada `DELETE /api/v1/OrdemServicoItens/{id}` executada com sucesso;
   - Item removido da tabela;
   - Total dos itens e saldo pendente recalculados para baixo;
   - Toast de sucesso apresentado.

### Cenário 4: Bloqueio em OS Concluída ou Cancelada
1. Acessar uma OS com status `Concluida` ou `Cancelada`.
2. **Verificar**:
   - Os botões "Adicionar Peça" e "Adicionar Serviço" estão desabilitados ou ocultos;
   - A coluna de ações/exclusão de itens não permite mutações.
