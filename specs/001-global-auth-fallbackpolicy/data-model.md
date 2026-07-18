# Modelo de Dados - US-001

## Visao Geral

A feature nao introduz novas tabelas nem altera schema de banco. O modelo abaixo descreve entidades logicas de controle de acesso e seus estados para validacao funcional.

## Entidades

### 1. EndpointApi

- Descricao: Recurso HTTP exposto pela API.
- Campos:
  - caminho: string (ex.: /api/v1/Clientes)
  - metodoHttp: string (GET, POST, PUT, DELETE)
  - tipoAcesso: enum {Protegido, Publico}
  - controlador: string
  - acao: string
- Regras de validacao:
  - tipoAcesso = Publico somente para excecoes explicitas aprovadas.
  - Endpoints de negocio devem permanecer com tipoAcesso = Protegido.

### 2. PoliticaAutorizacaoGlobal

- Descricao: Regra padrao aplicada a toda API para exigir autenticacao.
- Campos:
  - nome: string (FallbackPolicy)
  - requerUsuarioAutenticado: boolean
  - ativa: boolean
- Regras de validacao:
  - ativa deve ser true em ambiente de execucao normal.
  - requerUsuarioAutenticado deve ser true quando ativa.

### 3. ExcecaoAcessoPublico

- Descricao: Declaracao explicita de endpoint que nao exige token.
- Campos:
  - endpoint: referencia para EndpointApi
  - motivoNegocio: string
  - atributo: string (AllowAnonymous)
- Regras de validacao:
  - Excecao permitida apenas para endpoint de login nesta feature.
  - Excecao deve ser explicita no codigo via atributo correspondente.

### 4. ResultadoAutenticacaoRequisicao

- Descricao: Resultado da avaliacao de autenticacao para uma requisicao HTTP.
- Campos:
  - possuiToken: boolean
  - tokenValido: boolean
  - endpointPublico: boolean
  - statusHttp: int
- Regras de validacao:
  - endpointPublico = false e tokenValido = false implica statusHttp = 401.
  - endpointPublico = true permite processamento sem token para fluxo de login.

## Relacionamentos

- EndpointApi 1:N ExcecaoAcessoPublico
- PoliticaAutorizacaoGlobal 1:N EndpointApi (aplicacao por padrao)
- ResultadoAutenticacaoRequisicao referencia 1 EndpointApi por avaliacao

## Transicoes de Estado

### Endpoint protegido sem token

1. Requisicao recebida
2. FallbackPolicy aplicada
3. Token ausente/invalido detectado
4. Resposta 401 Unauthorized

### Endpoint publico de login sem token

1. Requisicao recebida
2. Excecao AllowAnonymous reconhecida
3. Fluxo de login executado
4. Resposta conforme validacao de credenciais (200, 400 ou 401)
