# Contratos de Interface: Formulário de Cadastro de Veículo

## 1. Rota e Página de Cadastro

### 1.1 `VeiculoCadastroComponent`
- **Seletor**: `app-veiculo-cadastro`
- **Localização**: `src/app/features/motos/pages/veiculo-cadastro/`
- **Standalone**: `true`
- **Rota**: `/motos/novo` (protegida por `authGuard`, declarada antes de `/motos/:id`)
- **Estratégia de Detecção**: `ChangeDetectionStrategy.OnPush`
- **Estrutura de Estado (Reactive Forms + Signals)**:
  - Form: `formBuilder.group(...)`
  - Signals:
    - `loadingMarcas = signal(false)`
    - `loadingModelos = signal(false)`
    - `loadingClientes = signal(false)`
    - `submitting = signal(false)`
    - `errorMessage = signal<string | null>(null)`
    - `marcas = signal<VeiculoMarca[]>([])`
    - `modelosFiltrados = signal<VeiculoModelo[]>([])`
    - `sugestoesClientes = signal<ClienteOpcao[]>([])`
    - `clienteSelecionadoNome = signal<string>('')`

---

## 2. Campos do Formulário e Validações

| Campo | Controle | Tipo / Componente | Obrigatório | Validações |
| :--- | :--- | :--- | :--- | :--- |
| **Proprietário** | `clienteId` | Autocomplete / Input de busca com dropdown | **Sim** | `Validators.required`, `Validators.min(1)` |
| **Placa** | `placa` | Input Text (auto uppercase) | **Sim** | `Validators.required`, `placaValidator()` |
| **Marca** | `marcaId` | Select / Dropdown | **Sim** | `Validators.required`, `Validators.min(1)` |
| **Modelo** | `modeloId` | Select / Dropdown (dependente da marca) | **Sim** | `Validators.required`, `Validators.min(1)` |
| **Ano Fabricação** | `anoFab` | Input Number | Não | `Validators.min(1950)`, `Validators.max(anoAtual + 1)` |
| **Ano Modelo** | `anoMod` | Input Number | Não | `Validators.min(1950)`, `Validators.max(anoAtual + 2)` |
| **Cor** | `cor` | Input Text | Não | Máx. 40 caracteres |
| **Chassi** | `chassi` | Input Text (auto uppercase) | Não | Máx. 17 caracteres |
| **KM Atual** | `km` | Input Number | Não | `Validators.min(0)` |
| **Combustível** | `combustivel` | Select (Gasolina, Etanol, Flex, Elétrica) | Não | Valores válidos |
| **Observações** | `observacao` | Textarea | Não | Máx. 500 caracteres |
| **Veículo Principal** | `principal` | Checkbox / Toggle | Não | Booleano (default: `false`) |

---

## 3. Validador de Placa (`placaValidator`)

- **Arquivo**: `src/app/shared/validators/placa-validator.ts`
- **Assinatura**: `export function placaValidator(): ValidatorFn`
- **Padrões Aceitos**:
  1. **Mercosul**: `^[A-Z]{3}[0-9][A-Z][0-9]{2}$` (ex: `ABC1D23`, `BRA2E19`)
  2. **Tradicional Brasileiro**: `^[A-Z]{3}-?[0-9]{4}$` (ex: `ABC-1234`, `ABC1234`)
- **Erro emitido**: `{ placa: { message: 'Placa inválida. Utilize o formato Mercosul (ABC1D23) ou tradicional (ABC-1234)' } }`

---

## 4. Integração na Listagem (`VeiculoLista`)

- **Cabeçalho**:
  - Inserção do botão `+ Nova Moto` ou `Novo Veículo` no `header-actions` de `veiculo-lista.html`.
  - Navegação para `/motos/novo`.

