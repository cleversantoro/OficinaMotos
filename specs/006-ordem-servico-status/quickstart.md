# Quickstart: US-006 — Criar enum OrdemServicoStatus

## Objetivo

Validar que a conversão de `string` para enum de status de ordem de serviço foi aplicada corretamente no domínio, nos DTOs da API e na migração do banco.

## Pré-requisitos

- .NET SDK 8 instalado
- Projeto `oficina-motos-api` disponível localmente
- Banco MySQL configurado para o ambiente do projeto
- Ferramenta `dotnet ef` disponível na solução ou via `dotnet tool` do projeto

## Cenários de validação

### 1. Build do backend

```bash
cd oficina-motos-api
 dotnet build
```

Resultado esperado:

- Compilação sem erros
- Entidades e DTOs reconhecem `OrdemServicoStatus`
- O enum é acessível via `OficinaMotos.Domain.Enums`

### 2. Verificação do mapeamento EF Core

- Confirmar em `OficinaContext` / configuração da entidade `OrdemServico` que a propriedade `Status` usa o enum e a conversão para string.
- Verificar que `builder.Property(e => e.Status).HasConversion<string>();` foi aplicada.

Resultado esperado:

- A entidade persiste corretamente no banco sem quebrar a coluna `status`.
- Os valores do enum são convertidos para texto em persistência.

### 3. Aplicação da migração

```bash
cd oficina-motos-api
 dotnet ef database update
```

Resultado esperado:

- Migration `AddOrdemServicoStatusEnum` aplicada com sucesso.
- O banco mantém o comportamento esperado de conversão entre enum e string.

### 4. Smoke test do fluxo de OS

- Criar uma ordem de serviço com status `Aberta`.
- Atualizar para `EmAndamento`.
- Marcar como `AguardandoPeca`.
- Finalizar como `Concluida`.
- Cancelar como `Cancelada`.

Resultado esperado:

- Todos os estados devem ser aceitos conforme o enum.
- Status fora do enum devem ser rejeitados.

## Evidências esperadas

- `OrdemServico.Status` não é mais `string`.
- DTOs de criação/atualização/resposta refletem `OrdemServicoStatus`.
- A migration foi adicionada ao projeto de infraestrutura.
- A API continua consumindo os dados da ordem de serviço de forma consistente.
