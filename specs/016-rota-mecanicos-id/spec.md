# Especificação de Funcionalidade: Registrar rota /mecanicos/:id e Link na Listagem

**Feature Branch**: `016-rota-mecanicos-id`

**Criado**: 2026-11-04

**Status**: Draft

**Entrada**: US-016 — Registrar rota `/mecanicos/:id`
- **Prioridade**: 🟡 Should | **Estimativa**: S | **Sprint**: 3 | **Depende**: US-001
- **Critério de aceite**:
  - Rota `/mecanicos/:id` exibe a página completa de detalhes do mecânico com layout dedicado, dados cadastrais, KPIs e abas informativas (especialidades, disponibilidades, certificações, experiências, documentos, endereços e contatos).
  - Tabela de mecânicos (`MecanicoListaComponent`) possui coluna de link ou nome clicável direcionando para a rota `/mecanicos/:id`.
  - Botão de retorno permitindo voltar com facilidade à listagem `/mecanicos`.
  - Tratamento resiliente para identificadores inválidos ou mecânicos não encontrados.
- **Tasks**:
  - [ ] T-016.1 — Adicionar `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` em `app.routes.ts`
  - [ ] T-016.2 — Adicionar coluna de link em `MecanicoListaComponent`

**Dependências**: US-001 (Autenticação e RBAC), [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md), [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md), [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md), `MecanicosService`, `MecanicosController` (`GET /api/v1/Mecanicos/{id}`).

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Acesso Direto e Visualização Completa de Mecânico pela Rota `/mecanicos/:id` (Prioridade: P1)

Como gerente ou atendente da oficina, quero acessar a rota `/mecanicos/:id` (seja por URL direta, bookmark ou navegação interna) para visualizar o perfil completo do profissional com todas as suas competências técnicas, horários de disponibilidade, certificações e histórico cadastral em uma página dedicada e responsiva.

**Por que esta prioridade**: Permitir acesso a uma URL persistente e compartilhável de cada mecânico é fundamental para a gestão da oficina, auditoria operacional e vínculo direto com Ordens de Serviço. A visualização isolada em página completa (alinhada a `/clientes/:id` e `/motos/:id`) oferece melhor ergonomia que modais em telas de trabalho diário.

**Teste independente**: Acessar diretamente no navegador a URL `/mecanicos/1` (ou qualquer ID válido). O sistema deve realizar a chamada `GET /api/v1/Mecanicos/1`, exibir os dados cadastrais do mecânico (nome, código, nível, status, valor/hora, carga horária), abas com especialidades, disponibilidades e certificações, sem depender de estado prévio em memória ou de modais.

**Cenários de aceitação**:

1. **Dado** que o usuário autenticado digita ou navega para `/mecanicos/1`, **quando** a rota for resolvida, **então** o componente de detalhes de mecânico deve ser carregado dentro do layout principal da aplicação (`MainLayout`).
2. **Dado** o carregamento da página, **quando** os dados estiverem sendo obtidos via `MecanicosService.get(1)`, **então** deve ser apresentado um indicador visual de carregamento (`loading`).
3. **Dado** o retorno com sucesso da API, **quando** os dados forem renderizados, **então** a tela deve exibir o cabeçalho com identificação do mecânico, card de perfil com métricas (valor/hora, carga horária semanal) e abas tabuladas para "Especialidades", "Disponibilidade", "Certificações", "Experiências", "Documentos", "Endereços" e "Contatos".
4. **Dado** que o usuário está na página de detalhes `/mecanicos/:id`, **quando** clicar no botão "Voltar", **então** o sistema deve navegar para `/mecanicos` preservando a experiência de navegação SPA sem reload total.

---

### História de Usuário 2 — Navegação a Partir da Tabela de Mecânicos via Link e Ações (Prioridade: P1)

Como usuário na tela de listagem de mecânicos (`/mecanicos`), quero clicar no nome do profissional ou no botão de visualização para ser redirecionado diretamente à rota `/mecanicos/:id`, facilitando a inspeção detalhada do colaborador.

**Por que esta prioridade**: Proporciona uma jornada fluida de consulta; a listagem é o ponto de partida mais comum dos usuários para consultar o histórico e detalhes técnicos de cada colaborador.

**Teste independente**: Acessar `/mecanicos`, localizar uma linha de mecânico cadastrado, clicar sobre o link do nome ou no botão de visualização ("Visualizar") e constatar que a URL do navegador é alterada para `/mecanicos/:id` e que a página de detalhes correspondente é exibida.

**Cenários de aceitação**:

1. **Dado** a tabela de mecânicos em `/mecanicos`, **quando** a lista for renderizada, **então** a coluna de "Nome" deve conter um link de navegação (`routerLink`) e a coluna de ações deve conter o botão de visualização com link/navegação para `/mecanicos/:id`.
2. **Dado** o clique com botão esquerdo ou comando de teclado em uma das âncoras de detalhes, **quando** acionado, **então** a aplicação deve navegar para `/mecanicos/:id` sem recarregar o navegador.
3. **Dado** que o usuário clica com o botão do meio ou "Abrir em nova aba" no link, **quando** a nova aba abrir, **então** a rota `/mecanicos/:id` deve carregar corretamente e de forma independente.
4. **Dado** uma linha com mecânico específico, **quando** o usuário clicar no botão de exclusão, **então** a navegação para `/mecanicos/:id` NÃO deve ser disparada (propagação de eventos isolada).

---

### História de Usuário 3 — Tratamento de Mecânico Não Encontrado ou ID Inválido (Prioridade: P2)

Como usuário do sistema, caso eu acesse uma rota `/mecanicos/:id` com um identificador inexistente ou formato incorreto, quero receber uma mensagem clara e amigável de orientação com um botão para retornar à lista de mecânicos, evitando telas em branco ou travamento da aplicação.

**Por que esta prioridade**: Assegura a robustez do sistema e evita frustração ou perda de contexto por parte do usuário em cenários de links quebrados ou IDs excluídos.

**Teste independente**: Acessar `/mecanicos/999999` ou `/mecanicos/invalido`. Verificar que o sistema captura o erro (404 ou formato inválido), não trava o console, exibe um painel de estado amigável ("Mecânico não encontrado") com botão "Voltar para Mecânicos" direcionando a `/mecanicos`.

**Cenários de aceitação**:

1. **Dado** que o usuário acessa `/mecanicos/:id` com um ID numérico que não existe no banco (HTTP 404), **quando** a API retornar erro, **então** o componente deve exibir um card informativo amigável informando que o colaborador não foi localizado.
2. **Dado** que o usuário digita um parâmetro alfanumérico não conversível (ex.: `/mecanicos/teste`), **quando** a página for inicializada, **então** o componente deve identificar o parâmetro inválido, sinalizar o erro e disponibilizar o botão de retorno seguro.
3. **Dado** o card de erro exibido na tela, **quando** o usuário clicar em "Voltar para Mecânicos", **então** o roteador deve conduzi-lo para a rota `/mecanicos`.

---

## Casos de Borda

1. **Acesso com ID não numérico** (ex.: `/mecanicos/abc`): O componente deve validar a conversão para número; se for `NaN` ou <= 0, deve exibir estado amigável de erro sem realizar chamadas desnecessárias à API.
2. **Ordem de precedência de rotas**: A rota `/mecanicos/novo` DEVE estar registrada antes de `/mecanicos/:id` em `app.routes.ts`, para evitar que a palavra "novo" seja tratada como o parâmetro `:id`.
3. **Mecânico sem relações filhas**: Mecânicos recém-cadastrados podem não possuir especialidades, disponibilidades, certificações, experiências, endereços ou contatos cadastrados. As abas devem exibir mensagens claras de estado vazio (ex.: "Nenhuma certificação cadastrada") sem disparar exceções de nulo.
4. **Mecânico com status inativo**: O badge de status e indicadores devem refletir com precisão a inatividade (estilo visual atenuado / badge cinza ou vermelho).
5. **Valor por hora nulo ou zerado**: Formatação monetária segura (`R$ 0,00` ou `—`) sem gerar falhas de execução no template.
6. **Atualização direta via F5 / Deep Linking**: O componente deve buscar as informações de forma autônoma pelo `ActivatedRoute` sem supor dados mantidos em memória de telas anteriores.

---

## Requisitos Funcionais (FR)

- **FR-001**: Adicionar a rota `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` em `src/app/app.routes.ts` sob a rota principal (`MainLayout`) com proteção `canActivate: [authGuard]`.
- **FR-002**: A rota estática `/mecanicos/novo` DEVE permanecer posicionada ESTRITAMENTE ANTES de `/mecanicos/:id` na lista de rotas filhas.
- **FR-003**: Adaptar o componente de detalhes de mecânico (`src/app/features/mecanicos/pages/mecanico-detalhe/mecanico-detalhe.ts` e `.html`) para suportar operação como página roteada autônoma:
  - Injetar `ActivatedRoute`, `Router` e `MecanicosService`;
  - Ler o parâmetro `id` via `route.snapshot.paramMap.get('id')` ou subscription de parâmetros;
  - Buscar o registro completo via `MecanicosService.get(id)`;
  - Gerenciar estados de `loading`, `error` e `mecanico` através de propriedades reativas / Angular Signals;
  - Exibir botão de retorno para `/mecanicos` no cabeçalho da página.
- **FR-004**: Manter compatibilidade de nomenclatura exportando `MecanicoDetalheComponent` (e mantendo alias ou equivalência com `MecanicoDetalhe`) em `mecanico-detalhe.ts`.
- **FR-005**: Na página de detalhes (`mecanico-detalhe.html`), renderizar layout de página estruturado contendo:
  - Header da página com breadcrumb ou botão "← Voltar para Mecânicos", nome do mecânico e nível;
  - Card de identificação e resumo com avatar, status (ativo/inativo), código funcional, data de admissão, CPF e KPIs de valor/hora e carga horária semanal;
  - Abas informativas completas (Especialidades, Disponibilidade, Certificações, Experiências, Documentos, Endereços e Contatos) com fallbacks claros para listas vazias.
- **FR-006**: Na tela de listagem de mecânicos (`src/app/features/mecanicos/pages/mecanico-lista/mecanico-lista.html` e `.ts`):
  - Incluir link de navegação na célula do nome do mecânico direcionando para `['/mecanicos', m.id]`;
  - Atualizar o botão de ação "Visualizar" para navegar para a rota `['/mecanicos', m.id]` via `routerLink` ou `Router.navigate`;
  - Preservar o isolamento do clique para não disparar ações indesejadas (como exclusão).
- **FR-007**: Assegurar fidelidade visual com o design system Dark Mode da aplicação (`#0f172a`, `#1e293b`, `#334155`, `#f8fafc`, destaques em laranja `#f97316`).
- **FR-008**: Implementar testes unitários automatizados cobrindo a resolução da rota, carregamento com sucesso, tratamento de parâmetros inválidos ou erro 404 e navegação a partir da lista.

---

## Requisitos Não Funcionais (NFR)

- **NFR-001 (Consistência Arquitetural)**: Componente 100% Standalone Angular 21, alinhado com as diretrizes de [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md) e [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md).
- **NFR-002 (Performance & UX)**: Renderização da página em menos de 250ms após recebimento dos dados da API, sem re-renderizações desnecessárias.
- **NFR-003 (Acessibilidade)**: Elementos de navegação semânticos (`<a>` com `routerLink`, `aria-label` descritivos nos botões de ação e títulos de abas).
- **NFR-004 (Sem alterações no Banco de Dados)**: A funcionalidade consome a API REST já existente (`GET /api/v1/Mecanicos/{id}`), sem necessidade de novas migrations ou scripts SQL.

---

## Entidades Envolvidas

- **Mecanico (`MecanicoResponseDTO`)**: Entidade central representando o colaborador técnico da oficina. Atributos principais exibidos: `id`, `codigo`, `nome`, `sobrenome`, `documentoPrincipal`, `dataAdmissao`, `status`, `nivel`, `valorHora`, `cargaHorariaSemanal`.
- **Especialidades (`MecanicoEspecialidade`)**: Competências técnicas associadas ao mecânico, indicando se é principal e nível de proficiência.
- **Disponibilidades (`MecanicoDisponibilidade`)**: Horários e dias da semana em que o mecânico atua na oficina.
- **Certificações, Experiências, Documentos, Endereços e Contatos**: Sub-entidades relacionais detalhadas nas abas da página.

---

## Critérios de Sucesso Mensuráveis (SC)

- **SC-001**: A rota `/mecanicos/:id` carrega o componente `MecanicoDetalheComponent` exibindo os dados completos do profissional para qualquer ID válido.
- **SC-002**: A navegação a partir de `/mecanicos` através do link no nome ou do botão "Visualizar" conduz para `/mecanicos/:id` em 1 clique.
- **SC-003**: O botão "Voltar" na página de detalhes retorna com precisão para a rota `/mecanicos`.
- **SC-004**: IDs inválidos ou inexistentes exibem mensagem tratada com opção de retorno seguro sem gerar erros não capturados no console.
- **SC-005**: 100% dos testes unitários do frontend executando com sucesso via Vitest (`npm test -- --no-watch`).
- **SC-006**: Build de produção Angular (`npm run build`) concluído com sucesso e sem avisos de tipos ou rotas quebradas.
- **SC-007**: Backlog do projeto em [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) atualizado com status concluído após implementação.

---

## Premissas

- O backend já disponibiliza o endpoint `GET /api/v1/Mecanicos/{id}` retornando o objeto `MecanicoResponseDTO` populado com todas as coleções dependentes.
- O serviço `MecanicosService.get(id)` já está implementado e pronto para consumo em `src/app/core/services/mecanicos.service.ts`.
- O controle de acesso e autenticação é gerenciado centralmente por `authGuard`.
- Nenhuma alteração no esquema do banco MySQL 8.0 é requerida para esta história.
