# Implementation Plan: Reativar Soft Delete em BaseEntity

**Branch**: `[005-reativar-soft-delete-baseentity]` | **Date**: 2026-08-04 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/005-reativar-soft-delete-baseentity/spec.md`

## Summary

Reativar exclusão lógica padronizada no backend da API para todas as entidades derivadas de `BaseEntity`, adicionando os campos `IsDeleted` e `DeletedAt`, aplicando query filter global no `OficinaContext` e mantendo a semântica dos endpoints DELETE como remoção lógica via repositório base. A abordagem preserva compatibilidade funcional, rastreabilidade de dados e rollback de schema por migration reversível.

## Technical Context

**Language/Version**: C# 12 em .NET 8 (`net8.0`)

**Primary Dependencies**: ASP.NET Core Web API, Entity Framework Core 8.0.22, Pomelo.EntityFrameworkCore.MySql 8.0.3

**Storage**: MySQL 8+ (InnoDB, utf8mb4) via EF Core migrations

**Testing**: `dotnet build` + validação de migration/rollback + smoke test de endpoints DELETE e consultas GET afetadas

**Target Platform**: Backend ASP.NET Core hospedável em Windows/Linux

**Project Type**: Web service monolítico em Clean Architecture (API/Application/Domain/Infrastructure)

**Performance Goals**: Manter latência e throughput atuais de leitura sem degradação perceptível após filtro global

**Constraints**: Não quebrar contratos REST atuais; manter reversibilidade da migration; não introduzir hard delete por engano

**Scale/Scope**: Mudança transversal nas entidades que herdam `BaseEntity` nos contextos Cadastro, OS, Estoque e Financeiro

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Pré-Research Gate

- Princípio I (Domínio Primeiro): PASS
  - Regra permanece na camada Domain/Infrastructure (`BaseEntity`, `Repository`, `OficinaContext`), sem lógica de negócio em controllers.
- Princípio II (API RESTful Versionada): PASS
  - Endpoints `/api/v1/*` existentes serão mantidos, alterando apenas semântica interna de persistência.
- Princípio III (Segurança por Design): PASS
  - Sem relaxamento de autenticação/autorização; comportamento de exclusão não interfere em RBAC.
- Princípio V (Integridade e Rastreabilidade): PASS
  - Soft delete reforça retenção histórica e rastreabilidade de dados.
- Princípio VII (Documentação como Fonte de Verdade): PASS
  - Artefatos da feature gerados em `specs/005-reativar-soft-delete-baseentity/`.

### Pós-Design Gate

- PASS sem violações: `research.md`, `data-model.md`, `contracts/soft-delete-contract.md` e `quickstart.md` mantêm aderência aos princípios constitucionais.

## Project Structure

### Documentation (this feature)

```text
specs/005-reativar-soft-delete-baseentity/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── soft-delete-contract.md
└── tasks.md
```

### Source Code (repository root)

```text
oficina-motos-api/
├── src/
│   ├── OficinaMotos.API/
│   │   └── Controllers/
│   ├── OficinaMotos.Application/
│   ├── OficinaMotos.Domain/
│   │   ├── Common/
│   │   │   └── BaseEntity.cs
│   │   └── Interfaces/Repositories/
│   │       └── IRepository.cs
│   └── OficinaMotos.Infrastructure/
│       ├── Context/
│       │   └── OficinaContext.cs
│       ├── Repositories/
│       │   └── Repository.cs
│       ├── EntitiesConfiguration/
│       └── Migrations/
└── OficinaMotos.slnx
```

**Structure Decision**: Implementar a feature exclusivamente no backend `oficina-motos-api`, concentrando alterações em Domain + Infrastructure (modelo base, contexto EF, repositório base e migrations), preservando contratos da camada API.

## Complexity Tracking

Sem violações de constituição que exijam justificativa.
