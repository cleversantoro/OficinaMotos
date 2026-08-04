START TRANSACTION;

ALTER TABLE `seg_usuarios_perfis` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_usuarios_perfis` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `seg_usuarios` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_usuarios` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `seg_refresh_tokens` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_refresh_tokens` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `seg_permissoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_permissoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `seg_perfis_permissoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_perfis_permissoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `seg_perfis` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_perfis` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `seg_modulos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_modulos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `seg_audit_log` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `seg_audit_log` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_pagamentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_pagamentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_ordens_historico` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_ordens_historico` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_ordens` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_ordens` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_observacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_observacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_itens` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_itens` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_checklists` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_checklists` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_avaliacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_avaliacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `os_anexos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `os_anexos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `fin_pagamentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `fin_pagamentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `fin_metodos_pagamento` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `fin_metodos_pagamento` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `fin_lancamentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `fin_lancamentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `fin_historico` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `fin_historico` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `fin_contas_receber` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `fin_contas_receber` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `fin_contas_pagar` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `fin_contas_pagar` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `fin_anexos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `fin_anexos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_pecas_historico` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_pecas_historico` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_pecas_fornecedores` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_pecas_fornecedores` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_pecas_anexos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_pecas_anexos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_pecas` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_pecas` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_movimentacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_movimentacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_localizacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_localizacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_fabricantes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_fabricantes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `est_categorias` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `est_categorias` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_veiculos_modelos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_veiculos_modelos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_veiculos_marcas` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_veiculos_marcas` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_veiculos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_veiculos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_experiencias` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_experiencias` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_especialidades_rel` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_especialidades_rel` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_especialidades` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_especialidades` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_enderecos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_enderecos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_documentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_documentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_disponibilidades` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_disponibilidades` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_contatos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_contatos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos_certificacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos_certificacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_mecanicos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_mecanicos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_segmentos_rel` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_segmentos_rel` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_segmentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_segmentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_representantes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_representantes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_enderecos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_enderecos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_documentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_documentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_contatos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_contatos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_certificacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_certificacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_bancos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_bancos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores_avaliacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores_avaliacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_fornecedores` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_fornecedores` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_pj` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_pj` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_pf` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_pf` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_origens` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_origens` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_lgpd_consentimentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_lgpd_consentimentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_indicacoes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_indicacoes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_financeiro` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_financeiro` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_enderecos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_enderecos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_documentos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_documentos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_contatos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_contatos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes_anexos` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes_anexos` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `cad_clientes` ADD `DeletedAt` datetime(6) NULL;

ALTER TABLE `cad_clientes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260804044157_AddSoftDeleteToBaseEntity', '8.0.22');

COMMIT;

