# Guia de Validação: Formulário de Nova Ordem de Serviço

## Pré-requisitos

- Node.js e npm compatíveis com `oficina-motos-web`.
- Dependências instaladas.
- API disponível pelo `proxy.conf.json`.
- Sessão autenticada com `ordens × criar`.
- Pelo menos um cliente com veículo e um mecânico disponíveis.

## Preparação

```powershell
Set-Location C:\Projetos\OficinaMotos\oficina-motos-web
npm install
npm run build
```

Resultado esperado: compilação sem erros e rota lazy de criação incluída no bundle.

## Testes unitários

```powershell
npm test -- --watch=false
```

Resultado esperado: testes do formulário, serviços, guard e navegação passam. Se a suíte global estiver bloqueada por testes legados fora desta feature, registrar o erro completo sem mascará-lo.

## Cenário 1: Acesso autorizado

1. Autenticar com uma sessão contendo `ordens × criar`.
2. Abrir `/ordens/novo`.
3. Confirmar que `OsCadastroComponent` e os quatro campos obrigatórios são exibidos.

Resultado esperado: o formulário aparece sem redirecionamento.

## Cenário 2: Acesso negado

1. Autenticar com uma sessão sem `ordens × criar`.
2. Abrir `/ordens/novo` diretamente.

Resultado esperado: exibir `Você não tem permissão para acessar esta área.` e redirecionar para `/dashboard`.

## Cenário 3: Cliente, veículo e mecânico

1. Informar um termo no autocomplete de cliente.
2. Selecionar um cliente.
3. Confirmar que os veículos carregados pertencem somente ao cliente selecionado.
4. Trocar o cliente e confirmar que o veículo anterior foi limpo.
5. Selecionar um veículo e um mecânico.

Resultado esperado: todos os campos mantêm relacionamentos válidos e o envio só fica possível com dados obrigatórios completos.

## Cenário 4: Validação e submissão

1. Tentar enviar o formulário vazio.
2. Confirmar mensagens de validação e ausência de chamada de criação.
3. Preencher cliente, veículo, descrição e mecânico.
4. Enviar duas vezes rapidamente.

Resultado esperado: o envio vazio é bloqueado; com dados válidos, apenas uma chamada é realizada e o botão indica processamento.

## Cenário 5: Sucesso e erro

1. Simular criação bem-sucedida com `id` positivo.
2. Confirmar navegação para `/ordens/:id`.
3. Repetir simulando erro da API.

Resultado esperado: no sucesso, navegar para o ID criado; no erro, permanecer no formulário com mensagem compreensível.

## Referências

- Especificação: [spec.md](spec.md)
- Contrato de UI: [contracts/ui.md](contracts/ui.md)
- Modelo: [data-model.md](data-model.md)
