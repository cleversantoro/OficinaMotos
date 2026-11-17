# Contratos de API e Componentes: Relatório PDF de OS e Gerenciamento de Usuários/Perfis (US-035 & US-036)

**Feature Branch**: `029-os-pdf-seguranca-ui`  
**Date**: 2026-11-16

---

## 1. Contrato HTTP — `GET /api/v1/OrdemServicos/{id}/pdf` (`US-035`)

- **Controller**: `OrdemServicosController` (`oficina-motos-api/src/OficinaMotos.API/Controllers/OrdemServico/OrdemServicosController.cs`)
- **Autorização**: `[Authorize]` (requer Bearer token JWT válido; retorna `401 Unauthorized` quando ausente/inválido)
- **Método e Rota**: `GET /api/v1/OrdemServicos/{id}/pdf`
- **Route Parameters**:
  - `id` (`long`): Identificador da Ordem de Serviço.
- **Responses**:
  - `200 OK`:
    - `Content-Type: application/pdf`
    - `Content-Disposition: attachment; filename=os-{id}.pdf`
    - Corpo: binário PDF válido (`%PDF-1.x...`) gerado pelo `PdfService` usando `QuestPDF`.
  - `404 NotFound` (`application/json`):
    ```json
    {
      "message": "Ordem de serviço não encontrada."
    }
    ```

---

## 2. Contrato de Frontend — Impressão de PDF no `OsDetalheComponent` (`T-035.4`)

- **`apiPaths.ordens`** (`src/app/core/services/api-paths.ts`):
  ```typescript
  ordens: {
    base: '/api/v1/OrdemServicos',
    pdf: (id: number | string) => `/api/v1/OrdemServicos/${id}/pdf`,
    // ...
  }
  ```
- **`OrdensService.downloadPdf(id: number | string): Observable<Blob>`**:
  - Executa `GET` em `/api/v1/OrdemServicos/${id}/pdf` com `responseType: 'blob'`.
- **`OsDetalheComponent`** (`src/app/features/ordens-servico/pages/os-detalhe/os-detalhe.ts`):
  - Renderiza botão `"Imprimir OS"` (`data-testid="btn-imprimir-os"` / `label="Imprimir OS"`, `icon="pi pi-print"`) no cabeçalho da OS.
  - Ao clicar, aciona `imprimirOs()`, que chama `ordensService.downloadPdf(ordem.id)` e faz o download/abertura do PDF `os-${ordem.id}.pdf`.

---

## 3. Contrato de Segurança, Rotas e Componentes (`US-036`)

### 3.1 `apiPaths.seguranca` (`src/app/core/services/api-paths.ts`)
```typescript
seguranca: {
  usuarios: '/api/v1/Usuarios',
  perfis: '/api/v1/Perfis',
  permissoes: '/api/v1/Permissoes',
  modulos: '/api/v1/Modulos',
}
```

### 3.2 `roleGuard` / `RoleGuard` (`src/app/core/auth/role.guard.ts`) (`T-036.4`)
- Verifica se `authService.currentRole()` pertence aos perfis autorizados (`route.data?.['roles'] ?? ['Admin', 'Administrador']`), aceitando tanto `'Admin'` quanto `'Administrador'` (case-insensitive via `hasAnyRole`).
- Se autorizado: retorna `true`.
- Se não autorizado: emite `toast.warn('Acesso negado', ...)` e retorna `UrlTree` para `['/dashboard']`.

### 3.3 Rotas Registradas em `app.routes.ts` (`T-036.3`)
- `/seguranca/usuarios` → `UsuarioListaComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)
- `/seguranca/usuarios/novo` → `UsuarioFormComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)
- `/seguranca/usuarios/:id/editar` → `UsuarioFormComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)
- `/seguranca/perfis` → `PerfilListaComponent` (`canActivate: [roleGuard]`, `data: { roles: ['Admin', 'Administrador'] }`)

### 3.4 Componentes de UI (`T-036.1` & `T-036.2`)
- **`UsuarioListaComponent`** (`features/seguranca/pages/usuario-lista/usuario-lista.ts`):
  - Lista usuários (`GET /api/v1/Usuarios`), permite busca/filtro, botão `"Novo Usuário"` (`/seguranca/usuarios/novo`), ação de editar (`/seguranca/usuarios/:id/editar`), atalho para `/seguranca/perfis` e exclusão de usuário (`DELETE /api/v1/Usuarios/:id`).
- **`UsuarioFormComponent`** (`features/seguranca/pages/usuario-form/usuario-form.ts`):
  - Formulário reativo para criação (`POST /api/v1/Usuarios`) e edição (`PUT /api/v1/Usuarios/:id`), carregando perfis de `GET /api/v1/Perfis` e validando nome, login, e-mail, perfil e senha (na criação).
- **`PerfilListaComponent`** (`features/seguranca/pages/perfil-lista/perfil-lista.ts`):
  - Lista perfis de acesso (`GET /api/v1/Perfis`) e permissões disponíveis (`GET /api/v1/Permissoes`), permitindo criar/editar perfil e gerenciar as permissões vinculadas (`PUT /api/v1/Perfis/:id/permissoes`).
