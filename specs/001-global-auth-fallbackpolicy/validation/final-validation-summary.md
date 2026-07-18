# T016 - Resumo Final de Conformidade (SC-001 a SC-004)

Data: 2026-07-18

## SC-001

Criterio: 100% dos controladores de negocio rejeitam requisicoes sem token valido.

Status: ATENDIDO

Evidencias:

- FallbackPolicy global aplicada em Program.cs.
- Auditoria de controllers sem uso indevido de [AllowAnonymous].
- Testes reais em endpoint de negocio representativo (Clientes) com 401 sem token e token invalido.

## SC-002

Criterio: Endpoints publicos de autenticacao para login acessiveis sem token.

Status: ATENDIDO

Evidencias:

- [AllowAnonymous] aplicado em AuthController.Login.
- POST /api/v1/Auth/login sem token processado com resposta de negocio.

## SC-003

Criterio: Nenhum endpoint de negocio acessivel sem autenticacao valida.

Status: ATENDIDO

Evidencias:

- FallbackPolicy obrigando autenticacao por padrao.
- Validacao 401 em endpoint de negocio e auditoria de excecoes publicas.

## SC-004

Criterio: Chamada sem token para controlador representativo confirma bloqueio consistente.

Status: ATENDIDO

Evidencias:

- GET /api/v1/Clientes sem token => 401 Unauthorized.

## Conclusao Geral

A US-001 foi implementada e validada com sucesso, mantendo login publico e protegendo endpoints de negocio por autenticacao JWT global.
