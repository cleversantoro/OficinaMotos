# Data Model: Relatório PDF de Ordem de Serviço e Interface de Gerenciamento de Usuários e Perfis (US-035 & US-036)

**Feature Branch**: `029-os-pdf-seguranca-ui`  
**Date**: 2026-11-16

---

## 1. Backend — Serviço de Relatório PDF de Ordem de Serviço (`US-035`)

### 1.1 `IPdfService` (`OficinaMotos.Application.Interfaces.OrdemServico`)
| Método | Parâmetros | Retorno | Descrição |
| :--- | :--- | :--- | :--- |
| `GerarOrdemServicoPdfAsync` | `long id` | `Task<byte[]?>` | Carrega a `OrdemServico` com `Cliente`, `Veiculo`, `Mecanico`, `Itens`, `Observacoes` e `Pagamentos` e gera o binário PDF via `QuestPDF`. Retorna `null` se a OS não existir. |
| `GerarOrdemServicoPdf` | `long id` | `byte[]?` | Versão síncrona para geração do PDF da OS por `long id`. |
| `GerarOrdemServicoPdf` | `Guid id` | `byte[]?` | Sobrecarga compatível com a assinatura nominal de `T-035.2`. |

### 1.2 Dados Renderizados no Documento PDF da OS
- **Cabeçalho**: Identificação da Oficina MotoPro, Número da OS (`#Id`), Status atual (`Aberta`, `Em Andamento`, `Aguardando Peça`, `Concluída`, `Cancelada`), Data de Abertura e Data de Conclusão.
- **Seção Dados Gerais**:
  - Cliente (`NomeExibicao` / `Nome`, `Documento`, `Telefone`, `Email`)
  - Veículo (`Placa`, `Marca`/`Modelo`, `AnoFab`/`AnoMod`, `Cor`, `Km`)
  - Mecânico Responsável (`Nome` `Sobrenome`)
  - Descrição do Problema
- **Seção Itens da OS**: Tabela contendo `Descrição`, `Qtd`, `Valor Unitário (R$)`, `Subtotal (R$)` e `Total Geral dos Itens`.
- **Seção Resumo Financeiro e Pagamentos**: `Total da OS`, `Total Pago`, `Saldo Pendente` e listagem de pagamentos (`Método`, `Status`, `Data`, `Valor`).

---

## 2. Backend — DTOs de Segurança (`OficinaMotos.Application.DTOs.Requests.Seguranca` & `Responses.Seguranca`)

### 2.1 `CreateSegUsuarioDTO` / `UpdateSegUsuarioDTO`
| Propriedade | Tipo C# | Obrigatório | Descrição |
| :--- | :--- | :---: | :--- |
| `Nome` | `string` | Sim | Nome completo do usuário |
| `Email` | `string` | Sim | E-mail único do usuário |
| `Login` | `string` | Sim | Login único de acesso |
| `Senha` | `string` (apenas Create) | Sim (Create) | Senha em texto plano (convertida em hash BCrypt custo 12) |
| `Telefone` | `string?` | Não | Telefone de contato |
| `PerfilId` | `long` (Create) / `long?` (Update) | Sim (Create) | ID do perfil de acesso vinculado ao usuário |
| `Status` | `int` (Update) | Sim (Update) | `1` = Ativo, `0` = Inativo, `2` = Bloqueado |

---

## 3. Modelos TypeScript do Frontend (`oficina-motos-web/src/app/core/models/seguranca.ts`)

```typescript
export interface SegPerfilResumo {
  id: number;
  nome: string;
  nivel: number;
}

export interface SegUsuarioTableItem {
  id: number;
  nome: string;
  login: string;
  email: string;
  status: number;
  statusDescricao: string;
  perfis: string;
  ultimoLogin?: string | null;
}

export interface SegUsuario {
  id: number;
  nome: string;
  email: string;
  login: string;
  telefone?: string | null;
  fotoUrl?: string | null;
  status: number;
  statusDescricao: string;
  ultimoLogin?: string | null;
  createdAt?: string;
  updatedAt?: string | null;
  perfis: SegPerfilResumo[];
}

export interface CreateSegUsuarioRequest {
  nome: string;
  email: string;
  login: string;
  senha: string;
  telefone?: string | null;
  perfilId: number;
}

export interface UpdateSegUsuarioRequest {
  nome: string;
  email: string;
  login: string;
  telefone?: string | null;
  status: number;
  perfilId?: number | null;
}

export interface SegPermissao {
  id: number;
  moduloId: number;
  moduloNome: string;
  acao: string;
  codigo: string;
  descricao?: string | null;
}

export interface SegPerfil {
  id: number;
  nome: string;
  descricao?: string | null;
  nivel: number;
  status: number;
  createdAt?: string;
  updatedAt?: string | null;
  permissoes: SegPermissao[];
}

export interface CreateSegPerfilRequest {
  nome: string;
  descricao?: string | null;
  nivel: number;
}

export interface UpdateSegPerfilRequest {
  nome: string;
  descricao?: string | null;
  nivel: number;
  status: number;
}

export interface SetPerfilPermissoesRequest {
  permissaoIds: number[];
}
```
