# Plano de Implementacao: US-003 - Implementar Refresh Token

**Branch**: `[003-refresh-token]` | **Data**: 2026-07-18 | **Spec**: [spec.md](./spec.md)

**Entrada**: Especificacao da feature em [specs/003-refresh-token/spec.md](./spec.md)

## Resumo

Estender o fluxo atual de autenticacao baseado em JWT para emitir e persistir refresh token por sessao, permitindo renovacao controlada do access token sem exigir novo login manual durante o uso do sistema.

Implementar no backend uma nova entidade seg_ para persistencia do refresh token, evoluir o contrato de login para retornar a credencial de renovacao, adicionar os endpoints [POST] /api/v1/Auth/refresh e [POST] /api/v1/Auth/logout, e registrar auditoria coerente com as politicas existentes.

No frontend Angular, evoluir o AuthService para armazenar e revogar a nova credencial, registrar os endpoints em apiPaths e alterar o errorInterceptor para executar um unico fluxo de auto-refresh por vez, repetir a requisicao original apenas uma vez e encerrar a sessao quando a renovacao falhar.

## Contexto Tecnico

**Linguagem/Versao**: C# com .NET 8 no backend e TypeScript 5.9 com Angular 21 no frontend

**Dependencias Principais**:

- Backend: ASP.NET Core Web API, Microsoft.AspNetCore.Authentication.JwtBearer 8.0.22, Entity Framework Core 8, Pomelo.EntityFrameworkCore.MySql 8.0.3
- Frontend: Angular HttpClient funcional com interceptors, RxJS 7.8, Signals no AuthService, PrimeNG/Tailwind sem impacto direto no fluxo de auth

**Armazenamento**: MySQL 8+ via EF Core; nova tabela seg_refresh_tokens e sincronizacao da documentacao SQL em oficina-motos-docs

**Testing**: dotnet build da solucao + validacao manual dos endpoints de auth; frontend com testes unitarios em .spec.ts para AuthService e errorInterceptor via ng test/TestBed

**Plataforma Alvo**: API ASP.NET Core hospedavel em Windows/Linux e SPA Angular executada em navegadores modernos

**Tipo de Projeto**: Aplicacao web em multiplos repositorios com backend API, frontend SPA e repositorio de documentacao SQL

**Metas de Performance**:

- Renovacao automatica de sessao concluida em ate 5 segundos nos cenarios validos definidos pela spec
- Apenas um refresh em voo por cliente ao receber 401 concorrentes

**Restricoes**:

- JWT continua obrigatorio para endpoints protegidos, com excecao justificada do endpoint de refresh
- Frontend deve usar [apiPaths](../../oficina-motos-web/src/app/core/services/api-paths.ts) em vez de URLs hardcoded
- Refresh token deve ser invalidavel por logout e inutilizavel apos expiracao ou revogacao
- Uma requisicao protegida pode ser repetida no maximo uma vez apos refresh bem-sucedido
- Auditoria permanece INSERT-ONLY em seg_audit_log

**Escala/Escopo**:

- 1 entidade nova de seguranca com migration EF
- 2 novos endpoints de auth e extensao nao quebrante do contrato de login
- Ajustes no slice de auth do frontend e em seus testes
- Atualizacao de documentacao tecnica de banco e contratos da feature

## Verificacao da Constituicao

*GATE: deve passar antes da Fase 0 e ser revalidado apos a Fase 1.*

### Gates pre-Fase 0

1. Principio I (Dominio Primeiro): PASSA

- A feature permanece confinada ao bounded context de Seguranca, sem mover regra de negocio para controller ou UI.

1. Principio II (API RESTful Versionada): PASSA

- Os novos endpoints seguem o padrao /api/v1/Auth/* e o frontend deve registra-los em [oficina-motos-web/src/app/core/services/api-paths.ts](../../oficina-motos-web/src/app/core/services/api-paths.ts).

1. Principio III (Seguranca por Design): PASSA COM EXCECAO JUSTIFICADA

- Login continua anonimo e logout permanece autenticado por JWT.
- O endpoint de refresh precisa aceitar apenas refresh token valido, porque seu objetivo e exatamente recuperar uma sessao cujo access token expirou.
- A excecao fica limitada a um endpoint de troca de credencial com controles compensatorios: token opaco, hash em repouso, expiracao, revogacao, auditoria e rejeicao de replay apos logout.

1. Principio IV (Frontend Reativo com Componentes Standalone): PASSA

- A mudanca fica concentrada em AuthService, interceptors funcionais e constantes centrais, sem introduzir NgModules nem estado paralelo ao slice atual.

1. Principio V (Integridade e Rastreabilidade de Dados): PASSA

- A nova persistencia segue prefixo seg_, PK bigint, FK explicita para seg_usuarios, campos de auditoria base e exclusao logica por revogacao.

1. Principio VI (Qualidade e Testabilidade): PASSA

- O plano inclui testes unitarios do frontend para o fluxo de 401 e retry unico; o backend atualmente nao possui projeto dedicado de testes, entao a validacao executavel minima fica em build e cenarios de endpoint desta feature.

1. Principio VII (Documentacao como Fonte de Verdade): PASSA

- O desenho inclui atualizacao da documentacao SQL em oficina-motos-docs e contratos formais da feature em specs/003-refresh-token.

**Resultado pre-Fase 0**: PASS

### Revalidacao pos-Fase 1

1. O modelo de dados define tabela seg_refresh_tokens com relacionamento explicito a seg_usuarios: PASSA

2. O contrato registra login, refresh e logout na mesma superficie versionada de Auth e corrige o uso de apiPaths no frontend: PASSA

3. O quickstart cobre sucesso, revogacao e falha controlada de auto-refresh: PASSA

4. A excecao de seguranca do endpoint de refresh esta documentada e justificada em Complexidade: PASSA

**Resultado pos-Fase 1**: PASS

## Estrutura do Projeto

### Documentacao da Feature

```text
specs/003-refresh-token/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
|   └── auth-refresh-contract.md
└── tasks.md
```

### Codigo-fonte (raiz do repositorio)

```text
oficina-motos-api/
├── src/
|   ├── OficinaMotos.API/
|   |   └── Controllers/
|   |       └── Auth/
|   |           └── AuthController.cs
|   ├── OficinaMotos.Application/
|   |   ├── DTOs/
|   |   |   ├── Requests/
|   |   |   |   └── Auth/
|   |   |   └── Responses/
|   |   |       └── Auth/
|   |   ├── Interfaces/
|   |   |   └── Seguranca/
|   |   └── Services/
|   |       └── Seguranca/
|   |           └── AuthService.cs
|   ├── OficinaMotos.Domain/
|   |   ├── Entities/
|   |   |   └── Seguranca.cs
|   |   └── Interfaces/
|   |       └── Repositories/
|   |           └── SegurancaRepo/
|   └── OficinaMotos.Infrastructure/
|       ├── Context/
|       |   └── OficinaContext.cs
|       ├── EntitiesConfiguration/
|       |   └── SegurancaConfig/
|       |       └── SegurancaConfiguration.cs
|       ├── Migrations/
|       └── Repositories/
|           └── SegurancaRepo/
├── README.md
└── OficinaMotos.slnx

oficina-motos-web/
└── src/app/
    ├── app.config.ts
    └── core/
        ├── auth/
        |   ├── auth.model.ts
        |   ├── auth.service.ts
        |   └── auth.guard.ts
        ├── interceptors/
        |   ├── error-interceptor.ts
        |   └── error-interceptor.spec.ts
        └── services/
            ├── api-client.service.ts
            └── api-paths.ts

oficina-motos-docs/
└── oficina_db_sql/
    └── oficina_db_table_seg_refresh_tokens.sql
```

**Decisao de Estrutura**: Feature transversal entre backend, frontend e documentacao SQL. A persistencia e os contratos de auth ficam no modulo de Seguranca da API; o comportamento de renovacao automatica fica no slice core/auth e core/interceptors do frontend; a tabela nova precisa ser refletida na pasta de schema do repositorio de documentacao.

## Rastreamento de Complexidade

- Violacao: Endpoint /api/v1/Auth/refresh sem JWT bearer valido.
- Why Needed: O refresh existe para recuperar uma sessao justamente quando o access token expirou; exigir bearer valido inviabilizaria o caso de uso principal.
- Simpler Alternative Rejected Because: Exigir JWT atual no refresh impediria renovacao apos expiracao e forcaria novo login manual, contrariando a spec.
