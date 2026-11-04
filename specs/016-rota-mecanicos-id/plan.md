# Plano de Implementação: Registrar rota `/mecanicos/:id` e Link na Listagem

**Branch**: `016-rota-mecanicos-id` | **Data**: 2026-11-04 | **Spec**: [spec.md](spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/agents.md`](file:///c:/Projetos/OficinaMotos/governance/agents.md)

**Entrada**: US-016 — Registrar rota `/mecanicos/:id`
- **Prioridade**: 🟡 Should | **Estimativa**: S | **Sprint**: 3 | **Depende**: US-001
- **Tasks**:
  - [ ] T-016.1 — Adicionar `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` em `app.routes.ts`
  - [ ] T-016.2 — Adicionar coluna de link em `MecanicoListaComponent`

---

## Resumo

Esta funcionalidade conclui o fluxo de navegação e consulta detalhada de mecânicos na aplicação web da Oficina MotoPro. Atualmente, a rota `/mecanicos/:id` não está registrada no roteador Angular e o componente `MecanicoDetalhe` opera apenas como um modal `<p-dialog>` dependente de inputs externos.

O plano técnico contempla:
1. **Adaptação de `MecanicoDetalheComponent` (`mecanico-detalhe.ts`, `.html`, `.scss`)**:
   - Suporte nativo para carregamento via rota `/mecanicos/:id` injetando `ActivatedRoute`, `Router` e `MecanicosService`.
   - Exportação de `MecanicoDetalheComponent` (mantendo alias `MecanicoDetalhe` para retrocompatibilidade).
   - Renderização em layout de página completa (Dark Theme) com cabeçalho de navegação ("← Voltar para Mecânicos"), estados de `loading` e `error`, card de perfil/KPIs e as 7 abas de detalhes do colaborador.
2. **Registro da Rota `/mecanicos/:id` (`app.routes.ts`) — T-016.1**:
   - Adição de `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` sob o `MainLayout` (protegido por `authGuard`), posicionado estritamente após `mecanicos/novo`.
3. **Coluna de Link e Navegação na Listagem (`mecanico-lista.ts`, `.html`, `.scss`) — T-016.2**:
   - Exportação de `MecanicoListaComponent` (com alias `MecanicoLista`).
   - Inclusão de link clicável (`[routerLink]="['/mecanicos', m.id]"`) na coluna de nome do mecânico e atualização do botão de ação "Visualizar" para navegar para `/mecanicos/:id`.
4. **Banco de Dados (MySQL 8.0)**:
   - Nenhuma alteração de banco de dados é necessária (endpoint `GET /api/v1/Mecanicos/{id}` já implementado e funcional).
5. **Qualidade e Governança**:
   - Testes unitários automatizados no Vitest para `MecanicoDetalheComponent`, `MecanicoListaComponent` e rotas.
   - Validação de build de produção (`npm run build`) e atualização de [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md).

---

## Artefatos de Design Gerados

- **Pesquisa Técnica**: [research.md](research.md)
- **Modelo de Dados**: [data-model.md](data-model.md)
- **Contratos de API**: [contracts/api.md](contracts/api.md)
- **Contratos de UI**: [contracts/ui.md](contracts/ui.md)
- **Guia Rápido de Verificação**: [quickstart.md](quickstart.md)

---

## Fases de Execução

### Fase 1: Adaptação de `MecanicoDetalheComponent` para Página Roteada (Suporte a T-016.1)
1. Atualizar `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-detalhe/mecanico-detalhe.ts`:
   - Implementar `OnInit` e injetar `ActivatedRoute`, `Router` e `MecanicosService` (com `optional: true` para manter compatibilidade em testes/uso modal).
   - Detectar parâmetro `:id` da rota, validar se é numérico válido e buscar dados via `MecanicosService.get(id)`.
   - Controlar propriedades `loading`, `error`, `isRoutedPage` e `mecanico`.
   - Exportar `MecanicoDetalheComponent` e `MecanicoDetalhe`.
2. Atualizar `mecanico-detalhe.html` e `mecanico-detalhe.scss`:
   - Quando em modo página (`isRoutedPage`), exibir cabeçalho com botão "← Voltar para Mecânicos" (`routerLink="/mecanicos"`), tratamento de `loading`/`error` e o layout de duas colunas (perfil + abas) diretamente na página.
   - Preservar fallback `<p-dialog>` caso utilizado via `@Input() visible`.

### Fase 2: Registro da Rota `/mecanicos/:id` (T-016.1)
1. Atualizar `oficina-motos-web/src/app/app.routes.ts`:
   - Importar `MecanicoDetalheComponent` de `./features/mecanicos/pages/mecanico-detalhe/mecanico-detalhe`.
   - Registrar `{ path: 'mecanicos/:id', component: MecanicoDetalheComponent }` logo após `{ path: 'mecanicos/novo', component: MecanicoCadastroComponent }`.

### Fase 3: Coluna de Link em `MecanicoListaComponent` (T-016.2)
1. Atualizar `oficina-motos-web/src/app/features/mecanicos/pages/mecanico-lista/mecanico-lista.ts`:
   - Injetar `Router` e atualizar `openDetails(item)` para navegar via `this.router.navigate(['/mecanicos', item.id])`.
   - Exportar `MecanicoListaComponent` (mantendo alias `MecanicoLista`).
2. Atualizar `mecanico-lista.html` e `mecanico-lista.scss`:
   - Adicionar link `[routerLink]="['/mecanicos', m.id]"` na célula de nome/código do mecânico e no botão de visualização da coluna de ações.
   - Estilizar o link com destaque hover no padrão Dark Theme (`#f97316`).

### Fase 4: Testes Unitários, Build e Governança
1. Criar testes unitários em `mecanico-detalhe.spec.ts` e `mecanico-lista.spec.ts` validando:
   - Carregamento de mecânico via parâmetro `:id` da rota;
   - Tratamento de ID inválido / erro 404;
   - Presença de links `['/mecanicos', m.id]` na listagem e navegação ao clicar em visualizar;
   - Configuração da rota `mecanicos/:id` em `app.routes.ts`.
2. Executar `npm test -- --no-watch` e `npm run build`.
3. Atualizar `governance/backlog.md` marcando `T-016.1` e `T-016.2` como concluídas na fase de implementação.

