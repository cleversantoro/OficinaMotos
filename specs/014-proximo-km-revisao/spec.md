# Especificação de Funcionalidade: Campo Próxima Revisão (KM) do Veículo

**Feature Branch**: `014-proximo-km-revisao`

**Criado**: 2026-09-13

**Status**: Draft

**Entrada**: US-014 — Adicionar campo `proximo_km_revisao` ao veículo
- **Prioridade**: 🟡 Should | **Estimativa**: S | **Sprint**: 3 | **Depende**: US-013
- **Tasks**:
  - [ ] T-014.1 — Adicionar `int? ProximoKmRevisao` em `Veiculo.cs`
  - [ ] T-014.2 — Criar migration `AddProximoKmRevisaoToVeiculo`
  - [ ] T-014.3 — Incluir campo no formulário

**Dependências**: US-013 (Formulário de Cadastro de Veículo), Entidade de Domínio `Veiculo`, Entity Framework Core (`OficinaContext`), DTOs de Veículo, Componente `VeiculoCadastroComponent` e página de detalhes `VeiculoDetalhe`.

---

## Cenários de Usuário e Testes *(obrigatório)*

### História de Usuário 1 — Registro da Próxima Revisão por Quilometragem no Cadastro do Veículo (Prioridade: P1)

Como atendente, mecânico ou recepcionista da oficina, quero informar a quilometragem prevista para a próxima revisão preventiva da moto (`proximo_km_revisao`) durante o cadastro ou edição do veículo, para que a oficina possa monitorar prazos de manutenção, alertar preventivamente os proprietários e oferecer serviços recorrentes.

**Por que esta prioridade**: A manutenção preventiva baseada em odômetro/quilometragem é o principal gerador de receita recorrente e retenção de clientes em oficinas de motocicletas. Sem esse campo persistido e exposto, a oficina perde a visibilidade sobre quando cada moto precisa retornar.

**Teste independente**: Acessar o formulário de cadastro de veículos em `/motos/novo`, preencher os dados obrigatórios do veículo e definir a "Próxima Revisão (KM)" como `15000` (com KM atual de `10000`). Salvar o formulário e verificar que a requisição `POST /api/v1/Veiculos` envia o campo `proximoKmRevisao: 15000`, a entidade é gravada no banco de dados na coluna `proximo_km_revisao`, e a informação é retornada no `VeiculoResponseDTO`.

**Cenários de aceitação**:

1. **Dado** que o usuário está na tela de cadastro de veículo (`/motos/novo`), **quando** a seção "Especificações Técnicas" for visualizada, **então** deve existir o campo numérico opcional "Próxima Revisão (KM)" (`proximoKmRevisao`).
2. **Dado** que o usuário preenche o valor `15000` no campo de próxima revisão e submete o formulário válido, **quando** a requisição for processada pela API, **então** o valor `15000` deve ser persistido na coluna `proximo_km_revisao` da tabela `cad_veiculos`.
3. **Dado** que o usuário deixa o campo "Próxima Revisão (KM)" em branco (vazio), **quando** o formulário for salvo, **então** o sistema deve enviar `null` e a coluna no banco deve aceitar o valor nulo sem erros.
4. **Dado** que o usuário insere um valor negativo (ex.: `-500`), **quando** o campo sofrer alteração ou perda de foco, **então** o formulário deve acusar erro de validação impedindo o envio com a mensagem "A quilometragem da próxima revisão não pode ser negativa".
5. **Dado** que o veículo já foi salvo com o valor de próxima revisão, **quando** o usuário abrir a tela de detalhe (`/motos/:id`), **então** o dado "Próxima Revisão" deve ser exibido com destaque (ex.: em KPI ou nos dados técnicos).

---

### História de Usuário 2 — Persistência e Evolução do Modelo de Dados Backend (Prioridade: P1)

Como desenvolvedor ou administrador do sistema, quero que a entidade de domínio `Veiculo`, a configuração do EF Core (`VeiculoConfiguration`), os DTOs (`CreateVeiculoDTO`, `UpdateVeiculoDTO`, `VeiculoResponseDTO`), o mapeamento AutoMapper e a migração de banco de dados suportem de forma consistente a coluna `proximo_km_revisao`, garantindo compatibilidade com registros existentes e futuras consultas analíticas.

**Por que esta prioridade**: Assegurar integridade relacional, consistência de tipos e evolução limpa do banco de dados relacional (PostgreSQL/SQL Server) via migração gerenciada pelo EF Core.

**Teste independente**: Executar a migração `AddProximoKmRevisaoToVeiculo` no banco de dados, verificar a presença da coluna `proximo_km_revisao` como `integer NULL` na tabela `cad_veiculos`, e executar testes de integração/unidade da API garantindo que operações CRUD respeitam o novo campo.

**Cenários de aceitação**:

1. **Dado** a classe `Veiculo.cs` no domínio, **quando** inspecionada, **então** deve conter a propriedade `public int? ProximoKmRevisao { get; set; }`.
2. **Dado** o mapeamento `VeiculoConfiguration.cs`, **quando** executado pelo EF Core, **então** deve mapear a propriedade `ProximoKmRevisao` explicitamente para a coluna `proximo_km_revisao`.
3. **Dado** a migração gerada `AddProximoKmRevisaoToVeiculo`, **quando** aplicada (`Up`), **então** deve executar `AddColumn<int>("proximo_km_revisao", "cad_veiculos", nullable: true)`.
4. **Dado** que a migração é revertida (`Down`), **quando** executada, **então** deve executar `DropColumn("proximo_km_revisao", "cad_veiculos")`.
5. **Dado** os DTOs `CreateVeiculoDTO`, `UpdateVeiculoDTO` e `VeiculoResponseDTO`, **quando** enviados ou recebidos pela API de veículos, **então** devem serializar/desserializar `proximoKmRevisao` como número inteiro nulo ou positivo.
6. **Dado** o serviço `VeiculoService.cs`, **quando** o método `UpdateAsync` for executado, **então** deve atualizar `entity.ProximoKmRevisao = request.ProximoKmRevisao`.

---

## Requisitos Funcionais (FR)

- **FR-001**: O modelo de domínio `Veiculo` deve possuir a propriedade `ProximoKmRevisao` do tipo `int?` (inteiro anulável).
- **FR-002**: O mapeamento de infraestrutura EF Core (`VeiculoConfiguration`) deve associar a propriedade `ProximoKmRevisao` à coluna `proximo_km_revisao` na tabela `cad_veiculos`.
- **FR-003**: Deve existir uma migração do Entity Framework Core nomeada `AddProximoKmRevisaoToVeiculo` contendo as instruções `Up` e `Down` para inclusão/remoção da coluna `proximo_km_revisao`.
- **FR-004**: O DTO de criação `CreateVeiculoDTO` deve incluir a propriedade opcional `int? ProximoKmRevisao`.
- **FR-005**: O DTO de atualização `UpdateVeiculoDTO` deve incluir a propriedade opcional `int? ProximoKmRevisao`.
- **FR-006**: O DTO de resposta `VeiculoResponseDTO` deve incluir a propriedade `int? ProximoKmRevisao`.
- **FR-007**: O serviço de aplicação `VeiculoService` deve mapear e persistir o campo `ProximoKmRevisao` tanto no método `CreateAsync` quanto no método `UpdateAsync`.
- **FR-008**: A interface TypeScript `Veiculo` e os contratos de requisição `CreateVeiculoRequest` e `UpdateVeiculoRequest` no frontend (`veiculo.ts`) devem conter a propriedade opcional `proximoKmRevisao?: number | null`.
- **FR-009**: O formulário reativo em `VeiculoCadastroComponent` deve conter o controle `proximoKmRevisao` com validação de número não negativo (`Validators.min(0)`).
- **FR-010**: O template `veiculo-cadastro.html` deve exibir o campo de entrada "Próxima Revisão (KM)" na seção de Especificações Técnicas, com placeholder explicativo e mensagem de erro caso o valor seja inválido.
- **FR-011**: O método `salvar()` em `VeiculoCadastroComponent` deve converter e encaminhar o valor de `proximoKmRevisao` para o payload da requisição como `number` ou `null`.
- **FR-012**: A tela de detalhes do veículo (`veiculo-detalhe.html`) deve exibir a informação de "Próxima Revisão" quando cadastrada, permitindo visualização rápida da quilometragem planejada.
- **FR-013**: Deve haver validação nos testes unitários tanto para valores presentes quanto nulos do novo campo.

---

## Requisitos Não Funcionais (NFR)

- **NFR-001 (Compatibilidade Retroativa)**: A coluna `proximo_km_revisao` deve ser estritamente opcional/anulável (`nullable: true`), garantindo que nenhum registro pré-existente de motocicletas seja corrompido ou invalidado.
- **NFR-002 (Performance de Migração)**: A adição da coluna não deve bloquear tabelas nem exigir lock prolongado no banco de dados.
- **NFR-003 (Consistência de Nomenclatura)**: A coluna no banco deve seguir a convenção de snake_case (`proximo_km_revisao`), a entidade C# deve seguir PascalCase (`ProximoKmRevisao`), e o payload JSON / contratos TypeScript devem seguir camelCase (`proximoKmRevisao`).
- **NFR-004 (Aderência ao Design System)**: O campo no frontend deve seguir o mesmo padrão visual dark theme dos demais inputs numéricos (ano, KM), mantendo harmonia com o layout em grid.

---

## Casos de Borda

1. **Campo em branco/vazio**: Se o usuário não preencher a próxima revisão, o frontend envia `null` e o backend salva `NULL`. Nenhum erro deve ocorrer.
2. **Valor zero (`0`)**: O valor 0 é aceito matematicamente (`>= 0`), embora atípico para odômetros.
3. **Quilometragem da próxima revisão menor que a KM atual**: O sistema deve permitir ou alertar de forma não-bloqueante (já que o odômetro da moto pode ter sido atualizado recentemente ou o veículo pode estar atrasado na revisão). Não deve impedir o salvamento.
4. **Valor com texto ou caracteres especiais**: O input deve ser do tipo `number` e sanitizado contra caracteres não numéricos.
5. **Atualização de veículo existente sem o campo**: Caso um payload de atualização não informe `proximoKmRevisao`, ele deve ser tratado conforme os demais campos anuláveis sem quebrar o objeto.

---

## Critérios de Sucesso Mensuráveis (SC)

- **SC-001**: Migração `AddProximoKmRevisaoToVeiculo` aplicada com sucesso no esquema do banco de dados sem erros.
- **SC-002**: 100% de compilação da API `.NET` (`dotnet build`) sem advertências ou quebras de contrato.
- **SC-003**: 100% dos testes unitários do frontend (`npm test -- --no-watch`) passando com sucesso, incluindo os novos cenários de `proximoKmRevisao`.
- **SC-004**: Build do frontend (`npm run build`) concluído com sucesso e código de saída 0.
- **SC-005**: Dados de `proximoKmRevisao` enviados no formulário persistem no backend e são recuperados com fidelidade no `VeiculoResponseDTO`.

