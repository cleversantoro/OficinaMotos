# Guia Rápido: Campo `proximo_km_revisao` no Veículo

**Feature**: `014-proximo-km-revisao` | **Data**: 2026-09-13

---

## 1. Verificação do Backend

### Compilar a Solução .NET
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet build
```

### Executar a Migração
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-api
dotnet ef database update --project src/OficinaMotos.Infrastructure --startup-project src/OficinaMotos.API
```

---

## 2. Verificação do Frontend

### Executar os Testes Unitários
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

## 3. Fluxo de Validação Manual
1. Iniciar o backend com `dotnet run --project src/OficinaMotos.API`.
2. Iniciar o frontend com `npm run start` (ou `start:proxy`).
3. Fazer login como operador com permissão de veículos (`Recepcionista` ou `Administrador`).
4. Acessar `/motos` e clicar em **"+ Nova Moto"** (`/motos/novo`).
5. Preencher os dados obrigatórios e informar no campo **"Próxima Revisão (KM)"** o valor `12000`.
6. Clicar em **"Salvar Veículo"**.
7. Confirmar a persistência inspecionando a requisição de rede e a tela de listagem/detalhes.

