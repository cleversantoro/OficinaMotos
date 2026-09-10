# Especificação de Funcionalidade: Adicionar/Remover Peças e Serviços na OS

**Feature Branch**: `011-os-itens`

**Criado**: 2026-09-09

**Status**: Draft

**Entrada**: US-011 — Adicionar/remover peças e serviços na OS
- **Prioridade**: 🔴 Must | **Estimativa**: M | **Sprint**: 2 | **Depende**: US-010
- **Critério de aceite**:
  - Modal "Adicionar Peça": busca no estoque, quantidade, valor unitário
  - Modal "Adicionar Serviço": descrição livre, valor de mão de obra
  - Total da OS recalculado após cada alteração
- **Tasks**:
  - [ ] T-011.1 — Criar OsItemPecaModalComponent com autocomplete de estoque
  - [ ] T-011.2 — Criar OsItemServicoModalComponent
  - [ ] T-011.3 — Integrar POST /api/v1/OrdemServicoItens
  - [ ] T-011.4 — Integrar DELETE /api/v1/OrdemServicoItens/{itemId}
  - [ ] T-011.5 — Calcular e exibir totais sem reload completo

**Dependências**: US-010 — Criar página de detalhe real da OS

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Adicionar Peça do Estoque à Ordem de Serviço (Prioridade: P1)

Como mecânico ou recepcionista autorizado visualizando a página de detalhe da OS (`/ordens/:id`), quero abrir um modal para pesquisar peças cadastradas no estoque, definir a quantidade utilizada e o valor unitário, e adicioná-la aos itens da OS, para que as peças físicas consumidas na moto fiquem registradas e precificadas com exatidão.

**Por que esta prioridade**: A composição de peças de reposição representa a maior parte dos custos de insumos na oficina mecânica. Sem a seleção direta do catálogo do estoque, os operadores precisariam digitar códigos e preços manualmente, gerando inconsistências contábeis e falta de rastreabilidade.

**Teste independente**: Acessar uma OS ativa (`/ordens/:id`), acionar o botão "Adicionar Peça", digitar parte do código ou nome de uma peça existente no autocomplete, selecionar o item sugerido, ajustar a quantidade (ex.: 2 unidades) e o valor se necessário, clicar em "Adicionar" e confirmar que a nova peça passa a constar na tabela de itens com seu subtotal correto.

**Cenários de aceitação**:

1. **Dado** que o usuário está na tela `/ordens/:id` de uma OS ativa (status `Aberta`, `EmAndamento` ou `AguardandoPeca`), **quando** clicar no botão "Adicionar Peça", **então** o modal `OsItemPecaModalComponent` será aberto na tela.
2. **Dado** que o modal de peça está aberto, **quando** o usuário digitar ao menos 2 caracteres no campo de busca/autocomplete de peças, **então** o componente consultará o catálogo de peças ativas via `EstoqueService.pecas()` exibindo código, descrição, preço de venda e saldo em estoque.
3. **Dado** que o usuário selecionou uma peça sugerida, **quando** o clique de seleção for efetuado, **então** os campos do modal serão pré-preenchidos automaticamente com a descrição (`codigo - descricao`), valor unitário padrão (`precoUnitario`) e quantidade inicial sugerida (1).
4. **Dado** que a peça foi selecionada e o usuário ajustou a quantidade e/ou o valor unitário, **quando** os valores forem informados, **então** o modal apresentará em tempo real o subtotal calculado (`quantidade * valorUnitario`) formatado em moeda brasileira (`BRL`).
5. **Dado** que o formulário está preenchido com dados válidos, **quando** o usuário clicar no botão "Adicionar", **então** o sistema enviará uma requisição `POST /api/v1/OrdemServicoItens` contendo `ordemServicoId`, `pecaId`, `descricao`, `quantidade` e `valorUnitario`, mantendo o botão desabilitado e exibindo spinner durante o envio.
6. **Dado** que a requisição de cadastro foi concluída com sucesso (HTTP 201 Created), **quando** a resposta for recebida, **então** o modal será fechado, o novo item será inserido na lista local de itens da OS, uma notificação Toast de sucesso será apresentada e os totais serão atualizados sem recarregar a tela.
7. **Dado** que o envio falhou na comunicação com a API, **quando** o erro for retornado, **então** o modal permanecerá aberto, exibirá mensagem de erro explicativa via Toast e reabilitará os controles para nova tentativa.

---

### História de Usuário 2 — Adicionar Serviço / Mão de Obra à Ordem de Serviço (Prioridade: P1)

Como mecânico ou recepcionista autorizado, quero abrir um modal para adicionar um item de serviço ou mão de obra com descrição livre e valor da prestação do serviço, para discriminar as tarefas executadas no veículo.

**Por que esta prioridade**: Nem todas as atividades de um atendimento envolvem troca de peças físicas. Serviços como lavagem técnica, revisão geral, regulagem de válvulas e socorro mecânico são puramente mão de obra e precisam ser lançados com descrições específicas.

**Teste independente**: Acessar uma OS ativa, acionar o botão "Adicionar Serviço", preencher a descrição (ex.: "Revisão e limpeza de bicos injetores"), quantidade de serviços (ex.: 1) e valor da mão de obra (ex.: R$ 120,00), submeter o formulário e verificar a inserção imediata da linha de serviço na tabela de itens.

**Cenários de aceitação**:

1. **Dado** que o usuário está no detalhe de uma OS ativa, **quando** clicar no botão "Adicionar Serviço", **então** o modal `OsItemServicoModalComponent` será exibido.
2. **Dado** que o modal de serviço está aberto, **quando** o usuário preencher o campo de descrição do serviço (obrigatório, até 240 caracteres), quantidade (inteiro positivo $\ge 1$, padrão 1) e valor unitário de mão de obra (numérico decimal $\ge 0$), **então** o modal exibirá o subtotal calculado da mão de obra em tempo real.
3. **Dado** que os campos foram preenchidos corretamente, **quando** o usuário confirmar clicando em "Adicionar Serviço", **então** o sistema enviará `POST /api/v1/OrdemServicoItens` com `pecaId: null`, `ordemServicoId`, `descricao`, `quantidade` e `valorUnitario`.
4. **Dado** que o serviço foi registrado com sucesso pela API, **quando** a resposta for recebida, **então** o modal será fechado, o serviço será incluído na lista local de itens da OS, uma notificação Toast de sucesso será exibida e os totais financeiros da página serão recalculados.
5. **Dado** que o usuário tentou submeter o formulário sem preencher a descrição ou com valor de mão de obra inválido, **quando** clicar em salvar, **então** o sistema impedirá o envio e exibirá mensagens de validação nos campos obrigatórios.

---

### História de Usuário 3 — Remover Item (Peça ou Serviço) da Ordem de Serviço (Prioridade: P1)

Como usuário autorizado da oficina, quero excluir um item (seja peça ou serviço) da tabela de itens da OS, mediante confirmação explícita de exclusão, para ajustar o orçamento caso o cliente decline de um reparo ou um item tenha sido inserido erroneamente.

**Por que esta prioridade**: Erros operacionais de lançamento e recusas de itens pelo cliente antes da conclusão da OS fazem parte da rotina de qualquer oficina mecânica. A exclusão segura é mandatória para manter o fechamento de caixa e o faturamento corretos.

**Teste independente**: Localizar um item existente na tabela de itens da OS, clicar no botão de excluir (ícone de lixeira), responder afirmativamente ao diálogo de confirmação seguro (`Confirmation.confirmDelete`), observar a execução de `DELETE /api/v1/OrdemServicoItens/{itemId}` e verificar a remoção da linha da tabela com atualização dos totais.

**Cenários de aceitação**:

1. **Dado** que a tabela de itens da OS exibe um ou mais itens e a OS não está em status terminal, **quando** a lista for visualizada, **então** cada linha de item possuirá uma coluna de ações contendo o botão de exclusão.
2. **Dado** que o usuário clicou no botão de exclusão de um item, **quando** o clique for processado, **então** um diálogo de confirmação (`Confirmation.confirmDelete`) será apresentado na tela com texto claro indicando o nome do item a ser removido.
3. **Dado** que o usuário optou por cancelar o diálogo de confirmação, **quando** acionar "Cancelar" ou fechar o diálogo, **então** nenhuma chamada HTTP será disparada e o item permanecerá intacto na tabela.
4. **Dado** que o usuário confirmou a exclusão, **quando** a API retornar sucesso (HTTP 204 NoContent), **então** o item será removido da lista local de itens no signal da OS, um Toast de sucesso será exibido e os totais da OS serão recalculados sem recarregar a tela.
5. **Dado** que a requisição de exclusão retornou erro da API, **quando** a falha ocorrer, **então** o item permanecerá visível na tabela e um Toast de erro notificará o usuário da impossibilidade de exclusão.

---

### História de Usuário 4 — Recálculo Dinâmico e Reativo dos Totais e Resumo Financeiro sem Reload (Prioridade: P1)

Como operador ou gestor da oficina, quero que a inclusão ou remoção de itens recalcule automaticamente todos os valores consolidados da OS (total de itens, saldo devedor e total geral) em tempo real via reatividade sem necessidade de recarregar a página, para que o trabalho seja ágil e sem perda de contexto.

**Por que esta prioridade**: Um recarregamento completo da página causa piscadas de tela, perda de foco, re-execução desnecessária de consultas pesadas de cliente/veículo/mecanico e prejudica a experiência do usuário.

**Teste independente**: Adicionar um item de R$ 80,00 e conferir que o badge "Total Itens", o rodapé da tabela e os cards "Total da OS" e "Saldo Pendente" no bloco de pagamentos incrementam em exatos R$ 80,00 de maneira imediata; em seguida, remover o item e confirmar o decréscimo imediato para o valor anterior.

**Cenários de aceitação**:

1. **Dado** que um item foi adicionado ou excluído na OS, **quando** a coleção de itens do signal `ordem` for atualizada localmente, **então** o computed `totalItens` atualizará seu valor instantaneamente via soma das propriedades `total` (ou `quantidade * valorUnitario`).
2. **Dado** que o `totalItens` foi recalculado, **quando** o novo valor estiver disponível, **então** o computed `saldoPendente` (`Math.max(0, totalItens() - totalPago())`) atualizará seu valor e severidade visual imediatamente no resumo financeiro.
3. **Dado** que a OS não possuía itens cadastrados (estado vazio exibido), **quando** o primeiro item for adicionado com sucesso, **então** a mensagem de estado vazio desaparecerá instantaneamente e a tabela de itens será apresentada com o novo registro.
4. **Dado** que a OS possuía apenas um item e ele foi excluído com sucesso, **quando** a lista de itens ficar vazia, **então** a tabela desaparecerá e a mensagem informativa de estado vazio voltará a ser exibida.

---

### História de Usuário 5 — Proteção de Integridade em Ordens Finalizadas (Prioridade: P2)

Como gestor da oficina mecânica, quero que ordens finalizadas (com status `Concluida` ou `Cancelada`) fiquem protegidas contra adição ou remoção de peças e serviços, assegurando a integridade histórica, fiscal e contábil do sistema.

**Por que esta prioridade**: Mutações em ordens concluídas geram divergências irreversíveis entre o estoque físico, o caixa financeiro e as notas fiscais emitidas.

**Teste independente**: Abrir uma OS com status `Concluida` ou `Cancelada` e verificar que os botões "Adicionar Peça" e "Adicionar Serviço" estão ausentes/desabilitados e que nenhuma linha de item possui botão de exclusão.

**Cenários de aceitação**:

1. **Dado** que a OS possui status `Concluida` ou `Cancelada` (`isTerminal() === true`), **quando** a página for renderizada, **então** os botões "Adicionar Peça" e "Adicionar Serviço" no cabeçalho da seção de itens não serão exibidos (ou estarão desabilitados).
2. **Dado** que a OS está em estado terminal, **quando** a tabela de itens for exibida, **então** a coluna de ações de exclusão não será renderizada, impedindo a exclusão de itens.

---

### Casos de Borda

- **Peça com estoque zerado ou insuficiente**: O autocomplete exibe o saldo de estoque atual; caso o usuário selecione uma peça com estoque zerado, o sistema deve apresentar um alerta visual de advertência, mas sem bloquear a adição se a oficina operar com pedido sob demanda.
- **Formatação monetária e casas decimais**: Inserção de valores decimais fracionados (ex.: R$ 22,75) tratados com precisão em 2 casas decimais, evitando imprecisões de arredondamento de ponto flutuante.
- **Validação de quantidade inteira positiva**: Bloquear inserção de valores como 0, negativos ou não numéricos no campo quantidade de peças (quantidade mínima = 1).
- **Limite de caracteres na descrição do serviço**: O campo de descrição deve ter `maxlength="240"` no template com validação reativa para respeitar a restrição imposta por `CreateOrdemServicoItemValidator` no backend.
- **Prevenção de duplo clique (duplicação de itens)**: Botões de submissão dos modais exibem estado de carregamento (`loading`) e ficam desabilitados enquanto a requisição HTTP estiver em trânsito.
- **Fechamento voluntário sem salvar**: Pressionar a tecla ESC, o botão de fechar (X) ou o botão "Cancelar" do modal deve descartar o formulário sem salvar e sem disparar requisições.
- **Falha de rede na adição ou remoção**: Se a API retornar erro (HTTP 400, 404 ou 500), o sistema mantém os dados intactos, emite Toast de erro e permite que o usuário tente novamente sem recarregar a tela.

---

## Requisitos *(obrigatório)*

### Requisitos Funcionais

- **FR-001**: O sistema DEVE fornecer o componente standalone `OsItemPecaModalComponent` para seleção e adição de peças do estoque à Ordem de Serviço.
- **FR-002**: O modal de peças DEVE conter busca/autocomplete conectada ao serviço `EstoqueService.pecas()` que filtre peças ativas a partir de caracteres digitados.
- **FR-003**: Ao selecionar uma peça no autocomplete, o componente DEVE pré-preencher a descrição do item (`codigo - descricao`) e o valor unitário (`precoUnitario` da peça).
- **FR-004**: O modal de peças DEVE exibir a quantidade disponível em estoque da peça selecionada e exigir quantidade inteira $\ge 1$.
- **FR-005**: O sistema DEVE fornecer o componente standalone `OsItemServicoModalComponent` para registro de serviços e mão de obra na Ordem de Serviço.
- **FR-006**: O modal de serviços DEVE conter campo de descrição livre do serviço (obrigatório, máximo 240 caracteres), quantidade (padrão 1, $\ge 1$) e valor unitário de mão de obra (obrigatório, $\ge 0$).
- **FR-007**: Ambos os modais DEVEM calcular e apresentar em tempo real o subtotal estimado da linha (`quantidade * valorUnitario`) formatado em moeda brasileira (`BRL`).
- **FR-008**: O sistema DEVE persistir a criação de itens através do método `OrdensService.addItem()` enviando requisição `POST /api/v1/OrdemServicoItens` conforme rota mapeada em `apiPaths.ordens.itens`.
- **FR-009**: O payload de envio para criação de itens DEVE obedecer ao formato canônico:
  - `ordemServicoId`: identificador numérico da OS;
  - `pecaId`: identificador da peça no estoque para peças, ou `null` para serviços;
  - `descricao`: texto descritivo do item;
  - `quantidade`: quantidade de unidades/horas;
  - `valorUnitario`: valor monetário unitário.
- **FR-010**: O componente `OsDetalheComponent` DEVE disponibilizar botões de ação "Adicionar Peça" e "Adicionar Serviço" no cabeçalho da seção "Itens da OS".
- **FR-011**: O componente `OsDetalheComponent` DEVE disponibilizar botão de exclusão de item por linha na tabela de itens.
- **FR-012**: A exclusão de qualquer item da OS DEVE solicitar confirmação prévia do usuário por meio do serviço centralizado `Confirmation.confirmDelete()`.
- **FR-013**: Confirmada a exclusão, o sistema DEVE acionar o método `OrdensService.deleteItem(itemId)` enviando requisição `DELETE /api/v1/OrdemServicoItens/{itemId}`.
- **FR-014**: Após adição ou exclusão bem-sucedida de um item, o componente DEVE atualizar a coleção de itens diretamente no signal `ordem` sem recarregar a página (`no full reload`).
- **FR-015**: O recálculo do total da OS (`totalItens`), do total geral e do saldo devedor (`saldoPendente`) DEVE ocorrer de forma estritamente reativa via Angular Signals (`computed`).
- **FR-016**: Em ordens com status terminal (`Concluida` ou `Cancelada`), os botões de adicionar peças, adicionar serviços e excluir itens DEVEM ficar ocultos ou desabilitados.
- **FR-017**: Em caso de falha de validação ou de comunicação com a API, o sistema DEVE apresentar notificação amigável via serviço `Toast`, preservando a consistência dos dados em tela.

---

### Entidades Principais

- **Item da Ordem de Serviço (`OrdemServicoItem` / `CreateOrdemServicoItemRequest`)**:
  - `id`: Identificador único do item;
  - `ordemServicoId`: Identificador da OS vinculada;
  - `pecaId`: Chave estrangeira para o estoque (nulo quando serviço);
  - `descricao`: Descrição legível da peça ou serviço;
  - `quantidade`: Unidades consumidas ou quantidade de serviço;
  - `valorUnitario`: Preço unitário praticado;
  - `total`: Valor computado da linha (`quantidade * valorUnitario`).
- **Peça de Estoque (`EstoquePeca`)**: Peça física gerenciada no módulo de estoque contendo `id`, `codigo`, `descricao`, `precoUnitario` e saldo em estoque.
- **Ordem de Serviço (`OrdemServico`)**: Entidade agregadora do atendimento mecânico que engloba a coleção de itens, status operacional e vínculos cadastrais.
- **Resumo Financeiro da OS**: Valores agregados derivados da OS: total dos itens, total pago e saldo pendente a quitar.

---

## Critérios de Sucesso *(obrigatório)*

### Resultados Mensuráveis

- **SC-001**: Em 100% dos envios válidos de adição de peças, o item é gravado na API e renderizado na tabela de itens em menos de 1 segundo em condições normais de rede.
- **SC-002**: Em 100% dos envios válidos de adição de serviços, o registro é persistido com `pecaId: null` e passa a constar na lista de itens instantaneamente.
- **SC-003**: 100% das exclusões de itens exigem confirmação explícita no diálogo seguro antes do envio de requisição à API.
- **SC-004**: Em 100% das adições ou exclusões concluídas com êxito, os valores consolidados de `totalItens` e `saldoPendente` são atualizados instantaneamente sem recarregar a rota da página (0 reloads).
- **SC-005**: 100% das ordens com status `Concluida` ou `Cancelada` impedem a inclusão ou remoção de itens na interface de usuário.
- **SC-006**: 100% das chamadas de API para itens utilizam o serviço centralizado `OrdensService` e o caminho canônico mapeado em `apiPaths.ordens.itens`, com zero URLs codificadas diretamente em componentes.
- **SC-007**: 100% dos testes unitários cobrem os cenários de autocomplete, submissão de peça, submissão de serviço, confirmação de delete e recálculo reativo dos totalizadores.

---

## Premissas

- O backend `oficina-motos-api` já dispõe do controller `OrdemServicoItensController` com as rotas `POST /api/v1/OrdemServicoItens` e `DELETE /api/v1/OrdemServicoItens/{id}`.
- O catálogo de peças do estoque pode ser consultado pelo frontend através de `EstoqueService.pecas()` no endpoint `/api/v1/EstoquePecas`.
- O componente `OsDetalheComponent` já possui estrutura para exibição de itens e computa reativamente `totalItens` e `saldoPendente` a partir do signal `ordem`.
- O pacote PrimeNG 21 já instalado oferece componentes de diálogo (`DialogModule`) e interface necessários para os modais, integrando-se ao design dark do sistema.
- A baixa de estoque física e lançamentos contábeis automáticos são tratados em histórias subsequentes específicas dos épicos de Estoque e Financeiro.
