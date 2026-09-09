# Pesquisa Técnica: Página de Detalhe Real da Ordem de Serviço

## Decisão 1: Arquitetura do Componente com Signals e OnPush

**Decisão**: Estruturar `OsDetalheComponent` como componente standalone, utilizando `ChangeDetectionStrategy.OnPush` e Signals para gerenciamento de estado (`ordem`, `cliente`, `veiculo`, `mecanico`, `loading`, `error`, `submittingStatus`, `statusSelecionado`).

**Racional**: Segue as diretrizes do Angular 21 e o padrão adotado na aplicação (ex.: `OsCadastroComponent` e `OsListaComponent`). Signals fornecem reatividade granular, eliminam verificações de ciclo de vida desnecessárias e tornam o cálculo das transições válidas natural através de `computed()`.

**Alternativas consideradas**:
- Utilizar RxJS com streams e pipe `async` no template: rejeitado porque o padrão atual da base prioriza Signals para estado de componentes.
- Utilizar ChangeDetection padrão (Default): rejeitado porque OnPush oferece melhor desempenho e previsibilidade.

## Decisão 2: Resolução de Dados Enriquecidos de Cliente, Veículo e Mecânico

**Decisão**: O endpoint `GET /api/v1/OrdemServicos/{id}` retorna os dados da OS com `clienteId`, `veiculoId`, `mecanicoId` e as coleções de sub-recursos (`itens`, `observacoes`, `pagamentos`). O componente resolverá dados complementares legíveis:
- Para o Cliente: consulta `ClientesService.get(clienteId)` para obter nome completo/razão social e dados de contato;
- Para o Veículo: consulta `VeiculosService.get(veiculoId)` para obter placa, modelo, marca e cor;
- Para o Mecânico: consulta `MecanicosService.get(mecanicoId)` ou busca na lista de mecânicos para exibir o nome do profissional.
Caso alguma consulta complementar falhe ou atrase, a tela exibe fallbacks limpos (ex.: `Cliente #ID`, `Veículo #ID`) sem quebrar a renderização da OS.

**Racional**: Garante a conformidade com o critério de aceite da US-010 ("Card Dados Gerais com cliente e veículo") de forma legível e sem alterar contratos da API.

**Alternativas consideradas**:
- Exigir alteração no backend para criar um endpoint `GET /api/v1/OrdemServicos/{id}/detalhe-expandido`: rejeitado para manter a independência do frontend e evitar sobrecarga de novas APIs onde as existentes já atendem perfeitamente.
- Exibir apenas os IDs numéricos: rejeitado expressamente pelo critério de aceite e requisitos de usabilidade.

## Decisão 3: Matriz Estrita de Transições Válidas de Status

**Decisão**: Implementar no componente (e exportar como constante/helper compartilhável) o mapa determinístico de transições permitidas a partir do enum `OrdemServicoStatus`:

```typescript
export const OS_STATUS_TRANSITIONS: Record<string, string[]> = {
  Aberta: ['EmAndamento', 'Cancelada'],
  EmAndamento: ['AguardandoPeca', 'Concluida', 'Cancelada'],
  AguardandoPeca: ['EmAndamento', 'Cancelada'],
  Concluida: [],
  Cancelada: [],
};
```

A lista de opções para o dropdown de transição será calculada via `computed(() => ...)` baseado no status atual da ordem carregada. Quando a ordem estiver em estado terminal (`Concluida` ou `Cancelada`), o controle de transição ficará desabilitado e indicará visualmente que o ciclo foi concluído.

**Racional**: Garante consistência com as regras de negócio definidas na US-006 e impede que ordens transitem de forma anômala (ex.: de Aberta direto para Concluída sem execução, ou reabrir ordem concluída sem fluxo de garantia).

**Alternativas consideradas**:
- Exibir todos os 5 status no dropdown e deixar o backend validar: rejeitado porque gera frustração ao usuário com erros 400 tardios e viola o critério de aceite "Botão de alterar status com transições válidas".

## Decisão 4: Atualização de Status via `OrdensService.update`

**Decisão**: Ao acionar a alteração de status:
1. O usuário seleciona o novo status no dropdown e clica no botão "Alterar Status";
2. O sistema ativa o signal `submittingStatus.set(true)`;
3. Envia a requisição via `OrdensService.update(ordem.id, payload)` mantendo os dados da OS com o novo status;
4. Se o novo status for `Concluida` e não houver `dataConclusao`, atribui a data atual no payload;
5. Ao obter sucesso: atualiza o signal da `ordem`, reseta a seleção pendente, emite `Toast.success` e desbloqueia os controles;
6. Em caso de erro: mantém o status anterior, emite `Toast.error` e desbloqueia os controles.

**Racional**: Reutiliza o endpoint existente `PUT /api/v1/OrdemServicos/{id}`, atualiza a UI reativamente sem requisição de página inteira e previne duplicações de requisição.

**Alternativas consideradas**:
- Endpoint específico `PATCH /ordens/:id/status`: o backend atual não possui este endpoint, portanto o `PUT` existente atende com segurança.

## Decisão 5: Totalizadores e Cálculos Financeiros

**Decisão**: Implementar signals computados para:
- `totalItens`: soma de `item.total` (ou `item.quantidade * item.valorUnitario`);
- `totalPago`: soma dos registros em `pagamentos`;
- `saldoPendente`: `totalItens - totalPago`.
Apresentar valores monetários utilizando o pipe padrão `currency: 'BRL':'symbol':'1.2-2':'pt-BR'` ou formatação local equivalente.

**Racional**: Traz visibilidade imediata para a oficina e cliente sobre o valor orçado/executado vs valor quitado.

## Decisão 6: Apresentação de Itens, Observações e Pagamentos com Estados Vazios

**Decisão**:
- **Itens da OS**: Tabela organizada com colunas: Descrição, Quantidade, Valor Unitário, Subtotal. Rodapé com totalizador. Estado vazio: banner/mensagem explicativa caso não haja itens.
- **Observações**: Lista de cards/timeline com texto da observação, autor e data formatada. Estado vazio quando não houver notas.
- **Pagamentos**: Tabela com Valor, Método, Status e Data. Bloco resumo com total pago e saldo. Estado vazio quando não houver pagamentos.

**Racional**: Proporciona uma visualização limpa e profissional, sem crashes ou espaçamentos em branco órfãos caso a OS seja recém-aberta.

## Incertezas Resolvidas

- O componente `OsDetalheComponent` já possui a rota `/ordens/:id` mapeada em `app.routes.ts`;
- As entidades filhas (`itens`, `observacoes`, `pagamentos`) já são retornadas pelo `GetByIdAsync` do repositório da API;
- Não há necessidade de alterações estruturais no backend;
- As transições de status são puramente determinísticas com base no enum canônico `OrdemServicoStatus`.
