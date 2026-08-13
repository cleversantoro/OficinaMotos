# Quickstart: US-007 — Adicionar VeiculoId à entidade OrdemServico

## Objetivo

Validar que a entidade `OrdemServico` passou a exigir referência ao veículo, que o contrato da API foi atualizado e que a migration de banco foi criada corretamente.

## Pré-requisitos

- .NET SDK 8 instalado
- projeto `oficina-motos-api` disponível localmente
- banco MySQL configurado para o ambiente do projeto
- `dotnet ef` disponível para gerar e aplicar migrações

## Cenários de validação

### 1. Build do backend

```bash
cd oficina-motos-api
 dotnet build
```

Resultado esperado:

- compilação sem erros
- `OrdemServico` expõe `VeiculoId` e `Veiculo`
- `CreateOrdemServicoDTO` exige a propriedade `VeiculoId`

### 2. Verificação de mapeamento EF Core

- confirmar em `OrdemServicoConfigurations.cs` a relação `HasOne(e => e.Veiculo)` com `HasForeignKey(e => e.VeiculoId)`
- confirmar que a propriedade `VeiculoId` usa `long`
- validar que `OnDelete` foi configurado para preservar o histórico da OS

Resultado esperado:

- relacionamento explícito entre `os_ordens` e `cad_veiculos`
- persistência com integridade referencial respeitada

### 3. Geração da migration

```bash
cd oficina-motos-api
 dotnet ef migrations add AddVeiculoIdToOrdemServico
```

Resultado esperado:

- arquivo de migration criado em `OficinaMotos.Infrastructure/Migrations`
- a migration adiciona `veiculo_id` na tabela `os_ordens`
- a migration contém `Down` para reversão do campo e da FK

### 4. Smoke test do fluxo de criação de OS

- enviar um payload com `ClienteId`, `MecanicoId`, `DescricaoProblema` e `VeiculoId` válido
- confirmar que a API aceita a criação
- enviar um payload sem `VeiculoId`
- confirmar que a API rejeita a operação com erro de validação

Resultado esperado:

- ordens de serviço só são criadas com veículo informado
- payload inválido não consegue persistir a operação
