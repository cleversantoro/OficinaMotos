# T002 - Matriz de Validacao de Configuracao JWT

## Regras de referencia

- Fonte: contracts/jwt-configuration-contract.md
- Chave obrigatoria: Jwt:Key
- Sem fallback hardcoded
- Fail-fast para ausente/vazia/whitespace

## Matriz

| Cenario | Entrada | Resultado Esperado | Resultado Obtido | Status |
| --- | --- | --- | --- | --- |
| Startup sem chave JWT | Jwt:Key ausente/vazia | InvalidOperationException no startup com mensagem clara | Excecao disparada com mensagem de configuracao obrigatoria | PASS |
| Startup com chave valida | Jwt__Key definida no ambiente | API inicia sem fallback hardcoded | API iniciou e respondeu 401 em endpoint protegido (servico ativo) | PASS |
| Config versionada | appsettings.json | Jwt:Key sem segredo real | Jwt:Key = "" | PASS |
| Documentacao operacional | README.md | Instrucao clara de configuracao externa obrigatoria | Secao JWT atualizada com fail-fast e exemplo PowerShell | PASS |
