# Feature Specification: Reativar Soft Delete em BaseEntity

**Feature Branch**: `[005-reativar-soft-delete-baseentity]`

**Created**: 2026-08-04

**Status**: Draft

**Input**: User description: "US-005 — Reativar Soft Delete em BaseEntity"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Exclusão lógica padronizada (Priority: P1)

Como pessoa desenvolvedora do backend, quero que entidades derivadas de BaseEntity tenham estado de exclusão lógica, para preservar histórico e evitar perda definitiva de dados de negócio.

**Why this priority**: É a base da feature e habilita os demais comportamentos de leitura e API sem depender de ações manuais por entidade.

**Independent Test**: Pode ser testado ao excluir um registro e verificar que ele permanece armazenado com marcação de excluído, sem remoção física imediata.

**Acceptance Scenarios**:

1. **Given** uma entidade derivada de BaseEntity ativa, **When** ocorre operação de exclusão, **Then** a entidade é marcada como excluída logicamente.
2. **Given** uma entidade marcada como excluída, **When** uma rotina de persistência é concluída, **Then** os campos de exclusão lógica ficam gravados de forma consistente.

---

### User Story 2 - Leitura padrão sem registros excluídos (Priority: P1)

Como usuário do sistema, quero que listagens e buscas padrão ignorem registros excluídos logicamente, para enxergar apenas dados ativos sem ruído operacional.

**Why this priority**: Garante comportamento esperado da aplicação após a mudança de exclusão, evitando regressões funcionais em consultas de negócio.

**Independent Test**: Pode ser testado criando registros, excluindo parte deles logicamente e validando que consultas padrão não retornam os excluídos.

**Acceptance Scenarios**:

1. **Given** registros ativos e registros excluídos logicamente, **When** uma consulta padrão é executada, **Then** somente registros ativos são retornados.
2. **Given** um registro excluído logicamente, **When** uma busca por fluxo padrão ocorre, **Then** o registro não aparece no resultado.

---

### User Story 3 - Exclusão via API sem hard delete (Priority: P2)

Como equipe de segurança e dados, quero que endpoints DELETE executem exclusão lógica, para manter rastreabilidade e permitir restauração controlada quando necessário.

**Why this priority**: Complementa a política de retenção e auditoria, reduzindo risco de perda irreversível de informações.

**Independent Test**: Pode ser testado chamando endpoint DELETE e validando que o registro deixa de aparecer nas consultas padrão sem ser removido fisicamente.

**Acceptance Scenarios**:

1. **Given** um registro existente, **When** o endpoint DELETE é acionado, **Then** o registro é marcado como excluído logicamente.
2. **Given** um registro excluído logicamente por DELETE, **When** consultas padrão são realizadas, **Then** o registro não é mais exibido.

### Edge Cases

- O que acontece ao executar DELETE em registro já excluído logicamente? A operação deve manter idempotência funcional e não provocar remoção física acidental.
- Como o sistema se comporta quando uma entidade não herda corretamente de BaseEntity? A configuração deve impedir lacunas de cobertura e sinalizar inconsistências durante validação de desenvolvimento.
- O que acontece durante migração em base com dados existentes? Os novos campos devem assumir valores padrão seguros para evitar exclusão indevida em massa.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema MUST incluir os atributos de exclusão lógica em BaseEntity, contendo indicador de exclusão e data/hora opcional de exclusão.
- **FR-002**: O sistema MUST considerar novas entidades como não excluídas por padrão.
- **FR-003**: O sistema MUST aplicar filtro global de consulta para ignorar registros excluídos logicamente nas leituras padrão.
- **FR-004**: O sistema MUST garantir que operações DELETE da API resultem em exclusão lógica, sem remoção física direta do registro.
- **FR-005**: O sistema MUST disponibilizar operação de soft delete na base de repositórios para manter comportamento consistente entre módulos.
- **FR-006**: O sistema MUST gerar migração de banco reversível para adicionar os campos de exclusão lógica às tabelas impactadas.
- **FR-007**: O sistema MUST preservar compatibilidade com fluxos existentes de leitura e atualização para registros ativos.

### Key Entities *(include if feature involves data)*

- **BaseEntity**: entidade base compartilhada que passa a representar estado de exclusão lógica para todas as entidades derivadas.
- **Registro Excluído Logicamente**: instância de entidade com marcação de exclusão ativa e carimbo temporal de exclusão.
- **Filtro Global de Consulta**: regra transversal de leitura que define visibilidade apenas de registros não excluídos nos fluxos padrão.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% das entidades derivadas de BaseEntity utilizam o mesmo padrão de exclusão lógica após a entrega.
- **SC-002**: 100% das consultas padrão de negócio deixam de retornar registros marcados como excluídos.
- **SC-003**: 100% dos endpoints DELETE de controladores de negócio que utilizam o contrato base de repositório (`IRepository<T>`/`Repository<T>`) passam a marcar exclusão lógica em vez de remover fisicamente.
- **SC-004**: A migração é aplicada e revertida com sucesso em ambiente de validação sem perda de dados ativos.

## Assumptions

- A política de retenção do projeto privilegia exclusão lógica para entidades de negócio com histórico relevante.
- O escopo desta entrega não inclui tela administrativa de restauração, apenas o mecanismo de marcação e ocultação.
- As entidades de negócio relevantes herdam de BaseEntity e, portanto, podem receber a regra de filtro global.
- O padrão atual de rotas DELETE da API será mantido, alterando apenas o comportamento interno de persistência.
