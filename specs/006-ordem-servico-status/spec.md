# Feature Specification: US-006 — Criar enum OrdemServicoStatus

**Feature Branch**: `[006-ordem-servico-status]`

**Created**: 2026-08-12

**Status**: Draft

**Input**: User description: "US-006 — Criar enum OrdemServicoStatus\n**Prioridade:** 🔴 Must | **Estimativa:** S | **Sprint:** 1\n\n**Critério de aceite:**\n\n- Enum `OrdemServicoStatus`: `Aberta=1`, `EmAndamento=2`, `AguardandoPeca=3`, `Concluida=4`, `Cancelada=5`\n- Entidade `OrdemServico` usa o enum (substituindo `string Status`)\n- DTOs refletem o enum\n- Migration criada\n**Tasks:**\n\n- [ ] T-006.1 — Criar `OrdemServicoStatus.cs` em `OficinaMotos.Domain/Enums/`\n- [ ] T-006.2 — Atualizar propriedade `Status` em `OrdemServico.cs`\n- [ ] T-006.3 — Configurar `HasConversion<string>()` no DbContext\n- [ ] T-006.4 — Criar migration `AddOrdemServicoStatusEnum`\n- [ ] T-006.5 — Atualizar DTOs de OrdemServico"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Acompanhamento do progresso da ordem de serviço (Priority: P1)

Como um usuário da operação de oficina, quero que cada ordem de serviço tenha um status padronizado e legível, para que eu consiga acompanhar o andamento do trabalho e comunicar o estado correto ao cliente.

**Why this priority**: Este é o núcleo do fluxo de ordem de serviço. Sem status consistente, a operação perde rastreabilidade, a comunicação com o cliente fica ambígua e a evolução da OS torna-se difícil de controlar.

**Independent Test**: Pode ser validado criando uma ordem de serviço e confirmando que o sistema aceita somente os valores definidos de status e que cada etapa da operação é representada corretamente.

**Acceptance Scenarios**:

1. **Given** uma ordem de serviço recém-criada, **When** o sistema registra o status inicial, **Then** o valor deve ser `Aberta`.
2. **Given** uma ordem de serviço em execução, **When** a operação avança para acompanhamento do reparo, **Then** o status deve ser atualizado para `EmAndamento`.
3. **Given** uma ordem de serviço aguardando material, **When** o responsável registra a dependência de peça, **Then** o status deve ser `AguardandoPeca`.
4. **Given** uma ordem de serviço concluída, **When** o serviço é finalizado, **Then** o status deve ser `Concluida`.
5. **Given** uma ordem de serviço cancelada, **When** o registro é interrompido, **Then** o status deve ser `Cancelada`.

---

### User Story 2 - Integração entre API e banco de dados (Priority: P2)

Como um sistema integrado, quero que o enum de status seja persistido de forma consistente no banco e refletido nos DTOs da API, para que o comportamento seja igual em toda a aplicação e os dados permaneçam confiáveis.

**Why this priority**: O valor do status precisa ser consistente entre domínio, persistência e contrato de API. Essa consistência reduz erros de sincronização e facilita a manutenção do sistema ao longo do tempo.

**Independent Test**: Pode ser validado consultando a API e verificando que os DTOs retornam o status em um formato consistente, além de confirmar que o banco salva o valor esperado em relacionamento com o enum.

**Acceptance Scenarios**:

1. **Given** um registro existente em banco, **When** a API lê a ordem de serviço, **Then** o status deve ser retornado conforme o valor enum persistido.
2. **Given** uma ordem de serviço recebendo dados de entrada, **When** o payload inclui um valor válido de status, **Then** o sistema deve aceitar e persistir o valor sem perda de tipo.
3. **Given** uma tentativa de envio de status inválido, **When** o payload é processado, **Then** o sistema deve rejeitar o valor e impedir a persistência.

---

### User Story 3 - Evolução de dados históricos e migração (Priority: P3)

Como responsável pela manutenção do sistema, quero que a mudança de status de texto para enum seja conduzir por uma migração controlada, para que registros existentes não sejam perdidos e a evolução do banco seja segura.

**Why this priority**: A mudança de tipo exige cuidado para manter compatibilidade e rastreabilidade. Sem migração adequada, registros históricos podem ficar inconsistentes ou inviáveis de consultar.

**Independent Test**: Pode ser validado em ambiente de migração, verificando que a estrutura do banco foi atualizada e que o comportamento do sistema continua funcional após a aplicação da alteração.

**Acceptance Scenarios**:

1. **Given** um banco sem a nova enumeração, **When** a migração é aplicada, **Then** a estrutura deve ser criada sem quebrar o restante do modelo.
2. **Given** dados existentes de ordem de serviço, **When** a migração é executada, **Then** os valores de status devem ser preservados e convertidos de forma consistente para o novo modelo.

---

### Edge Cases

- O que acontece quando um payload de API envia um status desconhecido ou fora do enum?
- Como o sistema lida com ordens de serviço já existentes cujo status foi armazenado anteriormente como texto?
- O que acontece quando uma ordem de serviço tenta mudar de status em um fluxo inconsistente?
- Como o sistema deve tratar regras de dados quando a enumeração ainda não existe no banco em um ambiente em atualização?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema MUST disponibilizar o enum `OrdemServicoStatus` com os valores `Aberta`, `EmAndamento`, `AguardandoPeca`, `Concluida` e `Cancelada`, com os identificadores numéricos `1`, `2`, `3`, `4` e `5` respectivamente.
- **FR-002**: A entidade `OrdemServico` MUST usar o tipo enum para a propriedade `Status` em substituição ao tipo textual anterior.
- **FR-003**: Os DTOs de `OrdemServico` MUST refletir o tipo enum no contrato de entrada e saída, mantendo consistência com o modelo de domínio.
- **FR-003A**: O contrato de API MUST serializar `OrdemServicoStatus` em JSON como texto, usando os valores `"Aberta"`, `"EmAndamento"`, `"AguardandoPeca"`, `"Concluida"` e `"Cancelada"`, e não como valores numéricos.
- **FR-003B**: A entrada da API MUST aceitar apenas esses valores textuais para a propriedade `Status`, rejeitando qualquer valor fora do enum.
- **FR-004**: O DbContext MUST configurar a conversão de enum para string na persistência, garantindo que o valor do banco permaneça compatível com o modelo atual e a leitura em aplicação continue consistente.
- **FR-005**: O sistema MUST incluir uma migração de banco corretamente nomeada para a adoção do enum de status, preservando a integridade dos dados existentes.
- **FR-006**: A funcionalidade MUST garantir que a ordem de serviço continue representando um conjunto de estados bem definidos e rastreáveis ao longo do ciclo de atendimento.
- **FR-007**: O sistema MUST rejeitar valores de status fora do enum e impedir que estados inválidos sejam persistidos em novos registros.

### Key Entities *(include if feature involves data)*

- **OrdemServico**: Representa a ordem de serviço do cliente, com informações do atendimento e um estado atual do processo de execução.
- **OrdemServicoStatus**: Enum que define os estados válidos da ordem de serviço, incluindo a sequência de abertura, andamento, pendência por peça, conclusão e cancelamento.
- **DTO de OrdemServico**: Estrutura de transferência responsável por expor os dados da ordem de serviço em contratos de entrada e saída da aplicação.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% das ordens de serviço no sistema passam a usar um dos cinco estados válidos definidos pelo enum.
- **SC-002**: O status de cada ordem de serviço pode ser consultado e atualizado sem ambiguidade em qualquer fluxo de operação do sistema.
- **SC-003**: Os DTOs e as operações de API mostram o mesmo conjunto de valores do enum, reduzindo inconsistências entre backend e clientes.
- **SC-004**: A migração de banco é aplicada com sucesso em ambientes de atualização sem perda de registros e sem interrupção do fluxo principal de atendimento.

## Assumptions

- A mudança de tipo do status será aplicada no contexto de domínio de ordem de serviço sem alterar o restante dos fluxos do sistema.
- O status atual deve permanecer como parte central do acompanhamento operacional da oficina e não será substituído por outra estrutura de controle.
- Os dados existentes de ordens de serviço serão migrados de forma compatível com o novo modelo de enumeração.
- O projeto já dispõe da infraestrutura de Entity Framework e de migração necessária para publicar a mudança no banco.
