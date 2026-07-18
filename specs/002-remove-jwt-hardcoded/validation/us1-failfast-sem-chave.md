# T006 - Evidencia US1: Fail-fast sem Chave JWT

Data: 2026-07-18
Comando executado:

```powershell
dotnet run --project src/OficinaMotos.API/OficinaMotos.API.csproj
```

Resultado obtido:

```text
Unhandled exception. System.InvalidOperationException: Configuração obrigatória ausente: Jwt:Key. Defina a chave JWT por variável de ambiente ou configuração externa antes de iniciar a API.
   at Program.<Main>$(String[] args) in .../Program.cs:line 89
```

Conclusao:

- Startup interrompido imediatamente sem segredo configurado, conforme fail-fast esperado.
