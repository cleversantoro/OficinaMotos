# Data Model: US-006 — Criar enum OrdemServicoStatus

## 1. Entidade de domínio: OrdemServico

A entidade representa a ordem de serviço principal do contexto de operação da oficina.

### Campos relevantes

- `Id: long`
- `ClienteId: long`
- `MecanicoId: long`
- `DescricaoProblema: string`
- `Status: OrdemServicoStatus`
- `DataAbertura: DateTime`
- `DataConclusao: DateTime?`

### Regras de validação

- `Status` deve assumir apenas um dos valores válidos do enum.
- `Status` padrão para nova ordem deve ser `Aberta`.
- `DataAbertura` deve ser preenchida na abertura da ordem.
- `DataConclusao` deve ser preenchida somente quando a ordem deixa de estar em andamento.

### Transições de estado

1. `Aberta`
   - Estado inicial da ordem de serviço.
2. `EmAndamento`
   - Status usado durante a execução do atendimento.
3. `AguardandoPeca`
   - Status usado quando o reparo depende de material ou peça.
4. `Concluida`
   - Estado final do atendimento concluído.
5. `Cancelada`
   - Estado final da ordem interrompida ou cancelada.

## 2. Enum: OrdemServicoStatus

### Definição

```csharp
public enum OrdemServicoStatus
{
    Aberta = 1,
    EmAndamento = 2,
    AguardandoPeca = 3,
    Concluida = 4,
    Cancelada = 5
}
```

### Regras

- O valor numérico deve seguir a convenção do requisito de negócio.
- O enum deve ser usado como representação canônica do estado da ordem.
- O sistema deve rejeitar valores fora do enum nos fluxos de criação/atualização.

## 3. DTOs da aplicação

Os contratos de entrada e saída devem refletir o enum para manter consistência entre model e API.

### DTO de criação

- `CreateOrdemServicoDTO.Status: OrdemServicoStatus`
- Valor padrão para novo registro: `OrdemServicoStatus.Aberta`

### DTO de atualização

- `UpdateOrdemServicoDTO.Status: OrdemServicoStatus`
- Permite alteração entre os estados válidos do ciclo da ordem.

### DTO de resposta

- `OrdemServicoResponseDTO.Status: OrdemServicoStatus`
- Exposto de forma consistente para consumidores da API.

## 4. Mapeamento EF Core

### Regras de persistência

- O tipo da propriedade `OrdemServico.Status` passa a ser enum.
- A persistência em banco deve usar conversão para string para manter compatibilidade com a coluna `status` atual.
- O `OficinaContext` e a configuração de `OrdemServico` devem refletir a conversão do enum sem alterar a tabela de forma desnecessária.

### Compatibilidade

- Os valores persistidos continuam sendo representáveis por texto no banco.
- O modelo de domínio fica fortemente tipado, porém o armazenamento permanece compatível com a estrutura atual do sistema.

## 5. Migração

### Alteração esperada

- A migration `AddOrdemServicoStatusEnum` ajusta a coluna `status` para refletir o enum de forma controlada.
- Os valores já existentes, quando válidos, devem continuar preservados sem perda semântica.
- O `Down` deve reverter a alteração para o formato anterior, dentro do padrão adotado pelo projeto.
