# Especificação de Funcionalidade: Formulário de Cadastro de Veículo (Motocicleta)

**Feature Branch**: `013-cadastro-veiculo`

**Criado**: 2026-09-13

**Status**: Draft

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

**Dependências**: US-001 (Autenticação/Login Base), Cadastro de Clientes (`ClientesService`), API de Veículos (`VeiculosController`, `VeiculoMarcasController`, `VeiculoModelosController`).

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Cadastro Completo de Motocicleta com Vínculo Obrigatório ao Proprietário (Prioridade: P1)

Como recepcionista, atendente ou mecânico da oficina, quero acessar a rota `/motos/novo` para cadastrar uma nova motocicleta vinculada a um cliente já registrado no sistema, informando placa, marca, modelo, ano, cor, chassi e quilometragem atual, para que a moto fique disponível no histórico do cliente e possa receber Ordens de Serviço imediatamente.

**Por que esta prioridade**: O cadastro de veículos é um pré-requisito fundamental para a operação da oficina mecânica. Sem o veículo registrado e vinculado ao cliente proprietário, não é possível emitir Ordens de Serviço nem rastrear histórico de manutenção.

**Teste independente**: Acessar `/motos/novo`, selecionar um cliente existente via autocomplete, preencher uma placa válida (ex.: `BRA2E19`), selecionar marca e modelo em cascata, preencher ano, cor, chassi e KM, submeter o formulário e verificar que a moto é persistida no backend (`POST /api/v1/Veiculos`), uma notificação Toast de sucesso é exibida e o usuário é redirecionado para os detalhes da moto (`/motos/:id`) ou para a listagem (`/motos`).

**Cenários de aceitação**:

1. **Dado** que o usuário está autenticado e acessa a URL `/motos/novo`, **quando** a página carregar, **então** o formulário de cadastro de veículo deve ser exibido com os campos: Proprietário (Cliente), Placa, Marca, Modelo, Ano Fabricação, Ano Modelo, Cor, Chassi, KM, Combustível e Observações.
2. **Dado** que o usuário preenche todos os campos obrigatórios válidos e vincula um cliente proprietário, **quando** clicar em "Salvar Veículo", **então** o sistema deve disparar `POST /api/v1/Veiculos` com o payload correto (`clienteId`, `placa`, `modeloId`, `anoFab`, `anoMod`, `cor`, `chassi`, `km`, etc.).
3. **Dado** que a API responde com sucesso (HTTP 201 Created com objeto do veículo), **quando** o registro for confirmado, **então** deve ser exibido um Toast de sucesso informando que o veículo de placa informada foi cadastrado e a aplicação deve navegar para a rota de detalhe `/motos/:id` (ou `/motos`).
4. **Dado** que o formulário está com campos obrigatórios pendentes ou inválidos, **quando** o usuário tentar salvar, **então** o botão deve permanecer desabilitado ou o formulário deve exibir mensagens de validação destacando os campos pendentes sem realizar requisição HTTP.

---

### História de Usuário 2 — Validação Estrita de Placa Veicular (Padrão Mercosul e Tradicional) (Prioridade: P1)

Como operador do sistema da oficina, quero que o campo de placa valide automaticamente o formato brasileiro (tanto o padrão Mercosul `ABC1D23` quanto o padrão tradicional `ABC-1234`), convertendo automaticamente os caracteres para caixa alta e alertando em tempo real se a placa for inválida, para evitar duplicidades, erros de digitação e registros inconsistentes na base de dados.

**Por que esta prioridade**: A placa é o identificador visual primário da motocicleta na oficina. Registros com placas fora do padrão impedem buscas rápidas, quebram integrações de consulta veicular e geram registros duplicados.

**Teste independente**: No formulário de cadastro, inserir uma placa inválida (ex.: `1234ABC` ou `AB12345`) e verificar que o validador sinaliza erro; em seguida, digitar `abc1d23` e verificar que o campo converte para `ABC1D23`, é aceito como válido e a mensagem de erro desaparece. Repetir com placa cinza tradicional `abc-1234` e verificar validação positiva.

**Cenários de aceitação**:

1. **Dado** o campo de entrada "Placa", **quando** o usuário digita letras minúsculas, **então** o sistema deve normalizar automaticamente o valor para caixa alta (`toUpperCase`).
2. **Dado** uma placa digitada no padrão Mercosul (3 letras, 1 dígito, 1 letra, 2 dígitos - ex.: `BRA2E19`), **quando** o campo for validado, **então** a validação deve passar sem erros.
3. **Dado** uma placa digitada no padrão tradicional brasileiro (3 letras, hífen opcional, 4 dígitos - ex.: `ABC-1234` ou `ABC1234`), **quando** o campo for validado, **então** a validação deve passar sem erros.
4. **Dado** uma sequência de caracteres que não obedece a nenhum dos dois padrões (ex.: `AAAA111`, `123-ABCD`, menos de 7 caracteres ou mais de 8 caracteres), **quando** o campo perder o foco ou sofrer alteração, **então** deve ser exibida a mensagem de erro "Placa inválida. Utilize o formato Mercosul (ABC1D23) ou tradicional (ABC-1234)".
5. **Dado** que o validador customizado `placaValidator()` reside em `src/app/shared/validators/placa-validator.ts`, **quando** for invocado em testes unitários automatizados, **então** deve cobrir exaustivamente casos válidos e inválidos de ambos os padrões.

---

### História de Usuário 3 — Seleção em Cascata de Marca e Modelo de Motocicletas (Prioridade: P1)

Como atendente da oficina cadastrando uma moto, quero selecionar a marca da motocicleta (ex.: Honda, Yamaha, BMW, Kawasaki) e ver o campo de modelo ser filtrado dinamicamente para exibir apenas os modelos correspondentes àquela marca, para agilizar o preenchimento e evitar inconsistências como cadastrar uma "Fazer 250" sob a marca "Honda".

**Por que esta prioridade**: Previne inconsistências graves no catálogo de veículos da oficina. Garantir a integridade relacional entre Marca e Modelo facilita a compra de peças e o diagnóstico mecânico correto.

**Teste independente**: Abrir o formulário de cadastro; verificar que o campo de Modelo está inicialmente desabilitado com placeholder "Selecione primeiro a marca"; selecionar a marca "Yamaha"; verificar que o dropdown/autocomplete de Modelo é habilitado e lista apenas modelos Yamaha (ex.: Fazer FZ25, MT-03, Lander 250); alterar a marca para "Honda" e constatar que a seleção de modelo anterior é limpa e os novos modelos exibidos passam a ser exclusivamente Honda (ex.: CG 160, CB 500F, XRE 300).

**Cenários de aceitação**:

1. **Dado** o carregamento inicial do formulário `/motos/novo`, **quando** o usuário ainda não tiver selecionado uma marca, **então** o campo de seleção de modelo deve permanecer desabilitado informando a necessidade de escolha prévia da marca.
2. **Dado** que as marcas foram carregadas via `VeiculosService.marcas()`, **quando** o usuário seleciona uma marca específica, **então** os modelos dessa marca devem ser disponibilizados para seleção no campo de modelo.
3. **Dado** que um modelo já estava selecionado (ex.: "Lander 250" da Yamaha), **quando** o usuário trocar a marca para "Honda", **então** o campo de modelo deve ser resetado para vazio/nulo e a nova lista de modelos da Honda deve ser carregada.
4. **Dado** que a marca selecionada não possua modelos cadastrados no banco de dados, **quando** o campo de modelos for consultado, **então** deve exibir aviso amigável informando "Nenhum modelo cadastrado para esta marca".

---

### História de Usuário 4 — Autocomplete Dinâmico do Cliente Proprietário (Prioridade: P1)

Como operador da oficina, quero buscar o cliente proprietário do veículo por digitação de nome, nome de exibição ou documento (CPF/CNPJ) através de um autocomplete reativo, para localizar e vincular rapidamente o dono da moto sem precisar sair da tela ou memorizar o ID numérico do cliente.

**Por que esta prioridade**: Um veículo não pode ser registrado solto no sistema; deve obrigatoriamente pertencer a um cliente existente (`clienteId`). A busca rápida por nome/documento reduz tempo de atendimento no balcão da oficina.

**Teste independente**: No campo "Proprietário (Cliente)", digitar as primeiras 3 letras de um cliente (ex.: "Sil"); verificar exibição do dropdown com sugestões encontradas na API (`ClientesService.search`); clicar no cliente desejado; verificar que o nome é exibido no campo e o ID numérico do cliente é atribuído internamente ao controle `clienteId` do formulário reativo.

**Cenários de aceitação**:

1. **Dado** o campo "Proprietário", **quando** o usuário digita ao menos 2 caracteres, **então** o sistema deve disparar busca via `ClientesService.search(termo)` com tratamento de concorrência e debounce.
2. **Dado** que a busca retorna clientes correspondentes, **quando** as sugestões forem renderizadas, **então** cada item deve mostrar o nome do cliente e seu documento principal (CPF/CNPJ) formatado.
3. **Dado** que o usuário clica em uma das opções sugeridas, **quando** a seleção for feita, **então** o campo deve fixar o nome do cliente selecionado, armazenar o `clienteId` correspondente no formulário e fechar a lista de sugestões.
4. **Dado** que o usuário limpa ou apaga o texto do cliente após ter selecionado, **quando** o campo for esvaziado, **então** o `clienteId` deve voltar para valor nulo/inválido, impedindo a submissão até nova seleção válida.
5. **Dado** que nenhum cliente foi encontrado para o termo pesquisado, **quando** a API retornar lista vazia, **então** o sistema deve exibir a mensagem "Nenhum cliente encontrado com esse termo. Cadastre o cliente antes de registrar a moto".

---

### História de Usuário 5 — Validação de Metadados da Motocicleta e Prevenção de Erros (Prioridade: P2)

Como gestor da oficina mecânica, quero que o formulário valide anos de fabricação e modelo (valores numéricos razoáveis entre 1950 e ano corrente + 1), quilometragem (não negativa) e chassi (17 caracteres padrão VIN ou nulo), para manter a higienização cadastral dos veículos atendidos na oficina.

**Por que esta prioridade**: Previne cadastros com anos absurdos (ex.: ano 2099 ou ano 12), quilometragens negativas ou preenchimentos errôneos de chassi.

**Teste independente**: Preencher o ano de fabricação com `1800` ou ano superior ao próximo ano e verificar bloqueio; preencher KM com `-500` e verificar mensagem de validação "Quilometragem não pode ser negativa".

**Cenários de aceitação**:

1. **Dado** o campo "Ano de Fabricação" e "Ano do Modelo", **quando** informados valores numéricos, **então** devem ser números inteiros de 4 dígitos entre 1950 e o ano corrente + 1.
2. **Dado** o campo "Quilometragem (KM)", **quando** informado valor, **então** deve ser um número inteiro maior ou igual a zero.
3. **Dado** o campo "Chassi", **quando** preenchido, **então** deve aceitar até 17 caracteres alfanuméricos em caixa alta.
4. **Dado** que ocorra falha de rede ou erro na API (ex.: placa já existente retornando erro 400/409), **quando** a submissão falhar, **então** uma mensagem de erro compreensível deve ser apresentada ao usuário no Toast ou alerta em tela, mantendo os dados digitados intactos para correção.

---

## Casos de Borda e Tratamento de Exceções

1. **Tentativa de acesso direto à rota `/motos/novo`**: A rota deve estar protegida pelo `authGuard`, redirecionando usuários não autenticados para a tela de login.
2. **Precedência de Rotas no Angular Router**: No arquivo `app.routes.ts`, a rota `{ path: 'motos/novo', component: VeiculoCadastroComponent }` **deve obrigatoriamente** ser declarada **antes** da rota parametrizada `{ path: 'motos/:id', component: VeiculoDetalhe }`, caso contrário a rota parametrizada capturará a palavra `'novo'` como se fosse um `id`.
3. **Placa Duplicada**: Caso o backend retorne erro de violação de unicidade da placa, o formulário deve exibir aviso claro: "Já existe uma moto cadastrada com esta placa".
4. **Conexão Lenta na Busca de Clientes**: As requisições de autocomplete devem cancelar buscas anteriores obsoletas ou ignorar respostas fora de ordem para evitar sobrescrever a seleção atual do usuário.
5. **Navegação de Retorno**: Deve haver um botão "Voltar" ou "Cancelar" que retorne à listagem `/motos` sem submeter dados.

---

## Requisitos Funcionais (FR)

- **FR-001**: O componente `VeiculoCadastroComponent` deve ser criado como componente Standalone na pasta `src/app/features/motos/pages/veiculo-cadastro/`.
- **FR-002**: A rota `/motos/novo` deve ser registrada em `app.routes.ts` com proteção de autenticação (`authGuard`), posicionada antes de `/motos/:id`.
- **FR-003**: O validador customizado `placaValidator` deve ser criado em `src/app/shared/validators/placa-validator.ts` e exportado através do barrel `src/app/shared/validators/index.ts`.
- **FR-004**: O validador de placa deve validar tanto o padrão Mercosul (`^[A-Z]{3}[0-9][A-Z][0-9]{2}$`) quanto o padrão tradicional brasileiro (`^[A-Z]{3}-?[0-9]{4}$`), ignorando case na digitação e aceitando placas com ou sem hífen.
- **FR-005**: O campo de placa deve converter automaticamente a digitação para caixa alta e remover espaços extras.
- **FR-006**: O campo de cliente/proprietário é obrigatório e deve ser selecionado exclusivamente a partir de clientes existentes via autocomplete conectado a `ClientesService.search()`.
- **FR-007**: As marcas de veículos devem ser carregadas a partir de `VeiculosService.marcas()`.
- **FR-008**: O campo de modelo deve ser dependente da marca: fica desabilitado até a escolha de uma marca e filtra apenas os modelos vinculados ao `marcaId` escolhido.
- **FR-009**: Ao alterar a marca selecionada, o modelo previamente selecionado deve ser limpo automaticamente.
- **FR-010**: O formulário deve conter os campos opcionais: `anoFab`, `anoMod`, `cor`, `chassi`, `km`, `combustivel` (Gasolina, Etanol, Flex, Elétrica), `observacao`.
- **FR-011**: O formulário deve validar que `km` seja $\ge 0$ quando preenchido.
- **FR-012**: O formulário reativo deve controlar o estado de envio (`submitting`), desabilitando o botão de confirmação durante a requisição para evitar submissões duplicadas.
- **FR-013**: Ao salvar com sucesso via `VeiculosService.create()`, exibir notificação Toast de sucesso e redirecionar o usuário para `/motos` ou `/motos/:id`.
- **FR-014**: Adicionar botão de ação "Novo Veículo" ou "+ Nova Moto" no cabeçalho da listagem `VeiculoLista` apontando para `/motos/novo`.
- **FR-015**: A interface visual deve seguir estritamente o design system e paleta de cores dark mode da aplicação já estabelecidos em `cliente-cadastro` e `os-novo`.

---

## Requisitos Não Funcionais (NFR)

- **NFR-001**: O formulário deve ser construído utilizando Angular Reactive Forms (`FormBuilder`, `FormGroup`, `FormControl`, `Validators`).
- **NFR-002**: Utilizar a estratégia de detecção de mudanças `ChangeDetectionStrategy.OnPush` com Signals para variáveis de estado visual (`loading`, `submitting`, `marcas`, `modelosFiltrados`, `sugestoesClientes`).
- **NFR-003**: Compatibilidade total com testes unitários em Vitest (`veiculo-cadastro.spec.ts` e `placa-validator.spec.ts`).
- **NFR-004**: Responsividade em dispositivos móveis e desktops, com grid flexível adaptável.

---

## Critérios de Sucesso Mensuráveis (SC)

- **SC-001**: 100% de cobertura nos testes unitários do validador `placaValidator` para placas Mercosul válidas, placas antigas válidas e formatos inválidos.
- **SC-002**: Testes unitários de `VeiculoCadastroComponent` cobrindo renderização, validação de campos, cascata marca/modelo, busca de cliente e submissão bem-sucedida.
- **SC-003**: Execução de `npx ng test --no-watch` com 100% de aprovação na suíte de testes do frontend.
- **SC-004**: Execução de `npm run build` completada com 0 erros de compilação TypeScript/SCSS.
- **SC-005**: Tempo de resposta percebido da cascata marca -> modelos inferior a 200ms após a seleção da marca.
- **SC-006**: Redirecionamento e feedback visual concluídos sem nenhum recarregamento completo de página.
