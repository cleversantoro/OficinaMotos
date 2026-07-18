# T007 - Evidencia US1: Startup com Chave JWT Valida

Data: 2026-07-18
Comandos executados:

```powershell
$env:Jwt__Key = "jwt-chave-segura-ambiente-local-2026"
dotnet run --project "c:/Projetos/OficinaMotos/oficina-motos-api/src/OficinaMotos.API/OficinaMotos.API.csproj"
```

Validacao de servico ativo:

```powershell
curl.exe -i http://localhost:5287/api/v1/Clientes
```

Resultado obtido:

```text
HTTP/1.1 401 Unauthorized
WWW-Authenticate: Bearer
```

Conclusao:

- A API iniciou com chave valida e processou requisicao no endpoint protegido, comprovando startup funcional sem fallback hardcoded.
