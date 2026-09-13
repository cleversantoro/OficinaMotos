# Pesquisa Técnica: Formulário de Cadastro de Veículo (Motocicleta)

## Decisão 1: Arquitetura de Componente e Formulário no Frontend

**Decisão**: Implementar `VeiculoCadastroComponent` como componente Standalone (`standalone: true`), utilizando Angular Reactive Forms (`FormBuilder`, `FormGroup`, `Validators`) combinado com Signals para estados reativos de UI (`loadingMarcas`, `loadingModelos`, `loadingClientes`, `submitting`, `marcas`, `modelosFiltrados`, `sugestoesClientes`).

**Racional**:
- Alinha-se à Constituição (Princípio IV: Frontend Reativo com Componentes Standalone) e ao padrão já estabelecido com sucesso em `ClienteCadastro` e `OsCadastroComponent`.
- Reactive Forms oferece controle síncrono e determinístico sobre o ciclo de vida de validação (`valid`, `invalid`, `dirty`, `touched`), essencial para validações de placas e vínculos obrigatórios.
- Signals provêm reatividade de alta performance sem zone overhead excessivo e com detecção `ChangeDetectionStrategy.OnPush`.

**Alternativas consideradas**:
- Template-driven forms (`[(ngModel)]` com `ngForm`): Rejeitado por ser menos robusto para validações complexas assíncronas/em cascata e dificultar testes unitários isolados.
- Modais em vez de página dedicada: Rejeitado porque a especificação do backlog define explicitamente a rota `/motos/novo` e o formulário possui diversos campos cadastrais detalhados (proprietário, identificação, chassi, KM, especificações).

---

## Decisão 2: Validador de Placas Veiculares (`placaValidator`)

**Decisão**: Criar o validador `placaValidator` em `src/app/shared/validators/placa-validator.ts` com exportação no barrel `src/app/shared/validators/index.ts`. O validador aceitará:
1. **Padrão Mercosul**: `^[A-Z]{3}[0-9][A-Z][0-9]{2}$` (ex.: `BRA2E19`, `ABC1D23`)
2. **Padrão Tradicional**: `^[A-Z]{3}-?[0-9]{4}$` (ex.: `ABC-1234` ou `ABC1234`)

O validador executará:
- Remoção de espaços nas extremidades e conversão para maiúsculas antes do teste de expressão regular;
- Retorno de `null` se o campo estiver vazio (deixando a obrigatoriedade a cargo de `Validators.required`);
- Retorno de `{ placa: { message: 'Placa inválida. Utilize o formato Mercosul (ABC1D23) ou tradicional (ABC-1234)' } }` se a string não casar com nenhum dos padrões.

**Racional**:
- Motocicletas no Brasil utilizam ambos os padrões em circulação (frotas anteriores a 2018 mantêm placas cinzas tradicionais, enquanto motos novas ou transferidas adotam Mercosul).
- A unificação dos dois formatos com suporte a hífen opcional garante conveniência para o operador e rigor de dados.

**Alternativas consideradas**:
- Validar apenas Mercosul: Rejeitado porque muitas motos atendidas em oficinas mecânicas ainda possuem placa cinza antiga (formato `ABC-1234`).
- Validação no backend apenas: Rejeitado porque o feedback imediato no frontend melhora a experiência do usuário e previne requisições HTTP desnecessárias.

---

## Decisão 3: Seleção em Cascata de Marca e Modelo

**Decisão**:
- No `ngOnInit`, carregar a lista de marcas através de `VeiculosService.marcas()`.
- O controle de formulário `modeloId` começa desabilitado (`{ value: null, disabled: true }`).
- Quando o usuário seleciona uma marca (`onMarcaChange(marcaId)`):
  - O controle `modeloId` é redefinido para `null`;
  - Se a lista completa de modelos ainda não estiver em cache local, consulta `VeiculosService.modelos()`;
  - Os modelos são filtrados para `m.marcaId === marcaId` e atribuídos ao signal `modelosFiltrados`;
  - O controle `modeloId` é habilitado caso haja modelos disponíveis;
  - Se a marca for limpa, o campo `modeloId` é desabilitado novamente.

**Racional**:
- Previne inconsistência referencial (ex.: selecionar marca "Honda" e modelo "Yamaha Fazer 250").
- A filtragem local de modelos (ou por query param) é instantânea (< 50ms) e proporciona ótima fluidez de uso.

---

## Decisão 4: Autocomplete Dinâmico do Cliente Proprietário

**Decisão**:
Replicar a solução robusta já implementada e testada em `OsCadastroComponent`:
- Input de busca com signal `clienteTermo` e contador sequencial de requisições (`clienteBuscaSequencia`) para prevenir race conditions decorrentes de respostas assíncronas fora de ordem;
- Disparo da busca em `ClientesService.search(termo)` a partir de 2 caracteres digitados;
- Exibição de dropdown customizado com nome, nome de exibição e documento (CPF/CNPJ);
- Ao clicar no cliente desejado, atribuir seu `id` ao controle reativo `clienteId`, fixar o nome no input e fechar o dropdown;
- Se o usuário apagar o campo de texto, resetar `clienteId` para `0` ou `null`.

**Racional**:
- Oficinas possuem centenas ou milhares de clientes. Um dropdown `<select>` estático com todos os clientes seria lento e inviável.
- O autocomplete permite localizar o cliente tanto pelo nome quanto pelo CPF em milissegundos.

---

## Decisão 5: Precedência de Rotas no Angular Router

**Decisão**:
No arquivo `src/app/app.routes.ts`, posicionar a rota de criação antes da rota de detalhe:
```typescript
{ path: 'motos', component: VeiculoLista },
{ path: 'motos/novo', component: VeiculoCadastroComponent },
{ path: 'motos/:id', component: VeiculoDetalhe },
```

**Racional**:
- O Angular Router utiliza estratégia *first-match* (primeiro casamento de padrão).
- Se `motos/:id` fosse declarado antes de `motos/novo`, qualquer navegação para `/motos/novo` faria o roteador interpretar `'novo'` como o parâmetro `:id` da rota de detalhe, causando erro de conversão numérica e falha de carregamento.

---

## Decisão 6: Ponto de Entrada de Navegação na Listagem

**Decisão**:
Atualizar o cabeçalho de `VeiculoLista` (`veiculo-lista.html` e `veiculo-lista.scss`) adicionando o botão de ação principal `+ Nova Moto` ao lado da barra de busca, com diretiva `routerLink="/motos/novo"`.

**Racional**:
- Permite que o operador acesse o formulário de cadastro com um único clique a partir da tela de veículos que ele já utiliza no dia a dia.

