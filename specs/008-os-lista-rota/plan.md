# Plano de Implementação: Lista de Ordens e Rota Corrigida

**Branch**: `008-os-lista-rota` | **Data**: 2026-08-12 | **Spec**: [spec.md](spec.md)

**Entrada**: US-008 — Renomear `OsDetalhe` para `OsLista` e corrigir as rotas da área de ordens.

## Resumo

A feature reorganiza a página atual de ordens para uma lista denominada `OsListaComponent`, exposta em `/ordens`. A lista usará o `DataTable` compartilhado para paginação, filtragem, estados de carregamento e ações por linha. O cabeçalho terá a ação `Nova OS`, com navegação para `/ordens/novo`, e cada ordem terá uma ação de consulta para `/ordens/:id`. Não haverá alteração no backend, banco ou contrato HTTP.

## Contexto Técnico

**Linguagem/Versão**: TypeScript 5.9.2 com Angular 21

**Dependências Principais**: Angular Router, PrimeNG 21, PrimeIcons, RxJS 7.8, `DataTable` compartilhado

**Armazenamento**: N/A para esta feature; a lista consome o endpoint existente por `OrdensService`

**Testes**: Vitest pelo builder de testes do Angular, testes unitários de sessão/helper/guard/componente/rotas e teste E2E quando a infraestrutura estiver disponível

**Plataforma Alvo**: Navegadores suportados pelo Angular 21, desktop e viewport menor

**Tipo de Projeto**: Aplicação web SPA Angular standalone

**Metas de Desempenho**: O intervalo entre o início de `OrdensService.list()` e o estado final renderizado deve ser de até 2 segundos nos testes controlados, conforme SC-006

**Restrições**: Preservar `authGuard`, transportar permissões na sessão, autorizar `ordens × visualizar` e `ordens × criar` com guard e Toast de acesso negado, usar `loadComponent` nas rotas da feature, não hardcodar URLs de API, usar componentes standalone, Signals quando houver estado reativo novo, `OnPush` na lista e reutilizar `DataTable`

**Escala/Escopo**: Uma página de lista, configuração de rotas da área de ordens, estados da consulta e testes unitários; sem mudança de API ou persistência

## Verificação da Constituição

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Antes da pesquisa

- [x] **Domínio e limites**: a mudança permanece no bounded context de Ordem de Serviço e não cria entidades compartilhadas.
- [x] **API versionada**: nenhuma chamada nova; a origem continua `apiPaths.ordens.base` em `/api/v1/OrdemServicos`.
- [x] **Segurança**: as rotas continuam filhas do `MainLayout` protegido pelo `authGuard`; o guard da área valida permissões específicas e a ação de exclusão preserva RBAC.
- [x] **RBAC**: as permissões canônicas `visualizar` e `criar` serão transportadas na sessão e verificadas por módulo e ação.
- [x] **Frontend standalone**: os componentes serão standalone, carregados com `loadComponent`, usarão a infraestrutura shared existente e a lista aplicará `OnPush`.
- [x] **Qualidade**: serão adicionados testes de sessão, helper, guard, navegação, ações, estados e paginação; E2E será executado quando houver infraestrutura; TypeScript strict permanece ativo.
- [x] **Documentação**: a especificação, pesquisa, modelo, contrato e quickstart estão em português.

### Após o desenho

- [x] O desenho não introduz violação constitucional nem requer nova API, migration ou tabela.
- [x] O lazy loading será atendido por `loadComponent` nas rotas `/ordens`, `/ordens/novo` e `/ordens/:id`.
- [x] A autorização de consulta e criação será atendida por guard funcional baseado nas permissões reais da sessão, com fallback de papel somente se documentado pelo helper existente.
- [x] O acesso autenticado sem a permissão exigida exibirá `Você não tem permissão para acessar esta área.` e será redirecionado para `/dashboard`.
- [x] A paginação local foi escolhida porque o endpoint atual retorna coleção e a US-008 não define contrato server-side.
- [x] A lacuna de componentes para `/ordens/novo` e `/ordens/:id` foi explicitamente registrada no contrato e deve ser resolvida nas tarefas de implementação.
- [x] Não há complexidade excepcional a justificar.

## Estrutura do Projeto

### Documentação desta funcionalidade

```text
specs/008-os-lista-rota/
├── plan.md              # Saída do comando de planejamento
├── research.md          # Pesquisa da Fase 0
├── data-model.md        # Modelo da Fase 1
├── quickstart.md        # Guia da Fase 1
├── contracts/           # Contratos da Fase 1
└── tasks.md             # Tarefas geradas pelo comando de tarefas
```

### Código-fonte

```text
oficina-motos-web/
├── src/app/app.routes.ts
├── src/app/features/ordens-servico/pages/
│   ├── os-lista/
│   │   ├── os-lista.ts
│   │   ├── os-lista.html
│   │   └── os-lista.scss
│   ├── os-novo/                  # destino de criação
│   └── os-detalhe/               # destino individual
├── src/app/core/auth/ordens-permission.guard.ts
├── src/app/core/auth/auth.model.ts
├── src/app/core/auth/auth.service.ts
├── src/app/core/services/ordens.service.ts
├── src/app/core/models/ordem-servico.ts
└── src/app/shared/ui/data-table/
    ├── data-table.ts
    ├── data-table.html
    └── data-table.models.ts

specs/008-os-lista-rota/
├── plan.md
├── research.md
├── data-model.md
├── contracts/ui.md
└── quickstart.md
```

**Decisão de Estrutura**: manter a feature no frontend Angular existente, sob `features/ordens-servico/pages`, substituindo a pasta `os-detalhe` que hoje contém a lista. A infraestrutura de dados permanece em `core/services`, a tabela em `shared/ui/data-table` e o guard RBAC em `core/auth`. A configuração de rotas continua centralizada em `src/app/app.routes.ts`, com `loadComponent` para lazy loading.

## Rastreamento de Complexidade

Não há violações da constituição a justificar.
