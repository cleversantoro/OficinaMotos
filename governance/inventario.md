# Inventário de Módulos — Oficina MotoPro

**Gerado em:** 2026-07-18  
**Fonte:** Análise direta do código-fonte (controllers, services, repositories, entities, frontend features, SQL scripts)

---

## Índice

1. [Módulo Clientes](#módulo-clientes)
2. [Módulo Veículos](#módulo-veículos)
3. [Módulo Mecânicos](#módulo-mecânicos)
4. [Módulo Fornecedores](#módulo-fornecedores)
5. [Módulo Ordens de Serviço](#módulo-ordens-de-serviço)
6. [Módulo Estoque](#módulo-estoque)
7. [Módulo Financeiro](#módulo-financeiro)
8. [Módulo Segurança / Auth](#módulo-segurança--auth)
9. [Módulo Dashboard](#módulo-dashboard)
10. [Métricas Gerais](#métricas-gerais)
11. [Resumo de Status](#resumo-de-status)

---

## Módulo Clientes

| Campo | Detalhe |
|---|---|
| **Nome** | Clientes |
| **Descrição** | Cadastro e gestão completa de clientes PF e PJ, incluindo subentidades (endereços, contatos, documentos, anexos, dados financeiros, consentimentos LGPD, indicações, origem de cadastro) |
| **Status Geral** | **Parcial** — Backend 90%, Frontend 75% |
| **Cobertura de Testes** | 0% — nenhum spec ou teste unitário |
| **Cobertura da Documentação** | Alta — RESUMO_EXECUTIVO, ANALISE_PROJETO e PASSOS_IMPLEMENTACAO cobrem o módulo |
| **Prioridade** | Alta — módulo mais avançado; referência para os demais |
| **Dependências** | Veículos (1:N), Ordens de Serviço (1:N) |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `Cliente`, `ClienteAnexo`, `ClienteContato`, `ClienteDocumento`, `ClienteEndereco`, `ClienteFinanceiro`, `ClienteIndicacao`, `ClienteLgpdConsentimento`, `ClienteOrigem`, `ClientePf`, `ClientePj` |
| **Controllers** | `ClientesController`, `ClienteAnexosController`, `ClienteContatosController`, `ClienteDocumentosController`, `ClienteEnderecosController`, `ClienteFinanceirosController`, `ClienteIndicacoesController`, `ClienteLgpdConsentimentosController`, `ClienteOrigensController`, `ClientePfsController`, `ClientePjsController` (11 controllers) |
| **Services** | `ClienteService` + 10 subentidades (11 services) |
| **Repositórios** | `ClienteRepository` + 10 subentidades (11 repositórios) |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `ClienteLista`, `ClienteCadastro`, `ClienteDetalhe`, `ClienteEditar` (4 páginas) |
| **Services** | `ClientesService` (em `features/clientes/services/`) |

### Banco de Dados

`cad_clientes`, `cad_clientes_anexos`, `cad_clientes_contatos`, `cad_clientes_documentos`, `cad_clientes_enderecos`, `cad_clientes_financeiro`, `cad_clientes_indicacoes`, `cad_clientes_lgpd_consentimentos`, `cad_clientes_origens`, `cad_clientes_pf`, `cad_clientes_pj` **(11 tabelas)**

### Gaps

- `[Authorize]` ausente nos 11 controllers (OWASP A01)
- Frontend não exibe subentidades de Indicações e LGPD
- `IsDeleted` (soft delete) comentado — deletes são físicos

---

## Módulo Veículos

| Campo | Detalhe |
|---|---|
| **Nome** | Veículos |
| **Descrição** | Cadastro de motocicletas vinculadas a clientes, com marcas, modelos, dados técnicos (placa, chassi, RENAVAM, KM) |
| **Status Geral** | **Parcial** — Backend 80%, Frontend 40% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Média |
| **Prioridade** | Alta — bloqueador para Ordens de Serviço |
| **Dependências** | Clientes (N:1), Ordens de Serviço (bloqueado — VeiculoId ausente) |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `Veiculo`, `VeiculoMarca`, `VeiculoModelo` |
| **Controllers** | `VeiculosController`, `VeiculoMarcasController`, `VeiculoModelosController` (3 controllers) |
| **Services** | `VeiculoService`, `VeiculoMarcaService`, `VeiculoModeloService` (3 services) |
| **Repositórios** | `VeiculoRepository`, `VeiculoMarcaRepository`, `VeiculoModeloRepository` (3 repositórios) |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `VeiculoLista`, `VeiculoDetalhe` (feature em `features/motos/`) |
| **Services** | `VeiculosService` |

### Banco de Dados

`cad_veiculos`, `cad_veiculos_marcas`, `cad_veiculos_modelos` **(3 tabelas)**

### Gaps

- `[Authorize]` ausente nos 3 controllers
- Campo `proximo_km_revisao` / `ultimo_km_registrado` ausente na entidade e no schema SQL
- `VeiculoId` ausente na entidade `OrdemServico`
- Sem formulário de cadastro/edição de veículo no frontend
- Rota `/motos/novo` inexistente em `app.routes.ts`

---

## Módulo Mecânicos

| Campo | Detalhe |
|---|---|
| **Nome** | Mecânicos |
| **Descrição** | Cadastro de colaboradores mecânicos com especialidades, certificações, disponibilidades, documentos, endereços, contatos e histórico de experiências |
| **Status Geral** | **Parcial** — Backend 85%, Frontend 40% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Média |
| **Prioridade** | Média |
| **Dependências** | Ordens de Serviço (N:1 — MecanicoId em OrdemServico) |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `Mecanico`, `MecanicoCertificacao`, `MecanicoContato`, `MecanicoDisponibilidade`, `MecanicoDocumento`, `MecanicoEndereco`, `MecanicoEspecialidade`, `MecanicoEspecialidadeRel`, `MecanicoExperiencia` |
| **Controllers** | `MecanicosController` + 8 subentidades (9 controllers) |
| **Services** | `MecanicoService` + 8 subentidades (9 services) |
| **Repositórios** | `MecanicoRepository` + 8 subentidades (9 repositórios) |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `MecanicoLista`, `MecanicoDetalhe` (2 páginas) |
| **Services** | `MecanicosService` |

### Banco de Dados

`cad_mecanicos`, `cad_mecanicos_certificacoes`, `cad_mecanicos_contatos`, `cad_mecanicos_disponibilidades`, `cad_mecanicos_documentos`, `cad_mecanicos_enderecos`, `cad_mecanicos_especialidades`, `cad_mecanicos_especialidades_rel`, `cad_mecanicos_experiencias` **(9 tabelas)**

### Gaps

- `[Authorize]` ausente nos 9 controllers
- Sem formulário de cadastro/edição no frontend
- Rota `/mecanicos/:id` **completamente ausente** em `app.routes.ts`
- Rota `/mecanicos/novo` inexistente

---

## Módulo Fornecedores

| Campo | Detalhe |
|---|---|
| **Nome** | Fornecedores |
| **Descrição** | Cadastro de fornecedores de peças e serviços, incluindo segmentos, representantes, bancos, certificações, contatos, documentos, endereços e avaliações |
| **Status Geral** | **Parcial** — Backend 85%, Frontend 40% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Baixa |
| **Prioridade** | Média |
| **Dependências** | Estoque (fornecedores de peças via `est_pecas_fornecedores`) |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `Fornecedor`, `FornecedorAvaliacao`, `FornecedorBanco`, `FornecedorCertificacao`, `FornecedorContato`, `FornecedorDocumento`, `FornecedorEndereco`, `FornecedorRepresentante`, `FornecedorSegmento`, `FornecedorSegmentoRel` |
| **Controllers** | `FornecedoresController` + 9 subentidades (10 controllers) |
| **Services** | `FornecedorService` + 9 subentidades (10 services) |
| **Repositórios** | `FornecedorRepository` + 9 subentidades (10 repositórios) |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `FornecedorLista`, `FornecedorDetalhe` (2 páginas) |
| **Services** | `FornecedoresService` |

### Banco de Dados

`cad_fornecedores`, `cad_fornecedores_avaliacoes`, `cad_fornecedores_bancos`, `cad_fornecedores_certificacoes`, `cad_fornecedores_contatos`, `cad_fornecedores_documentos`, `cad_fornecedores_enderecos`, `cad_fornecedores_representantes`, `cad_fornecedores_segmentos`, `cad_fornecedores_segmentos_rel` **(10 tabelas)**

### Gaps

- `[Authorize]` ausente nos 10 controllers
- Sem formulário de cadastro no frontend
- Rota `/fornecedores/novo` inexistente

---

## Módulo Ordens de Serviço

| Campo | Detalhe |
|---|---|
| **Nome** | Ordens de Serviço |
| **Descrição** | Abertura, acompanhamento e encerramento de OS, com itens de serviço/peça, checklists, observações, avaliações, histórico de status e pagamentos |
| **Status Geral** | **Parcial** — Backend 75%, Frontend 20% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Média |
| **Prioridade** | Alta — core do negócio |
| **Dependências** | Clientes (N:1), Mecânicos (N:1), Estoque (via OrdemServicoItem), Financeiro (via OrdemServicoPagamento) |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `OrdemServico`, `OrdemServicoAnexo`, `OrdemServicoAvaliacao`, `OrdemServicoChecklist`, `OrdemServicoHistorico`, `OrdemServicoItem`, `OrdemServicoObservacao`, `OrdemServicoPagamento` |
| **Controllers** | `OrdemServicosController` + 7 subentidades (8 controllers) |
| **Services** | `OrdemServicoService` + 7 subentidades (8 services) |
| **Repositórios** | `OrdemServicoRepository` + 7 subentidades (8 repositórios) |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `OsDetalhe` (1 página — funciona como lista de OS, não como detalhe real) |
| **Services** | `OrdensService` |

### Banco de Dados

`os_ordens`, `os_anexos`, `os_avaliacoes`, `os_checklists`, `os_itens`, `os_observacoes`, `os_ordens_historico`, `os_pagamentos` **(8 tabelas)**

### Gaps

- `[Authorize]` ausente nos 8 controllers
- `VeiculoId` **ausente** na entidade `OrdemServico` — vínculo moto↔OS não existe
- Sem formulário de criação/edição de OS no frontend
- Rotas `/ordens/novo` e `/ordens/:id` inexistentes
- `OsDetalhe` age como lista (violação SRP + nomenclatura errada)

---

## Módulo Estoque

| Campo | Detalhe |
|---|---|
| **Nome** | Estoque |
| **Descrição** | Controle de peças e insumos, com categorias, fabricantes, localizações físicas, movimentações (entrada/saída), histórico de preços e vínculo com fornecedores |
| **Status Geral** | **Parcial** — Backend 85%, Frontend 25% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Baixa |
| **Prioridade** | Alta — impacta OS (consumo de peças) |
| **Dependências** | Fornecedores (via `est_pecas_fornecedores`), Ordens de Serviço |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `EstoquePeca`, `EstoqueCategoria`, `EstoqueFabricante`, `EstoqueLocalizacao`, `EstoqueMovimentacao`, `EstoquePecaAnexo`, `EstoquePecaFornecedor`, `EstoquePecaHistorico` |
| **Controllers** | `EstoquePecasController` + 7 subentidades (8 controllers) |
| **Services** | `EstoquePecaService` + 7 subentidades (8 services) |
| **Repositórios** | `EstoquePecaRepository` + 7 subentidades (8 repositórios) |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `EstoqueLista` (1 página) |
| **Services** | `EstoqueService` |

### Banco de Dados

`est_pecas`, `est_categorias`, `est_fabricantes`, `est_localizacoes`, `est_movimentacoes`, `est_pecas_anexos`, `est_pecas_fornecedores`, `est_pecas_historico` **(8 tabelas)**

### Gaps

- `[Authorize]` ausente nos 8 controllers
- Sem formulário de cadastro/edição de peça no frontend
- Sem página de detalhe de peça
- Rotas `/estoque/novo` e `/estoque/:id` inexistentes

---

## Módulo Financeiro

| Campo | Detalhe |
|---|---|
| **Nome** | Financeiro |
| **Descrição** | Gestão de contas a pagar e a receber, lançamentos financeiros, controle de pagamentos vinculados a OS, métodos de pagamento, histórico e anexos |
| **Status Geral** | **Parcial** — Backend 80%, Frontend 20% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Baixa |
| **Prioridade** | Alta — core do negócio |
| **Dependências** | Ordens de Serviço (pagamentos), Clientes (contas a receber) |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `FinanceiroContaPagar`, `FinanceiroContaReceber`, `FinanceiroPagamento`, `FinanceiroLancamento`, `FinanceiroHistorico`, `FinanceiroAnexo`, `FinanceiroMetodoPagamento` |
| **Controllers** | `FinanceiroContasPagarController`, `FinanceiroContasReceberController`, `FinanceiroPagamentosController`, `FinanceiroLancamentosController`, `FinanceiroHistoricosController`, `FinanceiroAnexosController`, `FinanceiroMetodosPagamentoController` (7 controllers) |
| **Services** | 7 services correspondentes |
| **Repositórios** | 7 repositórios correspondentes |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `FinanceiroDashboard` (1 página — dados mockados) |
| **Services** | `FinanceiroService` |

### Banco de Dados

`fin_contas_pagar`, `fin_contas_receber`, `fin_pagamentos`, `fin_lancamentos`, `fin_historico`, `fin_anexos`, `fin_metodos_pagamento` **(7 tabelas)**

### Gaps

- `[Authorize]` ausente nos 7 controllers
- `FinanceiroPagamento` referencia `OrdemServico` diretamente — viola bounded contexts (CONSTITUTION.md §I)
- Sem listas de contas a pagar/receber no frontend
- Dashboard exibe dados mockados

---

## Módulo Segurança / Auth

| Campo | Detalhe |
|---|---|
| **Nome** | Segurança / Auth |
| **Descrição** | Autenticação JWT, gestão de usuários, perfis e permissões (RBAC), controle de módulos do sistema e audit log de acessos |
| **Status Geral** | **Parcial** — Backend 85%, Frontend 70% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Alta — SEGURANCA_USUARIOS.md, CONSTITUTION.md §III |
| **Prioridade** | Crítica — OWASP A01 bloqueante para todos os módulos |
| **Dependências** | Todos os módulos (RBAC deve proteger todos os endpoints) |

### Backend

| Camada | Artefatos |
|---|---|
| **Domain Entities** | `SegModulo`, `SegPerfil`, `SegPermissao`, `SegUsuario`, `SegPerfilPermissao`, `SegUsuarioPerfil`, `SegAuditLog` |
| **Controllers (Auth)** | `AuthController` (1 controller — público) |
| **Controllers (Segurança)** | `ModulosController`, `PerfisController`, `PermissoesController`, `UsuariosController`, `AuditLogController` (5 controllers — todos com `[Authorize]`) |
| **Services** | `AuthService`, `SegModuloService`, `SegPerfilService`, `SegPermissaoService`, `SegUsuarioService`, `SegAuditLogService` (6 services) |
| **Repositórios** | 7 repositórios |

### Frontend

| Tipo | Artefatos |
|---|---|
| **Páginas** | `Login` funcional |
| **Services** | `AuthService` com Angular Signals |
| **Guards/Interceptors** | `AuthGuard`, `AuthInterceptor` (Bearer token), `ErrorInterceptor` |

### Banco de Dados

`seg_usuarios`, `seg_perfis`, `seg_permissoes`, `seg_modulos`, `seg_perfis_permissoes`, `seg_usuarios_perfis`, `seg_audit_log` **(7 tabelas)**

### Gaps

- RBAC implementado no módulo Segurança mas **não aplicado** nos ~56 controllers de negócio (OWASP A01 — CRÍTICO)
- Sem gestão de usuários/perfis no frontend
- Refresh Token não implementado (token JWT fixo de 8h)

---

## Módulo Dashboard

| Campo | Detalhe |
|---|---|
| **Nome** | Dashboard |
| **Descrição** | Painel executivo com KPIs de OS em aberto, faturamento, peças em estoque crítico e próximas revisões de veículos |
| **Status Geral** | **Parcial** — Backend 50%, Frontend 60% |
| **Cobertura de Testes** | 0% |
| **Cobertura da Documentação** | Média |
| **Prioridade** | Média — depende dos demais módulos |
| **Dependências** | Todos os módulos (agrega dados) |

### Backend

Nenhum controller ou service dedicado. Agrega dados de outros módulos via endpoints existentes.

### Frontend

`DashboardPrincipal` (1 página com KPIs — dados estáticos/mockados)

### Gaps

- Controller `GET /api/v1/PedidosCompra` referenciado em `docs/dashboard.txt` **não existe**
- Campo `proximo_km_revisao` ausente em `Veiculo.cs`
- Gráficos interativos não implementados
- Sem endpoint de aggregation no backend

---

## Métricas Gerais

### Contagem por Camada

| Camada | Qtd |
|---|---|
| Controllers API | **62** (56 sem `[Authorize]`) |
| Services (Application) | **62** |
| Repositórios (Infrastructure) | **63** |
| Tabelas SQL | **63** |
| Frontend Features/Módulos | **9** |
| Frontend Pages implementadas | **~19** |
| Specs de teste | **2** (triviais) |
| Projetos de teste .NET | **0** |

### Distribuição de Controllers por Módulo

| Módulo | Controllers | `[Authorize]` |
|---|---|---|
| Clientes | 11 | ❌ Ausente |
| Mecânicos | 9 | ❌ Ausente |
| Fornecedores | 10 | ❌ Ausente |
| Ordens de Serviço | 8 | ❌ Ausente |
| Estoque | 8 | ❌ Ausente |
| Financeiro | 7 | ❌ Ausente |
| Veículos | 3 | ❌ Ausente |
| Auth | 1 | ✅ (endpoint público por design) |
| Segurança | 5 | ✅ Presente |
| **Total** | **62** | **6 protegidos / 56 expostos** |

---

## Resumo de Status

| Módulo | Backend | Frontend | Status Geral |
|---|---|---|---|
| Clientes | 90% | 75% | Parcial 🟡 |
| Veículos | 80% | 40% | Parcial 🟡 |
| Mecânicos | 85% | 40% | Parcial 🟡 |
| Fornecedores | 85% | 40% | Parcial 🟡 |
| Ordens de Serviço | 75% | 20% | Parcial 🟡 |
| Estoque | 85% | 25% | Parcial 🟡 |
| Financeiro | 80% | 20% | Parcial 🟡 |
| Segurança / Auth | 85% | 70% | Parcial 🟡 |
| Dashboard | 50% | 60% | Parcial 🟡 |

- **Módulos completos:** 0
- **Módulos parciais:** 9
- **Cobertura média de testes:** ~1%
- **Cobertura média backend:** ~82%
- **Cobertura média frontend:** ~43%
- **Completude geral do projeto:** ~53%

### Riscos Críticos

| Risco | Impacto | Módulos afetados |
|---|---|---|
| 🔴 56 controllers sem `[Authorize]` | OWASP A01 — API completamente pública | Todos exceto Segurança |
| 🔴 0 testes automatizados | Sem rede de segurança | Todos |
| 🟡 `VeiculoId` ausente em `OrdemServico` | OS não vincula veículo | OS, Veículos, Dashboard |
| 🟡 Dashboard com dados mockados | Produto parece funcionar mas não reflete dados reais | Dashboard |
| 🟡 Rota `/mecanicos/:id` ausente | `MecanicoDetalhe` inacessível | Mecânicos |