# T001 - Baseline de Build da API

Data: 2026-07-18
Comando executado:

```powershell
dotnet build OficinaMotos.slnx
```

Resultado:

- Build concluido com sucesso.
- Projetos compilados: Domain, Application, Infrastructure, API.
- Avisos conhecidos: NU1902 em OpenTelemetry.Exporter.OpenTelemetryProtocol (3 advisories).

Observacao:

- O baseline foi validado no inicio da implementacao para garantir ambiente compilavel antes da consolidacao final.
