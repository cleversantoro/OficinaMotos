# T008 - Auditoria de Controllers e AllowAnonymous

Data: 2026-07-18
Comando executado:

```powershell
$controllers = Get-ChildItem -Recurse -File -Path src/OficinaMotos.API/Controllers -Filter *Controller.cs
$business = $controllers | Where-Object { $_.Name -ne 'AuthController.cs' }
Select-String -Path ($controllers.FullName) -Pattern "\[AllowAnonymous\]"
```

Resultados:

- TOTAL_CONTROLLERS = 62
- BUSINESS_CONTROLLERS = 61
- Uso de [AllowAnonymous] encontrado somente em:
  - src/OficinaMotos.API/Controllers/Auth/AuthController.cs (action Login)

Conclusao:

- Nao foram encontrados usos indevidos de [AllowAnonymous] em controllers de negocio.
