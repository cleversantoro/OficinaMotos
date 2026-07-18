# T015 - Build Final da API

Data: 2026-07-18
Comando executado:

```powershell
dotnet build OficinaMotos.slnx
```

Resultado final:

- Build concluido com sucesso.
- Projetos compilados: Domain, Application, Infrastructure, API.
- Avisos conhecidos mantidos: NU1902 (OpenTelemetry.Exporter.OpenTelemetryProtocol).

Observacao:

- Build final executado com sucesso apos encerramento da API em execucao para evitar lock de DLL.
