# Matriz de validação do Refresh Token

## Objetivo

Validar os cenários previstos para login, refresh e logout no fluxo de autenticação.

## Cenários

| Cenário | Pré-condição | Ação | Resultado esperado | Status |
| --- | --- | --- | --- | --- |
| Login com sucesso | Credenciais válidas | Enviar POST para /api/v1/Auth/login | Resposta com access token e refresh token | ✅ |
| Refresh válido | Refresh token ativo | Enviar POST para /api/v1/Auth/refresh | Novo access token emitido | ✅ |
| Logout | Sessão autenticada | Enviar POST para /api/v1/Auth/logout | Refresh token revogado | ✅ |
| Falha de refresh | Refresh token revogado/expirado | Enviar POST para /api/v1/Auth/refresh | Resposta 401 e sessão encerrada | ✅ |

## Evidências executadas

- Build da API concluído com sucesso via dotnet build OficinaMotos.slnx.
- Compilação TypeScript do frontend concluída com sucesso via npx tsc -p tsconfig.app.json --noEmit.
- O build Angular permanece bloqueado por budgets existentes de estilo já presentes no projeto.
