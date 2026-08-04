# Modelo de Dados - US-003

## Visao Geral

A feature introduz persistencia explicita de credenciais de renovacao de sessao no modulo de Seguranca, sem alterar o modelo principal de identidade do usuario. O backend continua emitindo access tokens JWT, mas passa a controlar a recuperacao de sessao por meio da nova entidade RefreshToken.

## Entidades

### 1. RefreshToken

- Descricao: Credencial de renovacao associada a uma sessao autenticada de um usuario.
- Tabela prevista: seg_refresh_tokens
- Campos:
  - id: bigint
  - usuarioId: bigint
  - tokenHash: string
  - expiraEm: datetime
  - revogadoEm: datetime?
  - motivoRevogacao: string?
  - ipCriacao: string?
  - userAgentCriacao: string?
  - ultimoUsoEm: datetime?
  - createdAt: datetime
  - updatedAt: datetime?
- Regras de validacao:
  - tokenHash e obrigatorio e unico.
  - expiraEm deve ser maior que createdAt.
  - token so pode ser usado quando revogadoEm for nulo e expiraEm ainda estiver no futuro.
  - usuarioId deve referenciar um SegUsuario existente.

### 2. SegUsuario

- Descricao: Usuario autenticavel ja existente no modulo seg_, agora origem de uma ou mais credenciais de refresh.
- Campos relevantes para a feature:
  - id: bigint
  - email: string
  - login: string
  - status: int
  - ultimoLogin: datetime?
  - tentativasLogin: int
  - bloqueadoAte: datetime?
- Regras de validacao relevantes:
  - usuarios bloqueados, inativos ou com senha invalida nao recebem refresh token no login.
  - apenas usuarios autenticados com sucesso podem originar nova sessao renovavel.

### 3. SessaoAutenticada

- Descricao: Projecao logica do estado devolvido ao cliente apos login ou refresh.
- Campos:
  - accessToken: string
  - accessTokenExpiraEm: datetime
  - refreshToken: string? (presente no login; ausente no refresh se a estrategia sem rotacao for mantida)
  - refreshTokenExpiraEm: datetime?
  - userId: bigint
  - login: string
  - email: string
  - nome: string
  - roles: string[]
  - permissions: string[]
- Regras de validacao:
  - accessToken deve refletir o mesmo usuario associado ao refresh token validado.
  - dados de identidade retornados devem permanecer consistentes com o resultado de LoginDataResult.

### 4. EventoDeSessao

- Descricao: Registro auditavel de login, renovacao aceita, renovacao rejeitada e logout dentro de seg_audit_log.
- Campos relevantes:
  - usuarioId: bigint?
  - login: string?
  - acao: string
  - descricao: string?
  - ip: string?
  - userAgent: string?
  - createdAt: datetime
- Regras de validacao:
  - tabela permanece INSERT-ONLY.
  - renovacao bem-sucedida e falha de refresh devem ser distinguidas por descricao contextual, mesmo que reutilizem a taxonomia atual de acao.

## Relacionamentos

- SegUsuario 1:N RefreshToken
- SegUsuario 1:N EventoDeSessao
- RefreshToken 1:1 logico com SessaoAutenticada ativa no cliente

## Transicoes de Estado

### Login bem-sucedido

1. Usuario informa credenciais validas.
2. AuthService autentica SegUsuario e carrega perfis/permissoes.
3. Sistema gera access token JWT e refresh token opaco.
4. RefreshToken e persistido como ativo.
5. SessaoAutenticada e devolvida ao cliente com expiracoes correspondentes.

### Renovacao de sessao

1. Cliente envia refresh token valido para /api/v1/Auth/refresh.
2. Sistema localiza o hash correspondente e valida expiracao/revogacao/titularidade.
3. Sistema atualiza ultimoUsoEm.
4. Novo access token JWT e emitido.
5. SessaoAutenticada parcial e devolvida ao cliente mantendo o mesmo refresh token da sessao.

### Logout com revogacao

1. Cliente autenticado envia logout com refresh token corrente.
2. Sistema localiza o registro ativo correspondente.
3. Sistema preenche revogadoEm e motivoRevogacao.
4. Qualquer tentativa posterior de refresh para essa credencial passa a falhar.

### Falha de renovacao

1. Cliente envia refresh token inexistente, expirado ou revogado.
2. Sistema rejeita a renovacao sem emitir novo access token.
3. EventoDeSessao registra a rejeicao.
4. Cliente encerra a sessao local e exige novo login.
