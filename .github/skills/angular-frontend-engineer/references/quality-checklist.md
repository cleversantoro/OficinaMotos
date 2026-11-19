# Referência — Checklist de Qualidade Frontend

## Código e arquitetura
- [ ] Segue a estrutura e convenções existentes.
- [ ] Responsabilidades estão separadas.
- [ ] Tipos são explícitos e seguros.
- [ ] Não há `any` injustificado.
- [ ] Não há lógica complexa em templates.
- [ ] Não há duplicação evitável.
- [ ] Não há dependências ou abstrações desnecessárias.

## Angular
- [ ] Preferência pelo **Angular Material** (`@angular/material`) avaliada e **confirmada previamente com o usuário** antes de criar ou alterar páginas ou componentes.
- [ ] Componentes standalone conforme versão.
- [ ] Controle de fluxo nativo quando apropriado.
- [ ] `track` estável em listas.
- [ ] Signals/RxJS usados conforme o tipo de estado.
- [ ] Inscrições têm ciclo de vida seguro.
- [ ] Rotas lazy-loaded quando adequado.
- [ ] Formulários tipados e validados.

## UX e acessibilidade
- [ ] Estados loading, erro, vazio e sucesso.
- [ ] Labels e mensagens associadas.
- [ ] Teclado e foco visível.
- [ ] Contraste e nomes acessíveis.
- [ ] Confirmação para ações destrutivas.
- [ ] Feedback para ações do usuário.

## Responsividade
- [ ] 360 px.
- [ ] 390 px.
- [ ] 768 px.
- [ ] 1024 px.
- [ ] 1366 px.
- [ ] 1440 px.
- [ ] Sem overflow horizontal acidental.
- [ ] Tabelas, menus, formulários e modais adaptados.

## Segurança e performance
- [ ] Dados externos tratados como não confiáveis.
- [ ] Sem segredos no bundle.
- [ ] Sem bypass de sanitização injustificado.
- [ ] Imagens otimizadas.
- [ ] Rotas e conteúdo pesado carregados sob demanda quando adequado.
- [ ] Sem trabalho custoso repetido no template.

## Verificação
- [ ] Build executado.
- [ ] Lint executado, se configurado.
- [ ] Testes relevantes executados.
- [ ] Resultado e limitações reportados.
