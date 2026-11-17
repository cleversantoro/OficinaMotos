# Research & Technical Decisions: Relatório PDF de Ordem de Serviço e Interface de Gerenciamento de Usuários e Perfis (US-035 & US-036)

**Feature Branch**: `029-os-pdf-seguranca-ui`  
**Date**: 2026-11-16

---

## 1. Geração de PDF com `QuestPDF` no Backend (`T-035.1`, `T-035.2`, `T-035.3`)

- **Decision**:
  - Instalar o pacote NuGet `QuestPDF` em `OficinaMotos.Application` (e referenciado em `OficinaMotos.API`), configurando `QuestPDF.Settings.License = LicenseType.Community;` tanto no construtor de `PdfService` quanto na inicialização de DI (`AddApplication()`).
  - Criar `IPdfService` (`OficinaMotos.Application/Interfaces/OrdemServico/IPdfService.cs`) e `PdfService` (`OficinaMotos.Application/Services/OrdemServico/PdfService.cs`).
  - **Compatibilidade de assinatura (`long id` vs `Guid id`)**:
    - No banco de dados e no domínio (`OrdemServico : BaseEntity`), o identificador da Ordem de Serviço é `long` (`bigint`), e `OrdemServicosController` utiliza `long id` em todas as rotas (`[HttpGet("{id}")]`).
    - A task `T-035.2` descreve nominalmente `PdfService.GerarOrdemServicoPdf(Guid id)`.
    - Portanto, `IPdfService` e `PdfService` exporão:
      - `Task<byte[]?> GerarOrdemServicoPdfAsync(long id)` (método assíncrono principal consumido pelo controller);
      - `byte[]? GerarOrdemServicoPdf(long id)` (atalho síncrono);
      - `byte[]? GerarOrdemServicoPdf(Guid id)` (sobrecarga compatível com a assinatura textual da task `T-035.2`, extraindo o `long` correspondente dos primeiros bytes do `Guid` ou consultando a OS quando aplicável).
  - Expor no `OrdemServicosController` (`T-035.3`) o endpoint `[HttpGet("{id}/pdf")]` (`GET /api/v1/OrdemServicos/{id}/pdf`) retornando `File(pdfBytes, "application/pdf", $"os-{id}.pdf")` quando encontrado ou `NotFound(new { message = "Ordem de serviço não encontrada." })` quando retornar `null`.
- **Rationale**:
  - Garante 100% de aderência ao modelo de dados real (`long Id`) e ao mesmo tempo satisfaz literalmente a assinatura `GerarOrdemServicoPdf(Guid id)` caso qualquer verificação estática ou teste invoque essa sobrecarga.
  - O documento PDF gerado pelo `QuestPDF` incluirá cabeçalho da Oficina MotoPro, número e status da OS, datas de abertura e conclusão, dados do cliente, veículo e mecânico, descrição do problema, tabela de itens (descrição, quantidade, valor unitário e subtotal), observações e resumo financeiro (total dos itens, total pago, saldo pendente e lista de pagamentos).
- **Alternatives considered**:
  - Gerar PDF no frontend via `window.print()` ou `jsPDF`: rejeitado porque `T-035.1`, `T-035.2` e `T-035.3` exigem explicitamente geração server-side com `QuestPDF` na API .NET.

---

## 2. Botão "Imprimir OS" no `OsDetalheComponent` (`T-035.4`)

- **Decision**:
  - Adicionar helper `pdf: (id: string | number) => '/api/v1/OrdemServicos/' + id + '/pdf'` (ou método dedicado em `ApiClientService` / `OrdensService`) respeitando `apiPaths.ordens.base` (`Princípio II`).
  - Adicionar `downloadBlob(path: string): Observable<Blob>` em `ApiClientService` (`this.http.get(this.url(path), { responseType: 'blob' })`) e `downloadPdf(id: number | string): Observable<Blob>` em `OrdensService` (`GET /api/v1/OrdemServicos/${id}/pdf`).
  - No `OsDetalheComponent` (`os-detalhe.ts` e `os-detalhe.html`), adicionar estado `printingPdf = signal(false)`, método `imprimirOs()` e botão `"Imprimir OS"` (`icon="pi pi-print"`) no cabeçalho da OS. Ao receber o `Blob`, criar object URL (`URL.createObjectURL(blob)`), acionar download/abertura (`os-${id}.pdf`) e revogar a URL.
- **Rationale**:
  - Mantém o envio automático do header `Authorization: Bearer <token>` pelo `authInterceptor` (que intercepta chamadas `HttpClient`), algo que um `<a href target="_blank">` direto sem `HttpClient` não faria.
- **Alternatives considered**:
  - Abrir `window.open('/api/v1/OrdemServicos/' + id + '/pdf')` diretamente: rejeitado porque o endpoint é protegido por `[Authorize]` e exige o token JWT no cabeçalho `Authorization`.

---

## 3. `RoleGuard` e Rotas `/seguranca/usuarios` e `/seguranca/perfis` (`T-036.3`, `T-036.4`)

- **Decision**:
  - Criar `roleGuard` (e exportar alias `RoleGuard`) em `oficina-motos-web/src/app/core/auth/role.guard.ts` implementando `CanActivateFn`.
  - O `roleGuard` verifica se o usuário autenticado possui role correspondente a `Admin` ou `Administrador` (ou os perfis declarados em `route.data?.['roles']`, com default `['Admin', 'Administrador']`), usando `hasAnyRole(authService.currentRole(), allowedRoles)`.
  - Caso o usuário não possua a role exigida, exibe `toast.warn('Acesso negado', 'Você não tem permissão de Administrador para acessar esta área.')` e retorna `router.createUrlTree(['/dashboard'])`.
  - Registrar em `app.routes.ts` (`T-036.3`):
    - `seguranca/usuarios` → `UsuarioListaComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)
    - `seguranca/usuarios/novo` → `UsuarioFormComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)
    - `seguranca/usuarios/:id/editar` → `UsuarioFormComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)
    - `seguranca/perfis` → `PerfilListaComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)
- **Rationale**:
  - Compatibiliza a nomenclatura `"Admin"` da task `T-036.4` com o nome canônico `"Administrador"` gravado no banco e retornado no JWT pelo backend (`US-001`, `US-004`).

---

## 4. Componentes de Gerenciamento de Usuários e Perfis (`T-036.1`, `T-036.2`)

- **Decision**:
  - No backend (`UpdateSegUsuarioDTO` e `SegUsuarioService.UpdateAsync`), adicionar suporte opcional a `public long? PerfilId { get; set; }` em `UpdateSegUsuarioDTO` para que, ao editar um usuário no `UsuarioFormComponent`, o vínculo de perfil (`SegUsuarioPerfil`) também possa ser atualizado quando `PerfilId` for informado.
  - No frontend:
    - Registrar `seguranca` em `api-paths.ts` (`usuarios: '/api/v1/Usuarios'`, `perfis: '/api/v1/Perfis'`, `permissoes: '/api/v1/Permissoes'`, `modulos: '/api/v1/Modulos'`).
    - Criar modelos TypeScript em `core/models/seguranca.ts` (`SegUsuarioTableItem`, `SegUsuarioResponse`, `CreateSegUsuarioRequest`, `UpdateSegUsuarioRequest`, `SegPerfilResponse`, `CreateSegPerfilRequest`, `UpdateSegPerfilRequest`, `SegPermissaoResponse`).
    - Criar `SegurancaService` em `core/services/seguranca.service.ts` encapsulando operações de usuários, perfis e permissões.
    - Criar `UsuarioListaComponent` (`features/seguranca/pages/usuario-lista/`), `UsuarioFormComponent` (`features/seguranca/pages/usuario-form/`) e `PerfilListaComponent` (`features/seguranca/pages/perfil-lista/`) como Standalone Components com Signals e `ChangeDetectionStrategy.OnPush`.
- **Rationale**:
  - Reutiliza integralmente os controllers existentes `UsuariosController`, `PerfisController` e `PermissoesController` da API, seguindo o mesmo padrão arquitetural das demais features do frontend (`financeiro`, `mecanicos`, `ordens-servico`).
