# Quickstart: Validar Soft Delete em BaseEntity

## Objetivo

Validar ponta a ponta que operações DELETE executam exclusão lógica e que consultas padrão não retornam registros excluídos.

## Pré-requisitos

- .NET SDK 8 instalado.
- Banco MySQL da API disponível.
- String de conexão configurada em `OficinaMotos.API`.

## 1) Criar e aplicar migration

No diretório `oficina-motos-api`:

```powershell
cd .\oficina-motos-api

dotnet ef migrations add AddSoftDeleteToBaseEntity --project .\src\OficinaMotos.Infrastructure\OficinaMotos.Infrastructure.csproj --startup-project .\src\OficinaMotos.API\OficinaMotos.API.csproj

dotnet ef database update --project .\src\OficinaMotos.Infrastructure\OficinaMotos.Infrastructure.csproj --startup-project .\src\OficinaMotos.API\OficinaMotos.API.csproj
```

Resultado esperado:

- Migration criada em `src/OficinaMotos.Infrastructure/Migrations/`.
- Banco atualizado com colunas `IsDeleted` e `DeletedAt` nas tabelas mapeadas por entidades derivadas de `BaseEntity`.

## 2) Validar compilação

```powershell
dotnet build .\OficinaMotos.slnx
```

Resultado esperado:

- Build concluído sem erros de compilação.

## 3) Validar comportamento de DELETE (API)

1. Criar ou identificar um registro existente (ex.: cliente).
2. Executar endpoint DELETE correspondente.
3. Confirmar retorno HTTP esperado do endpoint (normalmente `204 No Content` quando sucesso).
4. Consultar novamente por listagem padrão do mesmo recurso.

Resultado esperado:

- O registro não aparece na listagem padrão.
- O registro ainda existe no banco com `IsDeleted = true` e `DeletedAt` preenchido.

## 4) Validar query filter global

Executar leitura padrão via endpoint `GET` de recurso afetado e comparar com consulta direta ao banco.

Resultado esperado:

- API não retorna linhas com `IsDeleted = true`.
- Consulta SQL direta mostra o registro mantido para histórico.

## 5) Validar rollback de migration

```powershell
dotnet ef database update 20260804011629_AddRefreshTokens --project .\src\OficinaMotos.Infrastructure\OficinaMotos.Infrastructure.csproj --startup-project .\src\OficinaMotos.API\OficinaMotos.API.csproj
```

Resultado esperado:

- Banco retorna ao estado anterior à migration de soft delete sem corrupção de schema.

## 6) Evidências da implementação (execução real)

### Build

- Comando: `dotnet build .\OficinaMotos.slnx`
- Resultado: sucesso de compilação da solução (`Domain`, `Application`, `Infrastructure`, `API`).
- Observação: permanecem avisos de vulnerabilidade NU1902 em `OpenTelemetry.Exporter.OpenTelemetryProtocol` (não bloqueantes).

### Testes automatizados

- Projeto: `tests/OficinaMotos.SoftDelete.Tests`
- Comando: `dotnet test`
- Resultado: 7 testes executados, 7 aprovados, 0 falhas.
- Cobertura validada:
  - transição de estado em `BaseEntity.Delete()`
  - `DeleteAsync` e `SoftDeleteAsync` no repositório
  - filtro global em `GetAllAsync`, `GetByIdAsync`, `FindAsync`
  - três fluxos DELETE (ClienteOrigem, VeiculoMarca, FornecedorSegmento)
  - default `IsDeleted = false` em inserções persistidas

### Migration e rollback

- Migration criada: `20260804044157_AddSoftDeleteToBaseEntity`
- Tentativa de `dotnet ef database update`: bloqueada em ambiente local por schema já existente (`Table 'cad_clientes_origens' already exists`) sem baseline compatível no histórico EF.
- Validação de reversibilidade realizada por geração de scripts:
  - forward: `softdelete-up.sql`
  - rollback: `softdelete-down.sql`

### Próxima ação para homologação completa de apply/rollback

- Executar `dotnet ef database update` em banco alinhado ao histórico de migrations da solução (ou criar baseline da base existente antes de aplicar migrations incrementais).

## Referências

- Modelo de dados: [data-model.md](./data-model.md)
- Contrato de comportamento da API: [contracts/soft-delete-contract.md](./contracts/soft-delete-contract.md)
