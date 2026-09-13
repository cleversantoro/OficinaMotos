# Guia de Inicialização Rápida (Quickstart): Cadastro de Veículo (Motocicleta)

Este guia fornece instruções diretas para executar, testar e validar o formulário de cadastro de veículos (US-013).

---

## 1. Executando os Testes Automatizados

### 1.1 Testes Unitários do Validador de Placa
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-web
npx ng test --no-watch --include src/app/shared/validators/placa-validator.spec.ts
```

### 1.2 Testes Unitários do Componente de Cadastro de Veículo
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-web
npx ng test --no-watch --include src/app/features/motos/pages/veiculo-cadastro/veiculo-cadastro.spec.ts
```

### 1.3 Verificação Completa da Suíte Frontend
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-web
npx ng test --no-watch
```

### 1.4 Build de Produção
```bash
cd c:\Projetos\OficinaMotos\oficina-motos-web
npm run build
```

---

## 2. Roteiro de Validação Manual Passo a Passo

### Cenário 1: Navegação a partir da Lista de Veículos
1. Inicie o frontend com `npm start` ou `npm run start:proxy`.
2. Acesse a rota de veículos: `http://localhost:4200/motos`.
3. Verifique a presença do botão **"+ Nova Moto"** / **"Novo Veículo"** no cabeçalho da página.
4. Clique no botão e confirme o redirecionamento imediato para `http://localhost:4200/motos/novo`.

### Cenário 2: Validação de Placa (Mercosul e Tradicional)
1. No campo **Placa**, digite letras minúsculas: `bra2e19`.
2. Observe que o campo converte automaticamente para `BRA2E19`.
3. Teste valores inválidos:
   - Digite `1234ABC` -> Deve exibir erro: *"Placa inválida. Utilize o formato Mercosul (ABC1D23) ou tradicional (ABC-1234)"*.
   - Digite `ABC12` -> Deve exibir erro de formato.
4. Digite uma placa tradicional: `ABC-1234` ou `ABC1234` -> A mensagem de erro deve sumir e o campo ficar válido.

### Cenário 3: Autocomplete do Cliente Proprietário
1. No campo **Proprietário (Cliente)**, digite ao menos 2 letras (ex.: `"Sil"`).
2. O dropdown deve exibir a lista de clientes encontrados com nome e documento.
3. Clique em um cliente: o nome é preenchido e o ID é vinculado.
4. Apague o texto: o vínculo deve ser desfeito e o formulário marcado como inválido.

### Cenário 4: Cascata Marca -> Modelo
1. Observe que o campo **Modelo** está inicialmente desabilitado com o aviso *"Selecione primeiro a marca"*.
2. Selecione a marca **"Honda"**: o campo de modelo é habilitado e exibe apenas modelos da Honda (ex.: CG 160, CB 500F, XRE 300).
3. Selecione um modelo (ex.: "CG 160").
4. Mude a marca para **"Yamaha"**: o modelo selecionado anteriormente deve ser limpo e a nova lista carregada (ex.: Fazer FZ25, MT-03, Lander 250).

### Cenário 5: Submissão e Redirecionamento
1. Preencha os campos complementares (Ano Fab: `2023`, Ano Mod: `2024`, Cor: `"Azul"`, KM: `15000`, Combustível: `"Flex"`).
2. Clique no botão **"Salvar Veículo"**.
3. O botão deve exibir estado de carregamento (`loading`/`submitting`).
4. Ao concluir, deve surgir uma notificação Toast de sucesso e o usuário deve ser redirecionado para a lista ou detalhe da moto.

