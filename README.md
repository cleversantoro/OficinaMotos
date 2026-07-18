# Oficina Motos API

API REST em .NET 8 para gestao completa de uma oficina de motos: clientes, veiculos, mecanicos, fornecedores, ordens de servico, estoque e financeiro. O projeto segue uma divisao em camadas (API, Application, Domain, Infrastructure), com validacoes, mapeamentos e observabilidade prontos para producao.

## Arquitetura e tecnologias
- ASP.NET Core 8 com controllers REST (`api/v1/[controller]`).
- Entity Framework Core + MySQL (contexto `OficinaContext`).
- FluentValidation para validacao das DTOs de entrada.
- AutoMapper para conversao entre entidades de dominio e DTOs.
- JWT (Bearer) pronto para ser habilitado; CORS liberado para `http://localhost:4200`.
- Observabilidade: Serilog (logs estruturados) + OpenTelemetry (tracing e metricas).
- Swagger/OpenAPI com suporte a JWT.

## Preparacao do ambiente
1) **Requisitos**: .NET 8 SDK, MySQL 8+, acesso a porta configurada.
2) **Banco**: crie a base (ex.: `oficina_db`). Ajuste as strings de conexao:
   - `appsettings.json` chave `ConnectionStrings:OficinaDb` (usada no `Program.cs`).
   - Opcionalmente defina `ConnectionStrings:DefaultConnection` com o mesmo valor (usada em `Infrastructure/IoC/DependencyInjection.cs`).
3) **JWT** (obrigatorio): defina `Jwt:Key` por variavel de ambiente ou provider de configuracao externa. A API falha no startup (fail-fast) se a chave estiver ausente, vazia ou com apenas espacos.
   - Exemplo PowerShell (sessao atual):
     - `$env:Jwt__Key = "minha-chave-jwt-super-segura"`
   - Em producao, use um segredo gerenciado (pipeline/secret store) e nao versione a chave no repositorio.
4) **Pacotes**: `dotnet restore`.

## Como executar
```bash
dotnet run --project src/OficinaMotos.API/OficinaMotos.API.csproj
```
- Swagger: http://localhost:5099/swagger (porta pode variar conforme o launchSettings).
- CORS: liberado para `http://localhost:4200` (Angular). Ajuste se necessario em `Program.cs`.
- Logs e traces: Serilog escreve no console; OpenTelemetry instrumenta HTTP/EF (configure exporters externos conforme sua infra).

## Camadas e responsabilidades
- **OficinaMotos.API**: Controllers, pipeline (JWT, CORS, Swagger, Serilog, OpenTelemetry) e tratamento global de erros (`GlobalExceptionHandler`).
- **OficinaMotos.Application**: Services, DTOs (Requests/Responses), validadores, mapeamentos AutoMapper e registro de dependencias.
- **OficinaMotos.Domain**: Entidades ricas (Cliente, Veiculo, Mecanico, Fornecedor, OrdemServico, Estoque, Financeiro) e contratos de repositorio.
- **OficinaMotos.Infrastructure**: EF Core (`OficinaContext`), configuracoes de entidades, implementacoes de repositorio e IoC de infraestrutura.

## Padrao de endpoints
- Base: `api/v1/{ControllerName}` (case-insensitive).
- Todos os controllers expostos seguem CRUD basico:
  - `GET /api/v1/{recurso}` -> lista.
  - `GET /api/v1/{recurso}/{id}` -> item por id.
  - `POST /api/v1/{recurso}` -> cria (DTO Create*).
  - `PUT /api/v1/{recurso}/{id}` -> atualiza (DTO Update*).
  - `DELETE /api/v1/{recurso}/{id}` -> remove.
- Responses usam DTOs de resposta especificas e retornam 404 quando o id nao e encontrado.

## Endpoints por dominio

### Clientes
- `GET /api/v1/Clientes` | `GET /api/v1/Clientes/{id}` | `POST /api/v1/Clientes` | `PUT /api/v1/Clientes/{id}` | `DELETE /api/v1/Clientes/{id}` – cadastro base.
- `GET /api/v1/ClientePfs` | ... | `DELETE /api/v1/ClientePfs/{id}` – dados de pessoa fisica.
- `GET /api/v1/ClientePjs` | ... | `DELETE /api/v1/ClientePjs/{id}` – dados de pessoa juridica.
- `GET /api/v1/ClienteOrigens` | ... | `DELETE /api/v1/ClienteOrigens/{id}` – origem do lead/cadastro.
- `GET /api/v1/ClienteLgpdConsentimentos` | ... | `DELETE /api/v1/ClienteLgpdConsentimentos/{id}` – consentimento LGPD.
- `GET /api/v1/ClienteIndicacoes` | ... | `DELETE /api/v1/ClienteIndicacoes/{id}` – indicacoes e referenciadores.
- `GET /api/v1/ClienteFinanceiros` | ... | `DELETE /api/v1/ClienteFinanceiros/{id}` – configuracao financeira (limites, prazos).
- `GET /api/v1/ClienteEnderecos` | ... | `DELETE /api/v1/ClienteEnderecos/{id}` – enderecos e CEP.
- `GET /api/v1/ClienteDocumentos` | ... | `DELETE /api/v1/ClienteDocumentos/{id}` – RG/CNH/etc.
- `GET /api/v1/ClienteContatos` | ... | `DELETE /api/v1/ClienteContatos/{id}` – telefones/emails.
- `GET /api/v1/ClienteAnexos` | ... | `DELETE /api/v1/ClienteAnexos/{id}` – uploads associados.

### Veiculos
- `GET /api/v1/Veiculos` | ... | `DELETE /api/v1/Veiculos/{id}` – veiculos vinculados ao cliente.
- `GET /api/v1/VeiculoMarcas` | ... | `DELETE /api/v1/VeiculoMarcas/{id}` – marcas.
- `GET /api/v1/VeiculoModelos` | ... | `DELETE /api/v1/VeiculoModelos/{id}` – modelos (ligados a marcas).

### Fornecedores
- `GET /api/v1/Fornecedores` | ... | `DELETE /api/v1/Fornecedores/{id}` – cadastro base.
- `GET /api/v1/FornecedorSegmentos` | ... | `DELETE /api/v1/FornecedorSegmentos/{id}` – segmentos de atuacao.
- `GET /api/v1/FornecedorSegmentosRel` | ... | `DELETE /api/v1/FornecedorSegmentosRel/{id}` – relacao fornecedor x segmento.
- `GET /api/v1/FornecedorEnderecos` | ... | `DELETE /api/v1/FornecedorEnderecos/{id}` – enderecos.
- `GET /api/v1/FornecedorContatos` | ... | `DELETE /api/v1/FornecedorContatos/{id}` – contatos gerais.
- `GET /api/v1/FornecedorRepresentantes` | ... | `DELETE /api/v1/FornecedorRepresentantes/{id}` – representantes comerciais.
- `GET /api/v1/FornecedorBancos` | ... | `DELETE /api/v1/FornecedorBancos/{id}` – dados bancarios.
- `GET /api/v1/FornecedorDocumentos` | ... | `DELETE /api/v1/FornecedorDocumentos/{id}` – documentos fiscais/contratuais.
- `GET /api/v1/FornecedorCertificacoes` | ... | `DELETE /api/v1/FornecedorCertificacoes/{id}` – certificacoes e compliance.
- `GET /api/v1/FornecedorAvaliacoes` | ... | `DELETE /api/v1/FornecedorAvaliacoes/{id}` – avaliacao/score do fornecedor.

### Mecanicos
- `GET /api/v1/Mecanicos` | ... | `DELETE /api/v1/Mecanicos/{id}` – cadastro base.
- `GET /api/v1/MecanicoEspecialidades` | ... | `DELETE /api/v1/MecanicoEspecialidades/{id}` – especialidades por tipo de servico.
- `GET /api/v1/MecanicoEspecialidadeRel` | ... | `DELETE /api/v1/MecanicoEspecialidadeRel/{id}` – relacao mecanico x especialidade.
- `GET /api/v1/MecanicoCertificacoes` | ... | `DELETE /api/v1/MecanicoCertificacoes/{id}` – certificacoes tecnicas.
- `GET /api/v1/MecanicoContatos` | ... | `DELETE /api/v1/MecanicoContatos/{id}` – contatos.
- `GET /api/v1/MecanicoDisponibilidades` | ... | `DELETE /api/v1/MecanicoDisponibilidades/{id}` – disponibilidade/agenda.
- `GET /api/v1/MecanicoDocumentos` | ... | `DELETE /api/v1/MecanicoDocumentos/{id}` – documentos pessoais/profissionais.
- `GET /api/v1/MecanicoEnderecos` | ... | `DELETE /api/v1/MecanicoEnderecos/{id}` – enderecos.
- `GET /api/v1/MecanicoExperiencias` | ... | `DELETE /api/v1/MecanicoExperiencias/{id}` – historico de experiencias.

### Ordens de Servico
- `GET /api/v1/OrdemServicos` | ... | `DELETE /api/v1/OrdemServicos/{id}` – OS principal.
- `GET /api/v1/OrdemServicoPagamentos` | ... | `DELETE /api/v1/OrdemServicoPagamentos/{id}` – pagamentos associados.
- `GET /api/v1/OrdemServicoObservacoes` | ... | `DELETE /api/v1/OrdemServicoObservacoes/{id}` – observacoes gerais.
- `GET /api/v1/OrdemServicoItens` | ... | `DELETE /api/v1/OrdemServicoItens/{id}` – itens/pecas/servicos.
- `GET /api/v1/OrdemServicoHistoricos` | ... | `DELETE /api/v1/OrdemServicoHistoricos/{id}` – historico de status.
- `GET /api/v1/OrdemServicoChecklists` | ... | `DELETE /api/v1/OrdemServicoChecklists/{id}` – checklists de inspecao.
- `GET /api/v1/OrdemServicoAvaliacoes` | ... | `DELETE /api/v1/OrdemServicoAvaliacoes/{id}` – avaliacao do cliente.
- `GET /api/v1/OrdemServicoAnexos` | ... | `DELETE /api/v1/OrdemServicoAnexos/{id}` – anexos e midias.

### Estoque
- `GET /api/v1/EstoqueCategorias` | ... | `DELETE /api/v1/EstoqueCategorias/{id}` – categorias de pecas.
- `GET /api/v1/EstoqueFabricantes` | ... | `DELETE /api/v1/EstoqueFabricantes/{id}` – fabricantes.
- `GET /api/v1/EstoqueLocalizacoes` | ... | `DELETE /api/v1/EstoqueLocalizacoes/{id}` – localizacoes fisicas.
- `GET /api/v1/EstoquePecas` | ... | `DELETE /api/v1/EstoquePecas/{id}` – cadastro de pecas.
- `GET /api/v1/EstoqueMovimentacoes` | ... | `DELETE /api/v1/EstoqueMovimentacoes/{id}` – movimentacao/ajuste de estoque.
- `GET /api/v1/EstoquePecaAnexos` | ... | `DELETE /api/v1/EstoquePecaAnexos/{id}` – anexos por peca.
- `GET /api/v1/EstoquePecaFornecedores` | ... | `DELETE /api/v1/EstoquePecaFornecedores/{id}` – fornecedores vinculados a uma peca.
- `GET /api/v1/EstoquePecaHistoricos` | ... | `DELETE /api/v1/EstoquePecaHistoricos/{id}` – historico de preco/estoque.

### Financeiro
- `GET /api/v1/FinanceiroMetodosPagamento` | ... | `DELETE /api/v1/FinanceiroMetodosPagamento/{id}` – meios de pagamento.
- `GET /api/v1/FinanceiroPagamentos` | ... | `DELETE /api/v1/FinanceiroPagamentos/{id}` – pagamentos recebidos/realizados.
- `GET /api/v1/FinanceiroAnexos` | ... | `DELETE /api/v1/FinanceiroAnexos/{id}` – anexos financeiros.
- `GET /api/v1/FinanceiroContasPagar` | ... | `DELETE /api/v1/FinanceiroContasPagar/{id}` – contas a pagar.
- `GET /api/v1/FinanceiroContasReceber` | ... | `DELETE /api/v1/FinanceiroContasReceber/{id}` – contas a receber.
- `GET /api/v1/FinanceiroHistoricos` | ... | `DELETE /api/v1/FinanceiroHistoricos/{id}` – historico contabile/financeiro.
- `GET /api/v1/FinanceiroLancamentos` | ... | `DELETE /api/v1/FinanceiroLancamentos/{id}` – lancamentos financeiros.

## Observacoes e boas praticas
- Habilite `[Authorize]` nos controllers (ja comentado em `ClientesController`) quando configurar JWT.
- Para deploy: troque `UseSwagger()`/`UseSwaggerUI()` para ambientes adequados e ajuste `RequireHttpsMetadata` do JWT.
- Considere alinhar as chaves `OficinaDb` e `DefaultConnection` para evitar divergencias entre `Program.cs` e o IoC de Infrastructure.
- Adicione exporters no OpenTelemetry conforme seu stack (Prometheus/OTLP/Jaeger) e sinks do Serilog para arquivo, Seq ou Elastic.
