# Pesquisa Técnica: Campo `proximo_km_revisao` no Veículo

**Feature**: `014-proximo-km-revisao` | **Data**: 2026-09-13

---

## 1. Contexto e Motivação

Na gestão de manutenção de motocicletas, o odômetro (quilometragem percorrida) é o principal indicador para intervenções preventivas recomendadas pelos fabricantes (trocas de óleo a cada 1.000 ou 3.000 km, revisão periódica a cada 6.000 ou 10.000 km, etc.).
A história de usuário **US-014** adiciona a propriedade `proximo_km_revisao` na entidade `Veiculo`, permitindo que o atendente/mecânico defina explicitamente quando a moto deve retornar para revisão preventiva.

---

## 2. Análise do Modelo de Dados e Banco de Dados

### 2.1 Tabela Atual (`cad_veiculos`)
Atualmente, a tabela `cad_veiculos` mapeada em `VeiculoConfigurations.cs` possui os campos:
- `Id` (BIGINT, PK)
- `Cliente_Id` (BIGINT, FK)
- `Placa` (VARCHAR(12))
- `Modelo_Id` (BIGINT, FK nullable)
- `Ano_Fab` (INT nullable)
- `Ano_Mod` (INT nullable)
- `Cor` (VARCHAR(50) nullable)
- `Chassi` (VARCHAR(80) nullable)
- `Renavam` (VARCHAR(20) nullable)
- `Km` (VARCHAR(20) nullable)
- `Combustivel` (VARCHAR(50) nullable)
- `Observacao` (VARCHAR(240) nullable)
- `Principal` (TINYINT(1))
- `Ativo` (TINYINT(1))
- `Created_At`, `Updated_At`, `IsDeleted`, `Deleted_At`

### 2.2 Nova Coluna: `proximo_km_revisao`
- Tipo no banco: `INT NULL` (MySQL)
- Nome no banco: `proximo_km_revisao` (snake_case padrão de banco relacional)
- Propriedade C#: `public int? ProximoKmRevisao { get; set; }`
- Propriedade TypeScript/JSON: `proximoKmRevisao?: number | null;`
- Justificativa do tipo `int?`: Quilometragens de motocicletas são grandezas inteiras discretas (ex.: `5000`, `12000`, `24000`). O uso de `int?` permite comparações numéricas diretas (`veiculo.Km >= veiculo.ProximoKmRevisao`), ordenações e consultas analíticas eficientes sem parsing de strings.

---

## 3. Análise da Camada Backend (.NET 8 Clean Architecture)

### 3.1 Entidade de Domínio
- Arquivo: `OficinaMotos.Domain/Entities/Veiculo.cs`
- Inclusão: `public int? ProximoKmRevisao { get; set; }`

### 3.2 Mapeamento EF Core
- Arquivo: `OficinaMotos.Infrastructure/EntitiesConfiguration/VeiculoConfig/VeiculoConfigurations.cs`
- Configuração:
  ```csharp
  builder.Property(e => e.ProximoKmRevisao)
         .HasColumnName("proximo_km_revisao");
  ```

### 3.3 Migração EF Core
- Nome da migration: `AddProximoKmRevisaoToVeiculo`
- Localização: `OficinaMotos.Infrastructure/Migrations/`
- Operação `Up`: `migrationBuilder.AddColumn<int>(name: "proximo_km_revisao", table: "cad_veiculos", type: "int", nullable: true);`
- Operação `Down`: `migrationBuilder.DropColumn(name: "proximo_km_revisao", table: "cad_veiculos");`

### 3.4 DTOs e Mapeamento AutoMapper
- `CreateVeiculoDTO.cs`: adicionar `public int? ProximoKmRevisao { get; set; }`
- `UpdateVeiculoDTO.cs`: adicionar `public int? ProximoKmRevisao { get; set; }`
- `VeiculoResponseDTO.cs`: adicionar `public int? ProximoKmRevisao { get; set; }`
- `VeiculoService.cs`:
  - `CreateAsync`: AutoMapper mapeia automaticamente de `CreateVeiculoDTO` para `Veiculo` (mesmo nome `ProximoKmRevisao`).
  - `UpdateAsync`: adicionar atribuição explícita `entity.ProximoKmRevisao = request.ProximoKmRevisao;`.

---

## 4. Análise da Camada Frontend (Angular 21)

### 4.1 Contratos TypeScript (`core/models/veiculo.ts`)
- Interface `Veiculo`: adicionar `proximoKmRevisao?: number | null;`
- Interface `CreateVeiculoRequest`: adicionar `proximoKmRevisao?: number | null;`
- Interface `UpdateVeiculoRequest`: herdada como type alias de `CreateVeiculoRequest`.

### 4.2 Formulário de Cadastro (`veiculo-cadastro`)
- Controle reativo no `FormBuilder`:
  ```typescript
  proximoKmRevisao: [null as number | null, [Validators.min(0)]],
  ```
- Submissão `salvar()`:
  ```typescript
  proximoKmRevisao: fv.proximoKmRevisao !== null && fv.proximoKmRevisao !== undefined && String(fv.proximoKmRevisao).trim() !== ''
    ? Number(fv.proximoKmRevisao)
    : null,
  ```
- Template HTML (`veiculo-cadastro.html`):
  Campo numérico posicionado na seção "Especificações Técnicas" ao lado do campo KM Atual:
  ```html
  <label class="field" [class.invalid]="hasError('proximoKmRevisao')">
    <span class="field-label">Próxima Revisão (KM)</span>
    <input
      type="number"
      class="form-control"
      formControlName="proximoKmRevisao"
      placeholder="Ex: 15000"
      min="0"
    />
    @if (hasError('proximoKmRevisao')) {
      <span class="error-msg">A quilometragem não pode ser negativa.</span>
    }
  </label>
  ```

### 4.3 Tela de Detalhes (`veiculo-detalhe`)
- Exibição de KPI ou dado técnico nos cards:
  ```html
  @if (veiculo.proximoKmRevisao) {
    <div class="kpi">
      <strong>{{ veiculo.proximoKmRevisao }} km</strong>
      <span>Próxima Revisão</span>
    </div>
  }
  ```

---

## 5. Estratégia de Testes

1. **Backend**:
   - `dotnet build`: Validar compilação limpa de todas as camadas.
   - Teste de unidade/integração para o `VeiculoService` cobrindo o mapeamento de `ProximoKmRevisao`.
2. **Frontend**:
   - Atualização de `veiculo-cadastro.spec.ts` para testar preenchimento, submissão no payload e validação de `Validators.min(0)`.
   - Execução de `npm test -- --no-watch` assegurando 100% de sucesso.
   - Execução de `npm run build` confirmando empacotamento de produção.

