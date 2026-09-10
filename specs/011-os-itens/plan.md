# Plano de Implementação: Adicionar/Remover Peças e Serviços na OS

**Branch**: `011-os-itens` | **Data**: 2026-09-09 | **Spec**: [spec.md](spec.md)

**Entrada**: US-011 — Adicionar/remover peças e serviços na OS

## Resumo

A funcionalidade adiciona a capacidade de gerenciar o escopo de itens na página de detalhe da OS (`/ordens/:id`), permitindo a inclusão de peças do estoque via modal com autocomplete, adição de serviços/mão de obra via modal dedicado e remoção de itens com confirmação. O recálculo de totais da OS (`totalItens` e `saldoPendente`) ocorre de forma estritamente reativa utilizando Angular Signals sem recarregamento da página. Ordens finalizadas (`Concluida` ou `Cancelada`) têm suas ações bloqueadas para garantir a integridade dos atendimentos.

## Contexto Técnico

**Linguagem/Versão**: TypeScript 5.9.2 com Angular 21

**Dependências Principais**: Angular Signals (`signal`, `computed`), Angular Common (`CurrencyPipe`), Angular Forms (`FormsModule`), PrimeNG 21 (`DialogModule`, `ButtonModule`, `InputTextModule`, `InputNumberModule`), Serviços compartilhados (`Toast`, `Confirmation`), Serviços de domínio (`OrdensService`, `EstoqueService`)

**Armazenamento**: API REST com MySQL 8 via endpoints `/api/v1/OrdemServicoItens` e `/api/v1/EstoquePecas`

**Testes**: Vitest via Angular unit test builder (`npx ng test --no-watch`) cobrindo modais e detalhe

**Plataforma Alvo**: Navegadores modernos (Desktop, Tablet e Mobile)

**Tipo de Projeto**: SPA Angular standalone com Clean Architecture no backend

**Metas de Desempenho**: Inclusão de item refletida em < 1s; autocomplete com debounce e busca incremental a partir de 2 caracteres; zero reloads de página inteira

**Restrições**: Respeitar RBAC e estado imutável de OS terminal; centralizar chamadas em `apiPaths.ordens.itens`; validar quantidade $\ge 1$ e descrição $\le 240$ caracteres

## Verificação da Constituição

- [x] **Domínio Primeiro (DDD)**: O sub-recurso Item pertence ao Bounded Context de Ordem de Serviço, referenciando o BC de Estoque apenas como fonte de consulta de dados de peças.
- [x] **API RESTful Versionada**: Utiliza endpoints canônicos `POST /api/v1/OrdemServicoItens` e `DELETE /api/v1/OrdemServicoItens/{id}` registrados em `api-paths.ts`.
- [x] **Segurança por Design**: Protegido por JWT e restrito a usuários autorizados; ações bloqueadas quando o status for terminal.
- [x] **Frontend Standalone + Signals**: `OsItemPecaModalComponent` e `OsItemServicoModalComponent` criados como standalone; reatividade no `OsDetalheComponent` usando Signals (`ordem.update`, `computed`).
- [x] **Integridade de Dados**: Exclusão segura com diálogo de confirmação (`Confirmation.confirmDelete`); validações síncronas de quantidade e tamanho de texto.
- [x] **Qualidade e Testabilidade**: Testes unitários cobrindo autocomplete, submissão de peça, submissão de serviço, confirmação de delete e recálculo reativo dos totais.

## Estrutura do Projeto

### Documentação desta funcionalidade

```text
specs/011-os-itens/
├── spec.md
├── plan.md
├── research.md
├── data-model.md
├── contracts/
│   └── ui.md
├── quickstart.md
└── checklists/
    └── requirements.md
```

### Código-fonte

```text
oficina-motos-web/
├── src/app/core/models/ordem-servico.ts
├── src/app/core/services/ordens.service.ts
├── src/app/core/services/estoque.service.ts
├── src/app/features/ordens-servico/components/
│   ├── os-item-peca-modal/
│   │   ├── os-item-peca-modal.ts
│   │   ├── os-item-peca-modal.html
│   │   ├── os-item-peca-modal.scss
│   │   └── os-item-peca-modal.spec.ts
│   └── os-item-servico-modal/
│       ├── os-item-servico-modal.ts
│       ├── os-item-servico-modal.html
│       ├── os-item-servico-modal.scss
│       └── os-item-servico-modal.spec.ts
└── src/app/features/ordens-servico/pages/os-detalhe/
    ├── os-detalhe.ts
    ├── os-detalhe.html
    ├── os-detalhe.scss
    └── os-detalhe.spec.ts
```

## Rastreamento de Complexidade

Não há violações arquiteturais ou desvios de constituição.
