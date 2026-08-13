# Feature Specification: US-007 — Adicionar VeiculoId à entidade OrdemServico

**Feature Branch**: `[007-veiculoid-ordem-servico]`

**Created**: 2026-08-12

**Status**: Draft

**Input**: User description: "US-007 — Adicionar VeiculoId à entidade OrdemServico\n**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 1\n\n**Critério de aceite:**\n\n- `OrdemServico` possui `long VeiculoId` (FK para `cad_veiculos`)\n- `CreateOrdemServicoDto` exige `VeiculoId`\n- Migration criada e reversível\n**Tasks:**\n\n- [ ] T-007.1 — Adicionar `long VeiculoId` e `Veiculo? Veiculo` em `OrdemServico.cs`\n- [ ] T-007.2 — Configurar FK no `OnModelCreating`\n- [ ] T-007.3 — Criar migration `AddVeiculoIdToOrdemServico`\n- [ ] T-007.4 — Atualizar `CreateOrdemServicoDto` com `[Required] long VeiculoId`"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Vinculação correta do veículo à ordem de serviço (Priority: P1)

Como responsável pela operação da oficina, quero que cada ordem de serviço esteja vinculada a um veículo específico, para que o histórico de atendimento fique consistente e seja possível rastrear o serviço prestado ao cliente correto.

**Why this priority**: Esta é a base da operação de atendimento. Sem a ligação explícita entre a ordem e o veículo, a oscilação de informações e o rastreio do serviço ficam inconsistentes e prejudicam planejamento e atendimento.

**Independent Test**: Pode ser validado ao criar uma ordem de serviço e confirmar que o sistema exige a referência ao veículo antes de permitir o registro da OS.

**Acceptance Scenarios**:

1. **Given** uma nova ordem de serviço em processo de cadastro, **When** o usuário informa o veículo correspondente, **Then** o sistema deve registrar a referência do veículo na ordem.
2. **Given** uma ordem de serviço já criada, **When** a operação da oficina consulta o atendimento, **Then** o veículo associado deve estar claramente identificado na estrutura da ordem.
3. **Given** uma tentativa de criar uma ordem de serviço sem informar o veículo, **When** a validação processa o pedido, **Then** o sistema deve rejeitar a operação antes de persistir o registro.

---

### User Story 2 - Validação e integridade de dados da API (Priority: P2)

Como cliente da API e usuário interno do processo, quero que o contrato de criação da ordem de serviço exija a referência do veículo, para que os dados enviados sejam consistentes e a aplicação não aceite registros incompletos.

**Why this priority**: O contrato de entrada precisa ser explícito para evitar que dados inválidos entrem no sistema. Isso reduz retrabalho, inconsistência de registros e erros no atendimento.

**Independent Test**: Pode ser validado ao enviar um payload de criação sem `VeiculoId` e verificar que a API recusa a operação, enquanto um payload completo é aceito.

**Acceptance Scenarios**:

1. **Given** um payload válido para criação da ordem de serviço, **When** o sistema processa a requisição, **Then** a operação deve ser aceita somente quando `VeiculoId` estiver presente.
2. **Given** um payload sem `VeiculoId`, **When** a API valida os dados, **Then** a solicitação deve ser rejeitada com erro de validação.
3. **Given** uma ordem de serviço em leitura ou consulta, **When** o retorno da API é produzido, **Then** a referência ao veículo deve ser exposta de forma consistente no contrato de resposta.

---

### User Story 3 - Evolução segura do banco e reversão de migração (Priority: P3)

Como responsável pela manutenção do sistema, quero que a alteração de estrutura do banco seja conduzida por uma migração segura e reversível, para que a mudança possa ser aplicada e revertida sem perda de integridade dos dados existentes.

**Why this priority**: A inclusão de uma chave estrangeira exige cuidado para garantir consistência histórica e permitir ajustes em ambientes de homologação e produção sem risco de corrupção de dados.

**Independent Test**: Pode ser validado em ambiente de migração verificando que a coluna e o relacionamento foram adicionados ao modelo e que a migração pode ser revertida de forma controlada.

**Acceptance Scenarios**:

1. **Given** um banco sem a coluna de vínculo do veículo, **When** a migração é aplicada, **Then** a estrutura deve ser criada sem quebrar o restante do modelo.
2. **Given** uma migração aplicada em ambiente controlado, **When** o processo de reversão é executado, **Then** a alteração deve ser removida de forma reversível e sem perda de informações fora do escopo da mudança.

---

### Edge Cases

- O que acontece quando o payload de criação da ordem de serviço envia `VeiculoId` vazio ou em formato inválido?
- Como o sistema lida com ordens de serviço existentes que ainda não possuem referência de veículo?
- O que acontece quando a chave estrangeira aponta para um veículo inexistente no banco?
- Como a migração deve se comportar em cenários de base legada que não contém registros de veículos relacionados?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema MUST manter a propriedade `VeiculoId` na entidade `OrdemServico`, usando o tipo `long` como identificador da referência ao veículo na tabela `cad_veiculos`.
- **FR-002**: A entidade `OrdemServico` MUST incluir a navegação `Veiculo` como relacionamento opcional para representar a associação ao veículo vinculado.
- **FR-003**: O relacionamento entre `OrdemServico` e `Veiculo` MUST ser configurado no `OnModelCreating` como chave estrangeira explícita, preservando a integridade referencial do domínio de cadastro.
- **FR-004**: O sistema MUST garantir que cada ordem de serviço seja vinculada a um veículo válido antes de ser persistida em um fluxo de criação.
- **FR-005**: O DTO de criação `CreateOrdemServicoDto` MUST exigir a propriedade `VeiculoId` como campo obrigatório em todas as operações de criação.
- **FR-006**: O contrato de criação MUST rejeitar payloads sem `VeiculoId`, impedindo registros incompletos e inconsistentes.
- **FR-007**: O sistema MUST incluir uma migração de banco nomeada `AddVeiculoIdToOrdemServico`, com atualização e reversão adequados para a nova coluna e relacionamento.
- **FR-008**: A migração MUST preservar os dados já existentes, adicionando a referência do veículo de forma controlada e sem quebrar o restante do modelo de dados.
- **FR-009**: O modelo de domínio MUST manter a associação entre ordem de serviço e veículo rastreável, sem perder o vínculo em operações futuras de consulta e manutenção.

### Key Entities *(include if feature involves data)*

- **OrdemServico**: Representa a ordem de serviço prestada ao cliente e inclui a referência ao veículo que originou ou recebeu o atendimento.
- **Veiculo**: Entidade de cadastro do veículo, que serve como referência principal para a vinculação da ordem de serviço.
- **CreateOrdemServicoDto**: Estrutura de entrada usada para criação de ordens de serviço, com validação obrigatória da referência ao veículo.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% das ordens de serviço criadas no sistema passam a conter uma referência válida de veículo.
- **SC-002**: 100% das requisições de criação de ordem de serviço com `VeiculoId` ausente são rejeitadas por validação antes da persistência.
- **SC-003**: A migração de adição do vínculo do veículo é aplicada com sucesso em ambientes de atualização sem perda de registros existentes e sem interrupção do fluxo principal de atendimento.
- **SC-004**: Os dados de ordem de serviço e veículo permanecem consistentes em consulta, manutenção e histórico do atendimento.

## Assumptions

- A relação entre ordem de serviço e veículo será tratada como parte do contexto de cadastro e operação da oficina.
- O veículo relacionado já existe no sistema e possui identificação estável por `long`.
- A alteração será aplicada no modelo de domínio e no banco sem impacto na regra de negócio de outras entidades do sistema.
- A migração será necessária para ambientes que já possuem ordens de serviço registradas e para futuras implementações de histórico e consulta.
