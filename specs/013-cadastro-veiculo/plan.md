# Plano de Implementação: Formulário de Cadastro de Veículo (Motocicleta)

**Branch**: `013-cadastro-veiculo` | **Data**: 2026-09-13 | **Spec**: [spec.md](spec.md)

**Entrada**: US-013 — Criar formulário de cadastro de veículo
- **Prioridade**: 🔴 Must | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-001
- **Critério de aceite**:
  - Rota `/motos/novo` exibe formulário
  - Campos: placa (validação Mercosul), marca, modelo, ano, cor, chassis, KM, proprietário
  - Vínculo obrigatório com cliente existente
- **Tasks**:
  - [ ] T-013.1 — Criar `VeiculoCadastroComponent`
  - [ ] T-013.2 — Registrar rota `/motos/novo`
  - [ ] T-013.3 — Validator de placa em `shared/validators/`
  - [ ] T-013.4 — Autocomplete de marcas e modelos em cascata
  - [ ] T-013.5 — Autocomplete de cliente proprietário

---

## Resumo

A funcionalidade entrega a tela de cadastro de veículos/motocicletas no frontend (`/motos/novo`) através do componente standalone `VeiculoCadastroComponent`. A tela permite registrar uma nova moto vinculada a um cliente proprietário obrigatório, garantindo validação estrita da placa (formatos Mercosul `ABC1D23` e tradicional `ABC-1234`), seleção em cascata de marcas e modelos de motocicletas, e preenchimento de metadados como ano de fabricação/modelo, cor, chassi e quilometragem atual.

No backend, os endpoints RESTful já estão disponibilizados em `VeiculosController` (`POST /api/v1/Veiculos`), `VeiculoMarcasController` (`GET /api/v1/VeiculoMarcas`) e `VeiculoModelosController` (`GET /api/v1/VeiculoModelos`). O serviço frontend `VeiculosService` é utilizado para enviar o payload tipado `CreateVeiculoRequest`, e `ClientesService.search()` é consumido para alimentar o autocomplete reativo do proprietário.

Ao salvar com sucesso, a interface exibe feedback via Toast e navega para a listagem `/motos` ou para a tela de detalhe do veículo (`/motos/:id`), sem recarregamento da aplicação.

---

## Contexto Técnico

**Linguagem/Versão**:
- Frontend: TypeScript 5.9.2 com Angular 21 (Standalone Components + Signals + Reactive Forms)
- Backend: C# 12 / .NET 8 (ASP.NET Core Web API, EF Core 8)

**Dependências Principais**:
- Frontend: `@angular/forms` (`ReactiveFormsModule`, `FormBuilder`, `Validators`), `@angular/router` (`Router`, `RouterLink`), PrimeNG 21 (`ButtonModule`), Serviços centrais (`Toast`, `VeiculosService`, `ClientesService`)
- Backend: `VeiculosController`, `VeiculoService`, `OficinaContext`

**Armazenamento**:
- Banco de dados relacional MySQL 8 (tabela `vei_veiculos`, `vei_marcas`, `vei_modelos`, `cli_clientes`)

**Testes**:
- Frontend: Vitest com Angular TestBed (`npx ng test --no-watch`) cobrindo `placaValidator` e `VeiculoCadastroComponent`
- Backend: xUnit para testes de serviço existentes em `OficinaMotos.SoftDelete.Tests`

**Plataforma Alvo**:
- Navegadores modernos (Desktop e Mobile) consumindo API RESTful

**Tipo de Projeto**:
- Web application com Clean Architecture no backend e SPA Standalone Components no frontend

**Metas de Desempenho**:
- Renderização do formulário em < 200ms
- Resposta da cascata marca -> modelos em < 200ms
- Zero recarregamento de página (0 reloads)

**Restrições**:
- Vínculo obrigatório com cliente existente (`clienteId > 0`);
- Rota `/motos/novo` obrigatoriamente declarada antes de `/motos/:id` no roteador do Angular para evitar conflito de captura de rota dinâmica;
- Validação estrita de placa permitindo Mercosul e tradicional com normalização para caixa alta;
- Utilização obrigatória das rotas centralizadas em `apiPaths` (`apiPaths.veiculos.base`, `marcas`, `modelos`).

---

## Verificação da Constituição

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- [x] **I. Domínio Primeiro (DDD)**: As entidades `Veiculo`, `VeiculoMarca` e `VeiculoModelo` residem na camada de domínio. O cadastro respeita o vínculo com o agregado `Cliente` sem misturar regras de negócio nos controllers.
- [x] **II. API RESTful Versionada**: Utiliza os endpoints canônicos versionados `/api/v1/Veiculos`, `/api/v1/VeiculoMarcas` e `/api/v1/VeiculoModelos` mapeados em `api-paths.ts`.
- [x] **III. Segurança por Design (RBAC + LGPD)**: Rota protegida por `authGuard`; token JWT enviado automaticamente nas requisições HTTP para a API.
- [x] **IV. Frontend Reativo com Componentes Standalone**: O novo componente `VeiculoCadastroComponent` é standalone (`standalone: true`); utiliza Reactive Forms para controle de validação e Signals para reatividade de interface (`loading`, `submitting`, `marcas`, `modelosFiltrados`).
- [x] **V. Integridade e Rastreabilidade de Dados**: Validação estrita de placa e chave estrangeira obrigatória para cliente; integridade referencial com o catálogo de modelos.
- [x] **VI. Qualidade e Testabilidade**: Suíte de testes unitários dedicada em Vitest para o validador de placa (`placa-validator.spec.ts`) e para o componente de cadastro (`veiculo-cadastro.spec.ts`).
- [x] **VII. Documentação como Fonte de Verdade**: Alinhado com o backlog (`EPIC-03`, `US-013`), rastreabilidade garantida em `specs/013-cadastro-veiculo/`.

### Reavaliação Pós-Design
- [x] O backend já possui todas as tabelas, entidades, DTOs e endpoints prontos para receber o cadastro de veículos.
- [x] A precedência da rota `/motos/novo` antes de `/motos/:id` foi tratada na arquitetura de rotas para evitar regressão na navegação.
- [x] O validador customizado é agnóstico e reutilizável em outros formulários da aplicação.

---

## Estrutura do Projeto

### Documentação desta funcionalidade

```text
specs/013-cadastro-veiculo/
├── spec.md
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── checklists/
│   └── requirements.md
└── contracts/
    └── ui.md
```

### Arquivos de Código-Fonte Envolvidos

```text
oficina-motos-web/
├── src/app/
│   ├── app.routes.ts                                         (MODIFICADO - registro da rota /motos/novo antes de /motos/:id)
│   ├── shared/
│   │   └── validators/
│   │       ├── placa-validator.ts                            (NOVO - validador customizado Mercosul e Tradicional)
│   │       ├── placa-validator.spec.ts                       (NOVO - testes unitários do validador de placa)
│   │       └── index.ts                                      (MODIFICADO - export do validador de placa)
│   └── features/
│       └── motos/
│           └── pages/
│               ├── veiculo-lista/
│               │   ├── veiculo-lista.html                    (MODIFICADO - botão de ação "+ Nova Moto" / "Novo Veículo")
│               │   └── veiculo-lista.scss                    (MODIFICADO - estilo do botão de criação)
│               └── veiculo-cadastro/
│                   ├── veiculo-cadastro.ts                   (NOVO - componente standalone do formulário de cadastro)
│                   ├── veiculo-cadastro.html                 (NOVO - template do formulário com tema dark)
│                   ├── veiculo-cadastro.scss                 (NOVO - estilização do formulário)
│                   └── veiculo-cadastro.spec.ts              (NOVO - testes unitários do componente)
```

---

## Fases de Implementação

### Fase 1: Validador de Placa Veicular (T-013.3)
1. Criar `src/app/shared/validators/placa-validator.ts`:
   - Função `isPlacaValid(placa: string): boolean`;
   - Função `cleanPlaca(placa: string): string`;
   - Validador `placaValidator(): ValidatorFn` compatível com Reactive Forms.
   - Padrões: Mercosul `^[A-Z]{3}[0-9][A-Z][0-9]{2}$` e Tradicional `^[A-Z]{3}-?[0-9]{4}$`.
2. Criar testes unitários em `src/app/shared/validators/placa-validator.spec.ts` validando placas válidas e inválidas.
3. Exportar no barrel `src/app/shared/validators/index.ts`.

### Fase 2: Componente `VeiculoCadastroComponent` (T-013.1, T-013.4, T-013.5)
1. Criar `src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.ts`:
   - Standalone component, `ChangeDetectionStrategy.OnPush`;
   - Reactive Form com `FormBuilder`: `clienteId`, `placa`, `marcaId`, `modeloId`, `anoFab`, `anoMod`, `cor`, `chassi`, `km`, `combustivel`, `observacao`, `principal`, `ativo`;
   - Signals para controle de estado: `loadingMarcas`, `loadingModelos`, `loadingClientes`, `submitting`, `errorMessage`, `marcas`, `modelosFiltrados`, `sugestoesClientes`, `clienteSelecionadoNome`;
   - Autocomplete de cliente com busca dinâmica via `ClientesService.search()`;
   - Cascata marca -> modelo: ao selecionar marca, carregar/filtrar modelos com `marcaId` e resetar seleção de modelo;
   - Submissão via `VeiculosService.create()` com tratamento de erros (400, 409, 500) e feedback com Toast.

### Fase 3: Template e Estilização Dark (T-013.1)
1. Criar `veiculo-cadastro.html`:
   - Estrutura em cards com grid responsivo: Dados do Proprietário, Identificação do Veículo (Placa, Chassi, Cor), Especificações (Marca, Modelo, Anos, Combustível, KM) e Observações;
   - Alertas visuais de erro nos inputs com `dirty` / `touched`;
   - Botões "Cancelar / Voltar" e "Salvar Veículo" com feedback de carregamento (`submitting`).
2. Criar `veiculo-cadastro.scss`:
   - Paleta dark nativa (`#0f172a`, `#1e293b`, `#334155`, `#f8fafc`, `#38bdf8`);
   - Inputs, selects, autocomplete dropdown e animações de foco consistentes.

### Fase 4: Registro de Rotas e Navegação (T-013.2)
1. Atualizar `src/app/app.routes.ts`:
   - Adicionar rota `{ path: 'motos/novo', component: VeiculoCadastroComponent }` estritamente antes de `{ path: 'motos/:id', component: VeiculoDetalhe }`.
2. Atualizar `veiculo-lista.html`:
   - Adicionar botão "Novo Veículo" / "+ Nova Moto" com link para `/motos/novo`.

### Fase 5: Testes Unitários do Componente
1. Criar `src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.spec.ts`:
   - Inicialização com formulário inválido;
   - Validação de obrigatoriedade de cliente e placa;
   - Busca e seleção de cliente via autocomplete;
   - Cascata de marca e modelo;
   - Submissão bem-sucedida chamando `VeiculosService.create()`, exibindo Toast e navegando;
   - Tratamento de erro na API com mensagem amigável.

### Fase 6: Validação Final e Governança
1. Executar testes de frontend: `npx ng test --no-watch`;
2. Executar build de produção do frontend: `npm run build`;
3. Atualizar `governance/backlog.md` marcando as tarefas T-013.1 a T-013.5 como concluídas.
