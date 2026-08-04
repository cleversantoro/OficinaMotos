# Quickstart de Validacao - US-003

## Objetivo

Validar ponta a ponta a emissao, renovacao automatica e revogacao de refresh token entre API e frontend.

## Prerequisitos

- .NET SDK 8 instalado
- Node.js com npm 10
- Banco MySQL acessivel pela configuracao local da API
- Jwt__Key configurada para a API
- Dependencias restauradas nos repositorios oficina-motos-api e oficina-motos-web

## Setup

1. Restaurar backend:

```powershell
cd oficina-motos-api
dotnet restore OficinaMotos.slnx
```

1. Restaurar frontend:

```powershell
cd ..\oficina-motos-web
npm install
```

1. Subir a API:

```powershell
cd ..\oficina-motos-api
dotnet run --project src/OficinaMotos.API/OficinaMotos.API.csproj
```

1. Subir o frontend:

```powershell
cd ..\oficina-motos-web
npm run start:proxy
```

## Cenario 1 - Login devolve sessao renovavel

1. Enviar login valido para /api/v1/Auth/login.
2. Confirmar que a resposta contem token, expiresAt, refreshToken e refreshTokenExpiresAt.
3. Confirmar que o frontend persiste access token, refresh token e snapshot do usuario.

Resultado esperado:

- Sessao criada com refresh token ativo no backend.
- Nenhuma URL de auth consumida fora de apiPaths.auth.*.

## Cenario 2 - Refresh manual com token valido

1. Obter o refresh token recebido no login.
2. Enviar POST para /api/v1/Auth/refresh com esse valor.
3. Confirmar recebimento de novo access token.

Resultado esperado:

- Response 200.
- Novo access token com nova expiracao.
- Mesmo refresh token continua valido ate logout ou vencimento.

## Cenario 3 - Auto-refresh do frontend apos 401

1. Simular expiracao do access token mantendo refresh token valido.
2. Executar uma acao autenticada no frontend.
3. Observar o fluxo do errorInterceptor.

Resultado esperado:

- O cliente chama /api/v1/Auth/refresh uma unica vez.
- A requisicao original e repetida apenas uma vez.
- O usuario permanece na mesma tela sem login manual.

## Cenario 4 - Logout revoga refresh token

1. Com sessao autenticada, executar logout no frontend.
2. Confirmar chamada a /api/v1/Auth/logout.
3. Reenviar manualmente o refresh token antigo para /api/v1/Auth/refresh.

Resultado esperado:

- Logout responde sucesso e limpa estado local.
- Tentativa posterior de refresh retorna 401.

## Cenario 5 - Falha de refresh encerra sessao local

1. Alterar o refresh token local para valor invalido ou usar um token expirado/revogado.
2. Disparar uma requisicao protegida que produza 401.

Resultado esperado:

- O cliente tenta renovar apenas uma vez.
- A renovacao falha com 401.
- O usuario e redirecionado para /login e o estado local e limpo.

## Validacoes Executaveis Minimas

### Backend

```powershell
cd oficina-motos-api
dotnet build OficinaMotos.slnx
```

### Frontend

```powershell
cd oficina-motos-web
npm test
```

## Referencias

- Especificacao: [spec.md](./spec.md)
- Plano: [plan.md](./plan.md)
- Pesquisa: [research.md](./research.md)
- Modelo de dados: [data-model.md](./data-model.md)
- Contrato: [contracts/auth-refresh-contract.md](./contracts/auth-refresh-contract.md)
