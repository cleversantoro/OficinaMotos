# Tasks: US-001 - Autenticacao Global por Padrao

**Input**: Documentos de design em /specs/001-global-auth-fallbackpolicy/

**Pre-requisitos**: plan.md (obrigatorio), spec.md (obrigatorio), research.md, data-model.md, contracts/, quickstart.md

**Testes**: Esta feature exige validacao manual via curl/HTTP conforme criterio de aceite (sem TDD automatizado obrigatorio neste escopo).

**Organizacao**: Tarefas agrupadas por historia de usuario para permitir implementacao e validacao independente.

## Fase 1: Setup (Inicializacao)

**Objetivo**: Preparar baseline e artefatos de evidencia para implementacao segura.

- [x] T001 Validar baseline de build da API com oficina-motos-api/OficinaMotos.slnx e registrar resultado em specs/001-global-auth-fallbackpolicy/validation/baseline-build.md
- [x] T002 Criar matriz de verificacao dos endpoints desta feature em specs/001-global-auth-fallbackpolicy/validation/endpoint-access-matrix.md usando specs/001-global-auth-fallbackpolicy/contracts/auth-fallback-policy.md como referencia

---

## Fase 2: Fundacional (Bloqueante)

**Objetivo**: Aplicar infraestrutura de autorizacao global que bloqueia todas as historias ate concluir.

**⚠️ CRITICO**: Nenhuma tarefa de historia de usuario deve iniciar antes desta fase.

- [x] T003 Adicionar import de autorizacao em oficina-motos-api/src/OficinaMotos.API/Program.cs para habilitar AuthorizationPolicyBuilder
- [x] T004 Configurar FallbackPolicy com RequireAuthenticatedUser em oficina-motos-api/src/OficinaMotos.API/Program.cs via builder.Services.AddAuthorization(options => ...)
- [x] T005 Validar ordem e presenca de middleware app.UseAuthentication()/app.UseAuthorization() em oficina-motos-api/src/OficinaMotos.API/Program.cs

**Checkpoint**: Base de autenticacao global pronta; historias podem iniciar.

---

## Fase 3: User Story 1 - Bloqueio Padrao de Endpoints de Negocio (Prioridade: P1) 🎯 MVP

**Meta**: Garantir que endpoints de negocio retornem 401 quando nao houver token JWT valido.

**Teste Independente**: Chamar GET /api/v1/Clientes sem token e com token invalido e confirmar 401.

### Testes da User Story 1

- [x] T006 [US1] Executar teste manual sem token para GET /api/v1/Clientes (controller em oficina-motos-api/src/OficinaMotos.API/Controllers/Cliente/ClientesController.cs) e registrar evidencia em specs/001-global-auth-fallbackpolicy/validation/us1-clientes-sem-token.md
- [x] T007 [US1] Executar teste manual com token invalido para GET /api/v1/Clientes e registrar evidencia em specs/001-global-auth-fallbackpolicy/validation/us1-clientes-token-invalido.md

### Implementacao da User Story 1

- [x] T008 [P] [US1] Auditar controladores de negocio em oficina-motos-api/src/OficinaMotos.API/Controllers/ para confirmar ausencia de [AllowAnonymous] indevido e registrar inventario em specs/001-global-auth-fallbackpolicy/validation/us1-auditoria-controllers.md
- [x] T009 [US1] Consolidar resultado de cobertura da politica global (56 controladores) em specs/001-global-auth-fallbackpolicy/validation/us1-cobertura-fallbackpolicy.md

**Checkpoint**: US1 funcional e validada de forma independente.

---

## Fase 4: User Story 2 - Excecao Controlada para Login Publico (Prioridade: P2)

**Meta**: Garantir que o endpoint de login permaneça publico mesmo com FallbackPolicy global ativa.

**Teste Independente**: Chamar POST /api/v1/Auth/login sem token e confirmar que a requisicao e processada (200/400/401 de negocio).

### Testes da User Story 2

- [x] T010 [P] [US2] Executar chamada sem token para POST /api/v1/Auth/login usando oficina-motos-api/src/OficinaMotos.API/Controllers/Auth/AuthController.cs e registrar evidencia em specs/001-global-auth-fallbackpolicy/validation/us2-login-sem-token.md
- [x] T011 [US2] Validar respostas de negocio do login (200/400/401) sem bloqueio por autenticacao previa e registrar evidencia em specs/001-global-auth-fallbackpolicy/validation/us2-login-respostas.md

### Implementacao da User Story 2

- [x] T012 [P] [US2] Adicionar atributo [AllowAnonymous] na action Login em oficina-motos-api/src/OficinaMotos.API/Controllers/Auth/AuthController.cs
- [x] T013 [US2] Confirmar compatibilidade do contrato de acesso publico em specs/001-global-auth-fallbackpolicy/contracts/auth-fallback-policy.md apos ajuste do AuthController

**Checkpoint**: US2 funcional e validada de forma independente.

---

## Fase 5: Polish e Itens Transversais

**Objetivo**: Fechamento da feature com validacao final e documentacao consolidada.

- [x] T014 [P] Atualizar passos finais de validacao em specs/001-global-auth-fallbackpolicy/quickstart.md com comandos efetivamente executados
- [x] T015 Executar build final da API via oficina-motos-api/OficinaMotos.slnx e registrar resultado em specs/001-global-auth-fallbackpolicy/validation/final-build.md
- [x] T016 Consolidar resumo final de conformidade dos criterios SC-001 a SC-004 em specs/001-global-auth-fallbackpolicy/validation/final-validation-summary.md

---

## Dependencias e Ordem de Execucao

### Dependencias por Fase

1. Fase 1 (Setup): inicia imediatamente.
2. Fase 2 (Fundacional): depende da Fase 1 e bloqueia todas as historias.
3. Fase 3 (US1): depende da Fase 2.
4. Fase 4 (US2): depende da Fase 2 e pode ocorrer em paralelo com US1 se houver equipe suficiente.
5. Fase 5 (Polish): depende da conclusao das historias selecionadas.

### Dependencias entre Historias

1. US1 (P1): pronta para MVP apos Fase 2.
2. US2 (P2): tecnicamente independente de US1 apos Fase 2, mas priorizada depois de US1 por valor de seguranca.

### Dependencias dentro de cada Historia

1. Validacoes manuais primeiro.
2. Ajustes de codigo em seguida.
3. Consolidacao de evidencias por ultimo.

---

## Oportunidades de Paralelismo

- T008 [P] [US1] pode rodar em paralelo com T006/T007 (arquivos de evidencia diferentes).
- T014 [P] pode rodar em paralelo com T015 (quickstart vs evidencias de build).

---

## Exemplo de Execucao Paralela: User Story 1

```bash
# Em paralelo, apos concluir a Fase 2:
# T006 -> validar 401 sem token no endpoint de clientes
# T008 -> auditar controllers para ausencia de AllowAnonymous indevido
```

## Exemplo de Execucao Paralela: User Story 2

```bash
# Em paralelo, apos concluir a Fase 2:
# T010 -> validar login sem token
# T012 -> adicionar AllowAnonymous no AuthController.Login
```

---

## Estrategia de Implementacao

### MVP Primeiro (Somente US1)

1. Concluir Fase 1.
2. Concluir Fase 2.
3. Concluir Fase 3 (US1).
4. Validar 401 em endpoint de negocio e cobertura de controladores.
5. Publicar MVP de seguranca global.

### Entrega Incremental

1. Setup + Fundacional.
2. US1 (seguranca global dos endpoints de negocio).
3. US2 (excecao publica de login).
4. Polish final com evidencias e resumo de conformidade.

### Estrategia para Time em Paralelo

1. Pessoa A: T003-T005 (fundacional).
2. Pessoa B: T006-T009 (US1) apos fundacional.
3. Pessoa C: T010-T013 (US2) apos fundacional.
4. Fechamento conjunto: T014-T016.

---

## Notas

- [P] indica tarefas em arquivos diferentes, sem dependencia direta de tarefa incompleta.
- [US1]/[US2] garante rastreabilidade de cada historia.
- Cada historia possui criterio de teste independente.
- Evitar alterar contratos de endpoint fora do escopo desta feature.
