# Contratos de Interface (UI): Detalhe de Mecânico e Link na Listagem

**Feature**: `016-rota-mecanicos-id` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Listagem de Mecânicos (`/mecanicos`) — `MecanicoListaComponent`

### 1.1 Coluna de Link e Ação de Visualização (T-016.2)
Na tabela de mecânicos (`mecanico-lista.html`):
- **Coluna "Nome"**: O nome completo do mecânico (`{{ m.nome }} {{ m.sobrenome }}`) passa a ser um link semântico `<a class="name-link" [routerLink]="['/mecanicos', m.id]">` com efeito hover em laranja (`#f97316`), permitindo clique direto ou abertura em nova guia.
- **Coluna "Ações"**: O botão de visualização ("Visualizar") direciona diretamente para a rota `['/mecanicos', m.id]` via `[routerLink]="['/mecanicos', m.id]"` ou método `openDetails(m)` navegando pelo `Router`.

---

## 2. Página de Detalhes do Mecânico (`/mecanicos/:id`) — `MecanicoDetalheComponent`

### 2.1 Estrutura de Página Roteada (T-016.1)
Quando acessado via rota `/mecanicos/:id`, o componente renderiza uma página completa no `MainLayout` (Dark Theme `#0f172a` / `#1e293b`):

1. **Barra de Topo / Cabeçalho**:
   - Botão/Link **"← Voltar para Mecânicos"** (`routerLink="/mecanicos"`).
   - Título da página com nome do mecânico e badge de código funcional (`#MEC-001`).

2. **Estados de Carregamento e Erro**:
   - **Loading**: Card centralizado com spinner e mensagem `"Carregando dados do mecânico..."`.
   - **Erro / Não Encontrado**: Card informativo com mensagem de erro amigável e botão **"← Voltar para Mecânicos"**.

3. **Layout Principal (Duas Colunas)**:
   - **Coluna Esquerda (Perfil & KPIs)**:
     - Avatar com iniciais e indicador de status ativo/inativo.
     - Nome completo, nível de senioridade e badge de status.
     - KPIs em destaque: **Valor/Hora (`R$ 0,00`)** e **Carga Semanal (`44h`)**.
     - Informações rápidas: Telefone principal, Cidade/UF principal, Data de Admissão, CPF e Código.
     - Botão de retorno **"Voltar"** para `/mecanicos`.
   - **Coluna Direita (Abas de Detalhes — `<p-tabs>`)**:
     - **Aba 0 — Especialidades**: Lista de competências técnicas e destaque para a especialidade principal.
     - **Aba 1 — Disponibilidade**: Grade semanal de horários e capacidade de atendimento.
     - **Aba 2 — Certificações**: Cursos, instituições, datas de conclusão e validade.
     - **Aba 3 — Experiências**: Histórico profissional anterior e resumo de atividades.
     - **Aba 4 — Documentos**: Documentos adicionais registrados.
     - **Aba 5 — Endereços**: Endereços cadastrados com indicação do principal.
     - **Aba 6 — Contatos**: Telefones, celulares e e-mails com indicação do principal.

