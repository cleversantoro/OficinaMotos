# Matriz de Cobertura — Oficina MotoPro

**Gerado em:** 2026-07-18
**Versão:** 2.0 — baseada em varredura do código-fonte
**Módulos:** Auth · Dashboard · Clientes · Veículos · Mecânicos · Fornecedores · OS · Estoque · Financeiro · Segurança · Infraestrutura

---

## Legenda

| Símbolo | Significado |
|---------|-------------|
| ✅ | Implementado / Existente / Funcional |
| ⚠️ | Parcial — existe mas incompleto ou com limitações |
| ❌ | Ausente / Não implementado |
| N/A | Não aplicável a esta camada |

**Definição de Status:**
- **Completo** — todas as camadas aplicáveis implementadas e funcionais (end-to-end)
- **Parcial** — ao menos API e Banco existem; Frontend parcial ou ausente
- **Pendente** — funcionalidade crítica não iniciada

> **Nota crítica sobre testes:** Backend com **0 arquivos de teste**. Frontend: **2 specs triviais** (~1% cobertura).

---

## Resumo por Camada

| Camada | Implementado | Total | % |
|--------|-------------|-------|---|
| API .NET 9 — controllers existem | 62 | 62 | 100% ✅ |
| API — com `[Authorize]` | 6 | 62 | 9.7% 🔴 |
| Frontend Angular 21 — páginas | ~15 | ~35 esperadas | 43% 🟡 |
| Banco de Dados — tabelas SQL | 63 | 63 | 100% ✅ |
| Testes Backend (.NET) | 0 | — | 0% 🔴 |
| Testes Frontend (Angular) | 2 specs triviais | — | ~1% 🔴 |

---

## Módulo 1 — Autenticação (Auth)

**API:** `AuthController` (1 controller) | **Frontend:** `features/auth/login/` · `AuthGuard` · interceptors | **Banco:** `seg_usuarios`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Login com credenciais (JWT) | ✅ | ✅ | ✅ LoginPage | ✅ | ❌ | Completo | 🔴 Alta |
| Logout / limpeza de sessão | ✅ | ⚠️ client-side only | ✅ AuthService | N/A | ❌ | Parcial | 🟡 Média |
| Refresh Token | ✅ | ❌ | ❌ | N/A | ❌ | Pendente | 🟡 Média |
| Guard de rotas (AuthGuard) | ✅ | N/A | ✅ authGuard | N/A | ❌ | Completo | 🔴 Alta |
| Interceptor JWT (token injection) | ✅ | N/A | ✅ AuthInterceptor | N/A | ❌ | Completo | 🔴 Alta |
| Interceptor de erros HTTP (401/403/500) | ✅ | N/A | ⚠️ stub | N/A | ⚠️ trivial | Parcial | 🔴 Alta |

**Subtotal — Auth:** 3 Completo · 2 Parcial · 1 Pendente

---

## Módulo 2 — Dashboard

**API:** sem controller dedicado | **Frontend:** `features/dashboard/pages/dashboard-principal/`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| KPIs principais (OS, receita, estoque) | ✅ | ❌ sem endpoint agregador | ⚠️ dados mockados | N/A | ❌ | Parcial | 🟡 Média |
| Gráficos de OS e financeiro | ✅ | ❌ | ⚠️ dados mockados | N/A | ❌ | Parcial | 🟡 Média |
| Alertas de estoque mínimo | ✅ | ❌ | ❌ | N/A | ❌ | Pendente | 🟡 Média |
| Calendário / agenda de OS | ✅ | ❌ | ❌ | N/A | ❌ | Pendente | 🟢 Baixa |

**Subtotal — Dashboard:** 0 Completo · 2 Parcial · 2 Pendente

---

## Módulo 3 — Clientes

**API:** 11 controllers | **Frontend:** 4 páginas (lista, cadastro, detalhe, editar) | **Banco:** 11 tabelas `cad_clientes_*`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Listagem de clientes (com busca) | ✅ | ✅ | ✅ cliente-lista | ✅ | ❌ | Completo | 🔴 Alta |
| Cadastro básico de cliente | ✅ | ✅ | ✅ cliente-cadastro | ✅ | ❌ | Parcial | 🔴 Alta |
| Edição de cliente | ✅ | ✅ | ⚠️ incompleto | ✅ | ❌ | Parcial | 🔴 Alta |
| Detalhe do cliente | ✅ | ✅ | ✅ cliente-detalhe | ✅ | ❌ | Completo | 🔴 Alta |
| Dados PF (CPF, nascimento) | ✅ | ✅ ClientePfsController | ❌ | ✅ cad_clientes_pf | ❌ | Parcial | 🔴 Alta |
| Dados PJ (CNPJ, razão social) | ✅ | ✅ ClientePjsController | ❌ | ✅ cad_clientes_pj | ❌ | Parcial | 🟡 Média |
| Endereços múltiplos | ✅ | ✅ ClienteEnderecosController | ❌ | ✅ | ❌ | Parcial | 🔴 Alta |
| Contatos múltiplos | ✅ | ✅ ClienteContatosController | ❌ | ✅ | ❌ | Parcial | 🔴 Alta |
| Documentos do cliente | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Anexos / upload de arquivos | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Limite de crédito / financeiro | ✅ | ✅ ClienteFinanceirosController | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Indicações de clientes | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Origens (canal de captação) | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| LGPD — consentimentos | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🔴 Alta |
| Validação CPF/CNPJ/e-mail/telefone | ✅ | N/A | ❌ | N/A | ❌ | Pendente | 🔴 Alta |
| Busca de CEP automática (ViaCEP) | ✅ | N/A | ❌ | N/A | ❌ | Pendente | 🟡 Média |

**Subtotal — Clientes:** 2 Completo · 12 Parcial · 2 Pendente

---

## Módulo 4 — Veículos

**API:** 3 controllers | **Frontend:** 2 páginas (lista, detalhe) | **Banco:** 3 tabelas `cad_veiculos*`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Listagem de veículos | ✅ | ✅ | ✅ veiculo-lista | ✅ | ❌ | Completo | 🔴 Alta |
| Detalhe do veículo | ✅ | ✅ | ✅ veiculo-detalhe | ✅ | ❌ | Completo | 🟡 Média |
| Cadastro de veículo (formulário) | ✅ | ✅ | ❌ sem rota /motos/novo | ✅ | ❌ | Pendente | 🔴 Alta |
| Vincular veículo ao cliente | ✅ | ✅ | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Marcas de veículos (CRUD) | ✅ | ✅ VeiculoMarcasController | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Modelos de veículos (CRUD) | ✅ | ✅ VeiculoModelosController | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Histórico de manutenções | ✅ | ⚠️ via OS | ❌ | N/A | ❌ | Pendente | 🟡 Média |
| Campo proximo_km_revisao | ✅ | ❌ campo ausente | ❌ | ❌ ausente | ❌ | Pendente | 🟡 Média |

**Subtotal — Veículos:** 2 Completo · 2 Parcial · 4 Pendente

---

## Módulo 5 — Mecânicos

**API:** 9 controllers | **Frontend:** 2 páginas (lista existe; detalhe existe mas SEM ROTA) | **Banco:** 9 tabelas `cad_mecanicos*`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Listagem de mecânicos | ✅ | ✅ | ✅ mecanico-lista | ✅ | ❌ | Completo | 🟡 Média |
| Detalhe do mecânico | ✅ | ✅ | ⚠️ sem rota /mecanicos/:id | ✅ | ❌ | Parcial | 🟡 Média |
| Cadastro de mecânico | ✅ | ✅ | ❌ sem rota /mecanicos/novo | ✅ | ❌ | Pendente | 🟡 Média |
| Certificações | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Contatos do mecânico | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Disponibilidades / agenda | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Documentos do mecânico | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Endereços do mecânico | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Especialidades (CRUD + vínculo) | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Experiências profissionais | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |

**Subtotal — Mecânicos:** 1 Completo · 8 Parcial · 1 Pendente

---

## Módulo 6 — Fornecedores

**API:** 10 controllers | **Frontend:** 2 páginas (lista, detalhe) | **Banco:** 10 tabelas `cad_fornecedores*`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Listagem de fornecedores | ✅ | ✅ | ✅ | ✅ | ❌ | Completo | 🟡 Média |
| Detalhe do fornecedor | ✅ | ✅ | ✅ | ✅ | ❌ | Completo | 🟡 Média |
| Cadastro de fornecedor | ✅ | ✅ | ❌ sem rota /fornecedores/novo | ✅ | ❌ | Pendente | 🟡 Média |
| Avaliações de fornecedores | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Dados bancários | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Certificações | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Contatos do fornecedor | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Documentos do fornecedor | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Endereços do fornecedor | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Representantes comerciais | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Segmentos (CRUD + vínculo) | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |

**Subtotal — Fornecedores:** 2 Completo · 8 Parcial · 1 Pendente

---

## Módulo 7 — Ordens de Serviço (OS)

**API:** 8 controllers | **Frontend:** 1 página (os-detalhe funciona como lista) | **Banco:** 8 tabelas `os_*`
⚠️ Rotas `/ordens/novo`, `/ordens/:id` **não existem**

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Listagem de OS | ✅ | ✅ | ⚠️ os-detalhe como lista | ✅ | ❌ | Parcial | 🔴 Alta |
| Criar nova OS (formulário completo) | ✅ | ✅ | ❌ sem rota /ordens/novo | ✅ | ❌ | Pendente | 🔴 Alta |
| Detalhe completo da OS | ✅ | ✅ | ❌ sem rota /ordens/:id | ✅ | ❌ | Pendente | 🔴 Alta |
| Edição de OS | ✅ | ✅ | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Vincular cliente e mecânico | ✅ | ✅ | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Vincular veículo à OS (VeiculoId) | ✅ | ❌ campo ausente | ❌ | ❌ ausente | ❌ | Pendente | 🔴 Alta |
| Itens da OS (peças + serviços + totais) | ✅ | ✅ OrdemServicoItensController | ❌ | ✅ os_itens | ❌ | Pendente | 🔴 Alta |
| Pagamentos da OS | ✅ | ✅ OrdemServicoPagamentosController | ❌ | ✅ os_pagamentos | ❌ | Pendente | 🔴 Alta |
| Checklist de inspeção | ✅ | ✅ | ❌ | ✅ | ❌ | Pendente | 🟡 Média |
| Histórico de alterações de status | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Observações da OS | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Avaliação do serviço (rating) | ✅ | ✅ | ❌ | ✅ | ❌ | Pendente | 🟢 Baixa |
| Anexos / fotos da OS | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Impressão PDF da OS | ✅ | ❌ | ❌ | N/A | ❌ | Pendente | 🟡 Média |

**Subtotal — OS:** 0 Completo · 4 Parcial · 10 Pendente

---

## Módulo 8 — Estoque

**API:** 8 controllers | **Frontend:** 1 página (estoque-lista) | **Banco:** 8 tabelas `est_*`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Listagem de peças | ✅ | ✅ | ✅ estoque-lista | ✅ | ❌ | Completo | 🔴 Alta |
| Cadastro de peça (formulário) | ✅ | ✅ | ❌ sem rota /estoque/novo | ✅ | ❌ | Pendente | 🔴 Alta |
| Detalhe / edição de peça | ✅ | ✅ | ❌ sem rota /estoque/:id | ✅ | ❌ | Pendente | 🟡 Média |
| Categorias de estoque (CRUD) | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Fabricantes (CRUD) | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Localizações / prateleiras | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Movimentações (entrada / saída) | ✅ | ✅ EstoqueMovimentacoesController | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Vínculo peça com fornecedor | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Histórico de preços / alterações | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Anexos de peça | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Alertas de estoque mínimo | ✅ | ❌ sem endpoint dedicado | ❌ | N/A | ❌ | Pendente | 🟡 Média |

**Subtotal — Estoque:** 1 Completo · 6 Parcial · 4 Pendente

---

## Módulo 9 — Financeiro

**API:** 7 controllers | **Frontend:** 1 página (financeiro-dashboard — dados mockados) | **Banco:** 7 tabelas `fin_*`

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Dashboard financeiro (KPIs reais) | ✅ | ❌ sem endpoint agregador | ⚠️ dados mockados | N/A | ❌ | Parcial | 🔴 Alta |
| Contas a Pagar — listagem | ✅ | ✅ FinanceiroContasPagarController | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Contas a Pagar — formulário | ✅ | ✅ | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Contas a Receber — listagem | ✅ | ✅ FinanceiroContasReceberController | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Contas a Receber — formulário | ✅ | ✅ | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Lançamentos financeiros | ✅ | ✅ FinanceiroLancamentosController | ❌ | ✅ | ❌ | Pendente | 🟡 Média |
| Registrar pagamentos | ✅ | ✅ FinanceiroPagamentosController | ❌ | ✅ | ❌ | Pendente | 🔴 Alta |
| Métodos de pagamento (CRUD) | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Histórico financeiro | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |
| Anexos financeiros | ✅ | ✅ | ❌ | ✅ | ❌ | Parcial | 🟢 Baixa |

**Subtotal — Financeiro:** 0 Completo · 4 Parcial · 6 Pendente

---

## Módulo 10 — Segurança (RBAC)

**API:** 5 controllers com `[Authorize]` | **Frontend:** sem página de administração | **Banco:** 7 tabelas `seg_*`
⚠️ **57 dos 62 controllers de negócio sem `[Authorize]`** — risco OWASP A01

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Gestão de Usuários (CRUD) | ✅ | ✅ UsuariosController [Authorize] | ❌ sem página admin | ✅ | ❌ | Parcial | 🔴 Alta |
| Gestão de Perfis / Roles | ✅ | ✅ PerfisController [Authorize] | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Gestão de Permissões | ✅ | ✅ PermissoesController [Authorize] | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Módulos do sistema | ✅ | ✅ ModulosController [Authorize] | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| Audit Log (rastreabilidade) | ✅ | ✅ AuditLogController [Authorize] | ❌ | ✅ | ❌ | Parcial | 🟡 Média |
| `[Authorize]` em TODOS os controllers | ✅ | ❌ 57/62 sem proteção | N/A | N/A | ❌ | Pendente | 🔴 Alta |

**Subtotal — Segurança:** 0 Completo · 5 Parcial · 1 Pendente

---

## Módulo 11 — Qualidade e Infraestrutura

| Requisito | Documentado | API | Frontend | Banco | Testes | Status | Prioridade |
|---|---|---|---|---|---|---|---|
| Testes unitários Backend (.NET) | ✅ | ❌ 0 arquivos | N/A | N/A | ❌ | Pendente | 🔴 Alta |
| Testes unitários Frontend (Angular) | ✅ | N/A | ❌ 2 triviais | N/A | ⚠️ ~1% | Pendente | 🔴 Alta |
| Testes de integração / E2E | ✅ | ❌ | ❌ | N/A | ❌ | Pendente | 🟡 Média |
| Toast / Notificações globais | ✅ | N/A | ❌ | N/A | ❌ | Pendente | 🔴 Alta |
| Loading Spinner global | ✅ | N/A | ❌ | N/A | ❌ | Pendente | 🟡 Média |
| Tratamento de erros HTTP global | ✅ | N/A | ⚠️ error-interceptor stub | N/A | ⚠️ trivial | Parcial | 🔴 Alta |
| Input masks (CPF/CNPJ/tel/CEP) | ✅ | N/A | ❌ | N/A | ❌ | Pendente | 🔴 Alta |
| Responsividade Mobile | ✅ | N/A | ❌ | N/A | ❌ | Pendente | 🟡 Média |
| Relatórios PDF / exportação Excel | ✅ | ❌ | ❌ | N/A | ❌ | Pendente | 🟢 Baixa |
| Refresh Token | ✅ | ❌ | ❌ | N/A | ❌ | Pendente | 🟡 Média |

**Subtotal — Infraestrutura:** 0 Completo · 1 Parcial · 9 Pendente

---

## Gaps Críticos

### Segurança — Riscos OWASP

| # | Gap | Risco OWASP | Impacto |
|---|-----|-------------|---------|
| G1 | **57/62 controllers sem `[Authorize]`** | A01 Broken Access Control | Crítico |
| G2 | Sem verificação de Role nos controllers de negócio | A01 Broken Access Control | Alto |
| G3 | Sem validação de CPF/CNPJ no backend | A03 Injection / Data Integrity | Médio |
| G4 | Interceptor 401/403 não funcional | A07 Auth Failures | Alto |

### Funcionalidade — Gaps de MVP

| # | Gap | Módulo | Impacto no Negócio |
|---|-----|--------|--------------------|
| G5 | OS sem formulário de criação/edição | OS | Crítico |
| G6 | Dashboard com dados mockados | Dashboard | Alto |
| G7 | Formulários ausentes: Veículo, Mecânico, Fornecedor, Peça | 4 módulos | Alto |
| G8 | Módulo Financeiro: lista e forms totalmente ausentes no frontend | Financeiro | Alto |
| G9 | `/mecanicos/:id` não roteado | Mecânicos | Médio |
| G10 | Toast/notificações ausentes — sem feedback visual | Infraestrutura | Alto |
| G11 | `VeiculoId` ausente em `OrdemServico` | OS, Veículos | Alto |

### Qualidade — Cobertura Zero

| # | Gap | Impacto |
|---|-----|---------|
| G12 | 0 testes no backend | Crítico |
| G13 | ~1% cobertura frontend | Alto |

---

## Próximas Prioridades (Roadmap de Cobertura)

### Sprint 1 — Segurança Crítica (desbloqueante)
1. Adicionar `[Authorize]` em todos os 57 controllers de negócio sem proteção
2. Implementar verificação de role nos endpoints sensíveis
3. Implementar `errorInterceptor` funcional: 401 → redirect login, 403 → mensagem, 500 → toast
4. Adicionar `VeiculoId` à entidade `OrdemServico` e migration

### Sprint 2 — MVP Ordens de Serviço
5. Criar página e rota `/ordens/novo` com formulário completo
6. Adicionar rota `/ordens/:id` com detalhe real de OS
7. Gestão de itens da OS (peças + serviços + totais)
8. Registro de pagamento da OS

### Sprint 3 — CRUDs Pendentes
9. Formulário de cadastro de veículo (`/motos/novo`) + vínculo com cliente
10. Formulário de cadastro de mecânico (`/mecanicos/novo`) + registrar rota `/mecanicos/:id`
11. Formulário de cadastro/edição de fornecedor (`/fornecedores/novo`)
12. Formulário de cadastro de peça em estoque + movimentações

### Sprint 4 — Financeiro e Clientes
13. Lista e formulário de Contas a Pagar e Contas a Receber
14. Registro de pagamentos financeiros
15. Completar edição de cliente (endereços, contatos, dados PF/PJ)
16. Validações de formulário: CPF/CNPJ, input masks, busca de CEP

### Sprint 5 — Qualidade
17. Configurar projeto `xUnit` + `Moq` no backend; meta: 60% nos services
18. Adicionar testes Angular para Services e Guards; meta: 50% cobertura
19. Configurar testes E2E (Playwright) para fluxos críticos

### Sprint 6 — Dashboard e Relatórios
20. Endpoint agregador de KPIs para o dashboard
21. Gráficos do dashboard com dados reais
22. Alertas de estoque mínimo
23. Relatório PDF de OS

---

## Painel de Resumo Consolidado

```
MATRIZ DE COBERTURA — OFICINA MOTOPRO
Data da análise: 2026-07-18
────────────────────────────────────────────────────────
Total de requisitos mapeados:            ~105
────────────────────────────────────────────────────────
Totalmente cobertos    (Completo):          11   (~10%)
Parcialmente cobertos  (Parcial) :          55   (~52%)
Sem cobertura          (Pendente):          39   (~37%)
────────────────────────────────────────────────────────
COBERTURA POR CAMADA
─────────────────────────────────────────────────────────
API (.NET 9) — controllers existem:  62/62   100%  ✅
API — controllers com [Authorize]:    6/62    9.7%  🔴
Frontend — páginas implementadas:   ~15/35   43%   🟡
Banco de Dados — tabelas definidas:  63/63   100%  ✅
Testes Backend (.NET):                0/62    0%    🔴
Testes Frontend (Angular):            2/~30   ~1%   🔴
────────────────────────────────────────────────────────
MÓDULOS COM MAIOR GAP (frontend pendente):
Ordens de Serviço  —  0 Completo, 10 Pendente  🔴
Infraestrutura     —  0 Completo,  9 Pendente  🔴
Financeiro         —  0 Completo,  6 Pendente  🔴
Veículos           —  2 Completo,  4 Pendente  🔴
Estoque            —  1 Completo,  4 Pendente  🔴

MÓDULOS PRONTOS PARA ESPECIFICAÇÃO (API + DB >= 80%):
✅ Clientes      — 11 controllers, 11 tabelas
✅ Mecânicos     —  9 controllers,  9 tabelas
✅ Fornecedores  — 10 controllers, 10 tabelas
✅ Estoque       —  8 controllers,  8 tabelas
✅ OS            —  8 controllers,  8 tabelas
✅ Segurança     —  5 controllers [Authorize], 7 tabelas
────────────────────────────────────────────────────────
```
