# Contratos de Teste: ClienteServiceTests e OrdemServicoServiceTests (US-027 & US-028)

**Branch**: `026-cliente-os-unit-tests` | **Data**: 2026-11-16

## 1. Contrato da Suíte `ClienteServiceTests` (`US-027`)
- **Caminho**: `oficina-motos-api/src/OficinaMotos.Tests/Services/ClienteServiceTests.cs`
- **Dependências**: `US-025` (`TestDbContextFactory`), `US-005` (`BaseEntity` Soft Delete)
- **Métodos de Teste Obrigatórios**:
  1. `[Fact] public async Task Create_PF_WithValidCpf_Succeeds()` (`T-027.1`)
  2. `[Fact] public async Task Create_DuplicateCpf_ThrowsDomainException()` (`T-027.2`)
  3. `[Fact] public async Task Delete_SetsIsDeletedTrue_NotPhysicalDelete()` (`T-027.3`)
  4. `[Fact] public async Task GetById_DeletedClient_ReturnsNull()` (`T-027.4`)

## 2. Contrato da Suíte `OrdemServicoServiceTests` (`US-028`)
- **Caminho**: `oficina-motos-api/src/OficinaMotos.Tests/Services/OrdemServicoServiceTests.cs`
- **Dependências**: `US-025` (`TestDbContextFactory`), `US-006` (`OrdemServicoStatus`), `US-007` (`VeiculoId` em `OrdemServico`)
- **Métodos de Teste Obrigatórios**:
  1. `[Fact] public async Task Create_WithoutVeiculoId_ThrowsValidationException()` (`T-028.1`)
  2. `[Fact] public async Task ChangeStatus_ValidTransition_Succeeds()` (`T-028.2`)
  3. `[Fact] public async Task ChangeStatus_InvalidTransition_ThrowsException()` (`T-028.3`)
  4. `[Fact] public async Task CalculateTotal_WithPecasAndServicos_IsCorrect()` (`T-028.4`)
