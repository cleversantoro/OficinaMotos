# Tarefas: Campo `proximo_km_revisao` no Veículo

**Entrada**: Documentos de design em `/specs/014-proximo-km-revisao/`
**Pré-requisitos**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/api.md`, `contracts/ui.md`, `quickstart.md`
**Organização**: Estruturadas por fases com foco em persistência relacional, contratos de API e reatividade na UI.

---

## Fase 1: Domínio, Infraestrutura e Banco de Dados (T-014.1, T-014.2)

**Objetivo**: Adicionar o campo na entidade de domínio e configurar a persistência e migração no EF Core.

- [x] T001 [T-014.1] Adicionar propriedade `int? ProximoKmRevisao` na entidade `Veiculo` em `oficina-motos-api/src/OficinaMotos.Domain/Entities/Veiculo.cs`
- [x] T002 [T-014.1] Adicionar mapeamento explícito da coluna `proximo_km_revisao` em `oficina-motos-api/src/OficinaMotos.Infrastructure/EntitiesConfiguration/VeiculoConfig/VeiculoConfigurations.cs`
- [x] T003 [T-014.2] Criar e aplicar a migração EF Core `AddProximoKmRevisaoToVeiculo` em `oficina-motos-api/src/OficinaMotos.Infrastructure/Migrations` e atualizar o snapshot do modelo

---

## Fase 2: Camada de Aplicação e DTOs (Backend)

**Objetivo**: Expor o campo nos DTOs de entrada e saída e sincronizar o serviço de aplicação.

- [x] T004 Adicionar `int? ProximoKmRevisao` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/Veiculo/CreateVeiculoDTO.cs`
- [x] T005 Adicionar `int? ProximoKmRevisao` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Requests/Veiculo/UpdateVeiculoDTO.cs`
- [x] T006 Adicionar `int? ProximoKmRevisao` em `oficina-motos-api/src/OficinaMotos.Application/DTOs/Responses/Veiculo/VeiculoResponseDTO.cs`
- [x] T007 Atualizar `VeiculoService.cs` para atribuir `entity.ProximoKmRevisao = request.ProximoKmRevisao;` no método `UpdateAsync`
- [x] T008 [P] Executar `dotnet build` na solução da API e garantir compilação com 0 erros

---

## Fase 3: Frontend Angular (T-014.3)

**Objetivo**: Adicionar suporte a `proximoKmRevisao` nos modelos, formulário de cadastro, detalhes e testes.

- [x] T009 [T-014.3] Adicionar `proximoKmRevisao?: number | null;` nas interfaces `Veiculo` e `CreateVeiculoRequest` em `oficina-motos-web/src/app/core/models/veiculo.ts`
- [x] T010 [T-014.3] Adicionar controle reativo `proximoKmRevisao` no `FormBuilder` e envio no payload em `oficina-motos-web/src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.ts`
- [x] T011 [T-014.3] Incluir campo numérico "Próxima Revisão (KM)" no template `oficina-motos-web/src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.html`
- [x] T012 [T-014.3] Exibir "Próxima Revisão" na página de detalhes `oficina-motos-web/src/app/features/motos/pages/veiculo-detalhe/veiculo-detalhe.html`
- [x] T013 [T-014.3] Atualizar testes unitários em `oficina-motos-web/src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.spec.ts` para cobrir o campo `proximoKmRevisao`

---

## Fase 4: Verificação e Governança

**Objetivo**: Assegurar integridade de toda a suíte de testes, build e atualização do backlog.

- [x] T014 [P] Executar a suíte de testes unitários com `npm test -- --no-watch` em `oficina-motos-web`
- [x] T015 [P] Executar `npm run build` em `oficina-motos-web`
- [x] T016 Atualizar `governance/backlog.md` marcando T-014.1 a T-014.3 como concluídas e atualizar status em `specs/014-proximo-km-revisao/tasks.md`

