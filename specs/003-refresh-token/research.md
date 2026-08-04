# Pesquisa Tecnica - US-003

## Decisao 1: Persistir refresh token opaco em tabela seg_refresh_tokens com hash em repouso

- Decision: Modelar o refresh token como credencial opaca de sessao, armazenando apenas seu hash e metadados de expiracao/revogacao em uma nova tabela seg_refresh_tokens ligada a seg_usuarios.
- Rationale: Evita persistir segredo reutilizavel em texto puro, atende ao padrao seg_ do modulo de seguranca e permite revogacao/auditoria por sessao.
- Alternatives considered:
  - Salvar o refresh token em texto puro: simplifica consulta, mas aumenta impacto de vazamento de banco.
  - Reaproveitar seg_usuarios com colunas adicionais: mistura credencial de sessao com identidade do usuario e dificulta multiplas sessoes.

## Decisao 2: Estender o contrato de login para devolver refresh token e expiracao de refresh

- Decision: Evoluir a resposta de login com campos adicionais refreshToken e refreshTokenExpiresAt, preservando o contrato atual de token, usuario, roles e permissions.
- Rationale: O login passa a ser a origem da sessao completa; adicionar campos e uma extensao nao quebrante e evita chamada extra imediatamente apos autenticar.
- Alternatives considered:
  - Criar endpoint separado para emissao do refresh token apos login: adiciona round-trip sem valor funcional.
  - Mover refresh token para cookie HttpOnly nesta entrega: exigiria revisao mais ampla de CORS, CSRF e estrategia de SPA, fora do escopo definido.

## Decisao 3: Manter um refresh token estavel por sessao nesta entrega, sem rotacao a cada refresh

- Decision: O endpoint de refresh gera apenas novo access token; o refresh token emitido no login permanece valido ate expirar ou ser revogado por logout.
- Rationale: Atende diretamente ao criterio de aceite e reduz complexidade de sincronizacao entre frontend, persistencia e revogacao em um projeto que ainda nao possui infraestrutura de sessao rotativa.
- Alternatives considered:
  - Rotacionar refresh token em toda renovacao: melhora defesa contra replay, mas amplia superficie de falha e exige invalidacao encadeada nao pedida nesta entrega.
  - Nao expirar refresh token: contraria o objetivo de seguranca e dificulta controle de sessao.

## Decisao 4: Tratar /api/v1/Auth/refresh como endpoint excepcional de troca de credencial

- Decision: Permitir que o refresh seja autenticado exclusivamente pelo refresh token recebido no corpo da requisicao; manter logout protegido por access token e contendo tambem o refresh token a revogar.
- Rationale: O access token pode estar expirado no momento do refresh; portanto, a unica credencial viavel para o fluxo e o proprio refresh token. O logout segue o padrao JWT da constituicao porque normalmente acontece com sessao ainda ativa.
- Alternatives considered:
  - Exigir JWT valido em /refresh: impede o caso principal de sessao expirada.
  - Tornar /logout anonimo: reduz garantias de autenticidade sem necessidade funcional equivalente.

## Decisao 5: Serializar falhas 401 em um unico fluxo de auto-refresh no frontend

- Decision: O cliente web deve coordenar um unico refresh em voo por vez e repetir cada requisicao protegida no maximo uma vez apos renovacao bem-sucedida.
- Rationale: Evita tempestade de requisicoes para /refresh, elimina loops infinitos e atende ao edge case de 401 concorrentes.
- Alternatives considered:
  - Fazer logout imediato no primeiro 401: interrompe experiencia mesmo quando a sessao ainda pode ser recuperada.
  - Tentar refresh para cada requisicao que falhar: gera concorrencia desnecessaria e alto risco de estado inconsistente.

## Decisao 6: Alinhar o slice de auth do frontend a apiPaths durante a mesma feature

- Decision: Registrar auth.login, auth.refresh e auth.logout em apiPaths e usar essas constantes no AuthService em vez de URLs montadas inline.
- Rationale: A constituicao define api-paths como fonte central dos endpoints; como esta feature toca o mesmo slice de autenticacao, a correcao deve ocorrer junto da implementacao.
- Alternatives considered:
  - Manter login hardcoded e usar apiPaths apenas para os endpoints novos: preserva divergencia arquitetural no mesmo modulo.
  - Adiar a correcao para refatoracao futura: aumenta risco de inconsistencias de URL no fluxo de auth.
