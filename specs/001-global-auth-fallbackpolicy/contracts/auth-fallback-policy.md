# Contrato de Interface - Politica Global de Autenticacao

## Objetivo

Definir o contrato funcional de acesso para endpoints protegidos e para a excecao publica de login apos ativacao de FallbackPolicy.

## Escopo

- Backend: oficina-motos-api
- Endpoints protegidos: todos os controladores de negocio
- Endpoint publico: POST /api/v1/Auth/login

## Regras Contratuais

1. Todo endpoint de negocio exige usuario autenticado por token JWT valido.
2. Requisicoes sem token ou com token invalido para endpoint protegido retornam 401.
3. O endpoint de login permanece publico por excecao explicita.

## Endpoint Publico

### POST /api/v1/Auth/login

- Autenticacao obrigatoria: Nao
- Atributo esperado: AllowAnonymous
- Request body (exemplo):

```json
{
  "email": "usuario@oficina.com",
  "password": "senha"
}
```

- Respostas esperadas:
  - 200: credenciais validas, retorna token e dados do usuario
  - 400: payload invalido (campos obrigatorios ausentes)
  - 401: credenciais invalidas ou usuario bloqueado

## Endpoints Protegidos

### Regra Geral

- Caminho: /api/v1/* (exceto /api/v1/Auth/login)
- Autenticacao obrigatoria: Sim (FallbackPolicy)

### Exemplo representativo

- Endpoint: GET /api/v1/Clientes
- Sem token: 401 Unauthorized
- Com token invalido: 401 Unauthorized
- Com token valido: processa regra de negocio/autorizacao especifica

## Criterios de Conformidade

- C1: FallbackPolicy configurada para RequireAuthenticatedUser
- C2: Login anotado com AllowAnonymous
- C3: Validacao manual com curl comprova 401 em endpoint de negocio sem token
