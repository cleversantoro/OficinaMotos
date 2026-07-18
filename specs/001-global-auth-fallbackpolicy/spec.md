# Especificacao da Feature: US-001 - Autenticacao Global por Padrao

**Branch da Feature**: `[001-global-auth-fallbackpolicy]`

**Criado em**: 2026-07-18

**Status**: Rascunho

**Entrada**: Descricao do usuario: "US-001 — Aplicar [Authorize] globalmente via FallbackPolicy"

## Cenarios de Usuario e Testes *(obrigatorio)*

### Historia de Usuario 1 - Bloqueio Padrao de Endpoints de Negocio (Prioridade: P1)

Como usuario nao autenticado, quero que endpoints de negocio bloqueiem acesso por padrao para impedir consultas e operacoes sem identidade valida.

**Por que esta prioridade**: Reduz risco de exposicao acidental de dados e garante conformidade com o modelo de seguranca definido para o sistema.

**Teste Independente**: Pode ser testado enviando requisicoes sem token para endpoints de negocio e verificando retorno de nao autenticado.

**Cenarios de Aceite**:

1. **Dado** uma requisicao sem token para um endpoint de negocio, **Quando** o sistema processa a chamada, **Entao** o acesso e negado com resposta de nao autenticado.
2. **Dado** uma requisicao com token invalido para um endpoint de negocio, **Quando** o sistema valida a autenticacao, **Entao** o acesso e negado com resposta de nao autenticado.
3. **Dado** uma requisicao com token valido para um endpoint de negocio, **Quando** o sistema processa a chamada, **Entao** o fluxo segue para validacoes funcionais e de permissao aplicaveis.

---

### Historia de Usuario 2 - Excecao Controlada para Login Publico (Prioridade: P2)

Como usuario ainda nao autenticado, quero acessar o endpoint de login sem token para obter credenciais de sessao validas.

**Por que esta prioridade**: Sem endpoint publico de login, nao ha como iniciar sessao e usar funcionalidades protegidas.

**Teste Independente**: Pode ser testado chamando o endpoint de login sem token e verificando que o endpoint permanece acessivel.

**Cenarios de Aceite**:

1. **Dado** uma requisicao sem token para o endpoint de login, **Quando** o sistema recebe a chamada, **Entao** a requisicao e aceita e segue validacao de credenciais.
2. **Dado** um endpoint definido como publico para autenticacao, **Quando** o sistema aplica politica global de autenticacao, **Entao** esse endpoint permanece fora da exigencia de token.

### Casos de Borda

- Requisicoes com token expirado devem receber resposta de nao autenticado, sem acesso parcial ao recurso.
- Endpoints explicitamente definidos como publicos devem continuar acessiveis mesmo apos a ativacao da protecao global.
- Endpoints de negocio novos criados futuramente devem herdar protecao por padrao sem configuracao adicional por endpoint.

## Requisitos *(obrigatorio)*

### Requisitos Funcionais

- **FR-001**: O sistema MUST exigir autenticacao valida por padrao para todos os endpoints de negocio da API.
- **FR-002**: O sistema MUST negar acesso a endpoints de negocio quando a requisicao nao apresentar token de autenticacao valido.
- **FR-003**: O sistema MUST manter o endpoint de login acessivel sem autenticacao para permitir inicio de sessao.
- **FR-004**: O sistema MUST permitir excecoes explicitas para endpoints publicos previamente autorizados pela regra de seguranca do produto.
- **FR-005**: O sistema MUST retornar resposta de nao autenticado de forma consistente em todos os controladores de negocio protegidos quando nao houver token valido.
- **FR-006**: O sistema MUST garantir que a politica global de autenticacao seja aplicada automaticamente a novos endpoints de negocio, exceto quando houver excecao publica explicita.

### Entidades-Chave *(incluir se a feature envolver dados)*

- **Endpoint de API**: Recurso HTTP exposto pela aplicacao, classificado como protegido (negocio) ou publico (autenticacao).
- **Token de Autenticacao**: Credencial apresentada pelo cliente para comprovar identidade e permitir acesso a recursos protegidos.
- **Politica Global de Acesso**: Regra de seguranca padrao que determina obrigatoriedade de autenticacao para endpoints protegidos.

## Criterios de Sucesso *(obrigatorio)*

### Resultados Mensuraveis

- **SC-001**: 100% dos 56 controladores de negocio rejeitam requisicoes sem token valido com resposta de nao autenticado.
- **SC-002**: 100% dos endpoints publicos de autenticacao definidos para login permanecem acessiveis sem token.
- **SC-003**: Em validacao de regressao de seguranca da sprint, nenhum endpoint de negocio fica acessivel sem autenticacao valida.
- **SC-004**: A validacao de uma chamada sem token para pelo menos um controlador de negocio representativo confirma comportamento consistente de bloqueio conforme a politica global.

## Premissas

- Existe um mecanismo de emissao de token de autenticacao ja funcional e fora do escopo desta feature.
- O endpoint de login e o unico endpoint publico obrigatorio nesta entrega.
- A classificacao de "controladores de negocio" considera os 56 controladores listados no escopo atual da API.
- Validacoes de permissao por perfil continuam ocorrendo apos autenticacao e nao sao alteradas por esta feature.
