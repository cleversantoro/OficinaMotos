# Modelo de Dados - US-002

## Visao Geral

A feature nao altera banco de dados. O modelo abaixo representa entidades logicas de configuracao de seguranca no startup da API.

## Entidades

### 1. ChaveJwtConfigurada

- Descricao: Valor de segredo usado para assinatura e validacao de tokens JWT.
- Campos:
  - fonteConfiguracao: enum {appsettings, variavel_ambiente, provider_externo}
  - valor: string
  - estaVaziaOuWhitespace: boolean
- Regras de validacao:
  - valor deve existir e nao pode ser vazio/whitespace.
  - quando invalida, startup deve falhar imediatamente.

### 2. ConfigJwtVersionada

- Descricao: Representacao da configuracao JWT no arquivo versionado da API.
- Campos:
  - caminho: string (Jwt:Key)
  - valorVersionado: string
  - contemSegredoSensivel: boolean
- Regras de validacao:
  - valorVersionado deve ser placeholder vazio no repositorio.
  - contemSegredoSensivel deve ser false em arquivos versionados.

### 3. RegraFailFastStartup

- Descricao: Politica de validacao de configuracao obrigatoria durante inicializacao.
- Campos:
  - condicaoFalha: string (chave ausente/vazia)
  - tipoExcecao: string (InvalidOperationException)
  - mensagemErro: string
- Regras de validacao:
  - deve interromper startup antes do mapeamento de controllers.
  - mensagem deve indicar claramente qual configuracao esta faltando.

## Relacionamentos

- RegraFailFastStartup valida 1 ChaveJwtConfigurada
- ConfigJwtVersionada referencia o mesmo caminho logico da ChaveJwtConfigurada

## Transicoes de Estado

### Startup sem chave JWT valida

1. Leitura da configuracao JWT
2. Validacao detecta chave ausente/vazia
3. Regra fail-fast dispara InvalidOperationException
4. Aplicacao nao sobe

### Startup com chave JWT valida

1. Leitura da configuracao JWT
2. Validacao aprova valor nao vazio
3. Chave e convertida para bytes de assinatura
4. Pipeline de autenticacao JWT e inicializado
