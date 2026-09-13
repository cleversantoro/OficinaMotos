# Regra de Automação de Scripts de Banco de Dados (MySQL 8.0)

Esta regra é de cumprimento obrigatório para todas as tarefas de engenharia de software neste repositório.

## Contexto
- SGBD: **MySQL versão 8.0**
- ORM: Entity Framework Core 8 com Pomelo MySQL Provider
- Pasta de Scripts: `oficina-motos-doc/scripts/` (ou `oficina-motos-docs/scripts/`)

## Diretrizes
1. Qualquer modificação que altere o esquema do banco de dados (tabelas, colunas, índices, constraints, migrations) exige a criação imediata de um arquivo `.sql` correspondente em `oficina-motos-doc/scripts/`.
2. A sintaxe deve ser 100% compatível com MySQL 8.0 (`InnoDB`, `utf8mb4`).
3. O script deve ser transacional (`START TRANSACTION; ... COMMIT;`).
4. Deve incluir instruções de Rollback / Down comentadas ao final.
5. O nome deve ser padronizado: `<YYYYMMDD>_<nome_da_migration_ou_mudanca>.sql`.

