# Research: US-006 — Criar enum OrdemServicoStatus

## Contexto

A feature altera o modelo `OrdemServico` no backend .NET/EF Core, trocando a propriedade textual `Status` por um enum tipado e mantendo a persistência compatível com o banco atual. O projeto já usa enumeração em outros domínios, como `ClienteStatus`, e a convenção de configuração EF Core é centralizada em `OficinaContext` + configurações por entidade em `OficinaMotos.Infrastructure/EntitiesConfiguration`.

## Decisão 1: Enum como valor de domínio explícito

- Decision: Criar `OficinaMotos.Domain/Enums/OrdemServicoStatus.cs` com os valores `Aberta = 1`, `EmAndamento = 2`, `AguardandoPeca = 3`, `Concluida = 4` e `Cancelada = 5`.
- Rationale: O enum centraliza o conjunto de estados válidos, elimina strings livres e reduz a chance de inconsistência entre aplicação e banco.
- Alternatives considered:
  - Manter `string Status`: rejeitada porque o requisito exige tipagem forte e padronização do domínio.
  - Usar `int` direto na entidade: rejeitada porque reduz legibilidade sem trazer ganho no modelo atual.

## Decisão 2: Persistência com conversão para string

- Decision: Configurar `builder.Property(e => e.Status).HasConversion<string>();` no mapeamento de `OrdemServico` (ou no `OnModelCreating` específico por entidade) para manter a coluna em texto no banco, compatível com o modelo existente em `os_ordens`.
- Rationale: O projeto já persiste alguns status em texto, e a conversão para string evita uma quebra de schema e acelera a adoção do enum sem mexer em outras partes do banco por engano.
- Alternatives considered:
  - Persistir como inteiro no banco: rejeitada porque o requisito e os padrões atuais do projeto indicam que a coluna atual é textual e a conversão para string reduz impacto migratório.
  - Alterar a coluna para `tinyint` sem conversão: rejeitada por ser uma mudança de contrato mais arriscada para dados já existentes.

## Decisão 3: Ajuste do modelo de domínio e DTOs

- Decision: Atualizar `OrdemServico.Status` para `OrdemServicoStatus` e refletir o mesmo tipo em `CreateOrdemServicoDTO` e `OrdemServicoResponseDTO` (e demais DTOs relevantes do pedido de OS).
- Rationale: Essa mudança precisa ser consistente em toda a stack: domínio, API de entrada/saída, serialização JSON e regras de validação.
- Alternatives considered:
  - Alterar só a entidade e deixar DTOs em `string`: rejeitada porque criaria divergência entre contrato e persistência.
  - Alterar DTOs sem atualizar entidade: rejeitada por invalidar a regra de negócio da feature.

## Decisão 4: Migração EF Core e compatibilidade de dados

- Decision: Criar a migration `AddOrdemServicoStatusEnum` com a alteração de tipo do campo `status` de string para enum convertido em string, preservando os valores atuais e gerando `Down` reversível.
- Rationale: O projeto segue padrão de migrações EF Core e exige rastreabilidade de schema em `OficinaMotos.Infrastructure/Migrations`.
- Alternatives considered:
  - Script SQL manual fora de migration: rejeitada por não seguir o padrão ativo do repositório e dificultar recorrência/rollback.
  - Alteração manual em snapshot sem migration: rejeitada porque não existe histórico versionado adequado.

## Decisão 5: Validação da feature

- Decision: Validar com build do backend, análise de migração e confirmação do mapeamento do enum em `OficinaContext`/configurações de entidade.
- Rationale: O repositório tem estrutura de Clean Architecture e migrações, mas não há suíte dedicada para essa mudança específica no escopo desta feature; a validação mínima precisa garantir a integração entre domínio e EF Core.
- Alternatives considered:
  - Aguardar apenas revisão de código sem execução: rejeitada por baixo grau de confiança.
  - Criar cobertura extensa de testes unitários para todos os fluxos: rejeitada pela estimativa S e pelo escopo da feature.

## Impactos e riscos conhecidos

- O status atual de ordens já armazenado pode aparecer em texto puro na base; a migration precisa converter valores válidos para os nomes do enum sem perda semântica.
- Qualquer endpoint ou UI que ainda enviar `string` para status deve ser ajustado para o enum, ou a serialização e conversão precisam aceitar os valores esperados.
- O nome do enum pode afetar serialização JSON caso a API use texto bruto; deve manter convenção compatível com o contrato do sistema.
