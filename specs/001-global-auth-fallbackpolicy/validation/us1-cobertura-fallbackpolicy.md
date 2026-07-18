# T009 - Cobertura da FallbackPolicy

Data: 2026-07-18

## Resumo da Auditoria

- Controllers totais encontrados em src/OficinaMotos.API/Controllers: 62
- Controllers de negocio (excluindo AuthController): 61
- Controllers com [AllowAnonymous] indevido em negocio: 0

## Evidencias Tecnicas

1. FallbackPolicy global configurada em Program.cs com RequireAuthenticatedUser.
2. GET /api/v1/Clientes sem token retorna 401.
3. GET /api/v1/Clientes com token invalido retorna 401.

## Conclusao

A politica global protege os endpoints de negocio por padrao e nao foram identificadas excecoes publicas indevidas.
