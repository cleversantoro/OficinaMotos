# Plano de Implementacao: US-002 - Remover Chave JWT Hardcoded

**Branch**: `[002-remove-jwt-hardcoded]` | **Data**: 2026-07-18 | **Spec**: [spec.md](./spec.md)

**Entrada**: Especificacao da feature em [specs/002-remove-jwt-hardcoded/spec.md](./spec.md)

## Resumo

Remover o fallback hardcoded da chave JWT no bootstrap da API e forcar validacao fail-fast na inicializacao quando o segredo obrigatorio estiver ausente ou vazio.
Substituir valor sensivel versionado por placeholder vazio no appsettings versionado.
Atualizar README da API com instrucoes objetivas para configuracao da chave JWT via ambiente/configuracao externa.

## Contexto Tecnico

**Linguagem/Versao**: C# com .NET 8 (ASP.NET Core Web API, TargetFramework net8.0)

**Dependencias Principais**:

- Microsoft.AspNetCore.Authentication.JwtBearer 8.0.22
- Microsoft.IdentityModel.Tokens
- Serilog e OpenTelemetry (nao alterados nesta feature)

**Armazenamento**: MySQL 8+ (sem mudanca de schema nesta feature)

**Testing**: Validacao por inicializacao da API com e sem configuracao de JWT + dotnet build da solucao

**Plataforma Alvo**: API backend em ambiente servidor Windows/Linux

**Tipo de Projeto**: Web service backend em arquitetura em camadas (API/Application/Domain/Infrastructure)

**Metas de Performance**: Sem meta nova de desempenho; alteracao focada em seguranca de startup

**Restricoes**:

- Nao permitir segredo padrao implicito no codigo
- Mensagem de erro de startup deve ser clara e acionavel
- Manter compatibilidade do fluxo JWT existente quando chave estiver configurada

**Escala/Escopo**:

- Arquivos principais: Program.cs, appsettings.json (API), README.md (API)
- Sem alteracao de contratos de endpoint e sem mudanca de modelo de dados

## Verificacao da Constituicao

*GATE: deve passar antes da Fase 0 e ser revalidado apos a Fase 1.*

### Gates pre-Fase 0

1. Principio III (Seguranca por Design): PASSA

- Remove segredo hardcoded e reforca controle de autenticacao JWT obrigatoria.

1. Principio II (API RESTful Versionada): PASSA

- Nao altera rotas, versao de API nem contratos HTTP funcionais dos endpoints.

1. Principio VII (Documentacao como Fonte de Verdade): PASSA

- Inclui atualizacao do README com instrucao de configuracao segura.

1. Integridade de configuracao: PASSA

- Sem segredo versionado e com fail-fast explicito.

**Resultado pre-Fase 0**: PASS

### Revalidacao pos-Fase 1

1. Design remove fallback hardcoded e adiciona validacao obrigatoria de chave JWT: PASSA

2. Contrato de configuracao externa documenta erro de startup esperado quando segredo ausente: PASSA

3. Guia de validacao cobre cenarios sem chave e com chave configurada: PASSA

**Resultado pos-Fase 1**: PASS

## Estrutura do Projeto

### Documentacao da Feature

```text
specs/002-remove-jwt-hardcoded/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── jwt-configuration-contract.md
└── tasks.md
```

### Codigo-fonte (raiz do repositorio)

```text
oficina-motos-api/
├── README.md
└── src/
    └── OficinaMotos.API/
        ├── Program.cs
        └── appsettings.json
```

**Decisao de Estrutura**: Implementacao focada em backend no projeto OficinaMotos.API e documentacao operacional no README da API.

## Rastreamento de Complexidade

Nenhuma violacao da constituicao identificada. Secao de justificativa nao aplicavel.
