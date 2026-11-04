# Plano de Implementação: Cadastro de Peça no Estoque e Badge de Estoque Crítico

**Branch**: `018-cadastro-peca-estoque` | **Data**: 2026-11-04 | **Spec**: [spec.md](spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/agents.md`](file:///c:/Projetos/OficinaMotos/governance/agents.md)

**Entrada**: US-018 — Criar formulário de cadastro de peça no estoque
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-001
- **Tasks**:
  - [ ] T-018.1 — Criar `EstoquePecaCadastroComponent`
  - [ ] T-018.2 — Registrar rota `/estoque/novo`
  - [ ] T-018.3 — Dropdowns de fabricante, categoria, fornecedor
  - [ ] T-018.4 — Badge de estoque crítico na lista quando `qtd < qtd_minima`

---

## Resumo

Esta funcionalidade implementa o formulário de cadastro de peças no estoque (`/estoque/novo`) e o badge de alerta de estoque crítico na listagem (`/estoque`).

O plano técnico contempla:
1. **Extensão do `EstoqueService`**: Adição do método `vincularFornecedor(body)` (`POST /api/v1/EstoquePecaFornecedores`).
2. **Componente `EstoquePecaCadastroComponent` (`T-018.1`, `T-018.3`)**:
   - Componente Angular 21 Standalone com `ChangeDetectionStrategy.OnPush`, Signals e Reactive Forms em Dark Theme.
   - Dropdowns dinâmicos de **Fabricante** (`EstoqueService.fabricantes()`), **Categoria** (`EstoqueService.categorias()`), **Fornecedor** (`FornecedoresService.list()`) e **Localização** (`EstoqueService.localizacoes()`).
   - Criação da peça via `EstoqueService.criarPeca()` e vínculo opcional com o fornecedor via `EstoqueService.vincularFornecedor()`.
3. **Rota `/estoque/novo` e Navegação (`T-018.2`)**:
   - Registro de `{ path: 'estoque/novo', component: EstoquePecaCadastroComponent }` em `app.routes.ts` sob o `MainLayout` (`authGuard`).
   - Adição de `RouterLink` e `routerLink="/estoque/novo"` ao botão **"+ Nova peça"** em `EstoqueLista`.
4. **Badge de Estoque Crítico na Listagem (`T-018.4`)**:
   - Atualização de `isCritico(item)` em `estoque-lista.ts` para avaliar estritamente `item.quantidade < item.estoqueMinimo`.
   - Exibição do badge `<span class="badge-critico">Crítico</span>` na coluna de estoque em `estoque-lista.html`.
5. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de esquema necessária.

---

## Artefatos de Design Gerados

- **Pesquisa Técnica**: [research.md](research.md)
- **Modelo de Dados**: [data-model.md](data-model.md)
- **Contratos de API**: [contracts/api.md](contracts/api.md)
- **Contratos de UI**: [contracts/ui.md](contracts/ui.md)
- **Guia Rápido de Verificação**: [quickstart.md](quickstart.md)

---

## Fases de Execução

### Fase 1: Serviço de Estoque
1. Atualizar `oficina-motos-web/src/app/core/services/estoque.service.ts`:
   - Adicionar `vincularFornecedor(body: any)` invocando `this.api.create(apiPaths.estoque.fornecedores, body)`.

### Fase 2: Componente `EstoquePecaCadastroComponent` e Dropdowns (T-018.1, T-018.3)
1. Criar `oficina-motos-web/src/app/features/estoque/pages/estoque-peca-cadastro/estoque-peca-cadastro.ts`, `.html` e `.scss`.

### Fase 3: Registro da Rota `/estoque/novo` e Badge de Estoque Crítico em `EstoqueLista` (T-018.2, T-018.4)
1. Atualizar `oficina-motos-web/src/app/app.routes.ts` registrando `{ path: 'estoque/novo', component: EstoquePecaCadastroComponent }`.
2. Atualizar `estoque-lista.ts`, `estoque-lista.html` e `estoque-lista.scss` com `routerLink="/estoque/novo"`, critério `item.quantidade < item.estoqueMinimo` e badge `.badge-critico`.

### Fase 4: Testes Unitários, Build e Governança
1. Criar `estoque-peca-cadastro.spec.ts` e `estoque-lista.spec.ts` no Vitest.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar `governance/backlog.md` marcando `T-018.1` a `T-018.4` como concluídas.

