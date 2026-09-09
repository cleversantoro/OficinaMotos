# Contrato de Interface: Página de Detalhe Real da Ordem de Serviço

## Identificação do Componente

- **Componente**: `OsDetalheComponent`
- **Rota**: `/ordens/:id`
- **Modo**: Angular Standalone Component (`ChangeDetectionStrategy.OnPush`)
- **Proteção**: `authGuard` + `ordensPermissionGuard` com ação `visualizar`
- **Parâmetro de Rota**: `:id` (inteiro positivo obrigatório)

## Layout e Seções Obrigatórias

### 1. Cabeçalho e Navegação
- Botão "Voltar para ordens" direcionando para `/ordens`;
- Título principal com identificador da OS (`Ordem de Serviço #<ID>`);
- Tag/Badge com o status atual formatado com severidade visual correspondente.

### 2. Card "Dados Gerais"
- **Cliente**: Exibição do nome/razão social e contato telefônico; link para detalhes do cliente se aplicável.
- **Veículo**: Placa em destaque, modelo, marca e cor; link para detalhes do veículo se aplicável.
- **Mecânico Responsável**: Nome do mecânico atribuído.
- **Datas**: Data de abertura (`dd/MM/yyyy HH:mm`) e data de conclusão (se preenchida).
- **Descrição do Problema**: Caixa de texto com relato detalhado do cliente ou diagnóstico inicial.

### 3. Controle de Status (Transição Válida)
- Dropdown de transições exibindo apenas opções autorizadas para o status vigente:
  - Se `Aberta`: opções `Em Andamento` e `Cancelada`;
  - Se `Em Andamento`: opções `Aguardando Peça`, `Concluída` e `Cancelada`;
  - Se `Aguardando Peça`: opções `Em Andamento` e `Cancelada`;
  - Se `Concluída` ou `Cancelada`: dropdown desabilitado ou substituído por mensagem de estado final.
- Botão "Alterar Status":
  - Desabilitado enquanto nenhum novo status for selecionado ou durante submissão (`submittingStatus`);
  - Ao clicar, submete requisição `PUT /api/v1/OrdemServicos/{id}`.

### 4. Seção "Itens da OS"
- Tabela com colunas:
  - `Descrição`: nome da peça ou descrição do serviço;
  - `Qtd`: quantidade com alinhamento numérico;
  - `Valor Unitário`: formatado em Real (`R$ 0,00`);
  - `Total`: quantidade × valor unitário (`R$ 0,00`).
- Linha de total geral consolidando o valor de todos os itens da OS.
- Estado vazio: alerta amigável quando `itens` for nulo ou lista vazia.

### 5. Seção "Observações"
- Listagem cronológica de apontamentos internos.
- Cada item exibe o texto da observação, autor e carimbo de data quando existentes.
- Estado vazio: mensagem informativa quando não houver observações registradas.

### 6. Seção "Pagamentos"
- Tabela de recebimentos com colunas:
  - `Valor`: valor recebido (`R$ 0,00`);
  - `Método`: tipo de pagamento (ex.: PIX, Cartão, Dinheiro);
  - `Status`: situação do pagamento;
  - `Data`: data da transação (`dd/MM/yyyy`).
- Resumo financeiro:
  - Total da OS (itens)
  - Total pago
  - Saldo a pagar / pendente
- Estado vazio: mensagem informativa quando não houver pagamentos registrados.

## Integração de Serviços

| Ação | Serviço / Método | Endpoint |
| --- | --- | --- |
| Buscar OS | `OrdensService.get(id)` | `GET /api/v1/OrdemServicos/{id}` |
| Atualizar Status | `OrdensService.update(id, payload)` | `PUT /api/v1/OrdemServicos/{id}` |
| Buscar Cliente | `ClientesService.get(clienteId)` | `GET /api/v1/Clientes/{id}` |
| Buscar Veículo | `VeiculosService.get(veiculoId)` | `GET /api/v1/Veiculos/{id}` |
| Buscar Mecânico | `MecanicosService.get(mecanicoId)` | `GET /api/v1/Mecanicos/{id}` |

## Payload de Atualização de Status

```json
{
  "clienteId": 1,
  "mecanicoId": 1,
  "veiculoId": 1,
  "descricaoProblema": "Descrição da OS",
  "status": "EmAndamento",
  "dataAbertura": "2026-09-08T10:00:00Z",
  "dataConclusao": null
}
```
