# Implementation Plan: US-007 — Adicionar VeiculoId à entidade OrdemServico

**Branch**: `[007-veiculoid-ordem-servico]` | **Date**: 2026-08-12 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/007-veiculoid-ordem-servico/spec.md`

## Summary

Adicionar a referência de veículo na entidade `OrdemServico`, incluindo a propriedade `VeiculoId` e a navegação `Veiculo`, configurar a relação foreign key no EF Core para `cad_veiculos`, atualizar o DTO de criação para exigir `VeiculoId` e registrar uma migração reversível `AddVeiculoIdToOrdemServico` que preserve a integridade das ordens existentes.

## Technical Context

**Language/Version**: C# 12 em .NET 8 (`net8.0`)

**Primary Dependencies**: ASP.NET Core Web API, Entity Framework Core 8, Pomelo.EntityFrameworkCore.MySql 8

**Storage**: MySQL 8+ (InnoDB, utf8mb4) via EF Core migrations

**Testing**: `dotnet build`, verificação de geração de migração, validação do contrato de criação e smoke test de criação de ordem de serviço

**Target Platform**: Backend ASP.NET Core em ambiente Windows/Linux

**Project Type**: Web service em Clean Architecture (API / Application / Domain / Infrastructure)

**Performance Goals**: manter a mesma latência de criação e consulta de ordens de serviço sem degradação funcional

**Constraints**: relacionamento obrigatório com veículo válido; migração reversível; compatibilidade com dados antigos sem perda de referência; manter padrão de SQL e mapeamento do projeto

**Scale/Scope**: alteração transversal na entidade `OrdemServico`, no `CreateOrdemServicoDTO`, no mapeamento do EF Core e na migração de banco

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Pré-Research Gate

- Princípio I (Domínio Primeiro): PASS
  - a alteração permanece no domínio e no mapeamento do EF Core, sem lógica de negócio espalhada em controllers
- Princípio II (API RESTful Versionada): PASS
  - o recurso continua dentro do mesmo contrato da API e a mudança é construtiva, não de quebra de versão
- Princípio III (Segurança por Design): PASS
  - sem relaxamento de autenticação/autorização; a mudança não altera permissões nem fluxo de segurança
- Princípio V (Integridade e Rastreabilidade): PASS
  - a chave estrangeira explicitamente configurada fortalece a integridade referencial entre ordem e veículo
- Princípio VII (Documentação como Fonte de Verdade): PASS
  - a feature está alinhada ao contexto de cadastro e operação da oficina, com documentação SQL e mapeamento de entidades já existentes

### Pós-Design Gate

- PASS sem violações: os artefatos de pesquisa, modelo de dados, contrato e quickstart seguem a constituição e o padrão do backend `oficina-motos-api`.

## Project Structure

### Documentation (this feature)

```text
specs/007-veiculoid-ordem-servico/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── ordem-servico-veiculo-contract.md
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
│   │   │   ├── OrdemServico.cs
│   │   │   └── Veiculo.cs
│   │   └── Common/
│   │       └── BaseEntity.cs
│   └── OficinaMotos.Infrastructure/
│       ├── Context/
│       │   └── OficinaContext.cs
│       ├── EntitiesConfiguration/
│       │   ├── OrdemServicoConfig/
│       │   │   └── OrdemServicoConfigurations.cs
│       │   └── VeiculoConfig/
│       │       └── VeiculoConfigurations.cs
│       └── Migrations/
├── OficinaMotos.slnx
└── README.md
```

**Structure Decision**: a alteração será concentrada no backend `oficina-motos-api`, especialmente em `OficinaMotos.Domain`, `OficinaMotos.Application.DTOs.Requests.OrdemServico` e `OficinaMotos.Infrastructure.EntitiesConfiguration.OrdemServicoConfig`, com uso do EF Core para aplicar e registrar a migração em `OficinaMotos.Infrastructure/Migrations`.

## Complexity Tracking

Sem violações de constituição que exijam justificação.
