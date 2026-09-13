# Guia Rápido: Cadastro de Mecânico (`/mecanicos/novo`)

**Feature**: `015-cadastro-mecanico` | **Data**: 2026-09-13
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

## 2. Teste Manual da Rota
1. Inicie a API e a aplicação Angular (`npm run start`).
2. Acesse a rota `/mecanicos`.
3. Clique no botão **"+ Novo Mecânico"**.
4. Observe a abertura do formulário em `/mecanicos/novo`.
5. Tente informar um CPF já cadastrado e note a sinalização imediata de duplicidade.
6. Informe um CPF válido e selecione 2 ou mais especialidades (marcando uma como principal).
7. Clique em **"Salvar Mecânico"** e verifique o redirecionamento com Toast de sucesso.

