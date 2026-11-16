# Quickstart: Validação dos Testes de Integração (US-029) e Specs Angular (US-030 & US-031)

**Feature**: `027-integration-angular-specs`  
**Date**: 2026-11-16

---

## Pré-requisitos

- **.NET 8 SDK** instalado (`dotnet --version`)
- **Node.js / npm 10** instalado em `oficina-motos-web/` (`npm --version`)

---

## 1. Validar Testes de Integração Backend (`US-029`)

A partir de `c:\Projetos\OficinaMotos\oficina-motos-api`:

```powershell
# Compilar toda a solução
dotnet build OficinaMotos.slnx

# Executar exclusivamente os testes de integração (AuthControllerIntegrationTests e ClienteControllerIntegrationTests)
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Integration"

# Executar toda a suíte de testes do backend (unitários + integração)
dotnet test OficinaMotos.slnx
```

**Resultado esperado**:
- `AuthControllerIntegrationTests` e `ClienteControllerIntegrationTests` aprovados (`Passed! - Failed: 0`).
- Nenhuma tentativa de conexão com MySQL externo durante os testes de integração.

---

## 2. Validar Specs de Services, Guards e Interceptors Angular (`US-030` & `US-031`)

A partir de `c:\Projetos\OficinaMotos\oficina-motos-web`:

```powershell
# Executar a suíte de testes unitários Angular (Vitest / ng test sem watch)
npm test -- --watch=false

# Verificar build de produção do frontend
npm run build
```

**Resultado esperado**:
- 100% de aprovação nas suítes:
  - `src/app/core/auth/auth.service.spec.ts` (`T-030.1`)
  - `src/app/core/services/clientes.service.spec.ts` (`T-030.2`)
  - `src/app/core/services/ordens.service.spec.ts` (`T-030.3`)
  - `src/app/core/auth/auth.guard.spec.ts` (`T-031.1`)
  - `src/app/core/auth/auth.interceptor.spec.ts` (`T-031.2`)
  - `src/app/core/interceptors/error-interceptor.spec.ts` (`T-031.3`)
