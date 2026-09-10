# Pesquisa Técnica: Adicionar/Remover Peças e Serviços na OS

## Decisão 1: Modais Especializados vs Modal Único Genérico

**Decisão**: Criar dois componentes de modal standalone distintos: `OsItemPecaModalComponent` e `OsItemServicoModalComponent`.

**Racional**:
- Uma peça do estoque requer busca dinâmica no catálogo, vínculo de `pecaId`, exibição de estoque atual disponível, código e unidade de medida.
- Um serviço/mão de obra não possui `pecaId` (deve ser `null`), necessita de um campo de texto livre com validação de até 240 caracteres e campo específico de valor de mão de obra.
- Separar os modais melhora a clareza para o usuário da oficina (botões claros "Adicionar Peça" e "Adicionar Serviço") e evita formulários com abas confusas ou campos condicionais poluídos.

**Alternativas consideradas**:
- Modal único com rádio "Peça / Serviço": rejeitado por gerar complexidade desnecessária no template e validações condicionais que aumentam a chance de bugs.

## Decisão 2: Autocomplete de Peças do Estoque

**Decisão**: Integrar com o serviço já existente `EstoqueService.pecas()` (`/api/v1/EstoquePecas`).
O autocomplete filtra localmente ou consulta a API conforme o usuário digita ao menos 2 caracteres. Ao selecionar uma peça:
- Atribui `pecaId = peca.id`;
- Preenche a descrição automaticamente como `${peca.codigo} - ${peca.descricao}`;
- Preenche o valor unitário com `peca.precoUnitario`;
- Exibe o estoque disponível (`peca.quantidade`) para que o operador saiba se há peças físicas disponíveis.

**Racional**: Reutiliza a infraestrutura de serviços do projeto, não cria endpoints novos e oferece uma experiência ágil.

## Decisão 3: Integração HTTP para Adição e Remoção de Itens

**Decisão**:
- Adicionar métodos tipados em `OrdensService`:
  - `addItem<T, B>(body: B)`: invoca `this.api.create(apiPaths.ordens.itens, body)` (`POST /api/v1/OrdemServicoItens`);
  - `deleteItem(itemId: string | number)`: invoca `this.api.remove(apiPaths.ordens.itens, itemId)` (`DELETE /api/v1/OrdemServicoItens/{id}`).
- Na adição: o backend retorna o `OrdemServicoItemResponseDTO` completo (com `id`, `total`, etc.).
- Na exclusão: o backend retorna `204 NoContent`.

**Racional**: Mantém o padrão de serviços centralizados exigido pela Constituição (Princípio II), garantindo zero URLs literais nos componentes.

## Decisão 4: Recálculo Reativo sem Reload Completo

**Decisão**:
- No `OsDetalheComponent`, o estado da OS é armazenado no signal `ordem`.
- Ao receber o item criado da API:
  ```typescript
  this.ordem.update(atual => {
    if (!atual) return null;
    return {
      ...atual,
      itens: [...(atual.itens || []), novoItem]
    };
  });
  ```
- Ao excluir um item com sucesso:
  ```typescript
  this.ordem.update(atual => {
    if (!atual) return null;
    return {
      ...atual,
      itens: (atual.itens || []).filter(i => i.id !== itemId)
    };
  });
  ```
- Como `totalItens` e `saldoPendente` são Signals computados (`computed()`), qualquer alteração no signal `ordem` recalcula instantaneamente os totais, as somas das tabelas e os cards do resumo financeiro sem necessidade de recarregar a rota ou fazer novas requisições de cliente/veículo.

## Decisão 5: Confirmação Segura de Exclusão

**Decisão**: Utilizar o serviço wrapper existente `Confirmation.confirmDelete(descricaoItem)`.
Caso o usuário confirme, o botão exibe spinner de carregamento, dispara `OrdensService.deleteItem` e notifica sucesso via `Toast.success`.

**Racional**: Previne exclusões acidentais que impactariam o faturamento e histórico da ordem.
