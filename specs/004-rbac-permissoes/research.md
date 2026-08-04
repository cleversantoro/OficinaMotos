# Research - RBAC por permissões em controllers de negócio

## 1. Fonte canônica de perfis e permissões

- Decision: Usar a constituição do projeto e a documentação de segurança do domínio como fonte de verdade para perfis, ações e módulos.
- Rationale: A feature é transversal e precisa manter consistência com os 6 perfis e as ações `visualizar`, `criar`, `editar`, `excluir`, `exportar` e `aprovar` já publicados.
- Alternatives considered: Definir uma matriz nova localmente na feature. Rejeitado porque criaria divergência com a documentação canônica.

## 2. Forma de aplicação no backend

- Decision: Aplicar autorização por papel nos controllers de negócio que expõem operações destrutivas, preservando o comportamento de 403 para acesso insuficiente.
- Rationale: O backend é a última linha de defesa; a UI pode esconder ações, mas não pode ser a única barreira.
- Alternatives considered: Restringir apenas a interface. Rejeitado porque não impede chamadas diretas à API.

## 3. Forma de aplicação no frontend

- Decision: O frontend deve ocultar botões e ações destrutivas com base no papel do usuário autenticado e nas regras de acesso da feature.
- Rationale: A interface precisa refletir o que o usuário realmente pode executar para reduzir erro operacional e tentativa de ações inválidas.
- Alternatives considered: Manter botões visíveis e apenas desabilitá-los. Rejeitado porque o critério de aceite pede ocultação.

## 4. Situação atual do código

- Decision: Reaproveitar a base já existente de autenticação JWT no backend e a guarda de autenticação global no frontend, adicionando a camada de autorização por papel em cima dela.
- Rationale: O projeto já possui autenticação consolidada e rotas protegidas; a nova necessidade é granularidade de autorização.
- Alternatives considered: Introduzir um mecanismo de segurança paralelo. Rejeitado por duplicar responsabilidades e aumentar risco de inconsistência.

## 5. Contrato de UI para ações por linha

- Decision: Usar o padrão já existente de visibilidade por ação nas tabelas para controlar botões destrutivos por papel.
- Rationale: O componente de tabela já suporta regras de visibilidade e desabilitação, o que reduz o impacto da feature na UI.
- Alternatives considered: Criar um novo componente de tabela específico para RBAC. Rejeitado por duplicação desnecessária.
