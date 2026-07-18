# T011 - Evidencia US2: Respostas de Negocio no Login

Data: 2026-07-18

## Chamadas realizadas

1. POST /api/v1/Auth/login com payload JSON malformado:

- Resultado: 400 Bad Request (validacao/model binding)

1. POST /api/v1/Auth/login com JSON valido e credenciais de teste:

- Resultado: 401 Unauthorized com mensagem de credenciais invalidas/bloqueio

## Conclusao

- O endpoint permanece publico e responde conforme regra de negocio (400/401), sem exigir token JWT para acessar a action.
