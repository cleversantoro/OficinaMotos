# Guia de Validação: Página de Detalhe Real da Ordem de Serviço

## Pré-requisitos

- Node.js v24+ e npm instalados.
- Dependências instaladas no frontend: `cd c:/Projetos/OficinaMotos/oficina-motos-web && npm install`.
- Sessão de usuário autenticado no sistema com perfil de acesso às ordens de serviço.

## Verificação de Build e Testes Automatizados

Executar a partir da pasta do frontend:

```powershell
Set-Location C:\Projetos\OficinaMotos\oficina-motos-web
npm run build
npm test -- --watch=false
```

Resultado esperado: Build compila com sucesso e testes unitários de `OsDetalheComponent` passam sem erros.

## Roteiro de Validação Manual

### Cenário 1: Visualização Completa da Ordem de Serviço
1. Acessar a aplicação e efetuar login.
2. Navegar para a listagem em `/ordens`.
3. Clicar no ícone de "olho" (visualizar) de uma ordem existente ou acessar diretamente `/ordens/1`.
4. **Verificar**:
   - Card "Dados Gerais" exibe número da OS, cliente com nome e contato, veículo com placa e modelo, mecânico e descrição.
   - Status atual visível com badge colorido condizente.
   - Seção "Itens da OS" exibe a tabela com peças/serviços e valor total somado.
   - Seção "Observações" exibe anotações da equipe.
   - Seção "Pagamentos" exibe pagamentos e cálculo do saldo restante.

### Cenário 2: Controle e Transição de Status
1. Com uma OS no status `Aberta`:
   - Verificar que o seletor de transição oferece somente: `Em Andamento` e `Cancelada`.
2. Selecionar `Em Andamento` e clicar em "Alterar Status":
   - Verificar feedback visual de envio (botão desabilitado durante requisição).
   - Verificar Toast de confirmação de alteração.
   - Verificar que o badge foi atualizado para `Em Andamento` e novas transições tornam-se disponíveis (`Aguardando Peça`, `Concluída`, `Cancelada`).
3. Com uma OS no status `Concluída` ou `Cancelada`:
   - Verificar que o controle de transição não permite novas seleções (estado terminal).

### Cenário 3: Exibição de Estados Vazios
1. Acessar uma OS recém-criada (sem itens cadastrados, sem observações e sem pagamentos).
2. **Verificar**:
   - Seção de itens informa "Nenhum item adicionado a esta OS".
   - Seção de observações informa "Nenhuma observação registrada para esta OS".
   - Seção de pagamentos informa "Nenhum pagamento registrado para esta OS".
   - O layout permanece harmônico e sem erros no console.

### Cenário 4: Identificador Inválido ou Inexistente
1. Acessar na URL: `/ordens/invalido` ou `/ordens/-5`.
2. **Verificar**: Mensagem clara de "Identificador de ordem inválido" e ausência de chamadas à API.
3. Acessar `/ordens/9999999` (ID inexistente que retorna 404 da API):
4. **Verificar**: Mensagem amigável de erro com botão ou link visível para retornar à listagem (`/ordens`).

### Cenário 5: Responsividade
1. Reduzir a viewport para largura mobile (ex.: 375px ou 414px).
2. **Verificar**: Cards empilham verticalmente sem cortes; tabelas possuem rolagem horizontal suave e botões mantêm toque confortável.
