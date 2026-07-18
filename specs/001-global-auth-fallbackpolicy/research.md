# Pesquisa Tecnica - US-001

## Decisao 1: Aplicar autenticacao global via FallbackPolicy

- Decision: Configurar AuthorizationOptions.FallbackPolicy com RequireAuthenticatedUser no pipeline da API.
- Rationale: Atende o requisito de proteger por padrao todos os endpoints de negocio e reduz risco de endpoint sem [Authorize] por esquecimento.
- Alternatives considered:
  - Decorar 56 controladores manualmente com [Authorize]: aumenta risco de omissao e custo de manutencao.
  - Usar policy nomeada por controller/acao: exige adocao manual em cada endpoint e nao garante protecao padrao futura.

## Decisao 2: Manter login como excecao explicita com AllowAnonymous

- Decision: Adicionar [AllowAnonymous] no endpoint AuthController.Login.
- Rationale: Preserva a capacidade de obter JWT sem autenticacao previa e evita bloqueio do fluxo de entrada.
- Alternatives considered:
  - Criar rota separada fora de controller autenticado: aumenta complexidade sem ganho funcional.
  - Desativar autorizacao global e usar whitelist invertida: enfraquece postura de seguranca por padrao.

## Decisao 3: Validar comportamento com teste HTTP de regressao minima

- Decision: Validar com chamadas HTTP sem token para endpoint de negocio representativo (ClientesController) e para /auth/login.
- Rationale: Confirma de forma objetiva os criterios de aceite de 401 em endpoint protegido e acesso publico no login.
- Alternatives considered:
  - Validacao apenas por inspeccao de codigo: nao comprova comportamento em runtime.
  - Cobertura E2E completa de todos os 56 controladores nesta fase: custo alto para escopo S da sprint.

## Decisao 4: Manter contrato de erro HTTP semantico

- Decision: Preservar retorno 401 para requisicoes sem token invalido/ausente em endpoints protegidos.
- Rationale: Alinha com a constituicao (Principio II e III) e com expectativas de clientes da API.
- Alternatives considered:
  - Retornar 403 para usuario nao autenticado: semantica incorreta para ausencia de autenticacao.
  - Customizar resposta para 200 com payload de erro: quebra contrato HTTP e consumidores existentes.
