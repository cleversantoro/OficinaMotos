# Tasks: US-003 - Implementar Refresh Token

**Input**: Documentos de design em /specs/003-refresh-token/

**Pre-requisitos**: plan.md (obrigatorio), spec.md (obrigatorio), research.md, data-model.md, contracts/, quickstart.md

**Testes**: Esta feature exige testes unitarios no frontend para AuthService/errorInterceptor e validacao executavel minima com build da API e cenarios ponta a ponta descritos em quickstart.md.

**Organizacao**: Tarefas agrupadas por historia de usuario para permitir implementacao e validacao independente de cada incremento.

## Fase 1: Setup (Inicializacao)

**Objetivo**: Preparar trilha de validacao e contratos operacionais da feature.

- [ ] T001 Criar pasta de evidencias em specs/003-refresh-token/validation/.gitkeep
- [ ] T002 Criar matriz de validacao da feature em specs/003-refresh-token/validation/refresh-token-validation-matrix.md com base em specs/003-refresh-token/contracts/auth-refresh-contract.md e specs/003-refresh-token/quickstart.md

---

## Fase 2: Fundacional (Bloqueante)

**Objetivo**: Introduzir a infraestrutura compartilhada de persistencia e contratos de autenticacao que bloqueia todas as historias.

**⚠️ CRITICO**: Nenhuma historia deve iniciar antes da conclusao desta fase.

- [ ] T003 Adicionar entidade RefreshToken e navegacao em seg_usuarios em oficina-motos-api/src/OficinaMotos.Domain/Entities/Seguranca.cs
- [ ] T004 [P] Criar interface de repositorio para refresh tokens em oficina-motos-api/src/OficinaMotos.Domain/Interfaces/Repositories/SegurancaRepo/IRefreshTokenRepository.cs
- [ ] T005 Atualizar OficinaContext com DbSet de refresh token em oficina-motos-api/src/OficinaMotos.Infrastructure/Context/OficinaContext.cs
- [ ] T006 [P] Mapear seg_refresh_tokens em oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/SegurancaConfig/SegurancaConfiguration.cs
- [ ] T007 Implementar repositorio de refresh tokens em oficina-motos-api/src/OficinaMotos.Infrastructure/Repositories/SegurancaRepo/RefreshTokenRepository.cs
- [ ] T008 Criar migration EF para seg_refresh_tokens em oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations/
- [ ] T009 [P] Criar script SQL documentado da nova tabela em oficina-motos-docs/oficina_db_sql/oficina_db_table_seg_refresh_tokens.sql
- [ ] T010 Criar DTOs de request/response para refresh e logout em oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/Auth/RefreshTokenRequestDTO.cs, oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/Auth/LogoutRequestDTO.cs e oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/Auth/RefreshTokenResponseDTO.cs
- [ ] T011 Atualizar LoginResponseDTO para incluir refreshToken e refreshTokenExpiresAt em oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/Auth/LoginResponseDTO.cs
- [ ] T012 Evoluir IAuthService com operacoes de emissao, renovacao e revogacao de refresh token em oficina-motos-api/src/OficinaMotos.Application/Interfaces/Seguranca/IAuthService.cs

**Checkpoint**: Persistencia, contratos e documentação de schema prontos para sustentar as historias de usuario.

---

## Fase 3: User Story 1 - Renovar sessao sem interromper trabalho (Prioridade: P1) 🎯 MVP

**Meta**: Emitir refresh token no login, permitir renovacao de access token no backend e armazenar a sessao renovavel no frontend.

**Teste Independente**: Fazer login, obter refresh token, chamar /api/v1/Auth/refresh com token valido e confirmar novo access token sem exigir novo login manual.

### Testes da User Story 1

- [ ] T013 [P] [US1] Criar testes unitarios do AuthService cobrindo persistencia de access token e refresh token em oficina-motos-web/src/app/core/auth/auth.service.spec.ts
- [ ] T014 [US1] Registrar roteiro e evidencias do cenario de login + refresh valido em specs/003-refresh-token/validation/us1-login-refresh-valido.md

### Implementacao da User Story 1

- [ ] T015 [US1] Implementar geracao de refresh token, hash e persistencia no fluxo de login em oficina-motos-api/src/OficinaMotos.Application/Services/Seguranca/AuthService.cs
- [ ] T016 [US1] Atualizar AuthController para devolver refresh token no login e expor POST /api/v1/Auth/refresh em oficina-motos-api/src/OficinaMotos.API/Controllers/Auth/AuthController.cs
- [ ] T017 [US1] Atualizar modelos de autenticacao do frontend para incluir refresh token e expiracao em oficina-motos-web/src/app/core/auth/auth.model.ts
- [ ] T018 [US1] Registrar auth.login, auth.refresh e auth.logout em oficina-motos-web/src/app/core/services/api-paths.ts
- [ ] T019 [US1] Refatorar AuthService para usar apiPaths e persistir refresh token em oficina-motos-web/src/app/core/auth/auth.service.ts
- [ ] T020 [US1] Consolidar conformidade de FR-001, FR-002, FR-003 e SC-001 em specs/003-refresh-token/validation/us1-conformidade.md

**Checkpoint**: US1 funcional e validada de forma independente com login renovavel e refresh manual bem-sucedido.

---

## Fase 4: User Story 2 - Encerrar sessao de forma segura (Prioridade: P2)

**Meta**: Revogar o refresh token no logout para impedir reuso da sessao depois do encerramento.

**Teste Independente**: Autenticar, executar logout, tentar reutilizar o mesmo refresh token em /api/v1/Auth/refresh e confirmar rejeicao com 401.

### Testes da User Story 2

- [ ] T021 [US2] Registrar roteiro e evidencias de logout com tentativa posterior de refresh em specs/003-refresh-token/validation/us2-logout-revogacao.md

### Implementacao da User Story 2

- [ ] T022 [US2] Implementar revogacao de refresh token e auditoria de logout em oficina-motos-api/src/OficinaMotos.Application/Services/Seguranca/AuthService.cs
- [ ] T023 [US2] Expor POST /api/v1/Auth/logout com bearer JWT obrigatorio em oficina-motos-api/src/OficinaMotos.API/Controllers/Auth/AuthController.cs
- [ ] T024 [US2] Atualizar AuthService para chamar logout remoto antes de limpar sessao local em oficina-motos-web/src/app/core/auth/auth.service.ts
- [ ] T025 [US2] Consolidar conformidade de FR-005, FR-006, FR-010 e SC-002 em specs/003-refresh-token/validation/us2-conformidade.md

**Checkpoint**: US2 funcional e validada de forma independente com revogacao efetiva do refresh token no logout.

---

## Fase 5: User Story 3 - Tratar falhas de renovacao com clareza (Prioridade: P3)

**Meta**: Automatizar o refresh em respostas 401, evitar concorrencia/loop infinito e encerrar a sessao local quando a renovacao falhar.

**Teste Independente**: Simular access token expirado com refresh valido e confirmar retry unico; depois simular refresh invalido e confirmar limpeza da sessao e redirecionamento para login.

### Testes da User Story 3

- [ ] T026 [P] [US3] Ampliar testes do errorInterceptor para refresh unico, retry unico e falha com logout em oficina-motos-web/src/app/core/interceptors/error-interceptor.spec.ts
- [ ] T027 [US3] Registrar roteiro e evidencias dos cenarios de auto-refresh e falha controlada em specs/003-refresh-token/validation/us3-auto-refresh-falha-controlada.md

### Implementacao da User Story 3

- [ ] T028 [US3] Rejeitar refresh token expirado, revogado ou desconhecido com auditoria consistente em oficina-motos-api/src/OficinaMotos.Application/Services/Seguranca/AuthService.cs
- [ ] T029 [US3] Implementar operacao de refresh no AuthService do frontend com controle de sessao e atualizacao do token atual em oficina-motos-web/src/app/core/auth/auth.service.ts
- [ ] T030 [US3] Atualizar errorInterceptor para coordenar um unico refresh em voo, repetir requisicao uma vez e executar logout local em falha definitiva em oficina-motos-web/src/app/core/interceptors/error-interceptor.ts
- [ ] T031 [US3] Consolidar conformidade de FR-004, FR-007, FR-008, FR-009, SC-003 e SC-004 em specs/003-refresh-token/validation/us3-conformidade.md

**Checkpoint**: US3 funcional e validada de forma independente com auto-refresh seguro e tratamento claro de falhas.

---

## Fase 6: Polish e Itens Transversais

**Objetivo**: Fechar a feature com validacao final, documentacao atualizada e evidencias executaveis.

- [ ] T032 [P] Atualizar quickstart com resultados efetivamente executados em specs/003-refresh-token/quickstart.md
- [ ] T033 Executar build final da API com oficina-motos-api/OficinaMotos.slnx e registrar evidencia em specs/003-refresh-token/validation/final-backend-build.md
- [ ] T034 Executar suite de testes do frontend e registrar evidencia em specs/003-refresh-token/validation/final-frontend-tests.md
- [ ] T035 Consolidar resumo final da feature e rastreabilidade de criterios em specs/003-refresh-token/validation/final-validation-summary.md

---

## Dependencias e Ordem de Execucao

### Dependencias por Fase

1. Fase 1 (Setup): inicia imediatamente.
2. Fase 2 (Fundacional): depende da Fase 1 e bloqueia todas as historias.
3. Fase 3 (US1): depende da Fase 2.
4. Fase 4 (US2): depende da Fase 2 e pode evoluir em paralelo a US1, mas valida melhor quando o fluxo de login renovavel ja existir.
5. Fase 5 (US3): depende da Fase 2 e do endpoint funcional de refresh da US1 para validacao ponta a ponta.
6. Fase 6 (Polish): depende da conclusao das historias selecionadas.

### Dependencias entre Historias

1. US1 (P1): pronta para MVP apos Fase 2 e entrega o fluxo minimo de sessao renovavel.
2. US2 (P2): independente em regra de negocio apos Fase 2, mas reutiliza a persistencia e o contrato criados para US1.
3. US3 (P3): depende funcionalmente da existencia do refresh de US1 para validar auto-refresh e falha controlada no cliente.

### Dependencias dentro de cada Historia

1. Testes e roteiros de validacao primeiro.
2. Backend da historia antes do ajuste correspondente de cliente.
3. Conformidade e evidencias por ultimo.

---

## Oportunidades de Paralelismo

- T004 [P] e T006 [P] podem ser executadas em paralelo apos T003.
- T009 [P] pode ocorrer em paralelo com T008 durante a fase fundacional.
- T013 [P] pode ocorrer em paralelo com T014 enquanto a implementacao backend da US1 avanca.
- T026 [P] pode ocorrer em paralelo com T027 antes do ajuste final do interceptor.
- T032 [P] pode ocorrer em paralelo com T033 e T034 no fechamento.

---

## Exemplo de Execucao Paralela: User Story 1

```bash
# Em paralelo, apos concluir a Fase 2:
# T013 -> testes unitarios do AuthService no frontend
# T014 -> roteiro/evidencias de login + refresh valido
```

## Exemplo de Execucao Paralela: User Story 3

```bash
# Em paralelo, apos US1 funcional:
# T026 -> testes do errorInterceptor
# T027 -> roteiro/evidencias de auto-refresh e falha controlada
```

---

## Estrategia de Implementacao

### MVP Primeiro (Somente US1)

1. Concluir Fase 1.
2. Concluir Fase 2.
3. Concluir Fase 3 (US1).
4. Validar login renovavel e refresh manual com token valido.
5. Publicar MVP de sessao renovavel sem logout revogavel nem auto-refresh completo.

### Entrega Incremental

1. Setup + Fundacional.
2. US1 para emissao e renovacao manual do access token.
3. US2 para revogacao segura no logout.
4. US3 para auto-refresh, concorrencia controlada e falha com limpeza da sessao.
5. Polish final com build, testes e resumo de conformidade.

### Estrategia para Time em Paralelo

1. Pessoa A: T003-T008 no backend fundacional.
2. Pessoa B: T009-T014 em documentacao/testes enquanto a fundacao estabiliza.
3. Pessoa C: T017-T019 no frontend apos T016.
4. Fechamento conjunto: T032-T035.

---

## Notas

- [P] indica tarefas em arquivos diferentes, sem dependencia direta de tarefa incompleta.
- [US1], [US2] e [US3] garantem rastreabilidade por historia.
- Cada historia possui criterio de teste independente e artefato de evidencia correspondente.
- A feature envolve tres repositorios: API, web e documentacao SQL.
