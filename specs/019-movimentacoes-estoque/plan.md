# Plano de Implementação: Registrar Movimentações de Entrada e Saída de Estoque (US-019)

**Branch**: `019-movimentacoes-estoque` | **Data**: 2026-11-04 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/019-movimentacoes-estoque/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/agents.md`](file:///c:/Projetos/OficinaMotos/governance/agents.md)

**Entrada**: US-019 — Registrar movimentações de entrada e saída de estoque
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-018
- **Tasks**:
  - [ ] T-019.1 — Criar `EstoqueEntradaModalComponent`
  - [ ] T-019.2 — Criar `EstoqueSaidaModalComponent`
  - [ ] T-019.3 — Integrar `POST /api/v1/EstoqueMovimentacoes`
  - [ ] T-019.4 — Criar `EstoquePecaDetalheComponent` com histórico
  - [ ] T-019.5 — Registrar rota `/estoque/:id`

---

## Resumo

Esta funcionalidade implementa o controle completo de movimentações de entrada e saída de peças no estoque, além da página de detalhes da peça (`/estoque/:id`) com histórico cronológico de movimentações.

O plano técnico contempla:
1. **Extensão do [`EstoqueService`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/estoque.service.ts) (`T-019.3`)**:
   - Adição do método `listarMovimentacoes(params?: Record<string, any>)` (`GET /api/v1/EstoqueMovimentacoes`).
   - Utilização de `movimentar(body)` (`POST /api/v1/EstoqueMovimentacoes`) em conjunto com `atualizarPeca(id, body)` (`PUT /api/v1/EstoquePecas/:id`) para persistir a movimentação e atualizar o saldo em estoque.
2. **Modal de Entrada `EstoqueEntradaModalComponent` (`T-019.1`, `T-019.3`)**:
   - Componente Angular 21 Standalone com `<p-dialog>`, Signals e Reactive Forms.
   - Campos `quantidade` (`Validators.required`, `Validators.min(1)`) e `referencia` (`Validators.required`, `Validators.maxLength(200)`).
   - Envia `POST /api/v1/EstoqueMovimentacoes` (`tipo: "ENTRADA"`) e atualiza `peca.quantidade` para `saldoAtual + quantidade`.
3. **Modal de Saída `EstoqueSaidaModalComponent` (`T-019.2`, `T-019.3`)**:
   - Componente Angular 21 Standalone com `<p-dialog>`, Signals e Reactive Forms.
   - Campos `quantidade` (`Validators.required`, `Validators.min(1)`, validando `quantidade <= peca.quantidade`) e `referencia` (`Validators.required`, `Validators.maxLength(200)`).
   - Envia `POST /api/v1/EstoqueMovimentacoes` (`tipo: "SAIDA"`) e atualiza `peca.quantidade` para `saldoAtual - quantidade`.
4. **Página de Detalhes e Histórico `EstoquePecaDetalheComponent` (`T-019.4`)**:
   - Exibe dados cadastrais da peça, indicadores de saldo (com badge **Crítico** quando `quantidade < estoqueMinimo`), botões para abrir os modais de Entrada e Saída, e tabela cronológica de movimentações.
5. **Rota `/estoque/:id` e Navegação na Listagem (`T-019.5`)**:
   - Registro de `{ path: 'estoque/:id', component: EstoquePecaDetalheComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts) após `estoque/novo`.
   - Adição de links `[routerLink]="['/estoque', item.id]"` no nome do produto e no botão "Visualizar" em [`estoque-lista.html`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.html).
6. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de esquema necessária.

---

## Contexto Técnico

**Linguagem/Versão**: TypeScript 5.9, Angular 21 (Standalone Components, Signals, Reactive Forms)
**Dependências Principais**: `@angular/common`, `@angular/forms`, `@angular/router`, `primeng/dialog`, `primeng/button`, `rxjs`
**Armazenamento**: API REST .NET 10 + MySQL 8.0 (endpoints existentes `/api/v1/EstoquePecas` e `/api/v1/EstoqueMovimentacoes`)
**Testes**: Vitest (`npm test -- --no-watch`) + `npm run build`
**Plataforma Alvo**: Aplicação Web SPA (Desktop/Tablet em Dark Theme)
**Tipo de Projeto**: Web application (`oficina-motos-web` + `oficina-motos-api`)

## Verificação da Constituição (Governance Check)

- **Clean Architecture & Padrões Angular 21 (`governance/arquitetura.md`)**: Atendido. Componentes standalone com `ChangeDetectionStrategy.OnPush`, Signals e Reactive Forms.
- **Fonte Única da Verdade do Banco (`governance/decisoes.md`)**: Atendido. Nenhuma alteração de banco de dados necessária.
- **Idioma e Documentação (`governance/gemini.md`)**: Atendido. Todos os artefatos e mensagens de UI estão em português (pt-BR).

## Estrutura do Projeto

### Documentação (esta funcionalidade)

```text
specs/019-movimentacoes-estoque/
├── spec.md              # Especificação de requisitos (US-019)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Pesquisa técnica de endpoints, modais e rotas
├── data-model.md        # Modelo de dados e regras de validação
├── quickstart.md        # Guia rápido de verificação e testes
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    ├── api.md           # Contratos REST consumidos
    └── ui.md            # Contratos de UI e estados dos componentes
```

### Código-Fonte (repositório)

```text
oficina-motos-web/src/app/
├── app.routes.ts                                                                      # [MODIFICAR] Registrar rota estoque/:id (T-019.5)
├── core/
│   └── services/
│       └── estoque.service.ts                                                         # [MODIFICAR] Adicionar listarMovimentacoes() (T-019.3)
└── features/
    └── estoque/
        ├── components/
        │   ├── estoque-entrada-modal/
        │   │   ├── estoque-entrada-modal.ts                                           # [CRIAR] Modal de entrada de estoque (T-019.1, T-019.3)
        │   │   ├── estoque-entrada-modal.html                                         # [CRIAR] Template do modal de entrada
        │   │   ├── estoque-entrada-modal.scss                                         # [CRIAR] Estilos do modal de entrada
        │   │   └── estoque-entrada-modal.spec.ts                                      # [CRIAR] Testes unitários do modal de entrada
        │   └── estoque-saida-modal/
        │       ├── estoque-saida-modal.ts                                             # [CRIAR] Modal de saída de estoque (T-019.2, T-019.3)
        │       ├── estoque-saida-modal.html                                           # [CRIAR] Template do modal de saída
        │       ├── estoque-saida-modal.scss                                           # [CRIAR] Estilos do modal de saída
        │       └── estoque-saida-modal.spec.ts                                        # [CRIAR] Testes unitários do modal de saída
        └── pages/
            ├── estoque-lista/
            │   ├── estoque-lista.html                                                 # [MODIFICAR] Links para ['/estoque', item.id] (T-019.5)
            │   ├── estoque-lista.scss                                                 # [MODIFICAR] Estilo do link no nome da peça
            │   └── estoque-lista.spec.ts                                              # [MODIFICAR] Testar link para /estoque/:id
            └── estoque-peca-detalhe/
                ├── estoque-peca-detalhe.ts                                            # [CRIAR] Página de detalhe + histórico (T-019.4)
                ├── estoque-peca-detalhe.html                                          # [CRIAR] Template de detalhe + histórico
                ├── estoque-peca-detalhe.scss                                          # [CRIAR] Estilos Dark Theme
                └── estoque-peca-detalhe.spec.ts                                       # [CRIAR] Testes unitários de detalhe e histórico
```

---

## Fases de Execução

### Fase 1: Serviço de Estoque (`T-019.3`)
1. Atualizar [`estoque.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/estoque.service.ts) adicionando `listarMovimentacoes(params?: Record<string, any>)`.

### Fase 2: Modais de Entrada e Saída (`T-019.1`, `T-019.2`, `T-019.3`)
1. Criar `EstoqueEntradaModalComponent` (`estoque-entrada-modal.ts`, `.html`, `.scss`) integrando `EstoqueService.movimentar()` (`tipo: 'ENTRADA'`) + `EstoqueService.atualizarPeca()`.
2. Criar `EstoqueSaidaModalComponent` (`estoque-saida-modal.ts`, `.html`, `.scss`) validando `quantidade <= peca.quantidade` e integrando `EstoqueService.movimentar()` (`tipo: 'SAIDA'`) + `EstoqueService.atualizarPeca()`.

### Fase 3: Página de Detalhe da Peça com Histórico e Rota `/estoque/:id` (`T-019.4`, `T-019.5`)
1. Criar `EstoquePecaDetalheComponent` (`estoque-peca-detalhe.ts`, `.html`, `.scss`) exibindo os dados da peça, indicadores de saldo, botões de movimentação e tabela de histórico.
2. Registrar `{ path: 'estoque/:id', component: EstoquePecaDetalheComponent }` em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts).
3. Atualizar [`estoque-lista.html`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.html) e [`estoque-lista.scss`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.scss) para navegar para `['/estoque', item.id]`.

### Fase 4: Testes Unitários, Build e Governança
1. Criar testes unitários `estoque-entrada-modal.spec.ts`, `estoque-saida-modal.spec.ts`, `estoque-peca-detalhe.spec.ts` e atualizar `estoque-lista.spec.ts`.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) marcando `T-019.1` a `T-019.5` como concluídas na etapa `/speckit-implement`.
