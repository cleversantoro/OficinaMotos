# Especificação de Funcionalidade: Testes Unitários de ClienteService e OrdemServicoService (US-027 & US-028)

**Feature Branch**: `026-cliente-os-unit-tests`

**Created**: 2026-11-16

**Status**: Draft

**Input**: User description: "US-027 — Testes unitários de ClienteService | Prioridade: 🟡 Should | Estimativa: M | Sprint: 5 | Depende: US-025, US-005 | Tasks: [ ] T-027.1 — Create_PF_WithValidCpf_Succeeds, [ ] T-027.2 — Create_DuplicateCpf_ThrowsDomainException, [ ] T-027.3 — Delete_SetsIsDeletedTrue_NotPhysicalDelete, [ ] T-027.4 — GetById_DeletedClient_ReturnsNull || US-028 — Testes unitários de OrdemServicoService | Prioridade: 🟡 Should | Estimativa: M | Sprint: 5 | Depende: US-025, US-006, US-007 | Tasks: [ ] T-028.1 — Create_WithoutVeiculoId_ThrowsValidationException, [ ] T-028.2 — ChangeStatus_ValidTransition_Succeeds, [ ] T-028.3 — ChangeStatus_InvalidTransition_ThrowsException, [ ] T-028.4 — CalculateTotal_WithPecasAndServicos_IsCorrect"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Validar Regras de Negócio e Exclusão Lógica no `ClienteService` com Testes Unitários (Priority: P1)

Como engenheiro de software do backend, quero dispor de uma suíte completa de testes unitários para `ClienteService` (`ClienteServiceTests`) em `OficinaMotos.Tests`, validando o cadastro de cliente Pessoa Física com CPF válido (`T-027.1`), o bloqueio de cadastro com CPF duplicado mediante exceção de domínio `DomainException` (`T-027.2`), a exclusão lógica (`IsDeleted = true` sem remoção física no banco) (`T-027.3`) e a omissão de clientes excluídos logicamente na busca por identificador (`GetByIdAsync` retornando `null`) (`T-027.4`).

**Why this priority**: Cumpre integralmente a `US-027` (`T-027.1` a `T-027.4`), assegurando a integridade cadastral do Bounded Context de Cadastro (unicidade de CPF e preservação de histórico via Soft Delete conforme `US-005` e Princípios V e VI da Constituição).

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~ClienteServiceTests"` a partir de `oficina-motos-api/` e verificar a aprovação dos 4 testes canônicos (`Create_PF_WithValidCpf_Succeeds`, `Create_DuplicateCpf_ThrowsDomainException`, `Delete_SetsIsDeletedTrue_NotPhysicalDelete` e `GetById_DeletedClient_ReturnsNull`).

**Acceptance Scenarios**:

1. **Given** os dados válidos de um novo cliente Pessoa Física (`CreateClienteDTO` com `Nome`, `Cpf` válido e inédito, `Email` e `Telefone`), **When** `Create_PF_WithValidCpf_Succeeds` (`T-027.1`) executa `ClienteService.CreateAsync`, **Then** o cliente é persistido com `Tipo = ClienteTipo.PessoaFisica`, `Status = ClienteStatus.Ativo`, `Documento` igual ao CPF informado e `IsDeleted = false`, retornando um `ClienteResponseDTO` preenchido com `Id > 0`.
2. **Given** um cliente ativo já cadastrado com determinado CPF no repositório, **When** `Create_DuplicateCpf_ThrowsDomainException` (`T-027.2`) executa `ClienteService.CreateAsync` informando o mesmo CPF, **Then** a operação é rejeitada antes da persistência lançando `DomainException` com mensagem indicando que já existe cliente cadastrado com o CPF informado.
3. **Given** um cliente ativo previamente persistido no `OficinaContext` em memória, **When** `Delete_SetsIsDeletedTrue_NotPhysicalDelete` (`T-027.3`) executa `ClienteService.DeleteAsync(id)`, **Then** o método retorna `true` e a consulta direta com `IgnoreQueryFilters()` comprova que o registro permanece fisicamente na tabela com `IsDeleted = true` e `DeletedAt` preenchido (`US-005`).
4. **Given** um cliente que sofreu exclusão lógica (`DeleteAsync(id)` executado), **When** `GetById_DeletedClient_ReturnsNull` (`T-027.4`) executa `ClienteService.GetByIdAsync(id)`, **Then** o método retorna `null` devido à aplicação do filtro global `!IsDeleted` de `BaseEntity`.

---

### User Story 2 - Validar Criação, Transições de Status e Cálculo de Total no `OrdemServicoService` com Testes Unitários (Priority: P1)

Como engenheiro de software do backend, quero dispor de uma suíte completa de testes unitários para `OrdemServicoService` (`OrdemServicoServiceTests`) em `OficinaMotos.Tests`, validando a exigência de `VeiculoId` na abertura da OS (`T-028.1`), a mudança de status em transições válidas (`T-028.2`), o bloqueio de transições de status inválidas com lançamento de exceção (`T-028.3`) e o cálculo exato do valor total da Ordem de Serviço combinando peças e serviços (`T-028.4`).

**Why this priority**: Cumpre integralmente a `US-028` (`T-028.1` a `T-028.4`), protegendo o fluxo operacional central da oficina (Bounded Context de Ordem de Serviço) e garantindo que os contratos introduzidos em `US-006` (`OrdemServicoStatus`) e `US-007` (`VeiculoId` obrigatório) permaneçam cobertos por testes automatizados.

**Independent Test**: Executar `dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~OrdemServicoServiceTests"` a partir de `oficina-motos-api/` e verificar a aprovação dos 4 testes canônicos (`Create_WithoutVeiculoId_ThrowsValidationException`, `ChangeStatus_ValidTransition_Succeeds`, `ChangeStatus_InvalidTransition_ThrowsException` e `CalculateTotal_WithPecasAndServicos_IsCorrect`).

**Acceptance Scenarios**:

1. **Given** uma requisição `CreateOrdemServicoDTO` sem vínculo válido de veículo (`VeiculoId <= 0`), **When** `Create_WithoutVeiculoId_ThrowsValidationException` (`T-028.1`) executa `OrdemServicoService.CreateAsync`, **Then** o serviço interrompe a criação antes de chamar o repositório e lança `ValidationException` (`FluentValidation.ValidationException`) apontando a falha de validação em `VeiculoId`.
2. **Given** uma Ordem de Serviço existente em estado `Aberta` (ou `EmAndamento`), **When** `ChangeStatus_ValidTransition_Succeeds` (`T-028.2`) aciona a transição de status para um estado subsequente válido (ex.: `Aberta` → `EmAndamento` ou `EmAndamento` → `Concluida`), **Then** o novo `OrdemServicoStatus` é aplicado e persistido com sucesso (preenchendo `DataConclusao` quando `Concluida`).
3. **Given** uma Ordem de Serviço em estado terminal (`Concluida` ou `Cancelada`) ou diante de uma transição proibida pelas regras do ciclo de vida da OS, **When** `ChangeStatus_InvalidTransition_ThrowsException` (`T-028.3`) tenta alterar o status para um estado inválido (ex.: `Cancelada` → `EmAndamento` ou `Concluida` → `Aberta`), **Then** o serviço rejeita a operação e lança exceção (`DomainException` / `InvalidOperationException`) sem persistir a alteração.
4. **Given** uma Ordem de Serviço contendo itens de peças (`PecaId` preenchido, ex.: 2 unidades a R$ 85,00 = R$ 170,00) e itens de serviços/mão de obra (`PecaId == null`, ex.: 1 serviço a R$ 130,00 = R$ 130,00), **When** `CalculateTotal_WithPecasAndServicos_IsCorrect` (`T-028.4`) calcula o total da OS, **Then** o valor total retornado corresponde exatamente à soma de peças e serviços (`R$ 300,00`).

---

### Edge Cases

- **Compatibilidade da hierarquia de `DomainException`**: `DomainException` deve herdar de `InvalidOperationException` (que herda de `Exception`), garantindo que `ClientesController` (que captura `Exception` e retorna `400 BadRequest`) e `GlobalExceptionHandler` continuem funcionando sem quebra de contrato, ao mesmo tempo em que testes unitários podem asserir especificamente `ThrowsAsync<DomainException>()` ou `ThrowsAsync<InvalidOperationException>()`.
- **Itens de OS com `Total` pré-calculado vs. `Quantidade * ValorUnitario`**: No cálculo do total da Ordem de Serviço (`T-028.4`), caso um `OrdemServicoItem` possua `Total > 0` explícito ou apenas `Quantidade` e `ValorUnitario` informados (`Total == 0`), a regra de cálculo deve considerar `i.Total > 0 ? i.Total : i.Quantidade * i.ValorUnitario`, mantendo consistência com a regra de quitação já utilizada em `RegistrarPagamentoAsync`.
- **Transições de status via `ChangeStatusAsync` e `UpdateAsync`**: A validação de transição de `OrdemServicoStatus` deve estar encapsulada de forma reutilizável tanto no método explícito de mudança de status (`ChangeStatusAsync`) quanto quando o status é modificado via `UpdateAsync`, evitando que estados terminais (`Concluida`, `Cancelada`) sejam revertidos inadvertidamente.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O projeto `OficinaMotos.Domain` DEVE disponibilizar a exceção de domínio `DomainException` (em `OficinaMotos.Domain.Exceptions.DomainException`, derivada de `InvalidOperationException`) para sinalizar violações de regras de negócio como duplicidade de CPF e transições inválidas de estado (`T-027.2`, `T-028.3`).
- **FR-002**: O método `ClienteService.CreateAsync(CreateClienteDTO request)` DEVE verificar a existência prévia de cliente com o mesmo CPF via `IClienteRepository.GetByCpfAsync` e lançar `DomainException` caso já exista registro ativo com o mesmo documento (`T-027.1`, `T-027.2`).
- **FR-003**: O método `ClienteService.DeleteAsync(long id)` DEVE realizar a exclusão lógica do cliente através do repositório (`IsDeleted = true`, `DeletedAt != null`), mantendo o registro físico na tabela e fazendo com que chamadas subsequentes a `ClienteService.GetByIdAsync(id)` retornem `null` (`T-027.3`, `T-027.4`).
- **FR-004**: O método `OrdemServicoService.CreateAsync(CreateOrdemServicoDTO request)` DEVE validar os dados obrigatórios da requisição (incluindo `VeiculoId > 0` conforme `CreateOrdemServicoValidator` / `US-007`) e lançar `FluentValidation.ValidationException` quando `VeiculoId` não for informado (`VeiculoId <= 0`) (`T-028.1`).
- **FR-005**: O `OrdemServicoService` (e `IOrdemServicoService`) DEVE suportar e validar a mudança de status de uma Ordem de Serviço (`ChangeStatusAsync(long id, OrdemServicoStatus novoStatus)` e validação em `UpdateAsync`), permitindo transições válidas do fluxo operacional (`Aberta` → `EmAndamento` / `AguardandoPeca` / `Cancelada`; `EmAndamento` ↔ `AguardandoPeca`; `EmAndamento` / `AguardandoPeca` → `Concluida` / `Cancelada`) e lançando exceção (`DomainException`) para transições inválidas (como reabrir uma OS `Concluida` ou `Cancelada`, ou concluir diretamente uma OS `Aberta` sem passar por execução) (`T-028.2`, `T-028.3`).
- **FR-006**: O `OrdemServicoService` (e `IOrdemServicoService`) DEVE disponibilizar o cálculo do valor total da Ordem de Serviço (`CalculateTotal(OrdemServico ordem)` e `CalculateTotalAsync(long ordemServicoId)`) somando corretamente todos os itens de peças (`PecaId != null`) e serviços (`PecaId == null`) da OS (`T-028.4`).
- **FR-007**: O projeto `OficinaMotos.Tests` DEVE conter a classe `ClienteServiceTests` em `oficina-motos-api/src/OficinaMotos.Tests/Services/ClienteServiceTests.cs` implementando os 4 testes unitários da `US-027`:
  - `Create_PF_WithValidCpf_Succeeds` (`T-027.1`);
  - `Create_DuplicateCpf_ThrowsDomainException` (`T-027.2`);
  - `Delete_SetsIsDeletedTrue_NotPhysicalDelete` (`T-027.3`);
  - `GetById_DeletedClient_ReturnsNull` (`T-027.4`).
- **FR-008**: O projeto `OficinaMotos.Tests` DEVE conter a classe `OrdemServicoServiceTests` em `oficina-motos-api/src/OficinaMotos.Tests/Services/OrdemServicoServiceTests.cs` implementando os 4 testes unitários da `US-028`:
  - `Create_WithoutVeiculoId_ThrowsValidationException` (`T-028.1`);
  - `ChangeStatus_ValidTransition_Succeeds` (`T-028.2`);
  - `ChangeStatus_InvalidTransition_ThrowsException` (`T-028.3`);
  - `CalculateTotal_WithPecasAndServicos_IsCorrect` (`T-028.4`).

### Key Entities *(include if feature involves data)*

- **`DomainException`** (`OficinaMotos.Domain.Exceptions`): Exceção de domínio lançada quando invariantes ou regras de negócio são violadas (ex.: CPF duplicado em `ClienteService`, transição inválida de status em `OrdemServicoService`).
- **`Cliente` / `ClienteService`** (`OficinaMotos.Domain.Entities` / `OficinaMotos.Application.Services.Cliente`): Entidade e serviço de aplicação responsáveis pelo ciclo de vida de clientes PF/PJ, unicidade de documento e exclusão lógica (`BaseEntity`).
- **`OrdemServico` / `OrdemServicoItem` / `OrdemServicoService`** (`OficinaMotos.Domain.Entities` / `OficinaMotos.Application.Services.OrdemServicoRepo`): Entidades e serviço de aplicação responsáveis pela abertura vinculada a `VeiculoId`, máquina de estados (`OrdemServicoStatus`) e totalização financeira de peças e serviços.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `dotnet test` em `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~ClienteServiceTests"` executa e aprova 100% dos 4 testes unitários de `ClienteService` (`T-027.1` a `T-027.4`).
- **SC-002**: `dotnet test` em `oficina-motos-api/src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~OrdemServicoServiceTests"` executa e aprova 100% dos 4 testes unitários de `OrdemServicoService` (`T-028.1` a `T-028.4`).
- **SC-003**: `dotnet test` em `oficina-motos-api/OficinaMotos.slnx` executa toda a suíte de testes existente (`Domain`, `AuthServiceTests`, `OrdemServicoPagamentoServiceTests`, `ClienteServiceTests` e `OrdemServicoServiceTests`) com 100% de aprovação e zero regressões.

## Assumptions

- Nenhuma alteração de schema de banco de dados ou nova migration do Entity Framework Core é necessária para `US-027` e `US-028`, pois `IsDeleted`/`DeletedAt` (`US-005`), `OrdemServicoStatus` (`US-006`) e `VeiculoId` (`US-007`) já estão presentes no modelo e nas migrations existentes.
- Os testes que validam o comportamento integrado de Soft Delete e filtros globais do EF Core (`T-027.3` e `T-027.4`) utilizam `TestDbContextFactory.Create()` (`US-025`) com `ClienteRepository` sobre o provedor `InMemory`, garantindo fidelidade real ao `OnModelCreating` do `OficinaContext`.
