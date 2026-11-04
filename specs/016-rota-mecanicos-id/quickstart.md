# Guia Rápido: Detalhe de Mecânico (`/mecanicos/:id`)

**Feature**: `016-rota-mecanicos-id` | **Data**: 2026-11-04
**Referências de Governança**: [`governance/arquitetura.md`](file:///c:/Projetos/OficinaMotos/governance/arquitetura.md)

---

## 1. Comandos de Verificação

### Executar os Testes Unitários do Frontend
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm test -- --no-watch
```

### Executar o Build de Produção
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm run build
```

---

## 2. Roteiro de Teste Manual
1. Inicie a API (.NET 8) e a aplicação Angular (`npm run start`).
2. Acesse `/mecanicos`.
3. Na listagem, clique sobre o **nome do mecânico** (link na coluna Nome) ou no botão **"Visualizar"** na coluna Ações.
4. Verifique o redirecionamento para `/mecanicos/:id` (ex.: `/mecanicos/1`).
5. Confira na página de detalhes:
   - Card lateral com avatar, status, Valor/Hora, Carga Horária Semanal, CPF e Data de Admissão.
   - Abas de **Especialidades**, **Disponibilidade**, **Certificações**, **Experiências**, **Documentos**, **Endereços** e **Contatos**.
6. Clique no botão **"← Voltar para Mecânicos"** e verifique o retorno para `/mecanicos`.
7. Teste acessar `/mecanicos/999999` diretamente na barra de endereços e confira a mensagem amigável de erro com botão de retorno.

