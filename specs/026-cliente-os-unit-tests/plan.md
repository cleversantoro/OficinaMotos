# Plano de Implementação: Testes Unitários de ClienteService e OrdemServicoService (US-027 & US-028)

**Branch**: `026-cliente-os-unit-tests` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/026-cliente-os-unit-tests/spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

**Entrada**:
- **US-027 — Testes unitários de ClienteService** (`Prioridade: 🟡 Should | Estimativa: M | Sprint: 5 | Depende: US-025, US-005`)
  - [ ] T-027.1 — `Create_PF_WithValidCpf_Succeeds`
  - [ ] T-027.2 — `Create_DuplicateCpf_ThrowsDomainException`
  - [ ] T-027.3 — `Delete_SetsIsDeletedTrue_NotPhysicalDelete`
  - [ ] T-027.4 — `GetById_DeletedClient_ReturnsNull`
- **US-028 — Testes unitários de OrdemServicoService** (`Prioridade: 🟡 Should | Estimativa: M | Sprint: 5 | Depende: US-025, US-006, US-007`)
  - [ ] T-028.1 — `Create_WithoutVeiculoId_ThrowsValidationException`
  - [ ] T-028.2 — `ChangeStatus_ValidTransition_Succeeds`
  - [ ] T-028.3 — `ChangeStatus_InvalidTransition_ThrowsException`
  - [ ] T-028.4 — `CalculateTotal_WithPecasAndServicos_IsCorrect`

---

## Resumo

Este plano detalha a implementação das suítes de testes unitários `ClienteServiceTests` (`US-027`) e `OrdemServicoServiceTests` (`US-028`) dentro do projeto `OficinaMotos.Tests` (`EPIC-05 — Testes e Qualidade`), bem como os ajustes nos contratos de domínio e aplicação necessários para suportar os 8 cenários canônicos: introdução de `DomainException` em `OficinaMotos.Domain.Exceptions`, uso de `DomainException` na verificação de CPF duplicado em `ClienteService`, validação de `VeiculoId` (`FluentValidation.ValidationException`), máquina de transições de `OrdemServicoStatus` (`ChangeStatusAsync` / `UpdateAsync`) e cálculo do total de peças e serviços (`CalculateTotal` / `CalculateTotalAsync`) em `OrdemServicoService`.

---

## Contexto Técnico

**Language/Version**: C# 12 / .NET 8.0 (`net8.0`)  
**Primary Dependencies**: `xunit` 2.9.3, `Moq` 4.20.72, `FluentAssertions` 6.12.2, `Microsoft.EntityFrameworkCore.InMemory` 8.0.22, `FluentValidation` 12.1.1, `AutoMapper` 16.1.1  
**Storage**: `EF Core InMemory` (`TestDbContextFactory.Create()`) com `ClienteRepository` / `OrdemServicoRepository` e mocks com `Moq`  
**Testing**: `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj`  
**Target Platform**: .NET 8 SDK / Test Host  
**Project Type**: Backend Web API (`OficinaMotos.Domain`, `OficinaMotos.Application`, `OficinaMotos.API`) + xUnit Test Project (`OficinaMotos.Tests`)  
**Performance Goals**: Execução das suítes `ClienteServiceTests` e `OrdemServicoServiceTests` em < 2 segundos em memória  
**Constraints**: Nenhuma alteração de schema MySQL; preservar compatibilidade total dos controllers REST (`ClientesController`, `OrdemServicosController`) e dos testes já existentes (`AuthServiceTests`, `OrdemServicoPagamentoServiceTests`, `SoftDeleteAndOrdemServicoContractTests`)  
**Scale/Scope**: 1 classe de exceção de domínio (`DomainException`), evolução de `ClienteService` e `OrdemServicoService`/`IOrdemServicoService`, e 2 novas classes de testes unitários (`ClienteServiceTests` e `OrdemServicoServiceTests`) totalizando no mínimo 8 novos testes canônicos

---

## Constitution Check

*GATE: Validado antes da Fase 0 e revalidado após o design da Fase 1.*

| Princípio da Constituição / Governança | Status | Justificativa / Verificação |
| :--- | :---: | :--- |
| **I. Domínio Primeiro (DDD)** | ✅ Pass | Regras de negócio (unicidade de CPF com `DomainException`, obrigatoriedade de `VeiculoId`, máquina de transição de `OrdemServicoStatus` e cálculo de total de itens) residem nas camadas `Domain` e `Application`, nunca em Controllers. |
| **II. API RESTful Versionada** | ✅ Pass | Mantém inalteradas as rotas `/api/v1/Clientes` e `/api/v1/OrdemServicos`; `DomainException` herda de `InvalidOperationException` (`Exception`), preservando o tratamento HTTP 400 em `ClientesController` e `GlobalExceptionHandler`. |
| **III. Segurança por Design (RBAC + LGPD)** | ✅ Pass | Nenhuma alteração em `[Authorize]` ou políticas RBAC nos controllers existentes. |
| **V. Integridade e Rastreabilidade de Dados** | ✅ Pass | Valida diretamente a exclusão lógica (`IsDeleted = true`, `DeletedAt != null`) e o filtro global `!IsDeleted` de `BaseEntity` (`US-005`), sem remoção física. |
| **VI. Qualidade e Testabilidade** | ✅ Pass | Cumpre diretamente o Princípio VI ao adicionar cobertura unitária para `ClienteService` (`US-027`) e `OrdemServicoService` (`US-028`). |
| **VII. Documentação como Fonte de Verdade** | ✅ Pass | Implementa integralmente as tasks `T-027.1`..`T-027.4` e `T-028.1`..`T-028.4` especificadas em `governance/backlog.md`. |

---

## Project Structure

### Documentation (this feature)

```text
specs/026-cliente-os-unit-tests/
├── spec.md              # Especificação de requisitos (US-027 e US-028)
├── plan.md              # Este arquivo (Plano de Implementação)
├── research.md          # Fase 0: Pesquisa técnica sobre DomainException, Soft Delete, Status e Total da OS
├── data-model.md        # Fase 1: Contratos de DomainException, ClienteService e OrdemServicoService
├── quickstart.md        # Fase 1: Guia rápido de execução de dotnet build e dotnet test
├── checklists/
│   └── requirements.md  # Checklist de qualidade da especificação
└── contracts/
    └── test-contracts.md # Fase 1: Contratos das suítes ClienteServiceTests e OrdemServicoServiceTests
```

### Source Code (repository root)

```text
oficina-motos-api/
└── src/
    ├── OficinaMotos.Domain/
    │   └── Exceptions/
    │       └── DomainException.cs                                 # [CRIAR] Exceção de domínio derivada de InvalidOperationException (T-027.2, T-028.3)
    ├── OficinaMotos.Application/
    │   ├── Interfaces/OrdemServico/
    │   │   └── IOrdemServicoService.cs                            # [MODIFICAR] Adicionar ChangeStatusAsync, CalculateTotal e CalculateTotalAsync (T-028.2..T-028.4)
    │   ├── Services/Cliente/
    │   │   └── ClienteService.cs                                  # [MODIFICAR] Lançar DomainException para CPF duplicado em CreateAsync (T-027.2)
    │   └── Services/OrdemServico/
    │       └── OrdemServicoService.cs                             # [MODIFICAR] Validar VeiculoId em CreateAsync (T-028.1), transições de status em ChangeStatusAsync/UpdateAsync (T-028.2, T-028.3) e cálculo de total em CalculateTotal/CalculateTotalAsync (T-028.4)
    ├── OficinaMotos.API/
    │   └── Middlewares/
    │       └── GlobalExceptionHandler.cs                          # [MODIFICAR] Mapear DomainException e ValidationException para 400 BadRequest
    └── OficinaMotos.Tests/
        └── Services/
            ├── ClienteServiceTests.cs                             # [CRIAR] Testes unitários de ClienteService (T-027.1, T-027.2, T-027.3, T-027.4)
            └── OrdemServicoServiceTests.cs                        # [CRIAR] Testes unitários de OrdemServicoService (T-028.1, T-028.2, T-028.3, T-028.4)
```

**Structure Decision**: Mantém a organização Clean Architecture em `oficina-motos-api/src/` e adiciona `ClienteServiceTests.cs` e `OrdemServicoServiceTests.cs` em `src/OficinaMotos.Tests/Services/`, ao lado de `AuthServiceTests.cs` e `OrdemServicoPagamentoServiceTests.cs`.

---

## Fases de Execução

### Fase 1: Contratos de Domínio e Evolução de `ClienteService` (`US-027`)
1. Criar `OficinaMotos.Domain/Exceptions/DomainException.cs` herdando de `InvalidOperationException`.
2. Atualizar `ClienteService.CreateAsync` para lançar `DomainException` quando `_repository.GetByCpfAsync(request.Cpf)` retornar um cliente existente (`T-027.2`), mantendo o mapeamento PF (`T-027.1`) e o comportamento de `DeleteAsync`/`GetByIdAsync` (`T-027.3`, `T-027.4`).
3. Criar `OficinaMotos.Tests/Services/ClienteServiceTests.cs` implementando:
   - `Create_PF_WithValidCpf_Succeeds` (`T-027.1`)
   - `Create_DuplicateCpf_ThrowsDomainException` (`T-027.2`)
   - `Delete_SetsIsDeletedTrue_NotPhysicalDelete` (`T-027.3`)
   - `GetById_DeletedClient_ReturnsNull` (`T-027.4`)

### Fase 2: Evolução de `OrdemServicoService` e Implementação de `OrdemServicoServiceTests` (`US-028`)
1. Atualizar `OrdemServicoService.CreateAsync` para validar o `CreateOrdemServicoDTO` (garantindo que `VeiculoId <= 0` lance `FluentValidation.ValidationException` conforme `CreateOrdemServicoValidator` / `T-028.1`).
2. Adicionar `ChangeStatusAsync(long id, OrdemServicoStatus novoStatus)` (e validação de transição quando o status mudar em `UpdateAsync`) em `IOrdemServicoService` e `OrdemServicoService`, permitindo transições válidas (`T-028.2`) e lançando `DomainException` (`InvalidOperationException`) para transições inválidas (`T-028.3`).
3. Adicionar `CalculateTotal(OrdemServico ordem)` e `CalculateTotalAsync(long id)` em `IOrdemServicoService` e `OrdemServicoService` somando itens de peças e serviços (`T-028.4`).
4. Criar `OficinaMotos.Tests/Services/OrdemServicoServiceTests.cs` implementando:
   - `Create_WithoutVeiculoId_ThrowsValidationException` (`T-028.1`)
   - `ChangeStatus_ValidTransition_Succeeds` (`T-028.2`)
   - `ChangeStatus_InvalidTransition_ThrowsException` (`T-028.3`)
   - `CalculateTotal_WithPecasAndServicos_IsCorrect` (`T-028.4`)

### Fase 3: Validação e Governança
1. Executar `dotnet build OficinaMotos.slnx` e `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj`.
2. Atualizar [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) na fase `/speckit.implement`.

---

## Complexity Tracking

Nenhuma violação da Constituição foi identificada. A tabela de desvios de complexidade não se aplica.
