# Especificacao da Feature: US-002 - Remover Chave JWT Hardcoded

**Branch da Feature**: `[002-remove-jwt-hardcoded]`

**Criado em**: 2026-07-18

**Status**: Rascunho

**Entrada**: Descricao do usuario: "US-002 — Remover chave JWT hardcoded"

## Cenarios de Usuario e Testes *(obrigatorio)*

### Historia de Usuario 1 - Inicializacao Segura sem Segredo Versionado (Prioridade: P1)

Como responsavel por operacao da API, quero que a aplicacao falhe de forma clara ao iniciar sem chave JWT configurada, para evitar execucao insegura com segredo padrao embutido.

**Por que esta prioridade**: Elimina risco critico de seguranca por segredo hardcoded e impede deploy inseguro em qualquer ambiente.

**Teste Independente**: Iniciar a aplicacao sem variavel de configuracao da chave JWT e confirmar excecao explicita de startup (fail-fast).

**Cenarios de Aceite**:

1. **Dado** que a chave JWT nao esta configurada, **Quando** a API inicia, **Entao** a inicializacao falha com mensagem clara indicando configuracao obrigatoria.
2. **Dado** que a chave JWT esta configurada, **Quando** a API inicia, **Entao** o pipeline de autenticacao e configurado sem uso de fallback hardcoded.

---

### Historia de Usuario 2 - Configuracao e Documentacao Operacional Clara (Prioridade: P2)

Como desenvolvedor/DevOps, quero encontrar instrucoes claras no README e placeholder sem segredo no appsettings, para configurar ambientes sem vazar credenciais no repositório.

**Por que esta prioridade**: Reduz erro operacional e evita reincidencia de segredo sensivel no controle de versao.

**Teste Independente**: Verificar que appsettings versionado nao contem segredo JWT real e que README descreve variavel obrigatoria de configuracao.

**Cenarios de Aceite**:

1. **Dado** o arquivo de configuracao versionado, **Quando** ele for revisado, **Entao** o campo de segredo JWT aparece vazio (placeholder) e sem valor real.
2. **Dado** o README atualizado, **Quando** um novo membro configurar ambiente local, **Entao** ele encontra instrucoes objetivas para definir a variavel de chave JWT.

### Casos de Borda

- Configuracao presente, mas com valor vazio ou apenas espacos, deve ser tratada como ausente e disparar fail-fast.
- Configuracao definida com nome incorreto de chave/variavel deve resultar em erro de inicializacao equivalente a configuracao ausente.
- Nao deve existir comportamento silencioso que substitua segredo ausente por valor padrao em codigo.

## Requisitos *(obrigatorio)*

### Requisitos Funcionais

- **FR-001**: O sistema MUST remover qualquer fallback hardcoded para chave JWT na configuracao de autenticacao.
- **FR-002**: O sistema MUST validar na inicializacao se a chave JWT obrigatoria esta configurada com valor nao vazio.
- **FR-003**: O sistema MUST interromper startup com excecao clara quando a chave JWT obrigatoria estiver ausente ou vazia.
- **FR-004**: O sistema MUST manter no arquivo de configuracao versionado apenas placeholder vazio para segredo JWT, sem valor sensivel.
- **FR-005**: O sistema MUST documentar no README como configurar a chave JWT obrigatoria por variavel de ambiente/configuracao externa.
- **FR-006**: O sistema MUST impedir que ambiente de execucao utilize chave JWT padrao implicita.

### Entidades-Chave *(incluir se a feature envolver dados)*

- **ChaveJWTConfigurada**: Valor de segredo lido de fonte de configuracao externa para assinatura e validacao de tokens.
- **ConfigVersionada**: Arquivo versionado de configuracao da aplicacao que deve conter apenas placeholders nao sensiveis.
- **RegraFailFastStartup**: Validacao obrigatoria de inicializacao que bloqueia execucao sem segredo JWT valido.

## Criterios de Sucesso *(obrigatorio)*

### Resultados Mensuraveis

- **SC-001**: 0 segredos JWT reais presentes nos arquivos de configuracao versionados da API.
- **SC-002**: 100% das inicializacoes sem chave JWT configurada falham imediatamente com mensagem de erro clara.
- **SC-003**: 100% das inicializacoes com chave JWT valida prosseguem sem depender de valor padrao embutido.
- **SC-004**: README da API passa a incluir instrucoes de configuracao da chave JWT e pode ser seguido por novo colaborador sem consulta adicional.

## Premissas

- A chave JWT continuara sendo fornecida por configuracao externa (variavel de ambiente ou mecanismo equivalente ja suportado no projeto).
- Nao faz parte do escopo desta feature introduzir rotacao automatica de segredo ou gerenciador dedicado de segredos.
- O contrato funcional dos endpoints autenticados nao muda; a feature atua somente na seguranca de configuracao de startup.
- O arquivo appsettings versionado permanece no repositório como arquivo de exemplo sem segredo real.
