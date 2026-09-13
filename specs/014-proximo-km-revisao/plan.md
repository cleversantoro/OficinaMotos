# Plano de Implementação: Campo `proximo_km_revisao` no Veículo

**Branch**: `014-proximo-km-revisao` | **Data**: 2026-09-13 | **Spec**: [spec.md](spec.md)

**Entrada**: US-014 — Adicionar campo `proximo_km_revisao` ao veículo
- **Prioridade**: 🟡 Should | **Estimativa**: S | **Sprint**: 3 | **Depende**: US-013
- **Tasks**:
  - [ ] T-014.1 — Adicionar `int? ProximoKmRevisao` em `Veiculo.cs`
  - [ ] T-014.2 — Criar migration `AddProximoKmRevisaoToVeiculo`
  - [ ] T-014.3 — Incluir campo no formulário

---

## Resumo

A funcionalidade entrega a extensão de ponta a ponta para a propriedade `proximo_km_revisao` na entidade `Veiculo`. Isso permite planejar a quilometragem recomendada para a próxima revisão preventiva da moto, tanto no cadastro quanto em edições e consultas.

O escopo compreende:
1. **Domínio e Infraestrutura**: inclusão de `ProximoKmRevisao` em `Veiculo.cs`, configuração do mapeamento em `VeiculoConfigurations.cs` e criação da migração `AddProximoKmRevisaoToVeiculo`.
2. **Camada de Aplicação**: atualização de `CreateVeiculoDTO`, `UpdateVeiculoDTO`, `VeiculoResponseDTO` e sincronização no método `UpdateAsync` do `VeiculoService`.
3. **Frontend Angular**: atualização das interfaces em `core/models/veiculo.ts`, adição do controle reativo e validação em `VeiculoCadastroComponent`, renderização do campo com mensagens inline em `veiculo-cadastro.html` e exibição nas visualizações do veículo.
4. **Garantia de Qualidade**: testes automatizados no frontend com Vitest e compilação limpa de ambas as camadas.

---

## Contexto Técnico

**Linguagem/Versão**:
- Backend: C# 12 / .NET 8 (ASP.NET Core Web API, EF Core 8)
- Frontend: TypeScript 5.9.2 com Angular 21 (Signals + Standalone Components + Reactive Forms)

**Armazenamento**:
- Banco de dados MySQL 8 com a tabela `cad_veiculos` recebendo a coluna `proximo_km_revisao int NULL`.

**Testes**:
- Backend: `dotnet build` e testes automatizados existentes
- Frontend: Vitest com Angular TestBed (`npm test -- --no-watch`) cobrindo `VeiculoCadastroComponent`

---

## Fases de Execução

### Fase 1: Domínio e Banco de Dados (T-014.1, T-014.2)
1. Adicionar `public int? ProximoKmRevisao { get; set; }` na classe `Veiculo` (`OficinaMotos.Domain/Entities/Veiculo.cs`).
2. Configurar o mapeamento da coluna em `VeiculoConfigurations.cs`:
   `builder.Property(e => e.ProximoKmRevisao).HasColumnName("proximo_km_revisao");`
3. Criar a migração EF Core `AddProximoKmRevisaoToVeiculo` adicionando a coluna `proximo_km_revisao` do tipo `int` (anulável) na tabela `cad_veiculos`.
4. Atualizar o `OficinaContextModelSnapshot.cs`.

### Fase 2: Camada de Aplicação e DTOs (Backend)
1. Atualizar `CreateVeiculoDTO.cs` com `public int? ProximoKmRevisao { get; set; }`.
2. Atualizar `UpdateVeiculoDTO.cs` com `public int? ProximoKmRevisao { get; set; }`.
3. Atualizar `VeiculoResponseDTO.cs` com `public int? ProximoKmRevisao { get; set; }`.
4. Atualizar `VeiculoService.cs` para atribuir `entity.ProximoKmRevisao = request.ProximoKmRevisao;` no `UpdateAsync`.
5. Compilar a solução .NET e verificar 0 erros.

### Fase 3: Frontend Angular (T-014.3)
1. Atualizar interfaces em `oficina-motos-web/src/app/core/models/veiculo.ts`:
   - `Veiculo`: `proximoKmRevisao?: number | null;`
   - `CreateVeiculoRequest`: `proximoKmRevisao?: number | null;`
2. Adicionar controle no formulário reativo de `VeiculoCadastroComponent`:
   - `proximoKmRevisao: [null as number | null, [Validators.min(0)]]`
   - Extrair e converter valor no método `salvar()`.
3. Atualizar template `veiculo-cadastro.html`:
   - Campo "Próxima Revisão (KM)" na seção Especificações Técnicas.
4. Atualizar `veiculo-detalhe.html`:
   - Exibir o campo na seção de Dados Técnicos / KPI quando presente.
5. Atualizar testes unitários em `veiculo-cadastro.spec.ts` para testar envio do campo no payload e validação de número negativo.

### Fase 4: Verificação e Governança
1. Executar testes frontend (`npm test -- --no-watch`).
2. Executar build frontend (`npm run build`).
3. Executar compilação backend (`dotnet build`).
4. Atualizar `governance/backlog.md` marcando T-014.1 a T-014.3 como concluídas.

