<!--
SYNC IMPACT REPORT
==================
Versão: 0.0.0 → 1.0.0 (MAJOR — constituição inicial, criada do template)
Princípios adicionados:
  I.   Domínio Primeiro (DDD)
  II.  API RESTful Versionada
  III. Segurança por Design (RBAC + LGPD)
  IV.  Frontend Reativo com Componentes Standalone
  V.   Integridade e Rastreabilidade de Dados
  VI.  Qualidade e Testabilidade
  VII. Documentação como Fonte de Verdade
Seções adicionadas:
  - Stack Tecnológica
  - Padrões de Segurança e Compliance
  - Workflow de Desenvolvimento
Templates verificados:
  - spec-template.md    ✅ alinhado (user stories, FR, entidades)
  - plan-template.md    ✅ alinhado (Constitution Check presente)
  - tasks-template.md   ✅ alinhado (fases e paralelismo)
TODOs diferidos:
  - Nenhum campo obrigatório ficou pendente.
-->

# Oficina MotoPro — Constituição do Projeto

## Princípios Fundamentais

### I. Domínio Primeiro (DDD)

O sistema é estruturado em quatro **Bounded Contexts** autônomos, que DEVEM ser respeitados em todas as
implementações:

| Contexto | Papel | Responsabilidade |
|---|---|---|
| **Cadastro** | Supplier | Clientes, Veículos, Mecânicos, Fornecedores |
| **Ordem de Serviço** | Customer | OS, Orçamentos, Checklists, Histórico |
| **Estoque** | Supplier | Peças, Movimentações, Reservas |
| **Financeiro** | Consumer (eventos) | Faturas, Recebíveis, NF-e |

**Regras inegociáveis:**
- Cada BC DEVE possuir suas próprias entidades de domínio — nunca compartilhar classes entre contextos.
- Comunicação síncrona (REST) é permitida apenas para **leituras de referência** (ex.: consultar cliente na OS).
- Fluxos de negócio entre OS → Financeiro/Estoque DEVEM usar **eventos de domínio** (Published Language)
  para reduzir acoplamento.
- A camada de Anti-Corruption (ACL) DEVE ser introduzida sempre que modelos de BCs distintos precisarem
  se integrar, protegendo o modelo interno de contaminação.
- Nenhuma lógica de negócio pode residir em Controllers ou componentes de UI — ela pertence ao Domain
  ou Application layer da API, e aos Services do frontend.

**Rationale:** A separação por BC garante que mudanças em um contexto não quebrem outros, permitindo
evolução independente, deploys autônomos e times paralelos.

---

### II. API RESTful Versionada

Todos os endpoints da API DEVEM seguir o padrão `/api/v1/{Recurso}` e ser registrados centralmente
no arquivo `api-paths.ts` do frontend.

**Regras inegociáveis:**
- Novos endpoints DEVEM ser adicionados à versão corrente (`v1`) enquanto não houver quebra de contrato.
- Alterações que quebrem o contrato (remoção de campo, mudança de tipo) DEVEM criar uma nova versão
  (`v2`) — nunca alterar silenciosamente a versão em uso.
- Sub-recursos seguem o padrão `/{RecursoPai}/{id}/{SubRecurso}` ou como endpoint independente quando
  o acesso direto for necessário (ex.: `/api/v1/ClienteEnderecos`).
- Respostas de erro DEVEM usar os status HTTP semânticos: 400 (validação), 401 (não autenticado),
  403 (sem permissão), 404 (não encontrado), 500 (erro interno).
- O frontend NUNCA deve hardcodar URLs — toda chamada HTTP DEVE referenciar `apiPaths` de `api-paths.ts`.

**Rationale:** Centralizar endpoints evita divergências entre frontend e backend, facilita auditorias
e garante que mudanças de URL sejam propagadas de um único lugar.

---

### III. Segurança por Design (RBAC + LGPD) — NÃO NEGOCIÁVEL

Segurança não é opcional. Todo acesso ao sistema DEVE ser validado pelo modelo RBAC definido no módulo
`seg_` do banco de dados.

**Regras inegociáveis:**
- Autenticação via **JWT** é obrigatória em todos os endpoints, exceto `/api/v1/Auth/login`.
- Autorização DEVE verificar perfil e permissão específica (`modulo × ação`) antes de executar qualquer
  operação sensível. Os 6 perfis (Administrador → Consulta) e as 46 permissões são a referência canônica.
- Senhas DEVEM usar hash **bcrypt com custo mínimo 12** — texto puro é proibido em qualquer camada.
- A tabela `seg_audit_log` é **INSERT-ONLY**: nenhum registro pode ser alterado ou excluído. Ações
  auditadas: LOGIN, LOGOUT, LOGIN_FAIL, CREATE, UPDATE, DELETE, VIEW (dados sensíveis), EXPORT, APPROVE.
- Dados pessoais de clientes são protegidos pela LGPD — consentimentos DEVEM ser registrados em
  `cad_clientes_lgpd_consentimentos` antes de qualquer processamento de dados sensíveis.
- O frontend DEVE implementar guards de rota baseados no perfil do usuário autenticado.
- Tentativas de login com falha DEVEM incrementar `Tentativas_Login`; após limite, bloquear conta
  preenchendo `Bloqueado_Ate`.

**Rationale:** Falhas de segurança em sistemas com dados financeiros e pessoais têm impacto legal e
reputacional severo. RBAC granular garante que cada perfil veja e faça apenas o que lhe compete.

---

### IV. Frontend Reativo com Componentes Standalone

O frontend DEVE usar o paradigma moderno do Angular 21: Standalone Components e Signals para
reatividade, eliminando NgModules desnecessários.

**Regras inegociáveis:**
- Todo componente DEVE ser **standalone** (`standalone: true`) — NgModules são proibidos em código novo.
- Estado reativo DEVE usar **Signals** (`signal`, `computed`, `effect`) — `BehaviorSubject` e `Subject`
  são permitidos apenas em serviços de infraestrutura legados já existentes.
- A infraestrutura shared já construída DEVE ser reutilizada — nunca duplicar:
  - Toast → `shared/services/toast.ts` + `shared/ui/toast/`
  - Loading → `shared/services/loading.ts` + `shared/ui/loading-spinner/`
  - Confirmação → `shared/services/confirmation.ts`
  - CEP → `shared/services/cep.ts` (ViaCEP)
  - Upload → `shared/ui/file-upload/`
  - Tabela → `shared/ui/data-table/`
  - Validators → `shared/validators/` (CPF/CNPJ, e-mail, telefone, CEP)
- Máscaras de input DEVEM usar **ngx-mask** (já instalado) — nunca implementar lógica de máscara manual.
- Rotas DEVEM usar **lazy loading** para módulos de features — apenas o `MainLayout` e `LoginPage`
  são carregados eagerly.
- `OnPush` change detection DEVE ser aplicado em componentes de lista e tabelas de dados.

**Rationale:** Standalone + Signals é a direção oficial do Angular; garante bundles menores, melhor
performance e código mais previsível sem ciclos de detecção desnecessários.

---

### V. Integridade e Rastreabilidade de Dados

O banco de dados é a fonte de verdade do sistema e DEVE ter integridade garantida em nível de banco,
não apenas na aplicação.

**Regras inegociáveis:**
- Engine DEVE ser **InnoDB** para todas as tabelas — MyISAM é proibido.
- Charset DEVE ser **utf8mb4 / utf8mb4_unicode_ci** em todas as tabelas e colunas de texto.
- Prefixos de tabela por domínio DEVEM ser respeitados:
  `cad_` (Cadastro), `os_` (Ordens), `est_` (Estoque), `fin_` (Financeiro), `seg_` (Segurança).
- **Foreign Keys** com comportamento explícito (`CASCADE` ou `SET NULL`) são obrigatórias em todos os
  relacionamentos — sem FKs implícitas ou apenas no código.
- PKs de tabelas de dados DEVEM ser `bigint` auto-incremento; tabelas de domínio (perfis, módulos)
  podem usar `int`.
- Campos de auditoria (`Criado_Em`, `Atualizado_Em`, `Criado_Por`) DEVEM estar presentes em todas as
  tabelas principais.
- Exclusão lógica (`Ativo`, `Status`) é preferível à exclusão física para entidades com histórico.

**Rationale:** Integridade referencial no banco garante consistência mesmo quando a aplicação falha;
prefixos facilitam manutenção e identificação visual imediata do domínio de cada tabela.

---

### VI. Qualidade e Testabilidade

Código sem teste é código com comportamento desconhecido. A cobertura mínima aceita é 70% para
serviços e guards.

**Regras inegociáveis:**
- **TypeScript strict mode** (`"strict": true`) DEVE estar ativo — nunca desabilitar verificações
  de tipo para resolver erros de compilação.
- Validações de negócio (CPF, CNPJ, e-mail, CEP, placa) DEVEM usar os validators de `shared/validators/`
  — nunca validar apenas no backend ou apenas no frontend.
- Todo Service do frontend DEVE ter arquivo `.spec.ts` correspondente.
- Guards e interceptors DEVEM ter testes unitários cobrindo os cenários de autorização e erro.
- O Error Interceptor (`core/interceptors/error-interceptor.ts`) DEVE tratar: 401 → redirecionar para
  login, 403 → toast de permissão negada, 500 → toast de erro interno.
- Cobertura de testes E2E DEVE cobrir os fluxos críticos: login, cadastro de cliente, criação de OS.

**Rationale:** Validação dupla (frontend + backend) previne dados inválidos mesmo em caso de bypass
da UI; testes garantem que refatorações e novas features não quebrem comportamentos existentes.

---

### VII. Documentação como Fonte de Verdade

A pasta `oficina-motos-docs/` é a referência canônica do sistema. Toda nova especificação DEVE ser
consistente com ela antes de ser implementada.

**Regras inegociáveis:**
- Os bounded contexts definidos em `oficina_de_motos_bounded_contexts_c_4_context_map.md` DEVEM ser
  respeitados em toda arquitetura nova.
- O schema de banco de dados em `oficina_db_sql/` é a referência para entidades — nenhuma tabela nova
  pode ser criada sem ser adicionada à documentação SQL correspondente.
- As telas HTML em `pages/` (CadastroCliente, dashboard, Estoque, financeiro, mecanicos, OS, veiculos)
  são os protótipos de referência para implementação do frontend.
- O mapa de endpoints do `dashboard.txt` define a origem de cada métrica do dashboard — qualquer
  desvio DEVE ser documentado.
- Novas features DEVEM passar por spec (`speckit.specify`) antes de planejamento e implementação.

**Rationale:** Com três repositórios distintos (docs, api, web), a documentação centralizada evita
divergências silenciosas que resultam em retrabalho e bugs de integração.

---

## Stack Tecnológica

### Backend (oficina-motos-api)

| Camada | Tecnologia |
|---|---|
| Framework | .NET (ASP.NET Core Web API) |
| Arquitetura | Clean Architecture (API / Application / Domain / Infrastructure) |
| Autenticação | JWT Bearer Token |
| Banco de Dados | MySQL 8+ (InnoDB, utf8mb4) |
| ORM | Entity Framework Core |
| Versionamento de API | URL path versioning (`/api/v1/`) |

### Frontend (oficina-motos-web)

| Camada | Tecnologia |
|---|---|
| Framework | Angular 21 (Standalone Components) |
| UI Library | PrimeNG 21 |
| CSS | Tailwind CSS 3 |
| Reatividade | Angular Signals |
| Máscaras | ngx-mask 21 |
| Gráficos | Chart.js 4 |
| Testes | Vitest |
| Gerenciador de Pacotes | npm 10 |

### Convenções de Organização do Frontend

```
src/app/
├── core/          # auth, interceptors, guards, models globais, services de API
├── features/      # um diretório por módulo de negócio (clientes, ordens-servico, etc.)
│   └── [modulo]/
│       ├── components/   # componentes internos do módulo
│       ├── models/       # modelos específicos do módulo
│       ├── pages/        # componentes de página (rotas)
│       └── services/     # serviços específicos do módulo (se necessário)
├── layout/        # MainLayout, header, sidebar, footer
└── shared/        # reutilizáveis entre módulos
    ├── constants/
    ├── models/
    ├── services/  # toast, loading, cep, confirmation
    ├── ui/        # componentes visuais reutilizáveis
    ├── utils/
    └── validators/
```

---

## Padrões de Segurança e Compliance

### Perfis de Acesso (Imutáveis — alterar requer atualização desta constituição)

| Nível | Perfil | Escopo |
|---|---|---|
| 1 | Administrador | Acesso total (46 permissões) |
| 2 | Gerente | Total exceto gestão de usuários e configurações |
| 3 | Recepcionista | Clientes, OS (criar/editar), Fornecedores (visualizar) |
| 4 | Financeiro | Dashboard, Clientes (visualizar), OS (visualizar), Financeiro (completo) |
| 5 | Mecânico | Dashboard, Clientes/Veículos (visualizar), OS (visualizar/editar/aprovar), Estoque (visualizar/editar) |
| 6 | Consulta | Somente `visualizar` em todos os módulos |

### Ações de Permissão Disponíveis
`visualizar` · `criar` · `editar` · `excluir` · `exportar` · `aprovar`

### Módulos do Sistema (11)
`/dashboard` · `/clientes` · `/mecanicos` · `/veiculos` · `/ordens` · `/estoque` ·
`/financeiro` · `/relatorios` · `/configuracoes` · `/seguranca` · `/fornecedores`

### Checklist de Segurança para Novas Features
- [ ] Endpoint protegido por `[Authorize]` no backend?
- [ ] Guard de rota no frontend verificando perfil?
- [ ] Ações sensíveis registradas no `seg_audit_log`?
- [ ] Dados pessoais processados com consentimento LGPD?
- [ ] Inputs sanitizados e validados (frontend + backend)?
- [ ] Erros HTTP tratados pelo Error Interceptor?

---

## Workflow de Desenvolvimento

### Ciclo de Vida de uma Feature

```
1. Spec       → /speckit.specify  (define user stories, FR, entidades)
2. Clarify    → /speckit.clarify  (resolve ambiguidades antes de planejar)
3. Plan       → /speckit.plan     (design técnico, constitution check, estrutura)
4. Tasks      → /speckit.tasks    (lista ordenada de tarefas com dependências)
5. Implement  → /speckit.implement (execução, uma tarefa por vez)
6. Converge   → /speckit.converge  (valida o que foi implementado vs spec)
```

### Prioridades de Implementação (conforme documentação)

| Prioridade | Item |
|---|---|
| 🔴 Alta | OS: formulário completo (criar, editar, detalhe real) |
| 🔴 Alta | Veículos: formulário cadastro/editar |
| 🟡 Média | Estoque: cadastro, detalhe, movimentações |
| 🟡 Média | Fornecedores/Mecânicos: formulários |
| 🟡 Média | Financeiro: CRUD contas a pagar/receber |
| 🟡 Média | Lazy loading de rotas |
| 🟢 Baixa | Segurança: UI admin (usuários, perfis) |
| 🟢 Baixa | Relatórios (PDF/Excel) |
| 🟢 Baixa | Responsividade mobile / PWA |
| 🟢 Baixa | Refresh Token |

### Gates de Qualidade (obrigatórios antes de merge)

1. **Constitution Check** — a feature viola algum princípio desta constituição?
2. **Compilação** — `ng build` e `dotnet build` sem erros.
3. **Testes** — serviços e guards com cobertura ≥ 70%.
4. **Segurança** — checklist de segurança desta constituição preenchido.
5. **Consistência de Documentação** — entidades novas adicionadas ao schema SQL em `oficina-motos-docs`.

---

## Governança

Esta constituição **supera qualquer outra prática ou convenção** adotada anteriormente no projeto.
Em caso de conflito entre esta constituição e um padrão local de um arquivo, a constituição prevalece.

**Processo de Emenda:**
1. Propor alteração documentando: motivo, impacto nos princípios existentes e plano de migração.
2. Atualizar esta constituição incrementando a versão conforme as regras semânticas:
   - **MAJOR** — remoção ou redefinição incompatível de princípio.
   - **MINOR** — novo princípio ou seção adicionada / expansão material.
   - **PATCH** — clarificações, correções de texto, sem mudança de semântica.
3. Propagar alterações para os templates em `.specify/templates/` que referenciem os princípios alterados.
4. Registrar a alteração no bloco `SYNC IMPACT REPORT` no topo deste arquivo.

**Revisão de Compliance:**
- Toda PR de feature DEVE incluir o Constitution Check do `plan-template.md` preenchido.
- Violações identificadas em revisão DEVEM ser justificadas com entrada na tabela de Complexity Tracking.
- A constituição DEVE ser revisada a cada 3 meses ou a cada nova fase do projeto.

**Referência de Desenvolvimento em Tempo de Execução:** `oficina-motos-docs/markdown/`

**Version**: 1.0.0 | **Ratified**: 2026-07-18 | **Last Amended**: 2026-07-18
