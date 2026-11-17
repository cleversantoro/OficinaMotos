# Quickstart Validation Guide: Relatório PDF de OS e Interface de Gerenciamento de Usuários e Perfis (US-035 & US-036)

**Feature Branch**: `029-os-pdf-seguranca-ui`  
**Date**: 2026-11-16

---

## Pré-requisitos

- **.NET SDK 8.0+** instalado (`dotnet --version`)
- **Node.js 20+ / npm 10+** instalado (`npm --version`)

---

## 1. Validar Backend (`US-035`: `T-035.1` a `T-035.3`)

A partir do diretório `oficina-motos-api/`:

```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet build OficinaMotos.slnx
dotnet test src/OficinaMotos.Tests/OficinaMotos.Tests.csproj --filter "FullyQualifiedName~Pdf|FullyQualifiedName~OrdemServico"
```

**Resultado esperado**:
- Compilação limpa sem erros com o pacote `QuestPDF` instalado (`T-035.1`).
- Aprovação de todos os testes unitários e de integração:
  - `PdfService.GerarOrdemServicoPdfAsync(long id)` e `PdfService.GerarOrdemServicoPdf(Guid id)` geram bytes PDF válidos iniciados por `%PDF-` (`T-035.2`);
  - `GET /api/v1/OrdemServicos/{id}/pdf` retorna `200 OK` com `Content-Type: application/pdf` para OS existente e `404 NotFound` para OS inexistente (`T-035.3`).

---

## 2. Validar Frontend (`US-035`: `T-035.4` & `US-036`: `T-036.1` a `T-036.4`)

A partir do diretório `oficina-motos-web/`:

```powershell
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm test -- --watch=false
npm run build
```

**Resultado esperado**:
- Todas as suítes Vitest passam com 100% de sucesso, incluindo:
  - `os-detalhe.spec.ts`: botão `"Imprimir OS"` visível e acionando download de `GET /api/v1/OrdemServicos/{id}/pdf` (`T-035.4`);
  - `role.guard.spec.ts`: acesso permitido apenas para `Admin` / `Administrador` e bloqueado com redirecionamento para `/dashboard` para demais perfis (`T-036.4`);
  - `seguranca.service.spec.ts`, `usuario-lista.spec.ts`, `usuario-form.spec.ts` e `perfil-lista.spec.ts`: listagem, criação/edição de usuários e gerenciamento de perfis e rotas `/seguranca/usuarios` e `/seguranca/perfis` (`T-036.1`, `T-036.2`, `T-036.3`).
- `npm run build` conclui sem erros de compilação TypeScript ou template Angular.
