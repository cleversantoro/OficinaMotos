# Regra: Governança como Fonte Única da Verdade (`governance/`)

Esta regra é mandatória para qualquer planejamento, arquitetura, desenvolvimento de código e atualização de tarefas neste repositório.

## Diretrizes Mandatórias
1. Toda documentação oficial, arquitetura de sistemas, Bounded Contexts, stack tecnológico, decisões arquiteturais (ADRs), backlog de produto, métricas de cobertura e roadmap residem na pasta `governance/`.
2. **Integração Obrigatória com Speckit**:
   - Sempre que qualquer comando Speckit (`/speckit-specify`, `/speckit.plan`, `/speckit.tasks`, `/speckit.implement`) for acionado, o agente **DEVE consultar primeiramente a pasta `governance/`** para buscar instruções, regras de arquitetura, padrões de código, Bounded Contexts e requisitos do projeto.
3. **Documentos Oficiais de Referência**:
   - `governance/arquitetura.md`: Arquitetura, bounded contexts, padrões técnicos e Clean Code.
   - `governance/decisoes.md`: Registros de Decisão Arquitetural (ADRs) e restrições.
   - `governance/backlog.md`: Backlog oficial com histórias de usuário (`US-xxx`), tarefas (`T-xxx`) e critérios de aceite.
   - `governance/roadmap.md`: Planejamento de Sprints e marcos.
   - `governance/inventario.md`: Inventário de tabelas, entidades e endpoints.
4. Todo trabalho deve estar alinhado com as especificações contidas nesses documentos.
5. O backlog oficial em `governance/backlog.md` deve ser mantido sempre atualizado ao concluir tarefas.
