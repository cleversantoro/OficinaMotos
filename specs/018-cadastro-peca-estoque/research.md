# Pesquisa Técnica: Cadastro de Peça no Estoque e Badge de Estoque Crítico

**Feature**: `018-cadastro-peca-estoque` | **Data**: 2026-11-04
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

---

## 1. Contexto e Motivação

No módulo de Estoque (`features/estoque`), a tela de listagem (`EstoqueLista`) já está implementada em `/estoque`, porém o botão **"+ Nova peça"** ainda não possui navegação nem componente de cadastro vinculado (`/estoque/novo`). Além disso, a task `T-018.4` especifica a exibição de um badge explícito de **Estoque Crítico** na listagem quando `quantidade < estoqueMinimo` (`qtd < qtd_minima`).

A história **US-018** entrega:
- **T-018.1**: Criar `EstoquePecaCadastroComponent` (`estoque-peca-cadastro.ts`, `.html`, `.scss`).
- **T-018.2**: Registrar rota `/estoque/novo` em `app.routes.ts` e habilitar `routerLink="/estoque/novo"` na listagem.
- **T-018.3**: Dropdowns dinâmicos de **Fabricante**, **Categoria** e **Fornecedor** (além de localização).
- **T-018.4**: Badge de estoque crítico na lista (`EstoqueLista`) quando `quantidade < estoqueMinimo`.

---

## 2. Análise de Serviços e Modelos

### 2.1 `EstoqueService` (`src/app/core/services/estoque.service.ts`)
- Métodos já existentes:
  - `pecas()` -> `GET /api/v1/EstoquePecas`
  - `criarPeca(body)` -> `POST /api/v1/EstoquePecas`
  - `categorias()` -> `GET /api/v1/EstoqueCategorias`
  - `fabricantes()` -> `GET /api/v1/EstoqueFabricantes`
  - `localizacoes()` -> `GET /api/v1/EstoqueLocalizacoes`
  - `fornecedores(params)` -> `GET /api/v1/EstoquePecaFornecedores`
- Adicionaremos `vincularFornecedor(body: any)` apontando para `this.api.create(apiPaths.estoque.fornecedores, body)` (`POST /api/v1/EstoquePecaFornecedores`) para vincular o fornecedor selecionado no dropdown (`T-018.3`) à peça recém-criada.

### 2.2 `FornecedoresService` (`src/app/core/services/fornecedores.service.ts`)
- O método `list()` (`GET /api/v1/Fornecedores`) retorna a lista de fornecedores cadastrados, alimentando o dropdown de **Fornecedor** no `EstoquePecaCadastroComponent`.

### 2.3 Regra do Badge de Estoque Crítico (`T-018.4`)
- Em `EstoqueLista` (`estoque-lista.ts`):
  - Ajustar `isCritico(item: EstoquePeca): boolean` para avaliar estritamente `item.quantidade < item.estoqueMinimo`.
  - Nas estatísticas (`estatisticas`), usar `this.itens.filter(i => this.isCritico(i)).length`.
  - Em `estoque-lista.html`, exibir `<span class="badge-critico">Crítico</span>` ao lado da quantidade quando `isCritico(item)` for verdadeiro.

---

## 3. Análise de Backend e Banco de Dados (MySQL 8.0)

- **Endpoints existentes**:
  - `POST /api/v1/EstoquePecas` (`CreateEstoquePecaDTO`)
  - `GET /api/v1/EstoqueCategorias`
  - `GET /api/v1/EstoqueFabricantes`
  - `GET /api/v1/EstoqueLocalizacoes`
  - `GET /api/v1/Fornecedores`
  - `POST /api/v1/EstoquePecaFornecedores` (`CreateEstoquePecaFornecedorDTO`)
- **Mudanças de schema**: Nenhuma alteração de banco de dados (MySQL 8.0) é necessária.

