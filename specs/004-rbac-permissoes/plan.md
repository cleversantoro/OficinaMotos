# Implementation Plan: RBAC por permissões em controllers de negócio

**Branch**: `004-rbac-permissoes` | **Date**: 2026-08-03 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from [spec.md](spec.md)

## Summary

Aplicar controle de acesso por papel nas operações destrutivas dos módulos de negócio, usando a matriz canônica de perfis e permissões do módulo `seg_` como fonte de verdade. A entrega cobre backend e frontend: proteção explícita nas operações sensíveis, retorno de 403 para acesso insuficiente e ocultação dos botões destrutivos na interface quando o papel não tiver permissão.

## Technical Context

**Language/Version**: Backend em C# / .NET 8; frontend em TypeScript / Angular 21

**Primary Dependencies**: ASP.NET Core Web API, JWT Bearer, Entity Framework Core, Pomelo MySQL, Angular Router, Signals, PrimeNG, ngx-mask, Vitest

**Storage**: MySQL 8+ com esquema `seg_` já documentado em [oficina-motos-docs/markdown/SEGURANCA_USUARIOS.md](../../oficina-motos-docs/markdown/SEGURANCA_USUARIOS.md)

**Testing**: xUnit / testes de API no backend quando aplicável; Vitest no frontend; validação manual de autorização e visibilidade de ações

**Target Platform**: Aplicação web com API REST e frontend Angular, executada localmente em Windows durante o desenvolvimento

**Project Type**: Web application com backend e frontend separados

**Performance Goals**: Responder negações de acesso com 403 sem degradar os fluxos autorizados; manter visibilidade de ações consistente com o perfil autenticado

**Constraints**: Respeitar a constituição do projeto, o padrão `/api/v1/`, JWT obrigatório em rotas protegidas, e a matriz oficial de perfis/permissões publicada em [CONSTITUTION.md](../../oficina-motos-docs/markdown/CONSTITUTION.md)

**Scale/Scope**: Feature transversal aos controllers de negócio e às telas de ação destrutiva dos módulos principais, com foco inicial nos fluxos de cliente, veículo, ordem, estoque, financeiro, fornecedores e mecânicos

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Domínio primeiro: a feature não introduz novo contexto; reutiliza o módulo `seg_` e os módulos de negócio já existentes.
- API REST versionada: a autorização é aplicada sobre rotas v1 existentes, sem quebrar contrato de URL.
- Segurança por design: atende à exigência de autenticação, autorização por papel e ocultação de ações sensíveis na UI.
- Frontend reativo com componentes standalone: a decisão de visibilidade no frontend deve usar a arquitetura já adotada no app Angular.
- Integridade e rastreabilidade: o comportamento de 403 e a matriz de permissões preservam a origem documental canônica.
- Qualidade e testabilidade: a feature é diretamente testável por cenários de autorização e visibilidade.
- Documentação como fonte de verdade: a matriz de perfis e permissões já existe em [SEGURANCA_USUARIOS.md](../../oficina-motos-docs/markdown/SEGURANCA_USUARIOS.md) e [CONSTITUTION.md](../../oficina-motos-docs/markdown/CONSTITUTION.md).

## Project Structure

### Documentation (this feature)

```text
specs/004-rbac-permissoes/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
└── tasks.md
```

### Source Code (repository root)

```text
oficina-motos-api/
├── src/OficinaMotos.API/Controllers/
├── src/OficinaMotos.API/Program.cs
├── src/OficinaMotos.Application/
└── src/OficinaMotos.Infrastructure/

oficina-motos-web/
├── src/app/core/auth/
├── src/app/core/services/
├── src/app/shared/ui/data-table/
├── src/app/features/
└── src/app/app.routes.ts
```

**Structure Decision**: A feature atravessa os dois repositórios principais do workspace, com alteração concentrada nos controllers de negócio da API e nos pontos de ação visíveis no frontend Angular. Os artefatos de design ficam em `specs/004-rbac-permissoes/`.

## Complexity Tracking

Nenhuma violação estrutural da constituição foi identificada nesta fase.

## Phase 1 Outputs

- [research.md](research.md) concluído com decisões de arquitetura e validações de abordagem.
- [data-model.md](data-model.md) concluído com perfis, permissões, matriz e validação de estados.
- [quickstart.md](quickstart.md) concluído com cenários de validação manual.
- [contracts/rbac-access-contract.md](contracts/rbac-access-contract.md) concluído com o contrato de acesso entre API e interface.

## Post-Design Constitution Check

- Mantido: a feature continua aderente aos princípios de DDD, segurança por design, frontend standalone, rastreabilidade e documentação canônica.
- Mantido: não há necessidade de justificativa em Complexity Tracking.
