# Research: US-007 — Adicionar VeiculoId à entidade OrdemServico

## Contexto

A feature altera o modelo `OrdemServico` no backend .NET/EF Core para incluir a referência obrigatória ao veículo vinculado à OS. O projeto já possui a entidade `Veiculo` em `cad_veiculos` e a configuração de relacionamentos de domínio fica centralizada em `OficinaMotos.Infrastructure/EntitiesConfiguration` por entidade, com o `OficinaContext` aplicando todas as configurações via `ApplyConfigurationsFromAssembly`.

## Decisão 1: adicionar `VeiculoId` e navegação `Veiculo` à entidade

- Decision: Incluir na entidade `OrdemServico` as propriedades `long VeiculoId` e `Veiculo? Veiculo`.
- Rationale: a relação precisa ser explícita e rastreável no modelo de domínio, permitindo consulta ao veículo associado à OS e mantendo o relacionamento de negócio claro.
- Alternatives considered:
  - manter somente `long VeiculoId` sem navegação: rejeitada porque a modelagem do dominio e o padrão do projeto usam navegação para relacionamento principal.
  - usar um identificador diferente do padrão do domínio: rejeitada porque o projeto atual usa identificadores numéricos e a convenção de domínio já é `long` para as entidades principais.

## Decisão 2: configurar a FK em `OrdemServicoConfiguration`

- Decision: configurar no `OnModelCreating`/configuração da entidade `OrdemServico` a relação `HasOne(e => e.Veiculo).WithMany().HasForeignKey(e => e.VeiculoId).OnDelete(DeleteBehavior.Restrict);`.
- Rationale: a regra de negócio exige integração forte com o veículo, mas também preserva histórico e evita que a exclusão de veículo derrube ordens de serviço vinculadas.
- Alternatives considered:
  - `DeleteBehavior.Cascade`: rejeitada porque apaga o histórico do atendimento junto com o registro do veículo.
  - `DeleteBehavior.SetNull`: rejeitada porque o requisito exige que a ordem tenha veículo obrigatório e a referência não deve ficar nula em fluxo principal.

## Decisão 3: validar o contrato de criação com `Required`

- Decision: atualizar `CreateOrdemServicoDTO` para expor `long VeiculoId` com `[Required]` e garantir validação no request de criação.
- Rationale: isso impede a criação de ordens inválidas e alinha o modelo de entrada com a regra de negócio da relação com veículo.
- Alternatives considered:
  - manter `VeiculoId` opcional: rejeitada porque contraria o critério de aceite e abre dados inconsistentes.
  - validar apenas no serviço sem DTO: rejeitada porque a API precisa rejeitar payloads inválidos antes da persistência.

## Decisão 4: migração reversível e compatível

- Decision: gerar a migration `AddVeiculoIdToOrdemServico` adicionando a coluna `veiculo_id` com FK para `cad_veiculos` e manter `Down` que remove a coluna e o vínculo da tabela `os_ordens`.
- Rationale: a migração precisa ser aplicável em ambientes existentes sem destruir dados históricos e deve permitir rollback em homologação ou produção com controle.
- Alternatives considered:
  - atualizar manualmente o schema sem migration: rejeitada por não seguir o padrão do repositório e por não deixar histórico versionado.
  - usar apenas alteração de código sem migration: rejeitada porque o critério de aceite e o padrão do projeto exigem migration formal.

## Decisão 5: validação da feature

- Decision: verificar compilação do backend, confirmar a configuração do relacionamento no EF Core e validar a geração da migration com nome `AddVeiculoIdToOrdemServico`.
- Rationale: esta feature é de infraestrutura de domínio e persistência; a validação precisa cobrir o contrato de entrada, a modelagem e a fundação de dados.
- Alternatives considered:
  - revisar apenas código: rejeitada por não demonstrar que o relacionamento está corretamente refletido no banco.
  - criar testes extensivos de front-end: rejeitada pela estimativa S e pelo escopo de backend da feature.

## Impactos e riscos conhecidos

- Há modelos antigos de OS que podem não ter nenhuma referência de veículo; a migration precisa ser segura e permitir operação futura em base já populada.
- O relacionamento com `Veiculo` deve ser tratado como obrigatório em criação, mas sem quebrar o uso de ordens antigas em ambiente de transição.
- Qualquer consumidor da API que envie `veiculoId` sem valor válido precisa receber erro de validação explícita.
