# Data Model: US-007 — Adicionar VeiculoId à entidade OrdemServico

## 1. Entidade de domínio: OrdemServico

A entidade representa a ordem de serviço principal do contexto operacional da oficina e agora passa a possuir referência explícita ao veículo atendido.

### Campos relevantes

- `Id: long`
- `ClienteId: long`
- `Cliente: Cliente?`
- `MecanicoId: long`
- `Mecanico: Mecanico?`
- `VeiculoId: long`
- `Veiculo: Veiculo?`
- `DescricaoProblema: string`
- `Status: OrdemServicoStatus`
- `DataAbertura: DateTime`
- `DataConclusao: DateTime?`

### Regras de validação

- `VeiculoId` deve ser obrigatório na criação da ordem.
- `VeiculoId` deve apontar para um veículo existente em `cad_veiculos`.
- `Veiculo` deve ser navegável no modelo de domínio para leitura do veículo vinculado.
- a ordem deve manter o vínculo com o veículo mesmo após consultas e atualizações do atendimento.

## 2. Entidade de domínio: Veiculo

A entidade `Veiculo` permanece a referência canônica do cadastro do veículo.

### Campos relevantes do Veiculo

- `Id: long`
- `ClienteId: long`
- `Placa: string`
- `ModeloId: long?`
- `AnoFab: int?`
- `AnoMod: int?`
- `Cor: string?`
- `Chassi: string?`

### Relacionamento

- um veículo pode estar associado a várias ordens de serviço
- uma ordem de serviço deve apontar para um único veículo válido

## 3. DTO de criação

### `CreateOrdemServicoDTO`

- `ClienteId: long`
- `MecanicoId: long`
- `VeiculoId: long` (obrigatório)
- `DescricaoProblema: string`
- `Status: OrdemServicoStatus` (opcional com valor padrão `Aberta`)
- `DataAbertura: DateTime?`
- `DataConclusao: DateTime?`

### Regras de entrada

- `VeiculoId` deve ser obrigatório e ser considerado válido pela API.
- payloads sem `VeiculoId` devem ser rejeitados por validação.
- a criação da ordem deve refletir o vínculo do veículo no objeto de domínio e no banco.

## 4. Mapeamento EF Core

### Relacionamento esperado

- `OrdemServico` possui FK para `Veiculo` via `VeiculoId`.
- o mapeamento usa a configuração de entidade em `OrdemServicoConfigurations.cs` e não uma FK implícita dispersa pelo código.
- a relação precisa ser configurada com comportamento explícito em exclusão para preservar o histórico.

### Regras de persistência

- a coluna `veiculo_id` deve existir em `os_ordens`
- o banco deve garantir a integridade referencial com `cad_veiculos`
- a migração deve incluir o índice e a FK de forma reversível

## 5. Migração

### Alteração esperada

- migration: `AddVeiculoIdToOrdemServico`
- adiciona a coluna `veiculo_id` na tabela `os_ordens`
- cria a chave estrangeira para `cad_veiculos`
- preserva registros existentes sem alterar a estrutura das demais tabelas
- `Down` remove a coluna e a FK de forma controlada

### Compatibilidade

- o projeto continua com `os_ordens` e `cad_veiculos` como entidades principais do domínio da oficina
- ordens existentes sem veículo estão sujeitas a validação no ciclo de criação; a migração não deve preencher dados arbitrários sem origem válida
