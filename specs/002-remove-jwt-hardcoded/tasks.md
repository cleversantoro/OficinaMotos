# Tasks: US-002 - Remover Chave JWT Hardcoded

**Input**: Documentos de design em /specs/002-remove-jwt-hardcoded/

**Pre-requisitos**: plan.md (obrigatorio), spec.md (obrigatorio), research.md, data-model.md, contracts/, quickstart.md

**Testes**: Esta feature exige validacao de startup e verificacoes manuais conforme criterios de aceite (sem obrigatoriedade de suite automatizada nova).

**Organizacao**: Tarefas agrupadas por historia de usuario para permitir implementacao e validacao independente.

## Fase 1: Setup (Inicializacao)

**Objetivo**: Preparar baseline e trilha de evidencias da feature.

- [x] T001 Validar baseline de build da API com oficina-motos-api/OficinaMotos.slnx e registrar em specs/002-remove-jwt-hardcoded/validation/baseline-build.md
- [x] T002 Criar matriz de validacao da configuracao JWT em specs/002-remove-jwt-hardcoded/validation/jwt-validation-matrix.md com base em specs/002-remove-jwt-hardcoded/contracts/jwt-configuration-contract.md

---

## Fase 2: Fundacional (Bloqueante)

**Objetivo**: Eliminar fallback inseguro e garantir regra fail-fast de inicializacao.

**⚠️ CRITICO**: Nenhuma historia deve iniciar antes da conclusao desta fase.

- [x] T003 Ajustar leitura de Jwt:Key sem fallback hardcoded em oficina-motos-api/src/OficinaMotos.API/Program.cs
- [x] T004 Implementar validacao fail-fast para Jwt:Key (ausente/vazia/whitespace) com InvalidOperationException em oficina-motos-api/src/OficinaMotos.API/Program.cs
- [x] T005 Confirmar que a configuracao JWT continua funcional com chave valida em oficina-motos-api/src/OficinaMotos.API/Program.cs

**Checkpoint**: Startup seguro por configuracao obrigatoria estabelecido.

---

## Fase 3: User Story 1 - Inicializacao Segura sem Segredo Versionado (Prioridade: P1) 🎯 MVP

**Meta**: Impedir que a API suba sem chave JWT valida e eliminar segredo padrao embutido.

**Teste Independente**: Rodar API sem chave configurada e confirmar falha imediata com mensagem clara.

### Testes da User Story 1

- [x] T006 [US1] Executar startup sem Jwt:Key configurada e registrar evidencia da excecao em specs/002-remove-jwt-hardcoded/validation/us1-failfast-sem-chave.md
- [x] T007 [US1] Executar startup com Jwt:Key valida via ambiente e registrar evidencia de inicializacao em specs/002-remove-jwt-hardcoded/validation/us1-startup-com-chave.md

### Implementacao da User Story 1

- [x] T008 [P] [US1] Revisar Program.cs para confirmar ausencia de segredo literal hardcoded e registrar em specs/002-remove-jwt-hardcoded/validation/us1-auditoria-program.md
- [x] T009 [US1] Consolidar atendimento de FR-001, FR-002, FR-003 e FR-006 em specs/002-remove-jwt-hardcoded/validation/us1-conformidade-fr.md

**Checkpoint**: US1 funcional e validada de forma independente.

---

## Fase 4: User Story 2 - Configuracao e Documentacao Operacional Clara (Prioridade: P2)

**Meta**: Garantir configuracao versionada sem segredo e documentacao clara de setup da chave JWT.

**Teste Independente**: Verificar appsettings com placeholder vazio e README com instrucoes de configuracao externa.

### Testes da User Story 2

- [x] T010 [US2] Verificar que Jwt:Key versionada esta vazia em oficina-motos-api/src/OficinaMotos.API/appsettings.json e registrar em specs/002-remove-jwt-hardcoded/validation/us2-appsettings-placeholder.md
- [x] T011 [US2] Validar completude das instrucoes de configuracao JWT no README em oficina-motos-api/README.md e registrar em specs/002-remove-jwt-hardcoded/validation/us2-readme-validacao.md

### Implementacao da User Story 2

- [x] T012 [US2] Atualizar placeholder de segredo JWT para valor vazio em oficina-motos-api/src/OficinaMotos.API/appsettings.json
- [x] T013 [US2] Atualizar documentacao de variaveis/configuracao JWT obrigatoria em oficina-motos-api/README.md

**Checkpoint**: US2 funcional e validada de forma independente.

---

## Fase 5: Polish e Itens Transversais

**Objetivo**: Fechar a feature com validacao final e rastreabilidade completa.

- [x] T014 [P] Atualizar resultados efetivamente executados em specs/002-remove-jwt-hardcoded/quickstart.md
- [x] T015 Executar build final da API via oficina-motos-api/OficinaMotos.slnx e registrar em specs/002-remove-jwt-hardcoded/validation/final-build.md
- [x] T016 Consolidar conformidade de SC-001 a SC-004 em specs/002-remove-jwt-hardcoded/validation/final-validation-summary.md

---

## Dependencias e Ordem de Execucao

### Dependencias por Fase

1. Fase 1 (Setup): inicia imediatamente.
2. Fase 2 (Fundacional): depende da Fase 1 e bloqueia todas as historias.
3. Fase 3 (US1): depende da Fase 2.
4. Fase 4 (US2): depende da Fase 2 e pode ocorrer em paralelo com US1.
5. Fase 5 (Polish): depende da conclusao das historias selecionadas.

### Dependencias entre Historias

1. US1 (P1): pronta para MVP apos Fase 2.
2. US2 (P2): independente de US1 apos Fase 2, priorizada depois por valor de seguranca operacional.

### Dependencias dentro de cada Historia

1. Validacao de cenario primeiro.
2. Ajustes de codigo/configuracao em seguida.
3. Consolidacao de evidencias por ultimo.

---

## Oportunidades de Paralelismo

- T008 [P] [US1] pode rodar em paralelo com T006/T007 (auditoria vs execucao de cenarios).
- T014 [P] pode rodar em paralelo com T015 (documentacao de quickstart vs build final).

---

## Exemplo de Execucao Paralela: User Story 1

```bash
# Em paralelo, apos concluir a Fase 2:
# T006 -> validar fail-fast sem chave JWT
# T008 -> auditar Program.cs sem segredo hardcoded
```

## Exemplo de Execucao Paralela: User Story 2

```bash
# Em paralelo, apos concluir a Fase 2:
# T010 -> validar placeholder vazio no appsettings
# T013 -> atualizar README com instrucoes de JWT
```

---

## Estrategia de Implementacao

### MVP Primeiro (Somente US1)

1. Concluir Fase 1.
2. Concluir Fase 2.
3. Concluir Fase 3 (US1).
4. Validar fail-fast sem chave e startup com chave valida.
5. Publicar MVP de seguranca de configuracao.

### Entrega Incremental

1. Setup + Fundacional.
2. US1 (fail-fast e remocao de segredo hardcoded).
3. US2 (placeholder versionado e README operacional).
4. Polish final com evidencias e resumo de conformidade.

### Estrategia para Time em Paralelo

1. Pessoa A: T003-T005 (fundacional).
2. Pessoa B: T006-T009 (US1) apos fundacional.
3. Pessoa C: T010-T013 (US2) apos fundacional.
4. Fechamento conjunto: T014-T016.

---

## Notas

- [P] indica tarefas em arquivos diferentes, sem dependencia direta de tarefa incompleta.
- [US1]/[US2] garante rastreabilidade por historia.
- Cada historia possui criterio de teste independente.
- Evitar introduzir qualquer segredo real em arquivos versionados.
