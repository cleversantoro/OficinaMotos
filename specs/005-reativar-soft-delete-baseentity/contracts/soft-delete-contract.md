# Contract: Soft Delete na API de Negócio

## Objetivo

Definir o comportamento contratual das operações DELETE após adoção de exclusão lógica.

## Escopo

- Endpoints REST de recursos de negócio versionados em `/api/v1/*`.
- Regras de resposta HTTP e efeito de persistência para DELETE.
- Cobertura mínima desta feature para validação de contrato: `ClientesController`, `VeiculosController`, `FornecedoresController`, `MecanicosController`, `OrdemServicosController`, `EstoquePecasController` e `FinanceiroContasPagarController`.

## Regras contratuais

### 1. Semântica de exclusão

- Requisições DELETE não removem fisicamente o registro alvo.
- A API deve marcar o registro com exclusão lógica (`IsDeleted = true`, `DeletedAt` preenchido).
- A implementação deve passar pelo contrato base `IRepository<T>`/`Repository<T>` via `DeleteAsync`/`SoftDeleteAsync`.

### 2. Respostas HTTP

- `204 No Content`: exclusão lógica efetuada com sucesso.
- `404 Not Found`: recurso inexistente no escopo de leitura padrão.
- `401 Unauthorized`: token ausente/inválido.
- `403 Forbidden`: usuário autenticado sem permissão para operação.

### 3. Consistência de leitura

- Após DELETE com sucesso, consultas padrão (`GET` listagem e busca de fluxos normais) não devem retornar o recurso excluído.
- Consultas administrativas com `IgnoreQueryFilters()` não fazem parte deste contrato funcional da feature.

### 4. Idempotência funcional

- Repetir DELETE para o mesmo identificador não pode provocar hard delete.
- A segunda chamada deve manter estado coerente sem corromper auditoria de exclusão lógica.

## Cenários de verificação

1. Dado um recurso ativo existente, quando DELETE é executado, então retorna `204` e o recurso deixa de aparecer em listagem padrão.
2. Dado um recurso inexistente em leitura padrão, quando DELETE é executado, então retorna `404`.
3. Dado usuário sem permissão, quando DELETE é executado, então retorna `403`.

## Artefatos relacionados

- Especificação da feature: [../spec.md](../spec.md)
- Modelo de dados: [../data-model.md](../data-model.md)
- Guia de validação: [../quickstart.md](../quickstart.md)
