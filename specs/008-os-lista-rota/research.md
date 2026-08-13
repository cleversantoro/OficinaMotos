# Pesquisa: Lista de Ordens e Rota Corrigida

## Decisão 1: Renomear a página atual para `OsListaComponent`

**Decisão**: Mover a página de `src/app/features/ordens-servico/pages/os-detalhe/` para `src/app/features/ordens-servico/pages/os-lista/`, mantendo os arquivos TypeScript, HTML e SCSS no novo diretório, e renomear a classe para `OsListaComponent`.

**Racional**: A implementação atual é uma lista de ordens com busca, paginação manual, carregamento, exclusão e diálogo de detalhe. O nome `OsDetalhe` não representa o fluxo principal da rota `/ordens` e conflita com a necessidade de uma rota individual `/ordens/:id`.

**Alternativas consideradas**:

- Manter `OsDetalhe` e apenas alterar a rota: rejeitado porque não atende ao critério de aceite do nome do componente.
- Criar uma nova lista e manter a pasta antiga: rejeitado porque preserva duplicação e deixa ambígua a responsabilidade do componente.

## Decisão 2: Usar o `DataTable` compartilhado para paginação e ações

**Decisão**: Substituir a tabela HTML e a paginação manual pelo componente `DataTable`, configurando colunas tipadas, paginação local, estado de carregamento e ações por linha.

**Racional**: A constituição exige a reutilização de `shared/ui/data-table/`. O componente já suporta colunas, formatação de data, ações, filtro global, responsividade, loading e paginator do PrimeNG.

**Alternativas consideradas**:

- Continuar com `slice`, `first` e `rows` no componente: rejeitado porque duplica uma capacidade compartilhada e mantém a implementação fora do padrão do projeto.
- Criar outro componente de tabela específico para OS: rejeitado porque aumenta a superfície de manutenção sem necessidade.

## Decisão 3: Paginação no frontend na primeira versão

**Decisão**: Manter a chamada existente `OrdensService.list()` sem alterar o contrato da API e aplicar paginação local pelo `DataTable`.

**Racional**: O endpoint atual é consumido como uma coleção e a US-008 não solicita mudança de contrato, parâmetros ou resposta paginada. A paginação local atende à aceitação enquanto evita uma alteração backend desnecessária.

**Alternativas consideradas**:

- Introduzir paginação server-side no endpoint: rejeitado por estar fora do escopo e exigir mudanças coordenadas em API, DTOs e documentação.
- Fazer múltiplas chamadas por página: rejeitado porque não existe contrato confirmado para parâmetros de paginação no serviço atual.

## Decisão 4: Centralizar navegação no `Router`

**Decisão**: O botão “Nova OS” deve chamar `router.navigate(['/ordens/novo'])`; a ação de cada linha deve chamar `router.navigate(['/ordens', row.id])`.

**Racional**: A navegação dinâmica precisa usar o identificador real da linha e não deve montar URLs HTTP. O `Router` preserva o guard pai e permite testar os destinos sem depender de URLs hardcoded de API.

**Alternativas consideradas**:

- Usar `window.location`: rejeitado porque quebra o roteamento SPA e dificulta testes.
- Usar links com string concatenada: rejeitado para a ação dinâmica; o array de segmentos evita erros de codificação e mantém a navegação tipada no contexto Angular.

## Decisão 5: Registrar destinos de criação e detalhe como rotas explícitas

**Decisão**: `app.routes.ts` deve registrar `/ordens`, `/ordens/novo` e `/ordens/:id` em ordem específica, usando `loadComponent` para lazy loading e preservando os guards do `MainLayout` e da área de ordens. Como as páginas de criação e detalhe não foram encontradas no frontend atual, a implementação deverá conectar esses destinos aos componentes existentes quando disponíveis ou criar o menor componente de destino previsto no escopo da área.

**Racional**: Atualmente só existe a página `os-detalhe`; não há componente encontrado para `/ordens/novo` nem para `/ordens/:id`. Sem rotas explícitas, os links da lista cairiam no wildcard ou em uma tela incorreta.

**Alternativas consideradas**:

- Apontar `/ordens/:id` para a mesma lista: rejeitado porque não entrega uma rota individual semântica.
- Deixar a rota dinâmica sem componente definido: rejeitado porque produz navegação quebrada e não é verificável.

## Decisão 6: Guard de perfil para consulta e criação

**Decisão**: Estender o modelo de sessão para transportar `permissions`, mapear a permissão canônica `ordens × visualizar` para consulta e `ordens × criar` para criação, e criar um guard funcional específico para a área de ordens. A matriz de papéis pode permanecer como fallback de compatibilidade, mas a autorização final deve consultar as permissões reais do usuário. O acesso autenticado sem a permissão exigida será redirecionado para `/dashboard`.

**Racional**: O `authGuard` atual confirma apenas autenticação, `CurrentUser` não transporta permissões e a matriz atual trabalha somente com papéis. A constituição exige autorização específica por módulo e ação; usar `visualizar`/`criar` como vocabulário canônico evita confundir autorização real com fallback por papel.

**Alternativas consideradas**:

- Confiar somente na visibilidade do botão: rejeitado porque a URL de criação continuaria acessível por navegação direta.
- Duplicar listas de perfis no componente: rejeitado porque diverge da matriz RBAC compartilhada.
- Usar somente papéis para representar permissões: rejeitado porque não atende ao requisito constitucional de permissão específica por módulo e ação.

## Decisão 7: Tratamento de acesso negado

**Decisão**: O guard redirecionará usuários autenticados sem a permissão necessária para `/dashboard`; o fluxo exibirá a mensagem padronizada de permissão negada pelo serviço de Toast, sem carregar a tela protegida.

**Racional**: O destino é determinístico, evita tela quebrada e segue o requisito constitucional de tratamento de erros 403 pelo frontend.

## Decisão 8: Critério mensurável de localização da ação

**Decisão**: Substituir a métrica de usabilidade de 95% por testes automatizados de visibilidade e navegação para perfis autorizados e não autorizados.

**Racional**: O projeto não possui infraestrutura ou amostra definida para teste de usabilidade. A nova métrica é reproduzível no Vitest e verifica diretamente o risco funcional relevante: acesso indevido ou ação ausente para perfis autorizados.

**Alternativas consideradas**:

- Manter a pesquisa com usuários sem método definido: rejeitado por não ser verificável.
- Remover totalmente o critério: rejeitado porque a autorização da criação precisa permanecer mensurável.

## Decisão 9: Tratar estado e tipos com o padrão atual, sem alterar a API

**Decisão**: Usar `OrdemServico` como tipo das linhas, corrigindo o modelo para incluir `veiculoId` quando necessário para manter alinhamento com o contrato atual, e manter a chamada do serviço sob `OrdensService`.

**Racional**: A página atual usa `any[]`, enquanto o modelo tipado já existe. A lista precisa de `id`, `clienteId`, `mecanicoId`, `status`, `dataAbertura` e `descricaoProblema`; tipar esses dados reduz erros nas ações e na navegação.

**Alternativas consideradas**:

- Manter `any[]`: rejeitado porque enfraquece o TypeScript strict e torna a ação por linha menos segura.
- Criar um segundo modelo local duplicado: rejeitado porque a entidade de resposta já está no núcleo compartilhado.

## Decisão 10: Testar comportamento com Vitest e RouterTestingHarness

**Decisão**: Adicionar testes unitários para sessão, helper, guard, carregamento, colunas/ações, navegação do botão, navegação por id e paginação; testar a configuração de rotas e adicionar E2E quando a infraestrutura estiver disponível. Caso ela não exista, registrar a limitação em `oficina-motos-web/docs/TESTES_E2E.md`.

**Racional**: O projeto usa o builder de testes Angular com Vitest e não possui testes existentes para a área de ordens. Os testes unitários devem cobrir diretamente os critérios de aceite sem depender de banco ou API real; o E2E deve cobrir autenticação/autorização e navegação quando houver runner configurado.

**Alternativas consideradas**:

- Validar apenas com `ng build`: rejeitado porque compilação não confirma navegação nem ações.
- Criar E2E como único teste: rejeitado porque seria mais lento e frágil para comportamentos que podem ser testados unitariamente.

## Incertezas resolvidas

- A aplicação é Angular 21 com componentes standalone, PrimeNG 21 e Vitest.
- O endpoint de ordens é centralizado em `apiPaths.ordens.base` e consumido por `OrdensService.list()`.
- Não há componente de detalhe ou formulário de OS localizado no frontend atual; o plano inclui a resolução explícita dos destinos de rota.
- A documentação antiga menciona `/ordens/nova`, mas a especificação vigente exige `/ordens/novo`; o contrato desta feature segue `/ordens/novo`.
- `CurrentUser` e `LoginResponse` não transportam permissões; a implementação deve alinhar o modelo de sessão com o contrato atual de autenticação antes do guard.
- `visualizar` e `criar` são os nomes canônicos das permissões de negócio; nomes internos em inglês não devem substituir o contrato documentado.
