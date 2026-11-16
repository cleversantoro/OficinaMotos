# Pesquisa Técnica: Testes Unitários de ClienteService e OrdemServicoService (US-027 & US-028)

**Branch**: `026-cliente-os-unit-tests` | **Data**: 2026-11-16 | **Spec**: [spec.md](file:///c:/Projetos/OficinaMotos/specs/026-cliente-os-unit-tests/spec.md)

## 1. Exceção de Domínio (`DomainException`) e `ClienteService` (`US-027`)

- **Análise do código atual**:
  - Em `ClienteService.CreateAsync(CreateClienteDTO request)` (`oficina-motos-api/src/OficinaMotos.Application/Services/Cliente/ClienteService.cs`), quando `_repository.GetByCpfAsync(request.Cpf)` encontra um registro, o código atual executa `throw new Exception("Ja existe um cliente cadastrado com este CPF.");`.
  - Em `GlobalExceptionHandler.cs`, já existe um comentário prevendo `// if (exception is DomainException) ...`.
  - Em `T-027.2`, o método de teste exige explicitamente `Create_DuplicateCpf_ThrowsDomainException`.
- **Decision**:
  - Criar `DomainException` em `OficinaMotos.Domain.Exceptions.DomainException` herdando de `InvalidOperationException` (que por sua vez herda de `Exception`).
  - Atualizar `ClienteService.CreateAsync` para lançar `new DomainException("Ja existe um cliente cadastrado com este CPF.")`.
  - Atualizar `GlobalExceptionHandler.TryHandleAsync` para retornar `400 BadRequest` quando `exception is DomainException or FluentValidation.ValidationException`.
- **Rationale**:
  - Como `DomainException : InvalidOperationException : Exception`, o `ClientesController` (`catch (Exception ex)`) mantém 100% de compatibilidade, testes que verificam `ThrowsAsync<DomainException>()` (`T-027.2`) passam e testes que verificam `ThrowsAsync<DomainException>()` ou `ThrowsAsync<InvalidOperationException>()` em transições inválidas de OS (`T-028.3`) também funcionam de maneira uniforme.
- **Alternatives considered**:
  - Manter `throw new Exception(...)` em `ClienteService`: rejeitado porque viola o nome e o propósito explícito de `T-027.2 — Create_DuplicateCpf_ThrowsDomainException` e boas práticas de DDD.

## 2. Testes de Soft Delete em `ClienteService` (`T-027.3` e `T-027.4`)

- **Análise do código atual**:
  - `Cliente` herda de `BaseEntity` (`IsDeleted`, `DeletedAt`, `Delete()`).
  - `Repository<T>.DeleteAsync(long id)` chama `SoftDeleteAsync(id)`, que executa `entity.Delete()`, `_dbSet.Update(entity)` e `_context.SaveChangesAsync()`.
  - `OficinaContext.OnModelCreating` aplica o filtro global `HasQueryFilter(e => !e.IsDeleted)` em todas as entidades derivadas de `BaseEntity`.
  - `ClienteRepository.GetByIdAsync(long id)` usa `_dbSet...AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)`, respeitando o filtro global de `OficinaContext`.
- **Decision**:
  - Nos testes `Delete_SetsIsDeletedTrue_NotPhysicalDelete` (`T-027.3`) e `GetById_DeletedClient_ReturnsNull` (`T-027.4`), utilizar `TestDbContextFactory.Create()` + `ClienteRepository(context)` + `TestDbContextFactory.CreateMapper()` injetados no `ClienteService`.
  - Em `T-027.3`, após `await sut.DeleteAsync(created.Id)`, consultar `context.Clientes.IgnoreQueryFilters().SingleOrDefaultAsync(c => c.Id == created.Id)` e asserir que a entidade não é nula, `IsDeleted == true` e `DeletedAt != null`.
  - Em `T-027.4`, após `await sut.DeleteAsync(created.Id)`, chamar `await sut.GetByIdAsync(created.Id)` e asserir que retorna `null`.
- **Rationale**:
  - Testar `ClienteService` integrado ao `ClienteRepository` com `TestDbContextFactory` (`EF Core InMemory`) valida de ponta a ponta a integração entre `ClienteService`, `ClienteRepository`, `BaseEntity.Delete()` e o Query Filter do `OficinaContext` (`US-005` + `US-025`).

## 3. Validação, Transição de Status e Cálculo de Total em `OrdemServicoService` (`US-028`)

- **Análise dos 4 cenários de `US-028`**:
  1. `T-028.1 — Create_WithoutVeiculoId_ThrowsValidationException`:
     - Atualmente, `CreateOrdemServicoValidator` já define `RuleFor(x => x.VeiculoId).GreaterThan(0);`, porém `OrdemServicoService.CreateAsync` não executava a validação antes de mapear e salvar no repositório.
     - Ao validar `request` no início de `OrdemServicoService.CreateAsync` (lançando `FluentValidation.ValidationException` quando `request == null` ou `request.VeiculoId <= 0`), garantimos a invariante de domínio/aplicação mesmo quando o serviço é chamado fora do pipeline MVC.
  2. `T-028.2 — ChangeStatus_ValidTransition_Succeeds` e `T-028.3 — ChangeStatus_InvalidTransition_ThrowsException`:
     - O enum `OrdemServicoStatus` (`US-006`) possui 5 estados: `Aberta (1)`, `EmAndamento (2)`, `AguardandoPeca (3)`, `Concluida (4)` e `Cancelada (5)`.
     - Regras de transição de estado no ciclo de vida da OS:
       - De `Aberta`: pode ir para `EmAndamento`, `AguardandoPeca` ou `Cancelada`. (Não pode ir diretamente para `Concluida` sem execução).
       - De `EmAndamento`: pode ir para `AguardandoPeca`, `Concluida` ou `Cancelada`.
       - De `AguardandoPeca`: pode ir para `EmAndamento`, `Concluida` ou `Cancelada`.
       - De `Concluida` (terminal): não pode transicionar para nenhum outro status.
       - De `Cancelada` (terminal): não pode transicionar para nenhum outro status.
     - Expor `Task<OrdemServicoResponseDTO?> ChangeStatusAsync(long id, OrdemServicoStatus novoStatus)` em `IOrdemServicoService` e `OrdemServicoService` (além de aplicar a validação em `UpdateAsync` quando `entity.Status != request.Status`). Quando `novoStatus == OrdemServicoStatus.Concluida`, preencher `entity.DataConclusao ??= DateTime.UtcNow`. Quando a transição for inválida, lançar `DomainException` (`InvalidOperationException`).
  3. `T-028.4 — CalculateTotal_WithPecasAndServicos_IsCorrect`:
     - Uma `OrdemServico` possui `ICollection<OrdemServicoItem> Itens`, onde cada item representa uma peça (`PecaId != null`) ou um serviço/mão de obra (`PecaId == null`), contendo `Quantidade`, `ValorUnitario` e `Total`.
     - Expor `decimal CalculateTotal(OrdemServico ordem)` e `Task<decimal> CalculateTotalAsync(long id)` em `IOrdemServicoService` e `OrdemServicoService`, calculando `ordem.Itens.Sum(i => i.Total > 0 ? i.Total : i.Quantidade * i.ValorUnitario)` e reutilizando esse método dentro de `RegistrarPagamentoAsync`.
- **Decision**:
  - Implementar validação com `FluentValidation.ValidationException` em `OrdemServicoService.CreateAsync`, métodos `ChangeStatusAsync` e `CalculateTotal`/`CalculateTotalAsync` em `OrdemServicoService` e `IOrdemServicoService`, e criar `OrdemServicoServiceTests.cs` cobrindo `T-028.1` a `T-028.4`.
- **Rationale**:
  - Satisfaz diretamente todas as tasks de `US-028`, elimina duplicação da fórmula de cálculo de total em `RegistrarPagamentoAsync` e fortalece a camada de aplicação contra estados inconsistentes.
