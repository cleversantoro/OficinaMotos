# Modelo de Dados e Contratos de Teste: ClienteService e OrdemServicoService (US-027 & US-028)

**Branch**: `026-cliente-os-unit-tests` | **Data**: 2026-11-16

## 1. Exceção de Domínio `DomainException`

**Caminho**: `oficina-motos-api/src/OficinaMotos.Domain/Exceptions/DomainException.cs`

```csharp
using System;

namespace OficinaMotos.Domain.Exceptions
{
    public class DomainException : InvalidOperationException
    {
        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
```

## 2. Contratos de `ClienteService` Testados em `ClienteServiceTests` (`US-027`)

| Teste (`ClienteServiceTests`) | Entrada / Estado | Comportamento Esperado |
| :--- | :--- | :--- |
| `Create_PF_WithValidCpf_Succeeds` (`T-027.1`) | `CreateClienteDTO` com `Nome = "João da Silva"`, `Cpf = "12345678909"`, `Email = "joao@email.com"`, `Telefone = "11999998888"` | Retorna `ClienteResponseDTO` não nulo com `Id > 0`, `Nome = "João da Silva"`, `Documento = "12345678909"`, `Tipo = (int)ClienteTipo.PessoaFisica`, `Status = (int)ClienteStatus.Ativo`; registro persistido com `IsDeleted = false` |
| `Create_DuplicateCpf_ThrowsDomainException` (`T-027.2`) | Cliente ativo já existente com `Documento = "12345678909"` + nova chamada a `CreateAsync` com o mesmo `Cpf` | Lança `DomainException` com mensagem indicando duplicidade de CPF e não persiste segundo registro |
| `Delete_SetsIsDeletedTrue_NotPhysicalDelete` (`T-027.3`) | Cliente ativo persistido no `OficinaContext` (`InMemory`) + chamada a `DeleteAsync(id)` | Retorna `true`; consulta com `context.Clientes.IgnoreQueryFilters().SingleOrDefaultAsync(c => c.Id == id)` encontra o registro fisicamente com `IsDeleted == true` e `DeletedAt != null` |
| `GetById_DeletedClient_ReturnsNull` (`T-027.4`) | Cliente excluído logicamente via `DeleteAsync(id)` + chamada a `GetByIdAsync(id)` | Retorna `null` por ação do filtro global `!IsDeleted` de `BaseEntity` |

## 3. Contratos de `IOrdemServicoService` / `OrdemServicoService` (`US-028`)

### 3.1 Novos Métodos em `IOrdemServicoService`
```csharp
Task<OrdemServicoResponseDTO?> ChangeStatusAsync(long id, OrdemServicoStatus novoStatus);
decimal CalculateTotal(Domain.Entities.OrdemServico ordem);
Task<decimal> CalculateTotalAsync(long id);
```

### 3.2 Matriz de Transição de `OrdemServicoStatus` (`T-028.2`, `T-028.3`)
| Status Atual | Transições Válidas (`T-028.2`) | Transições Inválidas (`T-028.3` → `DomainException`) |
| :--- | :--- | :--- |
| `Aberta (1)` | `EmAndamento`, `AguardandoPeca`, `Cancelada` | `Concluida` |
| `EmAndamento (2)` | `AguardandoPeca`, `Concluida`, `Cancelada` | `Aberta` |
| `AguardandoPeca (3)` | `EmAndamento`, `Concluida`, `Cancelada` | `Aberta` |
| `Concluida (4)` | *(nenhuma — estado terminal)* | `Aberta`, `EmAndamento`, `AguardandoPeca`, `Cancelada` |
| `Cancelada (5)` | *(nenhuma — estado terminal)* | `Aberta`, `EmAndamento`, `AguardandoPeca`, `Concluida` |

### 3.3 Métodos Testados em `OrdemServicoServiceTests` (`US-028`)
| Teste (`OrdemServicoServiceTests`) | Entrada / Estado | Comportamento Esperado |
| :--- | :--- | :--- |
| `Create_WithoutVeiculoId_ThrowsValidationException` (`T-028.1`) | `CreateOrdemServicoDTO` com `ClienteId = 1`, `MecanicoId = 1`, `VeiculoId = 0`, `DescricaoProblema = "Revisão"` | Lança `FluentValidation.ValidationException` indicando erro na propriedade `VeiculoId` e não chama `AddAsync` no repositório |
| `ChangeStatus_ValidTransition_Succeeds` (`T-028.2`) | `OrdemServico` com `Status = OrdemServicoStatus.Aberta` transicionando para `EmAndamento` (e/ou `EmAndamento` → `Concluida`) | Atualiza `Status` com sucesso, define `DataConclusao` quando `Concluida`, persiste via `UpdateAsync` e retorna `OrdemServicoResponseDTO` atualizado |
| `ChangeStatus_InvalidTransition_ThrowsException` (`T-028.3`) | `OrdemServico` em estado `Cancelada` ou `Concluida` (ou `Aberta` → `Concluida`) tentando transicionar para estado inválido | Lança `DomainException` (`InvalidOperationException`) e não persiste alteração |
| `CalculateTotal_WithPecasAndServicos_IsCorrect` (`T-028.4`) | `OrdemServico` contendo itens de peças (`PecaId != null`, ex.: 2 × R$ 85,00) e serviços (`PecaId == null`, ex.: 1 × R$ 130,00) | `CalculateTotal` e `CalculateTotalAsync` retornam exatamente `300.00m` |
