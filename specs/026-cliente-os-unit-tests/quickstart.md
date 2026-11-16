# Guia Rápido de Verificação: ClienteServiceTests e OrdemServicoServiceTests (US-027 & US-028)

**Branch**: `026-cliente-os-unit-tests` | **Data**: 2026-11-16

## 1. Compilar a Solução Completa (`OficinaMotos.slnx`)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet build OficinaMotos.slnx
```

## 2. Executar os Testes Unitários de `ClienteService` (`US-027`)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~ClienteServiceTests"
```

## 3. Executar os Testes Unitários de `OrdemServicoService` (`US-028`)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~OrdemServicoServiceTests"
```

## 4. Executar Toda a Suíte `OficinaMotos.Tests`
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --verbosity normal
```
