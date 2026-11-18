# Referência — Responsive Design

## Estratégia

Use Mobile First como ponto de partida. Os breakpoints devem ser definidos quando o conteúdo deixa de funcionar bem, e não apenas para corresponder a modelos de aparelhos.

## Faixas iniciais para teste

| Faixa | Viewport de referência |
|---|---:|
| Celular estreito | 360 px |
| Celular comum | 390 px |
| Tablet | 768 px |
| Tablet/desktop compacto | 1024 px |
| Desktop | 1366 px |
| Desktop amplo | 1440 px |

São viewports de validação, não breakpoints obrigatórios.

## Regras

- Prefira Grid, Flexbox, `minmax()`, `clamp()` e unidades relativas.
- Evite larguras rígidas e alturas fixas em conteúdo variável.
- Garanta que filhos de flex/grid possam encolher (`min-width: 0`) quando necessário.
- Evite overflow horizontal da página.
- Em tabelas extensas, use rolagem localizada ou uma apresentação alternativa que preserve dados importantes.
- Em telas pequenas, priorize ações principais e empilhe formulários.
- Menus laterais devem virar drawer ou navegação compacta quando necessário.
- Modais precisam caber na viewport e permitir rolagem interna.
- Áreas clicáveis devem ser confortáveis para toque.
- Teste zoom e conteúdo com textos maiores.

## Revisão visual

Verifique:
- texto cortado ou sobreposto;
- botões fora da viewport;
- menus que não fecham;
- tabelas ilegíveis;
- modais inacessíveis;
- campos pequenos demais;
- rolagem horizontal acidental;
- foco e teclado em navegação móvel.
