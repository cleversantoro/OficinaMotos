# Guia Rápido: Cadastro de Peça no Estoque (`/estoque/novo`)

**Feature**: `018-cadastro-peca-estoque` | **Data**: 2026-11-04
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
2. Acesse `/estoque`.
3. Verifique que qualquer peça com `quantidade < estoqueMinimo` exibe o badge **"Crítico"** na coluna Estoque (`T-018.4`).
4. Clique no botão **"+ Nova peça"** e verifique o redirecionamento para `/estoque/novo` (`T-018.2`).
5. Preencha a descrição da peça, selecione **Fabricante**, **Categoria** e **Fornecedor** nos dropdowns (`T-018.3`), informe o preço de venda e quantidades (`quantidade: 2`, `estoqueMinimo: 5`).
6. Clique em **"Salvar Peça"** e verifique o redirecionamento para `/estoque` com o item recém-criado exibindo o badge de estoque crítico.

