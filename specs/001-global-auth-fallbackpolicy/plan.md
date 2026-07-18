# Plano de Implementacao: US-001 - Autenticacao Global por Padrao

**Branch**: `[001-global-auth-fallbackpolicy]` | **Data**: 2026-07-18 | **Spec**: [spec.md](./spec.md)

**Entrada**: Especificacao da feature em [specs/001-global-auth-fallbackpolicy/spec.md](./spec.md)

## Resumo

Aplicar autenticacao obrigatoria por padrao na API via FallbackPolicy para proteger todos os endpoints de negocio sem depender de decoracao individual com [Authorize].
Manter excecao explicita para o endpoint publico de login em AuthController com [AllowAnonymous].
Validar comportamento com chamadas sem token, garantindo resposta 401 em endpoint de negocio representativo e consistencia para os 56 controladores de negocio.

## Contexto Tecnico

**Linguagem/Versao**: C# com .NET 8 (ASP.NET Core Web API, TargetFramework net8.0)

**Dependencias Principais**:

- Microsoft.AspNetCore.Authentication.JwtBearer 8.0.22
- Microsoft.EntityFrameworkCore + Pomelo.EntityFrameworkCore.MySql
- Swashbuckle.AspNetCore
- Serilog
- OpenTelemetry

**Armazenamento**: MySQL 8+ via Entity Framework Core (OficinaContext)

**Testes**: Sem projeto de testes automatizados dedicado no repositório da API; validacao desta feature via chamadas HTTP (curl/Swagger) e build da solucao

**Plataforma Alvo**: API backend executada em ambiente servidor (Windows/Linux) para consumo via HTTP

**Tipo de Projeto**: Web service backend em arquitetura em camadas (API/Application/Domain/Infrastructure)

**Metas de Performance**: Sem meta nova de throughput para esta feature; manter impacto desprezivel no pipeline de autenticacao existente

**Restricoes**:

- Nao quebrar endpoint publico de login em /api/v1/Auth/login
- Nao alterar o modelo de emissao de JWT existente
- Preservar respostas HTTP semanticas (401 para nao autenticado)

**Escala/Escopo**:

- 56 controladores de negocio no escopo de protecao padrao
- 1 excecao publica obrigatoria (login)
- Alteracao concentrada na configuracao de autorizacao e no controlador de autenticacao

## Verificacao da Constituicao

*GATE: deve passar antes da Fase 0 e ser revalidado apos a Fase 1.*

### Gates pre-Fase 0

1. Principio II (API RESTful Versionada): PASSA

- Endpoint de login permanece em /api/v1/Auth/login, sem mudanca de versionamento.

1. Principio III (Seguranca por Design): PASSA

- Reforca regra nao negociavel de JWT obrigatorio para endpoints protegidos e excecao somente para login.

1. Principio VII (Documentacao como Fonte de Verdade): PASSA

- Artefatos de spec/plan/research/model/contracts/quickstart serao mantidos no fluxo Speckit da feature.

1. Regras de qualidade e rastreabilidade: PASSA

- Sem alteracao de modelo de dados; mudanca apenas de politica de autorizacao e contrato de acesso.

**Resultado pre-Fase 0**: PASS

### Revalidacao pos-Fase 1

1. Design manteve JWT como autenticacao padrao e login publico explicitamente anotado: PASSA

2. Contrato de acesso documenta 401 para endpoints protegidos e 200/400/401 para login: PASSA

3. Guia de validacao garante verificacao ponta a ponta da regra de seguranca: PASSA

**Resultado pos-Fase 1**: PASS

## Estrutura do Projeto

### Documentacao da Feature

```text
specs/001-global-auth-fallbackpolicy/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── auth-fallback-policy.md
└── tasks.md
```

### Codigo-fonte (raiz do repositorio)

```text
oficina-motos-api/
├── OficinaMotos.slnx
└── src/
    ├── OficinaMotos.API/
    │   ├── Program.cs
    │   └── Controllers/
    │       └── Auth/
    │           └── AuthController.cs
    ├── OficinaMotos.Application/
    ├── OficinaMotos.Domain/
    └── OficinaMotos.Infrastructure/

oficina-motos-docs/
oficina-motos-web/
```

**Decisao de Estrutura**: Esta feature sera implementada apenas no backend em oficina-motos-api, com mudanca de configuracao global em Program.cs e ajuste de anotacao no AuthController, mantendo os artefatos de planejamento em specs/001-global-auth-fallbackpolicy.

## Rastreamento de Complexidade

Nenhuma violacao da constituicao identificada. Secao de justificativa nao aplicavel.
