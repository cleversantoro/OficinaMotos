# Decisões Arquiteturais — Oficina MotoPro

**Última atualização:** 2026-07-18

Este arquivo registra as decisões arquiteturais e técnicas do projeto no formato ADR (Architecture Decision Record).
Para decisões detalhadas com diagramas e contexto de bounded contexts, consulte [arquitetura.md](arquitetura.md).

---

## ADR-001 — Clean Architecture com 4 projetos separados

**Data:** Dezembro/2025 | **Status:** Implementado

**Decisão:** Separar a solução em 4 projetos respeitando `API → Application → Domain ← Infrastructure`.

**Benefícios:** Isolamento de frameworks; testabilidade sem dependência de ASP.NET ou EF Core.

**Custo:** Boilerplate — cada entidade exige ~3-4 arquivos (interface repo + implementação + registro IoC + service).

---

## ADR-002 — Único DbContext para todos os Bounded Contexts

**Data:** Dezembro/2025 | **Status:** Implementado (débito arquitetural)

**Decisão:** Um único `OficinaContext` com 63 `DbSet<T>`.

**Benefícios:** Simplicidade — um schema, migrações centralizadas, joins SQL cross-BC.

**Custo:** Viola o isolamento de BCs. Impede deploy independente por contexto. `FinanceiroPagamento` referencia `OrdemServico` e `Cliente` diretamente (violação CONSTITUTION.md §I).

**Resolução planejada:** Introduzir Anti-Corruption Layer entre BC Financeiro e BC OS/Cadastro — substituir navigation properties por IDs externos com eventos de domínio.

---

## ADR-003 — JWT gerado no Controller

**Data:** Dezembro/2025 | **Status:** Implementado

**Decisão:** `AuthService` (Application) valida credenciais e retorna `LoginDataResult`. Serialização do JWT ocorre no `AuthController`.

**Custo:** Controller contém lógica não trivial. Candidato a extração em `ITokenService`.

---

## ADR-004 — Repositório Genérico `IRepository<T>` + Repositórios Especializados

**Data:** Dezembro/2025 | **Status:** Implementado

**Decisão:** `IRepository<T>` fornece CRUD básico; repositórios especializados herdam e adicionam queries específicas.

**Custo:** `GetAllAsync()` sem filtros pode retornar tabelas inteiras em produção (risco de performance).

---

## ADR-005 — Angular Signals para estado de autenticação

**Data:** 2025 | **Status:** Implementado

**Decisão:** `AuthService` usa `signal<CurrentUser | null>` + `computed(() => isAuthenticated)`.

**Benefícios:** Reatividade sem subscriptions. Compatível com OnPush. Performance otimizada.

---

## ADR-006 — `api-paths.ts` como registro centralizado de endpoints

**Data:** 2025 | **Status:** Implementado e respeitado

**Decisão:** Todos os endpoints declarados no objeto `apiPaths` em `core/services/api-paths.ts`. Services nunca hardcodam URLs.

**Benefícios:** Mudança de URL propagada em um único arquivo.

---

## ADR-007 — Observabilidade configurada desde o desenvolvimento

**Data:** 2025 | **Status:** Implementado

**Decisão:** Serilog + OpenTelemetry configurados desde o início do projeto.

**Stack:** Serilog (Console · Seq · Elasticsearch) + OTel (Prometheus · OTLP/Jaeger)

---

## Débitos de Decisão (a resolver)

| ID | Decisão Pendente | Sprint | Prioridade |
|----|-----------------|--------|-----------|
| DA-001 | Aplicar `[Authorize]` globalmente — 57 controllers expostos | Sprint 1 | 🔴 Crítico |
| DA-002 | Refatorar `FinanceiroPagamento` para ACL (Anti-Corruption Layer) | Pós v1.0 | 🟡 Médio |
| DA-003 | Mover chave JWT para variável de ambiente / secrets | Sprint 1 | 🔴 Crítico |
| DA-004 | Reativar Soft Delete em `BaseEntity` | Sprint 1 | 🔴 Crítico |
| DA-005 | Migrar token JWT de `localStorage` para `HttpOnly` cookie | Pós v1.0 | 🟡 Médio |
| DA-006 | Implementar lazy loading nas rotas Angular | Pós v1.0 | 🟢 Baixo |
| DA-007 | CORS: ler origens de `appsettings.json` em vez de hardcoded | Sprint 6 | 🟢 Baixo |
