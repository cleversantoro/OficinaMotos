# Plano de Implementação: Formulário de Cadastro de Fornecedor

**Branch**: `017-cadastro-fornecedor` | **Data**: 2026-11-04 | **Spec**: [spec.md](spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/agents.md`](file:///c:/Projetos/OficinaMotos/governance/agents.md)

**Entrada**: US-017 — Criar formulário de cadastro de fornecedor
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-001
- **Tasks**:
  - [ ] T-017.1 — Criar `FornecedorCadastroComponent`
  - [ ] T-017.2 — Registrar rota `/fornecedores/novo`
  - [ ] T-017.3 — Validator de CNPJ e integração `CepService`
  - [ ] T-017.4 — Dropdown de segmentos

---

## Resumo

Esta funcionalidade implementa o fluxo completo de cadastro de fornecedores na aplicação web da Oficina MotoPro (`/fornecedores/novo`). Atualmente, a listagem de fornecedores (`/fornecedores`) já exibe o botão **"+ Novo fornecedor"**, mas a rota `/fornecedores/novo` e o componente `FornecedorCadastroComponent` ainda não estavam implementados.

O plano técnico abrange:
1. **Extensão do `FornecedoresService`**: Adição do método `criarEndereco(body)` para persistência do endereço comercial principal (`POST /api/v1/FornecedorEnderecos`).
2. **Componente `FornecedorCadastroComponent` (`T-017.1`, `T-017.3`, `T-017.4`)**:
   - Componente Angular 21 Standalone com `ChangeDetectionStrategy.OnPush`, Signals e Reactive Forms.
   - Validação de CNPJ com `cnpjValidator()` e máscara automática `00.000.000/0000-00` (`T-017.3`).
   - Integração com `CepService.buscarCep()` (`src/app/shared/services/cep.ts`) para preenchimento automático de logradouro, bairro, cidade, UF e complemento (`T-017.3`).
   - Dropdown dinâmico de segmentos carregado via `FornecedoresService.segmentos()` (`T-017.4`).
3. **Registro da Rota `/fornecedores/novo` (`T-017.2`)**:
   - Registro de `{ path: 'fornecedores/novo', component: FornecedorCadastroComponent }` em `app.routes.ts` sob o `MainLayout` (`authGuard`), posicionado estritamente antes de `fornecedores/:id`.
4. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de schema é necessária (endpoints e tabelas já existentes).
5. **Qualidade e Governança**:
   - Suíte de testes unitários no Vitest (`fornecedor-cadastro.spec.ts`), validação de build de produção (`npm run build`) e atualização do [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md).

---

## Artefatos de Design Gerados

- **Pesquisa Técnica**: [research.md](research.md)
- **Modelo de Dados**: [data-model.md](data-model.md)
- **Contratos de API**: [contracts/api.md](contracts/api.md)
- **Contratos de UI**: [contracts/ui.md](contracts/ui.md)
- **Guia Rápido de Verificação**: [quickstart.md](quickstart.md)

---

## Fases de Execução

### Fase 1: Serviço de Fornecedores
1. Atualizar `oficina-motos-web/src/app/core/services/fornecedores.service.ts`:
   - Adicionar `criarEndereco(body: any)` invocando `this.api.create(apiPaths.fornecedores.enderecos, body)`.

### Fase 2: Componente `FornecedorCadastroComponent` (T-017.1, T-017.3, T-017.4)
1. Criar `oficina-motos-web/src/app/features/fornecedores/pages/fornecedor-cadastro/fornecedor-cadastro.ts`:
   - Standalone component, `OnPush`, `FormBuilder`, Signals (`loading`, `submitting`, `searchingCep`, `segmentos`).
   - Carregamento de segmentos ativos (`FornecedoresService.segmentos()`) e sugestão de código (`FOR-XXX`) no `ngOnInit()`.
   - Máscara e validação de CNPJ (`cnpjValidator()`), telefone e CEP (`cepValidator()`).
   - Método `buscarCep()` integrado ao `CepService.buscarCep()` preenchendo `logradouro`, `bairro`, `cidade`, `estado` (`uf`) e `complemento`.
   - Método `salvar()` enviando `FornecedoresService.create()`, seguido de `criarEndereco()` (se endereço informado) e `vincularSegmento()` (se segmento selecionado), exibindo Toast e redirecionando para `/fornecedores`.
2. Criar template `fornecedor-cadastro.html` e estilos `fornecedor-cadastro.scss` no padrão Dark Theme.

### Fase 3: Registro de Rota e Navegação (T-017.2)
1. Atualizar `oficina-motos-web/src/app/app.routes.ts`:
   - Importar `FornecedorCadastroComponent` e registrar `{ path: 'fornecedores/novo', component: FornecedorCadastroComponent }` antes de `fornecedores/:id`.

### Fase 4: Testes Unitários, Build e Governança
1. Criar `oficina-motos-web/src/app/features/fornecedores/pages/fornecedor-cadastro/fornecedor-cadastro.spec.ts` cobrindo:
   - Inicialização e carregamento do dropdown de segmentos;
   - Validação de CNPJ válido/inválido e máscara;
   - Integração com `CepService.buscarCep()` para autopreenchimento de endereço;
   - Submissão do formulário e criação de fornecedor + endereço;
   - Registro da rota `fornecedores/novo` antes de `fornecedores/:id` em `app.routes.ts`.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar `governance/backlog.md` marcando `T-017.1` a `T-017.4` como concluídas.

