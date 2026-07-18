# T015 - Build Final da API

Data: 2026-07-18
Comando executado:

```powershell
dotnet build OficinaMotos.slnx
```

Resultado final:

- Build concluido com sucesso apos encerrar a API em execucao.
- Projetos compilados: Domain, Application, Infrastructure, API.
- Avisos conhecidos mantidos: NU1902 (OpenTelemetry.Exporter.OpenTelemetryProtocol).

Observacao:

- Houve uma tentativa intermediaria com falha por lock de DLL enquanto a API estava em execucao; apos encerrar o processo, o build final passou.
