# Especificação de Funcionalidade: Página de Detalhe Real da Ordem de Serviço

**Feature Branch**: `010-os-detalhe`

**Criado**: 2026-09-08

**Status**: Draft

**Entrada**: US-010 — Criar página de detalhe real da OS

**Dependências**: US-006 — Criar enum OrdemServicoStatus; US-009 — Criar formulário de nova Ordem de Serviço

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Visualização Completa da Ordem de Serviço (Prioridade: P1)

Como usuário autorizado da oficina, quero acessar `/ordens/:id` e visualizar o detalhe completo da OS com card de dados gerais (cliente e veículo), para obter uma visão clara, unificada e confiável do atendimento.

**Por que esta prioridade**: O detalhe da OS é a tela central de consulta operacional da oficina. Usuários que chegam a partir da listagem de ordens ou do redirecionamento após criação de uma nova OS precisam enxergar todas as informações do atendimento de forma legível e sem dados técnicos brutos (como IDs soltos).

**Teste independente**: Acessar `/ordens/:id` com uma ordem válida e confirmar que o card de Dados Gerais exibe identificador da OS, nome e contato do cliente, veículo (placa e modelo), mecânico responsável, datas de abertura/conclusão e descrição do problema.

**Cenários de aceitação**:

1. **Dado** que o usuário possui a permissão `ordens × visualizar`, **quando** acessar `/ordens/:id` com um ID existente, **então** o componente `OsDetalheComponent` exibirá o card "Dados Gerais" contendo número da OS, dados legíveis do cliente, dados do veículo, mecânico, datas e descrição.
2. **Dado** que o parâmetro `:id` na URL não é um número inteiro positivo, **quando** o usuário carregar a rota, **então** o sistema exibirá uma mensagem de erro indicando identificador inválido e não disparará requisições ao servidor.
3. **Dado** que o identificador da OS não existe no sistema (retorno 404), **quando** o carregamento for concluído, **então** o sistema apresentará um estado de erro informando que a ordem de serviço não foi encontrada, com link para voltar à lista (`/ordens`).
4. **Dado** que os dados da OS estão sendo carregados, **quando** a requisição estiver pendente, **então** a tela exibirá um estado de carregamento acessível e desabilitará interações.

---

### História de Usuário 2 — Controle de Status com Transições Válidas (Prioridade: P1)

Como atendente, mecânico ou gerente autorizado, quero visualizar o status atual da OS e selecionar apenas transições de status válidas para o ciclo de vida da ordem, para garantir a integridade do processo de atendimento da oficina.

**Por que esta prioridade**: Alterações de status fora da ordem cronológica ou permitida causam inconsistências operacionais e contábeis graves (por exemplo, concluir uma ordem cancelada ou reabrir ordem concluída sem fluxo apropriado).

**Teste independente**: Carregar ordens em diferentes estados e verificar que o dropdown de transição restringe estritamente as opções conforme a matriz de transições válidas, persistindo a mudança com sucesso.

**Matriz de transições permitidas**:
- De `Aberta` (1) para: `EmAndamento` (2), `Cancelada` (5)
- De `EmAndamento` (2) para: `AguardandoPeca` (3), `Concluida` (4), `Cancelada` (5)
- De `AguardandoPeca` (3) para: `EmAndamento` (2), `Cancelada` (5)
- De `Concluida` (4): Nenhuma transição permitida (estado terminal)
- De `Cancelada` (5): Nenhuma transição permitida (estado terminal)

**Cenários de aceitação**:

1. **Dado** que uma OS possui status `Aberta`, **quando** o usuário abrir o controle de alteração de status, **então** somente as opções `EmAndamento` e `Cancelada` estarão disponíveis para seleção.
2. **Dado** que uma OS possui status `EmAndamento`, **quando** o usuário abrir o controle de alteração de status, **então** somente as opções `AguardandoPeca`, `Concluida` e `Cancelada` estarão disponíveis.
3. **Dado** que uma OS possui status `AguardandoPeca`, **quando** o usuário abrir o controle de alteração de status, **então** somente as opções `EmAndamento` e `Cancelada` estarão disponíveis.
4. **Dado** que uma OS possui status `Concluida` ou `Cancelada`, **quando** a tela de detalhe for exibida, **então** o controle de status indicará que a OS está em estado final e não permitirá selecionar nenhuma nova transição.
5. **Dado** que uma transição válida foi selecionada, **quando** o usuário confirmar a alteração, **então** o sistema atualizará o status via API, refletirá o novo status na tela, atualizará o badge visual e exibirá notificação de sucesso.
6. **Dado** que a atualização de status falhou na comunicação com a API, **quando** o erro for retornado, **então** o sistema manterá o status anterior na tela e exibirá uma mensagem de erro explicativa ao usuário.

---

### História de Usuário 3 — Seção "Itens da OS" (Prioridade: P1)

Como usuário da oficina, quero visualizar a lista de peças e serviços incluídos na OS com quantidades, valores unitários e totais, para acompanhar o escopo do serviço e o valor apurado.

**Por que esta prioridade**: Os itens definem o custo de peças e mão de obra do atendimento, sendo imprescindíveis para o cliente e para a oficina.

**Teste independente**: Abrir uma OS com itens e validar a apresentação da tabela com colunas de descrição, quantidade, valor unitário, subtotal por linha e o somatório geral dos itens.

**Cenários de aceitação**:

1. **Dado** que a OS possui um ou mais itens associados, **quando** o usuário visualizar a seção "Itens da OS", **então** uma tabela exibirá cada item com descrição, quantidade, valor unitário formatado em moeda brasileira e valor total da linha.
2. **Dado** que a tabela de itens é exibida, **quando** houver múltiplos itens, **então** o rodapé ou cabeçalho da seção apresentará o valor consolidado da soma dos itens.
3. **Dado** que a OS não possui nenhum item registrado, **quando** a seção for renderizada, **então** uma mensagem informativa de estado vazio ("Nenhum item adicionado a esta OS") será exibida.

---

### História de Usuário 4 — Seção "Observações" da OS (Prioridade: P2)

Como usuário da oficina, quero consultar as anotações e observações registradas para a OS em ordem cronológica, para entender o histórico de ocorrências e instruções especiais daquele atendimento.

**Por que esta prioridade**: Observações garantem a comunicação interna entre recepcionistas e mecânicos (ex.: relatos do cliente, peças fornecidas pelo cliente, autorizações verbais).

**Teste independente**: Abrir uma OS que possua observações e conferir a listagem das notas com autor, data (se disponíveis) e texto descritivo.

**Cenários de aceitação**:

1. **Dado** que a OS possui observações cadastradas, **quando** a seção "Observações" for exibida, **então** cada observação será apresentada em formato legível, contendo texto da observação e dados do autor/data caso registrados.
2. **Dado** que a OS não possui nenhuma observação, **quando** a seção for renderizada, **então** uma mensagem informativa amigável ("Nenhuma observação registrada para esta OS") será exibida.

---

### História de Usuário 5 — Seção "Pagamentos" da OS (Prioridade: P2)

Como usuário autorizado da oficina ou do setor financeiro, quero visualizar os pagamentos e quitações vinculados à OS, para verificar se o atendimento foi quitado ou se há saldo devedor.

**Por que esta prioridade**: A visualização financeira encerra o ciclo de atendimento e assegura o controle de recebimentos antes da liberação do veículo.

**Teste independente**: Abrir uma OS com registros de pagamento e confirmar que os pagamentos são listados com valor, método, status e data, além da exibição do valor total pago.

**Cenários de aceitação**:

1. **Dado** que a OS possui registros de pagamento, **quando** a seção "Pagamentos" for exibida, **então** uma tabela ou lista apresentará cada pagamento com valor formatado, método (ex.: PIX, Cartão, Dinheiro), status do pagamento e data.
2. **Dado** que pagamentos foram listados, **quando** os valores forem calculados, **então** a seção apresentará o valor total pago até o momento.
3. **Dado** que a OS não possui registros de pagamento, **quando** a seção for renderizada, **então** uma mensagem informativa amigável ("Nenhum pagamento registrado para esta OS") será exibida.

---

### Casos de Borda

- O parâmetro `:id` da URL não é numérico ou é menor/igual a zero (`/ordens/abc`, `/ordens/-1`).
- A ordem de serviço solicitada foi excluída ou não existe (HTTP 404).
- Falha de conexão de rede ou indisponibilidade da API durante a consulta inicial.
- O cliente vinculado à OS foi alterado ou não possui dados de contato cadastrados (exibir nome ou identificador padrão sem quebrar a tela).
- O veículo vinculado à OS não possui placa ou marca cadastrada (exibir fallback elegante como "Veículo sem placa cadastrada").
- A OS não possui mecânico designado (`mecanicoId = 0` ou sem mecânico vinculado).
- A OS não possui itens, observações nem pagamentos (a tela deve exibir todos os estados vazios harmoniosamente sem desconfigurar o layout).
- O usuário tenta disparar múltiplos cliques no botão de alterar status durante a requisição (o botão deve ser bloqueado e exibir indicador de carregamento).
- O usuário não possui a permissão `ordens × visualizar` (o `ordensPermissionGuard` bloqueia a navegação, emite notificação e redireciona para `/dashboard`).
- Visualização em telas estreitas / mobile: cards e tabelas de itens/pagamentos devem ser responsivos, com rolagem horizontal ou quebra de linhas limpa.

## Requisitos *(obrigatório)*

### Requisitos Funcionais

- **FR-001**: O sistema DEVE manter a rota protegida `/ordens/:id` acessível para usuários autenticados com a permissão canônica `ordens × visualizar`.
- **FR-002**: A rota `/ordens/:id` DEVE renderizar o componente standalone `OsDetalheComponent`.
- **FR-003**: O componente DEVE exibir o card **"Dados Gerais"** contendo:
  - Número de identificação da OS (#ID);
  - Status atual em formato de tag/badge visual com estilo semântico diferenciado por estado;
  - Identificação do Cliente (nome/razão social, CPF/CNPJ e contato telefônico quando disponíveis);
  - Identificação do Veículo (placa, marca/modelo e cor quando disponíveis);
  - Mecânico responsável atribuído ao atendimento;
  - Data e hora de abertura do atendimento formatada no padrão local (`dd/MM/yyyy HH:mm`);
  - Data e hora de conclusão (quando a OS estiver concluída);
  - Descrição do problema relatado.
- **FR-004**: O sistema DEVE resolver as informações de cliente e veículo através dos serviços existentes (`ClientesService` e `VeiculosService`) ou das entidades expandidas pela API, garantindo que o usuário não veja apenas IDs numéricos puros.
- **FR-005**: O componente DEVE fornecer o controle de **alteração de status**, composto por:
  - Exibição destacada do status atual;
  - Dropdown ou seletor de transições que ofereça **apenas transições válidas** segundo o ciclo de vida definido na matriz operacional:
    - Status `Aberta`: opções para transitar para `EmAndamento` e `Cancelada`;
    - Status `EmAndamento`: opções para transitar para `AguardandoPeca`, `Concluida` e `Cancelada`;
    - Status `AguardandoPeca`: opções para transitar para `EmAndamento` e `Cancelada`;
    - Status `Concluida` e `Cancelada`: desabilitado, indicando estado final da OS;
  - Botão de confirmação da transição de status selecionada.
- **FR-006**: Ao confirmar uma transição de status, o sistema DEVE enviar a atualização via `OrdensService.update` com o novo status e os dados da OS, desabilitando o botão durante a submissão e exibindo feedback de sucesso via Toast.
- **FR-007**: Em caso de falha na atualização de status pela API, o sistema DEVE manter o status original na interface, restaurar os controles interativos e apresentar mensagem de erro clara ao usuário.
- **FR-008**: O componente DEVE exibir a seção **"Itens da OS"**, apresentando:
  - Tabela com descrição do item/peça, quantidade, valor unitário formatado em BRL (`R$ 0,00`) e total por linha;
  - Total consolidado da soma dos itens da OS;
  - Mensagem explicativa de estado vazio quando a OS não possuir itens vinculados.
- **FR-009**: O componente DEVE exibir a seção **"Observações"**, apresentando:
  - Lista de apontamentos e observações associadas à OS com seu respectivo texto e, quando disponíveis, autor e data;
  - Mensagem amigável de estado vazio quando não houver observações registradas.
- **FR-010**: O componente DEVE exibir a seção **"Pagamentos"**, apresentando:
  - Listagem dos pagamentos registrados com valor monetário, método de pagamento, status do pagamento e data;
  - Totalizador do valor já pago;
  - Mensagem amigável de estado vazio quando não houver pagamentos registrados.
- **FR-011**: O componente DEVE incluir ação de retorno/navegação permitindo voltar com facilidade para a lista geral de ordens (`/ordens`).
- **FR-012**: Todas as requisições HTTP DEVEM utilizar exclusivamente os serviços Angular centralizados (`OrdensService`, `ClientesService`, `VeiculosService`) e as rotas mapeadas em `apiPaths`, sem URLs embutidas diretamente no componente.
- **FR-013**: A página DEVE ser totalmente responsiva, adaptando-se a dispositivos móveis e desktops sem quebras de layout ou sobreposição de textos.

### Entidades Principais

- **Ordem de Serviço (`OrdemServico`)**: Registro central do atendimento na oficina mecânica, contendo status, datas, descrições, chaves estrangeiras e coleções filhas.
- **Status da OS (`OrdemServicoStatus`)**: Enumeração canônica dos estados (`Aberta = 1`, `EmAndamento = 2`, `AguardandoPeca = 3`, `Concluida = 4`, `Cancelada = 5`).
- **Item da OS (`OrdemServicoItem`)**: Registro de peça ou serviço consumido na ordem, com quantidade, preço unitário e subtotal.
- **Observação da OS (`OrdemServicoObservacao`)**: Apontamento textual interno relacionado ao atendimento.
- **Pagamento da OS (`OrdemServicoPagamento`)**: Registro financeiro de quitação com valor, status, método e data.
- **Cliente**: Proprietário da moto atendida pela oficina.
- **Veículo**: Motocicleta vinculada ao atendimento e de propriedade do cliente selecionado.

## Critérios de Sucesso *(obrigatório)*

### Resultados Mensuráveis

- **SC-001**: Em 100% dos acessos autorizados a `/ordens/:id` com identificador válido, a página carrega e exibe as cinco seções fundamentais (Dados Gerais, Status Atual, Itens, Observações, Pagamentos) em menos de 2 segundos em condições normais de operação.
- **SC-002**: Em 100% das ordens consultadas, o dropdown de alteração de status apresenta exclusivamente as transições autorizadas para o status vigente, com 0% de opções inválidas ofertadas.
- **SC-003**: 100% das ordens com status `Concluida` ou `Cancelada` impedem novas transições de status na interface.
- **SC-004**: Em 100% dos atendimentos com dados de cliente e veículo encontrados, a interface exibe nomes e identificadores amigáveis (nome do cliente e placa/modelo) no lugar de IDs brutos.
- **SC-005**: 100% das seções de Itens, Observações e Pagamentos sem registros renderizam estados vazios claros e instrucionais, sem travamentos de script ou erros de renderização.
- **SC-006**: 100% das transições de status confirmadas com sucesso pela API são refletidas instantaneamente na interface sem necessidade de recarregar manualmente a página inteira.
- **SC-007**: 100% dos usuários sem permissão `ordens × visualizar` são impedidos de carregar a tela e redirecionados para `/dashboard` pelo guard de segurança.

## Premissas

- A rota `/ordens/:id` já está declarada no arquivo `app.routes.ts` com proteção do `ordensPermissionGuard` (`ordensAction: 'visualizar'`).
- O endpoint backend `GET /api/v1/OrdemServicos/{id}` retorna o DTO completo incluindo as coleções de `Itens`, `Observacoes` e `Pagamentos`.
- A atualização de status utiliza o endpoint padrão `PUT /api/v1/OrdemServicos/{id}` via `OrdensService.update()`.
- Os serviços `ClientesService` e `VeiculosService` continuam disponíveis para consulta de detalhes adicionais de exibição quando a resposta direta da OS trouxer apenas as chaves relacionais `clienteId` e `veiculoId`.
- O enum canônico `OrdemServicoStatus` criado na US-006 (`Aberta`, `EmAndamento`, `AguardandoPeca`, `Concluida`, `Cancelada`) rege todas as transições de ciclo de vida.
- Não faz parte do escopo da US-010 o formulário de cadastro de novos itens, novas observações ou novos pagamentos, os quais pertencem a tarefas e histórias subsequentes do backlog.
