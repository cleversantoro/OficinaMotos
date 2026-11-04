# Pesquisa Técnica: Formulário de Cadastro de Fornecedor

**Feature**: `017-cadastro-fornecedor` | **Data**: 2026-11-04
**Referências de Governança**:
- [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)
- [`governance/backlog.md`](file:///c:/Projetos/OficinaMotos/governance/backlog.md)
- [`governance/decisoes.md`](file:///c:/Projetos/OficinaMotos/governance/decisoes.md)
- [`governance/inventario.md`](file:///c:/Projetos/OficinaMotos/governance/inventario.md)

---

## 1. Contexto e Motivação

No módulo de Fornecedores (`features/fornecedores`), já existem as páginas `FornecedorLista` (`/fornecedores`) e `FornecedorDetalhe` (`/fornecedores/:id`). Na tela de listagem (`fornecedor-lista.html`), o botão **"+ Novo fornecedor"** já possui `routerLink="/fornecedores/novo"`, porém a rota e o componente de cadastro ainda não existem.

A história **US-017** entrega:
- **T-017.1**: Criar `FornecedorCadastroComponent` (`fornecedor-cadastro.ts`, `.html`, `.scss`).
- **T-017.2**: Registrar rota `/fornecedores/novo` em `app.routes.ts` antes de `/fornecedores/:id`.
- **T-017.3**: Validator de CNPJ (`cnpjValidator`) e integração com `CepService` (`ViaCEP`).
- **T-017.4**: Dropdown dinâmico de segmentos (`FornecedoresService.segmentos()`).

---

## 2. Análise de Serviços e Validadores Reutilizáveis

### 2.1 Validador de CNPJ (`src/app/shared/validators/document-validator.ts`)
- O validador `cnpjValidator()` já está implementado e exportado no barrel `src/app/shared/validators/index.ts`.
- Ele remove caracteres não numéricos (`replace(/\D/g, '')`), valida 14 dígitos, rejeita dígitos repetidos e valida os dois dígitos verificadores pelo algoritmo oficial da Receita Federal.
- Adicionaremos formatação visual em tempo real (`00.000.000/0000-00`) no evento de input do campo `documento`.

### 2.2 Serviço de CEP (`src/app/shared/services/cep.ts`)
- O serviço `CepService` (`providedIn: 'root'`) já está implementado em `src/app/shared/services/cep.ts`.
- O método `buscarCep(cep: string): Observable<Endereco>` limpa a máscara, consulta `https://viacep.com.br/ws/{cep}/json/` com timeout e retry, e retorna o objeto `Endereco` contendo `{ cep, logradouro, complemento, bairro, cidade, estado, uf, ibge, ddd }`.
- No `FornecedorCadastroComponent`, ao digitar 8 dígitos no campo `cep` ou clicar no botão "Buscar CEP", invocaremos `cepService.buscarCep(cep)` para preencher automaticamente `logradouro`, `bairro`, `cidade`, `estado` (`uf`) e `complemento`.

### 2.3 `FornecedoresService` (`src/app/core/services/fornecedores.service.ts`)
- Já possui:
  - `list()` -> `GET /api/v1/Fornecedores`
  - `create(body)` -> `POST /api/v1/Fornecedores`
  - `segmentos()` -> `GET /api/v1/FornecedorSegmentos`
  - `vincularSegmento(body)` -> `POST /api/v1/FornecedorSegmentosRel`
- Adicionaremos o método `criarEndereco(body: any)` apontando para `this.api.create(apiPaths.fornecedores.enderecos, body)` (`POST /api/v1/FornecedorEnderecos`) para persistir o endereço principal informado no formulário.

---

## 3. Análise de Backend e Banco de Dados (MySQL 8.0)

- **Endpoints existentes**:
  - `POST /api/v1/Fornecedores` (`CreateFornecedorDTO`)
  - `GET /api/v1/FornecedorSegmentos` (`FornecedorSegmentoResponseDTO`)
  - `POST /api/v1/FornecedorSegmentosRel` (`CreateFornecedorSegmentoRelDTO`)
  - `POST /api/v1/FornecedorEnderecos` (`CreateFornecedorEnderecoDTO`)
- **Mudanças de schema**: Nenhuma alteração de banco de dados (MySQL 8.0) é necessária.

