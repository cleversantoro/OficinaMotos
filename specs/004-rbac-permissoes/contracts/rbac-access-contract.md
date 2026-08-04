# Contract - RBAC de Acesso para Ações Destrutivas

## Propósito

Definir o comportamento esperado entre a API e o frontend quando uma ação destrutiva depende de papel/permissão.

## Fonte de verdade

- [CONSTITUTION.md](../../oficina-motos-docs/markdown/CONSTITUTION.md)
- [SEGURANCA_USUARIOS.md](../../oficina-motos-docs/markdown/SEGURANCA_USUARIOS.md)
- [spec.md](../spec.md)

## Regras contratuais

### Backend

- Operações destrutivas em áreas de negócio protegidas devem negar acesso quando o papel autenticado não estiver autorizado.
- O retorno esperado para acesso insuficiente é 403.
- Operações permitidas por papel devem continuar funcionando sem alteração de contrato funcional.

### Frontend

- A interface deve ocultar botões e ações destrutivas quando o papel autenticado não tiver permissão para executá-las.
- A visibilidade da ação deve ser derivada do papel atual, não apenas do estado visual anterior da tela.
- A UI não deve depender de bloqueio visual como única forma de segurança.

## Perfis e ações relevantes

- Perfis canônicos: Administrador, Gerente, Recepcionista, Financeiro, Mecânico e Consulta.
- Ações relevantes para esta feature: `editar` e `excluir`.

## Exemplos de comportamento esperado

- Consulta tentando editar ou excluir: ação oculta na UI e 403 na API.
- Mecânico tentando excluir fora do escopo autorizado: ação oculta ou negada conforme a regra do módulo, com 403 na API.
- Administrador executando ação destrutiva: ação visível e permitida.

## Critérios de conformidade

- Nenhuma tela deve expor botões destrutivos para papéis sem autorização.
- Nenhum controller de negócio deve aceitar uma operação destrutiva sem validar o papel.
- A regra aplicada na UI e na API deve derivar da mesma matriz canônica de acesso.
