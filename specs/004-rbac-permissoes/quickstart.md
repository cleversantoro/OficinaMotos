# Quickstart - RBAC por permissões em controllers de negócio

## Objetivo

Validar que usuários com papel insuficiente não conseguem executar ações destrutivas e não veem botões de ação que não lhes pertencem.

## Pré-requisitos

- API e frontend disponíveis localmente.
- Um usuário autenticado com papel de alto privilégio para comparação.
- Um usuário autenticado com papel restrito, como Consulta.

## Cenário 1: backend bloqueia operação destrutiva

1. Autentique-se com um usuário de papel restrito.
2. Escolha um recurso de negócio com ação de atualização ou exclusão.
3. Tente executar a operação diretamente pela API.
4. Resultado esperado: a resposta deve ser 403 para o papel sem permissão.

## Cenário 2: frontend oculta a ação

1. Autentique-se com um usuário de papel restrito.
2. Acesse uma tela que exiba ações destrutivas em um registro.
3. Verifique os botões disponíveis na linha ou no formulário.
4. Resultado esperado: as ações destrutivas não aparecem para o papel sem permissão.

## Cenário 3: papel autorizado mantém o fluxo

1. Autentique-se com um usuário autorizado para editar ou excluir.
2. Acesse a mesma tela do cenário anterior.
3. Execute a ação permitida.
4. Resultado esperado: a ação fica disponível e segue normalmente.

## Validação técnica recomendada

- Conferir que a proteção global de autenticação continua ativa.
- Conferir que os controllers de negócio sensíveis não expõem operações destrutivas sem restrição por papel.
- Conferir que a UI usa a regra de visibilidade para esconder botões destrutivos quando o papel não autoriza a ação.

## Critério de aceite prático

A feature está validada quando o mesmo papel é tratado da mesma forma no backend e no frontend: acesso insuficiente resulta em 403 e ausência de botões destrutivos, enquanto papéis autorizados continuam executando as mesmas ações.
