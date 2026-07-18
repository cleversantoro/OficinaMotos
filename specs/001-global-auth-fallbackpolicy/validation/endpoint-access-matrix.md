# T002 - Matriz de Verificacao de Acesso

## Regra Geral

- Politica global: FallbackPolicy com RequireAuthenticatedUser.
- Excecao publica: POST /api/v1/Auth/login com [AllowAnonymous].

## Matriz

| Endpoint | Tipo | Sem Token | Token Invalido | Token Valido |
| --- | --- | --- | --- | --- |
| GET /api/v1/Clientes | Protegido | 401 | 401 | 200/fluxo de negocio |
| POST /api/v1/Auth/login | Publico | 200/400/401 de negocio | 200/400/401 de negocio | 200/400/401 de negocio |

## Fonte de Contrato

- specs/001-global-auth-fallbackpolicy/contracts/auth-fallback-policy.md
