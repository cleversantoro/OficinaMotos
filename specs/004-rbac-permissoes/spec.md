# Feature Specification: RBAC por permissões em controllers de negócio

**Feature Branch**: `[004-rbac-permissoes]`

**Created**: 2026-08-03

**Status**: Draft

**Input**: User description: "US-004 — RBAC por permissão nos controllers de negócio"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Acesso coerente por papel (Priority: P1)

Como usuário autenticado do sistema, quero ver apenas as ações que meu papel permite nas telas e nos fluxos de negócio, para evitar tentativa de operações que não devo executar.

**Why this priority**: Reduz erro operacional, melhora clareza da interface e evita fricção ao executar tarefas legítimas.

**Independent Test**: Pode ser testado verificando, para cada papel relevante, quais botões de ação aparecem em uma tela de negócio e confirmando que somente ações permitidas ficam visíveis.

**Acceptance Scenarios**:

1. **Given** um usuário com papel autorizado para alterar registros, **When** acessa uma tela de negócio com ações de edição e exclusão, **Then** vê apenas os botões compatíveis com suas permissões.
2. **Given** um usuário com papel restrito, **When** acessa a mesma tela, **Then** os botões de ações destrutivas permanecem ocultos.

---

### User Story 2 - Bloqueio de operações destrutivas (Priority: P1)

Como sistema, quero impedir alterações e exclusões quando o papel do usuário não tiver permissão, para garantir controle consistente em todos os fluxos de negócio.

**Why this priority**: É o núcleo da proteção de acesso e evita que ações indevidas sejam executadas mesmo que a interface seja manipulada.

**Independent Test**: Pode ser testado tentando executar operações de alteração e exclusão com um papel sem permissão e verificando que a operação é recusada.

**Acceptance Scenarios**:

1. **Given** um usuário com permissão insuficiente, **When** tenta executar uma operação de alteração ou exclusão, **Then** a operação é negada.
2. **Given** um usuário com permissão adequada, **When** executa a mesma operação, **Then** a operação é concluída.

---

### User Story 3 - Matriz clara de permissões por papel (Priority: P2)

Como equipe de produto e segurança, quero uma matriz clara que relacione papéis e permissões, para padronizar decisões de acesso em todos os módulos de negócio.

**Why this priority**: Garante consistência entre as regras de acesso da interface e das operações, reduzindo ambiguidade sobre quem pode executar cada ação.

**Independent Test**: Pode ser testado revisando a matriz de permissões e confirmando que cada papel possui regras explícitas para ações de leitura, alteração e exclusão.

**Acceptance Scenarios**:

1. **Given** um papel definido na matriz, **When** a equipe consulta suas permissões, **Then** encontra de forma explícita quais ações sensíveis estão liberadas ou negadas.
2. **Given** uma nova tela ou fluxo de negócio, **When** a equipe aplica a matriz, **Then** a regra de acesso permanece consistente com os demais módulos.

### Edge Cases

- O que acontece quando o usuário possui papel válido, mas a tela recebeu estado desatualizado de permissões? A interface deve permanecer conservadora e ocultar ações não confirmadas.
- Como o sistema responde quando um usuário tenta contornar a interface e acionar uma operação não permitida? A operação deve ser recusada de forma consistente.
- O que acontece quando uma ação destrutiva está disponível em mais de um ponto da interface? Todos os pontos devem seguir a mesma regra de visibilidade e bloqueio.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema MUST restringir operações destrutivas de acordo com o papel do usuário autenticado.
- **FR-002**: O sistema MUST negar com status de acesso insuficiente qualquer tentativa de alteração ou exclusão feita por papel sem permissão.
- **FR-003**: O sistema MUST aplicar a mesma regra de acesso em todos os pontos de interação expostos ao usuário.
- **FR-004**: A interface MUST ocultar ações destrutivas quando o papel do usuário não tiver permissão para executá-las.
- **FR-005**: O sistema MUST manter uma matriz explícita de permissões por papel para orientar decisões de acesso em ações sensíveis.
- **FR-006**: O sistema MUST aplicar as mesmas regras de acesso em todas as áreas de negócio abrangidas por esta feature.
- **FR-007**: O sistema MUST preservar a autorização de operações permitidas para papéis autorizados, sem degradar fluxos legítimos.

### Key Entities *(include if feature involves data)*

- **Matriz de Permissões por Papel**: define quais papéis podem executar quais ações sensíveis em cada área de negócio.
- **Papel de Acesso**: representa o nível de permissão do usuário autenticado dentro do sistema.
- **Ação Sensível**: operação que altera ou remove dados e precisa de validação de autorização explícita.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 100% das tentativas de alteração ou exclusão feitas por papéis sem permissão são bloqueadas.
- **SC-002**: 100% das ações destrutivas exibidas nas telas para papéis sem permissão ficam ocultas.
- **SC-003**: Em uma matriz de teste cobrindo todos os papéis relevantes, nenhuma ação destrutiva autorizada incorretamente é executada.
- **SC-004**: Pelo menos 95% dos testes manuais de acesso entre perfis e ações têm resultado esperado na primeira execução.

## Assumptions

- O conjunto de papéis existentes no sistema já é a base para a matriz de acesso desta feature.
- A prioridade desta feature é proteger ações destrutivas e refletir a mesma restrição na interface.
- Fluxos de leitura permanecem inalterados nesta entrega, salvo quando a mesma tela precisar esconder controles de ação.
- O comportamento esperado de negação é consistente com o padrão de acesso insuficiente já adotado no sistema.
