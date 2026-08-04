# Research: Reativar Soft Delete em BaseEntity

## Contexto

A feature requer exclusão lógica transversal no backend .NET/EF Core do projeto `oficina-motos-api`, com impacto em `BaseEntity`, `OficinaContext`, repositório base e migrações.

## Decisão 1: Modelo canônico de soft delete em `BaseEntity`

- Decision: Adotar `bool IsDeleted` (default `false`) e `DateTime? DeletedAt`, mantendo `UpdatedAt` para auditoria de alteração.
- Rationale: `bool` simplifica query filters e indexação; `DeletedAt` preserva rastreabilidade temporal exigida por governança e histórico.
- Alternatives considered:

  - Apenas `DeletedAt` nullable: rejeitada por tornar filtros menos explícitos e gerar comparações de data em toda consulta.
  - Apenas `Status` textual: rejeitada por acoplamento com regras de domínio específicas por entidade.

## Decisão 2: Aplicação de filtro global no `OficinaContext`

- Decision: Aplicar `HasQueryFilter(e => !e.IsDeleted)` dinamicamente para todo tipo que herda `BaseEntity` em `OnModelCreating`.
- Rationale: Evita duplicação de filtro por entidade/configuração e reduz risco de esquecimento em novos agregados.
- Alternatives considered:

  - Filtros por `IEntityTypeConfiguration`: rejeitada por alta repetição e manutenção dispersa.
  - Filtrar somente em repositórios: rejeitada por não cobrir queries diretas via `DbSet`/includes.

## Decisão 3: Semântica de DELETE na camada de persistência

- Decision: `DeleteAsync(id)` do `Repository<T>` passa a executar soft delete chamando `entity.Delete()` e persistindo update da entidade.
- Rationale: Mantém contrato atual dos services/controllers sem alterar assinatura pública e centraliza a regra no repositório base.
- Alternatives considered:

  - Novo método exclusivo `SoftDeleteAsync`: rejeitada para esta sprint por aumentar refactor em dezenas de serviços já acoplados ao `DeleteAsync`.
  - Hard delete seletivo por módulo: rejeitada por conflitar com diretriz de rastreabilidade e retenção.

## Decisão 4: Estratégia de migração EF Core

- Decision: Criar migration `AddSoftDeleteToBaseEntity` adicionando colunas `IsDeleted` e `DeletedAt` nas tabelas mapeadas por entidades que herdam `BaseEntity`, com `Down` removendo as colunas.
- Rationale: Mantém histórico de schema, rollback seguro e aderência ao fluxo padrão do projeto (`OficinaMotos.Infrastructure.Migrations`).
- Alternatives considered:

  - Script SQL manual fora de migration: rejeitada por reduzir rastreabilidade e dificultar rollback automatizado.
  - Migration parcial por módulo: rejeitada por risco de comportamento inconsistente entre contextos.

## Decisão 5: Compatibilidade de leitura e testes

- Decision: Validar por testes de integração/repositório e smoke test de endpoints DELETE críticos, garantindo que registros excluídos não apareçam em consultas padrão.
- Rationale: Não há projeto de testes dedicado no repositório no momento; validar com escopo pragmático para sprint curta sem bloquear entrega.
- Alternatives considered:

  - Apenas validação manual de UI: rejeitada por baixa confiança no comportamento de persistência.
  - Cobertura unitária completa de todos os módulos nesta feature: rejeitada por custo além da estimativa S.

## Impactos e riscos conhecidos

- Entidades que não herdam `BaseEntity` ficarão fora do filtro global por design e devem ser tratadas em backlog técnico.
- Relações com `Cascade` continuam físicas no banco, porém não serão acionadas em soft delete (pois não há `DELETE` SQL), preservando integridade sem remoção automática.
- Consultas que usem `IgnoreQueryFilters()` devem ser auditadas para evitar exposição acidental de dados excluídos.
