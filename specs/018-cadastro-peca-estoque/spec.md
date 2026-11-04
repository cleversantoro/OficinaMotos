# Especificação de Funcionalidade: Formulário de Cadastro de Peça no Estoque e Badge de Estoque Crítico

**Feature Branch**: `018-cadastro-peca-estoque`

**Criado**: 2026-11-04

**Status**: Draft

**Entrada**: US-018 — Criar formulário de cadastro de peça no estoque
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-001
- **Critério de aceite**:
  - Rota `/estoque/novo` exibe o formulário de cadastro de peça no estoque (`EstoquePecaCadastroComponent`).
  - Campos: código/SKU, descrição da peça, fabricante (dropdown), categoria (dropdown), fornecedor principal (dropdown), localização física (dropdown), unidade de medida, status, preço unitário de venda, preço de custo no fornecedor, quantidade inicial, estoque mínimo, estoque máximo e observações.
  - Dropdowns dinâmicos para **Fabricante**, **Categoria** e **Fornecedor** (além de localização).
  - Na tela de listagem de estoque (`/estoque`), exibir badge visual de **Estoque Crítico** quando `quantidade < estoqueMinimo` (`qtd < qtd_minima`), além de habilitar a navegação do botão **"+ Nova peça"** para `/estoque/novo`.
- **Tasks**:
  - [ ] T-018.1 — Criar `EstoquePecaCadastroComponent`
  - [ ] T-018.2 — Registrar rota `/estoque/novo`
  - [ ] T-018.3 — Dropdowns de fabricante, categoria, fornecedor
  - [ ] T-018.4 — Badge de estoque crítico na lista quando `qtd < qtd_minima`

**Dependências**: US-001 (Autenticação e RBAC), US-017 (Fornecedores), [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md), [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md), [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md), `EstoqueService`, `FornecedoresService`, `EstoquePecasController` e `EstoquePecaFornecedoresController`.

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Cadastro Completo de Peça no Estoque (Prioridade: P1)

Como almoxarife, comprador ou administrador da oficina, quero acessar a rota `/estoque/novo` para cadastrar uma nova peça ou insumo no catálogo de estoque, informando código SKU, descrição, preço de venda, quantidades inicial/mínima/máxima e classificações, para que a peça possa ser controlada no inventário e utilizada nas Ordens de Serviço (`US-011`).

**Por que esta prioridade**: O catálogo de peças é essencial para o lançamento de itens nas Ordens de Serviço e para o controle de reposição de estoque. Atualmente, o botão "+ Nova peça" em `/estoque` não possui rota nem componente associado.

**Teste independente**: Acessar `/estoque/novo`, preencher código (`PEC-010`), descrição ("Filtro de Óleo Honda CB 500"), selecionar fabricante, categoria e fornecedor nos dropdowns, definir preço unitário (`45.90`), quantidade inicial (`10`), estoque mínimo (`5`), estoque máximo (`50`), clicar em "Salvar Peça" e verificar que `POST /api/v1/EstoquePecas` (e `POST /api/v1/EstoquePecaFornecedores` quando fornecedor selecionado) cria o registro, exibe Toast de sucesso e redireciona para `/estoque`.

**Cenários de aceitação**:

1. **Dado** que o usuário autenticado acessa `/estoque/novo`, **quando** a página carregar, **então** o formulário de cadastro deve ser exibido em cards temáticos: Identificação da Peça (SKU, Descrição, Unidade, Status), Classificação & Fornecedor (Categoria, Fabricante, Fornecedor, Localização), Precificação & Níveis de Estoque (Preço Unitário, Quantidade Inicial, Estoque Mínimo, Estoque Máximo) e Observações.
2. **Dado** que o usuário preenche todos os campos obrigatórios com valores válidos, **quando** clicar em "Salvar Peça", **então** o sistema deve disparar `POST /api/v1/EstoquePecas` e, caso um fornecedor tenha sido selecionado no dropdown, vincular via `POST /api/v1/EstoquePecaFornecedores`.
3. **Dado** que a API confirma o cadastro, **quando** finalizado, **então** o sistema deve exibir Toast de sucesso (`toast.success`) e redirecionar para `/estoque`.
4. **Dado** que o formulário possui controles obrigatórios vazios ou valores negativos inválidos, **quando** o usuário tentar salvar, **então** os campos inválidos devem ser destacados com mensagens claras sem chamar a API.

---

### História de Usuário 2 — Dropdowns Dinâmicos de Fabricante, Categoria e Fornecedor (Prioridade: P1)

Como responsável pelo estoque, quero selecionar o **Fabricante**, a **Categoria** e o **Fornecedor** da peça em dropdowns alimentados dinamicamente pela API, garantindo a padronização do catálogo e a rastreabilidade de quem fornece cada item.

**Por que esta prioridade**: Evita digitação livre inconsistente e conecta diretamente a peça às tabelas de domínio (`est_fabricantes`, `est_categorias` e `for_fornecedores`).

**Teste independente**: Abrir `/estoque/novo` e verificar que os dropdowns de Fabricante (`EstoqueService.fabricantes()`), Categoria (`EstoqueService.categorias()`), Localização (`EstoqueService.localizacoes()`) e Fornecedor (`FornecedoresService.list()`) são populados automaticamente na inicialização e permitem seleção.

**Cenários de aceitação**:

1. **Dado** a inicialização do `EstoquePecaCadastroComponent`, **quando** `ngOnInit` executar, **então** o componente deve buscar simultaneamente fabricantes, categorias, localizações e fornecedores ativos.
2. **Dado** que o usuário seleciona um Fabricante e uma Categoria nos respectivos dropdowns, **quando** submeter o formulário, **então** `fabricanteId` e `categoriaId` numéricos devem ser enviados no payload `CreateEstoquePecaDTO`.
3. **Dado** que o usuário seleciona um Fornecedor no dropdown, **quando** a peça for criada com sucesso, **então** o sistema deve registrar o vínculo em `EstoquePecaFornecedores` com `pecaId` e `fornecedorId` (e opcionalmente o preço de custo/prazo).

---

### História de Usuário 3 — Badge de Estoque Crítico na Listagem quando `qtd < qtd_minima` (Prioridade: P1)

Como gestor de estoque ou comprador, quero visualizar na tabela de estoque (`/estoque`) um **badge de estoque crítico** sempre que a quantidade atual da peça for estritamente menor que a quantidade mínima configurada (`quantidade < estoqueMinimo`), para identificar imediatamente os itens que precisam de reposição urgente.

**Por que esta prioridade**: Previne ruptura de estoque na oficina e atende diretamente à task `T-018.4`.

**Teste independente**: Na listagem `/estoque`, carregar uma peça com `quantidade: 2` e `estoqueMinimo: 5` (`2 < 5`) e outra com `quantidade: 5` e `estoqueMinimo: 5` (`5 < 5` é falso). Verificar que a primeira exibe o badge `.badge-critico` ("Estoque crítico" / "Crítico") e destaque visual de alerta, enquanto a segunda exibe estoque normal.

**Cenários de aceitação**:

1. **Dado** um item na listagem `EstoqueListaComponent` com `item.quantidade < item.estoqueMinimo`, **quando** a linha for renderizada, **então** o método `isCritico(item)` deve retornar `true` e exibir um badge de alerta visual `.badge-critico` com o texto `"Crítico"`.
2. **Dado** um item com `item.quantidade >= item.estoqueMinimo`, **quando** a linha for renderizada, **então** `isCritico(item)` deve retornar `false` e o badge de estoque crítico não deve ser exibido.
3. **Dado** os cards de estatísticas no topo de `/estoque`, **quando** calculados, **então** o contador de `"Estoque crítico"` deve contabilizar exatamente os itens onde `quantidade < estoqueMinimo`.

---

### História de Usuário 4 — Registro da Rota `/estoque/novo` e Botão na Listagem (Prioridade: P1)

Como usuário do sistema, quero clicar no botão **"+ Nova peça"** na tela `/estoque` e ser direcionado para `/estoque/novo`.

**Por que esta prioridade**: Integra a listagem ao formulário de criação com controle de acesso (`authGuard`).

**Teste independente**: Em `/estoque`, clicar no botão "+ Nova peça" e verificar a navegação para `/estoque/novo`. No formulário, clicar em "Cancelar" e verificar o retorno para `/estoque`.

**Cenários de aceitação**:

1. **Dado** o arquivo `src/app/app.routes.ts`, **quando** inspecionado, **então** deve conter `{ path: 'estoque/novo', component: EstoquePecaCadastroComponent }` sob o `MainLayout` (`canActivate: [authGuard]`).
2. **Dado** a tela `/estoque`, **quando** o usuário clicar no botão `"+ Nova peça"`, **então** a aplicação deve navegar via `routerLink="/estoque/novo"`.

---

## Casos de Borda

1. **Comparação estrita `qtd < qtd_minima` (`T-018.4`)**: Quando `quantidade === estoqueMinimo`, a peça está no limite mínimo, mas ainda não abaixo dele; portanto, `isCritico(item)` deve avaliar estritamente `item.quantidade < item.estoqueMinimo`.
2. **Estoque máximo menor que o estoque mínimo**: Caso o usuário informe `estoqueMaximo > 0` e `estoqueMaximo < estoqueMinimo`, o formulário deve alertar/validar ou garantir consistência nos valores numéricos (`Validators.min(0)`).
3. **Cadastro sem fornecedor selecionado**: O vínculo de fornecedor é opcional no momento do cadastro da peça; se `fornecedorId` for `null`, apenas `POST /api/v1/EstoquePecas` é executado.
4. **Ordem de rotas em `app.routes.ts`**: A rota `estoque/novo` deve ser posicionada logo após `estoque` (e antes de qualquer futura rota `estoque/:id` prevista na `US-019`).

---

## Requisitos Funcionais (FR)

- **FR-001**: Criar o componente standalone `EstoquePecaCadastroComponent` em `src/app/features/estoque/pages/estoque-peca-cadastro/estoque-peca-cadastro.ts` (com `.html` e `.scss`).
- **FR-002**: Utilizar `ChangeDetectionStrategy.OnPush`, Reactive Forms (`FormBuilder`) e Angular Signals (`loading`, `submitting`, `fabricantes`, `categorias`, `localizacoes`, `fornecedores`).
- **FR-003**: Registrar a rota `{ path: 'estoque/novo', component: EstoquePecaCadastroComponent }` em `src/app/app.routes.ts` sob o `MainLayout` (`authGuard`).
- **FR-004**: O formulário reativo deve conter os controles:
  - `codigo`: obrigatório, max 40 caracteres, sugestão automática (`PEC-XXX`);
  - `descricao`: obrigatória, mínimo 3 e máximo 200 caracteres;
  - `unidade`: obrigatória, opções `"UN"`, `"PAR"`, `"KIT"`, `"LT"`, `"MT"`, `"KG"` (default: `"UN"`);
  - `status`: obrigatório, opções `"Ativo"`, `"Inativo"` (default: `"Ativo"`);
  - `categoriaId`: opcional (`number | null`), selecionado via dropdown de categorias;
  - `fabricanteId`: opcional (`number | null`), selecionado via dropdown de fabricantes;
  - `fornecedorId`: opcional (`number | null`), selecionado via dropdown de fornecedores;
  - `localizacaoId`: opcional (`number | null`), selecionado via dropdown de localizações;
  - `precoUnitario`: obrigatório, numérico (`Validators.required`, `Validators.min(0.01)`);
  - `precoFornecedor`: opcional, numérico (`Validators.min(0)`);
  - `prazoEntregaFornecedor`: opcional, numérico (`Validators.min(0)`);
  - `quantidade`: obrigatória, inteiro (`Validators.required`, `Validators.min(0)`), default `0`;
  - `estoqueMinimo`: obrigatório, inteiro (`Validators.required`, `Validators.min(0)`), default `5`;
  - `estoqueMaximo`: obrigatório, inteiro (`Validators.required`, `Validators.min(0)`), default `50`;
  - `observacoes`: opcional, max 500 caracteres.
- **FR-005**: Adicionar método `vincularFornecedor(body: any)` em `EstoqueService` (`POST /api/v1/EstoquePecaFornecedores` usando `apiPaths.estoque.fornecedores`).
- **FR-006**: Carregar simultaneamente via `forkJoin` na inicialização: `EstoqueService.categorias()`, `EstoqueService.fabricantes()`, `EstoqueService.localizacoes()`, `FornecedoresService.list()` e `EstoqueService.pecas()` (para sugestão de código `PEC-XXX`).
- **FR-007**: Ao submeter o formulário válido, chamar `EstoqueService.criarPeca(payload)` e, caso `fornecedorId` esteja preenchido, encadear `EstoqueService.vincularFornecedor()`, exibir `toast.success` e navegar para `/estoque`.
- **FR-008**: Em `EstoqueLista` (`estoque-lista.ts`, `estoque-lista.html`, `estoque-lista.scss`):
  - Atualizar `isCritico(item)` para `item.quantidade < item.estoqueMinimo`;
  - Renderizar `<span class="badge-critico">Crítico</span>` na coluna de estoque quando `isCritico(item)` for `true`;
  - Adicionar `RouterLink` e `routerLink="/estoque/novo"` no botão `"+ Nova peça"`.
- **FR-009**: Criar testes unitários em `estoque-peca-cadastro.spec.ts` e `estoque-lista.spec.ts` cobrindo todos os critérios de aceite.

---

## Requisitos Não Funcionais (NFR)

- **NFR-001 (Arquitetura)**: Componente 100% Standalone Angular 21, `ChangeDetectionStrategy.OnPush`, Signals e Reactive Forms.
- **NFR-002 (Design System)**: Tema escuro (Dark Mode `#0f172a`, `#1e293b`, `#334155`, `#f8fafc`, `#f97316`, alerta crítico em vermelho `#ef4444`).
- **NFR-003 (Sem Alteração de Banco de Dados)**: Não há mudanças no banco MySQL 8.0; todos os endpoints e tabelas já estão disponíveis.

---

## Critérios de Sucesso Mensuráveis (SC)

- **SC-001**: Rota `/estoque/novo` registrada e acessível a partir do botão `"+ Nova peça"` em `/estoque`.
- **SC-002**: Dropdowns de Fabricante, Categoria e Fornecedor carregam opções da API e persistem corretamente os vínculos.
- **SC-003**: Lista de estoque exibe o badge `.badge-critico` sempre que `quantidade < estoqueMinimo`.
- **SC-004**: 100% dos testes unitários do frontend passando no Vitest (`npm test -- --no-watch`).
- **SC-005**: Build de produção Angular (`npm run build`) concluído sem erros.
- **SC-006**: Backlog em [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) atualizado.
