# Plano de Implementação: Formulário de Nova Ordem de Serviço

**Branch**: `009-os-cadastro-formulario` | **Data**: 2026-08-12 | **Spec**: [spec.md](spec.md)

**Entrada**: US-009 — Criar formulário de nova Ordem de Serviço.

## Resumo

A feature substitui a tela inicial de `/ordens/novo` por `OsCadastroComponent`, um formulário standalone com cliente autocomplete, veículo dependente do cliente, mecânico, descrição, validação e submissão. A criação usará `OrdensService.create()` e navegará para `/ordens/:id` após receber um ID válido. Não haverá alteração de banco ou endpoint fora dos ajustes de tipagem/métodos semânticos dos services existentes.

## Contexto Técnico

**Linguagem/Versão**: TypeScript 5.9.2 com Angular 21

**Dependências Principais**: Angular Reactive Forms, Signals, Angular Router, PrimeNG 21, RxJS 7.8, `Toast`, `ngx-mask` quando aplicável

**Armazenamento**: N/A; persistência ocorre pela API existente

**Testes**: Vitest pelo builder do Angular, testes de serviços/formulário/guard e E2E quando disponível

**Plataforma Alvo**: Navegadores suportados pelo Angular 21, desktop e viewport menor

**Tipo de Projeto**: Aplicação web SPA Angular standalone

**Metas de Desempenho**: Impedir submissões duplicadas e atualizar estados dependentes após cada resposta de consulta sem bloquear a interação principal

**Restrições**: Usar permissões da sessão, preservar `authGuard`/`ordensPermissionGuard`, não hardcodar URLs, usar services via `apiPaths`, Signals, standalone, responsividade e validação antes da criação

**Escala/Escopo**: Uma tela, três services de leitura, um service de criação, DTO de request e testes; sem CRUD de cliente/veículo/mecânico

## Verificação da Constituição

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [x] **Domínio**: a feature permanece no contexto de Ordem de Serviço e referencia dados de Cadastro somente por IDs/leituras de referência.
- [x] **Segurança**: a rota usa `authGuard` e `ordensPermissionGuard`; a submissão repete a verificação de `ordens × criar`.
- [x] **Frontend standalone**: `OsCadastroComponent` será standalone, usará Signals e Reactive Forms.
- [x] **API centralizada**: clientes, veículos, mecânicos e ordens usam services e `apiPaths`; o componente não monta URLs.
- [x] **Qualidade**: validações de campos, erros, duplicidade, navegação e vínculo cliente/veículo terão testes.
- [x] **Documentação**: todos os artefatos desta feature estão em português.

### Reavaliação após o desenho

- [x] Não há nova tabela, migration ou endpoint obrigatório.
- [x] `veiculoId` é tratado como parte do contrato já introduzido pela US-007.
- [x] A ausência de runner E2E não bloqueia o design; deve ser documentada durante a validação.

## Estrutura do Projeto

### Documentação desta funcionalidade

```text
specs/009-os-cadastro-formulario/
├── plan.md
├── research.md
├── data-model.md
├── contracts/ui.md
├── quickstart.md
└── tasks.md
```

### Código-fonte

```text
oficina-motos-web/
├── src/app/app.routes.ts
├── src/app/features/ordens-servico/pages/os-novo/
│   ├── os-novo.ts
│   ├── os-novo.html
│   └── os-novo.scss
├── src/app/core/auth/ordens-permission.guard.ts
├── src/app/core/models/ordem-servico.ts
├── src/app/core/services/clientes.service.ts
├── src/app/core/services/veiculos.service.ts
├── src/app/core/services/mecanicos.service.ts
├── src/app/core/services/ordens.service.ts
└── src/app/shared/services/toast.ts
```

**Decisão de Estrutura**: substituir a tela inicial existente em `features/ordens-servico/pages/os-novo` pelo formulário real. Services permanecem em `core/services`, autorização em `core/auth`, modelo de request em `core/models` e mensagens em `shared/services`.

## Rastreamento de Complexidade

Não há violações da constituição a justificar.
