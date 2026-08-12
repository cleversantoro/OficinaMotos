# Implementation Plan: US-006 — Criar enum OrdemServicoStatus

**Branch**: `[006-ordem-servico-status]` | **Date**: 2026-08-12 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/006-ordem-servico-status/spec.md`

## Summary

Adicionar o enum `OrdemServicoStatus` ao domínio da API, trocar a propriedade `Status` da entidade `OrdemServico` para tipo enum, ajustar os DTOs de criação/consulta, configurar a conversão para string no EF Core e publicar a migration `AddOrdemServicoStatusEnum` para manter compatibilidade do banco sem quebrar os fluxos de ordem de serviço.

## Technical Context

**Language/Version**: C# 12 em .NET 8 (`net8.0`)

**Primary Dependencies**: ASP.NET Core Web API, Entity Framework Core 8, Pomelo.EntityFrameworkCore.MySql 8

**Storage**: MySQL 8+ (InnoDB, utf8mb4) via EF Core migrations

**Testing**: `dotnet build`, validação de migration EF Core e smoke test dos fluxos de criação/atualização de OS

**Target Platform**: Backend ASP.NET Core hospedado em ambiente Windows/Linux

**Project Type**: Web service monolítico em Clean Architecture (API / Application / Domain / Infrastructure)

**Performance Goals**: manter a latência atual de consultas e operações de OS sem degradação percebível

**Constraints**: preservar compatibilidade com a coluna `status` atual em texto; evitar breaking changes não intencionais; manter migration reversível

**Scale/Scope**: alteração transversal em domínio, DTOs e persistência da entidade `OrdemServico`

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Pré-Research Gate

- Princípio I (Domínio Primeiro): PASS
  - A mudança fica na camada Domain e Infrastructure, sem lógica de negócio em controllers.
- Princípio II (API RESTful Versionada): PASS
  - Não altera contract versioning atual; apenas refina o tipo do status dentro do domínio e da API.
- Princípio III (Segurança por Design): PASS
  - Não impacta autenticação/autorização; não há relaxamento de regras de segurança.
- Princípio V (Integridade e Rastreabilidade): PASS
  - Enum reforça integridade sem perder rastreabilidade de dados e compatibilidade com o banco.
- Princípio VII (Documentação como Fonte de Verdade): PASS
  - A feature está alinhada com o contexto de ordem de serviço e com as convenções de schema do projeto.

### Pós-Design Gate

- PASS sem violações: os artefatos de pesquisa, modelo de dados, contrato e quickstart seguem a constituição e o padrão do backend `oficina-motos-api`.

## Project Structure

### Documentation (this feature)

```text
specs/006-ordem-servico-status/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── ordem-servico-status-contract.md
├── checklists/
│   └── requirements.md
└── spec.md
```

### Source Code (repository root)

```text
oficina-motos-api/
├── src/
│   ├── OficinaMotos.API/
│   │   └── Controllers/
│   │       └── OrdemServico/
│   ├── OficinaMotos.Application/
│   │   └── DTOs/
│   │       ├── Requests/OrdemServico/
│   │       └── Responses/OrdemServico/
│   ├── OficinaMotos.Domain/
│   │   ├── Entities/
│   │   │   └── OrdemServico.cs
│   │   ├── Enums/
│   │   │   └── OrdemServicoStatus.cs
│   │   └── Common/
│   │       └── BaseEntity.cs
│   └── OficinaMotos.Infrastructure/
│       ├── Context/
│       │   └── OficinaContext.cs
│       ├── EntitiesConfiguration/
│       │   └── OrdemServicoConfig/
│       │       └── OrdemServicoConfigurations.cs
│       └── Migrations/
├── OficinaMotos.slnx
└── README.md
```

**Structure Decision**: a implementação será backend-only, concentrada na API e no EF Core, com foco em `OficinaMotos.Domain`, `OficinaMotos.Application` e `OficinaMotos.Infrastructure`; o frontend não precisa ser alterado nesta sprint porque o requisito é de modelagem de domínio e persistência.

## Complexity Tracking

Sem violações de constituição que exijam justificação.
