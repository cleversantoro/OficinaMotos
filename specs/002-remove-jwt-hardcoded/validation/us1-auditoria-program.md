# T008 - Auditoria de Program.cs (Sem Segredo Hardcoded)

Data: 2026-07-18
Arquivo auditado:

- oficina-motos-api/src/OficinaMotos.API/Program.cs

Evidencias encontradas:

- Leitura de chave: `builder.Configuration["Jwt:Key"]`
- Validacao obrigatoria: `if (string.IsNullOrWhiteSpace(jwtKey)) throw new InvalidOperationException(...)`
- Uso da chave: `Encoding.ASCII.GetBytes(jwtKey)`
- Ausencia de literal de segredo default no codigo

Conclusao:

- Nao existe fallback hardcoded para Jwt:Key no bootstrap da API.
