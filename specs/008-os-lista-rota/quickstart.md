# Guia de Validação: Lista de Ordens e Rota Corrigida

## Pré-requisitos

- Node.js compatível com o projeto.
- npm 10.9.2 ou versão compatível.
- Dependências instaladas em `oficina-motos-web`.
- API disponível no endereço configurado pelo `proxy.conf.json`.
- Usuário autenticado com permissão de consulta de ordens.
- Pelo menos uma ordem de serviço para validar a ação por linha.

## Preparação

```powershell
Set-Location C:\Projetos\OficinaMotos\oficina-motos-web
npm install
```

## Validação estática e compilação

```powershell
npm run build
```

Resultado esperado: o Angular compila sem erros de TypeScript ou template, incluindo `OsListaComponent` e as rotas de ordens.

## Testes unitários

```powershell
npm test -- --watch=false
```

Resultado esperado: os testes do frontend passam, incluindo os cenários do componente de lista e das rotas.

## Cenário 1: Abrir a lista

1. Iniciar o frontend com `npm run start:proxy`.
2. Autenticar no sistema.
3. Navegar para `/ordens`.
4. Confirmar que o componente de lista é exibido.
5. Confirmar que o carregamento termina e que a tabela ou estado vazio aparece.

Resultado esperado: a lista usa o componente `OsListaComponent`, apresenta o cabeçalho e não abre a antiga tela identificada como `OsDetalhe`.

## Cenário 2: Paginar

1. Garantir mais registros do que o limite inicial da tabela.
2. Avançar uma página.
3. Retornar à página anterior.

Resultado esperado: o paginator altera os registros exibidos, mantém a rota `/ordens` e informa o intervalo/página atual.

## Cenário 3: Abrir uma ordem

1. Na tabela, localizar uma ordem com identificador válido.
2. Selecionar a ação de visualização da mesma linha.

Resultado esperado: a aplicação navega para `/ordens/:id`, usando o identificador daquela ordem.

## Cenário 4: Criar uma ordem

1. Em `/ordens`, selecionar o botão `Nova OS`.

Resultado esperado: a aplicação navega para `/ordens/novo`.

## Cenário 5: Validar autorização

1. Autenticar com uma sessão que contenha a permissão `ordens × criar`.
2. Acessar `/ordens` e confirmar que `Nova OS` está visível.
3. Autenticar com uma sessão sem a permissão `ordens × criar`.
4. Acessar `/ordens` e confirmar que `Nova OS` não inicia o fluxo de criação.
5. Tentar acessar `/ordens/novo` diretamente com a sessão sem a permissão.

Resultado esperado: o acesso de criação é bloqueado pelo guard, o sistema exibe `Você não tem permissão para acessar esta área.` e redireciona para `/dashboard`, independentemente da navegação usada.

## Cenário 6: Estados de erro e vazio

1. Executar a tela sem ordens.
2. Interromper ou simular uma falha da consulta.

Resultado esperado: o estado vazio e o estado de erro são distinguíveis; nenhum deles apresenta dados desatualizados como se fossem válidos.

## Referências

- Contrato da interface: [contracts/ui.md](contracts/ui.md)
- Modelo da tela: [data-model.md](data-model.md)
- Requisitos: [spec.md](spec.md)
