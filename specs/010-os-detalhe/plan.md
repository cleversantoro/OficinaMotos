# Plano de Implementação: Página de Detalhe Real da Ordem de Serviço

**Branch**: `010-os-detalhe` | **Data**: 2026-09-08 | **Spec**: [spec.md](spec.md)

**Entrada**: US-010 — Criar página de detalhe real da OS.

## Resumo

A feature substitui o componente placeholder atual de `/ordens/:id` por uma página de detalhe robusta, completa e reativa (`OsDetalheComponent`). A tela apresentará cinco blocos principais: (1) Card de "Dados Gerais" com cliente e veículo enriquecidos com nomes e placas; (2) Controle de status com dropdown de transições estritamente válidas e confirmação via API; (3) Seção "Itens da OS" com tabela de peças/serviços, subtotais e soma consolidada; (4) Seção "Observações" com notas cronológicas; e (5) Seção "Pagamentos" com quitações, total pago e saldo pendente. O componente utilizará Angular Signals, `ChangeDetectionStrategy.OnPush`, serviços centralizados em `apiPaths` e proteção por `ordensPermissionGuard`.

## Contexto Técnico

**Linguagem/Versão**: TypeScript 5.9.2 com Angular 21

**Dependências Principais**: Angular Signals, Angular Common (`DatePipe`, `CurrencyPipe`), Angular Router (`ActivatedRoute`, `Router`, `RouterLink`), PrimeNG 21 (`ButtonModule`, `TagModule`, `TableModule`), RxJS 7.8, `Toast`, `Confirmation`

**Armazenamento**: N/A no frontend; persistência e leitura via API REST existente

**Testes**: Vitest via builder do Angular (`npm test`), testes unitários do componente e testes de isolamento de transição de status

**Plataforma Alvo**: Navegadores modernos suportados pelo Angular 21, totalmente responsivo (desktop, tablet e mobile)

**Tipo de Projeto**: Aplicação web SPA Angular standalone

**Metas de Desempenho**: Carregamento inicial em < 2s; atualização imediata de status após resposta da API sem recarga de página; bloqueio estrito de cliques múltiplos/concorrentes durante submissões

**Restrições**: Proteger acesso com permissão canônica `ordens × visualizar`; centralizar endpoints exclusivamente em `apiPaths`; não expor IDs numéricos crus ao usuário; aplicar matriz rígida de transições válidas baseada no enum `OrdemServicoStatus`

**Escala/Escopo**: 1 rota (`/ordens/:id`), 1 componente (`OsDetalheComponent`), atualização de models e criação de suíte de testes unitários; sem CRUD de criação de novos itens/pagamentos (escopo de histórias posteriores)

## Verificação da Constituição

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [x] **Domínio**: A feature opera estritamente no Bounded Context de Ordem de Serviço, referenciando entidades de Cliente e Veículo somente para leitura e exibição complementar via seus serviços correspondentes.
- [x] **Segurança**: A rota está vinculada ao `authGuard` e ao `ordensPermissionGuard` exigindo `ordens × visualizar`.
- [x] **Frontend Standalone**: `OsDetalheComponent` é um componente standalone, aplicando Angular Signals (`signal`, `computed`) e `ChangeDetectionStrategy.OnPush`.
- [x] **API Centralizada**: Todas as chamadas usam os serviços `OrdensService`, `ClientesService` e `VeiculosService`, que respeitam `apiPaths`. Não há URLs literais construídas no componente.
- [x] **Integridade de Estados**: Transições de status obedecem estritamente às regras do enum `OrdemServicoStatus` (US-006).
- [x] **Documentação e Padrões**: Artefatos técnicos escritos em português e integrados ao Spec-Kit.

### Reavaliação após o desenho

- [x] Nenhuma nova migration ou alteração de schema backend é necessária; os endpoints `GET /api/v1/OrdemServicos/{id}` e `PUT /api/v1/OrdemServicos/{id}` suportam todos os dados requeridos.
- [x] O contrato de atualização preserva os dados originais da OS e apenas transita o status com integridade.
- [x] Resolução assíncrona de cliente e veículo não bloqueia a renderização dos dados básicos da OS.

## Estrutura do Projeto

### Documentação desta funcionalidade

```text
specs/010-os-detalhe/
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
├── src/app/app.routes.ts
├── src/app/features/ordens-servico/pages/os-detalhe/
│   ├── os-detalhe.ts
│   ├── os-detalhe.html
│   ├── os-detalhe.scss
│   └── os-detalhe.spec.ts
├── src/app/core/models/ordem-servico.ts
├── src/app/core/services/ordens.service.ts
├── src/app/core/services/clientes.service.ts
├── src/app/core/services/veiculos.service.ts
└── src/app/core/services/mecanicos.service.ts
```

**Decisão de Estrutura**: Evoluir o componente existente em `features/ordens-servico/pages/os-detalhe` substituindo a casca inicial pela implementação real com seus 5 blocos funcionais, adicionando testes unitários dedicados em `os-detalhe.spec.ts`.

## Rastreamento de Complexidade

Não há violações de arquitetura a justificar.
