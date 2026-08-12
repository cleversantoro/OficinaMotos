# Contract: OrdemServicoStatus

## Objetivo

Definir o contrato de status para a entidade `OrdemServico` e garantir consistência entre domínio, banco e API.

## Escopo

- Entidade `OrdemServico`
- DTOs de criação, atualização e resposta
- Persistência por EF Core
- Migração `AddOrdemServicoStatusEnum`

## Regras contratuais

### 1. Enum canônico

O status da ordem de serviço deve ser representado pelo enum `OrdemServicoStatus`.

Valores permitidos:

- `Aberta = 1`
- `EmAndamento = 2`
- `AguardandoPeca = 3`
- `Concluida = 4`
- `Cancelada = 5`

### 2. Persistência

- A propriedade `Status` da entidade deve ser do tipo `OrdemServicoStatus`.
- O valor deve ser convertido para string no banco para manter compatibilidade com a estrutura atual.
- Não devem ser persistidos valores fora da enumeração.

### 3. API

- DTOs de entrada/saída devem refletir o enum correspondente.
- Respostas JSON devem exibir o valor do enum como texto, e não como número.
- A API deve aceitar e devolver os seguintes valores de status: `"Aberta"`, `"EmAndamento"`, `"AguardandoPeca"`, `"Concluida"` e `"Cancelada"`.
- Requisições com status inválido devem falhar antes de persistência.

### 4. Migração

- A migration `AddOrdemServicoStatusEnum` deve registrar a alteração de schema do status da ordem de serviço.
- A migration deve manter o histórico de dados e permitir rollback reversível.

## Cenários de verificação

1. Dado uma operação de criação de ordem, quando o status vier válido, então o registro deve ser aceito.
2. Dado um status fora do enum, quando a API processa o payload, então a requisição deve falhar.
3. Dado uma ordem existente, quando o status é consultado, então o valor retornado deve refletir a enumeração documentada.
4. Dado um ambiente com migração aplicada, quando a base for acessada, então a coluna deve continuar representando o estado da ordem sem perda semântica.

## Artefatos relacionados

- [../spec.md](../spec.md)
- [../data-model.md](../data-model.md)
- [../quickstart.md](../quickstart.md)
