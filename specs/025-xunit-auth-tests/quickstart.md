# Guia Rápido de Verificação: OficinaMotos.Tests e AuthServiceTests (US-025 & US-026)

**Branch**: `025-xunit-auth-tests` | **Data**: 2026-11-16

## 1. Compilar a Solução Completa (`OficinaMotos.slnx`)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet build OficinaMotos.slnx
```

## 2. Executar os Testes Unitários do Projeto `OficinaMotos.Tests`
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --no-build --verbosity normal
```

## 3. Executar Apenas os Testes de `AuthService` (`US-026`)
```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~AuthServiceTests"
```
