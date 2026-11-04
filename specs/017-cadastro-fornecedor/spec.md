# Especificação de Funcionalidade: Formulário de Cadastro de Fornecedor

**Feature Branch**: `017-cadastro-fornecedor`

**Criado**: 2026-11-04

**Status**: Draft

**Entrada**: US-017 — Criar formulário de cadastro de fornecedor
- **Prioridade**: 🟡 Should | **Estimativa**: M | **Sprint**: 3 | **Depende**: US-001
- **Critério de aceite**:
  - Rota `/fornecedores/novo` exibe o formulário de cadastro de fornecedor.
  - Campos cadastrais, fiscais, comerciais e de endereço: código, tipo, razão social, nome fantasia, CNPJ com validação de dígitos verificadores e máscara, inscrição estadual/municipal, segmento principal (dropdown dinâmico), contatos (e-mail, telefone, website), condições comerciais (condição de pagamento, prazo médio de entrega, prazo de garantia, retirada local, atendimento personalizado, observações) e endereço principal com busca automática por CEP.
  - Validador de CNPJ (`cnpjValidator`) integrado ao controle `documento`.
  - Integração com `CepService` (`ViaCEP`) para autopreenchimento de logradouro, bairro, cidade e UF ao informar um CEP válido.
  - Dropdown de segmentos carregado dinamicamente via `FornecedoresService.segmentos()`.
- **Tasks**:
  - [ ] T-017.1 — Criar `FornecedorCadastroComponent`
  - [ ] T-017.2 — Registrar rota `/fornecedores/novo`
  - [ ] T-017.3 — Validator de CNPJ e integração `CepService`
  - [ ] T-017.4 — Dropdown de segmentos

**Dependências**: US-001 (Autenticação e RBAC), [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md), [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md), [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md), `FornecedoresService`, `CepService`, `FornecedoresController`, `FornecedorSegmentosController` e `FornecedorEnderecosController`.

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Cadastro Completo de Fornecedor no Sistema (Prioridade: P1)

Como comprador, gerente ou administrador da oficina, quero acessar a rota `/fornecedores/novo` para cadastrar um novo fornecedor de peças, insumos ou serviços, registrando seus dados empresariais, contato, condições comerciais e endereço, para que a oficina possa vincular peças em estoque e gerenciar compras e cotações.

**Por que esta prioridade**: O cadastro de fornecedores é pré-requisito direto para o cadastro de peças no estoque (`US-018`) e para a gestão financeira de contas a pagar (`EPIC-04`).Atualmente, o botão "+ Novo fornecedor" em `/fornecedores` aponta para `/fornecedores/novo`, mas a rota e o componente ainda não existem.

**Teste independente**: Acessar `/fornecedores/novo`, preencher código (`FOR-010`), razão social ("Moto Peças Brasil Ltda"), nome fantasia ("MotoBrasil"), CNPJ válido (`11.222.333/0001-81`), selecionar um segmento no dropdown, informar contato, preencher CEP válido para autopreencher o endereço, clicar em "Salvar Fornecedor" e verificar que `POST /api/v1/Fornecedores` é executado com sucesso, exibindo Toast e redirecionando para `/fornecedores`.

**Cenários de aceitação**:

1. **Dado** que o usuário autenticado acessa `/fornecedores/novo`, **quando** a página carregar, **então** o formulário de cadastro deve ser exibido organizado em cards temáticos: Identificação & Dados Fiscais, Segmento & Contato, Endereço Principal (com busca por CEP) e Condições Comerciais & Observações.
2. **Dado** que o usuário preenche todos os campos obrigatórios com valores válidos, **quando** clicar em "Salvar Fornecedor", **então** o sistema deve enviar `POST /api/v1/Fornecedores` com o payload `CreateFornecedorDTO` e, caso o endereço tenha sido preenchido, registrar o endereço principal via `POST /api/v1/FornecedorEnderecos`.
3. **Dado** que a API confirma a criação do fornecedor, **quando** o fluxo concluir, **então** o sistema deve exibir Toast de sucesso (`toast.success`) e navegar para `/fornecedores`.
4. **Dado** que o formulário possui campos obrigatórios vazios ou inválidos, **quando** o usuário tentar submeter, **então** os controles inválidos devem ser destacados com mensagens de erro claras em português sem realizar chamada à API.

---

### História de Usuário 2 — Validação de CNPJ e Autopreenchimento de Endereço via `CepService` (Prioridade: P1)

Como usuário responsável pelo cadastro de fornecedores, quero que o sistema valide automaticamente os dígitos verificadores do CNPJ digitado e preencha automaticamente o logradouro, bairro, cidade e estado ao informar o CEP do fornecedor via `CepService`, reduzindo erros de digitação e agilizando o cadastro.

**Por que esta prioridade**: Evita cadastros com CNPJ fiscalmente inválido e elimina o preenchimento manual repetitivo de dados de logradouro e município.

**Teste independente**: No campo CNPJ, digitar `11.111.111/1111-11` e verificar a mensagem "CNPJ inválido". Em seguida, digitar `11.222.333/0001-81` e verificar que o campo torna-se válido. Na seção de endereço, digitar o CEP `01001-000` e verificar que `CepService.buscarCep()` preenche automaticamente `logradouro` ("Praça da Sé"), `bairro` ("Sé"), `cidade` ("São Paulo") e `estado` ("SP").

**Cenários de aceitação**:

1. **Dado** o campo de CNPJ (`documento`), **quando** o usuário digita os números, **então** o sistema deve aplicar a máscara `00.000.000/0000-00` e validar a quantidade de dígitos (14) e os dígitos verificadores por meio de `cnpjValidator()`.
2. **Dado** um CNPJ com dígitos verificadores incorretos ou números repetidos, **quando** o controle for validado, **então** o formulário deve bloquear o envio e exibir a mensagem de erro correspondente.
3. **Dado** o campo de CEP na seção de endereço, **quando** o usuário digitar 8 dígitos válidos ou acionar o botão "Buscar CEP", **então** o componente deve invocar `CepService.buscarCep(cep)` e preencher automaticamente os campos `logradouro`, `bairro`, `cidade`, `estado` (UF) e `complemento` (quando retornado).
4. **Dado** um CEP inexistente na base do ViaCEP, **quando** a consulta retornar erro, **então** o sistema deve sinalizar o erro ao usuário e permitir o preenchimento manual dos campos de endereço sem travar o formulário.

---

### História de Usuário 3 — Dropdown Dinâmico de Segmentos de Fornecedor (Prioridade: P1)

Como gestor de compras, quero selecionar o segmento principal de atuação do fornecedor (ex.: Peças Originais OEM, Pneus & Câmaras, Lubrificantes & Fluidos, Acessórios, Ferramentas) em um dropdown alimentado pela API, para classificar corretamente a carteira de fornecedores da oficina.

**Por que esta prioridade**: A categorização por segmento permite filtrar fornecedores por especialidade de suprimento e vinculá-los adequadamente às categorias de peças.

**Teste independente**: Ao abrir `/fornecedores/novo`, verificar que o dropdown "Segmento Principal" é carregado com os segmentos ativos vindos de `FornecedoresService.segmentos()`, selecionar um segmento e confirmar que seu `id` é enviado em `segmentoPrincipalId` no payload de criação e vinculado em `FornecedorSegmentosRel`.

**Cenários de aceitação**:

1. **Dado** a inicialização do `FornecedorCadastroComponent`, **quando** `ngOnInit` executar, **então** o componente deve buscar a lista de segmentos via `FornecedoresService.segmentos()`.
2. **Dado** a lista de segmentos retornada pela API, **quando** renderizada no dropdown, **então** apenas segmentos ativos devem ser listados para seleção, exibindo seu nome.
3. **Dado** que o usuário seleciona um segmento principal no dropdown, **quando** o fornecedor for salvo, **então** o campo `segmentoPrincipalId` deve conter o ID numérico do segmento selecionado.

---

### História de Usuário 4 — Registro da Rota `/fornecedores/novo` e Navegação (Prioridade: P1)

Como usuário autenticado, quero clicar no botão "+ Novo fornecedor" na listagem (`/fornecedores`) e acessar a rota `/fornecedores/novo` sem conflito com a rota `/fornecedores/:id`.

**Por que esta prioridade**: Conecta a listagem existente ao novo formulário de cadastro, garantindo a precedência correta de rotas no Angular Router.

**Teste independente**: Em `/fornecedores`, clicar em "+ Novo fornecedor" e verificar que a rota `/fornecedores/novo` renderiza `FornecedorCadastroComponent` (e não `FornecedorDetalhe`). Clicar em "Cancelar" ou "← Voltar" e verificar o retorno para `/fornecedores`.

**Cenários de aceitação**:

1. **Dado** o arquivo `src/app/app.routes.ts`, **quando** inspecionado, **então** a rota `{ path: 'fornecedores/novo', component: FornecedorCadastroComponent }` deve estar registrada sob o `MainLayout` (com `authGuard`) e posicionada ESTRITAMENTE ANTES de `{ path: 'fornecedores/:id', component: FornecedorDetalhe }`.
2. **Dado** a listagem de fornecedores em `/fornecedores`, **quando** o usuário clicar em "+ Novo fornecedor", **então** o roteador deve abrir `/fornecedores/novo`.
3. **Dado** o botão "Cancelar" no formulário de cadastro, **quando** acionado, **então** o usuário deve retornar para `/fornecedores`.

---

## Casos de Borda

1. **Ordem de rotas no Angular Router**: Se `fornecedores/novo` fosse declarado após `fornecedores/:id`, o Angular interpretaria `"novo"` como `:id`. Por isso, `fornecedores/novo` deve obrigatoriamente anteceder `fornecedores/:id`.
2. **CNPJ com ou sem pontuação**: Antes de enviar ao backend, o campo `documento` pode ser higienizado (14 dígitos numéricos) ou formatado conforme padrão da base, e o `cnpjValidator()` deve validar corretamente em ambos os formatos.
3. **Falha ou lentidão na API ViaCEP**: Se o serviço externo ViaCEP estiver indisponível ou o CEP não for encontrado, o indicador `searchingCep` deve ser encerrado e os campos de endereço devem continuar editáveis manualmente.
4. **Cadastro sem endereço preenchido**: Os campos de endereço são opcionais para a criação básica do fornecedor; porém, se o usuário informar CEP/logradouro, os campos essenciais do endereço (`logradouro`, `numero`, `bairro`, `cidade`, `estado`) são enviados para criação do endereço principal.
5. **Lista de segmentos vazia na API**: Caso o backend ainda não possua segmentos cadastrados, o dropdown deve exibir opção padrão informativa sem impedir o salvamento do fornecedor (`segmentoPrincipalId: null`).

---

## Requisitos Funcionais (FR)

- **FR-001**: Criar o componente standalone `FornecedorCadastroComponent` em `src/app/features/fornecedores/pages/fornecedor-cadastro/fornecedor-cadastro.ts` (com `.html` e `.scss`).
- **FR-002**: Utilizar `ChangeDetectionStrategy.OnPush`, Reactive Forms (`FormBuilder`) e Angular Signals (`loading`, `submitting`, `searchingCep`, `segmentos`).
- **FR-003**: Registrar a rota `{ path: 'fornecedores/novo', component: FornecedorCadastroComponent }` em `src/app/app.routes.ts` antes de `fornecedores/:id`, protegida por `authGuard`.
- **FR-004**: O formulário reativo deve conter os seguintes controles e validações:
  - `codigo`: obrigatório, max 40 caracteres, com sugestão automática inicial (ex.: `FOR-001`);
  - `tipo`: obrigatório, opções `"FABRICANTE"`, `"DISTRIBUIDOR"`, `"IMPORTADOR"`, `"PRESTADOR_SERVICO"`, `"VAREJISTA"` (default: `"DISTRIBUIDOR"`);
  - `razaoSocial`: obrigatório, mínimo 3 e máximo 200 caracteres;
  - `nomeFantasia`: obrigatório, mínimo 2 e máximo 160 caracteres;
  - `documento` (CNPJ): obrigatório, validado com `cnpjValidator()` e máscara `00.000.000/0000-00`;
  - `inscricaoEstadual`: opcional, max 40 caracteres;
  - `inscricaoMunicipal`: opcional, max 40 caracteres;
  - `segmentoPrincipalId`: opcional/numérico, selecionado via dropdown dinâmico de segmentos;
  - `status`: obrigatório, opções `"ATIVO"`, `"INATIVO"`, `"SUSPENSO"` (default: `"ATIVO"`);
  - `email`: opcional, validado com `Validators.email`;
  - `telefonePrincipal`: opcional, com máscara de telefone/celular;
  - `website`: opcional, max 200 caracteres;
  - `condicaoPagamentoPadrao`: opcional (ex.: `"30/60/90 dias"`, `"À vista"`, `"28 dias"`);
  - `prazoEntregaMedio`: opcional, numérico (`Validators.min(0)`), em dias;
  - `prazoGarantiaPadrao`: opcional, texto livre (ex.: `"90 dias"`);
  - `atendimentoPersonalizado`: booleano (default: `false`);
  - `retiradaLocal`: booleano (default: `false`);
  - `observacoes`: opcional, max 500 caracteres;
  - Subgrupo/controles de endereço (`cep`, `logradouro`, `numero`, `bairro`, `cidade`, `estado`, `complemento`) com `cepValidator()` no campo `cep`.
- **FR-005**: Integrar `CepService` (`src/app/shared/services/cep.ts`) para consultar automaticamente o CEP ao atingir 8 dígitos numéricos ou clicar no botão de busca, preenchendo `logradouro`, `bairro`, `cidade`, `estado` (`uf`) e `complemento`.
- **FR-006**: Carregar os segmentos de fornecedor na inicialização via `FornecedoresService.segmentos()` e popular o dropdown de Segmento Principal.
- **FR-007**: Adicionar método `criarEndereco(body)` em `FornecedoresService` (usando `apiPaths.fornecedores.enderecos`) para persistir o endereço principal quando informado no formulário.
- **FR-008**: Ao submeter o formulário válido:
  1. Enviar `POST /api/v1/Fornecedores` via `FornecedoresService.create(payload)`;
  2. Se houver endereço preenchido (`logradouro` e `cidade`), enviar `POST /api/v1/FornecedorEnderecos`;
  3. Se houver `segmentoPrincipalId`, vincular opcionalmente em `FornecedoresService.vincularSegmento()`;
  4. Exibir `toast.success` e navegar para `/fornecedores`.
- **FR-009**: Em caso de erro na API, exibir `toast.error` preservando os dados preenchidos pelo usuário.
- **FR-010**: Criar suíte de testes unitários em `fornecedor-cadastro.spec.ts` validando criação do componente, validação de CNPJ, busca de CEP via `CepService`, dropdown de segmentos, submissão à API e registro da rota `/fornecedores/novo`.

---

## Requisitos Não Funcionais (NFR)

- **NFR-001 (Arquitetura)**: Componente 100% Standalone Angular 21, `ChangeDetectionStrategy.OnPush`, Signals e Reactive Forms, em conformidade com [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md).
- **NFR-002 (Design System)**: Tema escuro (Dark Mode `#0f172a`, `#1e293b`, `#334155`, `#f8fafc`, destaque `#f97316`) consistente com `MecanicoCadastroComponent` e `VeiculoCadastroComponent`.
- **NFR-003 (Sem Alteração de Banco de Dados)**: Não requer alterações no schema MySQL 8.0, pois todas as tabelas e endpoints de fornecedores já existem.

---

## Entidades Envolvidas

- **Fornecedor (`Fornecedor` / `CreateFornecedorDTO`)**: Entidade principal (`for_fornecedores`) com dados jurídicos, fiscais, contato e condições comerciais.
- **FornecedorSegmento (`FornecedorSegmento`)**: Catálogo de segmentos de fornecimento (`for_segmentos`) exibido no dropdown.
- **FornecedorEndereco (`CreateFornecedorEnderecoRequest`)**: Endereço comercial principal do fornecedor (`for_fornecedores_enderecos`) preenchido com apoio do `CepService`.

---

## Critérios de Sucesso Mensuráveis (SC)

- **SC-001**: Rota `/fornecedores/novo` acessível, protegida por `authGuard` e registrada antes de `/fornecedores/:id`.
- **SC-002**: Validador de CNPJ rejeita CNPJs inválidos e aceita CNPJs válidos com formatação automática.
- **SC-003**: Digitação ou busca de CEP válido preenche automaticamente logradouro, bairro, cidade e UF através do `CepService`.
- **SC-004**: Dropdown de segmentos carrega e lista os segmentos da API permitindo selecionar `segmentoPrincipalId`.
- **SC-005**: 100% dos testes unitários do frontend passando no Vitest (`npm test -- --no-watch`).
- **SC-006**: Build de produção Angular (`npm run build`) concluído sem erros.
- **SC-007**: Backlog em [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md) atualizado ao final da implementação.
