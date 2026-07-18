# T006 - Evidencia US1: Clientes sem Token

Data: 2026-07-18
Comando executado:

```powershell
curl.exe -i http://localhost:5287/api/v1/Clientes
```

Resultado obtido:

```text
HTTP/1.1 401 Unauthorized
Content-Length: 0
WWW-Authenticate: Bearer
```

Conclusao:

- Endpoint de negocio bloqueado sem token, conforme esperado.
