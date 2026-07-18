# T007 - Evidencia US1: Clientes com Token Invalido

Data: 2026-07-18
Comando executado:

```powershell
curl.exe -i -H "Authorization: Bearer token_invalido" http://localhost:5287/api/v1/Clientes
```

Resultado obtido:

```text
HTTP/1.1 401 Unauthorized
WWW-Authenticate: Bearer error="invalid_token"
```

Conclusao:

- Endpoint de negocio bloqueado com token invalido, conforme esperado.
