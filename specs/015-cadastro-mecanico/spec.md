# Especificação de Funcionalidade: Formulário de Cadastro de Mecânico

**Feature Branch**: `015-cadastro-mecanico`

**Criado**: 2026-09-13

**Status**: Draft

**Entrada**: US-015 — Criar formulário de cadastro de mecânico
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-001
- **Critério de aceite**:
  - Rota `/mecanicos/novo` exibe formulário de cadastro de profissional
  - Campos: código funcional, nome completo, apelido/nome social, CPF com validação de dígitos e unicidade, data de nascimento, data de admissão, status, nível de senioridade, valor da hora técnica, carga horária e observações
  - Multi-select de especialidades técnicas com opção de marcar a especialidade principal
  - Validação estrita de CPF único impedindo duplicidade no banco
- **Tasks**:
  - [ ] T-015.1 — Criar `MecanicoCadastroComponent`
  - [ ] T-015.2 — Registrar rota `/mecanicos/novo`
  - [ ] T-015.3 — Validator de CPF único
  - [ ] T-015.4 — Multi-select de especialidades

**Dependências**: US-001 (Autenticação e RBAC), [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md), [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md), [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md), `MecanicosService`, `MecanicosController` e `MecanicoEspecialidadesController`.

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Cadastro Completo de Mecânico no Sistema (Prioridade: P1)

Como gerente ou administrador da oficina, quero acessar a rota `/mecanicos/novo` para registrar um novo mecânico na equipe, informando dados pessoais, contratuais e valor da hora técnica, para que o profissional possa ser escalado em Ordens de Serviço e tenha sua produtividade acompanhada.

**Por que esta prioridade**: Os mecânicos são os executores operacionais da oficina. Sem mecânicos cadastrados e ativos, não é possível atribuir responsáveis técnicos nas Ordens de Serviço (`US-009` e `US-010`) nem calcular custos de mão de obra.

**Teste independente**: Acessar `/mecanicos/novo`, preencher um código funcional único (ex.: `MEC-010`), nome ("Carlos"), sobrenome ("Oliveira"), CPF válido e não cadastrado, data de admissão, nível ("Pleno"), valor hora (`85.00`), selecionar especialidades, salvar o formulário e verificar que a requisição `POST /api/v1/Mecanicos` cria o registro, redireciona para `/mecanicos` e exibe Toast de sucesso.

**Cenários de aceitação**:

1. **Dado** que o usuário autenticado acessa a URL `/mecanicos/novo`, **quando** a página carregar, **então** o formulário de cadastro de mecânico deve ser exibido com seções organizadas em cards: Identificação Pessoal, Dados Profissionais & Contratuais, Especialidades Técnicas e Observações.
2. **Dado** que o usuário preenche todos os campos obrigatórios válidos, **quando** clicar em "Salvar Mecânico", **então** o sistema deve disparar `POST /api/v1/Mecanicos` com o payload correto (`codigo`, `nome`, `sobrenome`, `documentoPrincipal`, `dataAdmissao`, `nivel`, `valorHora`, `status`, etc.).
3. **Dado** que a API responde com sucesso (HTTP 201 Created), **quando** o registro for confirmado, **então** o sistema deve exibir Toast de sucesso e navegar para a listagem `/mecanicos`.
4. **Dado** que o formulário contém campos obrigatórios não preenchidos ou inválidos, **quando** o usuário tentar salvar, **então** o formulário deve marcar os campos com destaque de erro e exibir mensagens claras inline sem realizar chamada à API.

---

### História de Usuário 2 — Validador de CPF Único (Prioridade: P1)

Como atendente ou administrador da oficina, quero que o campo de CPF do mecânico valide tanto os dígitos verificadores da Receita Federal quanto a unicidade do documento em relação aos mecânicos já cadastrados, para evitar fraudes, duplicidade cadastral e inconsistências fiscais.

**Por que esta prioridade**: Cada colaborador deve ter identificador fiscal unívoco no sistema. O cadastro duplicado de CPF corrompe o histórico de ordens, comissões e auditoria.

**Teste independente**: Inserir um CPF inválido matematicamente (ex.: `111.111.111-11`) e constatar erro "CPF inválido". Em seguida, inserir um CPF válido que já pertença a outro mecânico cadastrado e verificar que o validador sinaliza "Este CPF já está cadastrado para outro mecânico". Por fim, inserir um CPF válido não cadastrado e constatar validação positiva.

**Cenários de aceitação**:

1. **Dado** o campo de CPF, **quando** o usuário digita os números, **então** o campo deve aplicar máscara de formatação (`000.000.000-00`) e validar os dígitos verificadores.
2. **Dado** que o CPF digitado já existe na base de dados de mecânicos, **quando** o validador verificar o documento, **então** o campo deve ser marcado como inválido com a mensagem de erro "Este CPF já está cadastrado para outro mecânico".
3. **Dado** que o CPF digitado é matematicamente válido e não pertence a nenhum outro colaborador, **quando** o campo for validado, **então** nenhum erro de unicidade deve ser apresentado.
4. **Dado** que a validação de unicidade reside em validador reutilizável no frontend (`cpfUnicoMecanicoValidator` em `src/app/shared/validators/`), **quando** testada em suíte unitária, **então** deve cobrir casos válidos, duplicados e formatos incorretos.

---

### História de Usuário 3 — Seleção Múltipla de Especialidades Técnicas (Multi-Select) (Prioridade: P1)

Como gerente técnico da oficina, quero selecionar múltiplas especialidades nas quais o mecânico é capacitado (ex.: Injeção Eletrônica, Motor, Suspensão, Elétrica, Pintura) e eleger uma especialidade principal, para que o sistema possa sugerir o mecânico mais apto para cada tipo de serviço em uma Ordem de Serviço.

**Por que esta prioridade**: Oficinas modernas atendem motos de diversas cilindradas e tecnologias; saber as competências exatas de cada colaborador otimiza a produtividade da equipe e a qualidade do atendimento.

**Teste independente**: No formulário de cadastro, abrir o componente de seleção de especialidades carregadas dinamicamente via `MecanicosService.especialidades()`, selecionar 3 especialidades (ex.: "Motor", "Injeção Eletrônica" e "Freios"), marcar "Injeção Eletrônica" como principal, salvar o mecânico e conferir que as relações são devidamente persistidas.

**Cenários de aceitação**:

1. **Dado** o card "Especialidades Técnicas", **quando** a página for inicializada, **então** o sistema deve carregar as especialidades disponíveis via `MecanicosService.especialidades()`.
2. **Dado** a lista de especialidades, **quando** o usuário interagir com o componente multi-select / checkboxes estilizados, **então** deve ser possível marcar uma ou mais especialidades simultaneamente.
3. **Dado** que ao menos uma especialidade foi selecionada, **quando** o usuário definir uma como "Principal", **então** o campo `especialidadePrincipalId` do mecânico deve ser preenchido com o ID correspondente.
4. **Dado** que o mecânico foi salvo com múltiplas especialidades, **quando** o cadastro for finalizado com sucesso, **então** as relações devem ser vinculadas via `MecanicosService.vincularEspecialidade()`.

---

### História de Usuário 4 — Registro de Rota `/mecanicos/novo` e Acesso na Listagem (Prioridade: P1)

Como usuário do sistema, quero clicar no botão "+ Novo Mecânico" na listagem de mecânicos (`/mecanicos`) e ser conduzido imediatamente para a rota `/mecanicos/novo`, com controle de acesso RBAC e layout integrado.

**Por que esta prioridade**: Completar o ciclo de navegação sem dead links ou rotas órfãs.

**Teste independente**: Na tela `/mecanicos`, clicar no botão "+ Novo Mecânico" e verificar a navegação para `/mecanicos/novo`; conferir que no roteador Angular a rota `/mecanicos/novo` está devidamente registrada com `authGuard`.

**Cenários de aceitação**:

1. **Dado** a tabela de mecânicos em `/mecanicos`, **quando** o usuário clicar no botão "+ Novo Mecânico", **então** a aplicação deve navegar para `/mecanicos/novo` sem recarregar a página.
2. **Dado** a configuração de rotas em `app.routes.ts`, **quando** inspecionada, **então** deve conter `{ path: 'mecanicos/novo', component: MecanicoCadastroComponent, canActivate: [authGuard] }` posicionado antes de rotas com parâmetros dinâmicos.
3. **Dado** o botão "Cancelar" no formulário de cadastro, **quando** acionado, **então** o usuário deve retornar à rota `/mecanicos`.

---

## Requisitos Funcionais (FR)

- **FR-001**: Criar o componente standalone `MecanicoCadastroComponent` em `src/app/features/mecanicos/pages/mecanico-cadastro/mecanico-cadastro.ts`.
- **FR-002**: O componente deve utilizar `ChangeDetectionStrategy.OnPush`, Reactive Forms (`FormBuilder`) e Angular Signals para o estado da tela (`loading`, `submitting`, `especialidades`, `especialidadesSelecionadas`).
- **FR-003**: Registrar a rota `/mecanicos/novo` em `src/app/app.routes.ts` com proteção do `authGuard`.
- **FR-004**: O formulário reativo deve conter os seguintes controles com validações estritas:
  - `codigo`: obrigatório, max 40 caracteres, sugestão automática ou digitação livre (ex: `MEC-001`);
  - `nome`: obrigatório, mínimo 2 e máximo 160 caracteres;
  - `sobrenome`: opcional, max 160 caracteres;
  - `nomeSocial`: opcional, max 160 caracteres;
  - `documentoPrincipal` (CPF): obrigatório, 11 dígitos numéricos, validação de algoritmo de CPF e validação assíncrona/reativa de unicidade;
  - `dataNascimento`: opcional, data válida no passado;
  - `dataAdmissao`: obrigatória, data no passado ou presente (default: data atual);
  - `status`: obrigatório, default `"Ativo"`;
  - `nivel`: obrigatório, opções: `"Junior"`, `"Pleno"`, `"Senior"`, `"Especialista"` (default: `"Pleno"`);
  - `valorHora`: obrigatório, numérico positivo (`Validators.min(0)`);
  - `cargaHorariaSemanal`: obrigatório, default `44` horas;
  - `especialidadePrincipalId`: opcional/automático baseado na seleção;
  - `especialidadesIds`: array de IDs de especialidades selecionadas;
  - `observacoes`: opcional, max 500 caracteres.
- **FR-005**: Criar o validador de CPF único em `src/app/shared/validators/cpf-unico-mecanico.validator.ts` integrando com o `MecanicosService` e exportá-lo no barrel `src/app/shared/validators/index.ts`.
- **FR-006**: Carregar a lista de especialidades ativas via `MecanicosService.especialidades()` na inicialização (`ngOnInit`).
- **FR-007**: Implementar componente multi-select de especialidades no template com chips/checkboxes visuais que permitam marcar/desmarcar especialidades e eleger a principal.
- **FR-008**: Ao submeter o formulário com sucesso, disparar `POST /api/v1/Mecanicos` e, caso haja especialidades adicionais selecionadas, efetuar o vínculo das especialidades secundárias via `MecanicosService.vincularEspecialidade()`.
- **FR-009**: Exibir notificação Toast de sucesso (`toast.success`) e redirecionar para `/mecanicos`.
- **FR-010**: Tratar erros da API com notificação Toast de erro (`toast.error`) sem perder os dados preenchidos no formulário.
- **FR-011**: Atualizar o botão "+ Novo Mecânico" em `mecanico-lista.html` para direcionar via `routerLink="/mecanicos/novo"` com controle de permissão RBAC.
- **FR-012**: Estilizar o formulário em `mecanico-cadastro.scss` respeitando rigorosamente o tema dark da aplicação (`#0f172a`, `#1e293b`, `#334155`, `#f8fafc`, `#f97316`).
- **FR-013**: Criar suíte de testes unitários em `mecanico-cadastro.spec.ts` cobrindo inicialização, validação de campos obrigatórios, cálculo de CPF único, multi-select de especialidades e submissão com sucesso.

---

## Requisitos Não Funcionais (NFR)

- **NFR-001 (Consistência Arquitetural)**: O componente deve ser 100% Standalone, sem módulos legados, alinhado com [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md).
- **NFR-002 (Performance & UX)**: Renderização do formulário em menos de 200ms e transições suaves sem recarregar a aplicação.
- **NFR-003 (Acessibilidade e Usabilidade)**: Mensagens de erro claras em português para cada controle inválido, focando na prevenção de erros de digitação de CPF.
- **NFR-004 (Design System)**: Utilização padronizada dos tokens dark theme, cards agrupadores e botões com gradiente laranja `#f97316` idênticos ao padrão do projeto.

---

## Casos de Borda

1. **CPF repetido com pontuação diferente**: A validação deve limpar a máscara (`replace(/\D/g, '')`) antes de comparar com a base de dados.
2. **Nenhuma especialidade selecionada**: O cadastro de mecânico é permitido sem especialidades iniciais, ficando `especialidadePrincipalId = null`.
3. **Apenas uma especialidade selecionada**: Essa especialidade deve ser automaticamente definida como a `especialidadePrincipalId`.
4. **Remoção da especialidade principal**: Se a especialidade principal for desmarcada, o sistema deve automaticamente reatribuir a primeira especialidade restante ou deixar nulo.
5. **Valor hora zerado ou negativo**: Bloqueado pelo validador numérico (`Validators.min(0)`).

---

## Critérios de Sucesso Mensuráveis (SC)

- **SC-001**: Rota `/mecanicos/novo` totalmente funcional e protegida por autenticação.
- **SC-002**: Validador de CPF único impede submissão de CPFs já cadastrados com feedback visual instantâneo.
- **SC-003**: Seleção múltipla de especialidades com persistência no backend.
- **SC-004**: 100% dos testes unitários do frontend passando no Vitest (`npm test -- --no-watch`).
- **SC-005**: Build de produção Angular (`npm run build`) concluído sem erros.
- **SC-006**: Backlog em [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) atualizado com as tarefas concluídas.

