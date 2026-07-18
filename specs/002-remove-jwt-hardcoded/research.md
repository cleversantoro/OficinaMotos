# Pesquisa Tecnica - US-002

## Decisao 1: Remover fallback hardcoded do segredo JWT

- Decision: Eliminar valor padrao hardcoded na leitura da chave JWT em Program.cs.
- Rationale: Segredo embutido em codigo versionado viola principio de seguranca por design e permite execucao insegura.
- Alternatives considered:
  - Manter fallback apenas para desenvolvimento: continua expondo risco de propagacao para ambientes indevidos.
  - Fallback com segredo de baixa entropia: ainda e segredo hardcoded e nao elimina problema raiz.

## Decisao 2: Aplicar fail-fast no startup quando chave JWT ausente/vazia

- Decision: Validar chave JWT no bootstrap e lancar InvalidOperationException com mensagem clara quando ausente, vazia ou whitespace.
- Rationale: Falhar cedo impede API subir em estado inseguro e facilita diagnostico operacional imediato.
- Alternatives considered:
  - Apenas logar warning e seguir: permite ambiente inseguro em producao.
  - Deferir erro para primeira requisicao autenticada: feedback tardio e falha operacional dificil de rastrear.

## Decisao 3: Manter placeholder vazio em configuracao versionada

- Decision: Definir campo de segredo JWT no appsettings versionado como string vazia.
- Rationale: Mantem estrutura de configuracao esperada sem expor segredo real no repositorio.
- Alternatives considered:
  - Remover completamente a secao Jwt: pode quebrar expectativa de configuracao e onboarding.
  - Manter exemplo de segredo fake nao vazio: incentiva copia insegura e mascara ausencia de configuracao real.

## Decisao 4: Documentar configuracao de ambiente no README

- Decision: Incluir instrucao explicita de configuracao da chave JWT por variavel de ambiente ou provider externo.
- Rationale: Reduz erro de setup e evita reintroducao de segredo no appsettings.
- Alternatives considered:
  - Documentar apenas no spec da feature: insuficiente para onboarding operacional do repositorio.
  - Depender de conhecimento tacito da equipe: alto risco de configuracao incorreta.
