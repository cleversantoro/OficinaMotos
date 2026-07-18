# Quickstart de Validacao - US-001

## Objetivo

Validar ponta a ponta que a autenticacao global esta ativa por padrao e que o endpoint de login permanece publico.

## Prerequisitos

- .NET SDK 8 instalado
- Banco de dados configurado conforme appsettings da API
- API compilando e iniciando localmente
- Ferramenta curl disponivel

## Setup

1. Entrar no diretorio da API:

```powershell
cd oficina-motos-api
```

1. Restaurar e compilar:

```powershell
dotnet restore OficinaMotos.slnx
dotnet build OficinaMotos.slnx
```

1. Iniciar API:

```powershell
dotnet run --project src/OficinaMotos.API/OficinaMotos.API.csproj
```

## Cenario 1 - Endpoint protegido sem token deve retornar 401

Comando (ajuste porta se necessario):

```bash
curl -i http://localhost:5000/api/v1/Clientes
```

Resultado esperado:

- Status HTTP 401 Unauthorized
- Sem payload de sucesso de negocio

## Cenario 2 - Endpoint de login sem token deve permanecer acessivel

Comando:

```bash
curl -i -X POST http://localhost:5000/api/v1/Auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"usuario@oficina.com\",\"password\":\"senha\"}"
```

Resultado esperado:

- Endpoint responde sem exigir Bearer token previamente
- Status de negocio esperado: 200 (credenciais validas) ou 401 (credenciais invalidas) ou 400 (payload invalido)
- Nao deve ocorrer bloqueio por ausencia de token na chamada de login

## Cenario 3 - Endpoint protegido com token invalido deve retornar 401

Comando:

```bash
curl -i http://localhost:5000/api/v1/Clientes \
  -H "Authorization: Bearer token_invalido"
```

Resultado esperado:

- Status HTTP 401 Unauthorized

## Referencias

- Especificacao: [spec.md](./spec.md)
- Plano: [plan.md](./plan.md)
- Pesquisa: [research.md](./research.md)
- Modelo de dados: [data-model.md](./data-model.md)
- Contrato: [contracts/auth-fallback-policy.md](./contracts/auth-fallback-policy.md)

## Resultados Executados Nesta Implementacao

- GET /api/v1/Clientes sem token: 401 Unauthorized.
- GET /api/v1/Clientes com token invalido: 401 Unauthorized.
- POST /api/v1/Auth/login sem token: endpoint acessivel e processado (retornou 401 de negocio para credenciais invalidas).
- Build final: sucesso com avisos conhecidos de vulnerabilidade NU1902 em pacote OpenTelemetry.
