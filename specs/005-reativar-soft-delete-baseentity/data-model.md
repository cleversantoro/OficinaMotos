# Data Model: Reativar Soft Delete em BaseEntity

## 1. BaseEntity (modelo transversal)

Representa o contrato comum de persistência das entidades de domínio.

### Campos relevantes

- `Id: long` (PK)
- `CreatedAt: DateTime`
- `UpdatedAt: DateTime?`
- `IsDeleted: bool` (novo, default `false`)
- `DeletedAt: DateTime?` (novo)

### Regras de validação

- `IsDeleted` inicia como `false` em novas entidades.
- Ao excluir logicamente, `IsDeleted` deve ser `true`.
- Ao excluir logicamente, `DeletedAt` deve receber timestamp UTC.
- `DeletedAt` deve permanecer `null` enquanto `IsDeleted = false`.

### Transições de estado

1. `Ativo`:
   - Condição: `IsDeleted = false` e `DeletedAt = null`.
2. `Excluído logicamente`:
   - Trigger: operação DELETE de negócio via repositório base.
   - Efeito: `IsDeleted = true`, `DeletedAt = utcNow`, `UpdatedAt = utcNow`.

## 2. Registro Excluído Logicamente (visão comportamental)

Entidade de negócio que herda `BaseEntity` e foi marcada por exclusão lógica.

### Comportamento esperado

- Não deve aparecer em consultas padrão devido ao query filter global.
- Pode ser recuperada apenas por consultas administrativas explícitas com `IgnoreQueryFilters()` (fora do escopo desta feature).

## 3. Filtro Global de Consulta (OficinaContext)

Regra transversal aplicada no `OnModelCreating`.

### Regra

- Para todo tipo `T` que herda `BaseEntity`: aplicar `HasQueryFilter(e => !e.IsDeleted)`.

### Efeito no modelo

- Operações de leitura padrão em `DbSet<T>` passam a ignorar excluídos.
- `FindAsync`/queries do repositório obedecem o filtro global do EF Core.

## 4. Mapeamento de banco (migração)

### Alterações por tabela mapeada por `BaseEntity`

- Adicionar coluna `IsDeleted` (`tinyint(1)` / boolean), `NOT NULL`, default `false`.
- Adicionar coluna `DeletedAt` (`datetime(6)`), nullable.

### Rollback

- `Down` remove `DeletedAt` e `IsDeleted` das tabelas impactadas.
