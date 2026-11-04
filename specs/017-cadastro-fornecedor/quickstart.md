# Guia Rápido: Cadastro de Fornecedor (`/fornecedores/novo`)

**Feature**: `017-cadastro-fornecedor` | **Data**: 2026-11-04
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
2. Acesse `/fornecedores` e clique no botão **"+ Novo fornecedor"**.
3. Verifique a abertura da rota `/fornecedores/novo`.
4. No campo **CNPJ**, digite um número inválido (ex.: `11.111.111/1111-11`) e verifique a mensagem de erro; em seguida informe um CNPJ válido (`11.222.333/0001-81`).
5. Selecione um **Segmento Principal** no dropdown dinâmico.
6. No campo **CEP**, digite `01001-000` e verifique o autopreenchimento de Logradouro, Bairro, Cidade e UF via `CepService`.
7. Clique em **"Salvar Fornecedor"** e confira o Toast de sucesso e o redirecionamento para `/fornecedores`.

