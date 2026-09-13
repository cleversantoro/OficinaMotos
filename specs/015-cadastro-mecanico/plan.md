# Plano de Implementação: Formulário de Cadastro de Mecânico

**Branch**: `015-cadastro-mecanico` | **Data**: 2026-09-13 | **Spec**: [spec.md](spec.md)
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)

**Entrada**: US-015 — Criar formulário de cadastro de mecânico
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-001
- **Tasks**:
  - [ ] T-015.1 — Criar `MecanicoCadastroComponent`
  - [ ] T-015.2 — Registrar rota `/mecanicos/novo`
  - [ ] T-015.3 — Validator de CPF único
  - [ ] T-015.4 — Multi-select de especialidades

---

## Resumo

Esta funcionalidade entrega o fluxo completo de cadastro de profissionais mecânicos na aplicação web da Oficina MotoPro. Atualmente, a rota `/mecanicos` exibe a lista de mecânicos e possui um botão de cadastro sem navegação ativa, enquanto a rota `/mecanicos/novo` não está registrada.

O plano contempla:
1. **Validador de CPF Único (`cpfUnicoMecanicoValidator`)**: Criação de validador reutilizável que assegura a integridade de formato e a unicidade em relação aos mecânicos já cadastrados.
2. **Componente `MecanicoCadastroComponent`**: Componente standalone em Angular 21 com Signals, Reactive Forms, OnPush e Dark Theme.
3. **Multi-select de Especialidades**: Seleção múltipla interativa de competências técnicas com eleição da especialidade principal e persistência das relações.
4. **Roteamento e Ações de Lista**: Registro da rota `/mecanicos/novo` em `app.routes.ts` e atualização do botão "+ Novo Mecânico" na listagem.
5. **Garantia de Qualidade**: Testes unitários no Vitest cobrindo o validador e o componente, build de produção e atualização do backlog de governança.

---

## Fases de Execução

### Fase 1: Validador de CPF Único (T-015.3)
1. Criar `oficina-motos-web/src/app/shared/validators/cpf-unico-mecanico.validator.ts`:
   - Validar formatação e dígitos verificadores com `cleanDocument()` e cálculo de módulo 11.
   - Fornecer função que valida contra coleção de mecânicos ou chamada assíncrona.
2. Criar testes unitários em `cpf-unico-mecanico.validator.spec.ts`.
3. Exportar no barrel `src/app/shared/validators/index.ts`.

### Fase 2: Componente `MecanicoCadastroComponent` (T-015.1, T-015.4)
1. Criar classe `MecanicoCadastroComponent` (`mecanico-cadastro.ts`):
   - Standalone component, OnPush, `FormBuilder`, Signals (`loading`, `submitting`, `especialidades`, `selecionadas`, `principalId`).
   - Carregamento de especialidades no `ngOnInit` via `MecanicosService.especialidades()`.
   - Métodos para alternar seleção de especialidades (`toggleEspecialidade`) e definir especialidade principal (`setPrincipal`).
   - Método `salvar()` com chamada a `MecanicosService.create()` e subsequentes vínculos em `vincularEspecialidade()`.
2. Criar template `mecanico-cadastro.html`:
   - Cards temáticos: Identificação, Dados Profissionais, Especialidades (Multi-select) e Observações.
   - Mensagens de erro inline para cada controle inválido.
3. Criar estilos `mecanico-cadastro.scss`:
   - Compatibilidade total com o tema escuro do projeto.
4. Criar testes unitários `mecanico-cadastro.spec.ts`.

### Fase 3: Roteamento e Navegação (T-015.2)
1. Registrar a rota `/mecanicos/novo` em `src/app/app.routes.ts` com `authGuard`.
2. Adicionar `RouterLink` e atualizar o botão "+ Novo Mecânico" em `mecanico-lista.html` e `mecanico-lista.ts`.

### Fase 4: Validação Final e Governança
1. Executar suíte de testes com `npm test -- --no-watch`.
2. Executar compilação de produção com `npm run build`.
3. Atualizar tarefas em `governance/backlog.md` e marcar como concluídas.

