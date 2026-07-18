# Contrato de Configuracao - JWT sem Segredo Hardcoded

## Objetivo

Definir o contrato de configuracao obrigatoria da chave JWT para inicializacao segura da API.

## Escopo

- Projeto: oficina-motos-api
- Arquivos/locais impactados:
  - src/OficinaMotos.API/Program.cs
  - src/OficinaMotos.API/appsettings.json
  - README.md

## Regras Contratuais

1. Nao pode existir fallback hardcoded para chave JWT em codigo.
2. Chave JWT obrigatoria deve ser lida de configuracao externa.
3. Ausencia, vazio ou whitespace deve falhar startup com InvalidOperationException.
4. Arquivo appsettings versionado deve conter placeholder vazio para Jwt:Key.
5. README deve documentar como definir a chave JWT antes de subir a API.

## Contrato de Entrada

### Chave de configuracao esperada

- Caminho logico: Jwt:Key
- Forma valida:
  - String nao vazia
  - String nao composta apenas por espacos

### Fontes aceitas

- Variavel de ambiente (hierarquia equivalente para Jwt:Key)
- Provider de configuracao externo suportado pelo host
- appsettings local nao versionado (quando aplicavel)

## Contrato de Saida

### Startup com configuracao invalida

- Resultado: falha imediata de inicializacao
- Excecao: InvalidOperationException
- Mensagem esperada: deve indicar claramente que Jwt:Key e obrigatoria

### Startup com configuracao valida

- Resultado: inicializacao normal da API
- JWT bearer configurado com a chave fornecida externamente

## Criterios de Conformidade

- C1: Program.cs sem segredo default hardcoded
- C2: Program.cs com validacao fail-fast para Jwt:Key
- C3: appsettings.json versionado sem segredo real
- C4: README atualizado com instrucao de configuracao obrigatoria
