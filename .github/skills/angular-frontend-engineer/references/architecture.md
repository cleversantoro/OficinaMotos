# Referência — Arquitetura Frontend Angular

## Objetivo

Estabelecer critérios para estruturar aplicações Angular sem impor complexidade desnecessária.

## Princípios

- Organizar por funcionalidade/domínio.
- Separar apresentação, acesso a dados e regras de aplicação quando isso trouxer clareza.
- Manter dependências direcionadas: componentes de apresentação não devem conhecer detalhes de infraestrutura.
- Compartilhar elementos genéricos; manter elementos específicos dentro da feature.
- Evitar dependências circulares.
- Evitar diretórios globais que virem depósitos de código sem contexto.

## Limites de responsabilidade

| Elemento | Responsabilidade |
|---|---|
| Page/Container | Compor a página e coordenar o fluxo da feature |
| Presentational component | Exibir dados e emitir intenções do usuário |
| Data-access service | Consultar e executar operações na API |
| Facade (opcional) | Coordenar estado e operações quando a complexidade justificar |
| Model/DTO | Representar contratos e dados |
| Guard | Controlar navegação, nunca substituir autorização no backend |
| Interceptor | Tratar preocupações transversais HTTP |
| Shared component | UI genérica, sem regra de negócio específica |

## Regra prática

Não crie uma camada porque o desenho arquitetural a menciona. Crie-a quando houver responsabilidade real, complexidade ou benefício verificável.
