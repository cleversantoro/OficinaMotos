# Diretrizes e Regras do Projeto OficinaMotos

## 🏛️ Governança como Fonte Única da Verdade (`governance/`)

> [!IMPORTANT]
> **Toda a documentação, arquitetura, especificações e informações oficiais do projeto estão centralizadas na pasta `governance/`. Ela serve como a diretriz primária e mandatória para todo o desenvolvimento.**

1. **Documentos Oficiais de Referência**:
   - 📐 [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md): Visão geral do stack (.NET 8 Clean Architecture + Angular 21 Standalone Components & Signals), Bounded Contexts, padrões de design, convenções de pastas e regras de codificação.
   - 📋 [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md): Backlog oficial do produto com épicos, histórias de usuário (US-xxx), critérios de aceite, tarefas (T-xxx) e Definition of Done (DoD).
   - ⚖️ [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md): Architecture Decision Records (ADRs), débitos técnicos mapeados e restrições acordadas.
   - 🗺️ [`governance/roadmap.md`](file:///c:/Projetos/OficinaMotos/governance/roadmap.md): Planejamento de Sprints, marcos e releases.
   - 🔍 [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md): Inventário completo de entidades, tabelas, controllers, endpoints e componentes.
   - 🧪 [`governance/cobertura.md`](file:///c:/Projetos/OficinaMotos/governance/cobertura.md) & [`governance/auditoria.md`](file:///c:/Projetos/OficinaMotos/governance/auditoria.md): Critérios de qualidade, segurança e cobertura de testes.

2. **Obrigatoriedade de Consulta e Alinhamento**:
   - Antes de planejar ou iniciar qualquer nova história, tarefa ou refatoração, o agente deve consultar as diretrizes da pasta `governance/`.
   - Nenhuma decisão técnica ou arquitetural deve violar as ADRs documentadas em `governance/decisoes.md` ou os padrões de `governance/arquitetura.md`.

3. **Uso Mandatório com Speckit (`/speckit-specify`, `/speckit.plan`, `/speckit.tasks`, `/speckit.implement`)**:
   - Sempre que qualquer comando do Speckit for executado, o agente **DEVE buscar a pasta `governance/`** para extrair todas as instruções, diretivas de arquitetura, padrões de código, entidades e critérios de aceite antes de gerar arquivos de especificação, planos técnicos, tarefas ou implementação.

4. **Ciclo de Atualização do Backlog**:
   - Ao concluir qualquer tarefa ou história de usuário, o agente deve atualizar o status em `governance/backlog.md` marcando os itens correspondentes como concluídos (`[x]`).

---

## 🗄️ Regra Obrigatória: Scripts de Banco de Dados (MySQL 8.0)

> [!IMPORTANT]
> **Sempre que houver qualquer mudança no banco de dados** (criação ou exclusão de tabela, adição, remoção ou alteração de coluna, tipos de dados, chaves estrangeiras, índices, constraints ou migrações do Entity Framework Core):

1. **Versão do Banco de Dados**:
   - O banco de dados é **MySQL versão 8.0** (`Engine=InnoDB`, charset `utf8mb4`, collation `utf8mb4_unicode_ci`).
   - Todos os scripts devem utilizar estritamente a sintaxe compatível com o MySQL 8.0.

2. **Diretório Obrigatório para os Scripts**:
   - Todo script SQL deve ser salvo em: `oficina-motos-doc/scripts/` (ou `oficina-motos-docs/scripts/`).

3. **Padrão de Nomenclatura**:
   - O arquivo deve ser nomeado no formato: `<YYYYMMDD>_<descricao_da_alteracao>.sql` ou alinhado ao nome da migration EF Core.
   - Exemplo: `20260913_add_proximo_km_revisao_to_veiculo.sql`.

4. **Estrutura e Conteúdo do Script SQL**:
   - **Cabeçalho**: Informar a data, descrição da alteração, história de usuário / tarefa (US-xxx) e tabelas afetadas.
   - **Transação**: Encapsular as instruções DDL/DML entre `START TRANSACTION;` e `COMMIT;`.
   - **Sintaxe MySQL 8.0**: Utilizar comandos nativos MySQL (ex.: `ALTER TABLE \`tabela\` ADD COLUMN \`coluna\` ...`).
   - **Rollback / Down**: Incluir ao final do arquivo a seção comentada de reversão da alteração para facilitar rollbacks manuais em ambientes de homologação e produção.

5. **Critério de Conclusão (Definition of Done)**:
   - Nenhuma funcionalidade ou tarefa que altere o banco de dados pode ser considerada concluída sem que o script `.sql` correspondente tenha sido criado e testado nesta pasta.

