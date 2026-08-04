# Data Model - RBAC por permissões em controllers de negócio

## Entidades de domínio e de apoio

### Perfil de Acesso

Representa o papel atribuído ao usuário autenticado.

Campos relevantes:

- Id
- Nome
- Nivel
- Ativo

Regras:

- O nível define precedência de acesso no sistema.
- Os perfis canônicos são Administrador, Gerente, Recepcionista, Financeiro, Mecânico e Consulta.

### Permissao

Representa uma ação permitida em um módulo específico.

Campos relevantes:

- Id
- Modulo
- Acao
- Ativo

Regras:

- As ações canônicas são `visualizar`, `criar`, `editar`, `excluir`, `exportar` e `aprovar`.
- A permissão precisa ser interpretável como combinação módulo × ação.

### Matriz de Permissoes por Perfil

Relaciona quais perfis podem executar quais permissões.

Campos relevantes:

- PerfilId
- PermissaoId
- Permitido

Regras:

- Deve existir uma correspondência explícita para decisões de acesso sensíveis.
- A feature destrutiva só pode ser liberada quando a matriz autorizar o perfil para a ação correspondente.

### Ação Sensível

Representa uma operação com efeito modificador ou destrutivo na interface ou na API.

Campos relevantes:

- Nome da ação
- Tipo da ação
- Contexto do módulo
- Requer autorização explícita

Regras:

- Operações de alteração e exclusão entram nesta categoria.
- A visibilidade no frontend e a autorização no backend devem consultar a mesma regra base.

## Relacionamentos

- Um Perfil de Acesso pode estar vinculado a várias Permissões.
- Uma Permissao pode pertencer a um módulo e a uma ação específicos.
- A Matriz de Permissoes por Perfil é a camada de decisão usada para autorizar ou ocultar Ações Sensíveis.

## Estados e validações

- Permitido: o perfil pode executar a ação.
- Negado: o perfil não pode executar a ação e o sistema deve retornar 403 na API e ocultar a ação na UI.
- Indeterminado: não deve ser usado em produção; a regra precisa ser explícita para cada ação sensível tratada pela feature.

## Observações de consistência

- Este modelo reutiliza a taxonomia de perfis e ações já publicada em [CONSTITUTION.md](../../oficina-motos-docs/markdown/CONSTITUTION.md) e em [SEGURANCA_USUARIOS.md](../../oficina-motos-docs/markdown/SEGURANCA_USUARIOS.md).
- A feature não cria novos perfis; apenas aplica as regras existentes aos controllers e às ações destrutivas da interface.
