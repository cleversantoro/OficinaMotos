# T010 - Evidencia US2: Login sem Token

Data: 2026-07-18
Comando executado:

```powershell
Invoke-WebRequest -Uri 'http://localhost:5287/api/v1/Auth/login' -Method Post -ContentType 'application/json' -Body '{"email":"usuario@oficina.com","password":"senha"}'
```

Resultado obtido:

```text
STATUS=401
{"message":"Credenciais inválidas ou usuário bloqueado."}
```

Conclusao:

- O endpoint de login foi processado sem bloqueio de autenticacao previa (nao houve 401 do middleware por ausencia de token).
