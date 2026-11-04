# Pesquisa Técnica: Movimentações de Entrada e Saída de Estoque (US-019)

**Branch**: `019-movimentacoes-estoque` | **Data**: 2026-11-04 | **Spec**: [spec.md](spec.md)

## 1. Análise dos Endpoints e Contratos do Backend (.NET 10)

### 1.1 `POST /api/v1/EstoqueMovimentacoes` e `GET /api/v1/EstoqueMovimentacoes`
- **Controller**: `EstoqueMovimentacoesController` (`oficina-motos-api/src/OficinaMotos.API/Controllers/V1/Estoque/EstoqueMovimentacoesController.cs`).
- **DTO de Criação (`CreateEstoqueMovimentacaoDTO`)**:
  ```json
  {
    "pecaId": 10,
    "quantidade": 5,
    "tipo": "ENTRADA",
    "referencia": "NF 12345",
    "usuario": "Administrador"
  }
  ```
- **DTO de Resposta (`EstoqueMovimentacaoResponseDTO`)**:
  ```json
  {
    "id": 1,
    "pecaId": 10,
    "quantidade": 5,
    "tipo": "ENTRADA",
    "referencia": "NF 12345",
    "dataMovimentacao": "2026-11-04T22:50:00Z",
    "usuario": "Administrador"
  }
  ```
- **Observação de Sincronização de Saldo**:
  - `EstoqueMovimentacaoService.CreateAsync` insere o registro na tabela `est_movimentacoes`.
  - Para manter o saldo de `EstoquePeca` sincronizado imediatamente na UI e no banco sem alterar migrations, após `EstoqueService.movimentar(payload)` (`POST /api/v1/EstoqueMovimentacoes`), o frontend encadeia via `switchMap` a chamada `EstoqueService.atualizarPeca(peca.id, updatedPecaPayload)` (`PUT /api/v1/EstoquePecas/:id`) com `quantidade: novaQuantidade` (`peca.quantidade + quantidade` para `ENTRADA`, `peca.quantidade - quantidade` para `SAIDA`).

## 2. Análise do Serviço Frontend (`EstoqueService`)

Em [`estoque.service.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/core/services/estoque.service.ts):
- Já existem:
  - `getPeca(id: string | number)` -> `GET /api/v1/EstoquePecas/:id`
  - `atualizarPeca(id: string | number, body: any)` -> `PUT /api/v1/EstoquePecas/:id`
  - `movimentar(body: any)` -> `POST /api/v1/EstoqueMovimentacoes`
  - `categorias()`, `fabricantes()`, `localizacoes()`, `fornecedores(params)`
- Necessário adicionar:
  - `listarMovimentacoes(params?: Record<string, any>)` -> `this.api.list(apiPaths.estoque.movimentacoes, params)` (`GET /api/v1/EstoqueMovimentacoes`).

## 3. Componentes e Rotas

1. **`EstoqueEntradaModalComponent` (`T-019.1`)**:
   - Localização: `src/app/features/estoque/components/estoque-entrada-modal/estoque-entrada-modal.ts` (`.html`, `.scss`, `.spec.ts`).
   - Baseado em `<p-dialog>` do PrimeNG (seguindo o padrão de [`OsItemServicoModalComponent`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/ordens-servico/components/os-item-servico-modal/os-item-servico-modal.ts)).
   - Recebe `@Input() visible = false`, `@Input({ required: true }) peca!: EstoquePeca`, emite `@Output() visibleChange = new EventEmitter<boolean>()` e `@Output() saved = new EventEmitter<EstoqueMovimentacao>()`.
   - Reactive Form (`FormBuilder.nonNullable.group`):
     - `quantidade`: `[1, [Validators.required, Validators.min(1)]]`
     - `referencia`: `['', [Validators.required, Validators.maxLength(200)]]`

2. **`EstoqueSaidaModalComponent` (`T-019.2`)**:
   - Localização: `src/app/features/estoque/components/estoque-saida-modal/estoque-saida-modal.ts` (`.html`, `.scss`, `.spec.ts`).
   - Baseado em `<p-dialog>` do PrimeNG.
   - Recebe `@Input() visible = false`, `@Input({ required: true }) peca!: EstoquePeca`, emite `@Output() visibleChange = new EventEmitter<boolean>()` e `@Output() saved = new EventEmitter<EstoqueMovimentacao>()`.
   - Reactive Form (`FormBuilder.nonNullable.group`) com validação de saldo máximo (`quantidade <= peca.quantidade`).

3. **`EstoquePecaDetalheComponent` (`T-019.4`)**:
   - Localização: `src/app/features/estoque/pages/estoque-peca-detalhe/estoque-peca-detalhe.ts` (`.html`, `.scss`, `.spec.ts`).
   - Carrega via `forkJoin`: `getPeca(id)`, `listarMovimentacoes({ pecaId: id })`, `categorias()`, `fabricantes()`, `localizacoes()`.
   - Filtra `movimentacoes` por `m.pecaId === id` (garantindo consistência mesmo se o backend retornar todos os registros) e ordena por `dataMovimentacao` decrescente.
   - Exibe cabeçalho com dados da peça, cards de indicadores de saldo (Quantidade Atual com badge `Crítico` se `quantidade < estoqueMinimo`, Estoque Mínimo, Estoque Máximo, Preço Unitário), botões **"Registrar Entrada"** e **"Registrar Saída"**, e tabela de histórico de movimentações.

4. **Rota `/estoque/:id` e Navegação (`T-019.5`)**:
   - Em [`app.routes.ts`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/app.routes.ts), registrar `{ path: 'estoque/:id', component: EstoquePecaDetalheComponent }` logo após `{ path: 'estoque/novo', component: EstoquePecaCadastroComponent }`.
   - Em [`estoque-lista.html`](file:///c:/Projetos/OficinaMotos/oficina-motos-web/src/app/features/estoque/pages/estoque-lista/estoque-lista.html), adicionar `[routerLink]="['/estoque', item.id]"` no nome da peça e no botão de ação "Visualizar".

