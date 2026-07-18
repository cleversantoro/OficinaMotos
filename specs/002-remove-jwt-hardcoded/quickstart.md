# Quickstart de Validacao - US-002

## Objetivo

Validar que a API nao sobe sem chave JWT configurada e que o segredo nao fica versionado no appsettings.

## Prerequisitos

- .NET SDK 8 instalado
- Dependencias restauradas
- Acesso ao projeto oficina-motos-api

## Setup

1. Entrar no diretorio da API:

```powershell
cd oficina-motos-api
```

1. Restaurar dependencias:

```powershell
dotnet restore OficinaMotos.slnx
```

## Cenario 1 - Fail-fast sem chave JWT

1. Garantir que Jwt:Key esteja ausente ou vazia na configuracao efetiva.
2. Executar a API:

```powershell
dotnet run --project src/OficinaMotos.API/OficinaMotos.API.csproj
```

Resultado esperado:

- Startup falha imediatamente.
- Excecao do tipo InvalidOperationException.
- Mensagem clara indicando que Jwt:Key obrigatoria nao foi configurada.

## Cenario 2 - Startup com chave JWT valida via ambiente

PowerShell (sessao atual):

```powershell
$env:Jwt__Key = "minha-chave-super-segura-ambiente-local"
```

Executar:

```powershell
dotnet run --project src/OficinaMotos.API/OficinaMotos.API.csproj
```

Resultado esperado:

- API inicia com sucesso.
- Pipeline JWT fica funcional sem fallback hardcoded.

## Cenario 3 - Verificacao de segredo versionado

Arquivo a revisar:

- src/OficinaMotos.API/appsettings.json

Resultado esperado:

- Campo Jwt:Key presente como placeholder vazio.
- Nenhum segredo real versionado no repositorio.

## Cenario 4 - Verificacao de documentacao operacional

Arquivo a revisar:

- README.md (raiz de oficina-motos-api)

Resultado esperado:

- Instrucoes claras para configurar chave JWT obrigatoria via ambiente/configuracao externa.

## Referencias

- Especificacao: [spec.md](./spec.md)
- Plano: [plan.md](./plan.md)
- Pesquisa: [research.md](./research.md)
- Modelo de dados: [data-model.md](./data-model.md)
- Contrato: [contracts/jwt-configuration-contract.md](./contracts/jwt-configuration-contract.md)

## Resultados Executados Nesta Implementacao

- Startup sem Jwt:Key: falhou com InvalidOperationException e mensagem clara de configuracao obrigatoria.
- Startup com Jwt__Key via ambiente: API iniciou e respondeu 401 em endpoint protegido (servico ativo).
- appsettings.json: Jwt:Key mantido como placeholder vazio.
- README: instrucoes operacionais de configuracao externa da chave JWT atualizadas.
- Build final: sucesso com avisos conhecidos NU1902 no pacote OpenTelemetry.
