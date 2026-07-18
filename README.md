# Oficina MotoPro — Workspace

Sistema de gestão para oficinas de motocicletas. Monorepo composto por API .NET 8, SPA Angular 21 e documentação técnica.

---

## Repositórios

| Pasta | Repositório | Descrição |
|-------|------------|-----------|
| `oficina-motos-api/` | [cleversantoro/oficina-motos-api](https://github.com/cleversantoro/oficina-motos-api) | API REST .NET 8 — Clean Architecture |
| `oficina-motos-web/` | [cleversantoro/oficina-motos-web](https://github.com/cleversantoro/oficina-motos-web) | SPA Angular 21 — PrimeNG + TailwindCSS |
| `oficina-motos-docs/` | [cleversantoro/oficina-motos-docs](https://github.com/cleversantoro/oficina-motos-docs) | Scripts SQL, markdown e documentação técnica |

---

## Stack

| Camada | Tecnologia |
|--------|-----------|
| Backend | ASP.NET Core 8, Entity Framework Core + Pomelo (MySQL), JWT Bearer, FluentValidation, AutoMapper |
| Frontend | Angular 21, PrimeNG 21, TailwindCSS 3, Chart.js 4, ngx-mask |
| Banco | MySQL 8 |
| Observabilidade | Serilog, OpenTelemetry (Prometheus, OTLP/Jaeger) |
| Testes | xUnit + Moq (planejado), Karma/Jasmine (Angular) |

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [MySQL 8+](https://dev.mysql.com/downloads/)

---

## Início Rápido

### 1. Banco de dados

Crie o banco e execute os scripts SQL em ordem:

```bash
# Execute os scripts em oficina-motos-docs/oficina_db_sql/
mysql -u root -p < oficina-motos-docs/oficina_db_sql/oficina_db_database.sql
```

### 2. API (.NET 8)

```bash
cd oficina-motos-api

# Configure a string de conexão e a chave JWT
# (copie appsettings.json e ajuste ou use dotnet user-secrets)
dotnet user-secrets set "ConnectionStrings:OficinaDb" "Server=localhost;Database=oficina_db;User=root;Password=SUA_SENHA"
dotnet user-secrets set "Jwt:Key" "sua-chave-secreta-minimo-32-caracteres"

# Restore e execução
dotnet restore
dotnet run --project src/OficinaMotos.API/OficinaMotos.API.csproj
```

Swagger disponível em: `http://localhost:5099/swagger`

### 3. Frontend (Angular 21)

```bash
cd oficina-motos-web

npm install
ng serve
```

Aplicação disponível em: `http://localhost:4200`

> O proxy reverso em `proxy.conf.json` encaminha `/api/**` para `http://localhost:5099`.

---

## Estrutura do Workspace

```
OficinaMotos/
├── governance/              # Documentação de governança do projeto
│   ├── auditoria.md         # Auditoria técnica completa
│   ├── arquitetura.md       # Decisões de arquitetura, C4, bounded contexts
│   ├── backlog.md           # Product backlog com 36 user stories
│   ├── cobertura.md         # Matriz de cobertura por requisito
│   ├── decisoes.md          # ADRs — Architecture Decision Records
│   ├── inventario.md        # Inventário de módulos e artefatos
│   └── roadmap.md           # Roadmap até v1.0 com Gantt
│
├── oficina-motos-api/       # Backend .NET 8
│   └── src/
│       ├── OficinaMotos.API/           # Controllers, middlewares, Program.cs
│       ├── OficinaMotos.Application/   # Services, DTOs, mappings, validators
│       ├── OficinaMotos.Domain/        # Entidades, interfaces de repositório
│       └── OficinaMotos.Infrastructure/# Repositórios, EF Context, IoC
│
├── oficina-motos-web/       # Frontend Angular 21
│   └── src/app/
│       ├── core/            # Auth, interceptors, services globais, api-paths
│       ├── features/        # Módulos por bounded context
│       ├── layout/          # Header, sidebar, footer, main-layout
│       └── shared/          # Componentes UI reutilizáveis, validators, CEP
│
└── oficina-motos-docs/      # Documentação e scripts SQL
    ├── markdown/            # Documentação técnica e análises
    └── oficina_db_sql/      # Scripts DDL de criação das 63 tabelas
```

---

## Módulos do Sistema

| Módulo | Controllers API | Tabelas SQL | Status |
|--------|:-:|:-:|--------|
| Clientes | 11 | 11 | Backend 90% · Frontend 75% |
| Veículos | 3 | 3 | Backend 80% · Frontend 40% |
| Mecânicos | 9 | 9 | Backend 85% · Frontend 40% |
| Fornecedores | 10 | 10 | Backend 85% · Frontend 40% |
| Ordens de Serviço | 8 | 8 | Backend 75% · Frontend 20% |
| Estoque | 8 | 8 | Backend 85% · Frontend 25% |
| Financeiro | 7 | 7 | Backend 80% · Frontend 20% |
| Segurança / Auth | 6 | 7 | Backend 85% · Frontend 70% |
| Dashboard | — | — | Dados mockados no frontend |
| **Total** | **62** | **63** | **~53% geral** |

---

## Estado do Projeto (2026-07-18)

**Versão atual:** v0.5 — em desenvolvimento ativo

| Camada | Completude | Observação |
|--------|-----------|------------|
| Backend API | ~72% | CRUD completo; `[Authorize]` ausente em 57/62 controllers |
| Frontend Angular | ~48% | Infraestrutura shared completa; formulários de OS/Estoque/Financeiro ausentes |
| Banco de Dados | ~85% | Schema bem definido; campo `proximo_km_revisao` ausente em Veículo |
| Testes | ~2% | 0 projetos xUnit no backend; 2 specs triviais no frontend |

> **Atenção de segurança:** 57 controllers de negócio estão sem `[Authorize]` (OWASP A01).
> Não use em produção antes de aplicar a `FallbackPolicy` descrita no [backlog.md](governance/backlog.md) (US-001).

---

## Roadmap

| Versão | Data | Entregável |
|--------|------|-----------|
| v0.6 | 2026-08-03 | Segurança — `[Authorize]` global, JWT seguro, Soft Delete |
| v0.7 | 2026-08-19 | MVP OS — formulário, detalhe, itens, pagamento |
| v0.8 | 2026-09-22 | Cadastros completos + financeiro operacional |
| v0.9-rc | 2026-10-09 | Cobertura de testes >= 70% |
| **v1.0** | **2026-10-23** | Dashboard real, PDF de OS, deploy em produção |

Roadmap detalhado com Gantt: [governance/roadmap.md](governance/roadmap.md)

---

## Documentação de Governança

| Documento | Descrição |
|-----------|-----------|
| [auditoria.md](governance/auditoria.md) | Auditoria técnica — estado real do projeto, divergências e dívida técnica |
| [arquitetura.md](governance/arquitetura.md) | Arquitetura C4, bounded contexts, ADRs, débitos arquiteturais |
| [inventario.md](governance/inventario.md) | Inventário completo de artefatos por módulo |
| [cobertura.md](governance/cobertura.md) | Matriz de cobertura de ~105 requisitos por camada |
| [backlog.md](governance/backlog.md) | 36 user stories priorizadas com tasks e critérios de aceite |
| [roadmap.md](governance/roadmap.md) | Cronograma de sprints e milestones até v1.0 |
| [decisoes.md](governance/decisoes.md) | Architecture Decision Records (ADRs) |

---

## Licença

MIT — consulte o arquivo `LICENSE.txt` no repositório da API.
