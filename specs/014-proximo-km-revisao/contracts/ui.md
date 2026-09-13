# Contratos de Interface (UI): Campo `proximo_km_revisao`

**Feature**: `014-proximo-km-revisao` | **Data**: 2026-09-13

---

## 1. Formulário de Cadastro (`veiculo-cadastro.html`)

### 1.1 Posicionamento
O campo é inserido na seção **Especificações Técnicas** (Card 3), posicionado imediatamente após o campo "Quilometragem Atual (KM)".

### 1.2 Especificações dos Controles
| Propriedade | Valor |
|---|---|
| **Rótulo** | `Próxima Revisão (KM)` |
| **Elemento** | `<input type="number" formControlName="proximoKmRevisao" />` |
| **Placeholder** | `Ex: 15000` |
| **Obrigatoriedade** | Opcional (não possui asterisco `*`) |
| **Validações** | `min: 0` |
| **Mensagem de Erro** | `"A quilometragem não pode ser negativa."` (exibida quando dirty/touched e inválido) |
| **Normalização** | Converte valor vazio para `null`, strings numéricas para `number` no payload |

---

## 2. Tela de Detalhes (`veiculo-detalhe.html`)

### 2.1 Exibição em Card de Dados Técnicos
```html
@if (veiculo.proximoKmRevisao) {
  <div class="data">
    <p class="label">Próxima Revisão (KM)</p>
    <p class="value">{{ veiculo.proximoKmRevisao }} km</p>
  </div>
}
```

### 2.2 Exibição em KPI no Header
```html
@if (veiculo.proximoKmRevisao) {
  <div class="kpi">
    <strong>{{ veiculo.proximoKmRevisao }} km</strong>
    <span>Próxima Revisão</span>
  </div>
}
```

