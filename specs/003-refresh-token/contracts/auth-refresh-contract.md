# Contrato de Autenticacao - Refresh Token

## Objetivo

Definir a evolucao contratual do fluxo de autenticacao para suportar refresh token com renovacao de access token e revogacao por logout.

## Escopo

- Projeto backend: oficina-motos-api
- Projeto frontend: oficina-motos-web
- Artefatos impactados:
  - AuthController
  - AuthService / IAuthService
  - DTOs de Auth
  - apiPaths, AuthService e errorInterceptor no frontend

## Regras Contratuais

1. Login continua em /api/v1/Auth/login e passa a retornar tambem refreshToken e refreshTokenExpiresAt.
2. Refresh ocorre em /api/v1/Auth/refresh e aceita exclusivamente refresh token valido no corpo da requisicao.
3. Logout ocorre em /api/v1/Auth/logout, exige access token autenticado e revoga o refresh token informado no corpo.
4. Toda renovacao bem-sucedida pode repetir a requisicao original do cliente apenas uma vez.
5. Quando a renovacao falha, o cliente deve limpar a sessao local e redirecionar para login.
6. Os endpoints de auth do frontend devem ser consumidos via apiPaths.auth.*.

## Contratos de Endpoint

### 1. POST /api/v1/Auth/login

**Autenticacao**: anonima

#### Login request body

```json
{
  "email": "usuario@oficina.local",
  "password": "senha-segura"
}
```

#### Login response 200

```json
{
  "token": "jwt-access-token",
  "refreshToken": "opaque-refresh-token",
  "refreshTokenExpiresAt": "2026-07-25T13:45:00Z",
  "email": "usuario@oficina.local",
  "name": "Usuario da Oficina",
  "role": "Administrador",
  "expiresAt": "2026-07-18T21:45:00Z",
  "userId": 42,
  "login": "usuario.admin",
  "roles": ["Administrador"],
  "permissions": ["dashboard:visualizar", "clientes:editar"]
}
```

#### Login response 401

- Credenciais invalidas, usuario bloqueado ou usuario inativo.

### 2. POST /api/v1/Auth/refresh

**Autenticacao**: sem bearer JWT; validacao por refresh token valido

#### Refresh request body

```json
{
  "refreshToken": "opaque-refresh-token"
}
```

#### Refresh response 200

```json
{
  "token": "novo-jwt-access-token",
  "expiresAt": "2026-07-18T22:10:00Z",
  "userId": 42,
  "login": "usuario.admin",
  "email": "usuario@oficina.local",
  "name": "Usuario da Oficina",
  "role": "Administrador",
  "roles": ["Administrador"],
  "permissions": ["dashboard:visualizar", "clientes:editar"]
}
```

#### Refresh response 401

- Refresh token inexistente, expirado, revogado ou desvinculado de usuario valido.

### 3. POST /api/v1/Auth/logout

**Autenticacao**: bearer JWT obrigatorio

#### Logout request body

```json
{
  "refreshToken": "opaque-refresh-token"
}
```

#### Logout response 204

- Sessao encerrada e refresh token revogado.

#### Logout response 401

- Access token ausente/invalido.

## Contrato do Frontend

### Persistencia local

- localStorage deve manter:
  - access token atual
  - refresh token atual
  - snapshot do usuario autenticado com expiracao do access token

### Fluxo do interceptor

1. Requisicao autenticada retorna 401.
2. Se a requisicao ja tiver sido repetida uma vez, o erro nao e reprocessado.
3. Se houver refresh token disponivel, o cliente tenta /api/v1/Auth/refresh.
4. Se o refresh for bem-sucedido, o access token local e atualizado e a requisicao original e refeita uma unica vez.
5. Se o refresh falhar, o cliente executa logout local e envia o usuario para /login.

## Criterios de Conformidade

- C1: LoginResponseDTO inclui refreshToken e refreshTokenExpiresAt sem remover campos existentes.
- C2: /api/v1/Auth/refresh rejeita tokens expirados, revogados e desconhecidos com 401.
- C3: /api/v1/Auth/logout revoga o refresh token informado e impede novo refresh posterior.
- C4: apiPaths.auth contem login, refresh e logout.
- C5: errorInterceptor evita loop infinito e tempestade de refresh concorrente.
